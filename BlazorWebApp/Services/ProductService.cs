using BlazorWebApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorWebApp.Services
{

  public class ProductService : IProductService
  {

    private readonly HttpClient client;
    private readonly JsonSerializerOptions options;


    public ProductService(HttpClient httpClient)
    {
      client = httpClient;
      options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }   

    public async Task<List<Product>?> Get(int limit = 10, int offset = 0)
    {      
      var response = await client.GetAsync($"v1/products?limit={limit}&offset={offset}");
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }      

      return JsonSerializer.Deserialize<List<Product>>(content, options);
    }

    public async Task<List<Product>?> Get(Product product)
    {
      List<Product>? products;
      string filter = GetFilters(product);

      if (string.IsNullOrEmpty(filter))
      {
        products = await Get();
      }else{
        products = await Get(filter);
      }

      return products;
    }

    public async Task<Product> Get(int id)
    {
      var response = await client.GetAsync($"v1/products/{id}");
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }      

      return JsonSerializer.Deserialize<Product>(content, options)?? new Product();
    }

    public async Task Add(Product product)
    {
      var response = await client.PostAsync("v1/products", JsonContent.Create(product));
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
    }

    public async Task Update(Product product)
    {
      var response = await client.PutAsync($"v1/products/{product.Id}", JsonContent.Create(product));
      var content = await response.Content.ReadAsStringAsync();
      
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
    }

    public async Task Delete(int productId)
    {
      var response = await client.DeleteAsync($"v1/products/{productId}");
      var content = await response.Content.ReadAsStringAsync();
      
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
    }
    
    private async Task<List<Product>?> Get(string filter)
    {      
      var response = await client.GetAsync($"v1/products/{filter}");
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }      

      return JsonSerializer.Deserialize<List<Product>>(content, options);
    }

    private string GetFilters(Product product){
      string filter = string.Empty;

      if (product.CategoryId > 0)
      {
        filter = $"?categoryId={product.CategoryId}";
      }

      if (!string.IsNullOrEmpty(product.Title.Trim()))
      {
        filter += ((string.IsNullOrEmpty(filter)) ? "?" : "&") + $"title={product.Title.Trim()}";
      }

      return filter;
    }
  }  
}