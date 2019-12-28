using Microsoft.Extensions.Configuration;

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