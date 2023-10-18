using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeManagement.HolidayList
{
    public partial class HolidayList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //年号を取得
                    DateTime date = DateTime.Now;
                    string thisYear = date.Year.ToString() + "0101";
                    //祝日データ
                    DataTable HolidayDate;
                    //パラメーター
                    Dictionary<string, object> parameter = new Dictionary<string, object>();
                    parameter.Add("THISYEAR", thisYear);
                    HolidayDate = db.ExecuteSQLForDataTable(this.Page, "SELECT_Holiday", parameter);
                    gridViewHoliday.DataSource = HolidayDate;
                    gridViewHoliday.DataBind();
                    //祝日グリッドビューを表示
                    //GridViewHoliday_DataSetting();
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        /// <summary>
        /// 祝日グリッドビュー
        /// </summary>
        private void GridViewHoliday_DataSetting()
        {
            //try
            //{
            //    DateTime date = DateTime.Now;
            //    //年号を取得
            //    string thisYear = date.ToString().Substring(0, 4) + "0101";
            //    //祝日データ
            //    DataTable HolidayDate;
            //    //パラメーター
            //    Dictionary<string, object> parameter = new Dictionary<string, object>();
            //    parameter.Add("THISYEAR", thisYear);
            //    HolidayDate = db.ExecuteSQLForDataTable(this.Page, "SELECT_Holiday", parameter);
            //    gridViewHoliday.DataSource = HolidayDate;
            //    gridViewHoliday.DataBind();
            //}
            //catch(Exception ex)
            //{

            //}
        }
    }
   
}