using Hooray.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooray.Infrastructure.DBContexts.EntitieMappers
{
    public class HryUserFollowMapper : IEntityTypeConfiguration<HryUserFollow>
    {
        public void Configure(EntityTypeBuilder<HryUserFollow> builder)
        {
            builder.ToTable("hry_user_follow");

            builder.HasKey(e => e.CampaignUserFollowId)
                 .HasName("PRIMARY");

            builder.HasIndex(e => e.CampaignUserFollowId)
                .HasName("campaign_user_follow_id");

            builder.Property(e => e.CampaignUserFollowId)
               .HasColumnName("campaign_user_follow_id")
               .HasColumnType("int(11)");

            builder.Property(e => e.CompanyId)
                   .HasColumnName("company_id")
                   .HasColumnType("int(11)");

            //builder.Property(e => e.UserId)
            //      .HasColumnName("user_id")
            //      .HasColumnType("int(11)");

            builder.Property(e => e.UserId)
                  .HasColumnName("user_id")
                  .HasMaxLength(250)
                  .IsUnicode(false);

            builder.Property(e => e.IsNew)
                   .HasColumnName("is_new")
                   .HasColumnType("tinyint(4)")
                   .HasDefaultValueSql("'0'")
                   .HasComment("0=ยังไม่ตรวจ,1=ตรวจแล้ว");

            builder.Property(e => e.FollowDate).HasColumnName("follow_date");

            builder.Property(e => e.UnFollowDate).HasColumnName("unfollow_date");
        }
    }
}
