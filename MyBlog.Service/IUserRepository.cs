
using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
   public interface IUserRepository
    {
        bool Save(User user);
        User Details(int? Id);
        IEnumerable<User> UserIEmum { get; }
        IQueryable<User> UserList { get; }
        User Delete(int? Id);

        bool UniqueEmail(string email);

    }
}
