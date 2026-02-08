namespace LibraryManagementSystem.Application.Common.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }
}
