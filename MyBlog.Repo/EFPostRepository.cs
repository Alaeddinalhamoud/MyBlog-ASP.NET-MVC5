
using MyBlog.Data;
using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Repo
{
    public class EFPostRepository : IPostRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Post> LastPost
        {
            get
            {
                context.Database.Log = s => Debug.WriteLine(s);
                return  context.Posts.OrderByDescending(c => c.Create_time).Take(10);
            }
        }

        public IEnumerable<Post> PostIEnum
        {
            get
            {
                return context.Posts;
            }
        }

        public  IQueryable<Post> PostList
        {

            get
            {
                context.Database.Log = s => Debug.WriteLine(s);
                return    context.Posts.AsQueryable();
              
            }
        }

        public Post Delete(int? Id)
        {
            Post dbEntry = context.Posts.Find(Id);
            if(dbEntry != null)
            {
                context.Posts.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public Post Details(int? Id)
        {
            Post dbEntry = context.Posts.Find(Id);
            return dbEntry;
        }

        public void IncreaseFreqOne(int Id)
        {
            if (Id != 0)
            {
                Post model = new Post();
                model = context.Posts.Find(Id);
                model.Frequence = model.Frequence + 1;
                context.SaveChanges();
            }
        }
        public void DecreaseFreqOne(int Id)
        {
            Post model = new Post();
            if (Id != 0)
            {


                model = context.Posts.Find(Id);
                if (model.Frequence != 0)
                  { 
                       model.Frequence = model.Frequence - 1;
                  }
                context.SaveChanges();
            }
        }

        public async Task SaveAsync(Post post)
        {
           
            if (post.PostId==0)
            {
                var _post = new Post();//To get Post ID From AddPost to use it for Details
                _post.Title = post.Title;
                _post.Post_Content = post.Post_Content;
                _post.Create_time = post.Create_time;
                _post.Update_time = post.Update_time;
                _post.UserId = post.UserId;
                _post.Tages = post.Tages;
                _post.CategoryId = post.CategoryId;
                _post.FeaturedImage = post.FeaturedImage;
                context.Posts.Add(_post);
              await  context.SaveChangesAsync();
                post.PostId = _post.PostId;
            }
            else
            {
                Post dbEntry = context.Posts.Find(post.PostId);
                if(dbEntry != null)
                {
                    dbEntry.PostId = post.PostId;
                    dbEntry.Title = post.Title;
                    dbEntry.Post_Content = post.Post_Content;
                    dbEntry.Create_time = post.Create_time;
                    dbEntry.Update_time = post.Update_time;
                    dbEntry.Tages = post.Tages;
                    dbEntry.CategoryId = post.CategoryId;
                    dbEntry.FeaturedImage = post.FeaturedImage;
                await    context.SaveChangesAsync();
                    
                }
            }
        }
 
    }
}
