using MyBlogProject.Business.Abstract;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entites.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Concrete.Manager
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public Category Insert(Category category)
        {
            _categoryDal.Add(category);
            return category;
        }

        public void Remove(Category category)
        {
            _categoryDal.Delete(category);
        }

        public List<Category> GetAllList()
        {
            return _categoryDal.GetList();
        }

        public Category GetByItem(object item)
        {
            return _categoryDal.Get(c => c.CategoryId == (int)item);
        }

        public Category Update(Category category)
        {
            _categoryDal.Update(category);
            return GetByItem(category.CategoryId);
        }
    }
}