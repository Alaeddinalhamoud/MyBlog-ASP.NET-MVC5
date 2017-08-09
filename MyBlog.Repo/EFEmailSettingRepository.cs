using MyBlog.Data;

using MyBlog.Repo;
using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Repo
{
    public class EFEmailSettingRepository : IEmailSettingRepository

    {
        EFDbContext context =new EFDbContext();
        public EmailSetting GetEmailSetting
        {
            get
            {
                return context.EmailSettings.FirstOrDefault<EmailSetting>(); ;
            }
        }

        public EmailSetting Details(int? Id)
        {
            EmailSetting dbEntry = context.EmailSettings.Find(Id);
            return dbEntry;
        }

        public void Save(EmailSetting Emailsetting)
        {
            if (Emailsetting.Id == 0)
            {

                context.EmailSettings.Add(Emailsetting);

                context.SaveChanges();



            }
            else
            {
                EmailSetting dbEntry = context.EmailSettings.Find(Emailsetting.Id);
                if (dbEntry != null)
                {
                    dbEntry.Id = Emailsetting.Id;
                    dbEntry.SMTP_Server = Emailsetting.SMTP_Server;
                    dbEntry.Sender = Emailsetting.Sender;
                    dbEntry.SMTPServer_Port = Emailsetting.SMTPServer_Port;
                    dbEntry.UserName = Emailsetting.UserName;
                    dbEntry.Password = Emailsetting.Password;
                    dbEntry.EnableSSL = Emailsetting.EnableSSL;
                    dbEntry.Last_Update = Emailsetting.Last_Update;
                    dbEntry.UserId = Emailsetting.UserId;
                   

                    context.SaveChanges();

                }
            }

        }
    }
}
