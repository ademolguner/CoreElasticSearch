using MyBlogProject.Core.Business.EntityRepository;
using MyBlogProject.Entities.ComplexTypes;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Abstract
{
    public interface ITagService : IEntityCommonRepository<Tag>
    {
        //List<Tag> GetAllTags();

        //Tag GetTagById(int id);

        //Tag AddTag(Tag tag);

        //Tag UpdateTag(Tag tag);

        //void DeleteTag(Tag tag);
        List<PostTagsInfo> PostTagListForPost(int postID);

        // Burada yazılan metotlar iş kuralları gereğince gelen
        // ihtiyaçlara karşılık verebilmek için değişkenlik
        // gösterecektir.
        // Ben örnek olması için sadece olası bir kaç metot yazıp
        // içeriğini doldurdum.

        // Gerisi sizde :)
    }
}