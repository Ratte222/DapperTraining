using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        void Create(User user);
        void Delete(int id);
        User Get(int id);
        IEnumerable<User> GetUsers();
        void Update(User user);
    }
}
