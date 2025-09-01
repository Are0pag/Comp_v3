using Comp.ModelData.Comp.GenericParams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class GenParamsSetConfiguration : IEntityTypeConfiguration<GenParamsSet>
{
    public void Configure(EntityTypeBuilder<GenParamsSet> builder)
    {
        builder.ToTable("GenParamsSets");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
               
        builder.Property(e => e.Alias)
               .IsRequired()
               .HasMaxLength(100);
        
        // Связь один-ко-многим
        builder.HasMany(e => e.GenParamsItems)
               .WithOne(p => p.GenParamsSet)
               .HasForeignKey(p => p.GenParamsSetId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}