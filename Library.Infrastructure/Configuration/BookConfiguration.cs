using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace Library.Infrastructure.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
               .IsRequired();

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Publisher)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Description)
                .HasMaxLength(1000);

            builder.Property(b => b.TotalCopies)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(b => b.AvailableCopies)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.HasMany(b=>b.BookAuthors)
                .WithOne(ba=>ba.Book)
                .HasForeignKey(ba=>ba.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
