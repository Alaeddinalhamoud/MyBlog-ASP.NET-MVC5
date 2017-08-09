
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyBlog.Service;
using MyBlog.Data;

namespace MyBlog.Repo
{
   public class EFWidgetRepository: IWidgetRepository
    {
        EFDbContext context = new EFDbContext();

        public Widget GetSetting
        {
            get
            {
                return context.Widgets.FirstOrDefault<Widget>();
            }
        }

        public IEnumerable<Widget> WidgetIEnum
        {
            get
            {
                return context.Widgets;
            }
        }

        public List<Widget> WidgetList
        {
            get
            {
                return context.Widgets.ToList();
            }
        }

        public Widget Details(int? Id)
        {
            Widget dbEntry = context.Widgets.Find(Id);
            return dbEntry;
        }

        public void Save(Widget widget)
        {
            if (widget.WidgetId == 0)
            {


                context.Widgets.Add(widget);

                context.SaveChanges();



            }
            else
            {
                Widget dbEntry = context.Widgets.Find(widget.WidgetId);
                if (dbEntry != null)
                {
                    dbEntry.WidgetId = widget.WidgetId;
                    dbEntry.WidgetName = widget.WidgetName;
                    dbEntry.WidgetContent = widget.WidgetContent;
                    dbEntry.Update_Time = widget.Update_Time;
                    dbEntry.UserId = widget.UserId;


                    context.SaveChanges();

                }
            }

        }
    }
}
