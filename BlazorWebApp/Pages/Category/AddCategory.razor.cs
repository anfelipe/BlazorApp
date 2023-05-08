using Microsoft.AspNetCore.Components;

namespace BlazorWebApp.Pages.Category
{
  public partial class AddCategory
  {
    [Parameter]
    public string? IdCategoryParam { get; set; }

    private string NameBottonSave = "Save";

    private Models.Category category = new();

    public override async Task SetParametersAsync(ParameterView parameters)
    {
      if (parameters.TryGetValue<string>(nameof(IdCategoryParam), out var value))
      {
        if (value is not null)
        {
          category.Id = int.Parse(value ?? "0");
          NameBottonSave = "Update";
        }
      }

      await base.SetParametersAsync(parameters);
    }

    protected override async Task OnInitializedAsync()
    {
      if (category.Id > 0)
      {
        category = await categoryService.Get(category.Id);
      }
    }

    private async Task Save()
    {
      if (category.Id > 0)
      {
        await categoryService.Update(category);
        toastService.ShowSuccess("Category Updated");
      }
      else
      {
        await categoryService.Add(category);
        toastService.ShowSuccess("Category Created");
      }

      NavigationManager.NavigateTo("/category");
    }

    void GoBack()
    {
      NavigationManager.NavigateTo("/category");
    }
  }
}