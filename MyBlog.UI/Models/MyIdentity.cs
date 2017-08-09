
using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.UI.Models
{
  public  class MyIdentity: IIdentity
    {
        public IIdentity Identity { get; set; }
        public User User { get; set; }

        public MyIdentity(User user)
        {
            Identity = new GenericIdentity(user.Email);
            User = user;
        }

        public string AuthenticationType
        {
            get { return Identity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return Identity.IsAuthenticated; }
        }

        public string Name
        {
            get { return Identity.Name; }
        }
    }
}

