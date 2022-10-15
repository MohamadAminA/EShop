using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [MaxLength(100)]
        [MinLength(4)]
        [Required]
        public string Name { get; set; }


        [MaxLength(100)]
        [MinLength(10)]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        [MaxLength(15)]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string Phone { get; set; }


        [MaxLength(100)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public List<Cart> Carts { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public bool IsAdmin { get; set; } = false;
        [Required]
        public DateTime RegisterDate { get; set; }
        public List<Item> Favorites { get; set; }
    }
}