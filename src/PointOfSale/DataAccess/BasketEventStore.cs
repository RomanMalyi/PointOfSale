using System.Text.Json;
using EventStore.Client;
using PointOfSale.Domain;
using PointOfSale.Events;

namespace PointOfSale.DataAccess
{
    public class BasketEventStore
    {
        private readonly EventStoreClient client;

        public BasketEventStore(EventStoreClient client)
        {
            this.client = client;
        }

        public async Task<IWriteResult> AppendToStream(object @event, string streamId)
        {
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(@event);
            var eventData = new EventData(Uuid.NewUuid(),
                @event.GetType().ToString(),
                utf8Bytes.AsMemory());

            IWriteResult writeResult = await this.client
                .AppendToStreamAsync(streamId,
                    StreamState.Any,
                    new[] { eventData });

            return writeResult;
        }

        public async Task<List<IStoredEvent?>> ReadStream(string streamId)
        {
            List<IStoredEvent?> result = new List<IStoredEvent?>();
            var streamResult = this.client.ReadStreamAsync(Direction.Forwards,
                streamId,
                StreamPosition.Start);
            await foreach (var item in streamResult)
            {
                var types = typeof(BasketCreated).Assembly.GetTypes();
                Type existingType = types.FirstOrDefault(t => t.FullName.Equals(item.Event.EventType, StringComparison.OrdinalIgnoreCase));
                result.Add(JsonSerializer.Deserialize(item.Event.Data.Span, existingType)
                as IStoredEvent);
            }

            return result;
        }
    }
}
