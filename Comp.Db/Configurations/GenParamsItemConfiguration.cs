/*using Comp.ModelData.Comp.GenericParams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class GenParamsItemConfiguration : IEntityTypeConfiguration<GenParamsItem>
{
    public void Configure(EntityTypeBuilder<GenParamsItem> builder)
    {
        builder.ToTable("GenParamsItems");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
               
        builder.Property(e => e.GenKey)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(e => e.GenValue)
               .HasMaxLength(500);
        
        // Индекс для быстрого поиска
        builder.HasIndex(e => new { e.GenParamsSetId, e.GenKey })
               .IsUnique();
    }
}*/