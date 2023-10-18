using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.TodoListStaEnum;
using static EmployeeManagement.Enumerations.TodoKindEnum;

namespace EmployeeManagement.ToDoList
{
     
    public partial class TodoList : BasePage
    {
        
        Dictionary<string, object> para = new Dictionary<string, object>();
        DataTable dt = new DataTable();
        private　static string receiveSta = "";　//受信状態初期化
        private　static string sendSta = "";　　//送信状態初期化
        public static string title = "";        //画面タイトル
        private　static string sortString = "";　//ソート
        private　static bool btnSearch_Press ;  //検索ボダンのクリック状態
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                   
                    btnSearch_Press = false;
                    //受信状態、送信状態をURLから取得
                    receiveSta = Request.QueryString["ReceiveSta"];
                    sendSta = Request.QueryString["SendSta"];
                    sortString = "発信日時 DESC";
                    //データの表示
                    GetDb();
                    //ページ保持
                    if (Session["page"] != null && !string.IsNullOrEmpty(Session["page"].ToString()))
                    {
                        string[] s = Session["page"].ToString().Split(new char[] { ' ' });
                        if (s[1] == title)
                        {
                            GvTodoList.PageIndex = Convert.ToInt32(s[0]);
                        }
                        Session.Remove("page");
                    }
                    ViewState["search"] = txtTitle.Text + txtWriter.Text;
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// paraの追加
        /// </summary>
        /// <param name="title"></param>
        private void Add_Para(string title)
        {
            StringBuilder col = new StringBuilder();
            StringBuilder subtable = new StringBuilder();
            StringBuilder require = new StringBuilder();
            col.AppendLine(", s.社員名称");
            col.AppendLine(", s.社員フラグ");
            subtable.AppendLine("LEFT JOIN");
            subtable.AppendLine("社員マスタ s");
            subtable.AppendLine("ON");
            subtable.AppendLine("s.社員番号 = t.発信者番号");
            if (title== AllReceive.Name||title== Uncomplete.Name)
            {
                //TodoリストSELECT文追加
                col.AppendLine(", m.完了フラグ");
                col.AppendLine(", m.受信者番号");
                col.AppendLine(", m.開封日時");
                col.AppendLine(", m.コメント");
                col.AppendLine(", m.開封フラグ");
                //TodoリストFrom文追加
                subtable.AppendLine("LEFT JOIN");
                subtable.AppendLine("TODO明細 m");
                subtable.AppendLine("ON");
                subtable.AppendLine("m.TODO番号 = t.TODO番号");
                //TodoリストWHERE文追加
                require.AppendLine($"m.受信者番号 = {LoginUser.UserNo}");
                require.AppendLine($"AND t.状態 = {Sent.Id}");
                if( title == Uncomplete.Name)
                {
                    require.AppendLine($"AND m.完了フラグ = {Uncomplete.Id}");
                }
            }
            else
            {
                require.AppendLine($"t.発信者番号 = {LoginUser.UserNo}");
                if(title== Sent.Name)
                {
                    require.AppendLine($"AND t.状態 = {Sent.Id}");
                }
                else
                {
                    require.AppendLine($"AND t.状態 = {SendWaite.Id}");
                }
                
            }
            if (btnSearch_Press == true)
            {
                if (txtTitle.Text != "" && txtWriter.Text != "")
                {
                    require.AppendLine($" AND タイトル LIKE '%{txtTitle.Text.Trim()}%' ");
                    require.AppendLine($" AND 社員名称 LIKE '%{txtWriter.Text.Trim()}%' ");
                }
                else if (txtTitle.Text != "")
                {
                    require.AppendLine("AND t.タイトル LIKE '%" + txtTitle.Text.Trim() + "%'");
                }
                else if (txtWriter.Text != "")
                {
                    require.AppendLine("AND 社員名称 LIKE '%" + txtWriter.Text.Trim() + "%'");
                }
                
            }
            require.AppendLine("AND 社員フラグ=1");
            require.AppendLine($"ORDER BY {sortString}");
            para.Add("col", col);
            para.Add("subtable", subtable);
            para.Add("require", require);
        }

