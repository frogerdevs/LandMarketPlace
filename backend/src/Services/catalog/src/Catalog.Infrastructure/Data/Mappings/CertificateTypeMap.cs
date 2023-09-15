using Catalog.Domain.Entities.Certificate;

namespace Catalog.Infrastructure.Data.Mappings
{
    public class CertificateTypeMap : IEntityTypeConfiguration<CertificateType>
    {
        public void Configure(EntityTypeBuilder<CertificateType> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("CertificateType");

        }

    }
}
