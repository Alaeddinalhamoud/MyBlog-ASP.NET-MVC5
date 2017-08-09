
using MyBlog.Data;
using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Repo
{
 public   class EFSettingRepository: ISettingRepository
    {
        EFDbContext context = new EFDbContext();

        public Setting GetSetting
        {
            
            get
            {
                return  context.Settings.FirstOrDefault<Setting>();
            }
        }

        public Setting Details(int? Id)
        {
            Setting dbEntry = context.Settings.Find(Id);
            return dbEntry;
        }

        public void Save(Setting setting)
        {

            if (setting.Id == 0)
            {

                context.Settings.Add(setting);

                context.SaveChanges();



            }
            else
            {
                Setting dbEntry = context.Settings.Find(setting.Id);
                if (dbEntry != null)
                {
                    dbEntry.Id = setting.Id;
                    dbEntry.HomeImage = setting.HomeImage;
                    dbEntry.HomeImageText = setting.HomeImageText;
                    dbEntry.NumberOfLastPost = setting.NumberOfLastPost;
                    dbEntry.NumberOfCategory = setting.NumberOfCategory;
                    dbEntry.PostNumberInPage = setting.PostNumberInPage;
                    dbEntry.NumberOfTopPost = setting.NumberOfTopPost;
                    dbEntry.Update_Time = setting.Update_Time;
                    dbEntry.UserId = setting.UserId;
                    //dbEntry.DisplayLastCategory = setting.DisplayLastCategory;
                    //dbEntry.DisplayLastPost = setting.DisplayLastPost;
                    //dbEntry.DisplayFbWidget = setting.DisplayFbWidget;
                    //dbEntry.DisplayTwWidget = setting.DisplayTwWidget;
                    //dbEntry.DisplayGoogleWidget = setting.DisplayGoogleWidget;
                    //dbEntry.FBAppID = setting.FBAppID;
                    //dbEntry.FBAppSecret = setting.FBAppSecret;
                    //dbEntry.GoogleSitekey = setting.GoogleSitekey;
                    //dbEntry.GoogleSecretkey = setting.GoogleSecretkey;


                    context.SaveChanges();
                   
                }
            }


        }
    }
}
