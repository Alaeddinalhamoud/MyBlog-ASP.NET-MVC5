
using MyBlog.Data;
using MyBlog.Service;
using MyBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using X.PagedList;

namespace MyBlog.UI.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageRepository imageRepository;
        public ImageController(IImageRepository repo)
        {
            imageRepository=repo  ;
        }
        // GET: Image
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)

        {
            IEnumerable<Image> model = imageRepository.ImageList
                .OrderBy(p => p.Id)
                .OrderByDescending(p => p.Create_time)
                .ToPagedList(page ?? 1, 5); 
            return View(model);
        }
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult UploadImage()
        {
            return PartialView("_UploadImage");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            string _imgname = string.Empty;

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);
                   
                    _imgname = Guid.NewGuid().ToString();
                  
                    var _comPath = Server.MapPath("/Upload/Id_") + _imgname + _ext;
                    _imgname = "Id_" + _imgname + _ext;
                  
                    ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);
                    var _lenght = new FileInfo(path).Length;
                    //here to add Image Path to You Database ,
                    Image data = new Image();
                    data.Imagepath = _imgname;
                    data.Size = Convert.ToInt32(_lenght);
                    data.Create_time = DateTime.Now;

                     
                    bool result;
                    result= AddImage(data);
                    if (result==true)
                    {
                        TempData["message"] = string.Format("Image was Added Successfully");
                    }
                    // resizing image
               //     MemoryStream ms = new MemoryStream();
                //    WebImage img = new WebImage(_comPath);

               //     if (img.Width > 200)
               //         img.Resize(200, 200);
              //      img.Save(_comPath);
                    // end resize
                }
            }
            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Image image = imageRepository.Details(Id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int? Id)
        {

            Image image = imageRepository.Delete(Id);
            if (image != null)
            {
                TempData["message"] = string.Format("deleted");
            }
            return RedirectToAction("Index", "Image");
        }
        [Authorize(Roles = "Admin,SuperUser")]
        public bool AddImage(Image data)
        {
            var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
            int _CurrentUserId = Convert.ToInt32(identity.User.UserId);

          
           
            bool res=true;

            if (data != null)
            {
                Image obj = GetImageSession();
                obj.Id = data.Id;
                obj.Imagepath = data.Imagepath;
                obj.Size = data.Size;
                obj.Create_time = data.Create_time;
                obj.UserId = _CurrentUserId;
                imageRepository.Save(obj);
                int? Newid = obj.Id;
               
                 res = true;
            }
            else
            {
                 res = false;
            }
            return res;
        }



        ///Sessions
        ///

        private Image GetImageSession()
        {
            if (Session["image"] == null)
            {
                Session["image"] = new Image();
            }
            return (Image)Session["image"];
        }


    }
}