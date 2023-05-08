using BlazorWebApp.Models;

namespace BlazorWebApp.Services
{
  public interface ICategoryService
  {
    Task<List<Category>?> Get(int limit = 0, int offset = 0);

    Task<Category> Get(int id);

    Task Add(Category category);

    Task Update(Category category);

    Task Delete(int categoryId);
  }
}