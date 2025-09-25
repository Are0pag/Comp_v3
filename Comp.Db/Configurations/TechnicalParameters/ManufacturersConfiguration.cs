using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class ManufacturersConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder) {
        builder.ToTable("Manufacturers").HasKey(m => m.Id);
        
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.Designation).IsRequired(true);
        builder.Property(m => m.FullName).IsRequired(false);
        builder.Property(m => m.Url).IsRequired(false);
        builder.Property(m => m.Remark).IsRequired(false);
    }
}