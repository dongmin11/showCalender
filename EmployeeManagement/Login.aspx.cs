using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseUtil;
using EmployeeManagement.Enumerations;

namespace EmployeeManagement
{
    public partial class Login : BasePage
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger("AccessLog");
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void BtnSignin_Click(object sender, EventArgs e)
        {
            //入力判定
            if (string.IsNullOrEmpty(TxtUserName.Value.Trim()) || string.IsNullOrEmpty(TxtPassword.Value.Trim()))
            {
                LblLogin.Text = "ユーザーIDまたはパスワードに入力がありません";
                return;
            }
            //データ照合(DB側で条件検索し、ない場合ログイン失敗)
            try
            {
                string encrypted = this.EncryptString (TxtPassword.Value.Trim());// 暗号化
                Dictionary<string, object> para = new Dictionary<string, object>();
                para.Add("USERID", TxtUserName.Value.Trim());
                para.Add("PWD", encrypted);
                List<Dictionary<string, object>> datas = db.ExecuteSQForList(this.Page, "SELECT_LoginUser", para);
                if (datas != null && datas.Any())
                {
                    Session["LoginNo"] = this.EncryptString(datas[0]["社員番号"].ToString()); //セッション受け渡し
                    //ログイン状態を保持チェック処理
                    if (loginKeep.Checked == false)
                    {
                        Session.Timeout = 60;
                    }
                    else
                    {
                        Session.Timeout = 999;
                    }
                }
                else
                {
                    //エラー処理
                    LblLogin.Text = "ユーザーIDまたはパスワードが正しくありません";
                    return;
                }
            }
            catch
            {
                return;
            }
            Response.Redirect("./TopPage/TopPage.aspx");
        }
    }
}