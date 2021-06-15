
namespace Hooray.Core.AppsettingModels
{
    public class AppSettingsModel
    {
        public Logging Logging { get; set; }
        public LineMessages LineMessages { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }

        public string AllowedHosts { get; set; }

        public UrlDownloadApp UrlDownloadApp { get; set; }
    }
    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }

        public string Warning { get; set; }

        public string Error { get; set; }
    }

    public class LineMessages
    {
        public string ChannelAccessToken { get; set; }

        public MessageBody MessageBody { get; set; }
    }
    public class MessageBody
    {
        public string Type { get; set; }

        public string Text { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get;}
    }
    public class Resource
    {
        public string ApiKey { get; set; }
        public string Environment { get; set; }
        public string BaseUrl { get; set; }
        public string CompanyLogoPath { get; set; }
        public string CompanyLogoPathVideo { get; set; }
        public string YoutubeVideoURL { get; set; }
        public string CampaignVideoPath { get; set; }
        public string CampaignPhotoPath { get; set; }
        public string CampaignGalleryPath { get; set; }
        public string ScratchpadPhotoPath { get; set; }
        public string CommentPhotoPath { get; set; }
        public string DefaultLanguage { get; set; }
        public string CampaignBackgroundPath { get; set; }
        public string DealPhotoPath { get; set; }
        public string PerPageShop { get; set; }
        public string Url { get; set; }
        public string AccountSMS { get; set; }
        public string PasswordSMS { get; set; }
        public string PATH_PUSH_FILE { get; set; }
        public Photo Photo { get; set; }
    }
    public class Photo
    {
        public string UserPhotoPath { get; set; }
        public string SmsPhotoPath { get; set; }

    }
    public class UrlDownloadApp
    {
        public string UrlAndroid { get; set; }
        public string UrlIos { get; set; }

    }
}
