using System.Data;

namespace Hooray.Core.ViewModels
{
    public class StartupBadgeViewModel
    {
        public int friend_badge_count { get; set; }
        public int chat_badge_count { get; set; }
        public int gift_badge_count { get; set; }
        public int result_badge_count { get; set; }
        public int follow_badge_count { get; set; }

        public void loadDataStartupBadge(DataRow dr)
        {
            friend_badge_count = int.Parse(dr["un_new_friends"].ToString());
            chat_badge_count = int.Parse(dr["un_new_chat"].ToString());
            gift_badge_count = int.Parse(dr["un_new_gift"].ToString());
            result_badge_count = int.Parse(dr["un_new_result"].ToString());
            follow_badge_count = int.Parse(dr["un_new_follow"].ToString());
        }
    }
}
