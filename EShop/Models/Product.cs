using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Product
    {
		
		[Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }

        //navigation
        //public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
