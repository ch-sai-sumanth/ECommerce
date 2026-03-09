using System.Text.Json;
using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class DatabaseSeeder
{

    public static  async Task SeedAsync(IOptions<DatabaseSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);

        var products = db.GetCollection<Product>(settings.ProductsCollectionName);
        var brands = db.GetCollection<ProductBrand>(settings.BrandsCollectionName);
        var types = db.GetCollection<ProductType>(settings.TypesCollectionName);


        var seedBasePath = Path.Combine("Data", "SeedData");


        //seed brands
        List<ProductBrand> brandsList = new List<ProductBrand>();
        if (await brands.CountDocumentsAsync(_ => true) == 0)
        {
            var brandData = File.ReadAllText(Path.Combine(seedBasePath, "brands.json"));
            brandsList = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
            await brands.InsertManyAsync(brandsList);
        }
        else
        {
            brandsList = await brands.Find(_ => true).ToListAsync();
        }


        //seed types
        List<ProductType> typesList = new List<ProductType>();
        if (await types.CountDocumentsAsync(_ => true) == 0)
        {
            var typesData = File.ReadAllText(Path.Combine(seedBasePath, "types.json"));
            typesList = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            await types.InsertManyAsync(typesList);
        }
        else
        {
            typesList = await types.Find(_=>true).ToListAsync();
        }

        //seed Products
        List<Product> productsList = new List<Product>();
        if (await products.CountDocumentsAsync(_ => true) == 0)
        {
            var productData = File.ReadAllText(Path.Combine(seedBasePath, "products.json"));
            productsList = JsonSerializer.Deserialize<List<Product>>(productData);

            foreach(var product in  productsList)
            {
                //Reset Id to let Mongo generate one
                product.Id = null;
                //Default Created Date if not set
                if (product.CreatedDate == default)
                    product.CreatedDate = DateTime.UtcNow;
            }
            await products.InsertManyAsync(productsList);
        }
    }
}