using System;

namespace LibraryManagementSystem.Application.Common.Dtos
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public BookDto? Book { get; set; }
        public int BorrowerId { get; set; }
        public BorrowerDto? Borrower { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
