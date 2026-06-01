using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatsAppSalesAgent.Domain.Entities;

namespace WhatsAppSalesAgent.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.TransactionReference)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.TransactionReference)
            .IsUnique();

        builder.Property(p => p.PaymentIntentId)
            .HasMaxLength(100);

        builder.Property(p => p.ProviderTransactionId)
            .HasMaxLength(100);

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);

        builder.Property(p => p.AmountPaid)
            .HasPrecision(18, 2);

        builder.Property(p => p.AmountRefunded)
            .HasPrecision(18, 2);

        builder.Property(p => p.PlatformFee)
            .HasPrecision(18, 2);

        builder.Property(p => p.PaymentGatewayFee)
            .HasPrecision(18, 2);

        builder.Property(p => p.BusinessEarnings)
            .HasPrecision(18, 2);

        builder.Property(p => p.InstallmentAmount)
            .HasPrecision(18, 2);

        builder.Property(p => p.GatewayResponse)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.Metadata)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.CustomerName)
            .HasMaxLength(200);

        builder.Property(p => p.CustomerEmail)
            .HasMaxLength(200);

        builder.Property(p => p.CustomerPhone)
            .HasMaxLength(20);

        // Relationships
        builder.HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Business)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Owned entities
        builder.OwnsMany(p => p.PaymentLogs, logs =>
        {
            logs.WithOwner().HasForeignKey("PaymentId");
            logs.Property(l => l.Id).ValueGeneratedOnAdd();
            logs.Property(l => l.Status).IsRequired();
            logs.Property(l => l.Message).HasMaxLength(500);
            logs.Property(l => l.Amount).HasPrecision(18, 2);
            logs.Property(l => l.Timestamp).IsRequired();
        });

        builder.Ignore(p => p.DomainEvents);
    }
}