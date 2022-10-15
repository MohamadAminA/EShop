using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Models
{
    public class ProfileViewModel
    {

        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [Key]
        [MinLength(4, ErrorMessage = "{0} باید حداقل دارای ۴ کاراکتر باشد")]
        [Display( Name = "نام")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [MinLength(10, ErrorMessage = "{0} باید حداقل دارای ۱۰ کاراکتر باشد")]
        [EmailAddress( ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = "{0} باید حداقل دارای ۸ کاراکتر باشد")]
        [MaxLength(15, ErrorMessage = "{0} باید حداکثر دارای ۱۵ کاراکتر باشد")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "شماره موبایل")]
        public string Phone { get; set; }


        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [MinLength(8, ErrorMessage = "{0} باید حداقل دارای ۸ کاراکتر باشد")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string Password { get; set; }

        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [MinLength(8 , ErrorMessage = "{0} باید حداقل دارای ۸ کاراکتر باشد")]
        [DataType(DataType.Password,ErrorMessage = "لطفا به صورت پسورد وارد کنید")]
        [Compare("Password")]
        [Display(Name = "تکرار رمز عبور")]
        public string RePassword { get; set; }
        [Display(Name = "عکس")]
        public string Image { get; set; }

        [Display(Name = "عکس")]
        public IFormFile AddImage { get; set; }
    }
}
