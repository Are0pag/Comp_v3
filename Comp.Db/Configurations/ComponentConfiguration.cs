using Comp.ModelData.Comp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder) {
        builder.ToTable("Components").HasKey(c => c.Id);
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(c => c.Name).IsUnique();
        builder.Property(c => c.Name).IsRequired();
        
        builder.HasIndex(c => c.NomenclatureNumber).IsUnique();
        builder.Property(c => c.NomenclatureNumber).IsRequired();
        
        builder.HasOne(c => c.Category)
               .WithMany() 
               .HasForeignKey(c => c.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
        
        /*builder.HasOne(c => c.ConditionalDesignation)
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
               .OnDelete(DeleteBehavior.Restrict);*/
    }
}