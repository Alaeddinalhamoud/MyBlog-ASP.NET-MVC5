
using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public interface IEmailSettingRepository
    {
        EmailSetting GetEmailSetting { get; }
        void Save(EmailSetting Emailsetting);
        EmailSetting  Details(int? Id);
    }
}
