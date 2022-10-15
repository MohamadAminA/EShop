namespace EShop.Models
{
    public static class Extentions
    {
        public static string ToPrice(this long price)
        {
            return price.ToString("#,0");
        }
        public static string ToPrice(this int price)
        {
            return price.ToString("#,0");
        }
        public static string ToPrice(this string price)
        {
            return long.Parse(price).ToString("#,0");
        }
    }
}
