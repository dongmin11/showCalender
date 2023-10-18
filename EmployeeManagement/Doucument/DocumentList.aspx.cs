using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static EmployeeManagement.Enumerations.DocumentStaEnum;

namespace EmployeeManagement.Setting
{
    public partial class DocumentList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ListItem unapproved = new ListItem(Unapproved.Name, Unapproved.Id.ToString());
                ListItem remand = new ListItem(Remand.Name, Remand.Id.ToString());
                ListItem making = new ListItem(Making.Name, Making.Id.ToString());
                ListItem waittake = new ListItem(WaitTake.Name, WaitTake.Id.ToString());
                ListItem completed = new ListItem(Completed.Name, Completed.Id.ToString());
                ListItem delete = new ListItem(Delete.Name, Delete.Id.ToString());
                ListItem[] listItems = { unapproved, remand, making, waittake, completed, delete };
                dropStatus.Items.AddRange(listItems);
            }
        }
    }
}