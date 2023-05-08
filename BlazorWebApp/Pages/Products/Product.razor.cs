using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWebApp.Pages.Products
{
  public partial class Product
  {
    [Parameter]
    public string? IdCategoryParam { get; set; }

    private Models.Product product = new();

    private List<Models.Product>? products;

    private List<Models.Category> categories = new();

    public override async Task SetParametersAsync(ParameterView parameters)
    {
      if (parameters.TryGetValue<string>(nameof(IdCategoryParam), out var value))
      {
        if (value is not null)
        {
          product.CategoryId = int.Parse(value ?? "0");
        }
      }

      await base.SetParametersAsync(parameters);
    }

    protected override async Task OnInitializedAsync()
    {
      stateService.OnStateChange += StateHasChanged;
      GetState();
      await GetCategories();
      await GetProducts();
    }

    private async Task GetProducts()
    {
      products = null;
      if (product.CategoryId > 0 || !string.IsNullOrEmpty(product.Title.Trim()))
      {
        products = await productService.Get(product);
      }
      else
      {
        products = await productService.Get();
      }
    }

    private async Task GetCategories()
    {
      categories = await categoryService.Get() ?? new List<Models.Category>();
    }

    void EditProduct(Models.Product editProduct)
    {
      NavigationManager.NavigateTo($"/product/edit/{editProduct.Id}");
      stateService.SetValue(product);
    }

    void GetState()
    {
      if (stateService.ValueObject is not null)
      {
        product = (Models.Product)stateService.ValueObject;
      }
    }

    private async Task DeleteProduct(Models.Product product)
    {
      if (!await JSRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to remove {product.Title}"))
        return;
      await productService.Delete(product.Id);
      toastService.ShowSuccess("Product Delete");
      products = null;
      await GetProducts();
    }

    public void Dispose()
    {
      stateService.OnStateChange -= StateHasChanged;
    }
  }
}