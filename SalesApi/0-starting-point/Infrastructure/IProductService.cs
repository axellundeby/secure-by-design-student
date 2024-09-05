namespace SalesApi.Domain.Services;

public interface IProductService
{
    Task<(ReadDataResult, List<Product>?)> GetAllAvailableProducts();
    Task<(ReadDataResult, Product?)> GetWith(ProductId id);
}