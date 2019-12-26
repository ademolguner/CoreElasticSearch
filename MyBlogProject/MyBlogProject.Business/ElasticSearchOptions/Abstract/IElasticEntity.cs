using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Business.ElasticSearchOptions.Abstract
{
    public interface IElasticEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

    }
}
