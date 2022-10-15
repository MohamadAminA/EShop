using System.IO;
using System;
using System.Linq;
using EShop.Data;
using EShop.Models;
using EShop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using EShop.Data.Repositories;
using System.Collections.Generic;

namespace EShop.Pages.Admin
{
    public class EditProductModel : PageModel
    {
        EShopContext _context;
        [BindProperty]
        public AddEditProductViewModel Product { get; set; }
        [BindProperty]
        public List<int> CategoriesId { get; set; }

        public EditProductModel(EShopContext context)
        {
            _context = context;
            Product = new AddEditProductViewModel();
        }
        public void OnGet(int ProductId)
        {
            var currentProduct = _context.Products.Where(n => n.Id == ProductId);
            if (currentProduct == null)
            {
                RedirectToPage("./Index");
                return;
            }
            var CurrentProduct = currentProduct.Include(n => n.Item).ThenInclude(n=>n.Categories).Single();
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
                    Product.Image1.Add(itemimagename) ;
            }
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var _item = _context.Items.Include(n => n.Categories).Include(n=>n.Product).Where(n=>n.ProductId == Product.Id).Single();
            _item.Discount = Product.Discount;
            _item.Price = Product.Price;
            _item.QuantityInStock = Product.QuantityInStock;

            Product _product = _context.Products.Find(Product.Id);
            _product.Name = Product.Name;
            _product.Description = Product.Description;
            _product.Item = _item;

            _item.Product = _product;
            if (Product.Image0 != null)
            {
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
            }
            foreach (var item in _context.Categories.Include(n=>n.Items))
            {
                if(item.Items.Any(n=>n.Id == _item.Id))
                {
                    item.Items.Remove(_item);
                }
            }
            foreach (var categoryId in CategoriesId)
            {
                _item.Categories.Add(_context.Categories.Find(categoryId));
                _context.Categories.Find(categoryId).Items.Add(_item);
            }
            


            _context.SaveChanges();
            _item.ProductId = _product.Id;
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }
        public IActionResult OnGetDeleteImage(int ProductId,string ImageName)
        {
            string currentPath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "Resources", ImageName);

            if (string.IsNullOrWhiteSpace(ImageName) ||
            !System.IO.File.Exists(currentPath))
            {
                return RedirectToPage("EditProduct", ProductId);

            }
            System.IO.File.Delete(currentPath);
            var currentProduct = _context.Products.Find(ProductId);
            string DeletedImage = "";
            foreach (var itemProduct in currentProduct.Images.Split(','))
            {
                if (itemProduct != ImageName)
                {
                    DeletedImage = DeletedImage + itemProduct;
                    if(!string.IsNullOrWhiteSpace(DeletedImage))
                        if (DeletedImage.Last() != ',')
                            DeletedImage = DeletedImage + ',';
                }
            }
            if (!string.IsNullOrWhiteSpace(DeletedImage))
                if (DeletedImage.First() == ',')
                DeletedImage = DeletedImage.Skip(1).Select(n=>n).ToString();
            currentProduct.Images = DeletedImage;
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}