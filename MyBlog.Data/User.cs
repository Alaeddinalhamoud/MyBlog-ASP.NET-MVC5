using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{

    public class User
    {
        public int UserId { get; set; }
        [DisplayName("First Name")]
        public string FName { get; set; }
        [DisplayName("Second Name")]
        public string LName { get; set; }
     //   [Index(IsUnique = true)]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
       
        public string Email { get; set; }


        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        [DisplayName("Create Time")]
        public DateTime Create_time { get; set; }
        [DisplayName("Update Time")]
        public DateTime Update_Time{get;set;}
        [DisplayName("Last Login")]
        public DateTime Last_Login { get; set; }
        [DisplayName("Role")]
        public int RoleId { get; set; }

        virtual public Role RoleDetails { get; set; }
        virtual public IEnumerable<Role> IENUMRoleDetails { get; set; }

    }
}
