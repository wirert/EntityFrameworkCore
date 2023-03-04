namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            Console.WriteLine(CountBooks(db, 12));
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
                .Select (b => b.Title)
                .ToArray();

            return string.Join (Environment.NewLine, books);
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

            return string.Join (Environment.NewLine, authors);
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
                .Where(b =>b.Author.LastName.ToLower().StartsWith(input.ToLower()))
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

    }
}


