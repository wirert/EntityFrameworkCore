namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            Console.WriteLine(GetBooksByAgeRestriction(db, "teEN"));
        }

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
    }
}


