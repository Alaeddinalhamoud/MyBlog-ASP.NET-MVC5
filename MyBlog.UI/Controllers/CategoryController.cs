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
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository repositoryICategory;
        private readonly ISettingRepository repositorySetting;

        public CategoryController(ICategoryRepository repoICategory, ISettingRepository repoSetting)
        {

            repositoryICategory = repoICategory;
            repositorySetting = repoSetting;
        }

        // GET: Category
        [Authorize(Roles = "Admin")]
        //public ActionResult Index(int page = 1)
        //{
        //    int PageSize = 5;
        //    PostViewModel model = new PostViewModel
        //    {
        //        Categories = repositoryICategory.CategoryIEnum
        //        .OrderBy(p => p.CategoryId)
        //        .OrderByDescending(p => p.Create_time)
        //        .Skip((page - 1) * PageSize)
        //        .Take(PageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = PageSize,
        //            TotalItems = repositoryICategory.CategoryIList.Count()
        //        },
        //    };
        //    return View(model);
        //}

        public ActionResult Index()
        {
            IEnumerable<Category> model = repositoryICategory.CategoryIList.OrderBy(p => p.CategoryId)
            .OrderByDescending(p => p.Create_time); ;

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult NewCategory()
        {
            Category Model = new Category();
            Model.CategoryId = 0;
            return View(Model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult NewCategory(Category data)
        {
             
            Category obj = GetCategorySession();
            if (ModelState.IsValid)
            {
                obj.CategoryId = data.CategoryId;
                obj.CategoryName = data.CategoryName;
                if(obj.CategoryId == 0)
                { 
                obj.Create_time = DateTime.Now;//New Category , need new date
                }
                else
                {
                    obj.Create_time = data.Create_time;//Just read same last date no need change
                      }
                obj.Frequence = 0;//If not 0 will be Null on DB , we cant  do Null +1 .
                 
               repositoryICategory.Save(obj);
                if (obj != null)
                {
                    if (data.CategoryId == 0)
                    {
                        TempData["message"] = string.Format("{0} was Added Successfully", obj.CategoryName);
                    }
                    else
                    {
                        TempData["message"] = string.Format("{0} was Edited Successfully", obj.CategoryName);
                    }
                }
                return RedirectToAction("Index", "Category");
            }

            return View(data);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category model = repositoryICategory.Details(Id);

            if (model == null)
            {
                return HttpNotFound();
            }
            //Send you to NewPost.chtml to save copy same page 
            return View("NewCategory", model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Category category = repositoryICategory.Details(Id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int? Id)
        {
            Category _Category = repositoryICategory.Details(Id);
            if (_Category.Frequence != 0)
            {
                string CommnetError = string.Format("You cannot delete this Post , Because its has {0} Posts", _Category.Frequence.ToString());
                //  ModelState.AddModelError("", CommnetError);
                TempData["message"] = CommnetError;
                return View(_Category);
            }
            Category category = repositoryICategory.Delete(Id);
            if (category != null)
            {
                TempData["message"] = string.Format("{0} was deleted", category.CategoryName);
            }
            return RedirectToAction("Index", "Category");
        }

        [AllowAnonymous]
        [ChildActionOnly]
        
        public ActionResult LastCategory()
        {
            Setting _NumOfCategory;
            _NumOfCategory = repositorySetting.GetSetting;
            IEnumerable<Category> Model;
            Model = repositoryICategory.CategoryIList.Take(_NumOfCategory.NumberOfCategory).Distinct(); ;
            
            return PartialView("_LastCategory", Model);
        }

        ///Sessions
        ///
        private Category GetCategorySession()
        {
            if (Session["categoy"] == null)
            {
                Session["categoy"] = new Category();
            }
            return (Category)Session["categoy"];
        }


    }
}