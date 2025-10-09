using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class GpConfiguration : IEntityTypeConfiguration<GenericParametersSet>
{
    public void Configure(EntityTypeBuilder<GenericParametersSet> builder) {
        builder.ToTable(nameof(GenericParametersSet) + "s").HasKey(x => x.Id);
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(c => c.Name).IsUnique();
        builder.Property(c => c.Name).IsRequired();
        
        builder.Property(c => c.GpMain);
        builder.Property(c => c.Gp1);
        builder.Property(c => c.Gp2);
        builder.Property(c => c.Gp3);
        builder.Property(c => c.Gp4);
        builder.Property(c => c.Gp5);
    }
}