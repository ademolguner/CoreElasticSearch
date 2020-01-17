using MyBlogProject.Business.ElasticSearchOptions.Abstract;
using Nest;

namespace MyBlogProject.Business.ElasticSearchOptions.Conrete
{
    public class ElasticEntity<TEntityKey> : IElasticEntity<TEntityKey>
    {
        public virtual TEntityKey Id { get; set; }
        public virtual CompletionField Suggest { get; set; }
        public virtual string SearchableText { get; set; }
        public virtual double Score { get; set; }
    } 
}