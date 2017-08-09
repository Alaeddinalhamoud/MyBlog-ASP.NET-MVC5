
using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
   public interface IPostRepository
    {
        Task SaveAsync(Post post);
        IEnumerable<Post> PostIEnum { get; }
        IEnumerable<Post> LastPost { get; }

         Post Details(int? Id);
        IQueryable<Post> PostList { get; }
        
        Post Delete(int? Id);
        //To increase number of commnet one after add new comment
        void IncreaseFreqOne(int Id);
        void DecreaseFreqOne(int Id);

    }
}
