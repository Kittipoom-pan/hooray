using Hooray.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooray.Infrastructure.DBContexts.EntitieMappers
{
    public class HryCampaignDealMapper : IEntityTypeConfiguration<HryCampaignDeal>
    {
        public void Configure(EntityTypeBuilder<HryCampaignDeal> builder)
        {
            builder.ToTable("hry_campaign_deal");

            builder.HasKey(e => e.DealId)
             .HasName("PRIMARY");

            builder.HasIndex(e => e.DealId)
                .HasName("deal_id");

            builder.Property(e => e.DealId)
               .HasColumnName("deal_id")
               .HasColumnType("int(11)");

            builder.Property(e => e.CampaignId)
               .HasColumnName("campaign_id")
               .HasMaxLength(250)
               .IsUnicode(false);

            builder.Property(e => e.DealOnResultHour)
                  .HasColumnName("deal_on_result_hour")
                  .HasColumnType("int(11)");

            builder.Property(e => e.DealGalleryCount)
                  .HasColumnName("deal_gallery_count")
                  .HasColumnType("int(11)");

            builder.Property(e => e.LimitBuyDeal)
                  .HasColumnName("limit_buy_deal")
                  .HasColumnType("int(11)");

            builder.Property(e => e.UsedBuyDeal)
                  .HasColumnName("used_buy_deal")
                  .HasColumnType("int(11)");

            builder.Property(e => e.DealBuyDetail)
                  .HasColumnName("deal_buy_detail")
                  .HasMaxLength(1000)
                  .IsUnicode(false);

            builder.Property(e => e.DealUsedDetail)
                  .HasColumnName("deal_used_detail")
                  .HasMaxLength(1000)
                  .IsUnicode(false);

            builder.Property(e => e.CreateDate).HasColumnName("create_date");

            builder.Property(e => e.DealOnJoinExpireDate).HasColumnName("deal_on_join_expire_date");
        }
    }
}
