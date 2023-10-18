using System;
using BaseUtil;

namespace EmployeeManagement.Common
{
    public class CommonUtil
    {
        /// <summary>
        /// 文字列変換
        /// (空欄、NULLの場合、基準値に変換)
        /// </summary>
        /// <param name="txt">判定する文字列</param>
        /// <param name="baseline">基準値(指定無しは"")</param>
        /// <returns>基準値または特殊文字変換後の文字列</returns>
        public static string ChangeString(string txt, string baseline = "")
        {
            if (string.IsNullOrEmpty(txt))
            {
                return baseline;
            }
            else
            {
                //FormatUtil formatUtil = new FormatUtil();
                //return formatUtil.characterConvert(txt).ToString();
                if (txt.Contains("'") )
                {
                    txt=txt.Replace("'", "''");
                }
                return txt;
            } 
               
        }

        /// <summary>
        /// 数字(Int)型変換
        /// (変換失敗の場合、基準値に変換)
        /// </summary>
        /// <param name="txt">判定する文字列</param>
        /// <param name="baseline">基準値(指定なしは"0")</param>
        /// <returns>変換後の文字列</returns>
        public static int? ChangeInt(string txt , int? baseline = 0)
        {
            int ret = 0;
            if (int.TryParse(txt, out ret))
            {
                return ret;
            }
            else
            {
                return baseline;
            }
        }

        /// <summary>
        /// 数字(Long)型変換
        /// (変換失敗の場合、基準値に変換)
        /// </summary>
        /// <param name="txt">判定する文字列</param>
        /// <param name="baseline">基準値(指定なし"0")</param>
        /// <returns>変換後の文字列</returns>
        public static long? ChangeLong(string txt, long? baseline = 0)
        {
            long ret = 0;
            if (long.TryParse(txt, out ret))
            {
                return ret;
            }
            else
            {
                return baseline;
            }
        }

        /// <summary>
        /// 日付型変換
        /// (変換失敗の場合、基準値に変換)
        /// </summary>
        /// <param name="txt">判定する文字列</param>
        /// <param name="baseline">基準値(指定なしは"")</param>
        /// <returns>変換後の文字列("yyyy-MM-dd")</returns>
        public static string ChangeDate(string txt , string baseline = "")
        {
            DateTime ret;
            if(DateTime.TryParse(txt,out ret))
            {
                return ret.ToString("yyyy-MM-dd");
            }
            else
            {
                return baseline;
            }
        }
    }
}