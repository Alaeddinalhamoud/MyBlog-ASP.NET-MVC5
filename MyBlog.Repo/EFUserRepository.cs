
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using MyBlog.Service;
using MyBlog.Data;

namespace MyBlog.Repo
{
    public class EFUserRepository : IUserRepository
    {
        EFDbContext context = new EFDbContext();
        bool Succeeded;

        public IEnumerable<User> UserIEmum
        {
            get
            {
                return context.Users;
            }
        }

        public IQueryable<User> UserList
        {
            get
            {
                return context.Users.AsQueryable();
            }
        }

        public bool  Save(User user)
        {
          
           
            if (user.UserId == 0)
            {
                User _user = new User();
               // _user.UserId = user.UserId;
                _user.FName = user.FName;
                _user.LName = user.LName;
                _user.Email = user.Email;
              
                _user.Password = user.Password;
              
                _user.Create_time = user.Create_time;
               _user.Update_Time = user.Update_Time;// just create time no need update
                _user.Last_Login = user.Last_Login;
                _user.RoleId = user.RoleId;
                context.Users.Add(_user);
                user.UserId = _user.UserId;
              
               int res = context.SaveChanges();

                if (res> 0 )                 
                      
                {
                    Succeeded=  true;
                }
                else
                {
                    Succeeded= false;
                }
               
            }
            else
            {
                User dbEntry = context.Users.Find(user.UserId);
                if(dbEntry != null)
                {
                    dbEntry.UserId = user.UserId;
                    dbEntry.FName = user.FName;
                    dbEntry.LName = user.LName;
                    dbEntry.Email = user.Email;
                   
                    dbEntry.Password = user.Password;
                
                    dbEntry.Create_time = user.Create_time;
                    dbEntry.Update_Time = user.Update_Time;
                    dbEntry.Last_Login = user.Last_Login;
                    dbEntry.RoleId = user.RoleId;
                  //  context.Users.Add(dbEntry);
                    
                    if (context.SaveChanges() > 0)
                    {
                        Succeeded = true;
                    }
                    else
                    {
                        Succeeded = false;
                    }
                    user.UserId = dbEntry.UserId;
                }
            }
            
           
            return Succeeded;
        }

        public User Details(int? Id)
        {
            User dbEntry = context.Users.Find(Id);

            return dbEntry;
        }

        public User Delete(int? Id)
        {
            User dbEntry = context.Users.Find(Id);
            if (dbEntry != null)
            {
                context.Users.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public bool UniqueEmail(string email)
        {
            var result = true;
            var res= context.Users.FirstOrDefault(x => x.Email == email);
            if(res !=null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
