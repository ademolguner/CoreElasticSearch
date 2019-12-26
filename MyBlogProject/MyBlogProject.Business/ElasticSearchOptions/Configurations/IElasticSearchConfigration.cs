using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Business.ElasticSearchOptions.Configurations
{
   public  interface IElasticSearchConfigration
    {
        /// <summary>
        /// 
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        ///
        /// </summary>
        string AuthUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string AuthPassWord { get; set; }
    }
}
