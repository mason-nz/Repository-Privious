using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Data.OleDb;
using System.IO;
using BitAuto.YanFa.Crm2009;

namespace BitAuto.ISDC.CC2012.Web.MagazineReturn.MagezineInfoImport
{
    public class MagezineInfoImportHelper
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

        /// <summary>
        /// 上传文件根目录 /UploadFiles/BackMoney/
        /// </summary>
        private  string UpladFilesPath = BitAuto.ISDC.CC2012.BLL.Util.GetUploadTemp("\\");

        internal bool BatchImport(out string msg)
        {
            //保存文件
            string fileName = this.SaveFile();
          
            bool flag = DealDataImported(fileName, out msg);

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
            //else if (ext.ToLower() == ".xlsx")
            //{
            //    connStr = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + @fileName + ";Extended Properties=Excel 12.0;";
            //}
            else
            {
                //throw new Exception("上传文件应为xls或者xlsx格式的文件");
                throw new Exception("上传文件应为xls格式的文件");
            }
            //(2)读取校验数据
            List<MagezineInfoImportExceptionInfo> exceptionInfoList = new List<MagezineInfoImportExceptionInfo>();
            Dictionary<Entities.CC_MagazineReturn, string> magazineInfoList = new Dictionary<Entities.CC_MagazineReturn, string>();
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
                    if (idr[0].ToString().Trim().Equals("客户ID"))
                    {
                        startReadRow = rowNum;
                    }
                    if (rowNum > startReadRow)
                    {
                        this.Valid(rowNum, idr, exceptionInfoList, magazineInfoList);
                    }
                }
                idr.Close();
                idr = null;
            }
            //(3)如果校验通过，插入数据
            if (exceptionInfoList.Count > 0)
            {
                msg = JavaScriptConvert.SerializeObject(exceptionInfoList);
                success = false;
            }
            else
            {
                //System.Configuration.ConfigurationManager.AppSettings[];
                //插入数据

                foreach (KeyValuePair<Entities.CC_MagazineReturn, string> info in magazineInfoList)
                {
                    BitAuto.YanFa.Crm2009.Entities.MagazineDeliver deliver = new BitAuto.YanFa.Crm2009.Entities.MagazineDeliver();
                    deliver.CooperationName = "第" + CooperationName + "期";
                    deliver.CreateTime = DateTime.Now;
                    deliver.CreateUserID =int.Parse( UserID);
                    deliver.ExecCycle = ExecCycle;
                    deliver.Remark = "";
                    deliver.CustID = info.Key.CustID;
                    deliver.Type = 1;
                    deliver.Status = 0;
                    deliver.Remark = info.Value;
                    info.Key.CMDID = BitAuto.YanFa.Crm2009.BLL.MagazineDeliver.Instance.Insert(deliver);

                    BLL.CC_MagazineReturn.Instance.Insert(info.Key);
                }
                msg = "成功导入" + magazineInfoList.Count + "条数据";
            }
          
            return success;
        }

        /// <summary>
        /// 合同和它的金额(数组：0-总金额 1-现有金额)
        /// </summary>
        //private Dictionary<string, decimal[]> contractMoney = new Dictionary<string, decimal[]>();

        /// <summary>
        /// 校验
        /// </summary>
        private bool Valid(int rowNum, IDataReader reader, List<MagezineInfoImportExceptionInfo> exceptionInfoList, Dictionary< Entities.CC_MagazineReturn, string> magezineInfoList)
        {
            bool currentSuccess = true;
            MagezineInfoImportExceptionInfo ex = new MagezineInfoImportExceptionInfo();

            #region validate

            string custId = reader[0].ToString().Trim();
            string memberCode = reader[2].ToString().Trim();
            string contacter = reader[4].ToString().Trim(); ;

            int contractId = -1;
            string dmsMemberID = string.Empty;

            if (string.IsNullOrEmpty(custId) && string.IsNullOrEmpty(memberCode) && string.IsNullOrEmpty(contacter))
            {
                return currentSuccess;//此行为空行
            }

            //判断UserID参数

            int intval=0;
            if (!int.TryParse(UserID, out intval))
            {
                ex.Info.Append("用户ID参数不正确");
                currentSuccess = false;
            }

            //客户信息验证
            if (string.IsNullOrEmpty(custId))
            {
                ex.Info.Append("第" + rowNum + "行此[客户ID]不可为空。");
                currentSuccess = false;
            }
            else
            {
                BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(custId);
                if (custInfo == null)
                {
                    ex.Info.Append("第" + rowNum + "行此[客户ID]不存在。");
                    currentSuccess = false;
                }
            }

            //会员信息验证
            if (string.IsNullOrEmpty(memberCode))
            {
                ex.Info.Append("第" + rowNum + "行此[会员ID]不可为空。");
                currentSuccess = false;
            }
            else
            {
                List<BitAuto.YanFa.Crm2009.Entities.DMSMember> dmsMembers = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByCustID(custId);

                if (dmsMembers == null)
                {
                    ex.Info.Append("第" + rowNum + "行此[会员ID]不存在。");
                    currentSuccess = false;
                }
                else
                {
                    bool isExist = false;
                    foreach (BitAuto.YanFa.Crm2009.Entities.DMSMember dmsMember in dmsMembers)
                    {
                        if (dmsMember.MemberCode == memberCode.Trim())
                        {
                            isExist = true;
                            dmsMemberID = dmsMember.ID.ToString();
                        }
                    }
                    if (!isExist)
                    {
                        ex.Info.Append("第" + rowNum + "行此客户下不存在此[会员ID]。");
                        currentSuccess = false;
                    }
                }
            }

            if (string.IsNullOrEmpty(contacter))
            {
                ex.Info.Append("第" + rowNum + "行此[联系人]不可为空。");
                currentSuccess = false;
            }
            else
            {
                contacter = contacter.Replace('，', ',');
                string[] contacterArry = contacter.Split(',');

                foreach (string cter in contacterArry)
                {
                    BitAuto.YanFa.Crm2009.Entities.QueryContactInfo query = new BitAuto.YanFa.Crm2009.Entities.QueryContactInfo();
                    query.CustID = custId;
                    query.CName = cter.Trim();

                    int totalCount = 0;
                    DataTable dt = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfo(query, "", 1, 1, out totalCount);
                    if (dt.Rows.Count <= 0)
                    {
                        ex.Info.Append("第" + rowNum + "行联系人<" + cter + ">不存在。");
                        currentSuccess = false;
                    }
                    else
                    {
                        contractId = int.Parse(dt.Rows[0]["ID"].ToString());
                    }
                }
            }

            if (string.IsNullOrEmpty(CooperationName))
            {
                ex.Info.Append("合作名称不能为空");
                currentSuccess = false;
            }
            else
            {
                Entities.QueryCC_MagazineReturn query = new Entities.QueryCC_MagazineReturn();
                query.Title = "第" + cooperationName + "期";
                query.Status = 0;
                query.DMSMemberID = dmsMemberID;
                int totalCount = 0;
                DataTable dt = BLL.CC_MagazineReturn.Instance.GetCC_MagazineReturn(query, "", 1, 1, out totalCount);
                if (dt.Rows.Count > 0)
                {
                    ex.Info.Append("第" + rowNum + "行已经存在此期的会员");
                    currentSuccess = false;
                }
            }

            #endregion
            //如通过则插入数据
            if (currentSuccess)
            {
                Entities.CC_MagazineReturn info = new Entities.CC_MagazineReturn();
                info.ContactID = contractId;
                info.CreateTime = DateTime.Now;
                info.CreateUserID = int.Parse(UserID);
                info.CustID = custId;
                info.DMSMemberID = dmsMemberID;
                info.Title = "第" + cooperationName + "期";
                info.Status = 0;
                magezineInfoList.Add(info, contacter);
            }
            else
            {
                exceptionInfoList.Add(ex);
            }

            return currentSuccess;
        }
    }
    /// <summary>
    /// 批量导入时，校验EXCEL的异常信息
    /// </summary>
    public class MagezineInfoImportExceptionInfo
    {
        private StringBuilder info = new StringBuilder();
        /// <summary>
        /// 附加信息
        /// </summary>
        [JsonIgnore]
        public StringBuilder Info { get { return info; } set { info = value; } }

        public string Infomation { get { return info.ToString(); } }

        public MagezineInfoImportExceptionInfo()
        {
        }
    }
}