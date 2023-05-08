using BlazorWebApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorWebApp.Services
{

  public class CategoryService : ICategoryService
  {
    private readonly HttpClient client;
    private readonly JsonSerializerOptions options;

    public CategoryService(HttpClient httpClient)
    {
      client = httpClient;      
      options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task Add(Category category)
    {
      var response = await client.PostAsync("v1/categories", JsonContent.Create(category));
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
    }

    public async Task Delete(int categoryId)
    {
      var response = await client.DeleteAsync($"v1/categories/{categoryId}");
      var content = await response.Content.ReadAsStringAsync();
      
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
    }

    public async Task<List<Category>?> Get(int limit = 0, int offset = 0)
    {
      var response = await client.GetAsync($"v1/categories?limit={limit}&offset={offset}");
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }

      return JsonSerializer.Deserialize<List<Category>>(content, options);
    }

    public async Task<Category> Get(int id)
    {
      var response = await client.GetAsync($"v1/categories/{id}");
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }      

      return JsonSerializer.Deserialize<Category>(content, options)?? new Category();
    }

    public async Task Update(Category category)
    {
      var response = await client.PutAsync($"v1/categories/{category.Id}", JsonContent.Create(category));
      var content = await response.Content.ReadAsStringAsync();
      
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
    }
  }
}