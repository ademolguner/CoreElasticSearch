using  MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace  MyBlogProject.Business.Abstract
{
    public interface IPostTagService
    {

        List<PostTag> GetAll();

        PostTag GetById(int id);

        PostTag Add(PostTag postTag);

        PostTag Update(PostTag postTag);

        void Delete(PostTag postTag);

        List<PostTag> GetListByPostId(int postId);
        List<PostTag> GetListByTagId(int tagId);
    }
}
