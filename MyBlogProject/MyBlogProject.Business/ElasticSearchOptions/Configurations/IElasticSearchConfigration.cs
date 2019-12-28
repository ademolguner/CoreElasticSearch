namespace MyBlogProject.Business.ElasticSearchOptions.Configurations
{
    public interface IElasticSearchConfigration
    {
        /// <summary>
        ///
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        ///
        /// </summary>
        string AuthUserName { get; }

        /// <summary>
        ///
        /// </summary>
        string AuthPassWord { get; }
    }
}