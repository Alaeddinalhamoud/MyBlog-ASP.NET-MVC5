using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.UI.Models
{
    public class CommentViewModel
    {

        public IEnumerable<Comment> Comments { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int? Id { get; set; }

    }
 
}