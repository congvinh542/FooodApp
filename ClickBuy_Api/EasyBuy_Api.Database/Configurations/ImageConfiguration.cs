using ClickBuy_Api.Database.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyBuy_Api.Database.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Images>
    {
        public void Configure(EntityTypeBuilder<Images> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 

            #endregion Custom
        }
    }
}
