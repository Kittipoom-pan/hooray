using Hooray.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooray.Infrastructure.EntitieMappers
{
    public class LineChannelAccessMapper : IEntityTypeConfiguration<LineChannelAccess>
    {
        public void Configure(EntityTypeBuilder<LineChannelAccess> builder)
        {
            builder.ToTable("line_channel_access");

            builder.HasKey(e => e.Id)
                 .HasName("PRIMARY");

            builder.Property(e => e.CompanyId)
               .HasColumnName("company_id")
               .HasColumnType("int(11)");

            builder.Property(e => e.ChannelAccessToken).HasColumnName("channel_access_token");

            builder.Property(e => e.Environment).HasColumnName("environment");

            builder.Property(e => e.CreateDate).HasColumnName("create_date");

            builder.Property(e => e.UpdateDate).HasColumnName("update_date");

        }
    }
}
