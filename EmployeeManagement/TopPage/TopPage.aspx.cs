using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.TodoKindEnum;
using static EmployeeManagement.Enumerations.DocumentStaEnum;

namespace EmployeeManagement.TopPage
{
    public partial class TopPage : BasePage
    {
        //スケジュールテーブル
        public string ScheduleTable = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //今現在の時刻
                    DateTime date = DateTime.Now;
                    //スケジュールリスト
                    ScheduleTable = SetScheduleDate(date);
                    GvToDo.ShowHeader = false;
                    GvDocument.ShowHeader = false;
                    //グリッドビュー表示
                    GridViewDataSetting();
                }
            }
            catch(Exception ex)
            {
                //ログにエラーメッセージを表示
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
              "errormes",
              "<script language=\"JavaScript\">window.alert('エラーが発生しました');"
              + "dwinow.location.href = './TopPage/TopPage.aspx';</script>");
            }
        }
        /// <summary>
        /// グリッドビュー表示
        /// </summary>
        private void GridViewDataSetting()
        {
            //表示の修正
            try
            {
                //ログインユーザーの社員番号
                List<Dictionary<string, object>> loginUserInfo = new List<Dictionary<string, object>>();
                Dictionary<string, object> parame = new Dictionary<string, object>();
                parame.Add("UserNo", LoginUser.UserNo);
                loginUserInfo = db.ExecuteSQForList(this.Page, "SELECT_LoginUserInfo", parame);
                //ToDoデータ取得
                DataTable dt;
                Dictionary<string, object> parameter = new Dictionary<string, object>();
                parameter.Add("LoginUserNo", LoginUser.UserNo);
                dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_ToDoInfo", parameter);
                GvToDo.DataSource = dt;
                GvToDo.DataBind();
                //書類申請情報取得
                Dictionary<string, object> para = new Dictionary<string, object>();
                //ログインユーザーが管理部門の場合
                if (LoginUser.SyozokuID == 2)
                {
                    //全社員の未完了一覧情報を表示
                    para.Add("UserNo", "us.社員番号");
                }
                else
                {
                    //個人の未完了一覧情報を表示
                    para.Add("UserNo", LoginUser.UserNo);
                }
                //書類No変換
                dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_DocumentInfo", para);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["書類No"].ToString().IndexOf(",") > 0)
                    {
                        dt.Rows[i]["書類No"] = GetDocumentName(dt.Rows[i]["書類No"].ToString().Split(',')[0]) + "など";
                    }
                    else
                    {
                        dt.Rows[i]["書類No"] = GetDocumentName(dt.Rows[i]["書類No"].ToString());
                    }
                }
                GvDocument.DataSource = dt;
                GvDocument.DataBind();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 書類申請一覧グリッドビュー表示変換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //[書類No]表示文字数
                int maxString;
                //ログインユーザーが管理者
                if(LoginUser.SyozokuID == 2)
                {
                    //社員名称を表示する
                    ((BoundField)this.GvDocument.Columns[1]).DataField = "社員名称";
                    maxString = 10;
                }
                //管理部門でない
                else
                {
                    maxString = 13;
                }
                //[ステータス]変換
                if (e.Row.Cells[3].Text == Unapproved.Id.ToString())
                {
                    //未承認
                    e.Row.Cells[3].Text = Unapproved.Name;
                }
                else if (e.Row.Cells[3].Text == Remand.Id.ToString())
                {
                    //差戻し
                    e.Row.Cells[3].Text = Remand.Name;
                }
                else if (e.Row.Cells[3].Text == Making.Id.ToString())
                {
                    //作成中
                    e.Row.Cells[3].Text = Making.Name;
                }
                else if (e.Row.Cells[3].Text == WaitTake.Id.ToString())
                {
                    //受取待ち
                    e.Row.Cells[3].Text = WaitTake.Name;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //ツールチップ処理
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                    }
                    //社員名称
                    if(LoginUser.SyozokuID == 2)
                    {
                        if (e.Row.Cells[1].Text.Length > 5)
                        {
                            //社員名称
                            e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 4) + "...";
                        }
                    }
                    //書類名称
                    LinkButton documentlinkButton = (LinkButton)e.Row.FindControl("BtnDocument");
                    if (documentlinkButton.Text.Length > maxString)
                    {
                        documentlinkButton.Text = documentlinkButton.Text.Substring(0, maxString - 1) + "...";
                    }
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// TODO新着グリッドビュー表示変換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvToDo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //[種別]変換
                    if (e.Row.Cells[1].Text == Normal.Id.ToString())
                    {
                        //[重要]
                        e.Row.Cells[1].Text = Normal.ShortName;
                    }
                    else if (e.Row.Cells[1].Text == Communicate.Id.ToString())
                    {
                        //[至急]
                        e.Row.Cells[1].Text = Communicate.ShortName;
                    }
                    else if (e.Row.Cells[1].Text == Share.Id.ToString())
                    {
                        //[親展]
                        e.Row.Cells[1].Text = Share.ShortName;
                    }
                    LinkButton toDolinkButton = (LinkButton)e.Row.FindControl("BtnToDo");
                    if(toDolinkButton.Text.Length > 15)
                    {
                        toDolinkButton.Text = toDolinkButton.Text.Substring(0, 14) + "...";
                    }
                }
                e.Row.Cells[1].ToolTip = e.Row.Cells[1].Text;
            }
            catch (Exception ex)
            {
                //エラーメッセージ
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// スケジュール画面作成
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns></returns>
        private string SetScheduleDate(DateTime date)
        {
            //ログインユーザーのスケジュールを取得
            List<Dictionary<string, object>> scheduleList = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> employeeList = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> loginUserInfo = new List<Dictionary<string, object>>();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserNo", LoginUser.UserNo);
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            userInfo["社員番号"] = LoginUser.UserNo;
            userInfo["社員名称"] = LoginUser.UserName;
            loginUserInfo.Add(userInfo);
            // loginUserInfo = db.ExecuteSQForList(this.Page, "SELECT_LoginUserInfo", param);
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            for (int i = 0; i < 7; i++)
            {
                parameter.Add("Date" + (i + 1).ToString(), date.AddDays(i).ToString("yyyy/MM/dd"));
            }
            parameter.Add("WhereString", " AND sck.社員番号  = " + LoginUser.UserNo.ToString());
            scheduleList = db.ExecuteSQForList(this.Page, "SELECT_Schedule", parameter);
            return CreatSchedulTable(date, loginUserInfo, scheduleList);//渡す
        }

        /// <summary>
        /// リンクボタン遷移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //ToDo確認画面に遷移
                if (e.CommandName == "linkToDo")
                {
                    LinkButton linkToDo = (LinkButton)e.CommandSource;
                    string todoNo = linkToDo.CommandArgument;
                    Session["todoNo"] = todoNo;
                    Response.Redirect("~/ToDo/TodoConfirm.aspx",false);
                }
                //書類編集画面に遷移
                if (e.CommandName == "linkDocument")
                {
                    LinkButton linkDocument = (LinkButton)e.CommandSource;
                    string ApplyID = linkDocument.CommandArgument;
                    Session["applyid"] = ApplyID;
                    Response.Redirect("../Document/DocumentApply.aspx",false);
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }
    }
}