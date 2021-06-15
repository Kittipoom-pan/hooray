using Hooray.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooray.Infrastructure.DBContexts.EntitieMappers
{
    public class ClientDealRedeemLogMapper : IEntityTypeConfiguration<ClientDealRedeemLog>
    {
        public void Configure(EntityTypeBuilder<ClientDealRedeemLog> builder)
        {
            builder.ToTable("client_deal_redeem_log");

            builder.HasKey(e => e.Id)
             .HasName("PRIMARY");

            builder.HasIndex(e => e.Id)
                .HasName("id");

            builder.Property(e => e.DealId)
               .HasColumnName("deal_id")
               .HasColumnType("int(11)");

            builder.Property(e => e.DisplayName)
               .HasColumnName("display_name")
               .HasMaxLength(1000)
               .IsUnicode(false);

            builder.Property(e => e.UserId)
                  .HasColumnName("user_id")
                  .HasColumnType("int(11)");

            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        }
    }
}

