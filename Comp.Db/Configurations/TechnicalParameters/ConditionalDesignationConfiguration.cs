using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class ConditionalDesignationConfiguration : IEntityTypeConfiguration<ConditionalDesignation>
{
    public void Configure(EntityTypeBuilder<ConditionalDesignation> builder) {
        builder.ToTable(nameof(ConditionalDesignation) + "s").HasKey(e => e.Id);
        
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(ConditionalDesignation.MAX_NAME_LENGTH);
                
        builder.Property(e => e.Designation)
               .IsRequired()
               .HasMaxLength(ConditionalDesignation.MAX_DESIGNATION_LENGTH);
    }
}