        /// <summary>
        /// 一覧データ取得・画面タイトルの変更
        /// </summary>
        protected void GetDb()
        {

            if (receiveSta == Uncomplete.Id.ToString())
            {
                title = Uncomplete.Name;
            }
            else if (receiveSta == AllReceive.Id.ToString())
            {
                title = AllReceive.Name;
            }
            else if (sendSta == Sent.Id.ToString())
            {
                title = Sent.Name;
            }
            else if (sendSta == SendWaite.Id.ToString())
            {
                title = SendWaite.Name;
            }
            else
            {
                title = AllReceive.Name;
            }
            Add_Para(title);
            dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_TodoList", para);
            if (title == Sent.Name || title == SendWaite.Name)
            {
                dt.Columns.Add("完了フラグ");
                dt.Columns.Add("開封日時");
                dt.Columns.Add("コメント");
                GvTodoList.Columns[6].Visible = false;
                GvTodoList.Columns[3].HeaderStyle.CssClass = "a";
            }
            GvTodoList.ShowHeaderWhenEmpty = true;
            GvTodoList.DataSource = dt;
            GvTodoList.DataBind();
            if (GvTodoList.Rows.Count <= GvTodoList.PageSize)
                GvTodoList.Rows[GvTodoList.Rows.Count - 1].CssClass = "last-row";
        }

        /// <summary>
        /// 検索ボダン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnSearch_Press = true;
                GvTodoList.PageIndex = 0;
                GetDb();
               

            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 一覧表表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvTodoList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //種別カラム表示処理
                    if (e.Row.Cells[1].Text == Normal.Id.ToString())
                    {
                        e.Row.Cells[1].Text = Normal.ShortName;
                    }
                    else if (e.Row.Cells[1].Text == Communicate.Id.ToString())
                    {
                        e.Row.Cells[1].Text = Communicate.ShortName;
                    }
                    else if (e.Row.Cells[1].Text == Share.Id.ToString())
                    {
                        e.Row.Cells[1].Text = Share.Name;
                    }
                    else
                    {
                        e.Row.Cells[1].Text = GetTodoKindName(e.Row.Cells[1].Text);
                    }
                    //e.Row.Cells[1].Text = GetTodoKindName(e.Row.Cells[1].Text);
                    //状況カラム表示処理
                    para.Add("todoNo", ((HiddenField)e.Row.Cells[0].FindControl("TodoID")).Value);
                    dt = db.ExecuteSQLForDataTable(this.Page, "Select_TodoDetail", para);
                    int a = dt.Rows.Count;
                    DataRow[] drws = dt.Select("完了フラグ =" + 1);
                    int b = drws.Length;
                    e.Row.Cells[2].Text = b + "/" + a;
                    //コメントカラム表示処理
                    DataRow[] cmt = dt.Select("コメント Is Not Null And コメント <> ''");
                    int c = cmt.Length;
                    e.Row.Cells[7].Text = c.ToString();
                    para.Remove("todoNo");
                    //タイトル行の添付ファイルアイコン表示
                    string files = ((HiddenField)e.Row.Cells[0].FindControl("Files")).Value;
                    if(files!=""&& files!=null&&title!=SendWaite.Name)
                    {
                        e.Row.FindControl("image").Visible = true;
                    }
                }
            }
            catch(Exception x)
            {
                AppLog.Error(x.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// Todoリスト　タイトルセルのクリック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvTodoList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "GoConfirm")
                {
                    LinkButton lnkView = (LinkButton)e.CommandSource;
                    string todoNo = lnkView.CommandArgument;
                    Session["todoNo"] = todoNo;
                    Session["page"] = GvTodoList.PageIndex.ToString() +" "+ title;
                    if (title == SendWaite.Name)
                    {
                        Response.Redirect("TodoEdit.aspx",false);
                    }
                    else
                    {
                        if (title == Sent.Name)
                        {
                            Session["sent"] = Sent.Name;
                        }
                        Response.Redirect("TodoConfirm.aspx",false);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// Todoリスト(ソート処理）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvTodoList_Sorting(object sender, GridViewSortEventArgs e)
        {
            string[] s = sortString.Split(new char[] { ' ' });　//最後のsortを読込
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
            sortString = sSort;
            //データの表示
            GetDb();
            
        }

        /// <summary>
        /// Todo一覧GridView ページング処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvTodoList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvTodoList.PageIndex = e.NewPageIndex;
            GetDb();
        }
    }
}