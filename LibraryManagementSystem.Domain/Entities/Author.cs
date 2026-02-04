using System.Collections.Generic;

namespace LibraryManagementSystem.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
