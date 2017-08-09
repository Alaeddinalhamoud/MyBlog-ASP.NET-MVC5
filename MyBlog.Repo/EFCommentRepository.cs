using MyBlog.Data;

using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Repo
{
    public class EFCommentRepository : ICommentRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Comment> CommentIEnum
        {
            get
            {
                return context.Comments;
            }
        }

        public IQueryable<Comment> CommentList
        {
            get
            {
                return context.Comments.AsQueryable();
            }
        }

        public IEnumerable<Comment> Last10Comment
        {
            get
            {
                return context.Comments.OrderByDescending(c => c.Create_time).Take(10);
            }
        }

        public Comment Details(int? Id)
        {
            Comment dbEntry = context.Comments.Find(Id);

            return dbEntry;
        }

        public Comment Delete(int? Id)
        {
            Comment dbEntry = context.Comments.Find(Id);
            if (dbEntry != null)
            {
                context.Comments.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void Save(Comment commnet)
        {


            if (commnet.CommentId == 0)
            {
                Comment _Comment = new Comment();
                _Comment.Comment_Content = commnet.Comment_Content;
                _Comment.Create_time = commnet.Create_time;
                _Comment.Update_time = commnet.Update_time;
                
                _Comment.UserId = commnet.UserId;
                _Comment.PostId = commnet.PostId;
                _Comment.Publish = commnet.Publish;
                
                context.Comments.Add(_Comment);

                context.SaveChanges();



            }
            else
            {
                Comment dbEntry = context.Comments.Find(commnet.CommentId);
                if (dbEntry != null)
                {
                    dbEntry.CommentId = commnet.CommentId;
                    dbEntry.Comment_Content = commnet.Comment_Content;
                    dbEntry.Update_time = commnet.Update_time;
                    dbEntry.UserId = commnet.UserId;
                    dbEntry.PostId = commnet.PostId;
                    //  dbEntry.Create_time = category.Create_time;

                    //   context.Categories.Add(dbEntry);
                    dbEntry.Publish = commnet.Publish;
                    context.SaveChanges();
                    commnet.CommentId = dbEntry.CommentId;
                }
            }


        }
    }
}
