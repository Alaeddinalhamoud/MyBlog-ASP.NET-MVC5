using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{
   public class EmailSetting
    {
        public int Id { get; set; }
        [DisplayName("SMTP Server")]
        public string SMTP_Server { get; set; }
        [DisplayName("Sender")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Sender { get; set; }
        [DisplayName("SMTP Port")]
        public int  SMTPServer_Port { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password  { get; set; }
        [DisplayName("Enable SSL")]
        public bool EnableSSL { get; set; }
        [DisplayName("Last Update By")]
        public int UserId { get; set; }
        [DisplayName("Last Update")]
        public DateTime Last_Update { get; set; }
        public virtual User UserDetails { get; set; }
    }
}
