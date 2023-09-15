namespace Ordering.Domain.Entities.Base
{
    public abstract class BaseEntity : IBaseEntity
    {
    }
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
