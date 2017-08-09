
using MyBlog.Data;
using MyBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Repo
{
    public class EFCategoryRepository : ICategoryRepository
    {
        EFDbContext context = new EFDbContext();
      
        public IEnumerable<Category> CategoryIEnum
        {
            get
            {
                return context.Categories;
            }
        }

        public IQueryable<Category> CategoryIList
        {
            get
            {
                return context.Categories.AsQueryable();
            }
        }

        public IEnumerable<Category> LastCategory
        {
            get
            {
                return context.Categories.OrderByDescending(c => c.Create_time).Take(20);
            }
        }

        public  Category Delete(int? Id)
        {
            Category dbEntry = context.Categories.Find(Id);
            if (dbEntry != null)
            {
                context.Categories.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public Category Details(int? Id)
        {
            Category dbEntry = context.Categories.Find(Id);

            return dbEntry;
        }

        public void IncreaseFreqOne(int Id)
        {
           if(Id != 0)
            {
                Category model = new Category();
                model = context.Categories.Find(Id);
                model.Frequence = model.Frequence + 1;
                context.SaveChanges();
            }
        }


        public void DecreaseFreqOne(int Id)
        {
            Category model = new Category();
            if (Id != 0)
            {
                
                model = context.Categories.Find(Id);
                if(model.Frequence !=0)
                { 
                model.Frequence = model.Frequence - 1;
                
                context.SaveChanges();
                }
            }
        }

        public void  Save(Category category)
        {


            if (category.CategoryId == 0)
            {

                context.Categories.Add(category);

                context.SaveChanges();

             

            }
            else
            {
                Category dbEntry = context.Categories.Find(category.CategoryId);
                if (dbEntry != null)
                {
                    dbEntry.CategoryId = category.CategoryId;
                    dbEntry.CategoryName = category.CategoryName;
                    dbEntry.Create_time = category.Create_time;
                   // dbEntry.Frequence = category.Frequence;
                 //   context.Categories.Add(dbEntry);

                    context.SaveChanges();
                    category.CategoryId = dbEntry.CategoryId;
                }
            }


     
        }
    }
}
