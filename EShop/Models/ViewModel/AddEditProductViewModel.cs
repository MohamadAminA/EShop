using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EShop.Models.ViewModel
{
	public class AddEditProductViewModel
	{
		[Key]
		public int Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(3, ErrorMessage = "{0} باید حداقل دارای ۳ کاراکتر باشد")]
        [DataType(DataType.Text,ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText, ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تخفیف")]
        public float Discount { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "قیمت")]
        public long Price { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تعداد در انبار")]
        public int QuantityInStock { get; set; }

        [DataType(DataType.Upload,ErrorMessage = "{0} را به صورت صحیح وارد نمایید")]
        [Display(Name = "افزودن تصویر برای محصول")]
        public List<IFormFile> Image0 { get; set; }

        [Display(Name = "تصاویر محصول")]
        public List<string> Image1 { get; set; }
        public List<Category> Categories{ get; set; }
        public AddEditProductViewModel()
        {
            Image1 = new List<string>();
        }
    }
}