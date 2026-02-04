using System;

namespace LibraryManagementSystem.Domain.Entities
{
    public class Loan : BaseEntity
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int BorrowerId { get; set; }
        public Borrower? Borrower { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
