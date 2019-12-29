using MyBlogProject.Business.ElasticSearchOptions.Conrete;
using MyBlogProject.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlogProject.Business.ObjectDtos.Log
{
    public class ErrorLogElasticIndexDto : ElasticEntity<Guid>, IDto
    {
        public Guid TransactionId { get; set; }
        public string Message { get; set; }
        public DateTime ErrorDate { get; set; }
        public int UserId { get; set; }
    }
}
