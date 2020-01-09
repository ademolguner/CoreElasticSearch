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
            TagNameValues = new List<string>();// new HashSet<string>();
        }

        //public Guid ObjectId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string CategoryName { get; set; }
        public List<string> TagNameValues { get; set; }
        public string Url { get; set; }
        public string UserInfo { get; set; }
    }
}