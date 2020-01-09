using AutoMapper;
using MyBlogProject.Business.ObjectDtos;
using MyBlogProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlogProject.Business.Mappings
{
  public   class CustomMappingProfile:Profile
    {
        public CustomMappingProfile()
        {
            CreateMap<Post, PostInputDto>();
            CreateMap<PostInputDto, Post>();
        }
       
         
    }
}
