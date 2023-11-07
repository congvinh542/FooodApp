using ClickBuy_Api.Database.Entities.Catalog;
using ClickBuy_Api.Database.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBuy_Api.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // default
            builder.HasKey(x => x.Id);
            #region Custom 
            builder.HasOne(x => x.Image).WithMany(x => x.Users).HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.Cascade);
            #endregion Custom
        }
    }
}
