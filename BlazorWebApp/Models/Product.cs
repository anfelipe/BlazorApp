namespace BlazorWebApp.Models
{  
  public class Product
  {
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public long Price { get; set; }

    public string Description { get; set; } = string.Empty; 

    public int CategoryId { get; set; }    

    public Category? Category { get; set; }

    public String[]? Images { get; set; }
    
  }  
}
