using System.Collections.Generic;

namespace EShop.Models
{
	public class DetailsViewModel
	{
		public DetailsViewModel()
		{
			Categories = new List<Category>();
		}
		public Product Product { get; set; }
		public List<Category> Categories { get; set; }
	}
}
