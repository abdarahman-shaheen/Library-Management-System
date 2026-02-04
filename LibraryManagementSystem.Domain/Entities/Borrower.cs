namespace LibraryManagementSystem.Domain.Entities
{
    public class Borrower : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
