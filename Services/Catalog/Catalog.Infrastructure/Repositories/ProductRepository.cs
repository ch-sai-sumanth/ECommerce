using System.Diagnostics;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _productsCollection;
    private readonly IMongoCollection<ProductBrand> _brandsCollection;
    private readonly IMongoCollection<ProductType> _typesCollection;

    public ProductRepository(IOptions<DatabaseSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _productsCollection = database.GetCollection<Product>(settings.ProductsCollectionName);
        _brandsCollection = database.GetCollection<ProductBrand>(settings.BrandsCollectionName);
        _typesCollection = database.GetCollection<ProductType>(settings.TypesCollectionName);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
       return await _productsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!String.IsNullOrEmpty(catalogSpecParams.Search))
        {
            filter &= builder.Where(p=>p.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
        }

        if (!String.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            filter &= builder.Eq(p=>p.Brand.Id, catalogSpecParams.BrandId);
        }

        if (!String.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            filter &= builder.Eq(p=>p.Type.Id, catalogSpecParams.TypeId);
        }

        var totalItems = await _productsCollection.CountDocumentsAsync(filter);
        var data = await ApplyDataFilters(catalogSpecParams, filter);

        return new Pagination<Product>()
        {
            PageSize = catalogSpecParams.PageSize,
            PageIndex = catalogSpecParams.PageIndex,
            Count = (int) totalItems,
            Data = data
        };
    }


    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        var filter = Builders<Product>.Filter.Regex(p=>p.Name,new BsonRegularExpression($".*{name}.*","i"));
        return await _productsCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string name)
    {
       return await _productsCollection
           .Find(p=>p.Brand.Name.ToLower()==name.ToLower())
           .ToListAsync();
    }

    public async Task<Product> GetProductAsync(string productId)
    {
        return await _productsCollection.Find(p => p.Id == productId).FirstOrDefaultAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _productsCollection.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var updatedProduct = await _productsCollection.ReplaceOneAsync(p => p.Id == product.Id, product);
        return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string productId)
    {
        var deletedProduct = await _productsCollection.DeleteOneAsync(p => p.Id == productId);
        return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
    }

    public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
    {
        return await _brandsCollection.Find(b => b.Id == brandId).FirstOrDefaultAsync();
    }

    public async Task<ProductType> GetTypeByIdAsync(string typeId)
    {
       return await  _typesCollection.Find(t => t.Id == typeId).FirstOrDefaultAsync();
    }

    private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        var sortDef = Builders<Product>.Sort.Ascending(p => p.Name);

        if (!String.IsNullOrEmpty(catalogSpecParams.Sort))
        {
            sortDef = catalogSpecParams.Sort switch
            {
                "PriceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                "PriceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                _ => Builders<Product>.Sort.Ascending(p => p.Name)
            };
        }

        return await _productsCollection
            .Find(filter)
            .Sort(sortDef)
            .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
            .Limit(catalogSpecParams.PageSize)
            .ToListAsync();
    }

}