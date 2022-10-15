using System.Collections.Generic;
using System.Linq;
using EShop.Data.Repositories;
using EShop.Models;

namespace EShop.Data.Repositories
{
    public class Categories : ICategories
    {
        EShopContext _context;
        public Categories(EShopContext context)
        {
            _context = context;
        }
        

        public IEnumerable<CategoryViewModel> CategoryVMforShow()
        {
            return _context.Categories.Select(n => new CategoryViewModel
            {
                Name = n.Name,
                Description = n.Description,
                Id = n.Id,
                Count = n.Items.Count
            }).ToList();
        }

        public IEnumerable<Category> AllCategories()
        {
            return _context.Categories;
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(n=>n.Id == id).SingleOrDefault();
        }
    }
}
