using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{
  public  class Page
    {
        [Key]
      public  int PageId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [DisplayName("Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [DisplayName("Content")]
        public string PagesContent { get; set; }
        [Required(ErrorMessage = "Create Time is required")]
        [DisplayName("Create Time")]
        public DateTime Create_Time { get; set; }
        [Required(ErrorMessage = "Update time is required")]
        [DisplayName("Update time")]
        public DateTime Update_Time { get; set; }
     
        [DisplayName("Updated By")]
        public int UserId { get; set; }

        public virtual User UserDetail { get; set; }
        //virtual public User UserDetail { get; set; }
    }
}
