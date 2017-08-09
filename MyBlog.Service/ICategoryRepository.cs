
using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> LastCategory { get; }
        void Save(Category category);
        void IncreaseFreqOne(int Id);
        void DecreaseFreqOne(int Id);
        IEnumerable<Category> CategoryIEnum { get; }
        IQueryable<Category> CategoryIList { get; }
        Category Details(int? Id);
        Category Delete(int? Id);

    }
}
