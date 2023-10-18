using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using static EmployeeManagement.Common.CommonUtil;
using static EmployeeManagement.Enumerations.DocumentStaEnum;
using static EmployeeManagement.Enumerations.TdodoAttributeEnum;
using static EmployeeManagement.Enumerations.TodoKindEnum;
using static EmployeeManagement.Enumerations.TodoListStaEnum;



namespace EmployeeManagement.Document
{
    public partial class DocumentApply : BasePage
    {
        private static string dcid; 　　//書類番号
        private static bool error ;　　//入力エラー
        private static string ApplyID;　//申請ID　
        private static string Lockver;  //LockVer
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (!IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    {
                        ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                    }
                    ApplyID = "";
                    GetListItem();
                    //詳細モード
                    if (Session["applyid"]!=null&&!string.IsNullOrEmpty((string)Session["applyid"]))　
                    { 
                        btnConfirm.Text = "更新";
                        ApplyID = (string)Session["applyid"];　//セッションから申請ID修得
                        Session.Remove("applyid");
                        GetInfo();
                    }
                    //新規追加モード
                    else
                    {
                        btnConfirm.Text = "確定";
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
        /// リストアイテム取得・表示
        /// </summary>
        protected void GetListItem()
        {

            //チェックボックスリストアイテム
            cklDocument.DataSource = GetNameMst().Where(x => x["分類コード"].ToString().Equals("D0001") ).Select(x => new {
                名称 = x["名称"],
                SEQ = x["SEQ"]
            }).ToList();
            cklDocument.DataBind();
            
            //ユーザーコンボボックスアイテム
            DataTable dt = db.ExecuteSQLForDataTable(this.Page, "SELECT_User");
            dropUser.DataSource = dt;
            dropUser.DataBind();
            dropUser.SelectedValue= LoginUser.UserNo.ToString(); ;

            //承認者コンボボックスアイテム
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("NUMBER", ConfigurationManager.AppSettings["ManagementID"]);
            dropAuthorizer.DataSource = db.ExecuteSQLForDataTable(this.Page, "SELECT_SyainName", para);
            dropAuthorizer.DataBind();
            dropAuthorizer.AppendDataBoundItems = true;
            dropAuthorizer.Items.Insert(0, new ListItem("  ", ""));
            
            
            //ステータスアイテム
            ListItem listItem = new ListItem("", "");
            ListItem unapproved = new ListItem(Unapproved.Name, Unapproved.Id.ToString());
            ListItem remand = new ListItem(Remand.Name, Remand.Id.ToString());
            ListItem making = new ListItem(Making.Name, Making.Id.ToString());
            ListItem waittake = new ListItem(WaitTake.Name, WaitTake.Id.ToString());
            ListItem completed = new ListItem(Completed.Name, Completed.Id.ToString());
            ListItem delete = new ListItem(Delete.Name, Delete.Id.ToString());
            ListItem[] listItems = { listItem, unapproved, remand, making, waittake, completed, delete };
            dropStatus.Items.AddRange(listItems);
            
            //一般ユーザー場合、申請者コンボボックス非活性、承認情報エリア変更できない
            if (LoginUser.SyozokuID.ToString() != ConfigurationManager.AppSettings["ManagementID"] && LoginUser.Authority != Enumerations.AuthorityEnum.Personnel.Id)
            {
                dropUser.Visible = false;
                txtUser.Visible = true;
                txtUser.Text = LoginUser.UserName;
                txtCmt2.ReadOnly = true;
                dropAuthorizer.Visible = false;
                txtAuthorizer.Visible = true;
                dropStatus.Visible = false;
                txtStatus.Visible = true;
            }
        }

       /// <summary>
       /// DBからデータ取得
       /// </summary>
       /// <returns></returns>
        protected List<Dictionary<string, object>> GetDb()
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            string require = $"WHERE 申請ID = {ApplyID}";
            para.Add("require", require);
            List<Dictionary<string, object>> list = db.ExecuteSQForList(this.Page, "SELECT_DocumentList", para);
            return list;
        }

        /// <summary>
        /// 書類申請データ表示
        /// </summary>
        protected void GetInfo()
        {
            Lockver = "";
            List<Dictionary<string, object>> list = GetDb();
            string x = list[0].ToList()[0].ToString();
            dropUser.SelectedValue= list[0]["申請者No"].ToString();                                                //申請者表示
            foreach(ListItem listItem in cklDocument.Items)　　　　　　　　　　　　　　　　　　　　　　　　　　　　//書類名称表示
            {
                string[] vs = list[0]["書類No"].ToString().Split(',');
                for(int i = 0;i<vs.Length;i++)
                {
                    if (vs[i].Equals(listItem.Value))
                    {
                        listItem.Selected = true;
                    }
                }
            }
            txtDate.Text =DateTime.Parse(list[0]["受取予定日"].ToString()).ToString("yyyy/MM/dd");  　　　　　　　//予定日表示
            txtCmt1.Text = list[0]["申請書コメント"].ToString();　　　　　　　　　　　　　　　　　　　　　　　　　//申請コメント表示
            dropAuthorizer.SelectedValue= list[0]["承認者No"].ToString();　　　　　　　　　　　　　　　　　　　   //承認者表示
            dropStatus.SelectedValue= list[0]["ステータス"].ToString();　　　　　　　　　　　　　　　　　　　　　 //ステータス表示
            txtCmt2.Text= list[0]["承認者コメント"].ToString();                                                   //承認者コメント表示
            txtUser.Text = dropUser.SelectedItem.Text;　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　//ユーザーtextbox
            txtAuthorizer.Text = dropAuthorizer.SelectedItem.Text;　　　　　　　　　　　　　　　　　　　　　　　　//承認者textbox
            txtStatus.Text = dropStatus.SelectedItem.Text;　　　　　　　　　　　　　　　　　　　　　　　　　　　　//ステータスtextbox
            Lockver = list[0]["LockVer"].ToString();                                                              //LockVer取得
            //ログインユーザ申請者じゃない、そして承認ステータスは「完成」の場合、申請資料エリア変更できない
            if (LoginUser.UserNo.ToString() != list[0]["申請者No"].ToString()||dropStatus.SelectedValue== Completed.Id.ToString())
            {
                dropUser.Visible = false;
                txtUser.Visible = true;
                cklDocument.Attributes.Add("class", "notclickable");
                txtDate.Attributes.Add("class", "notclickable");
                txtCmt1.ReadOnly = true;
                //一般ユーザーの場合、戻るボダンだけ表示
                if(LoginUser.SyozokuID.ToString() != ConfigurationManager.AppSettings["ManagementID"] && LoginUser.Authority != Enumerations.AuthorityEnum.Personnel.Id)
                {
                    btnConfirm.Visible = false;
                    btnCancel.Text = "戻る";
                    btnCancel.CssClass = "btn-confirm";
                    btnCancel.OnClientClick = "";
                    ckTodo.Visible = false;
                }
            }
        }

        /// <summary>
        /// パラメータ取得
        /// </summary>
        /// <param name="documentname">書類名称</param>
        /// <param name="receiver">宛名先</param>
        /// <returns></returns>
        protected Dictionary<string,object> GetPara(string documentname,string receiver)
        { 
            string department = GetDepartmentName(LoginUser.SyozokuID);   　　　　//ログインユーザー部署名
            Dictionary<string, object> para = new Dictionary<string, object>();　 
            para.Add("UserID",dropUser.SelectedValue);　　　　　　　　　　　　　　//申請者ユーザー番号
            para.Add("DCID", dcid.TrimEnd(','));　　　　　　　　　　　　　　　　　//書類番号
            para.Add("ReceiveDate",txtDate.Text);　　　　　　　　　　　　　　　　 //受取予定日
            //para.Add("UserCmt",ChangeString(txtCmt1.Text));　　　　　　　　　　　 //申請コメント
            
            //承認者
            if (string.IsNullOrEmpty(dropAuthorizer.SelectedValue)||string.IsNullOrEmpty(txtAuthorizer.Text))　
            {
                para.Add("AuthorizerID", "0");
            }
            else
            {
                para.Add("AuthorizerID", dropAuthorizer.SelectedValue);
            }
            
            //ステータス
            if(string.IsNullOrEmpty(dropStatus.SelectedValue) || string.IsNullOrEmpty(txtStatus.Text))
            {
                para.Add("status", "0");
            }
            else
            {
                para.Add("status", dropStatus.SelectedValue);
            }
            para.Add("AuthorizerCmt",ChangeString(txtCmt2.Text));　　　　　　　　//承認コメント
            para.Add("RegisterID", LoginUser.UserNo);　　　　　　　　　　　　　　//登録者ユーザー番号
            para.Add("UpdateUserID", LoginUser.UserNo);　　　　　　　　　　　　　//更新者ユーザー番号
            if(!string.IsNullOrEmpty(ApplyID))
            {
                para.Add("ApplyID", ApplyID);　　　　　　　　　　　　　　　　　　//申請ID
            }
            para.Add("Receiver", receiver);                                      //宛名先名前
            para.Add("Senduser", department +"の"+ LoginUser.UserName);　　　　　//送信者名前
            para.Add("ApplyUser", dropUser.SelectedItem.Text);　　　　　　　　　 //申請者名前
            para.Add("Document", documentname.TrimEnd(','));　　　　　　　　　　 //申請書類名称
            para.Add("ApplyCmt",ChangeString( txtCmt1.Text));　　　　　　　　　　//申請コメント
            para.Add("Authorizer", dropAuthorizer.SelectedItem.Text);　　　　　　//承認者名前
            para.Add("Status", dropStatus.SelectedItem.Text);　　　　　　　　　　//ステータス名称
            return para;
        }

        /// <summary>
        /// Todo送信
        /// </summary>
        /// <param name="documentname">書類名称</param>
        protected void TodoSend(string documentname)
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            string department = GetDepartmentName(LoginUser.SyozokuID);
            string timenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string title = "";　　　　　　　　　　　　　　//TODOタイトル
            string text = "";　　　　　　　　　　　　　　　//TODO本文
            string filepath = "";　　　　　　　　　　　　//TODO本文テキストファイルパス
            string receiver = "";　　　　　　　　　　　　//宛名先
            DataTable dt = td.ContentsInputted(false); 　//TODO内容データテーブル<
            DataTable rcdt = new DataTable();　　　　　　//TODO受信者データテーブル
            //一般ユーザーの宛名先設定
            if (LoginUser.SyozokuID.ToString() != ConfigurationManager.AppSettings["ManagementID"] && LoginUser.Authority != Enumerations.AuthorityEnum.Personnel.Id)　//一般ユーザーの宛名先設定
            {
                receiver += "管理部";
                para.Add("NUMBER", ConfigurationManager.AppSettings["ManagementID"]);
                rcdt = db.ExecuteSQLForDataTable(this.Page, "SELECT_SyainName", para);
            }
            else　　　　//管理部員と管理者のユーザー宛名先設定
            {
                receiver += dropUser.SelectedItem.Text;
                rcdt.Columns.Add("社員番号");
                rcdt.Rows.Add(dropUser.SelectedValue);
            }
            filepath = ConfigurationManager.AppSettings["DocumentApplyTodoTxtFile"];　//書類申請用TODO本文テキスト　ファイルパス取得
            if (string.IsNullOrEmpty(ApplyID))　　　//新規追加の場合申請内容全部TODOで送信する
            {
                title += "書類申請の件(新規)";
                text += td.TodoText(filepath, GetPara(documentname, receiver));
            }
            else　　//更新の場合変更したないようだけTODOで送信する
            {
                title += "書類申請の件(更新)";
                List<Dictionary<string, object>> list = (List<Dictionary<string, object>>)ViewState["list"];
                para.Add("Receiver", receiver);
                para.Add("Senduser", department + "の" + LoginUser.UserName);
                para.Add("ApplyUser", "");
                if (list[0]["書類No"].ToString() != dcid.TrimEnd(','))      //申請書類比較
                {
                    para.Add("Document", documentname.TrimEnd(','));
                }
                else
                {
                    para.Add("Document", "");
                }

                if (DateTime.Parse(list[0]["受取予定日"].ToString()).ToString("yyyy/MM/dd") != txtDate.Text)　//受取予定日比較
                {
                    para.Add("ReceiveDate", txtDate.Text);
                }
                else
                {
                    para.Add("ReceiveDate", "");
                }

                if (list[0]["申請書コメント"].ToString() != txtCmt1.Text)　　//申請書コメント比較
                {
                    para.Add("ApplyCmt", ChangeString(txtCmt1.Text));
                }
                else
                {
                    para.Add("ApplyCmt", "");
                }

                if (list[0]["承認者No"].ToString() != dropAuthorizer.SelectedValue)　//承認者比較
                {
                    para.Add("Authorizer", dropUser.SelectedItem.Text);
                }
                else
                {
                    para.Add("Authorizer", "");
                }

                if (list[0]["ステータス"].ToString() != dropStatus.SelectedValue)　//ステータス比較
                {
                    title = "書類申請ステータス更新の件";　//TODOタイトル
                    para.Add("Status", dropStatus.SelectedItem.Text);
                }
                else
                {
                    para.Add("Status", "");
                }

                if (list[0]["承認者コメント"].ToString() != txtCmt2.Text)　　//承認者コメント比較
                {
                    para.Add("AuthorizerCmt", ChangeString(txtCmt2.Text));
                }
                else
                {
                    para.Add("AuthorizerCmt", "");
                }
                text += td.TodoText(filepath, para);
            }
            //         送信状態,　　発信者　　　 , 　　種別　　 ,タイトル,本文,発信日時,  登録ユーザー   ,登録日時,   更新ユーザー  ,更新日時,　　属性　　
            dt.Rows.Add(Sent.Id, LoginUser.UserNo, Communicate.Id, title, text, timenow, LoginUser.UserNo, timenow, LoginUser.UserNo, timenow, Important.Id);
            td.SendTodo(this.Page, dt, rcdt);
        }

