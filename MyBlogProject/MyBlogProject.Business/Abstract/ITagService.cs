using  MyBlogProject.Entities.Models;
using  MyBlogProject.Entities.ComplexTypes;
using System.Collections.Generic;

namespace  MyBlogProject.Business.Abstract
{
    public interface ITagService
    {
        List<Tag> GetAllTags();

        Tag GetTagById(int id);

        Tag AddTag(Tag tag);

        Tag UpdateTag(Tag tag);

        void DeleteTag(Tag tag);
        List<PostTagDto> PostTagListForPost(int postID);

        // Burada yazılan metotlar iş kuralları gereğince gelen 
        // ihtiyaçlara karşılık verebilmek için değişkenlik
        // gösterecektir.
        // Ben örnek olması için sadece olası bir kaç metot yazıp
        // içeriğini doldurdum.

        // Gerisi sizde :)
    }
}
