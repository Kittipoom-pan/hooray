using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.Interfaces
{
    
    public interface IMySQLManagerRepository
    {
        List<MessageUI> GetDisplayTextUI(string pLang);

        int CheckLastVerifyCode(string pMobile);

        Register RegisterCampaignApplicationNew(string pFacebookID, string pFirstName, string pLastName, string pDisplayName, string pEmail, string pBirthday, string pGender, string pMobile, string pFFirstName, string pFLastName, string pFDisplayName, string pFEmail, string pFBirthday, string pFGender, string pFMobile, string pRegisterType, string pDeviceToken, string pLang, float pLat, float pLng, string pDeviceID, int isPhoto, string pMobileVerifyCode, string pVersionApp, string pVersionIOS, string pVersionAndroid);
        Register RegisterCampaignApplicationNew2(string pFacebookID, string pFirstName, string pLastName, string pDisplayName, string pEmail, string pBirthday, string pGender, string pMobile, string pFFirstName, string pFLastName, string pFDisplayName, string pFEmail, string pFBirthday, string pFGender, string pFMobile, string pRegisterType, string pDeviceToken, string pLang, float pLat, float pLng, string pDeviceID, int isPhoto, string pMobileVerifyCode, string pVersionApp, string pVersionIOS, string pVersionAndroid);

        List<Sms> GetUserSMS(int pUserID);

        StartupBadge GetStartupBadge(int pUserID);

        bool InsertEventLog(int pUserID, int pCampaignID, string pEventName, string pEventDesc, int pEventType, string pImgName, string pDeviceInfo, float pLat, float pLng, string pDeviceOS, string pShopName);

        bool InsertSmsLog(int pUserID, string pMobile, string pSmsMsg, string pDeviceID, int pSmsType);

        bool SendMessage(string mobileNo, string message, string category, string senderName, out string status, out string messageID, out string taskID);

        DataTable GetLastGenVerifyMobile(string pDeviceID);
        bool VerifyCampaignMobile(int pUserID, string pVerifyCode);

        DataTable UpdateUserVerifyMobile(string pDeviceID, string pVerifyCode);

        UserInfo GetUserInformation2(int pUserID);

        List<CampaignNew> GetAllCampaign(int pUserID, string deviceType);
        bool CheckGroup(string pCampaignID);
        bool CheckWinGroup(int pUserID, string pCampaignID);
        int CheckFollowAndJoin(int pShopID, int pUserID, string pCampaignID);
        DataTable CheckCampaignBeforeJoin(string pCampaignID, int pUserID);
        int GetCampaignDigit(string pCampaignID);

        bool CheckJoinNumberRepeated(string pJoinNumber, string pCampaignID);
        List<Join> InsertNewUserJoin(string pCampaignID, int pUserID, double pEnjoyNumber, float pLat, float pLng, string verify);
        Join InsertNewUserJoinNew(string pCampaignID, int pUserID, double pEnjoyNumber, float pLat, float pLng, string verify);

        CampaignResult CheckResultAward(int pUserID, string pCampaignID);
        bool UpdateCPUserCodeNumber(int pCPUserCodeID, int pCheckAnnounce, double pJoinNumberRate);
        bool UpdateCurrentWin(string pCampaignID, int pCPUserCodeID, int pPrizeID);
        DataTable GetImageResult(string pCampaignID, int pImgResultType);
        bool UpdateResultImage(int pUserID, string pCampaignID, string pResultImg, string pResultImgName);
        int InsertNewUserPrize(string pCampaignID, int pUserID, int pCampaignUserCodeID, int pPrizeID);

        ImagePhoto GetImagePhoto(string pImagePath, string image_url);
        DataTable GetCount(string pCampaignID);
        DataTable GetPerPageCampaignComment(string pCampaignID, int pPage, int pPerPage);

        int GetTotalCampaignComment(string cpid);

        DataTable InsertNewComment(string pCampaignID, int pUserID, string pComment);
        List<Question> GetQuestion(string pCampaignID);

        List<UserPrize> GetAllPrizeList(int pUserID);
         PrizeDetailModel GetPrizeDetail(int pUserPrizeID);
        bool DeleteUserPrize(int pUserID, int pUserPrizeID);

        List<UserResult> GetAllResult(int pUserID, int pJoin, int pAnnouncetype, string deviceType);
        int CountPrizeBadge(int pUserID);
        List<CompanyDetailModel> GetAllFollowCompany(int pFollow, int pUserID);

        List<FollowModel> GetAllCampaignCompanyPage(int pCompanyID, int pUserID, int pPage, int pPerPage);

        int InsertNewNotificationFollowDetail(int pUserID, int pCompanID);

        int GetTotalCompanyFollowCampaign(int pCompanyID, int pUserID);

        List<FollowModel> GetPerPageCompanyFollowCampaign(int pCompanyID, int pUserID, int pPage, int pPerPage);

        CompanyDetail GetCompanyDetail(int pCompanyID, int pUserID);

        bool UpdateUserAddress(int pUserID, string pAddress, string pDistrict, string pAmphor, string pProvince, string pZipcode);

        UserInfoForEditModel UpdateUserInformation(int pUserID, string pFirstName, string pLastName, string pDisplayName, string pEmail, string pBirthday, string pGender, string pMobile, string pFFirstName, string pFLastName, string pFDisplayName, string pFEmail, string pFBirthday, string pFGender, string pFMobile, string pFacebookID, string pImageName, int pIsPhoto);

        bool VerifyDeviceToken(int pUserID, string pDeviceToken);
        bool UpdateUserNotification(int pUserID, int pStatus);
        string GetUniqueKey(int maxSize);
        bool UpdateRePrizeOTP(int pUserID, string pOtp);
        OldUserModel UpdateRestorePrize(int pUserID, string pFacebookID, string pOtpCode);
        int InsertLogPush(string pPushName, string pMobileType, int pCampaignID);
        void createPushFile(string message, string type, int badge, string targetDeviceToken, string deviceType, List<KeyValuePair<string, string>> extendParameter, int round_number);
        bool InsertFeedback(int pUserID, string pFeedback);
        bool CreatePassword(int userId, string passWord);

        List<StoreConfigModel> GetStoreConfig(string pMarket);
        Register GetUser(int pUserID);
        string ComputeSha256Hash(string rawData);
        bool CheckMobile(string moBile);

        Register Login(LoginModel model);
        HryUserProfile GetUserMobile(string mobile);
        bool InsertOtpDetail(int otpnumber, string detail, string mobile, out int number);
        bool CheckDateOtp( string mobile);
        bool CheckOtp(VerifyPasswordModel model);
        bool VerifyForgotPassword(VerifyPasswordModel model);

        int InsertLogReceiveDataNew(string pServiceName, string pReceiveData, string pTimeStamp);

        bool InsertQuestionResult(int pUserID, int pQuestionID, int pCampaignUserCodeID, string pAnswer01, string pAnswer02, string pAnswer03, string pAnswer04, string pAnswer05, string pAnswer06, string pAnswer07, string pAnswer08, string pAnswer09, string pAnswer10, string pAnswerOption);
    }
}
