using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}