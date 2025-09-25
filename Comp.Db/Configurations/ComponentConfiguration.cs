using Comp.ModelData.Comp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder) {
        builder.ToTable("Components").HasKey(c => c.Id);
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(c => c.ConditionalDesignation)
               .WithMany()
               .HasForeignKey(c => c.ConditionalDesignationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Manufacturer)
               .WithMany()
               .HasForeignKey(c => c.ManufacturerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.MeasurementUnit)
               .WithMany()
               .HasForeignKey(c => c.MeasurementUnitId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.TypeSize)
               .WithMany()
               .HasForeignKey(c => c.TypeSizeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}