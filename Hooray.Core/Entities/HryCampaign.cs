using Hooray.Infrastructure.DBContexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hooray.Core.Entities
{
    public partial class HryCampaign
    {
        [JsonProperty("campaign_id")]
        public string CampaignId { get; set; }
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }
        [JsonProperty("is_active")]
        public byte? IsActive { get; set; }
        [JsonProperty("campaign_name")]
        public string CampaignName { get; set; }
        [JsonProperty("campaign_shortcode")]
        public string CampaignShortcode { get; set; }
        [JsonProperty("campaign_sms_name")]
        public string CampaignSmsName { get; set; }
        [JsonProperty("campaign_desc")]
        public string CampaignDesc { get; set; }
        [JsonProperty("campaign_short_desc")]
        public string CampaignShortDesc { get; set; }
        [JsonProperty("campaign_result_desc")]
        public string CampaignResultDesc { get; set; }
        [JsonProperty("campaign_result_announce")]
        public string CampaignResultAnnounce { get; set; }
        [JsonProperty("campaign_type")]
        public int? CampaignType { get; set; }
        [JsonProperty("campaign_join_type")]
        public int? CampaignJoinType { get; set; }
        [JsonProperty("campaign_join_pin")]
        public string CampaignJoinPin { get; set; }
        [JsonProperty("campaign_display_distance")]
        public int? CampaignDisplayDistance { get; set; }
        [JsonProperty("campaign_pinned")]
        public byte? CampaignPinned { get; set; }
        [JsonProperty("sms_number")]
        public string SmsNumber { get; set; }
        [JsonProperty("sms_format_regex")]
        public string SmsFormatRegex { get; set; }
        [JsonProperty("result_process_type")]
        public int? ResultProcessType { get; set; }
        [JsonProperty("announcement_desc")]
        public string AnnouncementDesc { get; set; }
        [JsonProperty("prize_accept_type")]
        public int? PrizeAcceptType { get; set; }
        [JsonProperty("prize_accept_information")]
        public string PrizeAcceptInformation { get; set; }
        [JsonProperty("display_start_date")]
        public DateTime? DisplayStartDate { get; set; }
        [JsonProperty("display_end_date")]
        public DateTime? DisplayEndDate { get; set; }
        [JsonProperty("join_start_date")]
        public DateTime? JoinStartDate { get; set; }
        [JsonProperty("join_end_date")]
        public DateTime? JoinEndDate { get; set; }
        [JsonProperty("result_process_date")]
        public DateTime? ResultProcessDate { get; set; }
        [JsonProperty("result_display_end_date")]
        public DateTime? ResultDisplayEndDate { get; set; }
        [JsonProperty("result_check_end_date")]
        public DateTime? ResultCheckEndDate { get; set; }
        [JsonProperty("stat_like_count")]
        public int? StatLikeCount { get; set; }
        [JsonProperty("stat_like_fanpage_count")]
        public int? StatLikeFanpageCount { get; set; }
        [JsonProperty("stat_join_count")]
        public int? StatJoinCount { get; set; }
        [JsonProperty("stat_win_count")]
        public int? StatWinCount { get; set; }
        [JsonProperty("is_follow")]
        public byte? IsFollow { get; set; }
        [JsonProperty("is_question")]
        public byte? IsQuestion { get; set; }
        [JsonProperty("pushon_announce")]
        public byte? PushOnAnnounce { get; set; }
        [JsonProperty("pushon_open_campaign")]
        public byte? PushOnOpenCampaign { get; set; }
        [JsonProperty("require_prize_otp")]
        public byte? RequirePrizeOtp { get; set; }
        [JsonProperty("require_address")]
        public byte? RequireAddress { get; set; }
        [JsonProperty("require_address_after_win")]
        public byte? RequireAddressAfterWin { get; set; }
        [JsonProperty("is_share_reward")]
        public byte? IsShareReward { get; set; }
        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }
        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }
        [JsonProperty("result_digit")]
        public int? ResultDigit { get; set; }
        [NotMapped]
        public ICollection<HryMedia> Photos { get; set; }
        [NotMapped]
        public HryCompany Company { get; set; }
    }
}
