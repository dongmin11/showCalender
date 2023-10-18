using EmployeeManagement.Common.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.TdodoAttributeEnum;



namespace EmployeeManagement.ToDo
{
    public partial class TodoConfirm : BasePage
    {
        public static string ConfirmTitle = "";　//画面タイトル
        Dictionary<string, object> para = new Dictionary<string, object>();
        private static string savetime = "";//ToDo一時保存時間
        private static string todoNo = "";//todo番号
        private static string sendtime; 　//送信時間
        private static string path = "";　//フォルダパス

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var strList = new List<string>();　//データ取得用リスト
                if (!IsPostBack)
                {
                    //セッションから入力内容を取得
                    if (Session["Send"] != null )
                    {
                        p2.Visible = false;
                        p3.Visible = false;
                        ConfirmTitle = "メッセージ発信確認";
                        receiverGrid.ShowHeaderWhenEmpty = true;
                        strList = (List<string>)Session["Send"];
                        //添付ファイルを取得
                        if (Session["pathNow"] != null)
                        {
                            path = Session["pathNow"].ToString();
                            string[] filePaths = Directory.GetFiles(path + "\\");
                            File_Show(filePaths);　　　//ファイル表示
                        }
                        //データ表示
                        lbKind.Text = strList[0]; 　　　//種別表示
                        txtTile.Text = strList[2];　　　//タイトル表示
                        txtDetail.Text = strList[3];　　//本文の表示
                        savetime = strList[4]; 　　　　//ToDo一時保存時間取得
                        todoNo = strList[5];　　　　　　//ToDo番号取得
                        DataTable rcdt = db.ExecuteSQLForDataTable(this.Page, "Select_TodoDetail", Param());
                        receiverGrid.DataSource = rcdt; //受信者表示
                        receiverGrid.DataBind();
                        //para = Param();
                        Session.Remove("Send");
                        Session.Remove("dtReceiver");
                        Session.Remove("pathNow");
                    }
                    else
                    {
                        if(Request.UrlReferrer!=null)
                        {
                            ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                        }
                        p1.Visible = false;
                        lbText.Visible = false;
                        ConfirmTitle = "メッセージ確認";
                        //todo番号取得
                        if (Session["todoNo"] != null && !string.IsNullOrEmpty(Session["todoNo"].ToString()))
                        {
                            todoNo = Session["todoNo"].ToString();
                            Session.Remove("todoNo");
                        }
                        para = Param();
                        //todoデータ取得
                        DataTable dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_TodoList", para);
                        //var a = db.ExecuteSQForList(this.Page, "SELECT_TodoList", para);
                        //ToDo明細取得（受信者）取得
                        DataTable dataTable = db.ExecuteSQLForDataTable(this.Page, "Select_TodoDetail", para);
                        DataRow dr = dt.Rows[0];
                        strList.Add(dr["種別"].ToString());
                        strList.Add(dr["属性"].ToString());
                        //データ表示
                        lbKind.Text = GetTodoKindName(strList[0]);　　　　　　　　　　　//種別表示
                        txtTile.Text = dr["タイトル"].ToString();　　　　　　　        //タイトル表示
                        CmtShow(dataTable);　　　　　　　　　　　　　　　　　　       //コメント表示
                        txtDetail.Text = dr["本文"].ToString();                        //本文表示
                        path = UploadFile.upladedpath + "\\" + "TodoNO_" + todoNo + "\\";
                        if (Directory.Exists(path))　　　　　　　　　　　　　　        //添付ファイル表示
                        {
                            string[] filePaths = Directory.GetFiles(path);
                            File_Show(filePaths);
                        }
                        receiverGrid.DataSource = dataTable;　　　　　　　　　  　　　 //受信者表示　
                        receiverGrid.DataBind();　
                        db.ExecuteNonQuerySQL(this.Page, "UPDATE_TodoDetail1", para);　//開封日更新
                    }
                   　　　　　　　　　                       　　　　　　
                }
                if (strList.Count == 0)
                {
                    strList = (List<string>)ViewState["strList"];
                }
                Attribute_Show(strList);                                              //属性表示　
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// コメント表示
        /// </summary>
        /// <param name="dataTable"></param>
        private void CmtShow(DataTable dataTable )
        {
            //送信済み一覧から遷移来た時
            if (Session["sent"] != null && !string.IsNullOrEmpty(Session["sent"].ToString()))
            {
                btnConfirm.Visible = false;　//確認すボダン非表示
                p3.Visible = false;　　　　　//コメント部分非表示
            }
            else
            {
                //コメントの表示
                DataRow[] dataRow = dataTable.Select($"受信者番号 = {LoginUser.UserNo}");
                txtComment.Text = dataRow[0]["コメント"].ToString();
                if (dataRow[0]["完了フラグ"].ToString() == "1")
                {
                    ckComplete.Checked = true;　　　//完了チェックの表示
                }
            }
            Session.Remove("sent");
        }

        /// <summary>
        /// paraの追加
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> Param()
        {
            sendtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//送信日時（現在日時）
            StringBuilder require = new StringBuilder();
            para.Add("rsta", 1);　　　　　　　　　　　　　　　　//開封フラグ
            para.Add("savetime", "'" + savetime + "'");　　　　
            para.Add("sendtime", "'" + sendtime + "'");　　　　
            para.Add("userid1", LoginUser.UserNo);
            para.Add("sendtime1", "'" + sendtime + "'");
            para.Add("userid", LoginUser.UserNo);　
            para.Add("todoNo", todoNo);
            para.Add("cmt", "'" + txtComment.Text + "'");　　　//コメント
            if(!IsPostBack)
            {
                require.AppendLine($"t.TODO番号={todoNo}");
            }
            else
            {
                para.Add("sta", 0);　　　　　　　　　　　　　　　　
                para.Add("timeNow", "'" + savetime + "'");
            }
            para.Add("col", "");
            para.Add("subtable", "");
            para.Add("require", require);
            if(ckComplete.Checked)　//完了状態取得
            {
                para.Add("csta",1);
            }
            else
            {
                para.Add("csta",0);
            }
            return para;
        }

