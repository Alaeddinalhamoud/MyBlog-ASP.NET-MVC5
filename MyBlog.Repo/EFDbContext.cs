using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using MyBlog.Data;

namespace MyBlog.Repo
{
 public class EFDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<EmailSetting> EmailSettings { get; set; }



    }
}
