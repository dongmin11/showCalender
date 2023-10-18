using EmployeeManagement.Common.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using static EmployeeManagement.Common.CommonUtil;
using static EmployeeManagement.Enumerations.TdodoAttributeEnum;
using static EmployeeManagement.Enumerations.TodoKindEnum;
using static EmployeeManagement.Enumerations.TodoListStaEnum;

namespace EmployeeManagement.ToDo
{
    public partial class TodoEdit : BasePage
    {
        Dictionary<string, object> para = new Dictionary<string, object>();
        private static DataTable ReceiverDt; 　//受信者データテーブル初期化　　　　　　　　　　
        private static string attribute ;      //属性
        private static string timenow ;　　　　//現在時間
        public static string TodoEditTitle ; //タイトル初期化
        private static string todoNo;//ToDo番号初期化
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (!IsPostBack)
                {
                    
                    ReceiverDt = new DataTable();
                    if (ReceiverDt.Columns.Count==0)
                    {
                        //宛名先データテーブルカラムの追加
                        ReceiverDt.Columns.Add("社員番号");
                        ReceiverDt.Columns.Add("社員名称");
                        ReceiverDt.Columns.Add("部署名");
                    }
                    DplItem_Show();
                    if (Session["todoNo"] != null && !string.IsNullOrEmpty(Session["todoNo"].ToString()))
                    {
                        //TODO番号取得
                        todoNo = Session["todoNo"].ToString();
                        Session.Remove("todoNo");
                        TodoEditTitle = "TODO編集";
                        para = GetInfo();
                         //todoデータ取得
                        DataTable dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_TodoList", para);
                        //ToDo明細取得（受信者）取得
                        DataTable dataTable = db.ExecuteSQLForDataTable(this.Page, "Select_TodoDetail", para);
                        DataRow dr = dt.Rows[0];
                        //データ表示
                        Check_Show(dr); 　　　　　　　　　　　　　　　　　　//属性チェック表示
                        txtTile.Text = dr["タイトル"].ToString();　　　　　//タイトル表示　
                        txtDetail.Text = dr["本文"].ToString();　　　　　　//本文表示　
                        dropKind.SelectedValue = dr["種別"].ToString();　　//種別表示　
                        foreach (DataRow row in dataTable.Rows)
                        {
                            ReceiverDt.Rows.Add(row["受信者番号"].ToString(), row["社員名称"].ToString(), "[" + row["部署名"].ToString() + "]　");
                        }
                        RecieverGrid.DataSource = ReceiverDt;　　　　　　　//受信者表示
                        RecieverGrid.DataBind();
                    }
                    else
                    {
                        TodoEditTitle = "新規作成";
                        todoNo = "";
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
        /// ドロップダウンリストアイテム表示
        /// </summary>
        private void DplItem_Show()
        {
            //部門ドロップダウンリストデータバインド
            dropDepartment.DataSource = db.ExecuteSQLForDataTable(this.Page, "SELECT_DepartMent");
            dropDepartment.DataBind();
            dropDepartment.AppendDataBoundItems = true;
            dropDepartment.Items.Insert(0, new ListItem("部門を選択してください", "0"));
            dropKind.DataSource= GetNameMst().Where(x => x["分類コード"].ToString().Equals("E0001")).Select(x => new {
                名称 = x["名称"],
                SEQ = x["SEQ"]
            }).ToList();
            dropKind.DataBind();
        }

        /// <summary>
        /// 属性チェックの表示
        /// </summary>
        /// <param name="dr"></param>
        private void Check_Show(DataRow dr)
        {
            if (dr["属性"].ToString().Contains(Important.Id.ToString()))
            {
                ck1.Checked = true;
            }
            if (dr["属性"].ToString().Contains(Urgent.Id.ToString()))
            {
                ck2.Checked = true;
            }
            if (dr["属性"].ToString().Contains(Confidential.Id.ToString()))
            {
                ck3.Checked = true;
            }
            if (dr["属性"].ToString().Contains(Transfer.Id.ToString()))
            {
                ck4.Checked = true;
            }
        }

        /// <summary>
        /// paraの追加
        /// </summary>
        private Dictionary<string, object> GetInfo()
        {
            StringBuilder require = new StringBuilder();
            require.AppendLine($"t.[発信者番号] = {LoginUser.UserNo}");
            timenow =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            para.Add("todoSta",SendWaite.Id);     //送信状態
            //入力内容の取得
            para.Add("userid", LoginUser.UserNo);　
            para.Add("kind", dropKind.SelectedValue);
            para.Add("title", ChangeString(txtTile.Text));
            para.Add("detail",ChangeString(txtDetail.Text));
            para.Add("file1", ChangeString(FileUpload1.FileName));
            para.Add("file2", ChangeString(FileUpload2.FileName));
            para.Add("file3", ChangeString(FileUpload3.FileName));
            if (IsPostBack)
            {
                para.Add("timeNow",  timenow );
                para.Add("userid1", LoginUser.UserNo);
                para.Add("timeNow1",  timenow );
                para.Add("userid2", LoginUser.UserNo);
                para.Add("timeNow2",  timenow );
                para.Add("attribute",  attribute );
                para.Add("sta", 0);
                require.AppendLine($" AND [発信日時] = '{timenow}'");
            }
            else
            {
                require.AppendLine($" AND [TODO番号] =  {todoNo}  ");
            }
            if (!string.IsNullOrEmpty(todoNo))
            {
                para.Add("todoNo", todoNo);
            }
            para.Add("col", "");
            para.Add("subtable", "");
            para.Add("require", require);
            return para;
        }

        //Todo明細INSERT
        protected void Recerver_Insert()
        {
            foreach (DataRow dr in ReceiverDt.Rows)
            {
                para.Add("receiverNo", dr["社員番号"].ToString());
                db.ExecuteNonQuerySQL(this.Page, "INSERT_ToDoDetail", para);
                para.Remove("receiverNo");
            }
        }

        /// <summary>
        /// 部門ドロップダウンリスト選択肢変更処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dropDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("userid", dropDepartment.SelectedValue);
                MemberGrid.DataSource = db.ExecuteSQLForDataTable(this.Page, "Select_UN_OfDpt", para);
                MemberGrid.DataBind();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 社員・宛名先一覧表のクリック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                PostBackOptions myPostBackOptions = new PostBackOptions(this);
                myPostBackOptions.AutoPostBack = false;
                myPostBackOptions.RequiresJavaScriptProtocol = true;
                myPostBackOptions.PerformValidation = false;
                String evt = Page.ClientScript.GetPostBackClientHyperlink(sender as GridView, "Select$" + e.Row.RowIndex.ToString());
                e.Row.Attributes.Add("onclick", evt);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 宛名先追加・削除ボダン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecerverEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //宛名先追加ボダン処理
                if (((LinkButton)sender).ID == "btnAdd")
                {
                    foreach (GridViewRow row  in MemberGrid.Rows)
                    {
                        CheckBox chkUser = (CheckBox)row.FindControl("chkUser");
                        if (chkUser.Checked)
                        {
                            DataRow[] drws = ReceiverDt.Select("社員名称 = '"+ row.Cells[1].Text + "'").Where(x => x.Field<string>("部署名") == "["+dropDepartment.SelectedItem.Text+"]　").ToArray();
                            if (drws.Length == 0)
                            {
                                string x = ((HiddenField)row.Cells[0].FindControl("社員番号")).Value;
                                ReceiverDt.Rows.Add(x, row.Cells[1].Text, "[" + dropDepartment.SelectedItem.Text + "]　");
                            }
                        }
                    }
                }
                //宛名先削除ボダン処理
                else
                {
                    foreach (GridViewRow row in RecieverGrid.Rows)
                    {
                        CheckBox chkReceiver = (CheckBox)row.FindControl("chkReceiver");
                        if (chkReceiver.Checked)
                        {
                            for (int i = ReceiverDt.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = ReceiverDt.Rows[i];
                                if (dr["社員名称"].ToString() == row.Cells[1].Text&&dr["部署名"].ToString()==row.Cells[0].Text)
                                    dr.Delete();
                            }
                            ReceiverDt.AcceptChanges();
                        }
                    }
                }
                RecieverGrid.DataSource = ReceiverDt;
                RecieverGrid.DataBind();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 社員・宛名先一覧表選択処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        { 
            try
            {
                
                if (((GridView)sender).ID == "MemberGrid")　//社員一覧表
                {
                    GridViewRow rowUser = MemberGrid.Rows[e.NewSelectedIndex];
                    CheckBox chkUser = (CheckBox)rowUser.FindControl("chkUser");
                    if (chkUser.Checked)
                    {
                        chkUser.Checked = false;
                        rowUser.CssClass = "";
                    }
                    else
                    {
                        chkUser.Checked = true;
                        rowUser.CssClass = "row-selected";
                    }
                }
                else　//宛名先一覧表
                {
                    GridViewRow rowReceiver = RecieverGrid.Rows[e.NewSelectedIndex];
                    CheckBox chkReceiver = (CheckBox)rowReceiver.FindControl("chkReceiver");
                    if (chkReceiver.Checked)
                    {
                        chkReceiver.Checked = false;
                        rowReceiver.CssClass = "";
                    }
                    else
                    {
                        chkReceiver.Checked = true;
                        rowReceiver.CssClass = "row-selected";
                    }
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        //キャンセルボダン処理
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("TodoList.aspx?ReceiveSta=1");　//一覧画面に戻る
        }

        /// <summary>
        /// 送信ボダン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                attribute = null;
                //宛名先、タイトル、本文の入力判定
                bool entError = false;
                List<FileUpload> fu = new List<FileUpload>();
                fu.Add(FileUpload1);
                fu.Add(FileUpload2);
                fu.Add(FileUpload3);
                if (ReceiverDt.Rows.Count == 0)                   //宛名先チェック
                {
                    lbRecerver.Text = GetMessage("EmptyErrorMes");
                    entError = true;
                }
                if (string.IsNullOrEmpty(txtTile.Text))        　//タイトルチェック
                {
                    lbTile.Text = GetMessage("EmptyErrorMes");
                    entError = true;
                }
                if (string.IsNullOrEmpty(txtDetail.Text))　　　　//本文チェック
                {
                    lbDetail.Text = GetMessage("EmptyErrorMes");
                    entError = true;
                }
                foreach (FileUpload fileUpload in fu)　　　　　//ファイル容量チェック
                {
                    if (fileUpload.FileBytes.Length > Convert.ToInt32(ConfigurationManager.AppSettings["UploadFileLimit"])) //ファイル容量判定
                    {
                        lbFileupload.ForeColor = Color.Red;
                        entError = true;
                    }
                }
                //エラー判定
                if (entError == true)
                {
                    Master.ModalPopup(GetMessage("TxtErrorMes"));
                    return;
                }
                else
                {
                    lbRecerver.Text = "";
                    lbTile.Text = "";
                    lbDetail.Text = "";
                    lbFileupload.ForeColor = Color.Black;
                    //添付ファイルあるかどうかチェック
                    if (!string.IsNullOrEmpty(FileUpload1.PostedFile.FileName) || 
                        !string.IsNullOrEmpty(FileUpload2.PostedFile.FileName) || 
                        !string.IsNullOrEmpty(FileUpload3.PostedFile.FileName))
                    { 
                        //pathの保存
                        string path = UploadFile.tmppath;
                        UploadFile.TmpFileSave(fu, path + "\\");
                        Session["pathNow"] = path;
                    }
                }
                //宛名先の保存
                Session["dtReceiver"] = ReceiverDt;
                //属性チェックボックスのチェック判定
                if (ck1.Checked)
                {
                    attribute += Important.Id.ToString();
                }
                if (ck2.Checked)
                {
                    attribute += Urgent.Id.ToString();
                }
                if (ck3.Checked)
                {
                    attribute += Confidential.Id.ToString();
                }
                if (ck4.Checked)
                {
                    attribute += Transfer.Id.ToString();
                }
                 para = GetInfo();
                //ToDoリストINSERT
                if (string.IsNullOrEmpty(todoNo))
                {
                    //Todoリストへ新規作成INSERT
                    db.ExecuteNonQuerySQL(this.Page, "INSERT_ToDolist", para);
                    //ToDo番号取得
                    DataTable dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_TodoList", para);//ToDoデータ取得
                    DataRow dataRow = dt.Rows[0];
                    todoNo = dataRow["ToDo番号"].ToString();
                    para.Add("todoNo", todoNo);
                }
                //Todoリスト更新
                else
                {
                    db.ExecuteNonQuerySQL(this.Page, "UPDATE_ToDoLiat(After_Edit)", para);
                    db.ExecuteNonQuerySQL(this.Page, "DELETE_ToDoDetail", para);
                }
                //todo明細のINSERT
                Recerver_Insert();
                //遷移為、セッションに入力内容を保存
                var strList = new List<string>();
                strList.Add(dropKind.SelectedItem.Text);
                strList.Add(attribute);
                strList.Add(txtTile.Text);
                strList.Add(txtDetail.Text);
                strList.Add(timenow);
                strList.Add(todoNo);
                Session["Send"] = strList;
                Response.Redirect("TodoConfirm.aspx",false); //メッセージ発信画面に遷移する
            }
            
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }
    }
}