namespace PointOfSale.Domain
{
    public abstract class AggregateRoot
    {
        protected readonly List<IStoredEvent> EventsChanges = new List<IStoredEvent>();
        public IReadOnlyCollection<IStoredEvent> Changes => EventsChanges.AsReadOnly();

        protected void Apply(IStoredEvent e)
        {
            Mutate(e);
            EventsChanges.Add(e);
        }

        protected virtual void Mutate(IStoredEvent e)
        {
        }
    }
}
