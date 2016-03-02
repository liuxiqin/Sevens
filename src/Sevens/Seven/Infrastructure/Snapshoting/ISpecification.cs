namespace Seven.Infrastructure.Snapshoting
{
    public interface ISpecification<TEntity>
    {
        bool IsSatisfiedBy(TEntity entity);
    }
}