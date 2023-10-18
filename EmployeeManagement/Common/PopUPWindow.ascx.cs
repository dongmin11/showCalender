using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeManagement.Common
{
    public partial class PopUPWindow : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void ShowMessage(string message)
        {
            this.lblMsg.Text = message;
        }

        
    }
}