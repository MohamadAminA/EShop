using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
namespace EShop.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems=new List<CartItem>();
        }
        [Key]
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public int CartItemId { get; set; }
        [ForeignKey("CartItemId")]
        public List<CartItem> CartItems { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreateTime { get; set; }
        public bool IsPaid { get; set; }

        public long GetTotalPriceWithDiscount()
        {
            long totalPrice = 0;
            foreach (var item in CartItems)
            {
                totalPrice += item.GetTotalPriceWithDiscount();
            }
            return totalPrice;
        }

    }
}
