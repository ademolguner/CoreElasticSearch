namespace MyBlogProject.Business.ElasticSearchOptions.Abstract
{
    public interface IElasticEntity<TEntityKey>
    {
        TEntityKey Id { get; set; }
    }
}