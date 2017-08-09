using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{
  public  class UserRole
    {
        [Key]
        public int UserRolesId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get;set;}
    }
}
