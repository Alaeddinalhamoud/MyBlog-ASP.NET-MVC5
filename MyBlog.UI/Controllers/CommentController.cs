
using MyBlog.Data;
using MyBlog.Service;
using MyBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;


namespace MyBlog.UI.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository repositoryCommment;
        private readonly IPostRepository repositoryPost;

        public CommentController(ICommentRepository repoComment,IPostRepository repoPost)
        {
            repositoryCommment = repoComment;
            repositoryPost = repoPost;
        }
        // GET: Comment
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)
        {
            IEnumerable<Comment> model = repositoryCommment.CommentList.OrderBy(p => p.CommentId)
            .OrderByDescending(p => p.Create_time).ToPagedList(page ?? 1, 5);//5 is pagesize
         /*
          * //Fututer Plan to Delete All SuperUser ActionResult ,
          *  Check RoleId after send him to page only
           var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
            int _CurrentUserRole = Convert.ToInt32(identity.User.RoleId);
             */

            return View(model);
        }
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult SuperUserIndex(int? page)
        {

            IEnumerable<Comment> model = repositoryCommment.CommentList.OrderBy(p => p.CommentId)
            .OrderByDescending(p => p.Create_time).ToPagedList(page ?? 1, 5);//5 is pagesize

            return View(model);
            
        }
      
        [Authorize(Roles = "User,SuperUser,Admin")]
        public ActionResult AddNewComment()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,SuperUser,Admin")]
        public ActionResult AddNewComment(int? Id,Comment data)
        {

            var identity = (HttpContext.User as MyPrincipal).Identity as  MyIdentity;
            int _CurrentUserId = Convert.ToInt32(identity.User.UserId);


           

               
                Comment obj = GetCommentSession();
                
                obj.CommentId = data.CommentId;
            if(String.IsNullOrEmpty(data.Comment_Content))
            {
                TempData["message"] = string.Format(" Write Commnet Before Publish it .");
                return View();
            }
                obj.Comment_Content = data.Comment_Content;
                int PostId =Convert.ToInt32(Id);
               
                obj.Update_time = DateTime.Now;//Need solution for this field no need any value.
               
                obj.PostId = PostId;//PostId
                
                if(obj.PostId ==0)
                {

                    obj.PostId = data.PostId;
                   
                }
                if (obj.CommentId == 0)
                {
                    obj.Publish = false; //New Commnet need aprove(Dash) to publish it
                    obj.Create_time = DateTime.Now;
                    obj.UserId = Convert.ToInt32(_CurrentUserId);//SaME uSER Wrote it
                    //to increase frq for post , how many comment for post
                    repositoryPost.IncreaseFreqOne(obj.PostId);
                }

                else {
                    obj.Create_time = data.Create_time;
                    obj.UserId = data.UserId;
                    obj.Publish = false; //Edite  Commnet need aprove(Dash) to publish it ,again
                }
                
                repositoryCommment.Save(obj);
                int? Newid = obj.CommentId;
                if (obj != null)
                {
                    if (data.CommentId == 0)
                    {
                        TempData["message"] = string.Format(" Added Successfully , it's on Waiting list to aprove it.");
                    }
                    else
                    {
                        TempData["message"] = string.Format(" Edited Successfully, it's on Waiting list to aprove it.");
                    }
                }
                return RedirectToAction("Details","Post", new { Id = obj.PostId });
           
        }
        [Authorize(Roles = "User,SuperUser,Admin")]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment model = repositoryCommment.Details(Id);
           
            if (model == null)
            {
                return HttpNotFound();
            }
            //Send you to NewComment page.chtml to save copy same page 
            return View("AddNewComment", model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Comment _Comment = repositoryCommment.Details(Id);

            if (_Comment == null)
            {
                return HttpNotFound();
            }
            return View(_Comment);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int? Id)
        {

            Comment _Comment = repositoryCommment.Delete(Id);
            repositoryPost.DecreaseFreqOne(_Comment.PostId);
            if (_Comment != null)
            {
                TempData["message"] = string.Format("deleted");
            }
            return RedirectToAction("Details", "Post", new { Id = _Comment.PostId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CommentNeedAprove(int? page)
        { 
            IEnumerable<Comment> model = repositoryCommment.CommentList
                .Where(p => p.Publish == false)//just what need aprove
                .OrderBy(p => p.CommentId)
           .OrderByDescending(p => p.Create_time).ToPagedList(page ?? 1, 5);//5 is pagesize
            
            return View(model); 
        }
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult SuperUserCommentNeedAprove(int? page)
        {
            IEnumerable<Comment> model = repositoryCommment.CommentList
               .Where(p => p.Publish == false)//just what need aprove
               .OrderBy(p => p.CommentId)
          .OrderByDescending(p => p.Create_time).ToPagedList(page ?? 1, 5);//5 is pagesize

            return View(model);

        }
        [ValidateInput(false)]
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult PublishComment(int? Id)
        {
            var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
            int _CurrentUserRole = Convert.ToInt32(identity.User.RoleId);

            Comment _Comment = repositoryCommment.Details(Id);
            _Comment.Publish = true;//Aprove 
            repositoryCommment.Save(_Comment);
            
                TempData["message"] = string.Format(" Published Successfully");

            if (_CurrentUserRole == 1) { 
             return RedirectToAction("CommentNeedAprove", "Comment");
            }
            else
            {//SuperUser Role Page
                return RedirectToAction("SuperUserCommentNeedAprove", "Comment");
            }
        }
              

           
        
        ///Sessions
        ///
        private Comment GetCommentSession()
        {
            if (Session["commnet"] == null)
            {
                Session["commnet"] = new Comment();
            }
            return (Comment)Session["commnet"];
        }
    }
}