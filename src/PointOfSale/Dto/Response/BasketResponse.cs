using PointOfSale.Domain.Models;

namespace PointOfSale.Dto.Response
{
    public class BasketResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public List<Product> Products { get; set; }

        public BasketResponse(Basket basket)
        {
            Id = basket.BasketId;
            Amount = basket.TotalPrice;
            Discount = basket.Discount;
            Products = basket.Items.Select(i => i.Product).ToList();
        }
    }
}
