using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Enumerations
{
    /// <summary>
    /// 権限値値列挙型
    /// </summary>
    public class AuthorityEnum
    {
        /// <summary>
        /// 人事権限
        /// </summary>
        public static readonly KbnEnum Personnel = new KbnEnum(1, "人事権限", "人事");

        /// <summary>
        /// 一般権限
        /// </summary>
        public static readonly KbnEnum General = new KbnEnum(9, "一般権限", "一般");
    }

    public class SexEnum 
    {
        /// <summary>
        /// 性別不明
        /// </summary>
        public static readonly KbnEnum NoGender = new KbnEnum(0, "不明", "不");
        /// <summary>
        /// 男性
        /// </summary>
        public static readonly KbnEnum Man = new KbnEnum(1, "男性", "男");
        /// <summary>
        /// 女性
        /// </summary>
        public static readonly KbnEnum Woman = new KbnEnum(2, "女性", "女");
    }

    public class EmployeeFlagEnum
    {
        /// <summary>
        /// 削除フラグ
        /// </summary>
        public static readonly KbnEnum Delete = new KbnEnum(0, "削除", "削");
        /// <summary>
        ///正社員 
        /// </summary>
        public static readonly KbnEnum Parmanent = new KbnEnum(1, "正社員", "正");
        /// <summary>
        /// 外注
        /// </summary>
        public static readonly KbnEnum OutSourse = new KbnEnum(2, "外注", "外");
        /// <summary>
        /// その他
        /// </summary>
        public static readonly KbnEnum Other = new KbnEnum(9, "その他", "他");
    }

    public class SchudleKbnEnum
    {
        /// <summary>
        /// 休み
        /// </summary>
        public static readonly KbnEnum Holiday = new KbnEnum(1, "休み", "休み");
        /// <summary>
        /// 午前半休
        /// </summary>
        public static readonly KbnEnum HalfMorning = new KbnEnum(2, "午前半休", "午前休");
        /// <summary>
        /// 午後半休
        /// </summary>
        public static readonly KbnEnum HalfAfternoon = new KbnEnum(3, "午後半休", "午後休");
        /// <summary>
        /// 社内
        /// </summary>
        public static readonly KbnEnum InCompany = new KbnEnum(4, "社内", "社内");
        /// <summary>
        /// 外出
        /// </summary>
        public static readonly KbnEnum OutCompany = new KbnEnum(5, "外出", "外出");
        /// <summary>
        /// テレワーク
        /// </summary>
        public static readonly KbnEnum Telework = new KbnEnum(6, "テレワーク", "テレ");
        /// <summary>
        /// その他
        /// </summary>
        public static readonly KbnEnum Other = new KbnEnum(9, "その他", "他");
    }

    public class TodoKindEnum
    {
        /// <summary>
        /// 一般掲示・回覧
        /// </summary>
        public static readonly KbnEnum Normal = new KbnEnum(10, "一般掲示・回覧", "掲示");
        /// <summary>
        /// 連絡
        /// </summary>
        public static readonly KbnEnum Communicate = new KbnEnum(20, "連絡", "連絡");
        /// <summary>
        /// 連絡
        /// </summary>
        public static readonly KbnEnum Share = new KbnEnum(30, "共有", "共有");
    }

    public class TdodoAttributeEnum
    {
        /// <summary>
        /// 重要
        /// </summary>
        public static readonly KbnEnum Important = new KbnEnum(1, "[重要]", "重要");
        /// <summary>
        /// 至急
        /// </summary>
        public static readonly KbnEnum Urgent = new KbnEnum(2, "[至急]", "至急");
        /// <summary>
        /// 親展
        /// </summary>
        public static readonly KbnEnum Confidential = new KbnEnum(3, "[親展]", "親展");
        /// <summary>
        /// 転送
        /// </summary>
        public static readonly KbnEnum Transfer = new KbnEnum(4, "[転送]", "転送");
    }
    public class TodoListStaEnum
    {
        /// <summary>
        /// 未完了一覧
        /// </summary>
        public static readonly KbnEnum Uncomplete = new KbnEnum(0, "未完了一覧", "未完了一覧");
        /// <summary>
        /// すべて受信一覧
        /// </summary>
        public static readonly KbnEnum AllReceive = new KbnEnum(1, "すべて受信一覧", "すべて受信一覧");
        /// <summary>
        /// 送信待ち一覧
        /// </summary>
        public static readonly KbnEnum SendWaite = new KbnEnum(0, "送信待ち一覧", "送信待ち一覧");
        /// <summary>
        /// 送信済み一覧
        /// </summary>
        public static readonly KbnEnum Sent = new KbnEnum(1, "送信済み一覧", "送信済み一覧");
    }

    public class DocumentStaEnum
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly KbnEnum Unapproved = new KbnEnum(0, "未承認", "未承認");
        /// <summary>
        /// 
        /// </summary>
        public static readonly KbnEnum Remand = new KbnEnum(1, "差戻し", "差戻し");
        /// <summary>
        /// 
        /// </summary>
        public static readonly KbnEnum Making = new KbnEnum(2, "作成中", "作成中");
        /// <summary>
        /// 
        /// </summary>
        public static readonly KbnEnum WaitTake = new KbnEnum(3, "受取待ち", "受取待ち");
        /// <summary>
        /// 
        /// </summary>
        public static readonly KbnEnum Completed = new KbnEnum(4, "完了", "完了");
        /// <summary>
        /// 
        /// </summary>
        public static readonly KbnEnum Delete = new KbnEnum(9, "削除", "削除");

    }
    /// <summary>
    /// 区分グラス
    /// </summary>
    public class KbnEnum
    {
        public KbnEnum(int id, string name, string shortNm)
        {
            this.Id = id;
            this.Name = name;
            this.ShortName = shortNm;
        }
        

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; }

        public string ShortName { get;  }
    }
}