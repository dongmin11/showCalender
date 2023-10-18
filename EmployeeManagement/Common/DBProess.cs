using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
namespace EmployeeManagement.Common
{
    public class DBProess
    {
        log4net.ILog SQLLog = log4net.LogManager.GetLogger("SQLLog");

        /// <summary>
        /// データベース接続情報
        /// </summary>
        public SqlConnection conSql = null;

        /// <summary>
        /// データ検索
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sqlID"></param>
        /// <param name="parameters"></param>
        /// <returns>List<Dictionary<string, object>></returns>
        public List<Dictionary<string, object>> ExecuteSQForList(Page page, string sqlID, Dictionary<string, object> parameters = null)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            Dictionary<string, object> itemData_ = null;
            DataTable dt = new DataTable();
            //キャッシュからデータ取得

            String strContext = GetSqlContext(page, sqlID);
            strContext = GetSqlStatement(strContext, parameters);
            //パラメータ作成
            if (strContext != "")
            {
                dt = this.ExecuteQuerySQL(strContext);
                foreach (DataRow r in dt.Rows)
                {
                    itemData_ = new Dictionary<string, object>();
                    int cnt = 0;
                    foreach (object k in r.ItemArray.ToList())
                    {

                        string typeName = k.GetType().Name;
                        switch (typeName)
                        {
                            case "DateTime":
                                itemData_[dt.Columns[cnt].ColumnName] = (DateTime)k;
                                break;
                            default:
                                itemData_[dt.Columns[cnt].ColumnName] = k;
                                break;
                        }
                        cnt += 1;
                    }
                    results.Add(itemData_);
                }
            }

            return results;
        }

        /// <summary>
        /// データ検索
        /// </summary>
        /// <param name="sqlContext"></param>
        /// <returns>List<Dictionary<string, object>></returns>
        public List<Dictionary<string, object>> ExecuteSQForList(string sqlContext)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            Dictionary<string, object> itemData_ = null;
            DataTable dt = new DataTable();
            //パラメータ作成
            if (sqlContext != "")
            {
                dt = this.ExecuteQuerySQL(sqlContext);
                foreach (DataRow r in dt.Rows)
                {
                    itemData_ = new Dictionary<string, object>();
                    int cnt = 0;
                    foreach (object k in r.ItemArray.ToList())
                    {

                        string typeName = k.GetType().Name;
                        switch (typeName)
                        {
                            case "DateTime":
                                itemData_[dt.Columns[cnt].ColumnName] = (DateTime)k;
                                break;
                            default:
                                itemData_[dt.Columns[cnt].ColumnName] = k;
                                break;
                        }
                        cnt += 1;
                    }
                    results.Add(itemData_);
                }
            }

