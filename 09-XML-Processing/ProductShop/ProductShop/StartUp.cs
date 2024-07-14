using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext dbContext = new ProductShopContext();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();


            //string inputUserXml = File.ReadAllText("../../../Datasets/users.xml");
            //string inputProductsXml = File.ReadAllText("../../../Datasets/products.xml");
            //string inputCategoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            //string inputCategoriesProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            ////1. Import users
            //Console.WriteLine(ImportUsers(dbContext, inputUserXml));

            ////2. Import products
            //Console.WriteLine(ImportProducts(dbContext, inputProductsXml));

            ////3. Import categories
            //Console.WriteLine(ImportCategories(dbContext, inputCategoriesXml));

            ////4. Import categories and products
            //Console.WriteLine(ImportCategoryProducts(dbContext, inputCategoriesProductsXml));

            //5. Export Products In Range
            //Console.WriteLine(GetProductsInRange(dbContext));

            //6. Export Sold Products
            //Console.WriteLine(GetSoldProducts(dbContext));

            //7. Export Categories By Products Count
            //Console.WriteLine(GetCategoriesByProductsCount(dbContext));

            //8. Export Users and Products
            Console.WriteLine(GetUsersWithProducts(dbContext));
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var usersDtos = xmlHelper.Deserialize<ImportUserDto[]>(inputXml, "Users");

            ICollection<User> users = new List<User>();

            foreach (var userDto in usersDtos)
            {
                User user = new User()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Age = userDto.Age,
                };

                users.Add(user);
            }

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var productsDtos = xmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");

            ICollection<Product> products = new HashSet<Product>();

            foreach (var productDto in productsDtos)
            {
                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId.HasValue ? (int)productDto.BuyerId : null,
                };

                products.Add(product);
            }

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var categoriesDtos = xmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            ICollection<Category> categories = new HashSet<Category>();

            foreach (var categoryDto in categoriesDtos)
            {
                Category category = new Category()
                {
                    Name = categoryDto.Name,
                };

                categories.Add(category);
            }

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var categoriesProductsDtos =
                xmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            var validCategories = context.Categories
                .Select(c => c.Id)
                .ToArray();
            var validProducts = context.Products
                .Select(p => p.Id)
                .ToArray();

            ICollection<CategoryProduct> categoriesProducts = new HashSet<CategoryProduct>();

            foreach (var cpDto in categoriesProductsDtos)
            {
                if (validCategories.All(c => c != cpDto.CategoryId)
                    || validProducts.All(p => p != cpDto.ProductId))
                {
                    continue;
                }

                CategoryProduct categoryProduct = new CategoryProduct()
                {
                    CategoryId = cpDto.CategoryId,
                    ProductId = cpDto.ProductId,
                };

                categoriesProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoriesProducts);

            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    FullName = p.BuyerId.HasValue
                        ? p.Buyer.FirstName + " " + p.Buyer.LastName
                        : null,
                })
                .Take(10)
                .ToArray();

            var productDtos = products
                .Select(product => new ExportProductInRageDto()
                {
                    Name = product.Name,
                    Price = Math.Round(product.Price, 2),
                    Buyer = product.FullName
                })
                .ToArray();

            return xmlHelper.Serialize(productDtos, "Products");
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    FirstName = u.FirstName ?? null,
                    u.LastName,
                    SoldItems = u.ProductsSold
                        .Select(p => new
                        {
                            p.Name,
                            p.Price,
                        })
                        .ToArray(),
                })
                .Take(5)
                .ToArray();

            var userDtos = users
                .Select(u => new ExportUserAndProducts()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Products = u.SoldItems.Select(p => new SoldProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                    }).ToArray(),
                })
                .ToArray();

            return xmlHelper.Serialize(userDtos, "Users");
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            var categoriesRaw = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    NumberOfProducts = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .AsNoTracking()
                .ToArray()
                .OrderByDescending(c => c.NumberOfProducts)
                .ThenBy(c => c.TotalRevenue);

            var categoryDtos = categoriesRaw
                .Select(c => new ExportCategoryDto()
                {
                    Name = c.Name,
                    AveragePrice = Math.Round(c.AveragePrice, 6),
                    Count = c.NumberOfProducts,
                    TotalRevenue = Math.Round(c.TotalRevenue, 2),
                })
                .ToArray();

            return xmlHelper.Serialize(categoryDtos, "Categories");
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var usersRaw = context
                .Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new
                {
                    FirstName = u.FirstName ?? null,
                    u.LastName,
                    Age = u.Age ?? null,
                    productsCount = u.ProductsSold.Count,
                    SoldProducts = u.ProductsSold
                        .Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray(),
                })
                .AsNoTracking()
                .Take(10)
                .ToArray();

            var usersDtos = new ExportUserAndProductsWrapperDto()
            {
                Count = usersRaw.Length,
                Users = usersRaw
                    .Select(u => new ExportUserWithProducts()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        ProductsSold = new ExportUserProductSoldWrapper()
                        {
                            SoldProductsCount = u.SoldProducts.Length,
                            Products = u.SoldProducts
                                .Select(pr => new ExportUserProductSold()
                                {
                                    Name = pr.Name,
                                    Price = pr.Price,
                                })
                                .ToArray(),
                        }
                    })
                    .ToArray()
            };

            return xmlHelper.Serialize(usersDtos, "Users");
        }
    }
}