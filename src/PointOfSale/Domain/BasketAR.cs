using CSharpFunctionalExtensions;
using PointOfSale.Domain.Models;
using PointOfSale.Events;

namespace PointOfSale.Domain
{
    public class BasketAR : AggregateRoot
    {
        private Basket? basket;
        private int itemIdCounter;
        public string? BasketId => basket?.BasketId.ToString();
        public long Version { get; }

        public BasketAR(IReadOnlyCollection<IStoredEvent> events = null, long version = 0)
        {
            if (events == null || events.Count == 0) return;

            Load(events);
            Version = version;
        }

        public Result<Basket, Error> GetBasket()
        {
            if (basket == null)
            {
                return BasketErrors.BasketNotCreated;
            }

            return basket;
        }

        public Result<Basket, Error> Create(string storeId, string deviceId)
        {
            if (basket != null)
                return BasketErrors.BasketAlreadyCreated;

            Apply(BasketCreated(storeId, deviceId));

            return basket;
        }

        public Result<Basket, Error> AddItem(Product product)
        {
            if (basket == null)
                return BasketErrors.BasketNotCreated;

            Apply(ItemAdded(product));

            return basket;
        }

        private BasketCreated BasketCreated(string storeId, string deviceId)
        {
            return new BasketCreated
            {
                BasketId = Guid.NewGuid(),
                DeviceId = deviceId,
                StoreId = storeId,
                CreatedAt = DateTime.UtcNow
            };
        }

        private ItemAdded ItemAdded(Product product)
        {
            return new ItemAdded
            {
                BasketId = basket.BasketId,
                ItemId = itemIdCounter + 1,
                ExternalProductId = product.ExternalProductId,
                Category = product.Category,
                Code = product.Code,
                Price = product.Price,
                Name = product.Name,
                Discount = product.Discount,
                CreatedAt = DateTime.UtcNow
            };
        }

        private void When(BasketCreated @event)
        {
            itemIdCounter = 0;
            basket = new Basket
            {
                BasketId = @event.BasketId,
                DeviceId = @event.DeviceId,
                StoreId = @event.StoreId,
                Items = new List<BasketItem>()
            };
        }

        private void When(ItemAdded @event)
        {
            ++itemIdCounter;
            basket.Items.Add(
                new BasketItem
                {
                    ItemId = @event.ItemId,
                    Product = new Product
                    {
                        Category = @event.Category,
                        Code = @event.Code,
                        ExternalProductId = @event.ExternalProductId,
                        Name = @event.Name,
                        Price = @event.Price,
                        Discount = @event.Discount
                    },
                    Quantity = 1,
                });
        }

        private void Load(IEnumerable<IStoredEvent> events)
        {
            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }

        protected override void Mutate(IStoredEvent @event)
        {
            this.When((dynamic)@event);
        }
    }
}
