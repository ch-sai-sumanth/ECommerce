using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class TypeRepository : ITypeRepository
{

    private readonly IMongoCollection<ProductType> _productTypesCollection;

    public TypeRepository(IOptions<DatabaseSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _productTypesCollection = database.GetCollection<ProductType>(settings.TypesCollectionName);
    }
    public async Task<IEnumerable<ProductType>> GetAllTypesAsync()
    {
        return await _productTypesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<ProductType> GetByIdAsync(string id)
    {
        return await _productTypesCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
    }
}