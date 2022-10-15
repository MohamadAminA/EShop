using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Data;
using EShop.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;
using ZarinpalSandbox;
using EShop.Data.Repositories;

namespace EShop.Controllers
{

    [Authorize]
    public class ProductController : Controller
    {
        private readonly EShopContext _context;
        private readonly IAccount _account;

        public ProductController(EShopContext context,IAccount account)
        {
            _context = context;
            _account = account;
        }

        #region Cart
        public IActionResult RemoveFromCart(int? itemId)
        {
            User currentUser = _context.Users.Where(n => n.Id == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).Include(n => n.Carts).ThenInclude(n => n.CartItems).ThenInclude(n=>n.Item).SingleOrDefault();
            if (currentUser == null)
                return null;
            var UserCart = currentUser.Carts.FirstOrDefault(n => n.IsPaid == false);
            if (UserCart == null)
                return null;
            var UserCartItem = UserCart.CartItems.Find(n => n.Item.Id == itemId);
            if (UserCartItem == null)
                return null;
            if (UserCartItem.Quantity == 1)
                _context.CartItems.Remove(UserCartItem);
            else
                UserCartItem.Quantity -= 1;
            _context.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        public IActionResult AddToCart(int itemId)
        {
            var currentItem = _context.Items.Where(m => m.Id == itemId).Include(n => n.Product).Include(n => n.Categories).ThenInclude(n=>n.Items).SingleOrDefault();
            if (currentItem == null)
                return null;
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _context.Users.Where(n => n.Id == Guid.Parse(userId))
                .Include(n => n.Carts).ThenInclude(n=>n.CartItems).ThenInclude(n=>n.Item).Single();
            var UserCart = currentUser.Carts.Find(m => m.IsPaid == false);
            if (UserCart == null)
            {
                Cart NewCart = new Cart();
                currentUser.Carts.Add(NewCart);
                UserCart = NewCart;
            }
            var CartItem = UserCart.CartItems.Find(n => n.Item.Id == itemId);
            if (CartItem == null)
            {
                var NewCartItem = new CartItem() { Cart = UserCart, Item = currentItem, Quantity = 1 };
                UserCart.CartItems.Add(NewCartItem);
                CartItem = NewCartItem;
            }
            else
            {
                CartItem.Quantity += 1;
            }
            _context.SaveChanges();
            return RedirectToAction("ShowCart");
        }
        [Route("/Cart/{Name?}")]
        public IActionResult ShowCart(string Name)
        {
            var currentUser = _context.Users.Where(n => n.Id == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).Include(n=>n.Carts);
            if (currentUser.SingleOrDefault().Carts == null|| currentUser.SingleOrDefault().Carts.FirstOrDefault(n=>n.IsPaid==false)==null)
            {
                return View();
            }
            var UserCart = currentUser.Include(n => n.Carts).ThenInclude(n => n.CartItems).ThenInclude(n=>n.Item).ThenInclude(n => n.Categories).
                Include(n=>n.Carts).ThenInclude(n=>n.CartItems).ThenInclude(n=>n.Item).ThenInclude(n=>n.Product).
                SingleOrDefault().Carts.First(n => n.IsPaid == false);
            if (UserCart == null)
                return View();
            Name = currentUser.Single().Name;
            if (UserCart == null || UserCart.CartItems == null || UserCart.CartItems.Count == 0)
                return View("ShowCart", null);

            var CartVM = new CartViewModel()
            {
                CartItems = UserCart.CartItems,
                OrderTotaL = UserCart.GetTotalPriceWithDiscount()
            };

            return View("ShowCart", CartVM);
        }
        #endregion

        #region Product
        [AllowAnonymous]
        [Route("/Details/{Name?}")]
        public IActionResult Details(int id, string Name)
        {
            Product pro = _context.Products.Where(n => n.Id == id).Include(n => n.Item).ThenInclude(n => n.Categories).Include(n=>n.Item).ThenInclude(n=>n.FavoritesUser).Single();
            Name = pro.Name;
            List<Category> categories = pro.Item.Categories;
            Name = pro.Name;
            DetailsViewModel detail = new DetailsViewModel();
            detail.Product = pro;
            detail.Categories = categories;
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }


        [AllowAnonymous]
        [Route("/Category/{CategoryId}/{CategoryName}")]
        public IActionResult ShowCategoryProducts(int CategoryId, string CategoryName)
        {
            var category = _context.Categories.Where(n => n.Id == CategoryId && n.Name == CategoryName).Include(n => n.Items).ThenInclude(n => n.Product).SingleOrDefault();
            if (category == null)
                return null;
            return View(category);
        }

        #endregion

        #region Bookmark Product
        [Authorize]
        public IActionResult BookMark(int id, string Name)
        {
            Product pro = _context.Products.Where(n => n.Id == id).Include(n => n.Item).ThenInclude(n => n.Categories).Single();
            List<Category> categories = pro.Item.Categories;
            var user = _context.Users.Include(n => n.Favorites).Single(n => n.Id == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).Favorites;
            if (user.Exists(n => n.Id == id))
                user.Remove(pro.Item);
            else
                user.Add(pro.Item);
            _context.SaveChanges();
            Name = pro.Name;
            DetailsViewModel detail = new DetailsViewModel();
            detail.Product = pro;
            detail.Categories = categories;

            return View("Details", detail);
        }
        [Authorize]
        [Route("/Favorite")]
        public IActionResult Favorite()
        {
            var userFavorites = _context.Users.Include(n => n.Favorites).ThenInclude(n=>n.Product).Single(n => n.Id == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).Favorites;
            List<Product> products = new List<Product>();
            products = userFavorites.Select(n=>n.Product).ToList();
            return View(products);
        }
        #endregion

        #region Payment
        [Authorize]
        public IActionResult Payment()
        {
            var user = _account.GetUser(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var userCart = _context.Carts.Include(n => n.CartItems).ThenInclude(n=>n.Item).FirstOrDefault(n => n.User.Id == user.Id && n.IsPaid == false);
            if (userCart == null)
                return NotFound();
            int Money = int.Parse(userCart.GetTotalPriceWithDiscount().ToString());

            var payment = new Payment(Money);
            var result = payment.PaymentRequest($"پرداخت فاکتور شماره {userCart.Id}", $"http://Localhost:47010/ZarinPal/{userCart.Id}", user.Email, user.Phone);
            if(result.Result.Status == 100)
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/"+result.Result.Authority);
            else
            {
                return BadRequest();
            }
        }

        [Route("/ZarinPal/{CartId}")]
        [Authorize]
        public IActionResult ZarinPal(int CartId)
        {
            if (HttpContext.Request.Query["Status"]!=""&&
                HttpContext.Request.Query["Authority"] != "")
            {
                string Authority = HttpContext.Request.Query["Authority"].ToString();
                Cart userCart = _context.Carts.Where(n=>n.Id==CartId).Include(n=>n.CartItems).ThenInclude(n=>n.Item).FirstOrDefault();
                if(userCart == null)
                {
                    return NotFound();
                }
                var amount = int.Parse(userCart.GetTotalPriceWithDiscount().ToString());
                var payment = new Payment(amount);
                var result = payment.Verification(Authority);
                ViewBag.PaymentResult = result;
                ViewBag.Amount = amount;
                ViewBag.CartId = CartId;
                if(
                HttpContext.Request.Query["Status"] == "OK")
                {
                    if (result.Result.Status == 100)
                    {
                        userCart.IsPaid = true;
                        _context.Carts.Update(userCart);
                        _context.SaveChanges();
                        ViewBag.PaymentResult = result;
                    }
                }
                
            return View();

            }
            return NotFound();
        }

        #endregion

    }
}