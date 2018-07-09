using Facebook;
using MyBlog.Data;
using MyBlog.Service;
using MyBlog.UI.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace MyBlog.UI.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAuthentication repositoryIAuthentication;
        private readonly IUserRepository repositoryUser;
        private readonly IPostRepository repositoryPost;
        private readonly ICommentRepository repositoryComment;
        private readonly ICategoryRepository repositoryCategory;
        private readonly MembershipProvider repositoryMemberShipProvider;
        private readonly IDEncryptionRepository repositoryDEncryption;
        private readonly IEmailSettingRepository repositoryEmailSetting;
        private readonly ISettingRepository repositorySetting;
        public AccountController(IUserRepository repoUser,
                                 IAuthentication repoIAuthentication,
                                 IPostRepository repoPost,
                                 ICommentRepository repoComment,
                                 ICategoryRepository repoCategory,
                                 MembershipProvider repoMemberShipProvider,
                                 IDEncryptionRepository repoDEncryption,
                                 IEmailSettingRepository repoEmailSetting,
                                 ISettingRepository repoSetting)
        {
            repositoryUser = repoUser;
            repositoryIAuthentication = repoIAuthentication;
            repositoryCategory = repoCategory;
            repositoryComment = repoComment;
            repositoryPost = repoPost;
            repositoryMemberShipProvider = repoMemberShipProvider;
            repositoryDEncryption = repoDEncryption;
            repositoryEmailSetting = repoEmailSetting;
            repositorySetting = repoSetting;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
          
            DateTime Last24Hours = DateTime.Now.Date.AddHours(-24);
           
            DashbordVM model = new DashbordVM();
            model.NumberOfNewUser = repositoryUser.UserList.Where(p=>p.Create_time >= Last24Hours).Count();// >= 24H to get all users added last 24h
            model.NumberOfNewPost = repositoryPost.PostList.Where(p => p.Create_time >= Last24Hours).Count();
            model.NumberOfNewCategory = repositoryCategory.CategoryIList.Where(p => p.Create_time >= Last24Hours).Count();
            model.NumberOfNewComment = repositoryComment.CommentList.Where(p => p.Create_time >= Last24Hours).Count();
            model.NumberOfCommentNeedApprove = repositoryComment.CommentList.Where(p => p.Publish == false).Count();
            return View(model);
        }
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult SuperUserIndex()
        {
            DateTime Last24Hours = DateTime.Now.Date.AddHours(-24);

            DashbordVM model = new DashbordVM();
            model.NumberOfNewUser = repositoryUser.UserList.Where(p => p.Create_time.Date >= Last24Hours).Count();// >= 24H to get all users added last 24h
            model.NumberOfNewPost = repositoryPost.PostList.Where(p => p.Create_time.Date >= Last24Hours).Count();
            model.NumberOfNewCategory = repositoryCategory.CategoryIList.Where(p => p.Create_time.Date >= Last24Hours).Count();
            model.NumberOfNewComment = repositoryComment.CommentList.Where(p => p.Create_time.Date >= Last24Hours).Count();
            model.NumberOfCommentNeedApprove = repositoryComment.CommentList.Where(p => p.Publish == false).Count();
            return View(model);
        }
        // GET: Account
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }



        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
           
             


                ViewBag.ReturnUrl = returnUrl;
                return View();
           
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            bool isCapthcaValid = ValidateCaptcha(Request["g-recaptcha-response"]);
            if (ModelState.IsValid)
            {
                if (isCapthcaValid)
                {

                    // string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "SHA1");
                    string EncryptedPW = repositoryDEncryption.Encrypt(model.Password.TrimEnd());
                    User _User = null;

                    var IsValidUser = repositoryMemberShipProvider.ValidateUser(model.Email.TrimEnd(), EncryptedPW);

                    if (IsValidUser)
                    {

                        _User = repositoryUser.UserIEmum.Where(a => a.Email.Equals(model.Email.TrimEnd())).FirstOrDefault();
                        ///Last Login///
                        ///
                        _User.Last_Login = DateTime.Now;
                        repositoryUser.Save(_User);
                        ///
					}

                    if (_User != null)
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string data = js.Serialize(_User);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, _User.Email, DateTime.Now, DateTime.Now.AddMinutes(30), model.RememberMe, data);
                        string encToken = FormsAuthentication.Encrypt(ticket);
                        HttpCookie authoCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                        Response.Cookies.Add(authoCookies);
                        if (_User.RoleId == 1)//Admin
                        {
                            return Redirect(returnUrl ?? Url.Action("Index", "Account"));//Admin dash
                        }
                        else if (_User.RoleId == 3)//SuperUser
                        {
                            return Redirect(returnUrl ?? Url.Action("SuperUserIndex", "Account"));//SuperUser Dash
                        }
                        return Redirect(returnUrl ?? Url.Action("Index", "Home"));//User HomePage
                    }

                }
                else
                {
                    ModelState.AddModelError("", "You have put wrong Captcha,Please ensure the authenticity !!!");
                    ModelState.Remove("Password");

                    //Should load sitekey again
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Incorrect username or password");
                ModelState.Remove("Password");

                //Should load sitekey again
                return View();

            }
            return View();
        }




    // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel data)
        {
             var result=false;
            User obj = GetUserSession();
            if (ModelState.IsValid)
            {
                         
                obj.FName = data.FName;
                obj.LName = data.LName;
                bool EmailUniqe = repositoryUser.UniqueEmail(data.Email.TrimEnd());
                if (EmailUniqe==true)
                {
                    ModelState.AddModelError(string.Empty, "Email is Exist, Please Enter New One .");
                    return View(data);
                }
                obj.Email = data.Email.TrimEnd();
                //   string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(data.Password, "SHA1");
                string EncryptedPW = repositoryDEncryption.Encrypt(data.Password.TrimEnd());
                obj.Password = EncryptedPW; 
                obj.Create_time = DateTime.Now;
                obj.Update_Time = DateTime.Now;
                obj.Last_Login= DateTime.Now;
                obj.RoleId = 2;//TO be normal user Role
                result =  repositoryUser.Save(obj);

                ViewBag.UserID = obj.UserId;
                if (result == true)
                {

                    User _User = repositoryUser.UserIEmum.Where(a => a.Email.Equals(data.Email.TrimEnd())).FirstOrDefault();




                    if (_User != null)
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string datajs = js.Serialize(_User);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, _User.Email, DateTime.Now, DateTime.Now.AddMinutes(30), true, datajs);
                        string encToken = FormsAuthentication.Encrypt(ticket);
                        HttpCookie authoCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                        Response.Cookies.Add(authoCookies);
                        return Redirect(Url.Action("Index", "Home"));//User HomePage
                    }

                }

            }

            // If we got this far, something failed, redisplay form
            return View(data);
        }


        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user =repositoryUser.UserIEmum.Where(p => p.Email.Equals(model.Email)).FirstOrDefault();
                if (user != null)
                {
                    //Send Email with password
                    EMailPasswordSender(user.Email, user.Password);
                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", "Your Email is not exists");
                    return View();
                }

               
            }
            

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [Authorize(Roles = "User,Admin,SuperUser")]
        public ActionResult ResetPassword(int? Id)
        {
            var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
            int _CurrentUserId = Convert.ToInt32(identity.User.UserId);
            if (Id == null)
            {
                return View("Error");
            }
            if (Id == _CurrentUserId)
            {
                User _User = repositoryUser.UserIEmum.Where(p => p.UserId.Equals(Id)).FirstOrDefault();
                ResetPasswordViewModel _Email = new ResetPasswordViewModel();
                _Email.Email = _User.Email;
                return View(_Email);
            }
            else
            {
                return View("Error");
            }
           
          //  return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin,SuperUser")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string EncryptedOldPW = repositoryDEncryption.Encrypt(model.OldPassword);
            User _User = repositoryUser.UserIEmum.Where(p => p.Email.Equals(model.Email) && p.Password.Equals(EncryptedOldPW)).FirstOrDefault();
            if(_User != null)
            {
                string EncryptedNewPW = repositoryDEncryption.Encrypt(model.Password);
                _User.Password = EncryptedNewPW;
                repositoryUser.Save(_User);
                //Singout
                FormsAuthentication.SignOut();
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        private User GetUserSession()
        {
            if (Session["user"] == null)
            {
                Session["user"] = new User();
            }
            return (User)Session["user"];
        }



        //Email Sender 
        public void  EMailPasswordSender(string receiver,string Password)
        {
            EmailSetting _emailsetting = repositoryEmailSetting.GetEmailSetting;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(_emailsetting.SMTP_Server);

            mail.From = new MailAddress(_emailsetting.Sender);
            mail.To.Add(receiver);
            mail.Subject = "Your Password";
            string HashUserPassword = repositoryDEncryption.Decrypt(Password);
            mail.Body = HashUserPassword;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = _emailsetting.SMTPServer_Port;
            string HashEmailPassword = repositoryDEncryption.Decrypt(_emailsetting.Password);
            SmtpServer.Credentials = new NetworkCredential(_emailsetting.UserName, HashEmailPassword);
            NetworkCredential Credentials = new NetworkCredential(_emailsetting.Sender, HashEmailPassword);
            SmtpServer.Credentials = Credentials;

            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            SmtpServer.EnableSsl = _emailsetting.EnableSSL;

            SmtpServer.Send(mail);
        }
        //FaceBook login///
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        [AllowAnonymous]
        public ActionResult Facebook()
        {
          //  Setting _Setting = repositorySetting.GetSetting;
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {


                client_id = ConfigurationManager.AppSettings["FBAppID"],
                client_secret = ConfigurationManager.AppSettings["FBAppSecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"

            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            Setting _Setting = repositorySetting.GetSetting;
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FBAppID"],
                client_secret = ConfigurationManager.AppSettings["FBAppSecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code

            });
            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            dynamic FbData = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            User obj = GetUserSession();
            bool EmailUniqe = repositoryUser.UniqueEmail(FbData.email);

           if (EmailUniqe == true)
            {//Exsisit User
             /// login
                //need user details
                obj = repositoryUser.UserIEmum.Where(a => a.Email.Equals(FbData.email)).FirstOrDefault();
                ///Last Login///
                ///
                obj.Last_Login = DateTime.Now;
                repositoryUser.Save(obj);
                ///


            }
            else { //New User  register data
                   //  obj.UserId = FbData.id;
                
              //  string testid = FbData.id;
              //  int idddd = Convert.ToInt32(testid);
            obj.FName = FbData.first_name;
            obj.LName = FbData.last_name;
            obj.Email = FbData.email;


           string EncryptedPW = repositoryDEncryption.Encrypt("123456");//Defulte for test 
          
           obj.Password = EncryptedPW;
            obj.Create_time = DateTime.Now;
            obj.Update_Time = DateTime.Now;
            obj.Last_Login = DateTime.Now;
            obj.RoleId = 2;//TO be normal user Role
            repositoryUser.Save(obj);

             

            }
           // To get current user data
          User _User = repositoryUser.UserIEmum.Where(a => a.Email.Equals(obj.Email)).FirstOrDefault();
            

            //   string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(data.Password, "SHA1");
            if (_User != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string data = js.Serialize(_User);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, _User.Email, DateTime.Now, DateTime.Now.AddMinutes(30), false, data);
                string encToken = FormsAuthentication.Encrypt(ticket);
                HttpCookie authoCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                Response.Cookies.Add(authoCookies);
               
              
            }
            ////




                   return Redirect( Url.Action("Index", "Home"));//User HomePage


          

        }
        [AllowAnonymous]
        public  bool ValidateCaptcha(string response)
        {
          //  Setting _Setting = repositorySetting.GetSetting;

            //secret that was generated in key value pair  
           string secret = ConfigurationManager.AppSettings["GoogleSecretkey"]; //WebConfigurationManager.AppSettings["recaptchaPrivateKey"];

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            return Convert.ToBoolean(captchaResponse.Success);

        }



    }


}
