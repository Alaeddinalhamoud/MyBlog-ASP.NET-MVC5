
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyBlog.Data;
using MyBlog.Repo;
using MyBlog.Service;

namespace MyBlog.Repo
{
    public class EFRoleRepository : IRoleRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Role> RoleIEnum
        {
            get { return context.Roles; }
        }
    }
}
