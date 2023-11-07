using ClickBuy_Api.Database.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBuy_Api.Database.Configurations
{
    public class TinhConfiguration : IEntityTypeConfiguration<Tinh>
    {
        public void Configure(EntityTypeBuilder<Tinh> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 
            #endregion Custom
        }
    }
}