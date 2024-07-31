using BookShop.Initializer;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace BookShop
{
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //Exercise 02
            //string age = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, age!));

            //Exercise 03
            //Console.WriteLine(GetGoldenBooks(db));

            //Exercise 04
            //Console.WriteLine(GetBooksByPrice(db));

            //Exercise 05
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));

            //Exercise 06
            //string categories = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, categories));

            //Exercise 07
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            //Exercise 08
            //string name = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, name));

            //Exercise 09
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, input));

            //Exercise 10
            //int length = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, length));

            //Exercise 11
            //string authorName = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, authorName));

            //Exercise 12
            //Console.WriteLine(CountCopiesByAuthor(db));

            //Exercise 13
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //Exercise 14
            //Console.WriteLine(GetMostRecentBooks(db));

            //Exercise 15
            //IncreasePrices(db);

            //Exercise 16
            //Console.WriteLine(RemoveBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            try
            {
                //try to parse input to compare in where statement
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

                string[] result = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .Select(b => b.Title)
                    .OrderBy(b => b)
                    .ToArray();

                return string.Join(Environment.NewLine, result);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var result = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var result = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var result = context.Books
                .Where(b => b.BookCategories
                    .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var result = context.Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var result = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var result = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(a => new
                {
                    AuthorFullName = a.FirstName + " " + a.LastName,
                    AuthorBooks = a.Books.OrderBy(b => b.BookId).Select(b => b.Title),
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var a in authors)
            {
                foreach (var b in a.AuthorBooks)
                {
                    sb.AppendLine($"{b} ({a.AuthorFullName})");
                }
            }

            return sb.ToString().Trim();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var result = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.BookId)
                .ToArray()
                .Length;

            return result;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var result = context.Authors
                .Include(a => a.Books)
                .Select(a => new
                {
                    AuthorName = a.FirstName + " " + a.LastName,
                    BookCount = a.Books.Sum(b => b.Copies)
                })
                .ToArray()
                .OrderByDescending(b => b.BookCount);

            foreach (var a in result)
            {
                sb.AppendLine($"{a.AuthorName} - {a.BookCount}");
            }

            return sb.ToString().Trim();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesProfit = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name);

            StringBuilder sb = new StringBuilder();

            foreach (var cat in categoriesProfit)
            {
                sb.AppendLine($"{cat.Name} ${cat.TotalProfit:f2}");
            }

            return sb.ToString().Trim();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var result = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Select(b => new
                        {
                            b.Book.Title,
                            b.Book.ReleaseDate.Value.Year
                        })
                        .Take(3)
                        .ToArray(),

                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var cat in result)
            {
                sb.AppendLine($"--{cat.Name}");
                foreach (var book in cat.MostRecentBooks)
                {
                    sb.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return sb.ToString().Trim();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var booksToRisePrice = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in booksToRisePrice)
            {
                book.Price += 5;
            }

            context.SaveChanges();

        }

        public static int RemoveBooks(BookShopContext context)
        {
            var result = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.RemoveRange(result);

            context.SaveChanges();

            return result.Length;
        }
    }
}


