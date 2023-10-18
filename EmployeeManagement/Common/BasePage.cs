using System;
using System.Collections.Generic;
using System.Linq;
using BaseUtil;
using System.Web;
using System.Text;
using EmployeeManagement.Common;
using EmployeeManagement.Common.Class;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.SchudleKbnEnum;



namespace EmployeeManagement
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// ログインユーザ情報
        /// </summary>
        public static LoginInfo LoginUser = null;

        /// <summary>
        /// データベース接続
        /// </summary>
        public static DBProess db = new DBProess();

        /// <summary>
        /// Todo送信
        /// </summary>
        public static TODO td = new TODO();

        

        /// <summary>
        /// フォーマットクラス対象の定義
        /// </summary>
        public FormatUtil formatUtil = new FormatUtil();

        public log4net.ILog AppLog = log4net.LogManager.GetLogger("AppLog");

        public virtual void Page_PreLoad(object sender, System.EventArgs e)
        {
            //ログインページを場外
            if (this.Page.AppRelativeVirtualPath.IndexOf("Login.aspx") < 0)
            {
                //ログイン番号
                string userJud = Convert.ToString(Session["LoginNo"]);

                userJud = "IhJ17bo4xYY=";
                // セッションに保存がある場合
                if (!string.IsNullOrEmpty(userJud))
                {
                    //ログインユーザ初回検索
                    if (LoginUser == null)
                    {
                        Dictionary<string, object> para = new Dictionary<string, object>();
                        para.Add("社員番号", DecryptString(userJud));
                        List<Dictionary<string, object>> list = db.ExecuteSQForList(this.Page, "SELECT_LoginInfo", para);
                        LoginUser = new LoginInfo(list[0]);
                    }
                    // マスタページのログイン者名を表示
                     (Page.Master.FindControl("LoginName") as Label).Text = "[" + LoginUser.SyozokuName.ToString() + "] " + LoginUser.UserName;
                    //チェックがなかった場合のタイムアウト再設定
                    if (Session.Timeout < 60 && 0 < Session.Timeout)
                    {
                        Session.Timeout = 60;
                    }
                }
                else //セッション切れのログイン画面遷移
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }

        /// <summary>
        ///文字列を暗号化する
        ///</summary>
        ///<param name="str">暗号化する文字列</param>
        ///<returns>暗号化された文字列</returns>
        public string EncryptString(string str)
        {
            return PWDCoding.EncryptString(str, Global.pwdKey);
            
        }

        /// <summary>
        ///暗号化された文字列を復号化する
        ///</summary>
        ///<param name="str">暗号化された文字列</param>
        ///<returns>復号化された文字列</returns>
        public string DecryptString(string str)
        {
            return PWDCoding.DecryptString(str, Global.pwdKey);
        }

        /// <summary>
        /// メッセージ内容取得
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns>メッセージ内容</returns>
        public String GetMessage(String messageID)
        {
            String msg = "";
            var mst = Cache.Get("MassegeMst");
            if (mst != null)
            {
                msg = (mst as List<Dictionary<string, object>>).Where(x => x["メッセージID"].ToString().Equals(messageID))
                      .Select(x => x["メッセージ内容"].ToString()).FirstOrDefault();
            }
            else
            {
                var mstList = db.ExecuteSQForList("SELECT * FROM メッセージマスタ ");
                Cache.Insert("MassegeMst", mstList);
                msg = mstList.Where(x => x["メッセージID"].ToString().Equals(messageID))
                    .Select(x => x["メッセージ内容"].ToString()).FirstOrDefault();
            }
            return msg;
        }

        /// <summary>
        /// 名称マスタ取得
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetNameMst()
        {
            List<Dictionary<string, object>> results = null;
            var dmst = Cache.Get("NameMst");
            if (dmst != null)
            {
                results = dmst as List<Dictionary<string, object>>;
            }
            else
            {
                results = db.ExecuteSQForList("SELECT * FROM 名称マスタ ");
                Cache.Insert("NameMst", results);
            }
            return results;
        }

        /// <summary>
        /// 部署マスタ取得
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetDepartmentMst()
        {
            List<Dictionary<string, object>> results = null;
            var gdmst = Cache.Get("DepatmentMst");
            if (gdmst != null)
            {
                results = gdmst as List<Dictionary<string, object>>;
            }
            else
            {
                results = db.ExecuteSQForList("SELECT * FROM 部署マスタ ");
                Cache.Insert("DepatmentMst", results);
            }
            return results;
        }

        /// <summary>
        /// 部署名取得
        /// </summary>
        /// <param name="SyozokuID"></param>
        /// <returns></returns>
        public String GetDepartmentName(int SyozokuID)
        {
            string department = "";
            List<Dictionary<string, object>> bsmst = this.GetDepartmentMst();
            if (bsmst != null)
            {
                department = bsmst.Where(x => x["部署ＩＤ"].Equals(SyozokuID))
                    .Select(x => x["部署名"].ToString()).FirstOrDefault();
            }
            return department;
        }

        /// <summary>
        /// TODO種別名称取得
        /// </summary>
        /// <param name="kindID"></param>
        /// <returns></returns>
        public String GetTodoKindName(string kindID)
        {
            String mt = "";
            List<Dictionary<string, object>> dmst = this.GetNameMst();
            if (dmst != null)
            {
                mt = dmst.Where(x => x["分類コード"].ToString().Equals("E0001")).Where(x => x["SEQ"].ToString().Equals(kindID))
                        .Select(x => x["名称"].ToString()).FirstOrDefault();
            }
            return mt;
        }

        /// <summary>
        /// 書類名称取得
        /// </summary>
        /// <param name="documentID">書類ID</param>
        /// <returns>書類名称</returns>
        public String GetDocumentName(string documentID)
        {
            String dcmt = "";
            List<Dictionary<string, object>> dmst = this.GetNameMst();
            if (dmst != null)
            {
                dcmt = dmst.Where(x => x["分類コード"].ToString().Equals("D0001")).Where(x => x["SEQ"].ToString().Equals(documentID))
                        .Select(x => x["名称"].ToString()).FirstOrDefault();
            }
            return dcmt;
        }

        /// <summary>
        /// 祝日判定
        /// </summary>
        /// <param name="date_">yyyyMMdd</param>
        /// <returns>true/false</returns>
        public bool IsHoliday(String date_)
        {
            bool ret = false;
            //HokidayMstどこで定義？
            var mst = Cache.Get("HolidayMst");
            Dictionary<string, object> data_ = null;
            if (mst != null)
            {
                data_ = (mst as List<Dictionary<string, object>>).Where(x => x["date"].ToString().Equals(date_)).FirstOrDefault();
            }
            else
            {
                var mstList = db.ExecuteSQForList("SELECT * FROM カレンダーマスタ ");
                Cache.Insert("HolidayMst", mstList);
                data_ = mstList.Where(x => x["date"].ToString().Equals(date_)).FirstOrDefault();
            }
            ret = data_ == null ? false : true;
            return ret;
        }

        /// <summary>
        /// SQL本文取得
        /// </summary>
        /// <param name="sqlID">SQLID</param>
        /// <returns>SQL本文</returns>
        public String GetSqlContext( String sqlID)
        {
            string sqlContext = "";
            var mst = Cache.Get("SQLMst");
            if (Cache.Get("SQLMst") != null)
            {
                sqlContext = (mst as List<Dictionary<string, object>>).Where(x => x["SQLID"].ToString().Equals(sqlID))
                      .Select(x => x["SqlContext"].ToString()).FirstOrDefault();
            }
            else
            {
                var mstList = db.ExecuteSQForList("SELECT * FROM SQLマスタ ");
                Cache.Insert("SQLMst", mstList);
                sqlContext = mstList.Where(x => x["SQLID"].ToString().Equals(sqlID))
                    .Select(x => x["SqlContext"].ToString()).FirstOrDefault();
            }
            return sqlContext;
        }
        /// <summary>
        /// スケジュールテーブルデータ作成
        /// </summary>
        /// <param name="baseDate">ベース日付</param>
        /// <param name="employeeList">社員一覧</param>
        /// <param name="scheduleList">スケジュール一覧</param>
        /// <returns>テーブル文字列</returns>
        public string CreatSchedulTable(DateTime baseDate, List<Dictionary<string, object>> employeeList, List<Dictionary<string, object>> scheduleList)
        {
            StringBuilder builder = new StringBuilder();
            List<Dictionary<string, object>> list = null;
            var element = employeeList.SelectMany(x => x.Where(y => y.Key == "社員名称"));
            string url = "";
            string target = "";
            if (Request.Url != null)
            {
                ViewState["Url"] = Request.Url.AbsoluteUri.ToString();
                url = ViewState["Url"].ToString();
                target = "ScheduleList";
                if (url.Contains(target) == true)
                {
                    builder.AppendLine("<table class='margin-TopLeft' cellspacing='0' rules='all' border='1' id='MainContent_gridViewSchedule' style='max-width: 1131px;width: 100%; border - collapse:collapse; '>");
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<th scope = 'col' style = 'max-width: 1131px;width: 15%;height:7%;' > &nbsp;</th>");
                }
                else
                {
                    builder.AppendLine("<table class='schedule' cellspacing='0' rules='all' border='1' id='MainContent_gridViewSchedule' style='max-width: 1085px;width: 100%; border - collapse:collapse; '>");
                    builder.AppendLine("<tr>");
                }
            }
            ////builder.AppendLine("<tr>");

            //if (element.Any())
            //{
            //     builder.AppendLine("<table class='margin-TopLeft' cellspacing='0' rules='all' border='1' id='MainContent_gridViewSchedule' style='max-width: 1131px;width: 100%; border - collapse:collapse; '>");
            //}
            //else
            //{
            //    builder.AppendLine("<table class='schedule' cellspacing='0' rules='all' border='1' id='MainContent_gridViewSchedule' style='max-width: 1085px;width: 100%; border - collapse:collapse; '>");
            //}
            ////ヘッダ作成
            //builder.AppendLine("<tr>");
            //if(element.Any())
            
            
            for (int i = 0; i < 7; i++)
            {
                //祝日判定
                if (IsHoliday(baseDate.AddDays(i).ToString("yyyyMMdd")))
                {
                    builder.AppendLine("<th scope = 'col' style = 'max-width: 1085px;width: 15%;height:7%; background-color: #edb078;' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
                }
                //土曜日判定
                else if (baseDate.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                {
                    builder.AppendLine("<th scope = 'col' style = 'max-width: 1085px;width: 15%;height:7%; background-color: #A2D4FF;' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
                }
                //日曜日判定
                else if (baseDate.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                {
                    builder.AppendLine("<th scope = 'col' style = 'max-width: 1085px;width: 15%;height:7%; background-color: #edb078;' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
                }
                //平日
                else
                {
                    builder.AppendLine("<th scope = 'col' style = 'max-width: 1085px;width: 15%;height:7%' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
                }
            }
            builder.AppendLine("</tr>");
            //スケジュール詳細作成
            string content = "";
            if (employeeList != null && employeeList.Any())
            {
                foreach (Dictionary<string, object> employee in employeeList)
                {
                    // 日付モード
                    builder.AppendLine("<tr>");
                    if (url.Contains(target) == true)
                    {
                        builder.AppendLine("<td><a style='display:inline-block;height:110px;' >" + employee["社員名称"].ToString() + "</a></td>");
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        builder.AppendLine("<td style = 'vertical-align: top;height:110px;'>");
                        builder.AppendLine("<a  href='/Schedule/ScheduleEdit.aspx?No=" + employee["社員番号"].ToString() + "&Date=" + baseDate.AddDays(i).ToString("yyyyMMdd") + "' style='display:inline-block;height:20px;' >追加</a>");

                        list = GetSchedule(baseDate.AddDays(i), employee["社員番号"].ToString(), scheduleList);
                        if (list != null && list.Any())
                        {
                            foreach (Dictionary<string, object> item in list)
                            {
                                //休暇
                                if (item["スケジュールタイプ"].ToString() == Holiday.Id.ToString())
                                {
                                    if (item["内容"].ToString().Length > 5)
                                    {
                                        content = item["内容"].ToString().Substring(0, 5) + "...";
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + Holiday.Name + " 　" + content + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + Holiday.Name + " 　" + item["内容"].ToString() + "</a>");
                                    }
                                }
                                //午前休
                                else if (item["スケジュールタイプ"].ToString() == HalfMorning.Id.ToString())
                                {
                                    if (item["内容"].ToString().Length > 3)
                                    {
                                        content = item["内容"].ToString().Substring(0, 3) + "...";
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfMorning.Name + " 　" + content + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfMorning.Name + " 　" + item["内容"].ToString() + "</a>");
                                    }
                                }
                                //午後休
                                else if (item["スケジュールタイプ"].ToString() == HalfAfternoon.Id.ToString())
                                {
                                    if (item["内容"].ToString().Length > 3)
                                    {
                                        content = item["内容"].ToString().Substring(0, 3) + "...";
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfAfternoon.Name + " 　" + content + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfAfternoon.Name + " 　" + item["内容"].ToString() + "</a>");
                                    }
                                }
                                //その他のスケジュールタイプ
                                else
                                {
                                    builder.AppendLine("<span style='display:block;color:black;margin-top:3px;' > " + item["開始時刻"].ToString().Insert(2, ":") + "～" + item["終了時刻"].ToString().Insert(2, ":") + "</a>");
                                    if (item["内容"].ToString().Length > 9)
                                    {
                                        content = item["内容"].ToString().Substring(0, 9) + "...";
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;' > " + content + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;' > " + item["内容"].ToString() + "</a>");
                                    }
                                }
                            }
                        }
                        builder.AppendLine("</td>");
                    }
                    builder.AppendLine("</tr>");
                    //期間モード
                    list = null;
                    if (scheduleList != null && scheduleList.Any())
                    {
                        list = scheduleList.Where(x => x["社員番号"].ToString() == employee["社員番号"].ToString()
                                        && x["スケジュールモード"].ToString() == "2").ToList();
                    }
                    if (list != null && list.Any())
                    {
                        foreach (Dictionary<string, object> item in list)
                        {
                            builder.AppendLine("<tr>");
                            if (url.Contains(target) == true)
                            {
                                builder.AppendLine("<td>&nbsp;</td>");

                            }

                            int cnt = 0;
                            while (cnt < 7)
                            {
                                int colspan = GetColspan(baseDate.AddDays(cnt), item, 7 - cnt);
                                if (colspan == 0)
                                {
                                    builder.AppendLine("<td></td>");
                                    cnt++;
                                }
                                else
                                {
                                    if (colspan > 1)
                                    {
                                        builder.AppendLine("<td colspan='" + colspan.ToString() + "' style='background-color: #c9fa98;'>");
                                    }
                                    else
                                    {
                                        builder.AppendLine("<td style='background-color: #c9fa98;'>");
                                    }
                                    //表示内容
                                    //休暇

                                    //返ってきたものはstring型
                                    builder.AppendLine(ScheduleTypeName(item));
                                    builder.AppendLine("</td>");
                                    cnt = cnt + colspan;
                                }
                            }
                            builder.AppendLine("</tr>");
                        }
                    }
                }
            }
            builder.AppendLine("</table>");
            return builder.ToString();
        }
        /// <summary>
        /// 期間モード内容変換
        /// </summary>
        /// <param name="scheduleItem"></param>
        /// <returns></returns>
        private string ScheduleTypeName(Dictionary<string, object> scheduleItem)
        {
            StringBuilder builder = new StringBuilder();
            string scheduleType = scheduleItem["スケジュールタイプ"].ToString();
            string url = "<a  href='/Schedule/ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > ";
            string content = "(" + scheduleItem["内容"].ToString() + ")" + "</a>";
            if (scheduleType == Holiday.Id.ToString())
            {
                //スケジュールタイプ（内容）
                builder.AppendLine(url + Holiday.Name + content);
            }
            //午前休
            else if (scheduleType == HalfMorning.Id.ToString())
            {
                builder.AppendLine(url + HalfMorning.Name + content);
            }
            //午後休
            else if (scheduleType == HalfAfternoon.Id.ToString())
            {
                builder.AppendLine(url + HalfAfternoon.Name + content);
            }
            //社内
            else if (scheduleType == InCompany.Id.ToString())
            {
                builder.AppendLine(url + InCompany.Name + content);
            }
            //社外
            else if (scheduleType == OutCompany.Id.ToString())
            {
                builder.AppendLine(url + OutCompany.Name + content);
            }
            //テレワーク
            else if (scheduleType == Telework.Id.ToString())
            {
                builder.AppendLine(url +  Telework.Name + content);
            }
            //その他
            else if (scheduleType == Other.Id.ToString())
            {
                builder.AppendLine(url + Other.Name + content);
            }
            return builder.ToString();
        }


        /// <summary>
        /// 該当日付データ取得
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userNo"></param>
        /// <param name="scheduleList"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetSchedule(DateTime date, string userNo, List<Dictionary<string, object>> scheduleList)
        {
            List<Dictionary<string, object>> results = null;
            if (scheduleList != null && scheduleList.Any())
            {
                results = scheduleList.Where(x => x["社員番号"].ToString() == userNo &&
                DateTime.Parse(x["開始日"].ToString()).ToString("yyyyMMdd") == date.ToString("yyyyMMdd")
                && x["スケジュールモード"].ToString() == "1").ToList();
            }
            return results;
        }

        /// <summary>
        /// 期間データ作成
        /// </summary>
        /// <param name="date"></param>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private int GetColspan(DateTime date, Dictionary<string, object> schedule, int maxCnt)
        {
            int colspan = 0;
            for (int i = 0; i < maxCnt; i++)
            {
                if (int.Parse(DateTime.Parse(schedule["開始日"].ToString()).ToString("yyyyMMdd")) <= int.Parse(date.AddDays(i).ToString("yyyyMMdd"))
                    && int.Parse(DateTime.Parse(schedule["終了日"].ToString()).ToString("yyyyMMdd")) >= int.Parse(date.AddDays(i).ToString("yyyyMMdd")))
                {
                    colspan += 1;
                }
                if (i == 0 && colspan == 0)
                {
                    break;
                }
            }
            return colspan;
        }
    }
}