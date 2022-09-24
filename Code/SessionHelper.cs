using System.Web;

namespace QLNS.Code
{
    public class SessionHelper
    {
        //Luu session dang nhap
        public static void SetSession(UserSession session)
        {
            HttpContext.Current.Session["loginSession"] = session;
        }
        //Luu quyen
        public static void SetPermission(string quyen)
        {
            HttpContext.Current.Session["quyen"] = quyen;
        }
        //Lay session dang nhap
        public static UserSession GetSession()
        {
            var session = HttpContext.Current.Session["loginSession"];
            if (session == null)
                return null;
            else
            {
                return session as UserSession;
            }
        }
        //Kiem tra dang nhap
        public static bool IsLogin()
        {
            return GetSession() != null;
        }

        //Lay quyen dang nhap
        public static string GetPermission()
        {
            var quyen = HttpContext.Current.Session["quyen"] ?? null;
            return quyen as string;
        }
        //Xoa session dang nhap
        public static void RemoveSession()
        {
            HttpContext.Current.Session.Remove("loginSession");
            HttpContext.Current.Session.Remove("quyen");
        }
    }
}