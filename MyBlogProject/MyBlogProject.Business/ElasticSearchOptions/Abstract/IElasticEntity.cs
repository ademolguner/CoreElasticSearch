using System;
using System.Collections.Generic;
using System.Text;

namespace  MyBlogProject.Business.ElasticSearchOptions.Abstract
{
    public interface IElasticEntity<TEntityKey>
    {
        TEntityKey Id { get; set; }

    }
}
