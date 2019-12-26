using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Business.ElasticSearchOptions.Configurations
{
   public static class ElasticSearchConfigrationExtensions
    {
        public static IElasticSearchConfigration SearchConfiguration(this IConfiguration configuration)
        {
           return  configuration.Get<IElasticSearchConfigration>();
        }
    }
}
