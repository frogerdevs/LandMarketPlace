using Ordering.Domain.Entities.Base;

namespace Ordering.Application.Entities
{
    public class BaseEntity : IBaseEntity
    {
    }
    public class BaseEntity<Id> : BaseEntity, IBaseEntity<Id>
    {
        Id IBaseEntity<Id>.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
