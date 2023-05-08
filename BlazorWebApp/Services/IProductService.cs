using BlazorWebApp.Models;

namespace BlazorWebApp.Services
{
  public interface IProductService
  {
    Task<List<Product>?> Get(int limit = 10, int offset = 0);

    Task<List<Product>?> Get(Product product);

    Task<Product> Get(int id);

    Task Add(Product product);

    Task Update(Product product);

    Task Delete(int productId);
  }
}