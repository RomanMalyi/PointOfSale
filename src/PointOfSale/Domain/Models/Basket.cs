namespace PointOfSale.Domain.Models
{
    public class Basket
    {
        public Guid BasketId { get; set; }
        public string StoreId { get; set; }
        public string DeviceId { get; set; }
        public decimal TotalPrice => RoundToLowest(Items.Sum(i => i.Product.Price * i.Quantity) - Discount);

        public decimal Discount => RoundToLowest(Items.Sum(i => i.Product.Price * i.Quantity * i.Product.Discount));

        public IList<BasketItem> Items { get; set; }

        private static decimal RoundToLowest(decimal number)
        {
            return decimal.Truncate(number * 100) / 100m;
        }
    }
}
