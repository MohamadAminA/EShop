using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Models
{
    [Bind("Password", "Name", "Phone", "Email")]
    public class AccountViewModel
    {

        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [Key]
        [MinLength(4, ErrorMessage = "{0} باید حداقل دارای ۴ کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Remote("VerifyEmail", "Account")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [MinLength(10, ErrorMessage = "{0} باید حداقل دارای ۱۰ کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Remote("VerifyPhone", "Account")]
        [MinLength(8, ErrorMessage = "{0} باید حداقل دارای ۸ کاراکتر باشد")]
        [MaxLength(15, ErrorMessage = "{0} باید حداکثر دارای ۱۵ کاراکتر باشد")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره موبایل")]
        public string Phone { get; set; }


        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [MinLength(8, ErrorMessage = "{0} باید حداقل دارای ۸ کاراکتر باشد")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبور")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string Password { get; set; }

        [MaxLength(100, ErrorMessage = "{0} باید حداکثر دارای ۱۰۰ کاراکتر باشد")]
        [MinLength(8, ErrorMessage = "{0} باید حداقل دارای ۸ کاراکتر باشد")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        [Display(Name = "تکرار رمز عبور")]
        public string RePassword { get; set; }
    }
}
