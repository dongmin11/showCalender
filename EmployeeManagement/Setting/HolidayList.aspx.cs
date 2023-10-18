using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using EmployeeManagement.Enumerations;
using System.Globalization;
using System.Text;
using static EmployeeManagement.Common.CommonUtil;

namespace EmployeeManagement.HolidayList
{
    public partial class HolidayList : BasePage
    {
        //祝日データ
        static　DataTable HolidayDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string test = "2018/5/1";
                string moji = "あああ";
                string a = DateTime.Parse(test).ToString("yyyyMMdd");
                int aa;
                if(int.TryParse(a , out  aa))
                {
                    //成功
                }
                if(int.TryParse(moji , out aa))
                {
                    //失敗
                }
                //管理者以外場合
                if (LoginUser.Authority != AuthorityEnum.Personnel.Id)
                {
                    Response.Redirect("~/Login.aspx" , false);
                }
                if (!IsPostBack)
                {
                    //初期表示は一年分の祝日
                    txtDateStart.Text = DateTime.Now.Year.ToString() + "/01/01";
                    txtDateEnd.Text = DateTime.Now.Year.ToString() + "/12/31";
                    //表示件数初期値
                    dropNumber.SelectedValue = gridViewHoliday.PageSize.ToString();
                    //祝日一覧表示
                    GridViewHolidayDataSetting();
                }
            }
            catch(Exception ex)
            {
                //エラーメッセージ表示
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 祝日一覧
        /// </summary>
        private void GridViewHolidayDataSetting()
        {
            try
            {
                //開始日
                DateTime dateTimeS_;
                //終了日
                DateTime dateTimeE_;
                //日付の入力が正しい場合
                if (DateTime.TryParse(txtDateStart.Text, out dateTimeS_) && DateTime.TryParse(txtDateEnd.Text, out dateTimeE_))
                {
                    //年号を取得
                    string thisYearStart = dateTimeS_.ToString("yyyyMMdd");
                    string thisYearEnd = dateTimeE_.ToString("yyyyMMdd");
                    //パラメーター
                    Dictionary<string, object> parameter = new Dictionary<string, object>();
                    parameter.Add("Start", thisYearStart);
                    parameter.Add("End", thisYearEnd);
                    //今年度一年分
                    HolidayDate = db.ExecuteSQLForDataTable(this.Page, "SELECT_Holiday", parameter);
                    gridViewHoliday.DataSource = HolidayDate;
                    gridViewHoliday.DataBind();
                }
            }
            catch(Exception ex)
            {
                //エラーメッセージ表示
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 日付表示変換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewHoliday_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //行タイプがDataRowの場合
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string date = e.Row.Cells[0].Text;
                    e.Row.Cells[0].Text = date.Substring(0, 4) + "/" + date.Substring(4, 2) + "/" + date.Substring(6, 2);
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 表示件数変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //表示件数
                int number = Convert.ToInt32(dropNumber.SelectedValue);
                gridViewHoliday.PageSize = number;
                //修正
                GridViewHolidayDataSetting();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 検索ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //false = errorなし
                bool txtError = false;
                //日付が入力されていない場合
                if (string.IsNullOrWhiteSpace(txtDateStart.Text.Trim()) || string.IsNullOrWhiteSpace(txtDateEnd.Text.Trim()))
                {
                    lblSearchPeriod.Text = GetMessage("DateErrorMes");
                    txtError = true;
                }
                //入力が正しくない場合
                else if (DateTime.TryParse(txtDateStart.Text, out DateTime _) == false || DateTime.TryParse(txtDateEnd.Text, out DateTime _) == false)
                {
                    lblSearchPeriod.Text = GetMessage("DateErrorMes");
                    txtError = true;
                }
                //日付が矛盾している場合
                else if (DateTime.TryParse(txtDateStart.Text, out DateTime dtStart) &&
                         DateTime.TryParse(txtDateEnd.Text, out DateTime dtEnd))
                {
                    if (dtStart > dtEnd)
                    {
                        lblSearchPeriod.Text = GetMessage("DateErrorMes");
                        txtError = true;
                    }
                    else
                    {
                        //正しい場合はエラーメッセージを表示しない
                        lblSearchPeriod.Text = "";
                    }
                }
                //エラー判定
                if (txtError == true)
                {
                    return;
                }
                //Dictionary<string, object> parameter = new Dictionary<string, object>();
                ////開始日
                //string dateStart = txtDateStart.Text.Replace("/", "");
                ////終了日
                //string dateEnd = txtDateEnd.Text.Replace("/", "");
                //parameter.Add("Start", dateStart);
                //parameter.Add("End", dateEnd);
                GridViewHolidayDataSetting();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// ページ遷移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.gridViewHoliday.PageIndex = e.NewPageIndex;
                GridViewHolidayDataSetting();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //テキストが未入力の場合はリターン
                if (string.IsNullOrEmpty(txtCsv.Value))
                {
                    return;
                }
                //エラー件数
                int errorCount = 0;
                //日付未入力データ
                string errorDate = "";
                //未更新
                bool update = false;
                StreamReader csvReader = new StreamReader(fileCsv.PostedFile.InputStream,Encoding.GetEncoding("Shift_JIS"));
                while (!csvReader.EndOfStream)
                {
                    //行を取得
                    var line = csvReader.ReadLine();
                    var values = line.Split(',');
                    //日付
                    string date = values[0].ToString();
                    string checkDate = date.Replace("/", "");
                    int integer;
                    string holiday = "";
                    //数値に変換できる場合
                    if(int.TryParse(checkDate, out integer))
                    {
                        holiday = DateTime.Parse(date).ToString("yyyyMMdd");
                    }
                    //祝日名称
                    string name = ChangeString(values[1].ToString());
                    //祝日タイプ
                    string type = ChangeString(values[2].ToString());
                    //日付文字列判定
                    DateTime dateCsv;
                    string format = "yyyyMMdd";
                    CultureInfo ci = CultureInfo.CurrentCulture;
                    DateTimeStyles dts = DateTimeStyles.None;
                    if (DateTime.TryParseExact(holiday, format, ci, dts, out dateCsv))
                    {
                        //更新
                        update = true;
                        Dictionary<string, object> parameter = new Dictionary<string, object>();
                        //DBに既にデータがある場合は更新
                        if (IsHoliday(holiday))
                        {
                            //祝日名称がない
                            if(string.IsNullOrEmpty(name.Trim()))
                            {
                                //更新エラー
                                errorCount++;
                            }
                            //日付がある場合は更新処理を行う
                            if(!string.IsNullOrEmpty(name.Trim()))
                            {
                                //nameがある場合
                                parameter.Add("UPDATENAME", name);
                                parameter.Add("UPDATEDATE", holiday);
                                parameter.Add("UPDATETYPE", type);
                                parameter.Add("CSVDATE", holiday);
                                db.ExecuteNonQuerySQL(this.Page, "UPDATE_Holiday", parameter);
                            }
                        }
                        //DBに存在しない日付がある場合は追加する
                        else if(!IsHoliday(holiday) && !string.IsNullOrEmpty(name.Trim()))
                        {
                            parameter.Add("INSERTDATE", holiday);
                            parameter.Add("INSERTNAME", name);
                            parameter.Add("INSERTTYPE", type);
                            db.ExecuteNonQuerySQL(this.Page, "INSERT_Holiday", parameter);
                        }
                        else
                        {
                            //追加エラー
                            errorCount++;
                        }
                        //日付を取得
                        if (errorCount == 1)
                        {
                            //最初のエラーの日付を取得
                            errorDate = holiday.Substring(0, 4) + "年" + holiday.Substring(4, 2) + "月" + holiday.Substring(6, 2) + "日";
                        }
                    }
                }
                if(update == true)
                {
                    Cache.Remove("HolidayMst");
                    //問題なく全てのデータを更新した場合
                    if (errorCount == 0)
                    {
                        //更新メッセージ表示、スケジュール一覧画面に遷移
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                        "updatemes",
                        "<script language=\"JavaScript\">window.alert('更新しました');"
                        + "window.location.href = './HolidayList.aspx';</script>");
                    }
                    //更新したが一部不正なデータがあった場合
                    else if(errorCount > 0)
                    {
                        //正しくない日付とその件数を通知する
                        if(errorCount == 1)
                        {
                            Master.ModalPopup(errorDate + "の祝日名称");
                        }
                        else
                        {
                            Master.ModalPopup(errorDate + "の祝日名称が正しくありません" + "<br>" + "他" + (errorCount - 1).ToString() + "件");
                        }
                    }
                }
                else
                {
                    //csvファイルが正しくない場合
                    update = false;
                    Master.ModalPopup("csvファイルが正しくありません");
                    txtCsv.Value = "";
                    return;
                }
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }

        /// <summary>
        /// 祝日削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewHoliday_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //選択した行のデータを削除する
                string dateData = gridViewHoliday.DataKeys[Convert.ToInt32(e.RowIndex)].Value.ToString();
                Dictionary<string, object> parameter = new Dictionary<string, object>();
                parameter.Add("HolidayDate", dateData);
                db.ExecuteNonQuerySQL(this.Page, "DELETE_Holiday", parameter);
                GridViewHolidayDataSetting();
            }
            catch(Exception ex)
            {
                AppLog.Error(ex.Message);
                Master.ModalPopup(GetMessage("SystemErrorMes"));
            }
        }
    }
}