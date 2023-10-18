using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace EmployeeManagement.Setting
{
    public partial class PapersList : BasePage
    {
        private static string sort = "";//初期ソート
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    if (LoginUser.Authority != Enumerations.AuthorityEnum.Personnel.Id)
                    {
                        Response.Redirect("~/TopPage/TopPage.aspx", false);
                    }
                    sort = "SEQ ASC";
                    BindDB();
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// Db取得
        /// </summary>
        protected void BindDB()
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("SORT", sort);
            gvPapers.DataSource = db.ExecuteSQLForDataTable(this.Page, "SELECT_PAPERS", para);
            gvPapers.DataBind();
            if (gvPapers.Rows.Count <= gvPapers.PageSize)
                gvPapers.Rows[gvPapers.Rows.Count - 1].CssClass = "last-row";
        }

        /// <summary>
        /// 新規追加ボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("./PapersEdit.aspx");
        }

        /// <summary>
        /// 編集ボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPapers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Button btnView = (Button)e.CommandSource;
                Session["paperID"] = btnView.CommandArgument;
                Response.Redirect("./PapersEdit.aspx");
            }
        }

        /// <summary>
        /// ページボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPapers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPapers.PageIndex = e.NewPageIndex;
            BindDB();
        }

        /// <summary>
        /// 行数変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvPapers.PageSize = Convert.ToInt32(dropNumber.SelectedValue);
                BindDB();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// ソート処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPapers_Sorting(object sender, GridViewSortEventArgs e)
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
                BindDB();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }
    }
}