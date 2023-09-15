namespace IdentityServer.Data.Entites.Base
{
    public interface IBaseEntity
    {
    }
    public interface IBaseEntity<TId> : IBaseEntity
    {
        public TId Id { get; set; }
    }
}
