using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.DocumentStaEnum;
using System.Configuration;

namespace EmployeeManagement.Document
{
    public partial class DocumentList : BasePage
    {
        private static string sort = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    
                    sort = "申請日　DESC";
                    ListItem listItem = new ListItem("", "");
                    ListItem unapproved = new ListItem(Unapproved.Name, Unapproved.Id.ToString());
                    ListItem remand = new ListItem(Remand.Name, Remand.Id.ToString());
                    ListItem making = new ListItem(Making.Name, Making.Id.ToString());
                    ListItem waittake = new ListItem(WaitTake.Name, WaitTake.Id.ToString());
                    ListItem completed = new ListItem(Completed.Name, Completed.Id.ToString());
                    ListItem delete = new ListItem(Delete.Name, Delete.Id.ToString());
                    ListItem[] listItems = { listItem, unapproved, remand, making, waittake, completed, delete };
                    dropStatus.Items.AddRange(listItems);
                    Bind_GV();
                    dropNumber.SelectedValue = gvDcList.PageSize.ToString();
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// グリッドビュー　データバインド
        /// </summary>
        protected void Bind_GV()
        {
            GetParam();
            DataTable dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_DocumentList", GetParam());
            gvDcList.ShowHeaderWhenEmpty = true;
            gvDcList.DataSource = dt;
            gvDcList.DataBind();
            if (gvDcList.Rows.Count <= gvDcList.PageSize)
                gvDcList.Rows[gvDcList.Rows.Count - 1].CssClass = "last-row";
        }

        /// <summary>
        /// paramの取得
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string,object> GetParam()
        {
            Dictionary<string,object > para=new Dictionary<string, object>();
            StringBuilder require = new StringBuilder();
            
            //ログインユーザーの所属は管理部じゃない＆＆権限は人事権限じゃない
            if (LoginUser.SyozokuID.ToString() != ConfigurationManager.AppSettings["ManagementID"]&&LoginUser.Authority!= Enumerations.AuthorityEnum.Personnel.Id)
            {
                dropStatus.Items.Remove(dropStatus.Items.FindByValue(Delete.Id.ToString()));
                require.AppendLine("WHERE");
                require.AppendLine($"  申請者No = {LoginUser.UserNo}");
                require.AppendLine($"  AND　ステータス <> 9");
                if (!string.IsNullOrEmpty(dropStatus.SelectedValue))
                {
                    require.AppendLine($"  AND ステータス = {dropStatus.SelectedValue}");
                }
                gvDcList.Columns[1].Visible = false;
            }
            
            else
            {
                if (!string.IsNullOrEmpty(dropStatus.SelectedValue))
                {
                    require.AppendLine($"WHERE ステータス　= {dropStatus.SelectedValue}");
                }
            }
            require.AppendLine($"ORDER BY {sort}");
            para.Add("require", require);
            return para;
        }

        /// <summary>
        /// グリッドビュー　セル　テキスト　変更処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDcList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //ステータス表示変更
                    if (e.Row.Cells[5].Text == Unapproved.Id.ToString())
                    {
                        e.Row.Cells[5].Text = Unapproved.Name;
                    }
                    else if (e.Row.Cells[5].Text == Remand.Id.ToString())
                    {
                        e.Row.Cells[5].Text = Remand.Name;
                    }
                    else if (e.Row.Cells[5].Text == Making.Id.ToString())
                    {
                        e.Row.Cells[5].Text = Making.Name;
                    }
                    else if (e.Row.Cells[5].Text == WaitTake.Id.ToString())
                    {
                        e.Row.Cells[5].Text = WaitTake.Name;
                    }
                    else if (e.Row.Cells[5].Text == Completed.Id.ToString())
                    {
                        e.Row.Cells[5].Text = Completed.Name;
                    }
                    else
                    {
                        e.Row.Cells[5].Text = Delete.Name;
                    }
                    
                    //書類名称表示変更
                    if (e.Row.Cells[2].Text.IndexOf(",") >0)
                    {
                        e.Row.Cells[2].Text = GetDocumentName(e.Row.Cells[2].Text.Split(',')[0]) + " など";
                    }
                    else
                    {
                        e.Row.Cells[2].Text = GetDocumentName(e.Row.Cells[2].Text);
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
        /// グリッドビュー表示行数変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDcList.PageIndex = 0;
                int number = Convert.ToInt32(dropNumber.SelectedValue);
                gvDcList.PageSize = number;
                Bind_GV();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// ステータス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Bind_GV();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// グリッドビュー　ページング処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDcList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDcList.PageIndex = e.NewPageIndex;
                Bind_GV();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// グリッドビュー　ソート処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDcList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string[] s = sort.Split(new char[] { ' ' }); //最後のsortを読込
                string sSort = e.SortExpression;
                //ASC　DESCの変更まだ追加
                if (s[0] == sSort)
                {
                    if (s[1] == "ASC")
                        sSort += " DESC";
                    else
                        sSort += " ASC";
                }
                else
                {
                    sSort += " ASC";
                }
                sort = sSort;
                //データの表示
                Bind_GV();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 新規追加ボダン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("DocumentApply.aspx");
        }

        /// <summary>
        /// 詳細ボダン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDcList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if(e.CommandName=="Detail")
                {
                    Button btnView = (Button)e.CommandSource;
                    string ApplyID = btnView.CommandArgument;
                    Session["applyid"] = ApplyID;
                    Response.Redirect("DocumentApply.aspx",false);
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