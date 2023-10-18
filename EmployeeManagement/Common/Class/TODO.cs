using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;

namespace EmployeeManagement.Common.Class
{
    public class TODO:BasePage
    {
        /// <summary>
        /// TODO内容データテーブル
        /// </summary>
        /// <param name="uploadfile">添付ファイル</param>
        /// <returns></returns>
        public DataTable ContentsInputted(bool uploadfile)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("todoSta");   //todo発信状態
            dt.Columns.Add("userid");　　//発信者
            dt.Columns.Add("kind");      //種別
            dt.Columns.Add("title");     //タイトル
            dt.Columns.Add("detail");    //本文
            if(uploadfile==true)
            {
                dt.Columns.Add("file1");  //添付ファイル
                dt.Columns.Add("file2");
                dt.Columns.Add("file3");
            }
            dt.Columns.Add("timeNow");  　//発信日時
            dt.Columns.Add("userid1");　　//登録ユーザー
            dt.Columns.Add("timeNow1");   //登録日時
            dt.Columns.Add("userid2");　　//更新ユーザー
            dt.Columns.Add("timeNow2");　//更新日時
            dt.Columns.Add("attribute"); //属性
            return dt;
            
        }

        /// <summary>
        /// TODO送信
        /// </summary>
        /// <param name="page"></param>
        /// <param name="data">TODO内容データテーブル</param>
        /// <param name="rcdt">受信者データテーブル</param>
        public void SendTodo(Page page ,DataTable data,DataTable rcdt)
        {
            //string timenow = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            //ViewState["time"] = timenow;
            Dictionary<string, object> para = new Dictionary<string, object>();
            DataRow dataRow = data.Rows[0];

            //TodoリストINSERtT用パラメータ取得
            foreach (DataColumn dataColumn in data.Columns)
            {
                string columnname = dataColumn.ColumnName;
                para.Add(columnname,dataRow[columnname].ToString());
            }
            if(data.Columns.Count==11)
            {
                para.Add("file1",null);
                para.Add("file2", null);
                para.Add("file3", null);
            }
            db.ExecuteNonQuerySQL(page,"INSERT_ToDolist", para);　//TODOリストINSERTｓｑｌ
            //ToDoデータ取得
            StringBuilder require = new StringBuilder();
            require.AppendLine($"t.[発信者番号] = {LoginUser.UserNo}");
            require.AppendLine($" AND [発信日時] = '{para["timeNow"]}'");
            para.Add("require", require);
            para.Add("subtable", "");
            DataTable dt = db.ExecuteSQLForDataTable(page, "SELECT_TodoList", para);
            DataRow row = dt.Rows[0];
            string todoNo = row["ToDo番号"].ToString();
            para.Add("todoNo", todoNo);
            //Todo明細INSERT
            RecerverInsert(page,rcdt,para);
        }

        /// <summary>
        /// Todo明細INSERT
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rcdt">受信者データテーブル</param>
        /// <param name="para">TODO明細INSERT文用パラメータ</param>
        private void RecerverInsert(Page page,DataTable rcdt, Dictionary<string, object> para)
        {  
            foreach (DataRow dr in rcdt.Rows)
            {
                para.Add("receiverNo", dr["社員番号"].ToString());
                db.ExecuteNonQuerySQL(page, "INSERT_ToDoDetail", para);
                para.Remove("receiverNo");
            }
        }

        /// <summary>
        /// Todo本文作成
        /// </summary>
        /// <param name="filepath">本文テキストファイルパス</param>
        /// <param name="parameters">本文入れ替え用パラメータ</param>
        /// <returns></returns>
        public string TodoText(string filepath, Dictionary<string, object> parameters = null)
        {
            string text = "";
            if(File.Exists(filepath))
            {
                StreamReader sr = new StreamReader(filepath);
                text = sr.ReadToEnd();
            }
            if (parameters != null)
            {
                foreach (KeyValuePair<String, object> param in parameters)
                {
                    if (param.Value == null)
                    {
                        text = text.Replace("'#" + param.Key + "#'", "");
                        text = text.Replace("#" + param.Key + "#", "");
                    }
                    else
                    {
                        text = text.Replace("#" + param.Key + "#", param.Value.ToString());
                    }
                }
            }
            return text;
        }
    }
}