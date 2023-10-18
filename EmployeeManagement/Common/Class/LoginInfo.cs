using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Common.Class
{
    public class LoginInfo
    {
        /// <summary>
        /// ログインユーザ番号
        /// </summary>
        private long userNo;

        /// <summary>
        /// ログインID
        /// </summary>
        private string loginID;

        /// <summary>
        /// ログイン名前
        /// </summary>
        private string userName;

        /// <summary>
        /// 社員名称表示
        /// </summary>
        private string userNameShort;

        /// <summary>
        /// 性別
        /// </summary>
        private int sex;

        /// <summary>
        /// 人事権限
        /// </summary>
        private int authority;

        /// <summary>
        /// 部署ID
        /// </summary>
        private int syozokuID;

        /// <summary>
        /// 部署名称
        /// </summary>
        private string syozokuName;

        /// <summary>
        /// 部下情報
        /// </summary>
        private string bukaInfo;

        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="user">ユーザ情報</param>
        public LoginInfo(Dictionary<string, object> user)
        {
            this.userNo = long.Parse(user["社員番号"].ToString());
            this.loginID = user["ユーザー"].ToString();
            this.userName = user["社員名称"].ToString();
            this.userNameShort = user["社員名称表示"] != null ? user["社員名称表示"].ToString() : "";
            this.sex = user["性別"] != null ? int.Parse( user["性別"].ToString()) : 0;
            this.authority = user["人事権限"] != null ? int.Parse(user["人事権限"].ToString()) : 9;
            this.syozokuID = user["所属"] != null ? int.Parse(user["所属"].ToString()) : 0;
            this.bukaInfo = user["社員情報"] != null ? user["社員情報"].ToString() : "";
            this.syozokuName = user["部署名"] != null ? user["部署名"].ToString() : "";
            if (user["社員名称表示"] != null)
            {
                this.userNameShort = user["社員名称表示"].ToString();
            }
            else
            {
                 this.userNameShort = "";
            }
        }

        /// <summary>
        /// ログインユーザ番号
        /// </summary>
        public long UserNo
        {
            get
            {
                return userNo;
            }
            set
            {
                this.userNo = value;
            }
        }

        /// <summary>
        /// ログインID
        /// </summary>
        public string LoginID
        {
            get
            {
                return loginID;
            }
            set
            {
                this.loginID = value;
            }
        }

        /// <summary>
        /// ログイン名前
        /// </summary>
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }

        /// <summary>
        /// 社員名称表示
        /// </summary>
        public string UserNameShort
        {
            get
            {
                return this.userNameShort;
            }
            set
            {
                this.userNameShort = value;
            }
        }

        /// <summary>
        /// 性別
        /// </summary>
        public int Sex
        {
            get
            {
                return this.sex;
            }
            set
            {
                this.sex = value;
            }
        }

        /// <summary>
        /// 人事権限
        /// </summary>
        public int Authority
        {
            get
            {
                return this.authority;
            }
            set
            {
                this.authority = value;
            }
        }

        /// <summary>
        /// 部署ID
        /// </summary>
        public int SyozokuID
        {
            get
            {
                return this.syozokuID;
            }
            set
            {
                this.syozokuID = value;
            }
        }

        /// <summary>
        /// 部署名称
        /// </summary>
        public string SyozokuName
        {
            get
            {
                return this.syozokuName;
            }
            set
            {
                this.syozokuName = value;
            }
        }

        /// <summary>
        /// 部下情報
        /// </summary>
        public string BukaInfo
        {
            get
            {
                return this.bukaInfo;
            }
            set
            {
                this.bukaInfo = value;
            }
        }
    }
}