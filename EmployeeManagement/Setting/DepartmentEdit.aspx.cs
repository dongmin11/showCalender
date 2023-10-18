using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using static EmployeeManagement.Common.CommonUtil;

namespace EmployeeManagement.Setting
{
    public partial class DepartmentEdit : BasePage
    {
        public static string title = "";　//タイトル
        private static string departmentID = "";　//部署ID
        protected void Page_Load(object sender, EventArgs e)
        {
        　　try
            {
                if(!IsPostBack)
                {
                    if (LoginUser.Authority != Enumerations.AuthorityEnum.Personnel.Id)　　   //ユーザー権限チェック
                    {
                        Response.Redirect("~/TopPage/TopPage.aspx", false);
                    }
                    if (Session["DeparmentID"]!=null&&!string.IsNullOrEmpty(Session["DeparmentID"].ToString()))　
                    {
                        departmentID = Session["DeparmentID"].ToString();
                        Session.Remove("DeparmentID");
                        txtDepartmentName.Text = GetDepartmentName(Convert.ToInt32(departmentID));
                        title = "部署編集";
                        btnDelete.Visible = true;
                        btnEdit.Text = "更新";
                    }
                    else
                    {
                        departmentID = "";
                        title = "部署追加";
                        txtDepartmentName.Text = "";
                        btnEdit.Text = "追加";
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
        /// 追加・更新ボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lbDepartmentName.Text = "";
                //部署名称入力判定
                if (string.IsNullOrEmpty(txtDepartmentName.Text))
                {
                    lbDepartmentName.Text = GetMessage("EmptyErrorMes");
                    return;
                }
                else
                {
                    Cache.Remove("DepatmentMst");
                    lbDepartmentName.Text = "";
                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("DEPARTMENTNAME",ChangeString(txtDepartmentName.Text));
                    para.Add("TIMENOW", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    para.Add("USERNAME", LoginUser.UserName);
                    if (string.IsNullOrEmpty(departmentID))　　　//追加処理
                    {
                        db.ExecuteNonQuerySQL(this.Page, "INSERT_DepartmentMst", para);
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                   "insertmes",
                   "<script language=\"JavaScript\">window.alert('追加しました');"
                   + "window.location.href = './DepartmentList.aspx';</script>");
                    }
                    else                                        //更新処理
                    {
                        para.Add("DEPARTMENTID", departmentID);
                        db.ExecuteNonQuerySQL(this.Page, "UPDATE_DepartmentMst", para);
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                   "insertmes",
                   "<script language=\"JavaScript\">window.alert('更新しました');"
                   + "window.location.href = './DepartmentList.aspx';</script>");
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
            Response.Redirect("DepartmentList.aspx");
        }

        /// <summary>
        /// 削除ボダン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> para = new Dictionary<string, object>();
            para.Add("DEPARTMENTID", departmentID);
            db.ExecuteNonQuerySQL(this.Page, "DELETE_DepartmentMst", para);
            Cache.Remove("DepatmentMst");
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                   "insertmes",
                   "<script language=\"JavaScript\">window.alert('削除しました');"
                   + "window.location.href = './DepartmentList.aspx';</script>");
        }
    }
}