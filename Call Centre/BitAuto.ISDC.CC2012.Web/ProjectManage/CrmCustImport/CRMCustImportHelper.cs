using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.OleDb;
using System.Data;
using Newtonsoft.Json;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage.CrmCustImport
{
    public class CRMCustImportHelper
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

        private string userid;
        public string UserID
        {
            get
            {
                return string.IsNullOrEmpty(this.userid) ?
                    (this.userid = HttpUtility.UrlDecode(this.Request["userid"] + "").Trim().ToLower())
                    : this.userid;
            }
            set { this.userid = value; }
        }
        private string cooperationName;
        public string CooperationName
        {
            get
            {
                return string.IsNullOrEmpty(this.cooperationName) ?
                    (this.cooperationName = HttpUtility.UrlDecode(this.Request["CooperationName"] + "").Trim().ToLower())
                    : this.cooperationName;
            }
            set { this.cooperationName = value; }
        }

        private string execCycle;
        public string ExecCycle
        {
            get
            {
                return string.IsNullOrEmpty(this.execCycle) ?
                    (this.execCycle = HttpUtility.UrlDecode(this.Request["ExecCycle"] + "").Trim().ToLower())
                    : this.execCycle;
            }
            set { this.execCycle = value; }
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

        public bool BatchImport(out string msg)
        {
            //保存文件
            string fileName = this.SaveFile();

            return DealDataImported(fileName, out msg);
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
            string path = BLL.Util.GetUploadTemp("\\");
            string fullName = path + fileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return fullName;
        }
        private void ClearFiles(string fullName)
        {
            string path = Path.GetDirectoryName(fullName);
            Directory.Delete(path, true);
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
            List<ImportExceptionInfo> exceptionInfoList = new List<ImportExceptionInfo>();
            Dictionary<Entities.CC_MagazineReturn, string> magazineInfoList = new Dictionary<Entities.CC_MagazineReturn, string>();

            List<string> custidList = new List<string>();
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等    
                DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                //包含excel中表名的字符串数组
                string firstSheetName = dtSheetName.Rows[0]["TABLE_NAME"].ToString();
                //读取第一个sheet填充数据sheetConfig.Name.Replace(".", "#") + "$"
                OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName.Replace(".", "#") + "]", conn);
                OleDbDataAdapter adp = new OleDbDataAdapter(command);
                adp.Fill(ds);
                if (ds != null && ds.Tables[0].Rows.Count > 20000)
                {
                    msg = "导入文件中有" + ds.Tables[0].Rows.Count + "条数据，但一个项目中不能超过20000条";
                    return false;
                }
            }

            //校验数据
            this.Valid(ds, exceptionInfoList, sb);

            //(3)如果校验通过，插入数据
            if (exceptionInfoList.Count > 0)
            {
                msg = JavaScriptConvert.SerializeObject(exceptionInfoList);
                success = false;
            }
            else
            {
                msg = sb.ToString().Substring(0, sb.Length - 1);
            }
            //删除文件
            ClearFiles(fileName);
            return success;
        }

        /// <summary>
        /// 校验
        /// </summary>
        private bool Valid(DataSet ds, List<ImportExceptionInfo> exceptionInfoList, StringBuilder sb)
        {
            bool currentSuccess = true;
            ImportExceptionInfo ex = new ImportExceptionInfo();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(dr[0].ToString() + ",");
            }
            string custIDs = sb.ToString().Substring(0, sb.Length - 1);

            #region 判断客户ID是否存在

            DataTable custDt = BLL.ProjectInfo.Instance.p_GerNoExistsCustID(custIDs);
            if (custDt.Rows.Count > 0)
            {
                StringBuilder noExistsCustIDs = new StringBuilder();
                foreach (DataRow dr in custDt.Rows)
                {
                    noExistsCustIDs.Append(dr[0].ToString() + ",");
                }
                ex.Info.Append("Excel中有的客户ID不存在：<br/>" + noExistsCustIDs.ToString());
                currentSuccess = false;
                exceptionInfoList.Add(ex);
            }

            #endregion

            return currentSuccess;
        }
    }

    public class ImportExceptionInfo
    {
        private StringBuilder info = new StringBuilder();
        /// <summary>
        /// 附加信息
        /// </summary>
        [JsonIgnore]
        public StringBuilder Info { get { return info; } set { info = value; } }

        public string Infomation { get { return info.ToString(); } }

        public ImportExceptionInfo()
        {
        }
    }
}