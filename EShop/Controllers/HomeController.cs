using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        EShopContext _context;

        public HomeController(ILogger<HomeController> logger,EShopContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Route("ContactUS")]
        public IActionResult ContactUs()
        {
            return View("ContactUs");
        }
        [Route("AboutUS")]
        public IActionResult AboutUs()
        {
            return View("AboutUs");
        }
        public IActionResult Index()
        {
            var Products = _context.Products.Include(n=>n.Item).Select(n=>n).ToList();
            return View("Index",Products);
        }
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
