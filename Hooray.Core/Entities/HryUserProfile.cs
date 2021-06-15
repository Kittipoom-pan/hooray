using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryUserProfile
    {
        public int UserId { get; set; }
        public string DisplayFname { get; set; }
        //public string Password { get; set; }
        public string DisplayLname { get; set; }
        public string DisplayShowName { get; set; }
        public string DisplayGender { get; set; }
        public string DisplayMobile { get; set; }
        public DateTime? DisplayBirthday { get; set; }
        public string DisplayEmail { get; set; }
        public string KeyinFname { get; set; }
        public string KeyinLname { get; set; }
        public string KeyinShowName { get; set; }
        public string KeyinGender { get; set; }
        public string KeyinMobile { get; set; }
        public DateTime? KeyinBirthday { get; set; }
        public string KeyinEmail { get; set; }
        public string FbFname { get; set; }
        public string FbLname { get; set; }
        public string FbShowName { get; set; }
        public string FbGender { get; set; }
        public string FbMobile { get; set; }
        public DateTime? FbBirthday { get; set; }
        public string FbEmail { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Amphor { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public byte? AddressCompleted { get; set; }
        public string ImageNameProfile { get; set; }
        public int? FeyverId { get; set; }
        public string FacebookId { get; set; }
        public string RegisterType { get; set; }
        public short? RequireMobileVerify { get; set; }
        public string MobileVerifyCode { get; set; }
        public short? MobileVerifyFlag { get; set; }
        public DateTime? MobileVerifyDate { get; set; }
        public DateTime? LastMobileVerifyCodeGenDate { get; set; }
        public string FriendMobileList { get; set; }
        public string FriendFbList { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string TokenId { get; set; }
        public string OldTokenId { get; set; }
        public DateTime? TokenChangeDate { get; set; }
        public int? IsOnline { get; set; }
        public byte? IsAcceptTerm { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string UserLang { get; set; }
        public byte? IsAutoAdd { get; set; }
        public byte? IsSupport { get; set; }
        public byte? IsSearchFriend { get; set; }
        public int? TabMenu { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string OtpCode { get; set; }
        public DateTime? OtpExpireDate { get; set; }
        public short? IsNotification { get; set; }
        public byte? HasChangeMobile { get; set; }
        public int? NewUniqueMobile { get; set; }
        public DateTime? CreateDate { get; set; }
        public string RegisterProvince { get; set; }
        public string VersionApp { get; set; }
        public string VersionAndroid { get; set; }
        public string VersionIos { get; set; }
        public string LineUserId { get; set; }
    }
}
