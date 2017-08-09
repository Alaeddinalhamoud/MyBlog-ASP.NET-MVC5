using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public interface ICommentRepository
    {
        void Save(Comment commnet);
        IEnumerable<Comment> CommentIEnum { get; }
        IEnumerable<Comment> Last10Comment { get; }

        Comment Details(int? Id);
        IQueryable<Comment> CommentList { get; }

        Comment Delete(int? Id);
    }
}