        /// <summary>
        /// 添付フィイル表示
        /// </summary>
        /// <param name="filePaths"></param>
        protected void File_Show(string[] filePaths )
        {
            try
            {
                DataTable filedt = new DataTable();
                filedt.Columns.Add("Text");　　//ファイル名
                filedt.Columns.Add("Value");　 //ファイルパス
                filedt.Columns.Add("Byte");    //ファイルバイト
                foreach (string filePath in filePaths)
                {
                    FileInfo file = new FileInfo(filePath);
                    string size = "(" + file.Length.ToString() + "バイト)";

                    string[] fileName = Path.GetFileName(filePath).Split('_');
                    filedt.Rows.Add(fileName[1], filePath, size);
                }
                fileGrid.DataSource = filedt;
                fileGrid.DataBind();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 属性表示
        /// </summary>
        /// <param name="strList"></param>
        protected void Attribute_Show(List<string> strList)
        {
            if (strList!=null&&!string.IsNullOrEmpty(strList[1]))
            {
                if (strList[1].Contains(Important.Id.ToString()))
                {
                    Label lb1 = new Label();
                    lb1.Text = Important.Name;
                    lb1.ForeColor = Color.Red;
                    panel1.Controls.Add(lb1);
                }
                if (strList[1].Contains(Urgent.Id.ToString()))
                {
                    Label lb2 = new Label();
                    lb2.Text = Urgent.Name;
                    lb2.ForeColor = Color.Orange;
                    panel1.Controls.Add(lb2);
                }
                if (strList[1].Contains(Confidential.Id.ToString()))
                {
                    Label lb3 = new Label();
                    lb3.Text = Confidential.Name;
                    lb3.ForeColor = Color.Blue;
                    panel1.Controls.Add(lb3);
                }
                if (strList[1].Contains(Transfer.Id.ToString()))
                {
                    Label lb4 = new Label();
                    lb4.Text = Transfer.Name;
                    lb4.ForeColor = Color.Black;
                    panel1.Controls.Add(lb4);
                }
                ViewState["strList"] = strList;
            }
            
        }

        /// <summary>
        /// ファイルウンロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            string[] fileName = Path.GetFileName(filePath).Split('_');
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName[1]);
            Response.WriteFile(filePath);
            Response.End();
        }

        /// <summary>
        /// 送信ボダン・確認ボダン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //確認ボダン
                if (((Button)sender).ID == "btnConfirm")
                {
                    para = Param();
                    db.ExecuteNonQuerySQL(this.Page, "UPDATE_TodoDetail2", para);
                    DataTable dataTable = db.ExecuteSQLForDataTable(this.Page, "Select_TodoDetail", para);
                    receiverGrid.DataSource = dataTable;               //受信者表示　
                    receiverGrid.DataBind();
                }
                //送信ボダン
                else
                {
                    para = Param();
                    //Todoリスト送信状態更新
                    db.ExecuteNonQuerySQL(this.Page, "UPDATE_ToDoList(SendSta)", para);
                    //添付ファイル移動
                    if(Directory.Exists(path))
                    {
                        Directory.Move(path + "\\", UploadFile.upladedpath+"\\"+"TodoNO_"+todoNo+"\\");
                    }
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                        "insertmes",
                        "<script language=\"JavaScript\">window.alert('送信しました');"
                        + "window.location.href = './TodoList.aspx';</script>");
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 編集ボダンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                UploadFile.FileDelete(path);
                Session["todoNo"] = todoNo;
                Response.Redirect("TodoEdit.aspx",false);
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 戻る・キャンセルボダンの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //戻るボタン
                if (((Button)sender).ID == "btnReturn")
                {
                    string refUrl = ViewState["RefUrl"].ToString();
                    Response.Redirect(refUrl, false);
                }
                //キャンセルボダン
                else
                {
                    para = Param();
                    UploadFile.FileDelete(path);
                    //todo削除
                    db.ExecuteNonQuerySQL(this.Page, "DELETE_TodoList", para);
                    //todo明細削除
                    db.ExecuteNonQuerySQL(this.Page, "DELETE_ToDoDetail", para);
                    Response.Redirect("TodoList.aspx?SendSta=0",false);
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
                
        }

        /// <summary>
        /// 宛名先一覧表表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void receiverGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //完了カラム表示処理
                    if(e.Row.Cells[3].Text=="1")　
                    {
                        e.Row.Cells[3].Text = "完了";
                    }
                    else
                    {
                        e.Row.Cells[3].Text = " ";
                    }
                    //コメントに文字がない場合、コメントカラムに更新日を表示しません
                    string cmt = ((Label)e.Row.Cells[4].FindControl("lbCmt")).Text; 
                    if(string.IsNullOrEmpty(cmt)||string.IsNullOrWhiteSpace(cmt))
                    {
                        e.Row.FindControl("lbCmtDate").Visible = false;    
                    }
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        //保存ボダンの処理
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UploadFile.FileDelete(path);
            Response.Redirect("TodoList.aspx?SendSta=0");
        }
    }
}