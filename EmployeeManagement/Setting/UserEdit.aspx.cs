using EmployeeManagement.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using static  EmployeeManagement.Common.CommonUtil;

namespace EmployeeManagement.Setting
{
    public partial class UserEdit : BasePage
    {
        //初期値
        public string editTitle = "社員追加";//タイトル
        static string conectID = "";//ID用
        private static DataTable dtBuka;//部下一覧用DataTable
        /// <summary>
        /// 画面ロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //管理者以外場合
                if (LoginUser.Authority != AuthorityEnum.Personnel.Id)
                {
                    Response.Redirect("~/Login.aspx");
                }
                if (!IsPostBack)
                {
                    //追加・編集判定
                    if (Session["社員番号"] != null)
                    {
                        conectID = this.Session["社員番号"].ToString();
                        this.Session.Remove("社員番号");
                    }
                    else
                    {
                        conectID = "";
                    }
                    //DropDownListのセット
                    ListSetting();
                    if (conectID != "")
                    {
                        //更新画面処理
                        editTitle = "社員編集";
                        BtnUserEdit.Text = "更新";
                        Dictionary<string, object> param = new Dictionary<string, object>();
                        param.Add("USERID", conectID);
                        List<Dictionary<string, object>> listUser = db.ExecuteSQForList(this.Page, "SELECT_UserEdit", param);
                        DataSetting(listUser);
                    }
                    else
                    {
                        //新規部下作成
                        CreateBuka();
                    }
                }
                else if(conectID != "")
                {
                    editTitle = "社員編集";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Ddl等のデータセット
        /// </summary>
        private void ListSetting()
        {
            //権限名のDdl取得
            ListItem authorityPer = new ListItem(AuthorityEnum.Personnel.Name, AuthorityEnum.Personnel.Id.ToString());
            ListItem authorityGen = new ListItem(AuthorityEnum.General.Name, AuthorityEnum.General.Id.ToString());
            DdlAuthority.Items.Add(authorityGen);
            DdlAuthority.Items.Add(authorityPer);
            //部門Ddl取得
            DataTable dtDepart = db.ExecuteSQLForDataTable(this.Page, "SELECT_DepartMent");
            DdlDepart.DataSource = dtDepart;
            DdlDepart.DataBind();
            ListItem emptyItem = new ListItem("", "0");
            DdlDepart.Items.Add(emptyItem);
            DdlDepart.SelectedValue = "0";
            DdlBukaDepart.DataSource = dtDepart;
            DdlBukaDepart.DataBind();
            //性別Ddl取得
            ListItem sexNoGender = new ListItem(SexEnum.NoGender.Name, SexEnum.NoGender.Id.ToString());
            ListItem sexMan = new ListItem(SexEnum.Man.Name, SexEnum.Man.Id.ToString());
            ListItem sexWoman = new ListItem(SexEnum.Woman.Name, SexEnum.Woman.Id.ToString());
            ListItem[] sexItems = { sexNoGender, sexMan, sexWoman };
            DdlSex.Items.AddRange(sexItems);
            //社員フラグDdl取得
            ListItem empDelete = new ListItem(EmployeeFlagEnum.Delete.Name, EmployeeFlagEnum.Delete.Id.ToString());
            ListItem empParm = new ListItem(EmployeeFlagEnum.Parmanent.Name, EmployeeFlagEnum.Parmanent.Id.ToString());
            ListItem empKari = new ListItem(EmployeeFlagEnum.OutSourse.Name, EmployeeFlagEnum.OutSourse.Id.ToString());
            ListItem empOther = new ListItem(EmployeeFlagEnum.Other.Name, EmployeeFlagEnum.OutSourse.Id.ToString());
            ListItem[] empItems = {  empParm, empKari, empOther , empDelete };
            DdlEmployeeFlag.Items.AddRange(empItems);
            //国籍Ddl取得
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("CLASSNAME", "国籍");
            DataTable dtNational = db.ExecuteSQLForDataTable(this.Page, "SELECT_GetClass", param);
            DdlNational.DataSource = dtNational;
            DdlNational.DataBind();
            //学歴Ddl取得
            param["CLASSNAME"] = "学歴";
            DataTable dtStudies = db.ExecuteSQLForDataTable(this.Page, "SELECT_GetClass", param);
            DdlStudies.DataSource = dtStudies;
            DdlStudies.DataBind();
            //学位Ddl取得
            param["CLASSNAME"] = "学位";
            DataTable dtDegree = db.ExecuteSQLForDataTable(this.Page, "SELECT_GetClass", param);
            DdlDegree.DataSource = dtDegree;
            DdlDegree.DataBind();
            //社員ListBox取得
            ListBoxUpdate();
        }

        /// <summary>
        /// 社員情報を画面に反映
        /// </summary>
        /// <param name="listUser">該当社員情報</param>
        private void DataSetting(List<Dictionary<string, object>> listUser)
        {
            DateTime dateTime_ = DateTime.Now;
            TxtUser.Text = listUser[0]["ユーザー"].ToString();
            TxtPwd.Attributes.Add("value", this.DecryptString(listUser[0]["パスワード"].ToString()));
            TxtUserName.Text = listUser[0]["社員名称"].ToString();
            TxtKana.Text = listUser[0]["フリガナ"].ToString();
            TxtNickName.Text = listUser[0]["社員名称表示"].ToString();
            TxtMail.Text = listUser[0]["メール"].ToString();
            TxtCompanyNum.Text = listUser[0]["会社番号"].ToString();
            TxtCompany.Text = listUser[0]["社名"].ToString();
            DdlDepart.SelectedValue = listUser[0]["所属"].ToString();

            if (DateTime.TryParse(listUser[0]["入社日"].ToString(), out dateTime_))
            {
                TxtEntryDate.Text = dateTime_.ToString("yyyy/MM/dd");
            }
            DdlSex.SelectedValue = listUser[0]["性別"].ToString();
            if (DateTime.TryParse(listUser[0]["生年月日"].ToString(), out dateTime_))
            {
                TxtBirthDay.Text = dateTime_.ToString("yyyy/MM/dd");
            }
            DdlAuthority.SelectedValue = listUser[0]["権限"].ToString();
            DdlEmployeeFlag.SelectedValue = listUser[0]["社員フラグ"].ToString();
            TxtTelNum.Text = listUser[0]["電話"].ToString();
            DdlNational.SelectedValue = listUser[0]["国籍"].ToString();
            if (DateTime.TryParse(listUser[0]["来日年月日"].ToString(), out dateTime_))
            {
                TxtVisitDate.Text = dateTime_.ToString("yyyy/MM/dd");
            }
            DdlStudies.SelectedValue = listUser[0]["学歴"].ToString();
            TxtSchool.Text = listUser[0]["学校名"].ToString();
            TxtMajor.Text = listUser[0]["専攻学科"].ToString();
            DdlDegree.SelectedValue = listUser[0]["学位"].ToString();
            if (DateTime.TryParse(listUser[0]["卒業年月日"].ToString(), out dateTime_))
            {
                TxtGraduation.Text = dateTime_.ToString("yyyy/MM/dd");
            }
            TxtPostal.Text = listUser[0]["郵便番号"].ToString();
            TxtAddress.Text = listUser[0]["住所"].ToString();
            TxtNearby.Text = listUser[0]["最寄り駅"].ToString();
            TxtNote.Text = listUser[0]["備考"].ToString();
            DdlBukaDepart.SelectedValue = listUser[0]["所属"].ToString();
            //部下一覧の表示
            if (!string.IsNullOrEmpty(listUser[0]["社員情報"].ToString()))
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("BUKAID", $" IN ({listUser[0]["社員情報"]})");
                dtBuka = db.ExecuteSQLForDataTable(this.Page, "SELECT_BukaSearch", param);
                //部下ID保存
                ViewState["BukaID"] = listUser[0]["社員情報"].ToString();
            }
            else
            {
                CreateBuka();
            }
            GvBUkaInfo.DataSource = dtBuka;
            GvBUkaInfo.DataBind();
        }

