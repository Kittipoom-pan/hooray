using Hooray.Core.AppsettingModels;
using Hooray.Core.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hooray.Infrastructure.MySQLManager
{
    public class PushManager
    {
        //APNsManager apnManager;
        //PushNotification gcmManager;
        //ArrayList list;

        //#region new push

        ////public void WritePushMessage(string message, string type, UserProfile user, List<KeyValuePair<string, string>> extendParameter)
        ////{
        ////    createPushFile(message, type, 1, user.deviceToken, user.device, extendParameter);
        ////}

       

        //#endregion

        //public PushManager()
        //{
        //    apnManager = new APNsManager(true);

        //    gcmManager = new PushNotification();
        //    gcmManager.GoogleApiKey = WebConfigurationManager.AppSettings["GCM_API_Android"];
        //    gcmManager.SenderId = WebConfigurationManager.AppSettings["GCM_SID_Android"];

        //    list = new ArrayList();
        //}

        //public bool sendPushNotificationMassIOS(List<string> tokens, NotificationType type, object obj, int badge, String message)
        //{
        //    bool success = false;

        //    success = apnManager.SendPushNotifications(obj, type.ToString(), badge, message, tokens);


        //    return success;
        //}

        //public bool sendPushNotificationIOS(string deviceToken, NotificationType type, object obj, int badge, String message)
        //{
        //    bool success = false;

        //    success = apnManager.SendPushNotification(obj, type.ToString(), badge, message, deviceToken);

        //    return success;
        //}

        //public bool sendPushNotificationMassAndroid(List<string> tokens, NotificationType type, List<object> paramList, int badge, String message)
        //{
        //    bool success = false;

        //    list = new ArrayList();
        //    list.Add(new { event_type = type.ToString() });
        //    list.Add(new { messsage = message });
        //    if (paramList.Count < 6)
        //    {
        //        for (int j = paramList.Count; j < 6; j++)
        //        {
        //            paramList.Add(0);
        //        }
        //    }
        //    int i = 1;
        //    foreach (var item in paramList)
        //    {
        //        if (i == 1) list.Add(new { param1 = item });
        //        else if (i == 2) list.Add(new { param2 = item });
        //        else if (i == 3) list.Add(new { param3 = item });
        //        else if (i == 4) list.Add(new { param4 = item });
        //        else if (i == 5) list.Add(new { param5 = item });
        //        else if (i == 6) list.Add(new { param6 = item });
        //        i++;
        //    }
        //    string json = JsonConvert.SerializeObject(new { result = list });
        //    string jsonMsg = Utility.EscapeStringValue(json);

        //    success = gcmManager.SendPushNotificationAndroids(jsonMsg, type.ToString(), badge, message, tokens);

        //    list = new ArrayList();     // Clear ArrayList

        //    return success;
        //}

        //public bool sendPushNotificationAndroid(string deviceToken, NotificationType type, List<object> paramList, int badge, String message)
        //{
        //    bool success = false;

        //    list = new ArrayList();
        //    list.Add(new { event_type = type.ToString() });
        //    list.Add(new { messsage = message });
        //    if (paramList.Count < 6)
        //    {
        //        for (int j = paramList.Count; j < 6; j++)
        //        {
        //            paramList.Add(0);
        //        }
        //    }
        //    int i = 1;
        //    foreach (var item in paramList)
        //    {
        //        if (i == 1) list.Add(new { param1 = item });
        //        else if (i == 2) list.Add(new { param2 = item });
        //        else if (i == 3) list.Add(new { param3 = item });
        //        else if (i == 4) list.Add(new { param4 = item });
        //        else if (i == 5) list.Add(new { param5 = item });
        //        else if (i == 6) list.Add(new { param6 = item });
        //        i++;
        //    }
        //    string json = JsonConvert.SerializeObject(new { result = list });
        //    string jsonMsg = Utility.EscapeStringValue(json);

        //    success = gcmManager.SendPushNotificationAndroid(jsonMsg, type.ToString(), badge, message, deviceToken);

        //    list = new ArrayList();     // Clear ArrayList

        //    return success;
        //}
    }
    public enum NotificationType
    {
        LOGIN, ADDFRIEND, ACCEPTFRIEND, CHAT, ANNOUNCEMENT, NEWCAMPAIGN, NEWSHOP, RESTOREPRIZE
    }
}
