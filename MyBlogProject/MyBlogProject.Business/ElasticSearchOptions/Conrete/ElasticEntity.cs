using MyBlogProject.Business.ElasticSearchOptions.Abstract;
using Nest;

namespace MyBlogProject.Business.ElasticSearchOptions.Conrete
{
    public class ElasticEntity<TEntityKey> : IElasticEntity<TEntityKey>
    {
        public TEntityKey Id { get; set; }
        public CompletionField Suggest { get; set; }
        public string SearchableText { get; set; }
    }

    //public class ElasticEntity
    //{
    //    public CompletionField Suggest { get; set; }
    //}
}