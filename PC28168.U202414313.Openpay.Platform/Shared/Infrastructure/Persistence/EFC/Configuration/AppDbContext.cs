using Microsoft.EntityFrameworkCore;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202414313.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PC28168.U202414313.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Interceptors;

namespace PC28168.U202414313.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<DisbursementOrder>().HasKey(d => d.Id);
        builder.Entity<DisbursementOrder>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Entity<DisbursementOrder>()
            .Property(d => d.OrderTrackingCode)
            .HasConversion(vo => vo.Value.ToString(), value => new OpenpayReference(Guid.Parse(value)))
            .HasMaxLength(36)
            .IsRequired();
        builder.Entity<DisbursementOrder>()
            .HasIndex(d => d.OrderTrackingCode)
            .IsUnique();

        builder.Entity<DisbursementOrder>()
            .Property(d => d.AffiliateStoreId)
            .HasConversion(vo => vo.Value.ToString(), value => new StoreIdentifier(Guid.Parse(value)))
            .HasMaxLength(36)
            .IsRequired();

        builder.Entity<DisbursementOrder>().OwnsOne(d => d.ExecutionSchedule, es =>
        {
            es.WithOwner().HasForeignKey("Id");
            es.Property(p => p.ScheduledFrom).HasColumnName("scheduled_from").IsRequired();
            es.Property(p => p.ScheduledTo).HasColumnName("scheduled_to").IsRequired();
        });
        builder.Entity<DisbursementOrder>().Navigation(d => d.ExecutionSchedule).IsRequired();

        builder.Entity<DisbursementOrder>().OwnsOne(d => d.TransferCap, tc =>
        {
            tc.WithOwner().HasForeignKey("Id");
            tc.Property(p => p.Amount).HasColumnName("transfer_cap_amount").HasPrecision(18, 2).IsRequired();
            tc.Property(p => p.CurrencyCode).HasColumnName("transfer_cap_currency_code").HasMaxLength(10).IsRequired();
        });
        builder.Entity<DisbursementOrder>().Navigation(d => d.TransferCap).IsRequired();

        builder.Entity<DisbursementOrder>()
            .Property(d => d.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Entity<DisbursementOrder>()
            .Property(d => d.User)
            .HasMaxLength(20)
            .IsRequired();

        builder.Entity<DisbursementOrder>()
            .Property(d => d.Remarks)
            .HasMaxLength(85)
            .IsRequired(false);

        builder.UseSnakeCaseNamingConvention();
    }
}