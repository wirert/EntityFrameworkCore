namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using DataProcessor.ImportDto;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(BookImportDto[]), new XmlRootAttribute("Books"));
            var sb = new StringBuilder();

            BookImportDto[] bookDtos;
            using (var reader = new StringReader(xmlString))
            {
                bookDtos = (BookImportDto[])serializer.Deserialize(reader);
            }

            var books = new HashSet<Book>();

            foreach (var book in bookDtos)
            {
                if (!IsValid(book))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                books.Add(new Book()
                {
                    Name = book.Name,
                    Genre = (Genre)book.Genre,
                    Price = book.Price,
                    Pages = book.Pages,
                    PublishedOn = DateTime.ParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                });

                sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorDtos = JsonConvert.DeserializeObject<AuthorImportDto[]>(jsonString);
            var sb = new StringBuilder();
            var authors = new List<Author>();
            var validBookIds = context.Books.Select(b => b.Id).ToArray();

            foreach (var author in authorDtos)
            {
                if (!IsValid(author))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (authors.Exists(a => a.Email == author.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var authorEntity = new Author()
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Email = author.Email,
                    Phone = author.Phone,
                    AuthorsBooks = author.Books
                                        .Where(ab => validBookIds
                                        .Contains(ab.BookId))
                                        .Select(ab => new AuthorBook()
                                        {
                                            BookId = ab.BookId
                                        })
                                        .ToHashSet()
                };

                if (!authorEntity.AuthorsBooks.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(authorEntity);

                sb.AppendLine(string
                    .Format(SuccessfullyImportedAuthor, $"{authorEntity.FirstName} {authorEntity.LastName}", authorEntity.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}