using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.DAL.Entities;

namespace SocialNetwork.DAL.EF.Configurations
{
    public class PublicationConfiguration : IEntityTypeConfiguration<Publication>
    {
        public void Configure(EntityTypeBuilder<Publication> builder)
        {

            builder
                .HasMany(p => p.Comments)
                .WithOne(c => c.Publication)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(p => p.Likes)
                .WithOne(l => l.Publication)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
