using EmployeeManagement.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace EmployeeManagement.Setting
{
    public partial class UserList : BasePage
    {
        //初期値
        static string searchWord = "0 = 0";
        static string emploFlag = "";
        static DataTable dtUser;
        /// <summary>
        /// 画面ロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //管理者以外場合
                if (LoginUser.Authority != AuthorityEnum.Personnel.Id)
                {
                    Response.Redirect("../Login.aspx");
                }

                if (!IsPostBack)
                {
                    //ソート初期設定
                    this.ViewState["sort"] = "社員番号 ASC";
                    this.GvBind();
                    //Dropの値はグリッドのページサイズ
                    KennsuList.Text = GvUseList.PageSize.ToString();
                }
                GvUseList.DataSource = dtUser;
                GvUseList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// テーブル処理
        /// </summary>
        protected void GvBind()
        {
            try
            {
                //メモ:データ量に応じてSELECTする場合はTOPまたはLIMIT(件数、開始位置)OFFSETで開始位置のしても可
                //
                //データの取得
                Dictionary<string, object> param = new Dictionary<string, object>();
                if (CheckBoxDelete.Checked != false)
                {
                    emploFlag = "";
                }
                else
                {
                    emploFlag = " AND u.社員フラグ IN(1, 2, 9)";
                }
                param.Add("SEARCH", formatUtil.characterConvert(searchWord) + emploFlag);
                param.Add("Sort", ViewState["sort"]);
                dtUser = db.ExecuteSQLForDataTable(this.Page, "SELECT_UserList", param);
                //権限名の変換
                dtUser.Columns.Add("権限名");
                foreach(DataRow row in dtUser.Rows)
                {
                    if ((int)row["人事権限"] == AuthorityEnum.General.Id)
                    {
                        row["権限名"] = AuthorityEnum.General.Name;
                    }
                    else if ((int)row["人事権限"] == AuthorityEnum.Personnel.Id)
                    {
                        row["権限名"] = AuthorityEnum.Personnel.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 新規追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnUserList_Click(object sender, EventArgs e)
        {
            Response.Redirect("./UserEdit.aspx");
        }
        /// <summary>
        /// 表示件数変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void KennsuList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GvUseList.PageSize = Convert.ToInt32(KennsuList.SelectedValue);
                GvUseList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 検索ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            //検索が空欄かどうかの判定後、検索
            if (!string.IsNullOrEmpty(TxtSearch.Text.Trim()))
            {
                searchWord = $"u.ユーザー LIKE '%{formatUtil.characterConvert(TxtSearch.Text)}%'";
            }
            else
            {
                searchWord = "0 = 0";
            }
            GvBind();
            GvUseList.DataSource = dtUser;
            this.GvUseList.DataBind();
        }
        /// <summary>
        /// 詳細ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvUseList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnDetail")
            {
                this.Session["社員番号"] = GvUseList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();
                string test = GvUseList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();
                Response.Redirect("./UserEdit.aspx");
            }
        }
        /// <summary>
        /// データ内ページ遷移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvUseList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GvUseList.PageIndex = e.NewPageIndex;
            this.GvUseList.DataBind();
        }
        /// <summary>
        /// ソート機能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvUseList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string[] str_Sort = ViewState["sort"].ToString().Split(new char[] { ' ' }); //最後のsortを読込
                string sSort = e.SortExpression;
                //ASC　DESCの変更まだ追加
                if (str_Sort[0] == sSort)
                {
                    if (str_Sort[1] == "ASC")
                    {
                        sSort += " DESC";
                    }
                    else
                    {
                        sSort += " ASC";
                    }
                }
                else
                {
                    sSort += " ASC";
                }
                //新しいsortを保存する
                ViewState["sort"] = sSort;
                //DataTableのソート処理
                DataTable sortDt = dtUser.Clone();
                DataRow[] userRows = dtUser.Select("", sSort);
                foreach (var row in userRows)
                {
                    sortDt.ImportRow(row);
                }
                GvUseList.DataSource = sortDt;
                GvUseList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}