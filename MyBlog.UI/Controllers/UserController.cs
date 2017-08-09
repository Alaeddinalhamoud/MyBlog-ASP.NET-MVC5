
using MyBlog.Data;
using MyBlog.Service;
using MyBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.UI.Controllers
{
   
    public class UserController : Controller
    {
        // GET: User
        private readonly IUserRepository repositoryUser;
        private readonly IRoleRepository repositoryRole;
        private readonly IDEncryptionRepository repositoryDEncryption;
        private readonly ICommentRepository repositoryComment;

        public UserController(IUserRepository repoUser,
                              IRoleRepository repoRole,
                              IDEncryptionRepository repoDEncryption,
                              ICategoryRepository repoCategory,
                              ICommentRepository repoComment)
        {
            repositoryUser = repoUser;
            repositoryRole = repoRole;
            repositoryDEncryption = repoDEncryption;
            repositoryComment = repoComment;

        }
        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            var model = repositoryUser.UserList.OrderBy(p => p.UserId)
            .OrderByDescending(p => p.Create_time); ;
            return View(model);
        }
        //public ActionResult Index_old(int page = 1)
        //{
        //    int PageSize = 5;
        //    PostViewModel model = new PostViewModel
        //    {
        //        Users = repositoryUser.UserIEmum
        //        .OrderBy(p => p.UserId)
        //        .OrderByDescending(p => p.Create_time)
        //        .Skip((page - 1) * PageSize)
        //        .Take(PageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = PageSize,
        //            TotalItems = repositoryUser.UserList.Count()
        //        },
        //    };
        //    return View(model);
        //}

        //public ActionResult GetUser()
        //{
        //    IEnumerable<User> Model = repositoryUser.UserList;
        //    return View(Model);
        //}
        [Authorize(Roles = "User,SuperUser,Admin")]
        public ActionResult Details(int? Id)
        {

            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = repositoryUser.Details(Id);
         

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
           
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = repositoryUser.Details(Id);
            model.IENUMRoleDetails = repositoryRole.RoleIEnum;
            model.Password = repositoryDEncryption.Decrypt(model.Password);


            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(User data)
        {
            if (ModelState.IsValid)
            {
                User obj = GetUserSession();
                obj.UserId = data.UserId;
                obj.FName = data.FName;
                obj.LName = data.LName;
                obj.Email = data.Email;

                obj.Password =repositoryDEncryption.Encrypt(data.Password);
                obj.Create_time = data.Create_time;
                obj.Update_Time = DateTime.Now;//Need solution for this field no need any value
                obj.Last_Login = data.Last_Login;
                obj.RoleId = data.RoleId;
                repositoryUser.Save(obj);
                int? Newid = obj.UserId;
                if (obj != null)
                {
                    TempData["message"] = string.Format("{0} was Edited Successfully", obj.FName+ " "+obj.LName);
                   
                }
                return RedirectToAction("Details", new { Id = Newid });
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            User user = repositoryUser.Details(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int? Id)
        {

            User user = repositoryUser.Delete(Id);
            if (user != null)
            {
                TempData["message"] = string.Format("{0} was deleted", user.FName+" "+user.LName);
            }
            return RedirectToAction("Index", "User");
        }
        [Authorize(Roles = "User,SuperUser,Admin")]
        //UserControl For User Details
        public ActionResult UserFormDetails(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DashbordVM UserAndCommment = new DashbordVM();
            UserAndCommment.User = repositoryUser.Details(Id);
            UserAndCommment.NumberOfCommentNeedApprove= repositoryComment.CommentList.Where(p => p.Publish == false).Count();

            if (UserAndCommment == null)
            {
                return HttpNotFound();
            }
            return PartialView("_UserFormDetails", UserAndCommment);
        }

        private User GetUserSession()
        {
            if (Session["user"] == null)
            {
                Session["user"] = new User();
            }
            return (User)Session["user"];
        }
    }
}