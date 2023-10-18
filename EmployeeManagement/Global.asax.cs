using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;


namespace EmployeeManagement
{
    public class Global : HttpApplication
    {
        public static string pwdKey = System.Configuration.ConfigurationManager.AppSettings["PwdKey"];
        void Application_Start(object sender, EventArgs e)
        {
            // アプリケーションのスタートアップで実行するコードです
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Code that runs on application startup 
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Session_Start(object sender, EventArgs e)
        {
            // 新規セッションを開始したときに実行するコードです

            AddAccessLog();

        }

        public void AddAccessLog()
        {

            var log = log4net.LogManager.GetLogger("AccessLog");

            string strSw = "";
            try
            {
                if (Request.UserHostAddress is null)
                {
                    strSw += "0.0.0.0,";
                }
                else
                {
                    strSw += Request.UserHostAddress + " ,";
                }
            }
            catch
            {
                strSw += "0.0.0.0,";
            }
            try
            {
                if (System.Net.Dns.GetHostEntry(Request.UserHostName).HostName is null)
                {
                    strSw += " -- , ";
                }
                else
                {
                    strSw += " --" + System.Net.Dns.GetHostEntry(Request.UserHostName).HostName + ", ";
                }
            }
            catch
            {
                strSw += " -- , ";
            }
            try
            {
                if (Request.UserAgent is null)
                {
                    strSw += " -- , ";
                }
                else
                {
                    strSw += " --" + Request.UserAgent + ", ";
                }
            }
            catch
            {
                strSw += " -- , ";
            }
            try
            {
                if (Request.Url.ToString() is null)
                {
                    strSw += " -- ";
                }
                else
                {
                    strSw += " --" + Request.Url.ToString();
                }
            }
            catch
            {
                strSw += " -- ";
            }

            log.Info(strSw);

        }
    }
}