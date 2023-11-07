using ClickBuy_Api.Database.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBuy_Api.Database.Configurations
{
    public class HuyenConfiguration : IEntityTypeConfiguration<Huyen>
    {
        public void Configure(EntityTypeBuilder<Huyen> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 
            builder.HasOne(x => x.Tinh).WithMany(x => x.Huyens).HasForeignKey(x => x.TinhId).OnDelete(DeleteBehavior.Cascade);
            #endregion Custom
        }
    }
}
