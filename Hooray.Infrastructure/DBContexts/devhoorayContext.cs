using Hooray.Core.Entities;
using Hooray.Infrastructure.DBContexts.EntitieMappers;
using Hooray.Infrastructure.EntitieMappers;
using Microsoft.EntityFrameworkCore;

namespace Hooray.Infrastructure.DBContexts
{
    public partial class devhoorayContext : DbContext
    {
        public devhoorayContext()
        {
        }

        public devhoorayContext(DbContextOptions<devhoorayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HryApplicationAction> HryApplicationAction { get; set; }
        public virtual DbSet<HryCampaign> HryCampaign { get; set; }
        public virtual DbSet<HryCampaignAction> HryCampaignAction { get; set; }
        public virtual DbSet<HryCampaignPrize> HryCampaignPrize { get; set; }
        public virtual DbSet<HryCampaignPrizeGroup> HryCampaignPrizeGroup { get; set; }
        public virtual DbSet<HryCampaignRule> HryCampaignRule { get; set; }
        public virtual DbSet<HryCampaignTag> HryCampaignTag { get; set; }
        public virtual DbSet<HryCompany> HryCompany { get; set; }
        public virtual DbSet<HryCompanyBranch> HryCompanyBranch { get; set; }
        public virtual DbSet<HryMedia> HryMedia { get; set; }
        public virtual DbSet<HryTag> HryTag { get; set; }
        public virtual DbSet<HryTagTopic> HryTagTopic { get; set; }
        public virtual DbSet<HryUserJoin> HryUserJoin { get; set; }
        public virtual DbSet<HryUserPrize> HryUserPrize { get; set; }
        public virtual DbSet<HryUserProfile> HryUserProfile { get; set; }

        public virtual DbSet<SystemMessage> SystemMessage { get; set; }

        public virtual DbSet<LineChannelAccess> LineChannelAccess { get; set; }
        public virtual DbSet<HryUserFollow> HryUserFollow { get; set; }
        public virtual DbSet<HryCampaignDeal> HryCampaignDeal { get; set; }
        public virtual DbSet<ClientDealRedeemLog> ClientDealRedeemLog { get; set; }
        public virtual DbSet<HryOtpDetail> HryOtpDetail { get; set; }
        //public virtual DbSet<HryUserCode> HryUserCode { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LineChannelAccessMapper());

            modelBuilder.ApplyConfiguration(new HryUserFollowMapper());

            modelBuilder.ApplyConfiguration(new HryCampaignDealMapper());

            modelBuilder.ApplyConfiguration(new ClientDealRedeemLogMapper());

            modelBuilder.Entity<HryApplicationAction>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PRIMARY");

                entity.ToTable("hry_application_action");

                entity.HasIndex(e => e.CampaignId)
                    .HasName("campaign_id");

                entity.HasIndex(e => e.EventId)
                    .HasName("event_id");

                entity.HasIndex(e => e.EventName)
                    .HasName("event_name");

                entity.HasIndex(e => e.EventType)
                    .HasName("event_type");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.HasIndex(e => new { e.EventType, e.UserId })
                    .HasName("idx_lat_lng");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .HasColumnType("int(11)");

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.CampaignId)
                  .HasColumnName("campaign_id")
                  .HasMaxLength(250)
                  .IsUnicode(false);
                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.DeviceInfo).HasColumnName("device_info");

