using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace Catalog.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{

    private readonly IMongoCollection<ProductBrand> _brandsCollection;

    public BrandRepository(IOptions<DatabaseSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        _brandsCollection = db.GetCollection<ProductBrand>(settings.BrandsCollectionName);
    }
    public async Task<IEnumerable<ProductBrand>> GetAllBrandsAsync()
    {
        return await _brandsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<ProductBrand> GetByIdAsync(string id)
    {
       return await _brandsCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
    }
}