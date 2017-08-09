using MyBlog.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.UI.Models
{
    public class ChartMVL
    {
     public   IEnumerable<Post> Posts { get; set; }
     public   IEnumerable<Comment> Comments { get; set; }

    }

    public class ChartList3Felid
    {
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public int Month { get; set; }
    }
}