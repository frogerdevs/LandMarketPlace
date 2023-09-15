namespace Catalog.Domain.Entities.Base
{
    public abstract class BaseAuditableEntity : IBaseEntity
    {

        public DateTime Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
    }
    public abstract class BaseAuditableEntity<TKey> : IBaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
