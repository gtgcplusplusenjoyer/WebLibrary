using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .IsRequired();

            builder.Property(r => r.Token)
                .IsRequired()
                .HasMaxLength(512);

            builder.HasIndex(r => r.Token)
                .IsUnique();

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.ExpiresAt)
                .IsRequired();

            builder.Property(r => r.RevokedAt)
                .IsRequired(false);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Ignore(r => r.IsRevoked);

            builder.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