        /// <summary>
        /// 新規部下一覧 
        /// </summary>
        private void CreateBuka()
        {
            //データが空のカラムありのDataTable作成
            dtBuka = new DataTable();
            dtBuka.Columns.Add("社員番号");
            dtBuka.Columns.Add("社員名称");
            dtBuka.Columns.Add("部署名");
            dtBuka.Columns.Add("メール");
        }

        /// <summary>
        /// 部門変更処理
        /// （ドロップダウンリスト）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlBukaDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListBoxUpdate();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// リストボックスの更新処理
        /// </summary>
        private void ListBoxUpdate()
        {
            //所属社員検索
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("DEPART", DdlBukaDepart.SelectedValue);
            DataTable dtUser = db.ExecuteSQLForDataTable(this.Page, "SELECT_DepartUser", param);
            ListBuka.DataSource = dtUser;
            ListBuka.DataBind();
        }

        /// <summary>
        /// 部下追加処理
        /// （リストボックス）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ListBuka_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //重複判定
                for (int i = 0; i < GvBUkaInfo.Rows.Count; i++)
                {
                    if (GvBUkaInfo.DataKeys[i].Value.ToString() == ListBuka.SelectedValue)
                    {
                        return;
                    }
                }
                //所属部下判定
                string bukaNum =
                    ViewState["BukaID"] == null || ViewState["BukaID"].ToString() == "" ?
                    ListBuka.SelectedValue : $"{ViewState["BukaID"]},{ListBuka.SelectedValue}";
                ViewState["BukaID"] = bukaNum;
                //部下一覧に追加後表示
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("BUKAID", $" IN ({ListBuka.SelectedValue})");
                DataTable data = db.ExecuteSQLForDataTable(this.Page, "SELECT_BukaSearch", param);
                dtBuka.ImportRow(data.Rows[0]);
                GvBUkaInfo.DataSource = dtBuka;
                GvBUkaInfo.DataBind();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 部下削除ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvBUkaInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //保存部下IDの削除
                string afterBuka = ViewState["BukaID"].ToString().Replace(GvBUkaInfo.DataKeys[e.RowIndex].Value.ToString(), "");
                ViewState["BukaID"] = afterBuka.Trim(',').Replace(",,", ",");
                //部下一覧から削除
                dtBuka.Rows.RemoveAt(e.RowIndex);
                GvBUkaInfo.DataSource = dtBuka;
                GvBUkaInfo.DataBind();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新・追加ボタン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnUserEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.LblPwd.Text = "";
                //入力判定
                bool txtError = false;
                //入力必須項目
                txtError = TxtBoxValidation(TxtUser, LblUser, txtError);
                txtError = TxtBoxValidation(TxtPwd, LblPwd, txtError);
                txtError = TxtBoxValidation(TxtUserName, LblUserName, txtError);
                txtError = TxtBoxValidation(TxtKana, LblKana, txtError);
                txtError = TxtBoxValidation(TxtNickName, LblNickName, txtError);
                txtError = TxtBoxValidation(TxtMail, LblMail, txtError);
                //txtError = TxtBoxValidation(TxtBirthDay, LblBirthDay, txtError);
                txtError = DateValidation(TxtBirthDay, LblBirthDay, false);
                txtError = DateValidation(TxtEntryDate, LblEntryDate, txtError);
                txtError = DateValidation(TxtVisitDate, LblVisitDate, txtError);
                txtError = DateValidation(TxtGraduation, LblGraduation, txtError);
                
