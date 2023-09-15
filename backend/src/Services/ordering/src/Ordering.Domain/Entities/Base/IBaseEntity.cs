namespace Ordering.Domain.Entities.Base
{
    public interface IBaseEntity
    {
    }
    public interface IBaseEntity<TId> : IBaseEntity
    {
        public TId Id { get; set; }
    }
}