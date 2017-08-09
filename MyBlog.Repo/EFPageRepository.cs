
using MyBlog.Data;
using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Repo
{
   public class EFPageRepository:IPageRepository
    {

        EFDbContext context = new EFDbContext();

        public IEnumerable<Page> PageIEnum
        {
            get
            {
                return context.Pages;
            }
        }

        public List<Page> PageList
        {
            get
            {
                return context.Pages.ToList<Page>();
            }
        }

        public Page Details(int? Id)
        {
            Page dbEntry = context.Pages.Find(Id);
            return dbEntry;
        }

        public void Save(Page page)
        {
            if (page.PageId == 0)
            {
                Page _page = new Page();//To get Page ID From AddPage to use it for Details
                _page.Title = page.Title;
                _page.PagesContent = page.PagesContent;
                _page.Create_Time= page.Create_Time;
                _page.Update_Time = page.Update_Time;
                _page.UserId = page.UserId;
               
               
                context.Pages.Add(_page);
                context.SaveChanges();
                page.PageId = _page.PageId;
            }
            else
            {
                Page dbEntry = context.Pages.Find(page.PageId);
                if (dbEntry != null)
                {
                    dbEntry.PageId = page.PageId;
                    dbEntry.Title = page.Title;
                    dbEntry.PagesContent = page.PagesContent;
                    dbEntry.Create_Time = page.Create_Time;
                    dbEntry.Update_Time = page.Update_Time;
                    dbEntry.UserId = page.UserId;
                  
                    context.SaveChanges();

                }
            }
        }
    }
}