                if(this.TxtPwd.Text.Length < 8)
                {
                    this.LblPwd.Text = String.Format( GetMessage("PassWordCheck"), 8);
                    txtError = true;
                }

                //カタカナ氏名入力内容判定
                string kananame = TxtKana.Text;
                kananame = Regex.Replace(kananame, @"\s", "");
                if (!string.IsNullOrEmpty(TxtKana.Text.Trim())
                    && !Regex.IsMatch(kananame,
                     @"^[\p{IsKatakana}\u31F0-\u31FF\u3099-\u309C\uFF65-\uFF9F]+$"))
                {
                    LblKana.Text = GetMessage("TxtErrorMes");
                    txtError = true;
                }
                //メールアドレス入力内容判定
                if (!string.IsNullOrEmpty(TxtMail.Text.Trim()) && !Regex.IsMatch(TxtMail.Text,@"\A[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\z",RegexOptions.IgnoreCase))
                {
                    LblMail.Text = GetMessage("TxtErrorMes");
                    txtError = true;
                }
                //電話番号の判定
                if (!string.IsNullOrEmpty(TxtTelNum.Text.Trim()) && !Regex.IsMatch(TxtTelNum.Text, @"^[0-9]{0,4}-[0-9]{0,5}-[0-9]{0,5}$"))
                {
                        LblTelNum.Text = GetMessage("TxtErrorMes");
                        txtError = true;
                }
                else
                {
                    LblTelNum.Text = "";
                }
                ////郵便番号の判定
                //if (!string.IsNullOrEmpty(TxtPostal.Text.Trim()) && !Regex.IsMatch(TxtPostal.Text, @"^[0-9]{3}-[0-9]{4}$"))
                //{
                //        LblPostal.Text = GetMessage("TxtErrorMes");
                //        txtError = true;
                //}
                //else
                //{
                //    LblPostal.Text = "";
                //}
                //会社番号の判定
                if(!Regex.IsMatch(TxtCompanyNum.Text, @"^[0-9]{0,3}$"))
                {
                    LblCompanyNum.Text = GetMessage("TxtErrorMes");
                    txtError = true;
                }
                else
                {
                    LblCompanyNum.Text = "";
                }
                //エラーがあれば処理せず返す
                if (txtError == true)
                {
                    return;
                }
                //追加・更新処理
                Dictionary<string, object> param = GetEmploInfo();
                if (conectID == "")
                {
                    //追加処理(確認済み)
                    db.ExecuteNonQuerySQL(this.Page, "INSERT_UserInfo", param);
                }
                else
                {
                    //更新処理（確認済み）
                    db.ExecuteNonQuerySQL(this.Page, "UPDATE_UserInfo", param);
                }
                Response.Redirect("./UserList");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// テキストボックスの入力判定
        /// </summary>
        /// <param name="textBox_">判定用テキストボックス</param>
        /// <param name="label_">エラー表示用ラベル</param>
        /// <param name="txtError">エラー判定</param>
        /// <returns></returns>
        private bool TxtBoxValidation(TextBox textBox_, Label label_, bool txtError)
        {
            if (string.IsNullOrEmpty(textBox_.Text.Trim()))
            {
                //空の場合はtrueを返す
                label_.Text = GetMessage("EmptyErrorMes");
                return true;
            }
            else
            {
                //エラー判定の値をそのまま返す
                label_.Text = "";
                return txtError;
            }
        }

