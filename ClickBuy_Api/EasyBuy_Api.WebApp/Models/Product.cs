namespace ClickBuy_Api.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int	Price{ get; set; }
		public string?	Image{ get; set; }
		public string?	Description{ get; set; }
		public bool?	Status{ get; set; }
		public string?	Brand{ get; set; }
		public string?	NameCategory { get; set; }
		public DateTime? CreatedAt { get; set; }
		public int? CategoryId { get; set; }
		public Category? Categorys { get; set; }	
	}

}
