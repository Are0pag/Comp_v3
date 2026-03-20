using Comp.ModelData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class PaymentOrderConfig : IEntityTypeConfiguration<PaymentOrder>
{
    public void Configure(EntityTypeBuilder<PaymentOrder> builder) {
        builder.ToTable("PaymentOrders")
               .HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .ValueGeneratedOnAdd();
        
        builder.HasOne(p => p.Order)
               .WithMany()
               .HasForeignKey(p => p.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(p => p.Date)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.Number)
               .IsRequired();
        
        builder.Property(p => p.PaymentAmount)
               .IsRequired();
        
        builder.Property(p => p.PaymentPurpose)
               .IsRequired(false);
    }
}