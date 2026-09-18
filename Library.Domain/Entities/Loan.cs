using Library.Domain.Enums;

namespace Library.Domain.Entities
{
    public class Loan : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;
        public DateTime LoanDate {  get; set; }
        public DateTime DueDate {  get; set; }
        public DateTime? ReturnDate {  get; set; }
        public LoanStatus Status { get; set; }
    }
}
