namespace ClickBuy_Api.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }
		public int SubTotal { get; set; }
		public string? NamePayment { get; set; }

        public string? BankName { get; set; }
        public string? BankNumber { get; set; }

        public DateTime? CreatedAt { get; set; }
		public List<Cart>? Carts { get; set; }
	}
}
