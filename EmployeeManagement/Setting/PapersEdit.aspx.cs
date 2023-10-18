using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using static EmployeeManagement.Common.CommonUtil;


namespace EmployeeManagement.Setting
{
    public partial class PapersEdit : BasePage
    {
        public static string title ;　　//タイトル
        private  string paperid ;　　　 //書類番号
        private  string name ;　　　　　//書類名称
        Dictionary<string, object> para = new Dictionary<string, object>();
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
                    if (Session["paperID"]!=null&&!string.IsNullOrEmpty(Session["paperID"].ToString()))
                    {
                        title = "申請書類編集";
                        paperid = (string)Session["paperID"];
                        Session.Remove("paperID");
                        txtPaperID.Text = paperid;
                        name= GetDocumentName(paperid);
                        txtPaperName.Text = name; 　　　//書類名称の表示
                        btnEdit.Text = "更新";
                        btnDelete.Visible = true;
                    }
                    else
                    {
                        title = "申請書類追加";
                        paperid = "";
                        name = "";
                        txtPaperID.Text = "";
                        txtPaperName.Text = "";
                        btnDelete.Visible = false;
                    }
                    ViewState["PAPERID"] = paperid;
                    ViewState["PAPERNAME"] = name;
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 追加・更新ボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                paperid = ViewState["PAPERID"].ToString();
                name = ViewState["PAPERNAME"].ToString();
                bool enterror = false;　　　//入力エラー；
                
                lbPaperId.Text = "";
                lbPaperName.Text = "";
                //書類番号入力判定
                if(string.IsNullOrEmpty(txtPaperID.Text))
                {
                    lbPaperId.Text = GetMessage("EmptyErrorMes");
                    enterror = true;
                }
                else
                {
                    if(Regex.IsMatch(txtPaperID.Text, @"^[0-9]+$"))　　　//入力番号は半角数字かどうかチェック
                    {
                        string number = GetDocumentName(txtPaperID.Text);
                        if (paperid!=txtPaperID.Text&&!string.IsNullOrEmpty(number))　　//重複チェック
                        {
                            enterror = true;
                            lbPaperId.Text = "番号は使用されています";
                        }
                    }
                    else
                    {
                        enterror = true;
                        lbPaperId.Text = GetMessage("TxtErrorMes");
                    }
                }
                //書類名称入力判定
                if(string.IsNullOrEmpty(txtPaperName.Text))
                {
                    lbPaperName.Text = GetMessage("EmptyErrorMes");
                    enterror = true;
                }
                if (enterror == true)
                {
                    return;
                }
                else
                {
                    lbPaperId.Text = "";
                    lbPaperName.Text = "";
                    para.Add("PAPERID",ChangeInt( txtPaperID.Text));
                    para.Add("PAPERNAME",ChangeString(txtPaperName.Text));
                    Cache.Remove("NameMst");
                    if (string.IsNullOrEmpty(paperid))    //追加処理
                    {
                        db.ExecuteNonQuerySQL(this.Page, "INSERT_PAPERS",para);
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "insertmes",
                    "<script language=\"JavaScript\">window.alert('追加しました');"
                    + "window.location.href = './PapersList.aspx';</script>");
                    }
                    else
                    {
                        para.Add("ORIGINALID", paperid);
                        db.ExecuteNonQuerySQL(this.Page, "UPDATE_PAPERS", para);
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "insertmes",
                    "<script language=\"JavaScript\">window.alert('更新しました');"
                    + "window.location.href = './PapersList.aspx';</script>");
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
        /// キャンセルボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("./PapersList.aspx");
        }

        /// <summary>
        /// 削除ボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Cache.Remove("NameMst");
                para.Add("PAPERID",ViewState["PAPERID"].ToString());
                db.ExecuteNonQuerySQL(this.Page, "DELETE_PAPER",para);
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "insertmes",
                    "<script language=\"JavaScript\">window.alert('削除しました');"
                    + "window.location.href = './PapersList.aspx';</script>");
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }
    }
}