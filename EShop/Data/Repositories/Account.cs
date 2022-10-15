using System;
using System.Linq;
using EShop.Models;
using EShop.Models.ViewModel;

namespace EShop.Data.Repositories
{
    public class Account : IAccount
    {
        readonly EShopContext _context;
        public Account(EShopContext context)
        {
            
            _context = context;
        }
        public bool AddUser(User newUser)
        {
            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteUser(User user)
        {
            try
            {
                _context.Users.Remove(user);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        public bool DeleteUser(Guid userId)
        {
            try
            {
                _context.Entry(GetUser(userId)).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public bool EditUser(User user)
        {
            try
            {
                User mainUser = new()
                {
                    Id = user.Id
                };
                _context.Entry(mainUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                mainUser.Name = user.Name;
                mainUser.Password = user.Password;
                mainUser.Email = user.Email;
                mainUser.Phone = user.Phone;
                mainUser.Carts = user.Carts;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public User GetUser(Guid userId)
        {
            try
            {
                return _context.Users.Where(n=>n.Id == userId).SingleOrDefault();
            }
            catch (System.Exception)
            {
                throw new Exception($"User with ID : {userId} Not Found");
            }
        }

        public bool IsPhoneExist(string phone)
        {
            if(phone == null)
                throw new Exception("IAccount IsPhoneExist string phone is null");
            if (_context.Users.Any(n=>n.Phone == phone))
                return true;
            return false;
        }
        public bool IsEmailExist(string email)
        {
            if (email == null)
                throw new Exception("IAccount IsEmailExist string Email is null");
            if (_context.Users.Any(n => n.Email.ToLower() == email.ToLower()))
                return true;
            return false;
        }

        public bool SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsExist(LoginViewModel user)
        {
            if(_context.Users.Any(n=>(n.Password == user.Password) && (n.Email == user.PhoneOrEmail || n.Phone == user.PhoneOrEmail)))
            {
                return true;
            }
            return false;
        }

        public User FindUser(string EmailOrPhone)
        {
            try
            {
                return _context.Users.SingleOrDefault(n => n.Email == EmailOrPhone || n.Phone == EmailOrPhone);

            }
            catch (Exception)
            {
                throw new Exception($"Find User {EmailOrPhone} Failed");
            }
        }
    }
}
