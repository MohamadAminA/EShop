using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Models.ViewModel
{
    public class LoginViewModel
    {
        [Key]
        [Remote("VerifyPhone","Account")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "ایمیل یا شماره تلفن")]
        public string PhoneOrEmail { get; set; }


        [DataType(DataType.Password, ErrorMessage = "لطفا به صورت پسورد وارد کنید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
       

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