                entity.Property(e => e.DeviceOs)
                    .HasColumnName("device_os")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventDescription)
                    .HasColumnName("event_description")
                    .HasColumnType("longtext");

                entity.Property(e => e.EventName)
                    .HasColumnName("event_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type")
                    .HasColumnType("int(11)")
                    .HasComment("1=open app, 2= close app, 3= ,4= share campaign, 5= share result(sub reward), 6= ClickLinkFacebook, 7= feed detail, 8= shop detail, 9= signage,10= share result(no sub reward), 12=prize transfer, 13=prize transfer fb, 14=StockDeal");

                entity.Property(e => e.ImageName)
                    .HasColumnName("image_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("float(12,8)")
                    .HasDefaultValueSql("'0.00000000'");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("float(12,8)")
                    .HasDefaultValueSql("'0.00000000'");

                entity.Property(e => e.ShopName)
                    .HasColumnName("shop_name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                //entity.Property(e => e.UserId)
                //    .HasColumnName("user_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HryCampaign>(entity =>
            {
                entity.HasMany<HryMedia>(q => q.Photos)
                    .WithOne(c => c.HryCampaign)
                    .HasForeignKey(q => q.ObjectId)
                ;

                //entity.HasMany<HryMedia>(q => q.PhotoCampaign)
                //  .WithOne(c => c.HryCampaign)
                //  .HasForeignKey(q => q.ObjectId)
                //  ;

                entity.HasKey(e => e.CampaignId)
                    .HasName("PRIMARY");

                entity.ToTable("hry_campaign");

                entity.HasIndex(e => e.CampaignId)
                    .HasName("campaign_id");

                entity.HasIndex(e => e.CompanyId)
                    .HasName("fk_shop_info_shop_type");

                entity.HasIndex(e => e.DisplayStartDate)
                    .HasName("idx_showing_start_date");

                entity.HasIndex(e => e.JoinEndDate)
                    .HasName("join_end_date");

                entity.HasIndex(e => e.ResultProcessDate)
                    .HasName("announce_date");

                entity.HasIndex(e => new { e.CampaignId, e.ResultCheckEndDate })
                    .HasName("idx_statusRewardExpire");

                entity.HasIndex(e => new { e.CampaignId, e.ResultProcessDate })
                    .HasName("idx_statusAnnounce");

                entity.HasIndex(e => new { e.CompanyId, e.DisplayStartDate })
                    .HasName("idx_get_all_cp_shop");

                entity.HasIndex(e => new { e.CampaignType, e.DisplayStartDate, e.DisplayEndDate })
                    .HasName("idx_get_all_campaign2");

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.CampaignId)
                   .HasColumnName("campaign_id")
                   .HasMaxLength(250)
                   .IsUnicode(false);
                entity.Property(e => e.AnnouncementDesc).HasColumnName("announcement_desc");

                entity.Property(e => e.CampaignDesc)
                    .HasColumnName("campaign_desc")
                    .HasColumnType("longtext")
                    .HasComment("avoid problem on load datatable constraint @getprizelist2");

                entity.Property(e => e.CampaignDisplayDistance)
                    .HasColumnName("campaign_display_distance")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("Unit of Meter");

                entity.Property(e => e.CampaignJoinPin)
                    .HasColumnName("campaign_join_pin")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignJoinType)
                    .HasColumnName("campaign_join_type")
                    .HasColumnType("int(11)")
                    .HasComment("action require for joining - 0 = no action require, 1 = pin required ");

                entity.Property(e => e.CampaignName)
                    .HasColumnName("campaign_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignPinned)
                    .HasColumnName("campaign_pinned")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=ไม่ปักหมุด, 1=ปักหมุด");

                entity.Property(e => e.CampaignResultAnnounce)
                    .HasColumnName("campaign_result_announce")
                    .HasColumnType("longtext");

                entity.Property(e => e.CampaignResultDesc)
                    .HasColumnName("campaign_result_desc")
                    .HasColumnType("longtext");

                entity.Property(e => e.CampaignShortDesc).HasColumnName("campaign_short_desc");

                entity.Property(e => e.CampaignShortcode)
                    .HasColumnName("campaign_shortcode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignSmsName)
                    .HasColumnName("campaign_sms_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignType)
                    .HasColumnName("campaign_type")
                    .HasColumnType("int(6)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("1=news(ข่าวสาร),2=campaign(ชิงโชค),3=deal,4=news(รูป)");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DisplayEndDate).HasColumnName("display_end_date");

                entity.Property(e => e.DisplayStartDate).HasColumnName("display_start_date");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("0 = ไม่ใช้แล้ว, 1 = ใช้งานอยู่");

                entity.Property(e => e.IsFollow)
                    .HasColumnName("is_follow")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("0=ไม่ต้อง follow, 1=ต้อง follow");

                entity.Property(e => e.IsQuestion)
                    .HasColumnName("is_question")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=ไม่มีคำถาม, 1=มีคำถาม");

                entity.Property(e => e.IsShareReward)
                    .HasColumnName("is_share_reward")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=ไม่มีรางวัลตอน share, 1=มีรางวัลตอน share");

                entity.Property(e => e.JoinEndDate).HasColumnName("join_end_date");

                entity.Property(e => e.JoinStartDate).HasColumnName("join_start_date");

                entity.Property(e => e.PrizeAcceptInformation)
                    .HasColumnName("prize_accept_information")
                    .HasComment("ข้อความที่จะมาแสดงใต้ปุ่มกดรับสิทธิ์ เช่่น \"ต้องกดต่อหน้าพนักงานเท่านั้น รหัสรับรางวัลมีอายุ 10 นาที\"");

                entity.Property(e => e.PrizeAcceptType)
                    .HasColumnName("prize_accept_type")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("(1=มีปุ่มกดรับสิทธิ์ ในหน้า prize box, 2=ไม่ต้องกดรับสิทธิ์ Feyverly ส่งของให้เลย)");

                entity.Property(e => e.PushOnAnnounce)
                    .HasColumnName("push_on_announce")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("0=ไม่มี push บอกออกรางวัล, 1=มี push บอกออกรางวัล");

                entity.Property(e => e.PushOnOpenCampaign)
                    .HasColumnName("push_on_open_campaign")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.RequireAddress)
                    .HasColumnName("require_address")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=ไม่ต้องใส่ที่อยู่, 1=ต้องใส่ที่อยู่ด้วย");

                entity.Property(e => e.RequireAddressAfterWin)
                    .HasColumnName("require_address_after_win")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RequirePrizeOtp)
                    .HasColumnName("require_prize_otp")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=ไม่ต้อง verify รับของรางวัล, 1=ต้อง verify รับของรางวัล");

                entity.Property(e => e.ResultCheckEndDate).HasColumnName("result_check_end_date");

                entity.Property(e => e.ResultDisplayEndDate).HasColumnName("result_display_end_date");

                entity.Property(e => e.ResultProcessDate).HasColumnName("result_process_date");

                entity.Property(e => e.ResultProcessType)
                    .HasColumnName("result_process_type")
                    .HasColumnType("int(6)")
                    .HasDefaultValueSql("'2'")
                    .HasComment("1=สุ่มรหัสให้,2=รอประกาศผล,3=กำหนดตำแหน่ง,4=สุ่มรางวัลได้ทุกคน");

                entity.Property(e => e.SmsFormatRegex)
                    .HasColumnName("sms_format_regex")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SmsNumber)
                    .HasColumnName("sms_number")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StatJoinCount)
                    .HasColumnName("stat_join_count")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("จำนวนผู้เข้าร่วมปัจจุบัน");

                entity.Property(e => e.StatLikeCount)
                    .HasColumnName("stat_like_count")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StatLikeFanpageCount)
                    .HasColumnName("stat_like_fanpage_count")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StatWinCount)
                    .HasColumnName("stat_win_count")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ResultDigit)
                     .HasColumnName("result_digit")
                     .HasColumnType("int(11)")
                     .HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<HryCampaignAction>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PRIMARY");

                entity.ToTable("hry_campaign_action");

                entity.HasIndex(e => e.CampaignId)
                    .HasName("campaign_id");

                entity.HasIndex(e => e.EventId)
                    .HasName("event_id");

                entity.HasIndex(e => e.EventName)
                    .HasName("event_name");

                entity.HasIndex(e => e.EventType)
                    .HasName("event_type");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.HasIndex(e => new { e.EventType, e.UserId })
                    .HasName("idx_lat_lng");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .HasColumnType("int(11)");

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.CampaignId)
                    .HasColumnName("campaign_id")
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.DeviceInfo).HasColumnName("device_info");

                entity.Property(e => e.DeviceOs)
                    .HasColumnName("device_os")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventDescription)
                    .HasColumnName("event_description")
                    .HasColumnType("longtext");

                entity.Property(e => e.EventName)
                    .HasColumnName("event_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type")
                    .HasColumnType("int(11)")
                    .HasComment("1=open app, 2= close app, 3= ,4= share campaign, 5= share result(sub reward), 6= ClickLinkFacebook, 7= feed detail, 8= shop detail, 9= signage,10= share result(no sub reward), 12=prize transfer, 13=prize transfer fb, 14=StockDeal");

                entity.Property(e => e.ImageName)
                    .HasColumnName("image_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("float(12,8)")
                    .HasDefaultValueSql("'0.00000000'");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("float(12,8)")
                    .HasDefaultValueSql("'0.00000000'");

                entity.Property(e => e.ShopName)
                    .HasColumnName("shop_name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<HryCampaignPrize>(entity =>
            {
                entity.HasKey(e => e.PrizeId)
                    .HasName("PRIMARY");

                entity.ToTable("hry_campaign_prize");

                entity.HasIndex(e => e.CampaignId)
                    .HasName("fk_shop_info_shop_type");

                entity.HasIndex(e => e.PrizeId)
                    .HasName("prize_id");

                entity.Property(e => e.PrizeId)
                    .HasColumnName("prize_id")
                    .HasColumnType("int(11)");

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'0'");
                entity.Property(e => e.CampaignId)
                  .HasColumnName("campaign_id")
                  .HasMaxLength(250)
                  .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.IsSubReward)
                    .HasColumnName("is_sub_reward")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LimitQtyByPercent)
                    .HasColumnName("limit_qty_by_percent")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=จำนวน,1=percent");

                entity.Property(e => e.MasterRewardNo)
                    .HasColumnName("master_reward_no")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PrizeAcceptCondition)
                    .HasColumnName("prize_accept_condition")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("ข้อความที่จะมาแสดงใต้ปุ่มกดรับสิทธิ์ เช่่น \"ต้องกดต่อหน้าพนักงานเท่านั้น รหัสรับรางวัลมีอายุ 10 นาที\"");

                entity.Property(e => e.PrizeAcceptExpireDate).HasColumnName("prize_accept_expire_date");

                entity.Property(e => e.PrizeAcceptExpireDay)
                    .HasColumnName("prize_accept_expire_day")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("จำนวนวันที่จะบอกว่าต้องรับรางวัลภายในกี่วัน นับจากได้รางวัล");

                entity.Property(e => e.PrizeAcceptExpireType)
                    .HasColumnName("prize_accept_expire_type")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("1=prize_accept_expire_day, 2=prize_accept_expire_date");

                entity.Property(e => e.PrizeAcceptType)
                    .HasColumnName("prize_accept_type")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("(1=มีปุ่มกดรับสิทธิ์ ในหน้า prize box, 2=ไม่ต้องกดรับสิทธิ์ Feyverly ส่งของให้เลย)");

                entity.Property(e => e.PrizeAmount)
                    .HasColumnName("prize_amount")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PrizeDetail).HasColumnName("prize_detail");

                entity.Property(e => e.PrizeDigit)
                    .HasColumnName("prize_digit")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'6'");

                entity.Property(e => e.PrizeName)
                    .HasColumnName("prize_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrizeOrder)
                    .HasColumnName("prize_order")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PrizeQty)
                    .HasColumnName("prize_qty")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PrizeQtyUsage)
                    .HasColumnName("prize_qty_usage")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PrizeReceiveType)
                    .HasColumnName("prize_receive_type")
                    .HasColumnType("int(11)")
                    .HasComment("1=กดรับรหัสที่ร้าน, 2=ของรางวัลจะจัดส่งถึงคุณตามที่อยู่ที่คุณระบุ, 3=เชิญมารับของรางวัลที่.......ภายในวันที่......");

                entity.Property(e => e.PrizeRedeemCodeAge)
                    .HasColumnName("prize_redeem_code_age")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PrizeRedeemCodeDisplayType)
                    .HasColumnName("prize_redeem_code_display_type")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=แสดง code ปกติ, 1=barcode");

                entity.Property(e => e.PrizeRemarkCaption)
                    .HasColumnName("prize_remark_caption")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ShareResultFbMessageLose)
                    .HasColumnName("share_result_fb_message_lose")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ShareResultFbMessageWin)
                    .HasColumnName("share_result_fb_message_win")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateAnnounceDate).HasColumnName("update_announce_date");

                entity.Property(e => e.UpdateDate).HasColumnName("update_date");

                entity.Property(e => e.WinRate).HasColumnName("win_rate");
            });

            modelBuilder.Entity<HryCampaignPrizeGroup>(entity =>
            {
                entity.ToTable("hry_campaign_prize_group");

                entity.HasIndex(e => e.Id)
                    .HasName("prize_id");

                entity.HasIndex(e => e.PrizeId)
                    .HasName("fk_shop_info_shop_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GroupDesc).HasColumnName("group_desc");

                entity.Property(e => e.GroupId)
                    .HasColumnName("group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GroupName)
                    .HasColumnName("group_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PrizeId)
                    .HasColumnName("prize_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<HryCampaignRule>(entity =>
            {
                entity.ToTable("hry_campaign_rule");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.CampaignId)
                   .HasColumnName("campaign_id")
                   .HasMaxLength(250)
                   .IsUnicode(false);

                entity.Property(e => e.CampaignQuota)
                    .HasColumnName("campaign_quota")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.PeriodEnd).HasColumnName("period_end");

                entity.Property(e => e.PeriodStart).HasColumnName("period_start");

                entity.Property(e => e.RuleType)
                    .HasColumnName("rule_type")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("join, prize");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserQuota)
                    .HasColumnName("user_quota")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<HryCampaignTag>(entity =>
            {
                entity.ToTable("hry_campaign_tag");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedById)
                    .HasColumnName("created_by_id")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DealId)
                    .HasColumnName("deal_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TagId)
                    .HasColumnName("tag_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<HryCompany>(entity =>
            {
                entity.HasOne(q => q.HryCampaign)
                  .WithOne(c => c.Company)
                  .HasForeignKey<HryCampaign>(f => f.CompanyId)
               ;

              //  entity.HasMany<HryMedia>(q => q.Photos)
              //    .WithOne(c => c.HryCompany)
              //    .HasForeignKey(q => q.ObjectIdInt)
              //;

                entity.HasKey(e => e.CompanyId)
                    .HasName("PRIMARY");

                entity.ToTable("hry_company");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.ApprovalRequired)
                    .HasColumnName("approval_required")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0 = ไม่ต้อง approve, 1 =ต้อง approv");

                entity.Property(e => e.CompanyAddress).HasColumnName("company_address");

                entity.Property(e => e.CompanyContactInfo).HasColumnName("company_contact_info");

                entity.Property(e => e.CompanyInformation).HasColumnName("company_information");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("company_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ContactEmail)
                    .HasColumnName("contact_email")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ContactInfo)
                    .HasColumnName("contact_info")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'0'")
                    .HasComment("contact info of contact person ex. mobile");

                entity.Property(e => e.ContactPerson)
                    .HasColumnName("contact_person")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LoginEmail)
                    .HasColumnName("login_email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LoginPassword)
                    .HasColumnName("login_password")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HryCompanyBranch>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PRIMARY");

                entity.ToTable("hry_company_branch");

                entity.Property(e => e.BranchId)
                    .HasColumnName("branch_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BranchAddress).HasColumnName("branch_address");

                entity.Property(e => e.BranchDesc)
                    .HasColumnName("branch_desc")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BranchDisplayname)
                    .HasColumnName("branch_displayname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BranchName)
                    .HasColumnName("branch_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BranchOrder)
                    .HasColumnName("branch_order")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'0'");
                entity.Property(e => e.CampaignId)
                   .HasColumnName("campaign_id")
                   .HasMaxLength(250)
                   .IsUnicode(false);
                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("float(12,8)")
                    .HasDefaultValueSql("'0.00000000'");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("float(12,8)")
                    .HasDefaultValueSql("'0.00000000'");
            });

            modelBuilder.Entity<HryMedia>(entity =>
            {
                entity.ToTable("hry_media");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20)");

                //entity.Property(e => e.ObjectIdInt)
                //      .HasColumnName("object_Id")
                //      .HasColumnType("int(10)");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MediaType)
                    .HasColumnName("media_type")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_Id")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Objects)
                    .HasColumnName("objects")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("shop_logo, shop, campaign, campaign_bg ");

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HryTag>(entity =>
            {
                entity.ToTable("hry_tag");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedById)
                    .HasColumnName("created_by_id")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImageHover)
                    .HasColumnName("image_hover")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TagDescription).HasColumnName("tag_description");

                entity.Property(e => e.TagDescriptionCh).HasColumnName("tag_description_ch");

                entity.Property(e => e.TagDescriptionEn).HasColumnName("tag_description_en");

                entity.Property(e => e.TagDescriptionTh).HasColumnName("tag_description_th");

                entity.Property(e => e.TagName)
                    .HasColumnName("tag_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TagNameCh)
                    .HasColumnName("tag_name_ch")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TagNameEn)
                    .HasColumnName("tag_name_en")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TagNameTh)
                    .HasColumnName("tag_name_th")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TagTopicId)
                    .HasColumnName("tag_topic_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TagWeight)
                    .HasColumnName("tag_weight")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedById)
                    .HasColumnName("updated_by_id")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<HryTagTopic>(entity =>
            {
                entity.ToTable("hry_tag_topic");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.CampaignId)
                   .HasColumnName("campaign_id")
                   .HasMaxLength(250)
                   .IsUnicode(false);
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedById)
                    .HasColumnName("created_by_id")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TagTopicDescription).HasColumnName("tag_topic_description");

                entity.Property(e => e.TagTopicDescriptionCh).HasColumnName("tag_topic_description_ch");

                entity.Property(e => e.TagTopicDescriptionEn).HasColumnName("tag_topic_description_en");

                entity.Property(e => e.TagTopicDescriptionTh).HasColumnName("tag_topic_description_th");

                entity.Property(e => e.TagTopicName)
                    .IsRequired()
                    .HasColumnName("tag_topic_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TagTopicNameCh)
                    .HasColumnName("tag_topic_name_ch")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TagTopicNameEn)
                    .HasColumnName("tag_topic_name_en")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TagTopicNameTh)
                    .HasColumnName("tag_topic_name_th")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedById)
                    .HasColumnName("updated_by_id")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<HryUserJoin>(entity =>
            {
                entity.ToTable("hry_user_join");

                entity.HasIndex(e => e.CampaignId)
                    .HasName("fk_shop_info_shop_type");

                entity.HasIndex(e => e.CodeType)
                    .HasName("code_type");

                entity.HasIndex(e => e.CreateDate)
                    .HasName("idx_createdate");

                entity.HasIndex(e => e.IsWin)
                    .HasName("is_win");

                entity.HasIndex(e => e.PrizeId)
                    .HasName("prize_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.HasIndex(e => new { e.CampaignId, e.UserId })
                    .HasName("idx_get_all_result");

                entity.HasIndex(e => new { e.UserId, e.CodeType })
                    .HasName("idx_get_all_result_join");

                entity.HasIndex(e => new { e.CampaignId, e.UserId, e.CodeType })
                    .HasName("idx_user_campaign_join");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnswerForJoin)
                    .HasColumnName("answer_for_join")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'0'");
                entity.Property(e => e.CampaignId)
                   .HasColumnName("campaign_id")
                   .HasMaxLength(250)
                   .IsUnicode(false);
                entity.Property(e => e.CheckAnnounce)
                    .HasColumnName("check_announce")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=ยังไม่ตรวจ,1=ตรวจแล้ว");

                entity.Property(e => e.CheckAnnounceDate).HasColumnName("check_announce_date");

                entity.Property(e => e.CodeType)
                    .HasColumnName("code_type")
                    .HasColumnType("int(6)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("1=sms,2=join,3=chat");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.HasShared)
                    .HasColumnName("has_shared")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HasWinInCampaign)
                    .HasColumnName("has_win_in_campaign")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsRead)
                    .HasColumnName("is_read")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsVerifyPrize)
                    .HasColumnName("is_verify_prize")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsWin)
                    .HasColumnName("is_win")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=ยังไม่ตรวจ,1=ถูกรางวัล,2=ไม่ถูกรางวัล");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("float(10,6)")
                    .HasDefaultValueSql("'0.000000'");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("float(10,6)")
                    .HasDefaultValueSql("'0.000000'");

                entity.Property(e => e.PrizeId)
                    .HasColumnName("prize_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RawRandomDate).HasColumnName("raw_random_date");

                entity.Property(e => e.RawRandomNumber)
                    .HasColumnName("raw_random_number")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ReceiveDate).HasColumnName("receive_date");

                entity.Property(e => e.RegisterFacebookId)
                    .HasColumnName("register_facebook_id")
                    .HasColumnType("int(11)")
                    .HasComment("ไม่ใช้แล้ว");

                entity.Property(e => e.RegisterFeyverId)
                    .HasColumnName("register_feyver_id")
                    .HasColumnType("int(11)")
                    .HasComment("ไม่ใช้แล้ว");

                entity.Property(e => e.ResultImage)
                    .HasColumnName("result_image")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("รูปผลรางวัล");

                entity.Property(e => e.ResultImageName)
                    .HasColumnName("result_image_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SmsImage)
                    .HasColumnName("sms_image")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'no-image.jpg'");

                entity.Property(e => e.SmsText).HasColumnName("sms_text");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<HryUserPrize>(entity =>
            {
                entity.HasKey(e => e.UserPrizeId)
                    .HasName("PRIMARY");

                entity.ToTable("hry_user_prize");

                entity.HasIndex(e => e.CampaignId)
                    .HasName("campaign_id");

                entity.HasIndex(e => e.CampaignUserCodeId)
                    .HasName("campaign_user_join_id");

                entity.HasIndex(e => e.PrizeId)
                    .HasName("prize_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.HasIndex(e => e.UserPrizeId)
                    .HasName("user_prize_id");

                entity.HasIndex(e => new { e.UserPrizeId, e.IsCancel })
                    .HasName("idx_get_prize_detail");

                entity.HasIndex(e => new { e.UserPrizeId, e.UpdateDate })
                    .HasName("idx_accept_user_prize");

                entity.Property(e => e.UserPrizeId)
                    .HasColumnName("user_prize_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AcceptDate).HasColumnName("accept_date");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddressPrizeComplete)
                    .HasColumnName("address_prize_complete")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Amphor)
                    .HasColumnName("amphor")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                //entity.Property(e => e.CampaignId)
                //    .HasColumnName("campaign_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.CampaignId)
                    .HasColumnName("campaign_id")
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.CampaignUserCodeId)
                    .HasColumnName("campaign_user_join_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CancelDate).HasColumnName("cancel_date");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExpireDate).HasColumnName("expire_date");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsCancel)
                    .HasColumnName("is_cancel")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PrizeId)
                    .HasColumnName("prize_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PrizeRedeemCode)
                    .HasColumnName("prize_redeem_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrizeRedeemCodeExpire).HasColumnName("prize_redeem_code_expire");

                entity.Property(e => e.PrizeRemark)
                    .HasColumnName("prize_remark")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnName("update_date");

                //entity.Property(e => e.UserId)
                //    .HasColumnName("user_id")
                //    .HasColumnType("int(11)");
                entity.Property(e => e.UserId)
                   .HasColumnName("user_id")
                   .HasMaxLength(250)
                   .IsUnicode(false);
                
       

                entity.Property(e => e.Zipcode)
                    .HasColumnName("zipcode")
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HryUserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                //entity.Property(e => e.LineId)
                //  .HasColumnName("line_id")
                //  .HasColumnType("int(11)");

                entity.ToTable("hry_user_profile");

                entity.HasIndex(e => e.DeviceId)
                    .HasName("device_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.HasIndex(e => new { e.UserId, e.VersionApp, e.VersionAndroid, e.VersionIos })
                    .HasName("update_user_migrate");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");


                entity.Property(e => e.LineUserId)
                     .HasColumnName("line_user_id")
                     .HasMaxLength(255)
                     .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddressCompleted)
                    .HasColumnName("address_completed")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Amphor)
                    .HasColumnName("amphor")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.DeviceId)
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceType)
                    .HasColumnName("device_type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayBirthday)
                    .HasColumnName("display_birthday")
                    .HasColumnType("date");

                entity.Property(e => e.DisplayEmail)
                    .HasColumnName("display_email")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayFname)
                    .HasColumnName("display_fname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayGender)
                    .HasColumnName("display_gender")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayLname)
                    .HasColumnName("display_lname")
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayMobile)
                    .HasColumnName("display_mobile")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayShowName)
                    .HasColumnName("display_show_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FacebookId)
                    .HasColumnName("facebook_id")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FbBirthday)
                    .HasColumnName("fb_birthday")
                    .HasColumnType("date");

                entity.Property(e => e.FbEmail)
                    .HasColumnName("fb_email")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.FbFname)
                    .HasColumnName("fb_fname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FbGender)
                    .HasColumnName("fb_gender")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FbLname)
                    .HasColumnName("fb_lname")
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.FbMobile)
                    .HasColumnName("fb_mobile")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FbShowName)
                    .HasColumnName("fb_show_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FeyverId)
                    .HasColumnName("feyver_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FriendFbList)
                    .HasColumnName("friend_fb_list")
                    .HasColumnType("longtext");

                entity.Property(e => e.FriendMobileList)
                    .HasColumnName("friend_mobile_list")
                    .HasColumnType("longtext");

                entity.Property(e => e.HasChangeMobile)
                    .HasColumnName("has_change_mobile")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ImageNameProfile)
                    .HasColumnName("image_name_profile")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsAcceptTerm)
                    .HasColumnName("is_accept_term")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsAutoAdd)
                    .HasColumnName("is_auto_add")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsNotification)
                    .HasColumnName("is_notification")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.IsOnline)
                    .HasColumnName("is_online")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsSearchFriend)
                    .HasColumnName("is_search_friend")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsSupport)
                    .HasColumnName("is_support")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.KeyinBirthday)
                    .HasColumnName("keyin_birthday")
                    .HasColumnType("date");

                entity.Property(e => e.KeyinEmail)
                    .HasColumnName("keyin_email")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.KeyinFname)
                    .HasColumnName("keyin_fname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KeyinGender)
                    .HasColumnName("keyin_gender")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.KeyinLname)
                    .HasColumnName("keyin_lname")
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.KeyinMobile)
                    .HasColumnName("keyin_mobile")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.KeyinShowName)
                    .HasColumnName("keyin_show_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginDate).HasColumnName("last_login_date");

                entity.Property(e => e.LastMobileVerifyCodeGenDate).HasColumnName("last_mobile_verify_code_gen_date");

                entity.Property(e => e.LastUpdateDate).HasColumnName("last_update_date");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("float(10,6)")
                    .HasDefaultValueSql("'0.000000'");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("float(10,6)")
                    .HasDefaultValueSql("'0.000000'");

                entity.Property(e => e.MobileVerifyCode)
                    .HasColumnName("mobile_verify_code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileVerifyDate).HasColumnName("mobile_verify_date");

                entity.Property(e => e.MobileVerifyFlag)
                    .HasColumnName("mobile_verify_flag")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.NewUniqueMobile)
                    .HasColumnName("new_unique_mobile")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OldTokenId)
                    .HasColumnName("old_token_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OtpCode)
                    .HasColumnName("otp_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OtpExpireDate).HasColumnName("otp_expire_date");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterProvince)
                    .HasColumnName("register_province")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterType)
                    .HasColumnName("register_type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RequireMobileVerify)
                    .HasColumnName("require_mobile_verify")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.TabMenu)
                    .HasColumnName("tab_menu")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'3'")
                    .HasComment("3=3menu, 5=5menu");

                entity.Property(e => e.TokenChangeDate).HasColumnName("token_change_date");

                entity.Property(e => e.TokenId)
                    .HasColumnName("token_id")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserLang)
                    .HasColumnName("user_lang")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VersionAndroid)
                    .HasColumnName("version_android")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VersionApp)
                    .HasColumnName("version_app")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VersionIos)
                    .HasColumnName("version_ios")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Zipcode)
                    .HasColumnName("zipcode")
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SystemMessage>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("system_message");


                entity.HasIndex(e => e.Id)
                    .HasName("id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");


                entity.Property(e => e.MessageCode)
                    .HasColumnName("message_code")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageType)
                   .HasColumnName("message_type")
                   .HasColumnType("smallint(6)");

                entity.Property(e => e.MessageText)
                    .HasColumnName("message_text")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MessageDesc)
                    .HasColumnName("message_desc")
                    .HasColumnType("text");

                entity.Property(e => e.MessageLang)
                    .HasColumnName("message_lang")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UiNameIos)
                    .HasColumnName("ui_name_ios")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UiNameAndroid)
                    .HasColumnName("ui_name_android")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdateDate)
                   .HasColumnName("last_update_date")
                   .HasColumnType("date");



            });

            modelBuilder.Entity<HryOtpDetail>(entity =>
            {
                entity.ToTable("hry_otp_detail");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");
                
                entity.Property(e => e.OtpNumber)
                    .HasColumnName("otp_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OtpSendDate).HasColumnName("otp_send_date");
                entity.Property(e => e.OtpVerifyDate).HasColumnName("otp_verify_date");

                entity.Property(e => e.OtpDetail)
                    .HasColumnName("otp_detail")
                    .HasMaxLength(255)
                    .IsUnicode(false); 
                
                entity.Property(e => e.MobileNo)
                    .HasColumnName("mobile_no")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OtpVerify)
                    .HasColumnName("otp_verify")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

              
                entity.Property(e => e.OtpType)
                    .HasColumnName("otp_type")
                    .HasColumnType("int(11)");
            });
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
