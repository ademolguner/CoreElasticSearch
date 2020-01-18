using MyBlogProject.Business.Abstract;
using MyBlogProject.Business.ElasticSearchOptions.Abstract;
using MyBlogProject.Business.ObjectDtos.Post;
using MyBlogProject.Core.Consts;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.ComplexTypes;
using MyBlogProject.Entities.Models;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace MyBlogProject.Business.Concrete.Manager
{
    public class PostManager : IPostService
    {
        public IElasticClient EsClient { get; set; }
        private readonly IPostDal _postDal;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly ITagService _tagService;
        private readonly IUserService _userService;

        public PostManager(IPostDal postDal, IElasticSearchService elasticSearchService, ITagService tagService, IUserService userService)
        {
            _postDal = postDal;
            _elasticSearchService = elasticSearchService;
            _tagService = tagService;
            _userService = userService;
        }

        public Post Insert(Post post)
        {
            _postDal.Add(post);
            return post;
        }

        public void Remove(Post post)
        {
            _postDal.Delete(post);
        }

        public List<Post> GetAllList()
        {
            return _postDal.GetList();
        }

        public Post GetByItem(object item)
        {
            return _postDal.Get(c => c.PostId == (int)item);
        }

        public Post Update(Post post)
        {
            _postDal.Update(post);
            return GetByItem(post.PostId);
        }

        public List<Post> GetPostsByCategoryId(int categoryId)
        {
            return _postDal.GetList(c => c.CategoryId == categoryId);
        }

        public List<Post> GetPostsByCategoryId(int categoryId, int lastAmount)
        {
            return _postDal.GetList(c => c.CategoryId == categoryId).Take(lastAmount).ToList();
        }

        public List<Post> GetPostsByUserId(int userId)
        {
            return _postDal.GetList(c => c.UserId == userId);
        }
        public PostDetailInfo GetPostDetail(int postId)
        {
            PostDetailInfo detailInfo = _postDal.PostDetail(postId);
            detailInfo.TagIds = _tagService.PostTagListForPost(postId).Select(x => x.TagValueId).ToList();
            return detailInfo;
        }

        public async Task<bool> PostAddOrUpdateElasticIndex(PostElasticIndexDto postElasticIndexDto)
        {
            try
            {
                // Her ekleme işleminde daha önce Index oluşturulup oluşturulmadığını kontrol ediyoruz.
                await _elasticSearchService.CreateIndexAsync<PostElasticIndexDto, int>(ElasticSearchItemsConst.PostIndexName);
                // Yeni bir elasticindex kayıt ekliyoruz(Document)
                await _elasticSearchService.AddOrUpdateAsync<PostElasticIndexDto, int>(ElasticSearchItemsConst.PostIndexName,postElasticIndexDto);
                return await Task.FromResult<bool>(true);
            }
            catch (Exception ex)
            {
                return await Task.FromException<bool>(ex);
            }
        }

        public async Task<bool> PostDeleteDocumentElasticIndex(PostElasticIndexDto postElasticIndexDto)
        {
            try
            {
                await _elasticSearchService.DeleteAsync<PostElasticIndexDto, int>(
                    ElasticSearchItemsConst.PostIndexName,
                    "postelasticindexdto",
                    postElasticIndexDto
                    );
                return true;
            }
            catch (Exception ex)
            {
                return await Task.FromException<bool>(ex);
            }
        }

        public async Task<List<PostElasticIndexDto>> SuggestSearchAsync(string searchText, int skipItemCount = 0, int maxItemCount = 5)
        {
            try
            {
                 var indexName = ElasticSearchItemsConst.PostIndexName;
                var queryy = new Nest.SearchDescriptor<PostElasticIndexDto>()  // SearchDescriptor burada oluşturacağız 
                              .Suggest(su => su
                                               .Completion("post_suggestions",
                                      c => c.Field(f => f.Suggest)
                                               .Analyzer("simple")
                                               .Prefix(searchText)
                                               .Fuzzy(f => f.Fuzziness(Nest.Fuzziness.Auto))
                                               .Size(10))
                                        );

                var returnData = await _elasticSearchService.SearchAsync<PostElasticIndexDto, int>(indexName, queryy, 0, 0);

                var data = JsonConvert.SerializeObject(returnData);
                var suggestsList = returnData.Suggest.Count > 0 ? from suggest in returnData.Suggest["post_suggestions"]
                                                                  from option in suggest.Options
                                                                  select new PostElasticIndexDto
                                                                  {
                                                                      Score = option.Score,
                                                                      CategoryName = option.Source.CategoryName,
                                                                      Title = option.Source.Title,
                                                                      UserInfo = option.Source.UserInfo,
                                                                      Suggest = option.Source.Suggest,
                                                                      Url = option.Source.Url,
                                                                      Id = option.Source.Id
                                                                  }
                                                                  : null;

                return await Task.FromResult(suggestsList.ToList());
            }
            catch (Exception ex)
            {
                return await Task.FromException<List<PostElasticIndexDto>>(ex);
            }
        }

        public async Task<List<PostElasticIndexDto>> GetSearchAsync(string searchText, int skipItemCount = 0, int maxItemCount = 100)
        {
            try
            {
                var currentUserId = 1;
                var selectedCategoryId = 6;
                var userFullNameText = _userService.GetByItem(currentUserId).FullName;
                // search descriptor yazmak gerekiyor
                var indexName = ElasticSearchItemsConst.PostIndexName;
                var searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>(); /* new Nest.SearchDescriptor<PostElasticIndexDto>()
                            .Query(q =>
                                           //Termler: Sadece boolen yani “Yes / No” veya string bir kelime ile eşleşebilecek durumlarda kullanılır.
                                           // q.Term(t => t.UserId, currentUserId)   // tek bir parametreye ait sorgulama için
                                           // coklu term işlemleri birden cok parametreye ait sart işlemi için kullanılır.
                                           q.Terms(t => t
                                                         .Field(ff => ff.UserId).Terms<int>(currentUserId)
                                                         .Field(ff => ff.CategoryId).Terms<int>(selectedCategoryId))
                                     // aranan kelime veya cümle geçmesi yeterlidir, bire bir eşleme istemez
                                     && q.MatchPhrasePrefix(m => m.Field(f => f.SearchingArea).Query(searchText))
                                   // aranan kelime veya cümlenin bire bir eşleşmesi gerekmektedir.
                                   //|| q.MatchPhrase(m=> m.Field(f=> f.SearchingArea).Query(searchText))
                                   );*/

                //Termler: Sadece boolen yani “Yes / No” veya string bir kelime ile eşleşebilecek durumlarda kullanılır.
                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                             .Query(q => q.Term(t => t.UserId, currentUserId));

                //  // coklu term işlemleri birden cok parametreye ait sart işlemi için kullanılır.
                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                            .Query(q => q.Terms(t => t.Field(ff => ff.UserId).Terms<int>(currentUserId)
                                                      .Field(ff => ff.CategoryId).Terms<int>(selectedCategoryId))
                                                );
                // aranan kelime veya cümle geçmesi yeterlidir, bire bir eşleme istemez
                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                            .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.SearchingArea).Query(searchText)));

                // aranan kelime veya cümlenin bire bir eşleşmesi gerekmektedir.
                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                            .Query(q => q.MatchPhrase(m => m.Field(f => f.SearchingArea).Query(searchText)));


                // komplex sorgular seklinde birleştirip yazabiliriz.
                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                            .Query(q => q.Terms(t => t.Field(ff => ff.UserId).Terms<int>(currentUserId)
                                                      .Field(ff => ff.CategoryId).Terms<int>(selectedCategoryId)
                                                )
                                     && q.MatchPhrasePrefix(m => m.Field(f => f.SearchingArea).Query(searchText))
                                   );


                //  arama kelimesi Core İşlemleri sql
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-multi-match-query.html
                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                         .Query(q => q
                            .MultiMatch(m => m.Fields(f => f.Field(ff => ff.SearchingArea, 2.0)
                                                            .Field(ff => ff.Title, 1.0)
                                                     )
                                              .Query(searchText)
                                              .Type(TextQueryType.BestFields)
                                              .Operator(Operator.Or)  // Operator.And  dene 
                                        )
                               );


                // 2.0 ve 1.0 ı vermeden yaz ve vererek yaz ama acıkla

                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                         .Query(q => q
                                      .MultiMatch(m => m.Fields(f => f.Field(ff => ff.SearchingArea, 2.0)
                                                                      .Field(ff => ff.Title, 1.0)
                                                               )
                                                        .Query(searchText)
                                                        .Type(TextQueryType.BestFields)
                                                        .Operator(Operator.Or) 
                                                        .MinimumShouldMatch(3)
                                                  )
                              )
                         .Sort(s => s.Descending(f => f.CreatedDate));



                //searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                //          .Query(q =>
                //                       q.MultiMatch(m => m.Fields(f => f.Field(ff => ff.SearchingArea, 2.0)
                //                                                       .Field(ff => ff.Title, 1.0)
                //                                                )
                //                                         .Query(searchText)
                //                                         .Type(TextQueryType.BestFields)
                //                                         .Operator(Operator.Or)  // Operator.And  dene 
                //                                   )
                //                    && q.Range(r => r.Field(rf => rf.TagNameValues.Count).GreaterThan(2))
                //                    || q.Range(r => r.Field(rf => rf.TagNameValues.Count).GreaterThanOrEquals(3))
                //               )
                //          .Sort(s => s.Descending(f => f.CreatedDate.Date));

                /*
                 *  https://www.elastic.co/guide/en/elasticsearch/painless/7.5/painless-operators-boolean.html
                 greater_than: expression '>' expression;
                 greater_than_or_equal: expression '>=' expression;
                 less_than: expression '<' expression;
                 greater_than_or_equal: expression '<=' expression;
                 instance_of: ID 'instanceof' TYPE;
                 equality_equals: expression '==' expression;
                 equality_not_equals: expression '!=' expression;
                 identity_equals: expression '===' expression;
                 identity_not_equals: expression '!==' expression;
                 boolean_xor: expression '^' expression;
                 boolean_and: expression '&&' expression;
                 boolean_and: expression '||' expression;
                 */

                /*
                 f.Field(ff => ff.SearchingArea, 2.0)  buradaki 2.0 işlemi boost işlemidir.
                 öncelik  ve katsayı işlemidir   ÖNCELİKLENDİRME İŞLEMİDİR.
                 */
                //searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                //          .Query(q =>
                //                       q.MultiMatch(m => m.Fields(f => f.Field(ff => ff.SearchingArea, 2.0)
                //                                                       .Field(ff => ff.Title, 1.0)
                //                                                )
                //                                         .Query(searchText)
                //                                         .Type(TextQueryType.BestFields)
                //                                         .Operator(Operator.Or)  // Operator.And  dene 
                //                                   )
                //                    && q.Range(r => r.Field(rf => rf.TagNameValues.Count).GreaterThan(2))
                //                    && q.Range(r => r.Field(rf => rf.TagNameValues.Count).GreaterThanOrEquals(3))
                //               )
                //          .Sort(s => s.Descending(f => f.CreatedDate.Date))
                //          .Skip(skipItemCount)
                //          .Take(maxItemCount);






                //  searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                //           .Query(q =>
                //q.Bool(b => b.Should(s => TermAny(s, "userCodeCores", userCodeList.ToArray())))
                //                        );


    //            searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
    //.Query(q => q
    //    .Bool(b => b
    //        .Should(
    //            bs => bs.Term(p => p.UserId, 1),
    //            bs => bs.Term(p => p.CategoryId, 5)
    //        ).MinimumShouldMatch(3)
    //    )
    //);
                /* bool tipinde sorgular
                
                 must=>Cümle (sorgu) eşleşen belgelerde görünmelidir ve skora katkıda bulunacaktır.
                 filter=> Yan tümce (sorgu) eşleşen belgelerde görünmelidir. Ancak zorunluluktan farklı olarak, sorgunun puanı dikkate alınmaz.
                 should=> Yan tümce (sorgu) eşleşen belgede görünmelidir. Zorunlu veya filtre yan tümcesi olmayan bir boole sorgusunda, bir veya daha fazla yan tümce, 
                          bir belgeyle eşleşmelidir. Eşleşmesi gereken minimum koşul cümlesi sayısı minimum_should_match parametresi kullanılarak ayarlanabilir.
                 must_not=> Yan tümce (sorgu) eşleşen belgelerde görünmemelidir.
                  */


                //searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>()
                //    .Query(q =>
                //                q.Bool(b => b
                //                            .MustNot(m => m.MatchAll())
                //                            .Should(m => m.MatchAll())
                //                            .Must(m => m.MatchAll())
                //                            .Filter(f => f.MatchAll())
                //                            .MinimumShouldMatch(1)
                //                            .Boost(2))
                //    );




                //QueryContainer qvw = new TermQuery { Field = "x", Value = "x" };
                //var xyz = Enumerable.Range(0, 1000).Select(f => qvw).ToArray();
                //var boolQuery = new BoolQuery
                //{
                //    Must = xyz
                //};

                //var c = new QueryContainer();
                //var qq = new TermQuery { Field = "x", Value = "x" };

                //for (var i = 0; i < 10; i++)
                //{
                //    c &= qq;
                //}


//                searchQuery = new Nest.SearchDescriptor<PostElasticIndexDto>().Query(q =>
//q.QueryString(qs =>
//qs.DefaultField(d => d.CategoryName).Query(" c# sql server ".Trim()).DefaultOperator(Operator.And)));
 

               var dataJson= _elasticSearchService.ToJson<PostElasticIndexDto>(searchQuery); 



                //, 0, 10,null, "<strong style=\"color: red;\">", "</strong>", false, new string[] { "Title" }
                var searchResultData = await _elasticSearchService.SimpleSearchAsync<PostElasticIndexDto, int>(indexName, searchQuery);
                                                                                                    
                if (searchResultData.Hits.Count > 0)
                { var data = JsonConvert.SerializeObject(searchResultData); } 

                //var midir = from opt in searchResultData.Hits
                //            select new PostElasticIndexDto
                //            {
                //                Score = (double)opt.Score,
                //                CategoryName = opt.Source.CategoryName,
                //                Title = opt.Source.Title,
                //                UserInfo = opt.Source.UserInfo,
                //                Suggest = opt.Source.Suggest,
                //                Url = opt.Source.Url,
                //                Id = opt.Source.Id,
                //                CategoryId = opt.Source.CategoryId,
                //                CreatedDate = opt.Source.CreatedDate,
                //                UserId = opt.Source.UserId,
                //                TagNameValues = opt.Source.TagNameValues,
                //                TagNameIds = opt.Source.TagNameIds
                //            };


                var result2 = from opt in searchResultData.Documents
                            select new PostElasticIndexDto
                            {
                                Score = (double)opt.Score,
                                CategoryName = opt.CategoryName,
                                Title = opt.Title,
                                UserInfo = opt.UserInfo,
                                Suggest = opt.Suggest,
                                Url = opt.Url,
                                Id = opt.Id,
                                CategoryId = opt.CategoryId,
                                CreatedDate = opt.CreatedDate,
                                UserId = opt.UserId,
                                TagNameValues = opt.TagNameValues,
                                TagNameIds = opt.TagNameIds,
                                SearchingArea=opt.SearchingArea
                            };

                return await Task.FromResult<List<PostElasticIndexDto>>(result2.ToList());
            }
            catch (Exception ex)
            {
                return await Task.FromException<List<PostElasticIndexDto>>(ex);
            }
        }


    }
}