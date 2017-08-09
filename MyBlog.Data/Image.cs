using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{
   public class Image
    {
        public int Id { get; set; }
        public string Imagepath { get; set; }
        public int Size { get; set; }

        [DisplayName("Create Time:")]
        [Column(TypeName = "DateTime2")]
        public  DateTime  Create_time { get; set; }
      
        public int UserId { get; set; }
        public virtual User UserDetails { get; set; }
    }
}
