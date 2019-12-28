using MyBlogProject.Business.Abstract;
using MyBlogProject.DataAccess.Abstract;
using MyBlogProject.Entities.Models;
using System.Collections.Generic;

namespace MyBlogProject.Business.Concrete.Manager
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<User> GetAllList()
        {
            return _userDal.GetList();
        }

        public User GetByItem(object item)
        {
            return _userDal.Get(c => c.UserId == (int)item);
        }

        public User Insert(User entity)
        {
            _userDal.Add(entity);
            return entity;
        }

        public void Remove(User entity)
        {
            _userDal.Delete(entity);
        }

        public User Update(User entity)
        {
            _userDal.Update(entity);
            return entity;
        }
    }
}