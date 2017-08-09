using MyBlog.Data;
using MyBlog.Service;
using MyBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;


namespace MyBlog.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingRepository repositorySetting;
        private readonly IEmailSettingRepository repositoryEmailSetting;
        private readonly IDEncryptionRepository repositoryDEncrption;
        public SettingController(ISettingRepository repoSetting,
                                 IEmailSettingRepository repoEmailSetting,
                                 IDEncryptionRepository repoDEncrption)
        {
            repositorySetting = repoSetting;
            repositoryEmailSetting = repoEmailSetting;
            repositoryDEncrption = repoDEncrption;
        }
        // GET: Setting
       
        public ActionResult Index()
        {
            return View();
        }
        //This Action Should Run once only Install time .
        public ActionResult CreateSetting()
        {
            Setting model = new Setting();
            model.Id = 0;
            model.Update_Time = DateTime.Now.Date;
            return View("UpdateSetting", model);
        }
        [HttpPost]
        public ActionResult UpdateSetting(Setting data )
        {
            Setting obj = GetSettingSession();

            var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
            int _CurrentUserId = Convert.ToInt32(identity.User.UserId);
            if (_CurrentUserId == 0)
            {
                //becouse Sometime id = 0 ?????!!!! maybe session die???????
                return View(data);
            }
            if (ModelState.IsValid)
            {
                obj.Id = data.Id;
                obj.HomeImage = data.HomeImage;
                obj.HomeImageText = data.HomeImageText;
                obj.NumberOfLastPost = data.NumberOfLastPost;
                obj.NumberOfCategory = data.NumberOfCategory;
                obj.PostNumberInPage = data.PostNumberInPage;
                obj.NumberOfTopPost = data.NumberOfTopPost;
                obj.Update_Time = DateTime.Now;
                obj.UserId = _CurrentUserId;
                //obj.DisplayLastCategory = data.DisplayLastCategory;
                //obj.DisplayLastPost = data.DisplayLastPost;
                //obj.DisplayFbWidget = data.DisplayFbWidget;
                //obj.DisplayTwWidget = data.DisplayTwWidget;
                //obj.DisplayGoogleWidget = data.DisplayGoogleWidget;
                //obj.FBAppID = data.FBAppID;
                //obj.FBAppSecret = data.FBAppSecret;
                //obj.GoogleSitekey = data.GoogleSitekey;
                //obj.GoogleSecretkey = data.GoogleSecretkey;
                repositorySetting.Save(obj);
                if (obj != null)
                {
                    if (data.Id == 0)
                    {
                        TempData["message"] = string.Format("Added Successfully");
                    }
                    else
                    {
                        TempData["message"] = string.Format("Edited Successfully");
                    }
                }
                return RedirectToAction("Details", "Setting",data);//SamePlace
            }

            return View(data);
        }
        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting model = repositorySetting.Details(Id);

            // model.UserDetails.FName
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("UpdateSetting",model);
        }
        //This Action Should Run once only Install time ........******//////
        public ActionResult CreateEMailSetting()
        {
            EmailSetting model = new EmailSetting();
            model.Id = 0;
            model.Last_Update = DateTime.Now.Date;
            return View("UpdateEMailSetting", model);
        }
        [HttpPost]
        public ActionResult UpdateEmailSetting(EmailSetting data)
        {
            EmailSetting obj = GetEmailSettingSession();

            var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
            int _CurrentUserId = Convert.ToInt32(identity.User.UserId);
            if (_CurrentUserId == 0)
            {
                //becouse Sometime id = 0 ?????!!!! maybe session die???????
                return View(data);
            }
            if (ModelState.IsValid)
            {
                obj.Id = data.Id;
                obj.SMTP_Server = data.SMTP_Server;
                obj.Sender = data.Sender;
                obj.SMTPServer_Port = data.SMTPServer_Port;
                obj.UserName = data.UserName;
                string HashPassword = repositoryDEncrption.Encrypt(data.Password);
                obj.Password = HashPassword;
                obj.EnableSSL = data.EnableSSL;
                obj.Last_Update = DateTime.Now;
                obj.UserId = _CurrentUserId;
                
                repositoryEmailSetting.Save(obj);
                if (obj != null)
                {
                    if (data.Id == 0)//New or Update
                    {
                        TempData["message"] = string.Format("Added Successfully");
                    }
                    else
                    {
                        TempData["message"] = string.Format("Edited Successfully");
                    }
                }
                return RedirectToAction("EmailSettingDetails", "Setting", data);//SamePlace
            }

            return View(data);
        }
        //Advance Setting For External Apps
        public ActionResult AdvancedSettings()
        {
            //AdvancedSettings _AdvancedSettings=new AdvancedSettings();
            AdvancedSettings _AdvancedSettings = GetAdvanceSettingSession();

            //Read setting 
            string _DisplayLastCategory = ReadSetting("DisplayLastCategory");
            string _DisplayLastPost= ReadSetting("DisplayLastPost");
            string _DisplayFbWidget = ReadSetting("DisplayFbWidget");
            string _DisplayTwWidget = ReadSetting("DisplayTwWidget");
            string _DisplayGoogleAdv = ReadSetting("DisplayGoogleAdv");
            string _DisplaydisqusWidget = ReadSetting("DisplaydisqusWidget");
            string _DisplayLocalCommentWidget = ReadSetting("DisplayLocalCommentWidget");
            string _DisplayFBLogin = ReadSetting("DisplayFBLogin");
            string _DisplayRegister = ReadSetting("DisplayRegister");


            //Send it to to model
            //  Setting to Show and Hiden Widgets

            if (_DisplayLastCategory == "none")
            { _AdvancedSettings.DisplayLastCategory = false; }
            else { _AdvancedSettings.DisplayLastCategory = true; }
           
            if (_DisplayLastPost == "none")
            { _AdvancedSettings.DisplayLastPost = false; }
            else { _AdvancedSettings.DisplayLastPost = true; }
            
            if (_DisplayFbWidget == "none")
            { _AdvancedSettings.DisplayFbWidget = false; }
            else { _AdvancedSettings.DisplayFbWidget = true; }
            
            if (_DisplayTwWidget == "none")
            { _AdvancedSettings.DisplayTwWidget = false; }
            else { _AdvancedSettings.DisplayTwWidget = true; }
           
            if (_DisplayGoogleAdv == "none")
            {_AdvancedSettings.DisplayGoogleAdv = false; }
            else { _AdvancedSettings.DisplayGoogleAdv = true; }

            if (_DisplaydisqusWidget == "none")
            { _AdvancedSettings.DisplaydisqusWidget = false; }
            else { _AdvancedSettings.DisplaydisqusWidget = true; }

            if (_DisplayLocalCommentWidget == "none")
            { _AdvancedSettings.DisplayLocalCommentWidget = false; }
            else { _AdvancedSettings.DisplayLocalCommentWidget = true; }


            if (_DisplayFBLogin == "none")
            { _AdvancedSettings.DisplayFBLogin = false; }
            else { _AdvancedSettings.DisplayFBLogin = true; }

            if(_DisplayRegister=="none")
            { _AdvancedSettings.DisplayRegister = false; }
            else { _AdvancedSettings.DisplayRegister = true; }
            ///

            _AdvancedSettings.FBAppID = ReadSetting("FBAppID");
            _AdvancedSettings.FBAppSecret = ReadSetting("FBAppSecret");
            _AdvancedSettings.GoogleSitekey = ReadSetting("GoogleSitekey");
            _AdvancedSettings.GoogleSecretkey = ReadSetting("GoogleSecretkey");


           return View(_AdvancedSettings);
        }
        [HttpPost]
        public ActionResult AdvancedSettings(AdvancedSettings data)
        {


            AdvancedSettings obj = GetAdvanceSettingSession();
            obj = data;
            //  Setting to Show and Hiden Widgets
            string _DisplayLastCategory;
            if (obj.DisplayLastCategory == false)
            { _DisplayLastCategory = "none"; }
            else { _DisplayLastCategory = "block"; }
            string _DisplayLastPost;
            if (obj.DisplayLastPost == false)
            { _DisplayLastPost = "none"; }
            else { _DisplayLastPost = "block"; }
            string _DisplayFbWidget;
            if (obj.DisplayFbWidget == false)
            { _DisplayFbWidget = "none"; }
            else { _DisplayFbWidget = "block"; }
            string _DisplayTwWidget;
            if (obj.DisplayTwWidget == false)
            { _DisplayTwWidget = "none"; }
            else { _DisplayTwWidget = "block"; }
            string _DisplayGoogleAdv;
            if (obj.DisplayGoogleAdv == false)
            { _DisplayGoogleAdv = "none"; }
            else { _DisplayGoogleAdv = "block"; }
            string _DisplaydisqusWidget;
            if (obj.DisplaydisqusWidget == false)
            { _DisplaydisqusWidget = "none"; }
            else { _DisplaydisqusWidget = "block"; }
            string _DisplayLocalCommentWidget;
            if (obj.DisplayLocalCommentWidget == false)
            { _DisplayLocalCommentWidget = "none"; }
            else { _DisplayLocalCommentWidget = "block"; }
            string _DisplayFBLogin;
            if (obj.DisplayFBLogin == false)
            { _DisplayFBLogin = "none"; }
            else { _DisplayFBLogin = "block"; }

            string _DisplayRegister;
            if (obj.DisplayRegister == false)
            { _DisplayRegister = "none"; }
            else { _DisplayRegister = "block"; }


            if (_DisplaydisqusWidget == _DisplayLocalCommentWidget)
            {
                TempData["message"] = string.Format("Please notice , you should Active one of the Comment Widgets.");
                return View();
            }
            if ((data.FBAppSecret==null) || (data.GoogleSecretkey == null))
            {
                TempData["message"] = string.Format("Please Check you FBAppSecret and GoogleSecretkey ");
                return View();
            }


            //New Key or Update
            AddUpdateAppSettings("DisplayLastCategory", _DisplayLastCategory);
            AddUpdateAppSettings("DisplayLastPost", _DisplayLastPost);
            AddUpdateAppSettings("DisplayFbWidget", _DisplayFbWidget);
            AddUpdateAppSettings("DisplayTwWidget", _DisplayTwWidget);
            AddUpdateAppSettings("DisplayGoogleAdv", _DisplayGoogleAdv);
            AddUpdateAppSettings("DisplaydisqusWidget", _DisplaydisqusWidget);
            AddUpdateAppSettings("DisplayLocalCommentWidget", _DisplayLocalCommentWidget);
            AddUpdateAppSettings("DisplayFBLogin", _DisplayFBLogin);
            AddUpdateAppSettings("DisplayRegister", _DisplayRegister);
            AddUpdateAppSettings("FBAppID", obj.FBAppID);
            AddUpdateAppSettings("FBAppSecret", obj.FBAppSecret);
            AddUpdateAppSettings("GoogleSitekey", obj.GoogleSitekey);
            AddUpdateAppSettings("GoogleSecretkey", obj.GoogleSecretkey);
          

            TempData["message"] = string.Format("Updated Successfully");


            return View();
        }
        //ReadSetting From  webconfig
        public string  ReadSetting(string key)
        {
           
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
            return result;
            
        }

        static void AddUpdateAppSettings(string key, string value)
        {
          
                var configFile = WebConfigurationManager.OpenWebConfiguration("~/");
              var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            
        

        public ActionResult EmailSettingDetails(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailSetting model = repositoryEmailSetting.Details(Id);
           

            // model.UserDetails.FName
            if (model == null)
            {
                return HttpNotFound();
            }
            model.Password = repositoryDEncrption.Decrypt(model.Password);
            return View("UpdateEMailSetting", model);
        }
        ///Sessions
        ///
        private Setting GetSettingSession()
        {
            if (Session["setting"] == null)
            {
                Session["setting"] = new Setting();
            }
            return (Setting)Session["setting"];
        }
        private EmailSetting GetEmailSettingSession()
        {
            if (Session["Emailsetting"] == null)
            {
                Session["Emailsetting"] = new EmailSetting();
            }
            return (EmailSetting)Session["Emailsetting"];
        }

        private AdvancedSettings GetAdvanceSettingSession()
        {
            if (Session["advancesetting"] == null)
            {
                Session["advancesetting"] = new AdvancedSettings();
            }
            return (AdvancedSettings)Session["advancesetting"];
        }

    }
}