using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EShop.Data;
using EShop.Models;

namespace EShop.Pages.Admin
{
    public class UserList : PageModel
    {
        public EShopContext _context;

        public UserList(EShopContext context)
        {
            _context = context;
        }

        public IList<User> User { get; set; }

        public async Task OnGetAsync()
        {
            User = await _context.Users.ToListAsync();
        }
    }
}
