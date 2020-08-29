using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.Web.Statistics
{
    public class MemberIDImportHelper
    {
        #region Common Properties

        private string action;
        /// <summary>
        /// 小写
        /// </summary>
        public string Action
        {
            get
            {
                return string.IsNullOrEmpty(this.action) ?
                    (this.action = HttpUtility.UrlDecode(this.Request["Action"] + "").Trim().ToLower())
                    : this.action;
            }
            set { this.action = value; }
        }

        //导入类型
        public int CarType
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["CarType"]) ? -1 : int.Parse(this.Request["CarType"]);
            }
        }

        private int currentPage = -1;
        public int CurrentPage
        {
            get
            {
                if (currentPage > 0) { return this.currentPage; }
                //currentPage = PagerHelper.GetCurrentPage();
                return currentPage;
            }
            set { currentPage = value; }
        }

        private int pageSize = -1;
        public int PageSize
        {
            get
            {
                if (pageSize > 0) { return this.pageSize; }
                //pageSize = PagerHelper.GetPageSize();
                return pageSize;
            }
            set { pageSize = value; }
        }

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        #endregion


        /// <summary>
        /// 上传文件根目录 /UploadFiles/BackMoney/
        /// </summary>
        private  string UpladFilesPath = BitAuto.ISDC.CC2012.BLL.Util.GetUploadTemp("\\");

        internal bool BatchImport(out string msg)
        {
            //保存文件
            string fileName = this.SaveFile();

            //校验 插入
            bool flag= DealDataImported(fileName, out msg);

            Directory.Delete(UpladFilesPath, true);

            return flag;

        }

        /// <summary>
        /// 保存文件
        /// </summary>
        private string SaveFile()
        {           
            HttpPostedFile hpf = System.Web.HttpContext.Current.Request.Files["Filedata"];
            if (hpf.ContentLength > 0)
            {
                //添加文件路径信息
                string fullName = GenPath(hpf.FileName);
                hpf.SaveAs(fullName);//保存上载文件
                return fullName;
            }
            else { throw new Exception("没有上传文件"); }
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <returns>绝对路径</returns>
        private string GenPath(string fileName)
        {
            string ext = Path.GetExtension(fileName);//文件后缀名
            string name = Path.GetFileNameWithoutExtension(fileName);
            name = name + "€" + Guid.NewGuid().ToString() + ext;

            if (!Directory.Exists(UpladFilesPath))
            {
                Directory.CreateDirectory(UpladFilesPath);
            }

            return Path.Combine(UpladFilesPath, name);
        }

      

        /// <summary>
        /// 处理导入的数据
        /// </summary>
        private bool DealDataImported(string fileName, out string msg)
        {
            bool success = true;
            msg = "";
            //(1)连接EXCEL文件
            string ext = Path.GetExtension(fileName);
            string connStr = string.Empty;
            if (ext.ToLower() == ".xls")
            {
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                throw new Exception("上传文件应为xls格式的文件");
            }
            //(2)读取校验数据
            List<MemberIDImportExceptionInfo> exceptionInfoList = new List<MemberIDImportExceptionInfo>();
            string MemberStr = string.Empty;
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等    
                DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                //包含excel中表名的字符串数组
                string firstSheetName = dtSheetName.Rows[0]["TABLE_NAME"].ToString();
                //读取第一个sheet填充数据sheetConfig.Name.Replace(".", "#") + "$"
                OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName.Replace(".", "#") + "]", conn);
                IDataReader idr = command.ExecuteReader();
                int rowNum = 1;
                int startReadRow = 0;
                while (idr.Read())
                {
                    ++rowNum;
                    if (idr[0].ToString().Trim().Contains("会员ID号"))
                    {
                        startReadRow = rowNum;
                    }
                    if (rowNum > startReadRow)
                    {
                        success = this.Valid(rowNum, idr, exceptionInfoList, ref MemberStr);
                    }
                }
                idr.Close();
                idr = null;
            }
            //(3)如果校验通过，导出数据
            if (success == false)
            {
                msg = JavaScriptConvert.SerializeObject(exceptionInfoList);
            }
            else
            {
                MemberStr = MemberStr.Substring(0, MemberStr.Length - 1);
                msg = MemberStr;
            }
        
            return success;
        }

        /// <summary>
        /// 校验
        /// </summary>
        private bool Valid(int rowNum, IDataReader reader, List<MemberIDImportExceptionInfo> exceptionInfoList, ref string MemberStr)
        {
            bool currentSuccess = true;
            MemberIDImportExceptionInfo ex = new MemberIDImportExceptionInfo();
            string MemberID = string.Empty;
            MemberID = reader[0].ToString().Trim();
            //会员ID号验证
            if (string.IsNullOrEmpty(MemberID) || MemberID.Length == 0)
            {
                ex.Info.Append("第" + rowNum + "行[会员ID号]不可为空。");
            }
            else
            {
                MemberStr += MemberID + ",";
            }
            return currentSuccess;
        }
    }
    /// <summary>
    /// 批量导入时，校验EXCEL的异常信息
    /// </summary>
    public class MemberIDImportExceptionInfo
    {

        private StringBuilder info = new StringBuilder();
        /// <summary>
        /// 附加信息
        /// </summary>
        [JsonIgnore]
        public StringBuilder Info { get { return info; } set { info = value; } }

        public string Infomation { get { return info.ToString(); } }

        public MemberIDImportExceptionInfo()
        {
        }
    }
}