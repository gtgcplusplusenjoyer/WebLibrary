using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loans");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .IsRequired();

            builder.Property(l => l.BookId)
                .IsRequired();

            builder.Property(l => l.CreatedAt)
                .IsRequired();

            builder.Property(l => l.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(l => l.DueDate)
                .IsRequired();

            builder.Property(l => l.LoanDate)
                .IsRequired();

            builder.Property(l => l.ReturnDate)
                .IsRequired(false);

            builder.HasOne(b => b.Book)
                .WithMany(l => l.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
