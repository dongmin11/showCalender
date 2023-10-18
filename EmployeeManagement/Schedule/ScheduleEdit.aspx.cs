using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.SchudleKbnEnum;
using static EmployeeManagement.Common.CommonUtil;
namespace EmployeeManagement.Schedule
{
    public partial class ScheduleEdit : BasePage
    {
        public string editTitle = "スケジュール追加";//タイトル
        static string userId = "";//社員番号
        static string scheduleNo = "";//スケジュール番号
        static DateTime startDate = DateTime.Now;
        static string url = "";
        static string topUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    {
                        ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                        url = ViewState["RefUrl"].ToString();
                        string target = "TopPage";
                        if (url.Contains(target) == true)
                        {
                            topUrl = url;
                        }
                        else
                        {
                            topUrl = "";
                        }
                    }
                    startDate = DateTime.Now;
                    //日付変更ボタン
                    if (Session["basedate"] != null)
                    {
                        startDate = DateTime.Parse(Session["basedate"].ToString());
                    }
                    //ヘッダー曜日
                    string headerDate = "";
                    if (Request.QueryString["No"] != null && Request.QueryString["Date"] != null)
                    {
                        //社員番号取得
                        userId = Request.QueryString["No"];
                        //日付取得
                        headerDate = Request.QueryString["Date"];
                        txtDateStart.Text = DateTime.Parse(headerDate.ToString().Substring(0, 4) + "/" + headerDate.ToString().Substring(4, 2) + "/" + headerDate.ToString().Substring(6, 2)).ToString("yyyy年MM月dd日(ddd)");
                        txtDateEnd.Text = DateTime.Parse(headerDate.ToString().Substring(0, 4) + "/" + headerDate.ToString().Substring(4, 2) + "/" + headerDate.ToString().Substring(6, 2)).ToString("yyyy年MM月dd日(ddd)");
                        scheduleNo = "";
                        cbSchedule.Checked = false;
                        txtDateEnd.CssClass = "none";
                        lblDateTilde.CssClass = "none";
                        editTitle = "スケジュール追加";
                    }
                    else if (Request.QueryString["SchedulNo"] != null)
                    {
                        //スケジュール番号取得
                        scheduleNo = Request.QueryString["SchedulNo"];
                        userId = "";
                    }
                    else
                    {
                        //スケジュール一覧画面に遷移
                        Response.Redirect("./ScheduleList.aspx");
                    }
                    if (Session["basedate"] != null)
                    {
                        DateTime baseDate = DateTime.Parse(Session["basedate"].ToString());
                        Session["ベース日付"] = baseDate.ToString();
                    }
                    //初期表示
                    SetInit();
                    //画面のURLを取得
                    if (Request.UrlReferrer != null)
                    {
                        ViewState["PreviousScreen"] = Request.UrlReferrer.ToString();
                    }
                    //スケジュール番号がある場合更新処理をする
                    if (!string.IsNullOrEmpty(scheduleNo))
                    {
                        editTitle = "スケジュール更新";
                        btnAdd.Text = "更新";
                        Dictionary<string, object> parameter = new Dictionary<string, object>();
                        parameter.Add("SCHEDULE", scheduleNo);
                        List<Dictionary<string, object>> listSchedule = db.ExecuteSQForList(this.Page, "SELECT_ScheduleEdit", parameter);
                        DataSetting(listSchedule);
                    }
                }
                else if (scheduleNo != "")
                {
                    editTitle = "スケジュール更新";
                }
                //追加編集
                if (editTitle == "スケジュール追加" && cbSchedule.Checked == true)
                {
                    txtDateEnd.CssClass = "inline";
                    lblDateTilde.CssClass = "inline";
                }
                else if (editTitle == "スケジュール追加" && cbSchedule.Checked == false)
                {
                    txtDateEnd.CssClass = "none";
                    lblDateTilde.CssClass = "none";
                }
                //更新編集
                if (editTitle == "スケジュール更新" && cbSchedule.Checked == true)
                {
                    txtDateEnd.CssClass = "inline";
                    lblDateTilde.CssClass = "inline";
                }
                else if (editTitle == "スケジュール更新" && cbSchedule.Checked == false)
                {
                    txtDateEnd.CssClass = "none";
                    lblDateTilde.CssClass = "none";
                }
               
                
            }
            catch (Exception)
            {
                //エラーメッセージ、一覧画面に遷移
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                "errormes",
                "<script language=\"JavaScript\">window.alert('エラーが発生しました');"
                + "dwinow.location.href = './ScheduleList.aspx';</script>");
            }
            //初期表示
            void SetInit()
            {
                //予定DDL初期表示
                dropSchedule.AppendDataBoundItems = true;
                dropSchedule.Items.Insert(0, new ListItem("予定を選択", "0"));
                //開始時刻初期表示
                //開始時間
                dropStartHour.AppendDataBoundItems = true;
                dropStartHour.Items.Insert(0, new ListItem("--時", "0"));
                //開始分数
                dropStertMinute.AppendDataBoundItems = true;
                dropStertMinute.Items.Insert(0, new ListItem("--分", "0"));
                //終了時刻初期表示
                //開始時間
                dropEndHour.AppendDataBoundItems = true;
                dropEndHour.Items.Insert(0, new ListItem("--時", "0"));
                //開始分数
                dropEndMinute.AppendDataBoundItems = true;
                dropEndMinute.Items.Insert(0, new ListItem("--分", "0"));
                //スケジュールドロップダウンリストにアイテム追加
                ListItem holiday = new ListItem(Holiday.Name, Holiday.Id.ToString());
                ListItem halfMorning = new ListItem(HalfMorning.Name, HalfMorning.Id.ToString());
                ListItem halfAfternoon = new ListItem(HalfAfternoon.Name, HalfAfternoon.Id.ToString());
                ListItem inCompany = new ListItem(InCompany.Name, InCompany.Id.ToString());
                ListItem outCompany = new ListItem(OutCompany.Name, OutCompany.Id.ToString());
                ListItem teleWork = new ListItem(Telework.Name, Telework.Id.ToString());
                ListItem other = new ListItem(Other.Name, Other.Id.ToString());
                ListItem[] sheduleKbn = { holiday, halfMorning, halfAfternoon, inCompany, outCompany, teleWork, other };
                dropSchedule.Items.AddRange(sheduleKbn);
            }
        }
        private void DataSetting(List<Dictionary<string, object>> listSchedule)
        {
            //データ取得表示
            txtDateStart.Text = DateTime.Parse(listSchedule[0]["開始日"].ToString()).ToString("yyyy年MM月dd日(ddd)");
            //期間モード
            if (listSchedule[0]["スケジュールモード"].ToString() == "2")
            {
                txtDateEnd.Text = DateTime.Parse(listSchedule[0]["終了日"].ToString()).ToString("yyyy年MM月dd日(ddd)");
                cbSchedule.Checked = true;
                lblDateTilde.CssClass = "inline";
                txtDateEnd.CssClass = "inline";
            }
            //日付モード
            else
            {
                txtDateEnd.Text = DateTime.Parse(listSchedule[0]["終了日"].ToString()).ToString("yyyy年MM月dd日(ddd)");
                cbSchedule.Checked = false;
                lblDateTilde.CssClass = "none";
                txtDateEnd.CssClass = "none";
            }
            dropStartHour.Text = listSchedule[0]["開始時刻"].ToString().Substring(0, 2) + "時";
            dropStertMinute.Text = listSchedule[0]["開始時刻"].ToString().Substring(2, 2) + "分";
            dropEndHour.Text = listSchedule[0]["終了時刻"].ToString().Substring(0, 2) + "時";
            dropEndMinute.Text = listSchedule[0]["終了時刻"].ToString().Substring(2, 2) + "分";
            dropSchedule.SelectedValue = listSchedule[0]["スケジュールタイプ"].ToString();
            txtSchedule.Text = listSchedule[0]["内容"].ToString();
            txtMemo.Text = listSchedule[0]["メモー"].ToString();
            ViewState["LockVerValue"] = Convert.ToInt32(listSchedule[0]["LockVer"].ToString());
        }
        /// <summary>
        /// 追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Addbtn_Click(object sender, EventArgs e)
        {
            try
            {
                //入力判定チェック
                bool txtError = false;
                //モード1日付判定
                if (cbSchedule.Checked == false && !string.IsNullOrWhiteSpace(txtDateStart.Text.Trim()) && DateTime.TryParse(txtDateStart.Text, out _) == false)
                {
                    //入力正しくない
                    lblError.Text = GetMessage("DateErrorMes");
                    txtError = true;
                }
                else if (cbSchedule.Checked == false && string.IsNullOrWhiteSpace(txtDateStart.Text.Trim()))
                {
                    //入力されていない
                    lblError.Text = GetMessage("EmptyErrorMes");
                    txtError = true;
                }
                //モード2日付判定
                else if (cbSchedule.Checked == true && !string.IsNullOrWhiteSpace(txtDateStart.Text.Trim()) && DateTime.TryParse(txtDateStart.Text, out DateTime _) == false)
                {
                    //開始日が入力されていて、入力内容が正しくない場合
                    lblError.Text = GetMessage("DateErrorMes");
                    cbSchedule.Checked = true;
                    txtError = true;
                }
                else if (cbSchedule.Checked == true && string.IsNullOrWhiteSpace(txtDateStart.Text.Trim()))
                {
                    //開始日が入力されていない場合
                    lblError.Text = GetMessage("EmptyErrorMes");
                    txtError = true;
                }
                else if (cbSchedule.Checked == true && !string.IsNullOrWhiteSpace(txtDateEnd.Text.Trim()) && DateTime.TryParse(txtDateEnd.Text, out DateTime _) == false)
                {
                    //終了日が入力されていて、入力内容が正しくない場合
                    lblError.Text = GetMessage("DateErrorMes");
                    txtError = true;
                }
                else if (cbSchedule.Checked == true && string.IsNullOrWhiteSpace(txtDateEnd.Text.Trim()))
                {
                    //終了日が入力されていない場合
                    lblError.Text = GetMessage("EmptyErrorMes");
                    txtError = true;
                }
                //日付比較
                else if (DateTime.TryParse(txtDateStart.Text, out DateTime dtStart) == true &&
                DateTime.TryParse(txtDateEnd.Text, out DateTime dtEnd) == true && cbSchedule.Checked == true)
                {
                    if (dtStart > dtEnd)
                    {
                        lblError.Text = GetMessage("DateErrorMes");
                        txtError = true;
                    }
                }
                else
                {
                    lblError.Text = "";
                }
                //時刻判定(入力されていない、開始日の方が大きい場合)
                if (dropStartHour.SelectedValue == "0" || dropStertMinute.SelectedValue == "0" || dropEndHour.SelectedValue == "0" || dropEndMinute.SelectedValue == "0")
                {
                    //入力が正しくありません
                    lblTime.Text = GetMessage("TxtErrorMes");
                    txtError = true;
                }
                else if (int.Parse(dropStartHour.Text.ToString().Substring(0, 2) + dropStertMinute.Text.ToString().Substring(0, 2)) > int.Parse(dropEndHour.Text.ToString().Substring(0, 2) + dropEndMinute.Text.ToString().Substring(0, 2)))
                {
                    //入力が正しくありません
                    lblTime.Text = GetMessage("TxtErrorMes");
                    txtError = true;
                }
                else
                {
                    lblTime.Text = "";
                }
                //スケジュールタイプ判定
                if (dropSchedule.SelectedValue == "0")
                {
                    //入力必須項目です
                    lblSchedule.Text = GetMessage("EmptyErrorMes");
                    txtError = true;
                }
                //内容テキスト入力判定
                else if (string.IsNullOrEmpty(txtSchedule.Text))
                {
                    //入力必須項目です
                    lblSchedule.Text = GetMessage("EmptyErrorMes");
                    txtError = true;
                }
                else
                {
                    lblSchedule.Text = "";
                }
                if (txtError == true)
                {
                    return;
                }
                //一覧画面に渡す
                ////再度確認
                //Session["lblDate"] = DateTime.Parse(txtDateStart.Text);
                //パラメータ取得
                Dictionary<string, object> parameter = new Dictionary<string, object>();
                parameter.Add("開始日", DateTime.Parse(ChangeString(txtDateStart.Text)).ToString("yyyy/MM/dd"));
                parameter.Add("開始時刻", dropStartHour.Text.Substring(0, 2) + dropStertMinute.Text.Substring(0, 2));
                //parameter.Add("開始時刻", dropStartHour.SelectedValue);
                parameter.Add("終了時刻", dropEndHour.Text.Substring(0, 2) + dropStertMinute.Text.Substring(0, 2));
                parameter.Add("スケジュールタイプ", this.dropSchedule.SelectedValue);
                parameter.Add("内容", ChangeString(txtSchedule.Text));
                parameter.Add("メモー", ChangeString(txtMemo.Text));
                parameter.Add("登録ユーザー", LoginUser.UserNo);
                parameter.Add("登録日時", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
                parameter.Add("更新ユーザー", LoginUser.UserNo);
                parameter.Add("更新日時", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
                //チェックボックスにチェックが入っている
                if (cbSchedule.Checked == true)
                {
                    //期間モード
                    parameter.Add("スケジュールモード", 2);
                    parameter.Add("終了日", DateTime.Parse(ChangeString(txtDateEnd.Text)).ToString("yyyy/MM/dd"));
                }
                //チェックボックスにチェックが入っていない場合
                else
                {
                    //日付モード
                    parameter.Add("スケジュールモード", 1);
                    parameter.Add("終了日", DateTime.Parse(ChangeString(txtDateStart.Text)).ToString("yyyy/MM/dd"));
                }
                //追加
                if (!string.IsNullOrEmpty(userId))
                {
                    //社員番号
                    int ID = Convert.ToInt32(userId);
                    parameter.Add("社員番号", ID);
                    db.ExecuteNonQuerySQL(this.Page, "INSERT_Schedule", parameter);
                    if(topUrl == "")
                    {
                        //追加メッセージ表示、スケジュール一覧画面に遷移
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                        "insertmes",
                        "<script language=\"JavaScript\">window.alert('追加しました');"
                        + "window.location.href = './ScheduleList.aspx';</script>");
                    }
                    else
                    {
                        //追加メッセージ表示、スケジュール一覧画面に遷移
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                        "insertmes",
                        "<script language=\"JavaScript\">window.alert('追加しました');"
                        + "window.location.href = '../TopPage/TopPage.aspx';</script>");

                    }

                }
                //更新
                else if (!string.IsNullOrEmpty(scheduleNo))
                {
                    //LockVer取得
                    List<Dictionary<string, object>> lockVer = new List<Dictionary<string, object>>();
                    parameter.Add("スケ番号", scheduleNo);
                    lockVer = db.ExecuteSQForList(this.Page, "SELECT_LockVer", parameter);
                    string x = lockVer[0]["LockVer"].ToString();
                    int a = Convert.ToInt32(lockVer[0]["LockVer"]);
                    int b = Convert.ToInt32(ViewState["LockVerValue"]);
                    //排他制御
                    if (Convert.ToInt32(lockVer[0]["LockVer"]) != Convert.ToInt32(ViewState["LockVerValue"]))
                    {
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                        "errormes",
                        "<script language=\"JavaScript\">window.alert('エラーが発生しました');"
                        + "window.location.href = './ScheduleList.aspx';</script>");
                    }
                    //最新LockVer
                    int UpdateLockVer = Convert.ToInt32(ViewState["LockVerValue"].ToString()) + 1;
                    int scheduleID = Convert.ToInt32(scheduleNo);
                    parameter.Add("LockVer", UpdateLockVer);
                    parameter.Add("スケジュール番号", scheduleID);
                    db.ExecuteNonQuerySQL(this.Page, "UPDATE_Schedule", parameter);
                    if (topUrl == "")
                    {
                        //更新メッセージ表示、スケジュール一覧画面に遷移
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                        "insertmes",
                        "<script language=\"JavaScript\">window.alert('更新しました');"
                        + "window.location.href = './ScheduleList.aspx';</script>");
                    }
                    else
                    {
                        //更新メッセージ表示、スケジュール一覧画面に遷移
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                        "insertmes",
                        "<script language=\"JavaScript\">window.alert('更新しました');"
                        + "window.location.href = '../TopPage/TopPage.aspx';</script>");

                    }
                }
            }
            catch (Exception ex)
            {
                //エラーメッセージ、一覧画面に遷移
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                "errormes",
                "<script language=\"JavaScript\">window.alert('エラーが発生しました');"
                + "window.location.href = './ScheduleList.aspx';</script>");

            }
        }
        /// <summary>
        /// キャンセルボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["startDate"] = startDate;
            //string url = ViewState["PreviousScreen"].ToString();
            if (topUrl != "")
            {
                Response.Redirect("../TopPage/TopPage.aspx");
            }
            else
            {
                Response.Redirect("../Schedule/ScheduleList.aspx");

            }

        }
    }
}
//最後に削除して整える

