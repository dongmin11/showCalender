using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeManagement.Setting
{
    public partial class DepartmentList : BasePage
    {
        private static string sort = "";//初期ソート
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (LoginUser.Authority != Enumerations.AuthorityEnum.Personnel.Id) //ユーザー権限チェック
                {
                    Response.Redirect("~/TopPage/TopPage.aspx", false);
                }
                if(!IsPostBack)
                {
                    sort = "部署ＩＤ ASC";
                    BindDB();
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        //DB取得
        private void BindDB()
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("SORT", sort);
            gvDeparment.DataSource = db.ExecuteSQLForDataTable(this.Page, "SELECT_DepartmentMst", para);
            gvDeparment.DataBind();
            if (gvDeparment.Rows.Count <= gvDeparment.PageSize)
                gvDeparment.Rows[gvDeparment.Rows.Count - 1].CssClass = "last-row";
        }

        //編集ボダン処理
        protected void gvDeparment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName=="Edit")
            {
                Button button = (Button)e.CommandSource;
                Session["DeparmentID"] = button.CommandArgument;
                Response.Redirect("DepartmentEdit.aspx");
            }
        }

        //ソート処理
        protected void gvDeparment_Sorting(object sender, GridViewSortEventArgs e)
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

        //ページボダン処理
        protected void gvDeparment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDeparment.PageIndex = e.NewPageIndex;
            BindDB();
        }

        //表示件数変更処理
        protected void dropNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDeparment.PageSize = Convert.ToInt32(dropNumber.SelectedValue);
                gvDeparment.PageIndex = 0;
                BindDB();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        //新規追加ボダン
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("DepartmentEdit.aspx");
        }
    }
}