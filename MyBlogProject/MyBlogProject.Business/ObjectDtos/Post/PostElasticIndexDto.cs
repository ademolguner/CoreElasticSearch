using MyBlogProject.Business.ElasticSearchOptions.Conrete;
using MyBlogProject.Core.Dtos;
using System;
using System.Collections.Generic;

namespace MyBlogProject.Business.ObjectDtos.Post
{
    public class PostElasticIndexDto : ElasticEntity<int>, IDto
    {
        public PostElasticIndexDto()
        {
            TagNameValues = new List<string>();
            TagNameIds = new List<int>();
        }
         
        public virtual string Title { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual List<string> TagNameValues { get; set; }
        public virtual List<int> TagNameIds { get; set; }
        public virtual string Url { get; set; }
        public virtual string UserInfo { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}