        /// <summary>
        /// 日付の入力判定
        /// </summary>
        /// <param name="textBox_">判定用テキストボックス</param>
        /// <param name="label_">エラー表示用ラベル</param>
        /// <param name="txtError">エラー判定</param>
        /// <returns></returns>
        private bool DateValidation(TextBox textBox_, Label label_, bool txtError)
        {
            if (string.IsNullOrEmpty(textBox_.Text.Trim()))
            {
                //空の場合はtrue(error)を返す
                return txtError;
            }
            else if (DateTime.TryParse(textBox_.Text, out _) == false)
            {
                //日付ではない場合はtrue(error)を返す
                label_.Text = GetMessage("DateErrorMes");
                return true;
            }
            else
            {
                //エラー判定の値をそのまま返す
                label_.Text = "";
                return txtError;
            }
        }

        /// <summary>
        /// 入力項目の取得
        /// </summary>
        /// <returns>Dictinary型string, object</returns>
        private Dictionary<string,object> GetEmploInfo()
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("USER", formatUtil.characterConvert(TxtUser.Text));
            param.Add("USERNAME", formatUtil.characterConvert(TxtUserName.Text));
            param.Add("PWD", this.EncryptString(TxtPwd.Text));
            param.Add("COMNUM", ChangeInt (TxtCompanyNum.Text,null));
            param.Add("COMNAME",ChangeString(TxtCompany.Text));
            param.Add("DEPART", DdlDepart.SelectedValue);
            param.Add("ENTRYDATE", ChangeDate(TxtEntryDate.Text, null));
            param.Add("BIRTHDAY", ChangeDate(TxtBirthDay.Text, null));
            param.Add("AUTHORITY", DdlAuthority.SelectedValue);
            param.Add("TELNUM", TxtTelNum.Text);
            param.Add("MAIL", TxtMail.Text);
            param.Add("KANA", TxtKana.Text);
            param.Add("SEX", DdlSex.SelectedValue);
            param.Add("NATIONAL", DdlNational.SelectedValue); 
            param.Add("VISITDATE", ChangeDate(TxtVisitDate.Text, null));
            param.Add("SCHOOL", ChangeString(TxtSchool.Text));
            param.Add("MAJOR",ChangeString(TxtMajor.Text));
            param.Add("DEGREE", DdlDegree.SelectedValue);
            param.Add("GRADUATION", ChangeDate(TxtGraduation.Text, null));
            param.Add("ADRESS", ChangeString(TxtAddress.Text));
            param.Add("NEARBY", ChangeString(TxtNearby.Text));
            param.Add("NICKNAME", ChangeString(TxtNickName.Text));
            param.Add("EMPFLAG", DdlEmployeeFlag.SelectedValue);
            param.Add("STUDIES", DdlStudies.SelectedValue);
            param.Add("NOTE", ChangeString(TxtNote.Text));
            param.Add("POSTAL", ChangeString(TxtPostal.Text.Replace("-", ""), null));
            param.Add("RECORDER", LoginUser.UserName);
            param.Add("RECORDDATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("UPDATER", LoginUser.UserName);
            param.Add("UPDATEDATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("BUKALIST", ViewState["BukaID"] == null ? "" : ViewState["BukaID"].ToString());
            param.Add("USERID", conectID);
            return param;
        }

         /// <summary>
         /// キャンセルボタン処理
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("./UserList");
        }
    }
}
    
