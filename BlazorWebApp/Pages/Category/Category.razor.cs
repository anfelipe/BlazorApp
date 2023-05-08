using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWebApp.Pages.Category
{
  public partial class Category
  {
    private List<Models.Category>? categories;

    protected override async Task OnInitializedAsync()
    {
      stateService.OnStateChange += StateHasChanged;
      await GetCategories();
    }

    private async Task GetCategories()
    {
      categories = await categoryService.Get() ?? new List<Models.Category>();
    }

    void AddCategory()
    {
      NavigationManager.NavigateTo($"/category/add/");
    }

    void EditCategory(Models.Category editCategory)
    {
      NavigationManager.NavigateTo($"/category/edit/{editCategory.Id}");
    }

    private void Products(int categoryId)
    {
      NavigationManager.NavigateTo($"/product/category/{categoryId}");
    }

    private async Task DeleteCategory(Models.Category category)
    {
      if (!await JSRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to remove {category.Name}"))
        return;
      categories = null;
      await categoryService.Delete(category.Id);
      toastService.ShowSuccess("Category Delete");
      await GetCategories();
    }

    public void Dispose()
    {
      stateService.OnStateChange -= StateHasChanged;
    }
  }
}