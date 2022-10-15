using System.ComponentModel.DataAnnotations;

namespace EShop.Models.ViewModel
{
	public class EnterViewModel
	{
        [Key]
        [Required(ErrorMessage = "لطفا ایمیل یا شماره تلفن خود را وارد کنید")]
        [Display(Name = "لطفا ایمیل یا شماره تلفن خود را وارد نمایید")]
        public string PhoneOrEmail { get; set; }
	}
}
