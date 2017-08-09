
using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public interface IWidgetRepository
    {
         IEnumerable<Widget> WidgetIEnum { get; }
        List<Widget> WidgetList { get; }
        Widget GetSetting { get; }
        void Save(Widget widget);
        Widget Details(int? Id);
    }
}
