using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BubbleShop.Domain.Entities;

namespace BubbleShop.Infrastructure.Persistence.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.RecipientName).IsRequired().HasMaxLength(200);
        builder.Property(d => d.AddressLine1).IsRequired().HasMaxLength(300);
        builder.Property(d => d.AddressLine2).HasMaxLength(300);
        builder.Property(d => d.City).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Postcode).IsRequired().HasMaxLength(20);
        builder.Property(d => d.Country).IsRequired().HasMaxLength(100);
        builder.Property(d => d.TrackingNumber).HasMaxLength(100);
        builder.Property(d => d.Provider).HasMaxLength(100);

        builder.HasIndex(d => d.TrackingNumber);
    }
}
