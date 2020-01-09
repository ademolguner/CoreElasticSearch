using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Abstract
{
    public interface IPostTagService : IEntityCommonRepository<PostTag>
    {
        //List<PostTag> GetAll();

        //PostTag GetById(int id);

        //PostTag Add(PostTag postTag);

        //PostTag Update(PostTag postTag);

        //void Delete(PostTag postTag);

        List<PostTag> GetListByPostId(int postId);

        void RemoveAllPostTagsByPostId(int postId);

        List<PostTag> GetListByTagId(int tagId);
    }
}