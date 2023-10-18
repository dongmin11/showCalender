using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.SchudleKbnEnum;
using static EmployeeManagement.Common.CommonUtil;

namespace EmployeeManagement.TopPage
{
    public partial class TopPage : BasePage
    {
            //スケジュールテーブル
        public string ScheduleTable = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //ポストバックされていない
            if (!IsPostBack)
            {
                //今現在の時刻
                DateTime date = DateTime.Now;
                //後で確認
                ScheduleTable = SetScheduleDate(date);//受け取る
                //ToDoデータ取得
                GvToDo.ShowHeader = false;
                DataTable dt = new DataTable();
                dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_TopPage");
                GvToDo.DataSource = dt;
                GvToDo.DataBind();
                //未完了一覧情報

            }
        }

        
        /// <summary>
        /// スケジュール画面データ作成
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns></returns>
        private string SetScheduleDate(DateTime date)
        {
            //ログインユーザーのスケジュールを取得
            List<Dictionary<string, object>> scheduleList = new List<Dictionary<string, object>>();
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            for (int i = 0; i < 7; i++)
            {
                parameter.Add("Date" + (i + 1).ToString(), date.AddDays(i).ToString("yyyy/MM/dd"));
            }
            parameter.Add("WhereString", " AND sck.社員番号  = " + LoginUser.UserNo.ToString());
            scheduleList = db.ExecuteSQForList(this.Page, "SELECT_Schedule", parameter);
            return CreatScheduleTable(date, scheduleList);//渡す
        }

