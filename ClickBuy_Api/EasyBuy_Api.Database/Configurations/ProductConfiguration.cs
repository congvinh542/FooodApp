using ClickBuy_Api.Database.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBuy_Api.Database.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 


            #endregion Custom
        }
    }
}