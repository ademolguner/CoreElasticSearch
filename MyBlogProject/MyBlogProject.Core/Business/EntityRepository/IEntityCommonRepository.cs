using MyBlogProject.Core.Entities;
using System.Collections.Generic;

namespace MyBlogProject.Core.Business.EntityRepository
{
    public interface IEntityCommonRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAllList();

        T GetByItem(object item);

        T Insert(T entity);

        T Update(T entity);

        void Remove(T entity);
    }
}