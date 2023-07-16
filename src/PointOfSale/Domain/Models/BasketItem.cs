namespace PointOfSale.Domain.Models
{
    public class BasketItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
