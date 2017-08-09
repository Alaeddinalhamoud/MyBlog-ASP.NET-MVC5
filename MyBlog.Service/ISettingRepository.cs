using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public interface ISettingRepository
    {
        Setting GetSetting { get; }
        void Save(Setting setting);
        Setting Details(int? Id);

    }
}
