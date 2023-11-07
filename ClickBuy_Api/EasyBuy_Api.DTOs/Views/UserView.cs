using ClickBuy_Api.DTOs.Queries.Base;

namespace ClickBuy_Api.DTOs.Views
{
    public class UserView : BaseView
    {
        public String? Email { get; set; }
        public String? Username { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? FullName => $"{FirstName} {LastName}";
        public List<String>? Roles { get; set; }
        public String? Phone { get; set; }
        public String? Address { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? TinhId { get; set; }
        public Guid? HuyenId { get; set; }
        public Guid? XaId { get; set; }
        public Guid? GenderId { get; set; }
        public List<RoleView>? RolesView { get; set; }
        public String? Token { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Int32? TotalRecords { get; set; }
        public String? IdString { get; set; }
    }
}
