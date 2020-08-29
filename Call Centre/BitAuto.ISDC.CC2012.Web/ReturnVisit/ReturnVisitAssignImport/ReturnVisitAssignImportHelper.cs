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
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.YanFa.Crm2009.Entities;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Web.Statistics
{
    public class ReturnVisitData//导入数据
    {
        public int UserID { get; set; }
        public string crmCustID { get; set; }//客户ID
        public string AgentNum { get; set; }//工号
        //public bool isYYKF { get; set; }
        //public bool isExistsCust { get; set; }

    }
    public class ReturnVisitAssignImportHelper
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
        private string UpladFilesPath = BitAuto.ISDC.CC2012.BLL.Util.GetUploadTemp("\\");

        internal bool BatchImport(out string msg, out string fileName, out string filePath)
        {
            //保存文件
            fileName = this.SaveFile();

            filePath = UpladFilesPath;
            //校验 插入
            bool flag = CheckDataImported(fileName, filePath, out msg);

            return flag;

        }

        internal bool UpdateImport(string fileName, string filePath, string updateflag)
        {
            //保存文件
            bool flag = true;
            //校验 插入
            try
            {
                string msg = "";

                if (updateflag == "update")
                {
                    flag = DealDataImported(fileName, true);
                }
                else if (updateflag == "onlyupload")
                {
                    flag = DealDataImported(fileName, false);
                }
                else if (updateflag == "editupload")
                {
                    flag = DealDataImported(fileName, true);
                }
                Directory.Delete(filePath, true);

            }
            catch (Exception)
            {

                flag = false;
            }
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
        /// 处理导入数据
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="delOldData">是否删除原始数据</param>
        /// <returns></returns>
        private bool DealDataImported(string fileName, bool delOldData)
        {
            bool success = true;

            string msg = "";
            string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            int rowNum = 1;
            int startReadRow = 0;
            Dictionary<int, List<string>> returnDict = new Dictionary<int, List<string>>();
            try
            {
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
                    OleDbDataAdapter odda = new OleDbDataAdapter(command);
                    odda.Fill(ds);
                }

                foreach (DataRow idr in ds.Tables[0].Rows)
                {
                    if (idr[0].ToString().Trim().Contains("客户ID"))
                    {
                        startReadRow = rowNum;
                    }
                    if (rowNum > startReadRow)
                    {
                        string agentNum = idr[1].ToString();//工号                  
                        string custid = idr[0].ToString();//客户id                          
                        custid = Regex.Replace(custid, @"\D*", "");
                        DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgentsByAgentNum(agentNum);//根据工号获得userid
                        int userid = BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(dt.Rows[0][0]);
                        if (returnDict.ContainsKey(userid))
                        {
                            returnDict[userid].Add(custid);
                        }
                        else
                        {
                            List<string> listAdd = new List<string>();
                            listAdd.Add(custid);
                            returnDict.Add(userid, listAdd);
                        }

                    }
                }

                foreach (KeyValuePair<int, List<string>> item in returnDict)
                {
                    if (delOldData)
                    {
                        AssignTaskNew(item.Key.ToString(), string.Join(",", item.Value.ToArray()), ref msg);

                    }
                    else
                    {
                        AssignUserMapping(item.Key, string.Join(",", item.Value.ToArray()), ref msg);
                    }

                }
            }
            catch (Exception)
            {
               
                success = false;
            }

            return success;
        }

        private bool CheckDataImported(string fileName, string filePath, out string msg)
        {
            bool checkData = true;//验证数据正确性
            msg = "导入出错，请重新导入！";
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
            Dictionary<string, ReturnVisitData> dictCust = new Dictionary<string, ReturnVisitData>();//key 为客户ID， value为（客服ID，客户ID，工号）
            bool isExistYYKF = false;//数据中存在运营客服 默认没有
            bool isExistsCustMember = false;//客户存在新车负责客服  默认没有

            DataSet ds = new DataSet();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等    
                    DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    //包含excel中表名的字符串数组
                    string firstSheetName = dtSheetName.Rows[0]["TABLE_NAME"].ToString();
                    //读取第一个sheet填充数据sheetConfig.Name.Replace(".", "#") + "$"
                    OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName.Replace(".", "#") + "]", conn);
                    OleDbDataAdapter odda = new OleDbDataAdapter(command);
                    odda.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 1000 || ds.Tables[0].Rows.Count == 0)
                {
                    msg = "数据行数必须大于0小于1000！";
                    return false;
                }
                int rowNum = 1;
                int startReadRow = 0;

                foreach (DataRow idr in ds.Tables[0].Rows)
                {
                    if (idr[0].ToString().Trim().Contains("客户ID"))
                    {
                        startReadRow = rowNum;
                    }
                    if (rowNum > startReadRow)
                    {
                        ++rowNum;
                        string agentNum = idr[1].ToString();//工号                  
                        string custid = idr[0].ToString();//客户id                          
                        custid = Regex.Replace(custid, @"\D*", "");
                        if (string.IsNullOrEmpty(custid) || string.IsNullOrEmpty(agentNum))
                        {
                            msg = "导入失败，第" + rowNum + "行客户ID或工号不可为空!";
                            checkData = false;
                            break;
                        }
                        BitAuto.YanFa.Crm2009.Entities.CustInfo custModel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(custid);

                        if (custModel == null)
                        {
                            msg = "导入失败，第" + rowNum + "行数据的客户ID不存在！";
                            checkData = false;
                            break;
                        }
                        if (!dictCust.ContainsKey(custid))
                        {
                            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgentsByAgentNum(agentNum);//根据工号获得userid
                            if (dt.Rows.Count == 0)
                            {
                                msg = "导入失败，第" + rowNum + "行数据的客服工号不存在！";
                                checkData = false;
                                break;
                            }
                            else
                            {
                                dictCust.Add(custid, new ReturnVisitData() { crmCustID = custid, AgentNum = agentNum, UserID = BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(dt.Rows[0][0]) });
                            }
                        }
                        else
                        {
                            msg = "导入失败，第" + rowNum + "数据中存在相同客户ID!";
                            checkData = false;
                            break;
                        }
                    }
                
                }
                if (checkData)
                {
                    int row = 0;

                    foreach (KeyValuePair<string, ReturnVisitData> item in dictCust)
                    {
                        row++;
                        bool isYYKF = BLL.ProjectTask_ReturnVisit.Instance.IsYYKF(item.Value.UserID);//是运行客服
                        if (isYYKF)
                        {
                            isExistYYKF = true;
                            // bool existFlag = BLL.EmployeeAgent.Instance.GetEmployeeAgentsByAgentNumBussniess(item.Key);
                            bool ExistsCustMember = BLL.ProjectTask_ReturnVisit.Instance.IsExistsCustMember(item.Value.UserID, "'" + item.Key + "'");
                            if (ExistsCustMember)//客户已存在新车负责客服
                            {
                                isExistsCustMember = true;
                                msg = "custidexists";
                                checkData = false;
                                break;
                            }
                        }

                    }
                    if (!isExistYYKF)//不存在运营客服 直接导入 FF浏览器CC使用直接导入有问题
                    {
                        msg = "onlyupload";
                        checkData = false;
                      //  DealDataImported(fileName, false);

                    }
                    else if (isExistYYKF && !isExistsCustMember)//存在运营客服,切不存在新车负责客服 即修改
                    {
                        msg = "editupload";
                        checkData = false;
                        //DealDataImported(fileName, true);
                    }

                }
                else//文件校验失败 直接删除
                {
                    Directory.Delete(filePath, true);
                    return false;
                }

            }
            catch (Exception ex)
            {

                msg = "分配失败！";
                Directory.Delete(filePath, true);
                checkData = false;
            }
            return checkData;

        }


        private void AssignUserMapping(int userId, string custIds, ref string msg)
        {

            try
            {
                BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.InsertBatch(custIds, userId);
                BLL.Util.InsertUserLog("分配客户ID是" + custIds + "的客户的负责员工" + userId);
           
            }
            catch (Exception ex)
            {
                msg = "分配失败！";
            }
        }

        private void AssignTaskNew(string userIds, string custIds, ref string msg)
        {
            string memberIdList = custIds;
            string[] userIdArry = userIds.Split(',');
            try
            {
                foreach (string userIdStr in userIdArry)
                {
                    int userId = -1;
                    if (int.TryParse(userIdStr, out userId))
                    {
                        //删除该客户在该业务线上的运营客服
                        //   BLL.ProjectTask_ReturnVisit.Instance.DeleteCustMemberOfBL(userId, memberIdList);
                        memberIdList = System.Text.RegularExpressions.Regex.Replace(memberIdList, @"\'", "");
                        BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.DeleteCustMemberByNewCar(userId, memberIdList);

                        //给CRM日志记录中添加数据
                        BitAuto.YanFa.Crm2009.BLL.UserActionLog.Instance.InsertLog("删除原来所分配的该业务线的运营客服;客户ID:" + memberIdList + ";负责运营坐席：" + userId, "", EnumActionType.Delete);

                        BLL.Util.InsertUserLog("删除分配客户ID是" + memberIdList + "的客户的负责员工" + userId);
                        custIds = custIds.Replace("'", "");
                        BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.InsertBatch(custIds, userId);

                        BLL.Util.InsertUserLog("分配客户ID是" + custIds + "的客户的负责员工" + userId);
                    }
                }
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = "分配客户出现异常";
                BLL.Util.InsertUserLog("分配客户出现异常，异常原因：" + ex.Message);
            }
        }

    }

}