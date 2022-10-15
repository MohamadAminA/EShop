using System.Linq;
using System.Threading.Tasks;
using EShop.Data;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.Component
{
    public class ProductItemComponent : ViewComponent
    {
        private EShopContext _context;
        public ProductItemComponent(EShopContext context)
        {
               _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(Product productItem)
        {

            return await Task.FromResult((IViewComponentResult)View("/Views/Component/ProductItemComponent.cshtml", productItem));
        }
    }
}
