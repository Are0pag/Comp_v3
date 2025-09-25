using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class MeasurementUnitConfiguration : IEntityTypeConfiguration<MeasurementUnit>
{
    public void Configure(EntityTypeBuilder<MeasurementUnit> builder) {
        builder.ToTable(nameof(MeasurementUnit) + "s").HasKey(mu => mu.Id);
        
        builder.Property(mu => mu.Id).ValueGeneratedOnAdd();
        builder.Property(mu => mu.Designation).IsRequired(true);
        builder.Property(mu => mu.Name).IsRequired(false);
    }
}