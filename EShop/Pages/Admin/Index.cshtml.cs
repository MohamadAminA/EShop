using System.Collections.Generic;
using System.Linq;
using EShop.Data;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EShop.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private EShopContext _context;
        public IndexModel(EShopContext context)
        {
            _context = context;
        }
        public List<Product> Products { get; set; }
        public void OnGet()
        {
            Products = _context.Products.Include(n => n.Item).ThenInclude(n=>n.Categories).Select(n=>n).ToList();
        }
        public void OnPost()
        {
        }
    }
}
