using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;


namespace EmployeeManagement.Common.Class
{
    public class UploadFile:BasePage
    {
        
        public static string tmppath = ConfigurationManager.AppSettings["ConnectTmpfile"] + LoginUser.UserNo ;
        public static string upladedpath = ConfigurationManager.AppSettings["Connectuploadfile"] + LoginUser.UserNo;

        /// <summary>
        /// 添付ファイル一時保存
        /// </summary>
        /// <param name="fu">ファイルアップロード　コントロール　リスト</param>
        /// <param name="path">フォルダパス</param>
        public static void TmpFileSave(List<FileUpload> fu, string path)
        {
            Directory.CreateDirectory(path);
            if (!Directory.Exists(upladedpath))
            {
                Directory.CreateDirectory(upladedpath);
            }
            int i = 1;
            foreach (FileUpload fileUpload in fu)
            {
                if(fileUpload.PostedFile!=null&&!string.IsNullOrEmpty(fileUpload.PostedFile.FileName))
                {
                    string fileName = fileUpload.PostedFile.FileName;
                    string filePath = Path.Combine(path, i + "_"+ fileName);
                    fileUpload.SaveAs(filePath);
                    i++;
                }
            }
        }

        /// <summary>
        /// フォルダ削除
        /// </summary>
        /// <param name="path">フォルダパス</param>
        public static void FileDelete(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        
    }
}