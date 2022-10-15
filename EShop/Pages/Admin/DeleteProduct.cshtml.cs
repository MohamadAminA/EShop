using EShop.Data;
using EShop.Models.ViewModel;
using EShop.Models;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EShop.Pages.Admin
{
    public class DeleteProductModel : PageModel
    {
        EShopContext _context;
        [BindProperty]
        public AddEditProductViewModel Product { get; set; }

        public DeleteProductModel(EShopContext context)
        {
            _context = context;
            Product = new AddEditProductViewModel();
        }
        public void OnGet(int ProductId)
        {
            var CurrentProduct = _context.Products.Where(n => n.Id == ProductId).Include(n => n.Item).Single();
            Product.Name = CurrentProduct.Name;
            Product.Id = CurrentProduct.Id;
            Product.Description = CurrentProduct.Description;
            Product.Price = CurrentProduct.Item.Price;
            Product.Discount = CurrentProduct.Item.Discount;
            Product.QuantityInStock = CurrentProduct.Item.QuantityInStock;
            Product.Categories = CurrentProduct.Item.Categories;
            string[] ImageNames = CurrentProduct.Images.Split(',');
            foreach (var itemimagename in ImageNames)
            {
                if (!string.IsNullOrWhiteSpace(itemimagename))
                    Product.Image1.Add(itemimagename);
            }
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            Item _item = _context.Items.Find(Product.Id);
            Product _product = _context.Products.Find(Product.Id);
            foreach (var itemImage in _product.Images.Split(','))
            {
                DeleteImage(_product.Id,itemImage);
            }
            _context.Products.Remove(_product);
            _context.Items.Remove(_item);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }
        public void DeleteImage(int ProductId, string ImageName)
        {
            string currentPath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "Resources", ImageName);

            if (string.IsNullOrWhiteSpace(ImageName) ||
            !System.IO.File.Exists(currentPath))
            {
                return ;

            }
            System.IO.File.Delete(currentPath);
            var currentProduct = _context.Products.Find(ProductId);
            string DeletedImage = "";
            foreach (var itemProduct in currentProduct.Images.Split(','))
            {
                if (itemProduct != ImageName)
                    DeletedImage = DeletedImage + itemProduct + ',';
            }
            currentProduct.Images = DeletedImage;
        }
    }
}
