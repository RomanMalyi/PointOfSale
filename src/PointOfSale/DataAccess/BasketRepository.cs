using PointOfSale.Domain;

namespace PointOfSale.DataAccess
{
    public class BasketRepository
    {
        private readonly BasketEventStore basketEventStore;

        public BasketRepository(BasketEventStore basketEventStore)
        {
            this.basketEventStore = basketEventStore;
        }

        public async Task<BasketAR?> Get(Guid basketId)
        {
            List<IStoredEvent?> result = await basketEventStore.ReadStream(basketId.ToString());
            if(result.Count == 0) return null;

            return new BasketAR(result);
        }

        public async Task Upsert(BasketAR basket)
        {
            //TODO: implement batch AppendToStream method 
            foreach (var change in basket.Changes)
            {
                await basketEventStore.AppendToStream(change, basket.BasketId);
            }
        }
    }
}
