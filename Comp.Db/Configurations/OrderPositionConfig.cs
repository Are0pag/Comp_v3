using Comp.ModelData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class OrderPositionConfig : IEntityTypeConfiguration<OrderPosition>
{
    public void Configure(EntityTypeBuilder<OrderPosition> builder) {
        builder.ToTable("OrderPositions")
               .HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .ValueGeneratedOnAdd();

        builder.HasOne(p => p.Position)
               .WithMany()
               .HasForeignKey(p => p.PositionId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.SupplierOrder)
               .WithMany()
               .HasForeignKey(p => p.SupplierOrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(p => p.OrderQuantity)
               .HasDefaultValue(0);
                
        builder.Property(p => p.ReceivedQuantity)
               .HasDefaultValue(0);
                
        builder.Property(p => p.UnitPrice)
               .HasDefaultValue(0);
                
        builder.Property(p => p.TotalCost)
               .HasDefaultValue(0);

        builder.Property(p => p.ReceiveStatus)
               .HasDefaultValue(ReceiveStatus.NotReceived.ToString());
    }
}