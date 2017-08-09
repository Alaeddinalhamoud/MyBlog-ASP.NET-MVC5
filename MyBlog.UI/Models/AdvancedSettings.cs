using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.UI.Models
{
    public class AdvancedSettings
    {
        [DisplayName("Last Category")]
        public bool DisplayLastCategory { get; set; }
        [DisplayName("Last Post")]
        public bool DisplayLastPost { get; set; }
        [DisplayName("Facebook")]
        public bool DisplayFbWidget { get; set; }
        [DisplayName("Twitter")]
        public bool DisplayTwWidget { get; set; }
        [DisplayName("Google Adv")]
        public bool DisplayGoogleAdv { get; set; }
        [DisplayName("disqus Widget")]
        public bool DisplaydisqusWidget { get; set; }
        [DisplayName("Local Comment Widget")]
        public bool DisplayLocalCommentWidget { get; set; }
        [DisplayName("FB Login")]
        public bool DisplayFBLogin { get; set; }
        [DisplayName("Register")]
        public bool DisplayRegister { get; set; }
        [DisplayName("FB App ID")]

        public string FBAppID { get; set; }
        [DisplayName("FB App Secret")]
        [DataType(DataType.Password)]
        public string FBAppSecret { get; set; }
        [DisplayName("Google Site key")]
        public string GoogleSitekey { get; set; }
        [DisplayName("Google Secret key")]
        [DataType(DataType.Password)]
        public string GoogleSecretkey { get; set; }
    }
}