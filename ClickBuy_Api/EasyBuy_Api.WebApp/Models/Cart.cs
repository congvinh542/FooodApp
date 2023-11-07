namespace ClickBuy_Api.Models
{
	public class Cart
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int TotalPrice { get; set; }
		public int ProductId { get; set; }
		public string? Size { get; set; }
		public DateTime CreatedAt { get; set; }
		public Product? Product { get; set; }
	}
}
