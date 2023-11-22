using ClickBuy_Api.Database.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBuy_Api.Database.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 

            #endregion Custom
        }
    }
}