using Microsoft.AspNetCore.Components;

namespace BlazorWebApp.Pages.Products
{
  public partial class AddProduct
  {
    [Parameter]
    public string? IdProductParam { get; set; }

    private string NameBottonSave = "Save";

    private string Image { get; set; } = string.Empty;

    private Models.Product product = new();

    private List<Models.Category> categories = new();

    public override async Task SetParametersAsync(ParameterView parameters)
    {
      if (parameters.TryGetValue<string>(nameof(IdProductParam), out var value))
      {
        if (value is not null)
        {
          product.Id = int.Parse(value ?? "0");
          NameBottonSave = "Update";
        }
      }

      await base.SetParametersAsync(parameters);
    }

    protected override async Task OnInitializedAsync()
    {
      if (product.Id > 0)
      {
        product = await productService.Get(product.Id);
        Image = product.Images?[0] ?? string.Empty;
        product.CategoryId = product.Category?.Id ?? 0;
      }

      categories = await categoryService.Get() ?? new List<Models.Category>();
    }

    private async Task Save()
    {
      product.Images = new string[1]
      {
                Image
      };
      if (product.Id > 0)
      {
        await productService.Update(product);
        toastService.ShowSuccess("Product Updated");
      }
      else
      {
        await productService.Add(product);
        toastService.ShowSuccess("Product Created");
      }

      NavigationManager.NavigateTo("/product");
    }
  }
}