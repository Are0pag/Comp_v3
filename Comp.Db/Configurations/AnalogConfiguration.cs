using Comp.ModelData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class AnalogConfiguration : IEntityTypeConfiguration<Analog>
{
    public void Configure(EntityTypeBuilder<Analog> builder) {
        builder.ToTable("Analogs")
               .HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();

        builder.Property(a => a.IsAllCount)
               .HasColumnType("BIT");

        builder.HasOne(a => a.SourceComponent)
               .WithMany()
               .HasForeignKey(a => a.SourceComponentId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(a => a.RelatedComponent)
               .WithMany()
               .HasForeignKey(a => a.RelatedComponentId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}