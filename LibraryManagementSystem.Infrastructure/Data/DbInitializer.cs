using System.Text;
using System.Security.Cryptography;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(LibraryDbContext context)
        {


            // Seed/Update Users
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes("Password123!");
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var hash = Convert.ToBase64String(hashBytes);

            if (!context.Users.Any())
            {
                var users = new User[]
                {
                    new User 
                    { 
                        Username = "admin", 
                        Email = "admin@library.com", 
                        PasswordHash = hash, 
                        Role = "Admin",
                        RefreshToken = Guid.NewGuid().ToString(),
                        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
                    },
                    new User 
                    { 
                        Username = "user", 
                        Email = "user@library.com", 
                        PasswordHash = hash, 
                        Role = "User",
                        RefreshToken = Guid.NewGuid().ToString(),
                        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
                    }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
            else
            {
                // Ensure existing users have a refresh token (for migration purposes)
                var existingUsers = context.Users.Where(u => u.RefreshToken == null).ToList();
                if (existingUsers.Any())
                {
                    foreach (var user in existingUsers)
                    {
                        user.RefreshToken = Guid.NewGuid().ToString();
                        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                    }
                    context.SaveChanges();
                }
            }

            // Seed Authors
            if (!context.Authors.Any())
            {
                var authors = new Author[]
                {
                    new Author { Name = "J.K. Rowling", Bio = "British author, best known for the Harry Potter series." },
                    new Author { Name = "George R.R. Martin", Bio = "American novelist and short story writer, screenwriter, and television producer." },
                    new Author { Name = "J.R.R. Tolkien", Bio = "English writer, poet, philologist, and academic." }
                };
                context.Authors.AddRange(authors);
                context.SaveChanges();

                // Seed Books
                if (!context.Books.Any())
                {
                    var books = new Book[]
                    {
                        new Book { Title = "Harry Potter and the Philosopher's Stone", ISBN = "9780747532743", PublishedDate = new DateTime(1997, 6, 26), AuthorId = authors[0].Id },
                        new Book { Title = "Harry Potter and the Chamber of Secrets", ISBN = "9780747538493", PublishedDate = new DateTime(1998, 7, 2), AuthorId = authors[0].Id },
                        new Book { Title = "A Game of Thrones", ISBN = "9780553103540", PublishedDate = new DateTime(1996, 8, 1), AuthorId = authors[1].Id },
                        new Book { Title = "The Hobbit", ISBN = "9780048230873", PublishedDate = new DateTime(1937, 9, 21), AuthorId = authors[2].Id }
                    };
                    context.Books.AddRange(books);
                    context.SaveChanges();
                }
            }

            // Seed Borrowers
            if (!context.Borrowers.Any())
            {
                var borrowers = new Borrower[]
                {
                    new Borrower { Name = "John Doe", Email = "john@example.com", Phone = "555-0101" },
                    new Borrower { Name = "Jane Smith", Email = "jane@example.com", Phone = "555-0102" }
                };
                context.Borrowers.AddRange(borrowers);
                context.SaveChanges();
            }
        }
    }
}
