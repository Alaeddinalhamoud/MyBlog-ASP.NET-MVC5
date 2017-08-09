using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{
  public  class Widget
    {
        public int WidgetId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        public string WidgetName { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [DisplayName("Content")]
        [DataType(DataType.MultilineText)]
        public  string WidgetContent { get; set; }
        [Required(ErrorMessage = "Update time is required")]
        [DisplayName("Update Time")]
        public DateTime Update_Time { get; set; }
 
        [DisplayName("Update By")]
        public int UserId { get; set; }


        public virtual  User UserDetails { get; set; }
    }
}
