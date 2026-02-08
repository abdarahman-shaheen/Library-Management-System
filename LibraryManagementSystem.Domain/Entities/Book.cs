namespace LibraryManagementSystem.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public bool IsBorrowed { get; set; } = false;
    }
}
