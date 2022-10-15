using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EShop.Data;
using EShop.Models;
using EShop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;

namespace EShop.Pages.Admin
{
    public class AddProductModel : PageModel
    {
        EShopContext _context;
        [BindProperty]
        public AddEditProductViewModel Product { get; set; }
        [BindProperty]
        public List<int> CategoriesId { get; set; }
        public AddProductModel(EShopContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Item _item = new Item()
            {
                Discount = Product.Discount,
                Price = Product.Price,
                QuantityInStock = Product.QuantityInStock,
                Categories = new List<Category>(),
            };

            Product _product = new Product()
            {
                Name = Product.Name,
                Description = Product.Description,
                Item = _item,
            };
            _item.Product = _product;

            foreach (var Image in Product.Image0)
            {
                if (Image.Length > 0)
                {
                    Guid imageId;

                    imageId = Guid.NewGuid();
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", $"{imageId}{Path.GetExtension(Image.FileName)}");
                    using (FileStream imageFile = new FileStream(imagePath, FileMode.Create))
                    {
                        Image.CopyTo(imageFile);
                    }
                    _product.Images = string.Concat(_product.Images, $"{imageId}{Path.GetExtension(Image.FileName)},");

                }
            }
            foreach (var categoryId in CategoriesId)
            {
                _item.Categories.Add(_context.Categories.Find(categoryId));
            }

            _context.Products.Add(_product);
            _context.Items.Add(_item);
            _context.SaveChanges();
            _item.ProductId = _product.Id;
            _item.Id = _product.Id;
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
