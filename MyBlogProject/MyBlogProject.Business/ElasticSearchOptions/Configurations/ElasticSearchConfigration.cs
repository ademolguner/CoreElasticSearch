namespace MyBlogProject.Business.ElasticSearchOptions.Configurations
{
    public class ElasticSearchConfigration : IElasticSearchConfigration
    {
        public string ConnectionString { get { return "http://localhost:9200/"; } }
        public string AuthUserName { get { return "guest"; } }
        public string AuthPassWord { get { return "guest"; } }
    }
}