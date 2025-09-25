using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class TypeSizeConfiguration : IEntityTypeConfiguration<TypeSize>
{
    public void Configure(EntityTypeBuilder<TypeSize> builder) {
        builder.ToTable(nameof(TypeSize) + "s").HasKey(ts => ts.Id);
        
        builder.Property(ts => ts.Id).ValueGeneratedOnAdd();
        builder.Property(ts => ts.Designation);
        builder.Property(ts => ts.OutputsNumber);
        builder.Property(ts => ts.IsUsingSmd);
        builder.Property(ts => ts.Description);
    }
}