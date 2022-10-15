using System;
using EShop.Models;
using EShop.Models.ViewModel;

namespace EShop.Data.Repositories
{
    public interface IAccount
    {
        bool AddUser(User newUser);
        bool DeleteUser(User user);
        bool EditUser(User user);
        bool DeleteUser(Guid userId);
        User GetUser(Guid userId);
        bool IsEmailExist(string email);
        bool IsPhoneExist(string phone);
        bool SaveChanges();
        bool IsExist(LoginViewModel user);
        User FindUser(string EmailOrPhone);
    }
}
