using ClickBuy_Api.Database.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBuy_Api.Database.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 
            builder.HasOne(x => x.Images).WithMany(x => x.Categorys).HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.Cascade);
            #endregion Custom
        }
    }
}
