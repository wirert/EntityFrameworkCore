namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ValueGeneration;
    using System.Linq;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //IncreasePrices(db);

            Console.WriteLine(RemoveBooks(db));
        }

        // 02. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                            .AsNoTracking()
                            .Where(b => b.AgeRestriction == (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true))
                            .Select(b => b.Title)
                            .OrderBy(b => b)
                            .ToArray();

            sb.Append(string.Join(Environment.NewLine, books));

            return sb.ToString().TrimEnd();
        }

        //03. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .AsNoTracking()
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //04. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .AsNoTracking()
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //05. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .AsNoTracking()
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //06. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var inputCategories = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            var books = context.Books
                .AsNoTracking()
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .Where(b => b.BookCategories.All(c => inputCategories.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //07. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateAsDateTime = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var books = context.Books
                .AsNoTracking()
                .Where(b => b.ReleaseDate.Value.Date.CompareTo(dateAsDateTime) < 0)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //08. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            int inputLength = input.Length;

            var authors = context.Authors
                .AsNoTracking()
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .OrderBy(a => a)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        //09. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .AsNoTracking()
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //10. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //11. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .AsNoTracking()
                .Where(b => b.Title.Length > lengthCheck)
                .Count();
        }

        //12. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .AsNoTracking()
                .OrderByDescending(a => a.Books.Sum(b => b.Copies))
                .Select(a => $"{a.FirstName} {a.LastName} - {a.Books.Sum(b => b.Copies)}")
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        //13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var totalProfitByCategory = context.Categories
                 .AsNoTracking()
                 .Select(c => new
                 {
                     c.Name,
                     TotalProfit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                 })
                 .OrderByDescending(c => c.TotalProfit)
                 .ToArray();

            foreach (var c in totalProfitByCategory)
            {
                sb.AppendLine($"{c.Name} ${c.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    RecentBooks = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Take(3)
                        .Select(b => $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})")
                })
                .OrderBy(c => c.Name)
                .ToArray();

            foreach (var c in categories)
            {
                sb.AppendLine($"--{c.Name}")
                    .AppendLine(string.Join(Environment.NewLine, c.RecentBooks));
            }

            return sb.ToString().TrimEnd();
        }

        //15. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var b in books)
            {
                b.Price += 5;
            }

            context.SaveChanges();
        }

        //16. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200);

            int removedBooks = books.Count();

            context.Books.RemoveRange(books);

            context.SaveChanges();

            return removedBooks;
        }
    }
}


