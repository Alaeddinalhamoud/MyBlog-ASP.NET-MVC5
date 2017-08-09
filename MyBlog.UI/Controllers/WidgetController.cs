
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
    public class WidgetController : Controller
    {

        private readonly IWidgetRepository repositoryWidget;
         
        public WidgetController(IWidgetRepository repoWidget)
        {
            repositoryWidget = repoWidget;
            
        }
        // GET: Widget
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Widget> model = repositoryWidget.WidgetList;
             
        
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateWidget()
        {
            Widget model = new Widget();
            model.WidgetId = 0;
            model.Update_Time = DateTime.Now.Date;
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateWidget(Widget data)
        {
            Widget obj = GetWidgetSession();

            var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
            int _CurrentUserId = Convert.ToInt32(identity.User.UserId);
            if (_CurrentUserId == 0)
            {
                //becouse Sometime id = 0 ?????!!!! maybe session die???????
                return View(data);
            }
            if (ModelState.IsValid)
            {
                obj.WidgetId = data.WidgetId;
                obj.WidgetName = data.WidgetName;
                obj.WidgetContent = data.WidgetContent;
                obj.Update_Time = DateTime.Now;
                obj.UserId = _CurrentUserId;
                repositoryWidget.Save(obj);
                if (obj != null)
                {
                    if (data.WidgetId == 0)
                    {
                        TempData["message"] = string.Format("Added Successfully");
                    }
                    else
                    {
                        TempData["message"] = string.Format("Edited Successfully");
                    }
                }
                return RedirectToAction("Index", "Widget");//SamePlace
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
            Widget model = repositoryWidget.Details(Id);

            // model.UserDetails.FName
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("UpdateWidget", model);
        }

        public ActionResult GetWidget(int? Id)
        {
            Widget Model;
            Model = repositoryWidget.Details(Id); 

            return PartialView("_GetWidget", Model);
        }
      

        private Widget GetWidgetSession()
        {
            if (Session["widget"] == null)
            {
                Session["widget"] = new Widget();
            }
            return (Widget)Session["widget"];
        }
    }
}