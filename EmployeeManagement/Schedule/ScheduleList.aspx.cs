using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.SchudleKbnEnum;
using static EmployeeManagement.Common.CommonUtil;

namespace EmployeeManagement
{
    public partial class ScheduleList : BasePage
    {
        public string ScheduleTable = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //日付
                DateTime baseDate = DateTime.Now;
                if (Session["startDate"] != null)
                {
                    baseDate = DateTime.Parse(Session["startDate"].ToString());
                    //Session.Remove("startDate");
                }
                //部署一覧
                DataTable department_Datas = db.ExecuteSQLForDataTable(this.Page, "SELECT_DepartMent");
                dropDept.DataSource = department_Datas;
                dropDept.DataBind();
                dropDept.Items.Insert(0, new ListItem("すべて表示", "0"));
                dropDept.SelectedValue = LoginUser.SyozokuID.ToString();
                ScheduleTable = SetSchedulData(baseDate, Convert.ToInt32(dropDept.SelectedValue));
            }
        }

        /// <summary>
        /// 所属ドロップダウンリスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScheduleTable = SetSchedulData(DateTime.Parse(lblToday.Text), Convert.ToInt32(dropDept.SelectedValue));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// スケジュール画面データ作成
        /// </summary>
        /// <param name="baseDate">ベース日付</param>
        /// <param name="deptNo">所属番号</param>
        /// <returns>テーブル文字列</returns>
        private string SetSchedulData(DateTime baseDate, int deptNo)
        {

            Dictionary<string, object> parameter = new Dictionary<string, object>();

            //基本日付
            this.lblToday.Text = baseDate.ToString("yyyy年MM月dd日(ddd)");
            List<Dictionary<string, object>> employeeList = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> scheduleList = new List<Dictionary<string, object>>();

            // 社員一覧取得
            string deptSelect;
            if (deptNo == 0)
            {
                //すべて表示
                parameter = new Dictionary<string, object>();
                parameter.Add("NUMBER", "所属");
                employeeList = db.ExecuteSQForList(this.Page, "SELECT_SyainName", parameter);
                deptSelect = "所属";
            }
            else
            {
                //該当する社員を表示
                parameter = new Dictionary<string, object>();
                parameter.Add("NUMBER", deptNo);
                employeeList = db.ExecuteSQForList(this.Page, "SELECT_SyainName", parameter);
                deptSelect = dropDept.SelectedValue;
            }

            //スケジュール一覧取得
            parameter = new Dictionary<string, object>();
            //画面一週間分を選択
            for (int i = 0; i < 7; i++)
            {
                parameter.Add("Date" + (i + 1).ToString(), baseDate.AddDays(i).ToString("yyyy/MM/dd"));
            }
            //条件・DDLの部署に所属する者
            parameter.Add("WhereString", " AND 所属  = " + deptSelect);
            //条件に合うスケジュールデータ取得
            scheduleList = db.ExecuteSQForList(this.Page, "SELECT_Schedule", parameter);

            return CreatSchedulTable(baseDate, employeeList, scheduleList);
        }
        /// <summary>
        /// スケジュールテーブルデータ作成
        /// </summary>
        /// <param name="baseDate">ベース日付</param>
        /// <param name="employeeList">社員一覧</param>
        /// <param name="scheduleList">スケジュール一覧</param>
        /// <returns>テーブル文字列</returns>
        //public string CreatSchedulTable(DateTime baseDate, List<Dictionary<string, object>> employeeList, List<Dictionary<string, object>> scheduleList)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    List<Dictionary<string, object>> list = null;
        //    //テーブルデザイン
        //    builder.AppendLine("<table class='margin-TopLeft' cellspacing='0' rules='all' border='1' id='MainContent_gridViewSchedule' style='max-width: 1131px;width: 100%; border - collapse:collapse; '>");
        //    //ヘッダ作成
        //    builder.AppendLine("<tr>");
        //    builder.AppendLine("<th scope = 'col' style = 'max-width: 1131px;width: 12.5%;' > &nbsp;</th>");

        //    for (int i = 0; i < 7; i++)
        //    {
        //        //祝日判定
        //        if (IsHoliday(baseDate.AddDays(i).ToString("yyyyMMdd")))
        //        {
        //            builder.AppendLine("<th scope = 'col' style = 'max-width: 1131px;width: 12.5%; background-color: #edb078;' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
        //        }
        //        //土曜日判定
        //        else if (baseDate.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
        //        {
        //            builder.AppendLine("<th scope = 'col' style = 'max-width: 1131px;width: 12.5%; background-color: #A2D4FF;' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
        //        }
        //        //日曜日判定
        //        else if (baseDate.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
        //        {
        //            builder.AppendLine("<th scope = 'col' style = 'max-width: 1131px;width: 12.5%; background-color: #edb078;' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
        //        }
        //        //平日
        //        else
        //        {
        //            builder.AppendLine("<th scope = 'col' style = 'max-width: 1131px;width: 12.5%;' > " + baseDate.AddDays(i).ToString("dd日(ddd)") + "</th>");
        //        }
        //    }





        //    builder.AppendLine("</tr>");




        //    //スケジュール詳細作成
        //    string content = "";
        //    if (employeeList != null && employeeList.Any())
        //    {
        //        foreach (Dictionary<string, object> employee in employeeList)
        //        {
        //            // 日付モード
        //            builder.AppendLine("<tr>");
        //            builder.AppendLine("<td><a style='display:inline-block;height:110px;' >" + employee["社員名称"].ToString() + "</a></td>");
        //            for (int i = 0; i < 7; i++)
        //            {
        //                builder.AppendLine("<td style = 'vertical-align: top;'>");
        //                builder.AppendLine("<a  href='ScheduleEdit.aspx?No=" + employee["社員番号"].ToString() + "&Date=" + baseDate.AddDays(i).ToString("yyyyMMdd") + "' style='display:inline-block;height:20px;' >追加</a>");

        //                list = GetSchedule(baseDate.AddDays(i), employee["社員番号"].ToString(), scheduleList);
        //                if (list != null && list.Any())
        //                {
        //                    foreach (Dictionary<string, object> item in list)
        //                    {
        //                        //休暇
        //                        if (item["スケジュールタイプ"].ToString() == Holiday.Id.ToString())
        //                        {
        //                            if (item["内容"].ToString().Length > 5)
        //                            {
        //                                content = item["内容"].ToString().Substring(0, 5) + "...";
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + Holiday.Name + " 　" + content + "</a>");
        //                            }
        //                            else
        //                            {
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + Holiday.Name + " 　" + item["内容"].ToString() + "</a>");
        //                            }
        //                        }
        //                        //午前休
        //                        else if (item["スケジュールタイプ"].ToString() == HalfMorning.Id.ToString())
        //                        {
        //                            if (item["内容"].ToString().Length > 3)
        //                            {
        //                                content = item["内容"].ToString().Substring(0, 3) + "...";
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfMorning.Name + " 　" + content + "</a>");
        //                            }
        //                            else
        //                            {
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfMorning.Name + " 　" + item["内容"].ToString() + "</a>");
        //                            }
        //                        }
        //                        //午後休
        //                        else if (item["スケジュールタイプ"].ToString() == HalfAfternoon.Id.ToString())
        //                        {
        //                            if (item["内容"].ToString().Length > 3)
        //                            {
        //                                content = item["内容"].ToString().Substring(0, 3) + "...";
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfAfternoon.Name + " 　" + content + "</a>");
        //                            }
        //                            else
        //                            {
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;background-color: tomato;margin-top:3px;word-break: break-all;' > " + HalfAfternoon.Name + " 　" + item["内容"].ToString() + "</a>");
        //                            }
        //                        }
        //                        //その他のスケジュールタイプ
        //                        else
        //                        {
        //                            builder.AppendLine("<span style='display:block;color:black;margin-top:3px;' > " + item["開始時刻"].ToString().Insert(2, ":") + "～" + item["終了時刻"].ToString().Insert(2, ":") + "</a>");
        //                            if (item["内容"].ToString().Length > 9)
        //                            {
        //                                content = item["内容"].ToString().Substring(0, 9) + "...";
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;' > " + content + "</a>");
        //                            }
        //                            else
        //                            {
        //                                builder.AppendLine("<a title='" + item["内容"].ToString() + "' href='ScheduleEdit.aspx?SchedulNo=" + item["スケジュール番号"].ToString() + "' style='display:block;color:black;' > " + item["内容"].ToString() + "</a>");
        //                            }
        //                        }
        //                    }
        //                }
        //                builder.AppendLine("</td>");
        //            }
        //            builder.AppendLine("</tr>");
        //            //期間モード
        //            list = null;
        //            if (scheduleList != null && scheduleList.Any())
        //            {
        //                list = scheduleList.Where(x => x["社員番号"].ToString() == employee["社員番号"].ToString()
        //                                && x["スケジュールモード"].ToString() == "2").ToList();
        //            }
        //            if (list != null && list.Any())
        //            {
        //                foreach (Dictionary<string, object> item in list)
        //                {
        //                    builder.AppendLine("<tr>");
        //                    builder.AppendLine("<td>&nbsp;</td>");

        //                    int cnt = 0;
        //                    while (cnt < 7)
        //                    {
        //                        int colspan = GetColspan(baseDate.AddDays(cnt), item, 7 - cnt);
        //                        if (colspan == 0)
        //                        {
        //                            builder.AppendLine("<td></td>");
        //                            cnt++;
        //                        }
        //                        else
        //                        {
        //                            if (colspan > 1)
        //                            {
        //                                builder.AppendLine("<td colspan='" + colspan.ToString() + "' style='background-color: #c9fa98;'>");
        //                            }
        //                            else
        //                            {
        //                                builder.AppendLine("<td style='background-color: #c9fa98;'>");
        //                            }
        //                            //表示内容
        //                            //休暇

        //                            //返ってきたものはstring型
        //                            builder.AppendLine(ScheduleTypeName(item));
        //                            builder.AppendLine("</td>");
        //                            cnt = cnt + colspan;
        //                        }
        //                    }
        //                    builder.AppendLine("</tr>");
        //                }
        //            }
        //        }
        //    }
        //    builder.AppendLine("</table>");
        //    return builder.ToString();
        //}
        ////string は返す型、クラス名（受け取る型）
        //private string ScheduleTypeName(Dictionary<string, object> scheduleItem)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    if (scheduleItem["スケジュールタイプ"].ToString() == Holiday.Id.ToString())
        //    {
        //        //スケジュールタイプ（内容）
        //        builder.AppendLine("<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > " + Holiday.Name + " 　" + scheduleItem["内容"].ToString() + "</a>");
        //    }
        //    //午前休
        //    else if (scheduleItem["スケジュールタイプ"].ToString() == HalfMorning.Id.ToString())
        //    {
        //        builder.AppendLine("<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > " + HalfMorning.Name + "　 " + scheduleItem["内容"].ToString() + "</a>");
        //    }
        //    //午後休
        //    else if (scheduleItem["スケジュールタイプ"].ToString() == HalfAfternoon.Id.ToString())
        //    {
        //        builder.AppendLine("<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > " + HalfAfternoon.Name + "　 " + scheduleItem["内容"].ToString() + "</a>");
        //    }
        //    //社内
        //    else if (scheduleItem["スケジュールタイプ"].ToString() == InCompany.Id.ToString())
        //    {
        //        builder.AppendLine("<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > " + InCompany.Name + "　 " + scheduleItem["内容"].ToString() + "</a>");
        //    }
        //    //社外
        //    else if (scheduleItem["スケジュールタイプ"].ToString() == OutCompany.Id.ToString())
        //    {
        //        builder.AppendLine("<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all;' > " + OutCompany.Name + " 　" + scheduleItem["内容"].ToString() + "</a>");
        //    }
        //    //テレワーク
        //    else if (scheduleItem["スケジュールタイプ"].ToString() == Telework.Id.ToString())
        //    {
        //        builder.AppendLine("<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > " + Telework.Name + " 　" + scheduleItem["内容"].ToString() + "</a>");
        //    }
        //    //その他
        //    else if (scheduleItem["スケジュールタイプ"].ToString() == Other.Id.ToString())
        //    {
        //        builder.AppendLine("<a  href='ScheduleEdit.aspx?SchedulNo=" + scheduleItem["スケジュール番号"].ToString() + "' style='display:inline-block;color:black;word-break: break-all' > " + Other.Name + " " + scheduleItem["内容"].ToString() + "</a>");
        //    }
        //    return builder.ToString();
        //}


        ///// <summary>
        ///// 該当日付データ取得
        ///// </summary>
        ///// <param name="date"></param>
        ///// <param name="userNo"></param>
        ///// <param name="scheduleList"></param>
        ///// <returns></returns>
        //private List<Dictionary<string, object>> GetSchedule(DateTime date, string userNo, List<Dictionary<string, object>> scheduleList)
        //{
        //    List<Dictionary<string, object>> results = null;
        //    if (scheduleList != null && scheduleList.Any())
        //    {
        //        results = scheduleList.Where(x => x["社員番号"].ToString() == userNo &&
        //        DateTime.Parse(x["開始日"].ToString()).ToString("yyyyMMdd") == date.ToString("yyyyMMdd")
        //        && x["スケジュールモード"].ToString() == "1").ToList();
        //    }
        //    return results;
        //}

        ///// <summary>
        ///// 期間データ作成
        ///// </summary>
        ///// <param name="date"></param>
        ///// <param name="schedule"></param>
        ///// <returns></returns>
        //private int GetColspan(DateTime date, Dictionary<string, object> schedule, int maxCnt)
        //{
        //    int colspan = 0;
        //    for (int i = 0; i < maxCnt; i++)
        //    {
        //        if (int.Parse(DateTime.Parse(schedule["開始日"].ToString()).ToString("yyyyMMdd")) <= int.Parse(date.AddDays(i).ToString("yyyyMMdd"))
        //            && int.Parse(DateTime.Parse(schedule["終了日"].ToString()).ToString("yyyyMMdd")) >= int.Parse(date.AddDays(i).ToString("yyyyMMdd")))
        //        {
        //            colspan += 1;
        //        }
        //        if (i == 0 && colspan == 0)
        //        {
        //            break;
        //        }
        //    }
        //    return colspan;
        //}

        /// <summary>
        /// 先月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLastMonth_Click(object sender, EventArgs e)
        {
            DateTime baseDate = DateTime.Parse(lblToday.Text);
            ScheduleTable = SetSchedulData(baseDate.AddMonths(-1), Convert.ToInt32(dropDept.SelectedValue));
            Session["basedate"] = baseDate.AddMonths(-1);

        }

        /// <summary>
        /// 先週
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLastWeek_Click(object sender, EventArgs e)
        {
            DateTime baseDate = DateTime.Parse(lblToday.Text);
            ScheduleTable = SetSchedulData(baseDate.AddDays(-7), Convert.ToInt32(dropDept.SelectedValue));
            Session["basedate"] = baseDate.AddDays(-7);
        }

        /// <summary>
        /// 本日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToday_Click(object sender, EventArgs e)
        {
            DateTime baseDate = DateTime.Now;
            ScheduleTable = SetSchedulData(baseDate, Convert.ToInt32(dropDept.SelectedValue));
            Session["basedate"] = baseDate;
        }

        /// <summary>
        /// 翌週
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNextweek_Click(object sender, EventArgs e)
        {
            DateTime baseDate = DateTime.Parse(lblToday.Text);
            ScheduleTable = SetSchedulData(baseDate.AddDays(7), Convert.ToInt32(dropDept.SelectedValue));
            Session["basedate"] = baseDate.AddDays(7);
        }

        /// <summary>
        /// 翌月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNextMonth_Click(object sender, EventArgs e)
        {
            DateTime baseDate = DateTime.Parse(lblToday.Text);
            ScheduleTable = SetSchedulData(baseDate.AddMonths(1), Convert.ToInt32(dropDept.SelectedValue));
            Session["basedate"] = baseDate.AddMonths(1);
        }
    }
}
