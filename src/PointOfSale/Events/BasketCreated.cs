using PointOfSale.Domain;

namespace PointOfSale.Events
{
    public class BasketCreated: IStoredEvent
    {
        public Guid BasketId { get; set; }
        public string StoreId { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
