using Comp.ModelData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class SupplierOrderConfig : IEntityTypeConfiguration<SupplierOrder>
{
    public void Configure(EntityTypeBuilder<SupplierOrder> builder) {
        builder.ToTable("SupplierOrders")
               .HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .ValueGeneratedOnAdd();

        
        builder.Property(p => p.PurchaseOrderNumber)
               .IsRequired();
        
        builder.Property(p => p.InvoiceNumber)
               .IsRequired(false);
        
        builder.Property(p => p.Note)
               .IsRequired(false);
        

        builder.Property(p => p.OrderStatus)
               .IsRequired()
               .HasDefaultValue(OrderStatus.Created.ToString());
        
        builder.Property(p => p.VatStatus)
               .IsRequired()
               .HasDefaultValue(VatStatus.VatIncluded.ToString());
        
        
        builder.Property(p => p.ContractFilePath)
               .IsRequired(false);        
        
        builder.Property(p => p.InvoiceFilePath)
               .IsRequired(false);
        
        
        builder.Property(p => p.OrderDate)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.DeliveryDate)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");
        
        
        builder.HasOne(p => p.Counterparty)
               .WithMany()
               .HasForeignKey(p => p.CounterpartyId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}