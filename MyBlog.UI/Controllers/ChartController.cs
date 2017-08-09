
using MyBlog.Data;
using MyBlog.Service;
using MyBlog.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

namespace MyBlog.UI.Controllers
{
    public class ChartController : Controller
    {
        private readonly IPostRepository repositoryPost;
        private readonly ICategoryRepository repositoryICategory;
        private readonly ICommentRepository repositoryCommment;
        private readonly IUserRepository repositoryUser;
        private readonly IImageRepository repositoryImage;
        public ChartController(IPostRepository repoPost, ICategoryRepository repoICategory, ICommentRepository repoComment,IUserRepository repoUser
                                ,IImageRepository repoImage)
           {
            repositoryPost = repoPost;
            repositoryICategory = repoICategory;
            repositoryCommment = repoComment;
            repositoryUser = repoUser;
            repositoryImage = repoImage;
        }
        // GET: Chart
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult Index()
        {
            return View();
          
        }

        [Authorize(Roles = "SuperUser,Admin")]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetChartAllData(string aYear)
        {
            int Year = Convert.ToInt32(aYear);
            // All=User,Post,Comments,Category,Images
            IEnumerable<Post> _post = repositoryPost.PostList.Where(y => y.Create_time.Year == Year);
            IEnumerable<Comment> _comment = repositoryCommment.CommentList.Where(y => y.Create_time.Year == Year);
            IEnumerable<Category> _category=repositoryICategory.CategoryIList.Where(y => y.Create_time.Year == Year);
            IEnumerable<User> _user=repositoryUser.UserList.Where(y => y.Create_time.Year == Year);
            IEnumerable<Image>_image=repositoryImage.ImageList.Where(y => y.Create_time.Year == Year);
            //Post
            int Posttlist = _post.Count();
            //Comments
            int Commenttlist = _comment.Count();
            //Category
            int Categorylist = _category.Count();
            //User
            int Userlist = _user.Count();
            //Image
            int ImageList = _image.Count();



            // var result = repositoryICategory.CategoryIList;
            var chartData = new object[6];

            chartData[0] = new object[]{"Content","Number"};
            chartData[1] = new object[] { "Post", Posttlist };
            chartData[2] = new object[] { "Comment", Commenttlist };
            chartData[3] = new object[] { "Category", Categorylist };
            chartData[4] = new object[] { "User", Userlist };
            chartData[5] = new object[] { "Image", ImageList };


            // return chartData;
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult GetChart()
        //{

        //    var result = repositoryICategory.CategoryIList;

        //    var datachart = new object[result.Count ];
        //  //  datachart[0] = new object[] { "CategoryName", "Frequence" };

        //    int j = 0;

        //    foreach(var i in result)
        //    {

        //        datachart[j] = new object[] { i.CategoryName.ToString(), i.Frequence };
        //        j++;
        //    }

        //    string datastr = JsonConvert.SerializeObject(datachart, Formatting.None);


        //    ViewBag.dataj =new HtmlString(datastr);




        //    return View();
        //}


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetChartUserMonthly(string aYear)
        {
            int Year = Convert.ToInt32(aYear);
            IEnumerable<User> result = repositoryUser.UserList.Where(y => y.Create_time.Year == Year);
            List<User> resultlist = result
              .GroupBy(l => l.Create_time.Month)
              .Select(cl => new User
              {
                  Create_time = cl.First().Create_time,
                  UserId = cl.Count(),
              }).ToList();



            // var result = repositoryICategory.CategoryIList;
            var chartData = new object[resultlist.Count + 1];

            chartData[0] = new object[]{
                    "Monthly",
                    "User"
                };

            int j = 0;
            foreach (var i in resultlist)
            {
                j++;
                chartData[j] = new object[] { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i.Create_time.Month), i.UserId };
            }

            // return chartData;
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetChartImageMonthly(string aYear)
        {
            int Year = Convert.ToInt32(aYear);
            IEnumerable<Image> result = repositoryImage.ImageList.Where(y => y.Create_time.Year == Year);
            List<Image> resultlist = result
              .GroupBy(l => l.Create_time.Month)
              .Select(cl => new Image
              {
                  Create_time = cl.First().Create_time,
                  Id = cl.Count(),
              }).ToList();



            // var result = repositoryICategory.CategoryIList;
            var chartData = new object[resultlist.Count + 1];

            chartData[0] = new object[]{
                    "Monthly",
                    "Image"
                };

            int j = 0;
            foreach (var i in resultlist)
            {
                j++;
                chartData[j] = new object[] { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i.Create_time.Month), i.Id };
            }

            // return chartData;
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public JsonResult GetChartDataPostMonthly()
        //{
        //    IEnumerable<Post> result1 = repositoryPost.PostList.Where(y => y.Create_time.Year == 2016);
        //    List<Post> resultlist = result1
        //      .GroupBy(l => l.Create_time.Month)
        //      .Select(cl => new Post
        //      {
        //          Create_time = cl.First().Create_time,
        //          PostId = cl.Count(),
        //      }).ToList();



        //    // var result = repositoryICategory.CategoryIList;
        //    var chartData = new object[resultlist.Count + 1];

        //    chartData[0] = new object[]{
        //            "Monthly",
        //            "Post"
        //        };

        //    int j = 0;
        //    foreach (var i in resultlist)
        //    {
        //        j++;
        //        chartData[j] = new object[] { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i.Create_time.Month), i.PostId };
        //    }

        //    // return chartData;
        //    return Json(chartData, JsonRequestBehavior.AllowGet);
        //}
        [Authorize(Roles = "SuperUser,Admin")]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetChartDataPostComments(string aYear)
        {
            int  Year =Convert.ToInt32(aYear);

            IEnumerable<Post> PostResult = repositoryPost.PostList.Where(y => y.Create_time.Year == Year);
                IEnumerable<Comment> CommentResult = repositoryCommment.CommentList.Where(y => y.Create_time.Year == Year);
            ChartMVL PostCommentResult = new ChartMVL
            {
                Posts = PostResult,
                Comments = CommentResult
            };

            var PostList = PostCommentResult.Posts
              .GroupBy(l => l.Create_time.Month)
              .Select(cl => new Post
              {
                  Create_time = cl.First().Create_time,
                  PostId = cl.Count(),
              }).ToList();
            var CommentList = PostCommentResult.Comments
             .GroupBy(l => l.Create_time.Month)
             .Select(cl => new Comment
             {
                 Create_time = cl.First().Create_time,
                 CommentId = cl.Count(),
             }).ToList();

           var PostComment = (from post in PostList
                              join comment in CommentList
                              on post.Create_time.Month equals comment.Create_time.Month
                              into temp
                              from comment in temp.DefaultIfEmpty()
                              select new { PostId = post !=null ? post.PostId : 0,
                                           CommentId =comment != null?comment.CommentId :0,
                                           Month = post.Create_time.Month }).ToList();
           
            var chartData = new object[PostComment.Count + 1];

            chartData[0] = new object[]{
                    "Monthly",
                    "Post",
                    "Comment"
                };

            int j = 0;
            foreach (var i in PostComment)
            {
                j++;
                chartData[j] = new object[] { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i.Month), i.PostId,i.CommentId};
            }

          
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

       public ActionResult PostCommentChart()
        {
            return PartialView("_PostCommentChart");
        }
        public ActionResult AllDatabaseChart()
        {
            return PartialView("_AllDatabaseChart");
        }
        public ActionResult UserMonthlyChart()
        {
            return PartialView("_UserMonthlyChart");
        }


        public ActionResult ImageMonthlyChart()
        {
            return PartialView("_ImageMonthlyChart");
        }
    }
}