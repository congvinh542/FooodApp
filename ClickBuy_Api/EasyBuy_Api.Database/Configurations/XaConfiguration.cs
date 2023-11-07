using ClickBuy_Api.Database.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBuy_Api.Database.Configurations
{
    public class XaConfiguration : IEntityTypeConfiguration<Xa>
    {
        public void Configure(EntityTypeBuilder<Xa> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 
            builder.HasOne(x => x.Huyen).WithMany(x => x.Xas).HasForeignKey(x => x.HuyenId).OnDelete(DeleteBehavior.Cascade);
            #endregion Custom
        }
    }
}
