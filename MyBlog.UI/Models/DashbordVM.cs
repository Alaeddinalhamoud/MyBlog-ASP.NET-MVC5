
using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.UI.Models
{
    public class DashbordVM
    {
        public User User { get; set; }
        public int NumberOfNewPost { get; set; }
        public int NumberOfNewComment { get; set; }
        public int NumberOfNewCategory { get; set; }
        public int NumberOfNewUser { get; set; }
        public int NumberOfCommentNeedApprove { get; set; }
    }
}