        /// <summary>
        /// 確定ボダン・更新ボダンプログラム
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                dcid = "";　　　　　　　　　　//書類番号
                error = false;　　　　　　　　//入力エラー
                string documentname = "";　　　//書類名称
                foreach (ListItem listItem in cklDocument.Items)
                {
                    if (listItem.Selected)
                    {
                        dcid += listItem.Value + ",";
                        documentname += listItem.Text+",";
                    }
                }

                if (string.IsNullOrEmpty(dcid))　　　　　　　　　　　　　　//書類チェック
                {
                    lbDocument.Text = GetMessage("EmptyErrorMes");
                    error = true;
                }
                else
                {
                    lbDocument.Text = "";
                }

                if (string.IsNullOrEmpty(txtDate.Text))　　　　　　　　　　//受取予定日チェック
                {
                    lbDate.Text = GetMessage("EmptyErrorMes");
                    error = true;
                }
                else
                {
                    DateTime d;
                    if (!DateTime.TryParse(txtDate.Text, out d))
                    {
                        // 日付でない警告を通知する
                        lbDate.Text = GetMessage("DateErrorMes");
                        error = true;
                    }
                    else
                    {
                        lbDate.Text = "";
                    }
                }
                
                if (error == true)
                {
                    Master.ModalPopup(GetMessage("TxtErrorMes"));
                    return;
                }
                else
                {
                    lbDocument.Text = ""; lbDate.Text = "";
                    if (string.IsNullOrEmpty(ApplyID))
                    {
                        db.ExecuteNonQuerySQL(this.Page, "INSERT_DocumentList", GetPara("", ""));    //追加ｓｑｌ
                        
                    }
                    else
                    {
                        List<Dictionary<string, object>> list = GetDb();
                        ViewState["list"] = list;
                        if(list[0]["ステータス"].ToString() != dropStatus.SelectedValue&&string.IsNullOrEmpty(dropAuthorizer.SelectedValue))　//ステータス変更の場合、承認者チェック　　
                        {
                            lbAuthorizer.Text= GetMessage("EmptyErrorMes");
                            return;
                        }
                        else if(list[0]["LockVer"].ToString()!=Lockver)    //Lockverチェック
                        {
                            lbAuthorizer.Text = "";
                            GetInfo();
                            Master.ModalPopup(GetMessage("SystemErrorMes"));
                            return;
                        }
                        else
                        {
                            db.ExecuteNonQuerySQL(this.Page, "UPDATE_DocumentList", GetPara("", ""));　　　//更新ｓｑｌ
                        }
                        
                    }

                    if (ckTodo.Checked)                              //TODO送信
                    {
                        TodoSend(documentname);
                    }
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "insertmes",
                    "<script language=\"JavaScript\">window.alert('送信しました');"
                    + "window.location.href = './DocumentList.aspx';</script>");
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                if(!string.IsNullOrEmpty(ApplyID))
                {
                    GetInfo();
                }
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if(ViewState["RefUrl"]!=null&&!string.IsNullOrEmpty(ViewState["RefUrl"].ToString()))
            {
                Response.Redirect(ViewState["RefUrl"].ToString());
            }
            else
            {
                Response.Redirect("DocumentList.aspx");
            }
        }
    }
}