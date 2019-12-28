using MyBlogProject.Business.ElasticSearchOptions.Conrete;
using MyBlogProject.Core.Dtos;
using System;
using System.Collections.Generic;

namespace MyBlogProject.Business.ObjectDtos.Post
{
    public class PostElasticIndexDto : ElasticEntity<Guid>, IDto
    {
        public PostElasticIndexDto()
        {
            TagNameValues = new HashSet<string>();
        }

        public Guid ObjectId { get { return Guid.NewGuid(); } }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string CategoryName { get; set; }
        public ICollection<string> TagNameValues { get; set; }
        public string Url { get; set; }
    }
}