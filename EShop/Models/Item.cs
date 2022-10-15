using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public long Price { get; set; }
        [DataType(DataType.PhoneNumber)]
        public float Discount { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int QuantityInStock { get; set; }
        //Navigation
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public List<Category> Categories { get; set; }

        public List<User> FavoritesUser { get; set; }

        public long GetPriceWithDiscount()
        {
            long price = Price;
            price -= (long)(price * (Discount / 100));
            return price;
        }
    }
}
