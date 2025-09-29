using Comp.ModelData.SortingItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder) {
        builder.ToTable("Categories").HasKey(c => c.Id);
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.Property(c => c.Name)
               .IsRequired();
        
        // Самореференсная связь для иерархии категорий
        builder.HasOne(c => c.ParentCategory)
               .WithMany(c => c.Subcategories)
               .HasForeignKey(c => c.ParentCategoryId)
               .OnDelete(DeleteBehavior.Restrict); // Или DeleteBehavior.Cascade в зависимости от требований
        
        // Индекс для родительской категории для улучшения производительности
        builder.HasIndex(c => c.ParentCategoryId);
    }
}