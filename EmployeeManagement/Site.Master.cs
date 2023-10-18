using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EmployeeManagement.Common;

namespace EmployeeManagement
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(BasePage.LoginUser != null && BasePage.LoginUser.Authority != Enumerations.AuthorityEnum.Personnel.Id)
            {
                Setting.Visible = false;
            }
            else
            {
                Setting.Visible = true;
            }
            
        }

        protected void Logout_ServerClick(object sender, EventArgs e)
        {
            BasePage.LoginUser = null;
            Session.Remove("LoginNo");
            Response.Redirect("~/Login.aspx");
        }

        public void ModalPopup(string message)
        {
            pw1.Visible = true;
            pw1.ShowMessage(message);
        }

      

    }
}