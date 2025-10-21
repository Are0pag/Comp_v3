using Comp.ModelData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comp.Db.Configurations;

public class CounterpartyConfig : IEntityTypeConfiguration<Counterparty>
{
    public void Configure(EntityTypeBuilder<Counterparty> builder) {
        builder.ToTable("Counterparties")
               .HasKey(x => x.Id);
        
        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(c => c.ShortName)
               .IsRequired();
        
        builder.Property(c => c.CounterpartyTypeName)
               .IsRequired();

        builder.Property(c => c.FullName)
               .IsRequired(false); // как обозначить что необязательно, но должно быть уникально?
        
        builder.Property(c => c.CityName)
               .IsRequired(false);
        
        builder.Property(c => c.Address)
               .IsRequired(false);
        
        builder.Property(c => c.Tin)
               .IsRequired(false);
        
        builder.Property(c => c.ReasonCode)
               .IsRequired(false);
        
        
        /* Account */
        builder.Property(c => c.BankName)
               .IsRequired(false);
        
        builder.Property(c => c.SettlementAccount)
               .IsRequired(false);
        
        builder.Property(c => c.MinimumOrderAmount)
               .IsRequired(false);
        
        builder.Property(c => c.IsVatTaxpayer)
               .IsRequired(false)
               .HasColumnType("BIT");
        
        /* Contacts */
        
        builder.Property(c => c.PhoneNumber)
               .IsRequired(false);
        
        builder.Property(c => c.Email)
               .IsRequired(false);
        
        builder.Property(c => c.Website)
               .IsRequired(false);
        
        builder.Property(c => c.WebsiteLogin)
               .IsRequired(false);
        
        builder.Property(c => c.WebsitePassword)
               .IsRequired(false);
        
        builder.Property(c => c.Comment)
               .IsRequired(false);
    }
}