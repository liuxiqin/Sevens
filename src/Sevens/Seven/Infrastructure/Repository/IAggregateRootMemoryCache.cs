using Seven.Aggregates;

namespace Seven.Infrastructure.Repository
{
    public interface IAggregateRootMemoryCache
    {
        void Add(IAggregateRoot aggregateRoot);

        IAggregateRoot Get(string aggregateRootId);
    }
}