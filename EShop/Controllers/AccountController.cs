using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using EShop.Data.Repositories;
using EShop.Models;
using EShop.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace EShop.Controllers
{
    public class AccountController : Controller
    {
        IAccount _account;
        public AccountController(IAccount account)
        {
            _account = account;
            ViewBag.IsSucceeded = false;
        }

        #region Register

        [HttpGet]
        [Route("/Register")]
        public IActionResult RegisterAccount()
        {
                return View();
        }

        public IActionResult EnterRegister(AccountViewModel registerAccount)
        {
            return View("RegisterAccount",registerAccount);
        } 

        [HttpPost]
        [Route("/Register")]
        public IActionResult RegisterAccount(AccountViewModel registerAccount)
        {
           
            if (registerAccount == null)
                return View();
            if (ModelState.ErrorCount>1)
            {
                return View(registerAccount);
            }
            if (!string.IsNullOrWhiteSpace(registerAccount.Email) && _account.IsEmailExist(registerAccount.Email))
            {
                ModelState.AddModelError("Email", "این {0} قبلا ثبت شده است لطفا {0} دیگری وارد نمایید");
                return View(registerAccount);
            }
            if (_account.IsPhoneExist(registerAccount.Phone))
            {
                ModelState.AddModelError("Phone", "این {0} قبلا ثبت شده است لطفا {0} دیگری وارد نمایید");
                return View(registerAccount);
            }
            var User = new User
            {
                Name = registerAccount.Name,
                Password = registerAccount.Password,
                Phone = registerAccount.Phone,
                RegisterDate = System.DateTime.Now,
                IsAdmin = false
            };
            if (!string.IsNullOrWhiteSpace(registerAccount.Email))
            {
                User.Email = registerAccount.Email;
            }
            if (_account.AddUser(User) == false)
            {
                ModelState.AddModelError("Name", "عملیات ناموفق بود لطفا مجددا سعی نمایید");
                return View(registerAccount);
            }
            _account.SaveChanges();
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,User.Name),
                new Claim(ClaimTypes.NameIdentifier,User.Id.ToString()),
                new Claim("IsAdmin",User.IsAdmin.ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                IsPersistent = false
            };
            HttpContext.SignInAsync(principal, properties);
            return View("SuccessModalLogin", new SuccessViewModel() { IsCalledByLogin = false , Name = registerAccount.Name});
        }
        public IActionResult VerifyPhone(string Phone)
        {
            if (_account.IsPhoneExist(Phone))
                return Json("شماره موبایل وارد شده از قبل موجود است");
            bool IsAllDigits(string s) => s.All(char.IsDigit);
            if (!IsAllDigits(Phone))
            {
                return Json("تمام مقادیر وارد شده در شماره موبایل باید به صورت عدد باشد");
            }
            return Json(true);
        }
        public IActionResult VerifyEmail(string Email)
        {
            if (_account.IsEmailExist(Email))
                return Json("شماره موبایل وارد شده از قبل موجود است");
            if(!(Email.Contains("gmail.com")|| Email.Contains("yahoo.com")))
                return Json("ایمیل وارد شده باید با جیمیل یا یاهو باشد");
            return Json(true);
        }
        #endregion

        #region Login
        [Route("/Enter")]
        public IActionResult EnterAccount()
        {
            return View();
        }
        [Route("/Enter")]
        [HttpPost]
        public IActionResult EnterAccount(string PhoneOrEmail)
        {
            if (String.IsNullOrWhiteSpace(PhoneOrEmail))
            {
                return View((object)"ایمیل یا شماره موبایل وارد شده صحیح نمی باشد لطفا مجددا نلاش کنید");
            }
            if (_account.IsEmailExist(PhoneOrEmail) || _account.IsPhoneExist(PhoneOrEmail))
            {
                var Login = new LoginViewModel() { PhoneOrEmail = PhoneOrEmail,RememberMe = false};
                return View("LoginAccount", Login);
            }
            AccountViewModel accountRegister = new AccountViewModel();
            if (PhoneOrEmail.Contains('@'))
            {
                accountRegister.Email = PhoneOrEmail;
            }
            else
            {
                accountRegister.Phone = PhoneOrEmail;
            }
            return RedirectToAction("EnterRegister",accountRegister);
        }

        [Route("/Login")]
        [HttpGet]
        public IActionResult LoginAccount()
        {
            return View();
        }
        [Route("/Login")]
        [HttpPost]
        public IActionResult LoginAccount(LoginViewModel login)
       {
            if (!ModelState.IsValid)
                return View(login);
            if (!_account.IsExist(login))
            {
                ModelState.AddModelError("PhoneOrEmail", "این ایمیل یا تلفن و رمز عبور نامعتبر است لطفا مجددا تلاش نمایید");
                return View(login);
            }
            
            User CurrentUser = _account.FindUser(login.PhoneOrEmail);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,CurrentUser.Name),
                new Claim(ClaimTypes.NameIdentifier,CurrentUser.Id.ToString()),
                new Claim("IsAdmin",CurrentUser.IsAdmin.ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                IsPersistent = login.RememberMe
            };
            HttpContext.SignInAsync(principal, properties);
            return View("SuccessModalLogin", new SuccessViewModel() { IsCalledByLogin = false, Name = CurrentUser.Name });
        }
        public IActionResult LogoutAccount()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region User Profile
        [Route("/Profile/{Name?}")]
        public IActionResult Profile(string Name)
        {
            User CurrentUser = _account.GetUser(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            Name = CurrentUser.Name;
            var _user = new ProfileViewModel()
            {
                Name = CurrentUser.Name,
                Email = CurrentUser.Email,
                Phone = CurrentUser.Phone,
                Password = CurrentUser.Password,
                RePassword = CurrentUser.Password,
                Image = CurrentUser.Image
            };
            return View("Profile",_user);
        }
        [HttpPost]
        [Route("/Profile/{Name?}")]
        public IActionResult Profile(ProfileViewModel account , string Name)
        {
            User CurrentUser = _account.GetUser(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            Name = CurrentUser.Name;
            if (account.AddImage.Length > 0)
            {
                Guid imageId;

                imageId = Guid.NewGuid();
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", $"{imageId}{Path.GetExtension(account.AddImage.FileName)}");
                using (FileStream imageFile = new FileStream(imagePath, FileMode.Create))
                {
                    account.AddImage.CopyTo(imageFile);
                }
               
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", $"{account.AddImage}{Path.GetExtension(account.AddImage.FileName)}");
                System.IO.File.Delete(imagePath);
                CurrentUser.Image = $"{imageId}{Path.GetExtension(account.AddImage.FileName)}";
            }
            if (!string.IsNullOrWhiteSpace(account.Password))
            {
                CurrentUser.Password = account.Password;
            }
            CurrentUser.Email = account.Email;
            CurrentUser.Name = account.Name;
            CurrentUser.Phone = account.Phone;
            _account.SaveChanges();
            return RedirectToAction("Profile");
        }
        #endregion

    }
}
