using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class ManufacturersConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder) {
        builder.ToTable(nameof(Manufacturer) + "s").HasKey(m => m.Id);
        
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.Designation);
        builder.Property(m => m.FullName);
        builder.Property(m => m.Url);
        builder.Property(m => m.Remark);
    }
}