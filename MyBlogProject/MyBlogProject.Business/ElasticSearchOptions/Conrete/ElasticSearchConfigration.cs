using Microsoft.Extensions.Configuration;
using MyBlogProject.Business.ElasticSearchOptions.Abstract;

namespace MyBlogProject.Business.ElasticSearchOptions.Conrete
{
    public class ElasticSearchConfigration : IElasticSearchConfigration
    {
        public IConfiguration Configuration { get; }
        public ElasticSearchConfigration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string ConnectionString { get { return Configuration.GetSection("ElasticSearchOptions:ConnectionString:HostUrls").Value; } }
        public string AuthUserName { get { return Configuration.GetSection("ElasticSearchOptions:ConnectionString:UserName").Value; } }
        public string AuthPassWord { get { return Configuration.GetSection("ElasticSearchOptions:ConnectionString:Password").Value; } }
    }
}