            return results;
        }

        /// <summary>
        /// データ検索
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sqlID"></param>
        /// <param name="parameters"></param>
        /// <returns>データ検索</returns>
        public DataTable ExecuteSQLForDataTable(Page page, string sqlID, Dictionary<string, object> parameters = null)
        {
            DataTable dt = new DataTable();
            //キャッシュからデータ取得

            String strContext = GetSqlContext(page, sqlID);
            strContext = GetSqlStatement(strContext, parameters);
            //パラメータ作成
            if (strContext != "")
            {
                dt = this.ExecuteQuerySQL(strContext);
            }
            return dt;
        }

        /// <summary>
        /// データ検索
        /// </summary>
        /// <param name="sqlContext"></param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteSQLForDataTable(string sqlContext)
        {
            DataTable dt = new DataTable();

            //パラメータ作成
            if (sqlContext != "")
            {
                dt = this.ExecuteQuerySQL(sqlContext);

            }
            return dt;
        }


        /// <summary>
        /// 更新処理(SQL文を利用)
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public int ExecuteNonQuerySQL(string sqlContext)
        {
            int intUpdate = 0;
            SqlCommand cmd = new SqlCommand();
            try
            {
                //データベースに接続を取る
                dbConnection();
                cmd.Connection = conSql;
                cmd.CommandText = sqlContext;
                intUpdate = cmd.ExecuteNonQuery();
                SQLLog.Info("--" + sqlContext.Replace("\r", "").Replace("\n", ""));
            }
            catch (Exception ex)
            {
                intUpdate = -1;
                SQLLog.Error("--" + ex.Message.ToString() + ",  --" + sqlContext.Replace("\r", "").Replace("\n", ""));
                throw ex;
            }
            return intUpdate;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public int ExecuteNonQuerySQL(Page page, string sqlID, Dictionary<string, object> parameters = null)
        {
            int intUpdate = 0;
            SqlCommand cmd = new SqlCommand();

            String strContext = GetSqlContext(page, sqlID);
            strContext = GetSqlStatement(strContext, parameters);
            try
            {
                //データベースに接続を取る
                dbConnection();
                cmd.Connection = conSql;
                cmd.CommandText = strContext;
                intUpdate = cmd.ExecuteNonQuery();
                SQLLog.Info("--" + strContext.Replace("\r", "").Replace("\n", ""));
            }
            catch (Exception ex)
            {
                intUpdate = -1;
                SQLLog.Error("--" + ex.Message.ToString() + ",  --" + strContext.Replace("\r", "").Replace("\n", ""));
                throw ex;
            }
            return intUpdate;
        }

        #region #メソッド private
        /// <summary>
        /// SQL文を作成
        /// </summary>
        /// <param name="strContext"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private String GetSqlStatement(String strContext, Dictionary<string, object> parameters = null)
        {
            String ret = strContext;
            if (parameters != null)
            {
                foreach (KeyValuePair<String, object> param in parameters)
                {
                    if (param.Value == null)
                    {
                        ret = ret.Replace("'#" + param.Key + "#'", "null");
                        ret = ret.Replace("#" + param.Key + "#", "null");
                    }
                    else
                    {
                        ret = ret.Replace("#" + param.Key + "#", param.Value.ToString());
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// SQL文を取得
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sqlID"></param>
        /// <returns></returns>
        private String GetSqlContext(Page page, string sqlID)
        {
            String ret = "";
            List<Dictionary<string, object>> sqlMstList = new List<Dictionary<string, object>>();
            if (page.Cache.Get("SQLMst") != null)
            {
                sqlMstList = page.Cache.Get("SQLMst") as List<Dictionary<string, object>>;
            }
            else
            {
                sqlMstList = ExecuteSQList("SQLマスタ");
                page.Cache.Insert("SQLMst", sqlMstList);

            }
            ret = sqlMstList.Where(x => x["SQLID"].ToString() == sqlID).Select(x => x["SqlContext"].ToString()).FirstOrDefault();
            return ret;
        }


        /// <summary>
        /// DB接続
        /// </summary>
        /// <returns></returns>
        private bool dbConnection()
        {
            // 接続は無しの場合
            if (conSql == null)
            {
                // データベース情報を取得する
                String strConn = ConfigurationManager.AppSettings["connStr"] + "User ID=" + ConfigurationManager.AppSettings["uid"] + ";Password=" + ConfigurationManager.AppSettings["pwd"];
                conSql = new SqlConnection(strConn);
                conSql.Open();
            }
            else if (conSql.State == ConnectionState.Closed)
            {
                // DB接続を開く
                conSql.Open();
            }
            return true;
        }

        /// <summary>
        /// マスタ検索
        /// </summary>
        /// <param name="tableNm"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> ExecuteSQList(String tableNm)
        {
            Dictionary<string, object> itemData_ = new Dictionary<string, object>();

            DataTable dt = ExecuteQuerySQL("SELECT * FROM " + tableNm);
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            foreach (DataRow r in dt.Rows)
            {
                itemData_ = new Dictionary<string, object>();
                int cnt = 0;
                foreach (object k in r.ItemArray.ToList())
                {

                    string typeName = k.GetType().Name;
                    switch (typeName)
                    {
                        case "DateTime":
                            itemData_[dt.Columns[cnt].ColumnName] = (DateTime)k;
                            break;
                        default:
                            itemData_[dt.Columns[cnt].ColumnName] = k.ToString();
                            break;
                    }
                    cnt += 1;
                }
                results.Add(itemData_);
            }
            return results;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        private DataTable ExecuteQuerySQL(string sqlStr)
        {
            
            DataTable dt = new DataTable();
            try
            {
                //データベースに接続を取る
                dbConnection();
                //SQL文実行
                SqlDataAdapter adpt = new SqlDataAdapter(sqlStr, conSql);
                adpt.Fill(dt);
                SQLLog.Info("--" + sqlStr);
            }
            catch (Exception ex)
            {
                SQLLog.Error("--"  + ex.Message.ToString() + ",  --" + sqlStr);
                throw ex;
            }
            return dt;
        }
        #endregion
    }
}