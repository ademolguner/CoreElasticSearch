using Microsoft.Extensions.Configuration;
using MyBlogProject.Business.ElasticSearchOptions.Abstract;

namespace MyBlogProject.Business.ElasticSearchOptions.Configurations
{
    public static class ElasticSearchConfigrationExtensions
    {
        public static IElasticSearchConfigration SearchConfiguration(this IConfiguration configuration)
        {
            return configuration.Get<IElasticSearchConfigration>();
        }
    }
     
}