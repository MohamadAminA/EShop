using System.Collections.Generic;
using EShop.Models;

namespace EShop.Data.Repositories
{
    public interface ICategories
    {
        public IEnumerable<Category> AllCategories();
        public IEnumerable<CategoryViewModel> CategoryVMforShow();
        public Category GetCategory(int id);
    }
}
