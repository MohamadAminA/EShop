using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }



        public long GetTotalPriceWithDiscount()
        {
            long price =long.Parse((( Item.Price * ((100-Item.Discount)/100)).ToString()));
            price = price * Quantity;
            return price;
        }
        public long GetPriceWithoutDiscount()
        {
            long price = Item.Price * Quantity;
            return price;
        }
    }
}