using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext dbContext = new ProductShopContext();

        ////01.Import Users
        //string inputJson1 = File.ReadAllText(@"../../../Datasets/users.json");

        //string result = ImportUsers(dbContext, inputJson1);


        ////02.Import Products
        //string inputJson2 = File.ReadAllText(@"../../../Datasets/products.json");

        //Console.WriteLine(ImportProducts(dbContext, inputJson2));

        ////03.Import Categories
        //string inputJson3 = File.ReadAllText("../../../Datasets/categories.json");

        //Console.WriteLine(ImportCategories(dbContext, inputJson3));

        //04. Import CategoriesProducts
        //string inputJson4 = File.ReadAllText("../../../Datasets/categories-products.json");

        //Console.WriteLine(ImportCategoryProducts(dbContext, inputJson4));

        //05. Export Products in price range between 500 and 1000
        //Console.WriteLine(GetProductsInRange(dbContext));

        //File.WriteAllText("../../../Results/products-in-range.json", GetProductsInRange(dbContext));

        //06. Export sold products with at least 1 buyer
        //Console.WriteLine(GetSoldProducts(dbContext));

        //07. Get all categories
        //Console.WriteLine(GetCategoriesByProductsCount(dbContext));

        //08. Users with sold products
        Console.WriteLine(GetUsersWithProducts(dbContext));
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));

        ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

        ICollection<User> validUsers = new HashSet<User>();

        foreach (var userDto in userDtos)
        {
            User user = mapper.Map<User>(userDto);

            validUsers.Add(user);
        }

        //add all valid users to db
        context.Users.AddRange(validUsers);

        context.SaveChanges();

        return $"Successfully imported {validUsers.Count}";
    }

    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

        ICollection<Product> products = new HashSet<Product>();

        foreach (var p in productDtos)
        {
            var product = new Product()
            {
                Name = p.Name,
                BuyerId = p.BuyerId,
                SellerId = p.SellerId,
                Price = p.Price,
            };

            products.Add(product);
        }
        context.Products.AddRange(products);

        context.SaveChanges();

        return $"Successfully imported {productDtos.Length}";
    }

    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        ImportCategoryDto[] categoryDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

        ICollection<Category> categories = new HashSet<Category>();

        foreach (var c in categoryDtos)
        {
            if (c.Name == null) continue;

            var category = new Category()
            {
                Name = c.Name,
            };
            categories.Add(category);
        }

        context.Categories.AddRange(categories);

        context.SaveChanges();

        return $"Successfully imported {categories.Count}";
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        ImportCategoryProductDto[] importCategoryProductDtos =
            JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson)!;

        ICollection<CategoryProduct> categoriesProducts = new HashSet<CategoryProduct>();

        foreach (var cp in importCategoryProductDtos)
        {
            var categoryProduct = new CategoryProduct()
            {
                CategoryId = cp.CategoryId,
                ProductId = cp.ProductId,
            };

            categoriesProducts.Add(categoryProduct);
        }

        context.CategoriesProducts.AddRange(categoriesProducts);

        context.SaveChanges();

        return $"Successfully imported {categoriesProducts.Count}";
    }

    public static string GetProductsInRange(ProductShopContext context)
    {
        var productDtos = context.Products
            .Include(p => p.Seller)
            .Where(p => p.Price >= 500 &&
                        p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Select(p => new ExportProductDto(p))
            .ToArray();

        return JsonConvert.SerializeObject(productDtos, Formatting.Indented);
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        //TODO: try to rewrite with DTOs when knowing how
        var soldProductsDtos = context.Users
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Include(u => u.ProductsSold)
            .AsNoTracking()
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                soldProducts = u.ProductsSold.Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    buyerFirstName = p.Buyer.FirstName,
                    buyerLastName = p.Buyer.LastName
                })
            })
            .ToArray();

        return JsonConvert.SerializeObject(soldProductsDtos, Formatting.Indented);
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        var categoriesDtos = context.Categories
            .OrderByDescending(c => c.CategoriesProducts.Count)
            .Select(c => new
            {
                category = c.Name,
                productsCount = c.CategoriesProducts.Count,
                averagePrice = c.CategoriesProducts.Average(cat => cat.Product.Price).ToString("f2"),
                totalRevenue = c.CategoriesProducts.Sum(p => p.Product.Price).ToString("f2")
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(categoriesDtos, Formatting.Indented);
    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var users = context
            .Users
            .Where(u => u.ProductsSold.Any(p => p.BuyerId != null && p.Price != null))
            .Select(u => new
            {
                //UserDTO
                firstName = u.FirstName,
                lastName = u.LastName,
                age = u.Age,
                soldProducts = new
                {
                    //ProductWrapper DTO
                    count = u.ProductsSold.Count(p => p.Buyer != null),
                    products = u.ProductsSold.Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            //Product DTO
                            name = p.Name,
                            price = p.Price,
                        })
                        .ToArray(),
                }

            })
            .OrderByDescending(u => u.soldProducts.count)
            .AsNoTracking()
            .ToArray();

        var userWrapperDTO = new
        {
            usersCount = users.Length,
            users = users
        };

        return JsonConvert.SerializeObject(userWrapperDTO, Formatting.Indented, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
        });
    }
}
