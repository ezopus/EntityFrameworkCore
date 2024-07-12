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
        Console.WriteLine(GetProductsInRange(dbContext));

        //File.WriteAllText("../../../Results/products-in-range.json", GetProductsInRange(dbContext));

        //06.
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
}