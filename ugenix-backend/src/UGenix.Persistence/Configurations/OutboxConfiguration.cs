using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGenix.Persistence.Outbox;

namespace UGenix.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Content).HasColumnType("jsonb").IsRequired();
        
        // Critical index for the background processor
        builder.HasIndex(x => new { x.ProcessedOnUtc, x.OccurredOnUtc })
               .HasFilter("\"ProcessedOnUtc\" IS NULL");
    }
}

