namespace Catalog.Infrastructure.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string BrandsCollectionName { get; set; }
    public string TypesCollectionName { get; set; }
    public string ProductsCollectionName { get; set; }
}