        /// <summary>
        /// スケジュールテーブル作成
        /// </summary>
        /// <param name="date">日付</param>
        /// <param name="scheduleList">スケジュールリスト</param>
        /// <returns></returns>
        private string CreatScheduleTable(DateTime date, List<Dictionary<string, object>> scheduleList)//受け取る形
        {
            //文字列に変更を加える
            StringBuilder builder = new StringBuilder();
            //テーブル
            builder.AppendLine("<table class='schedule' cellspacing='0' border='1' style='max-width: 1085px; width: 100%; border - collapse:collapse; '>");
            //ヘッダー(１週間分作成する)
            builder.AppendLine("<tr  scope = 'col' style = 'max-width: 1085px;width: 15%; height: 7%;'>");
            for (int i = 0; i < 7; i++)
            {
                //土曜日
                if (date.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                {
                    builder.AppendLine("<th style = 'background-color: #A2D4FF;' > " + date.AddDays(i).ToString("dd日(ddd)") + "</th>");
                }
                //日曜日,祝日
                else if (date.AddDays(i).DayOfWeek == DayOfWeek.Sunday ||IsHoliday(date.AddDays(i).ToString("yyyyMMdd")))
                {
                    builder.AppendLine("<th style = 'background-color: #edb078;' > " + date.AddDays(i).ToString("dd日(ddd)") + "</th>");
                }
                //平日
                else
                {
                    builder.AppendLine("<th> " + date.AddDays(i).ToString("dd日(ddd)") + "</th>");
                }
            }
            builder.AppendLine("</tr>");
            //行を作成する
            if (LoginUser.UserNo.ToString() == scheduleList[0]["社員番号"].ToString())
            {
                //モード判定リスト
                List<Dictionary<string, object>> list = null;
                builder.AppendLine("<tr style = 'vertical-align: top; height: 110px;'>");
                for (int i = 0; i < 7; i++)
                {
                    builder.AppendLine("<td >");
                    builder.AppendLine("<a  href='ScheduleEdit.aspx?No=" + scheduleList[0]["社員番号"].ToString() + "&Date=" + date.AddDays(i).ToString("yyyyMMdd") + "' style='display:inline-block;height:20px;' >追加</a>");
                    if (scheduleList != null && scheduleList.Any())
                    {
                        list = GetScheduleMode1(date.AddDays(i), scheduleList);
                        if (list != null && list.Any())
                        {
                            foreach (Dictionary<string, object> item in list)
                            {
                                string  scheduleType = item["スケジュールタイプ"].ToString();
                                string hrefUrl = "<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > ";
                                string content = item["内容"].ToString();
                                //休み
                                if (scheduleType == Holiday.Id.ToString())
                                {
                                    if (content.Length > 5)
                                    {
                                        builder.AppendLine(hrefUrl + Holiday.Name + " 　" + content.Substring(0, 5) + "..." + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine(hrefUrl + Holiday.Name + " 　" + content + "</a>");
                                    }
                                }
                                //午前休
                                else if (scheduleType == HalfMorning.Id.ToString())
                                {
                                    if (content.Length > 3)
                                    {
                                        content = item["内容"].ToString().Substring(0, 3) + "...";
                                        builder.AppendLine(hrefUrl + HalfMorning.Name + " 　" + content.Substring(0, 3) + "..." + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine(hrefUrl + HalfMorning.Name + " 　" + content + "</a>");
                                    }
                                }
                                //午後休
                                else if (scheduleType == HalfAfternoon.Id.ToString())
                                {
                                    if (content.Length > 3)
                                    {
                                        content = item["内容"].ToString().Substring(0, 3) + "...";
                                        builder.AppendLine(hrefUrl + HalfAfternoon.Name + " 　" + content.Substring(0, 3) + "..." + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine(hrefUrl + HalfAfternoon.Name + " 　" + content + "</a>");
                                    }
                                }
                                //その他のスケジュールタイプ
                                else
                                {
                                    builder.AppendLine("<span style='display:block;color:black;margin-top:3px;' > " + item["開始時刻"].ToString().Insert(2, ":") + "～" + item["終了時刻"].ToString().Insert(2, ":") + "</a>");
                                    if (item["内容"].ToString().Length > 9)
                                    {
                                        content = item["内容"].ToString().Substring(0, 9) + "...";
                                        builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;' > " + content.Substring(0, 9) + "..." + "</a>");
                                    }
                                    else
                                    {
                                        builder.AppendLine("<a title='" + content + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;' > " +  content + "</a>");
                                    }
                                }
                            }
                        }
                        builder.AppendLine("</td>");
                    }
                    builder.AppendLine("</td>");
                }
                builder.AppendLine("</tr>");
                //期間モード取得
                //list = GetScheduleMode2(scheduleList);
                if (scheduleList != null && scheduleList.Any())
                {
                    list = scheduleList.Where(x => x["スケジュールモード"].ToString() == "2").ToList();
                }
                if(list != null && list.Any())
                {
                    foreach (Dictionary<string, object> item in list)
                    {
                        builder.AppendLine("<tr>");
                        int cnt = 0;
                        while (cnt < 7)
                        {
                            int colspan = GetColspan(date.AddDays(cnt), item, 7 - cnt);
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
                                builder.AppendLine(ScheduleTypeName(item));
                                builder.AppendLine("</td>");
                                cnt += colspan;
                            }
                        }
                        builder.AppendLine("</tr>");
                    }
                }
            }
            else
            {
                //ログイン画面に遷移する
            }
            builder.AppendLine("</table>");
            return builder.ToString();
        }

        /// <summary>
        /// 日付モード取得
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userNo"></param>
        /// <param name="scheduleList"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetScheduleMode1(DateTime date, List<Dictionary<string, object>> scheduleList)
        {
            List<Dictionary<string, object>> scheduleMode1 = null;
            if (scheduleList != null && scheduleList.Any())
            {
                scheduleMode1 = scheduleList.Where(x => DateTime.Parse(x["開始日"].ToString()).ToString("yyyyMMdd") == date.ToString("yyyyMMdd")
                && x["スケジュールモード"].ToString() == "1").ToList();
            }
            return scheduleMode1;
        }
        /// <summary>
        /// 期間データ作成
        /// </summary>
        /// <param name="date"></param>
        /// <param name="schedule"></param>
        /// <param name="maxCnt"></param>
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
        /// <summary>
        /// スケジュールタイプ名変換
        /// </summary>
        /// <param name="scheduleItem"></param>
        /// <returns></returns>
        private string ScheduleTypeName(Dictionary<string, object> scheduleItem)
        {
            StringBuilder builder = new StringBuilder();
            string scheType = scheduleItem["スケジュールタイプ"].ToString();
            string href = "<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > ";
            string scheContent = " 　" + scheduleItem["内容"].ToString() + "</a>";
            if (scheType == Holiday.Id.ToString())
            {
                //休み
                builder.AppendLine(href + Holiday.Name + scheContent);
            }
            else if (scheType == HalfMorning.Id.ToString())
            {　
                //午前半休
                builder.AppendLine(href + HalfMorning.Name + scheContent);
            }
            else if (scheType == HalfAfternoon.Id.ToString())
            {
                //午後休
                builder.AppendLine(href + HalfAfternoon.Name + scheContent);
            }
            else if (scheType == InCompany.Id.ToString())
            {
                //社内
                builder.AppendLine(href + InCompany.Name + scheContent);
            }
            else if (scheType == OutCompany.Id.ToString())
            {
                //社外
                builder.AppendLine(href + OutCompany.Name + scheContent);
            }
            else if (scheType == Telework.Id.ToString())
            {
                //テレワーク
                builder.AppendLine(href + Telework.Name + scheContent);
            }
            else if (scheType == Other.Id.ToString())
            {
                //その他
                builder.AppendLine(scheContent + Other.Name + scheContent);
            }
            return builder.ToString();
        }
    }
}
