using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Web.Util;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Text;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder.ImportExcelWriteBack
{
    /// <summary>
    /// Summary description for HandlerImport
    /// </summary>
    public class HandlerImport : IHttpHandler, IRequiresSessionState
    {

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private string RequestUserID
        {
            get { return HttpContext.Current.Request["userId"] == null ? "" : HttpContext.Current.Request["userId"].ToString(); }
        }

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

        /// <summary>
        /// 集中权限系统登录是的cookies的内容
        /// </summary>
        public string LoginCookiesContent
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("LoginCookiesContent");
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
           
                System.Diagnostics.Debug.WriteLine("[Handler]ProcessRequest begin...");
                context.Response.ContentType = "text/plain";

                try
                {
                    if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                    {
                        switch (Action)
                        {
                            case "batchimport":
                                string msg = "";
                                //AJAXHelper.WrapJsonResponse(BatchImport(out msg), "", msg);
                                BatchImport(out msg);
                                context.Response.Write(msg);
                                break;
                            default:
                                AJAXHelper.WrapJsonResponse(false, "", "没有对应的操作");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AJAXHelper.WrapJsonResponse(false, "", ex.Message);
                }
            
        }

        /// <summary>
        /// 上传文件根目录 /UploadFiles/BackMoney/
        /// </summary>
        private const string UpladFilesPath = "/Statistics/MemberIDImport/UpLoad/";

        internal bool BatchImport(out string msg)
        {
            BLL.Loger.Log4Net.Info("[HandlerImport]BatchImport begin...");
            try
            {
                //保存文件
                string fileName = this.SaveFile();

                //校验 插入
                return DealDataImported(fileName, out msg);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[HandlerImport]BatchImport errorStackTrace..." + ex.StackTrace);
                BLL.Loger.Log4Net.Info("[HandlerImport]BatchImport errorMessage..." + ex.Message);
                msg = ex.Message;
                return false;
            }


        }

        /// <summary>
        /// 保存文件
        /// </summary>
        private string SaveFile()
        {
            //先清空原有文件
            ClearFiles(DateTime.Now.AddDays(-1));

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

            DateTime time = DateTime.Now;
            string relatedPath = string.Format(UpladFilesPath + "{0}/{1}/{2}/{3}/", time.Year, time.Month, time.Day, time.Hour);
            string dir = HttpContext.Current.Server.MapPath("~" + relatedPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return Path.Combine(dir, name);
        }

        /// <summary>
        /// 清除指定时间之前的所有文件
        /// </summary>
        private void ClearFiles(DateTime datetime)
        {
            string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~" + UpladFilesPath));
            foreach (string name in files)
            {
                FileInfo fi = new FileInfo(name);
                if (fi.CreationTime < datetime)
                {
                    fi.Delete();
                }
            }
        }

        /// <summary>
        /// 处理导入的数据
        /// </summary>
        private bool DealDataImported(string fileName, out string msg)
        {
            System.Diagnostics.Debug.WriteLine("[HandlerImport]DealDataImported begin...");
            BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported begin...");
            bool success = true;
            msg = "";
            int sucYesCount = 0;//回访结果：是
            int sucNoCount = 0;//回访结果：否
            int FailCount = 0;//回写失败

            List<ExcelData> excelDataList = new List<ExcelData>();

            //(1)连接EXCEL文件
            string ext = Path.GetExtension(fileName);
            string connStr = string.Empty;
            if (ext.ToLower() == ".xls")
            {
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                //throw new Exception("上传文件应为xls或者xlsx格式的文件");
                throw new Exception("上传文件应为xls格式的文件");
            }

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

                string phone = Constant.STRING_INVALID_VALUE;
                string IsReturnVisit = Constant.STRING_INVALID_VALUE;
                GroupOrderDealHelper helper = new GroupOrderDealHelper();
                helper.userId = Convert.ToInt32(RequestUserID);

                List<myData> myDataList = new List<myData>();

                string whereYes = "";
                string whereNo = "";

                int rowNum = 0;
                int ibatch = 0;
                while (idr.Read())
                {
                    if (!string.IsNullOrEmpty(idr[0].ToString().Trim()))
                    {
                        phone = idr[0].ToString().Trim();
                    }

                    if (!string.IsNullOrEmpty(idr[1].ToString().Trim()))
                    {
                        IsReturnVisit = idr[1].ToString().Trim();
                    }

                    if (phone != Constant.STRING_INVALID_VALUE && IsReturnVisit != Constant.STRING_INVALID_VALUE)
                    {
                        if (IsReturnVisit == "是")
                        {
                            whereYes += "'" + phone + "',";
                        }
                        else
                        {
                            whereNo += "'" + phone + "',";
                        }
                    }

                    rowNum++;

                    ibatch = rowNum / 1000;
                    int iremainder = rowNum % 1000;

                    if (ibatch > 0 && iremainder == 0)
                    {
                        if (whereYes.EndsWith(","))
                        {
                            whereYes = whereYes.Substring(0, whereYes.Length - 1);
                        }

                        if (whereNo.EndsWith(","))
                        {
                            whereNo = whereNo.Substring(0, whereNo.Length - 1);
                        }

                        myData mydataYes = new myData();
                        myData mydataNo = new myData();

                        mydataYes.whereSql = " AND CustomerTel IN(" + BLL.Util.SqlFilterByInCondition(whereYes) + ")";
                        mydataYes.IsReturnVisit = "是";
                        myDataList.Add(mydataYes);

                        mydataNo.whereSql = " AND CustomerTel IN(" + BLL.Util.SqlFilterByInCondition(whereNo) + ")";
                        mydataNo.IsReturnVisit = "否";
                        myDataList.Add(mydataNo);

                        whereYes = "";
                        whereNo = "";
                    }

                    BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported rowNum..." + rowNum + ",phone is:" + phone + "...");
                }

                if (whereYes != "")
                {
                    if (whereYes.EndsWith(","))
                    {
                        whereYes = whereYes.Substring(0, whereYes.Length - 1);
                    }
                    myData mydataYes = new myData();

                    mydataYes.whereSql = " AND CustomerTel IN(" + BLL.Util.SqlFilterByInCondition(whereYes) + ")";
                    mydataYes.IsReturnVisit = "是";
                    myDataList.Add(mydataYes);
                }

                if (whereNo != "")
                {
                    if (whereNo.EndsWith(","))
                    {
                        whereNo = whereNo.Substring(0, whereNo.Length - 1);
                    }

                    myData mydataNo = new myData();

                    mydataNo.whereSql = " AND CustomerTel IN(" + BLL.Util.SqlFilterByInCondition(whereNo) + ")";
                    mydataNo.IsReturnVisit = "否";
                    myDataList.Add(mydataNo);
                }

                int i = 0;
                foreach (myData item in myDataList)
                {
                    i++;
                    BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported myDataList..批次:" + i + ",数据:" + item.whereSql + "!");

                    DataTable dt = BLL.GroupOrder.Instance.GetGroupOrder(item.whereSql);

                    BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported myDataList..批次:" + i + ",匹配订单数量:" + dt.Rows.Count + "条!");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {

                            switch (item.IsReturnVisit)
                            {
                                case "是":
                                    helper.RequestIsReturnVisit = ((int)Entities.IsReturnVisit.Yes).ToString();
                                    break;
                                case "否":
                                    helper.RequestIsReturnVisit = ((int)Entities.IsReturnVisit.No).ToString();
                                    break;
                                default:
                                    helper.RequestIsReturnVisit = ((int)Entities.IsReturnVisit.UnKnown).ToString();
                                    break;

                            }

                            helper.RequestTaskID = row["TaskID"].ToString().Trim();

                            try
                            {
                                helper.DealOrder(2, out msg);
                            }
                            catch (Exception ex)
                            {
                                msg = ex.Message;
                                BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported helper.DealOrder errorMessage is:" + ex.Message + "!");
                                BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported helper.DealOrder errorStackTrace is:" + ex.StackTrace + "!");
                            }

                            if (msg == "")
                            {

                                if (item.IsReturnVisit == "是")
                                {
                                    sucYesCount++;
                                }
                                else
                                {
                                    sucNoCount++;
                                }
                            }
                            else
                            {
                                FailCount++;
                                //失败任务，需生成对Excel文件
                                //字段：电话号码、处理结果、任务ID
                                ExcelData data = new ExcelData();
                                data.Phone = row["CustomerTel"].ToString().Trim();
                                data.TaskID = helper.RequestTaskID;
                                data.Msg = msg;

                                excelDataList.Add(data);

                                BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported FailCount..." + FailCount + ",phone is:" + data.Phone + ",TaskID is:" + data.TaskID + ",errorMsg is:" + data.Msg + "!");
                            }
                        }
                    }
                    else
                    {
                        //未找到订单的 电话号码
                        //noOrderCount++;
                        BLL.Loger.Log4Net.Info("[HandlerImport]未找到订单数据...批次:" + i + ",数据：" + item.whereSql + "!");
                    }
                }

                //msg = "{\"Result\":true,\"Msg\":\"成功回写处理结果：是,任务" + sucYesCount + "条！\n成功回写处理结果：否,任务" + sucNoCount + "条！\n失败任务" + FailCount + "条！\n未找到订单电话记录" + noOrderCount + "条！}";
                //msg = "{\"Result\":true,\"Msg\":\"成功回写处理结果：是,任务" + sucYesCount + "条！\"}";
                msg = "{\"Result\":true,\"Msg\":\"成功回写处理结果：是,任务" + sucYesCount + "条！\\n成功回写处理结果：否,任务" + sucNoCount + "条！\\n失败任务" + FailCount + "条！\",\"FailCount\":\"" + FailCount + "\"}";

                //查看是否有失败数据
                if (FailCount > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ExcelData item in excelDataList)
                    {
                        sb.Append(item.Phone + "," + item.TaskID + "," + item.Msg + ";");
                    }

                    string errorData = sb.ToString();
                    if (errorData != "")
                    {
                        errorData = errorData.Substring(0, errorData.Length - 1);
                        msg = "{\"Result\":true,\"Msg\":\"成功回写处理结果：是,任务" + sucYesCount + "条！\\n成功回写处理结果：否,任务" + sucNoCount + "条！\\n失败任务" + FailCount + "条！\",\"FailCount\":\"" + FailCount + "\",\"ErrorData\":\"" + errorData + "\"}";
                    }

                }

                BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported bye...");
                return success;
            }
        }


        private bool DealDataImported2(string fileName, out string msg)
        {
            System.Diagnostics.Debug.WriteLine("[Handler]DealDataImported begin...");
            bool success = true;
            msg = "";
            int sucYesCount = 0;//回访结果：是
            int sucNoCount = 0;//回访结果：否
            int FailCount = 0;//回写失败
            int noOrderCount = 0;//未找到匹配订单的电话

            List<ExcelData> excelDataList = new List<ExcelData>();
            ExcelData data = new ExcelData();

            //(1)连接EXCEL文件
            string ext = Path.GetExtension(fileName);
            string connStr = string.Empty;
            if (ext.ToLower() == ".xls")
            {
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                //throw new Exception("上传文件应为xls或者xlsx格式的文件");
                throw new Exception("上传文件应为xls格式的文件");
            }

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

                string phone = Constant.STRING_INVALID_VALUE;
                string IsReturnVisit = Constant.STRING_INVALID_VALUE;
                GroupOrderDealHelper helper = new GroupOrderDealHelper();
                helper.userId = Convert.ToInt32(RequestUserID);

                List<myData> myDataList = new List<myData>();
                myData mydataYes = new myData();
                myData mydataNo = new myData();
                string whereYes = "";
                string whereNo = "";

                int rowNum = 0;
                int ibatch = 0;
                while (idr.Read())
                {
                    if (!string.IsNullOrEmpty(idr[0].ToString().Trim()))
                    {
                        phone = idr[0].ToString().Trim();
                    }

                    if (!string.IsNullOrEmpty(idr[1].ToString().Trim()))
                    {
                        IsReturnVisit = idr[1].ToString().Trim();
                    }

                    if (phone != Constant.STRING_INVALID_VALUE && IsReturnVisit != Constant.STRING_INVALID_VALUE)
                    {
                        if (IsReturnVisit == "是")
                        {
                            whereYes += "'" + phone + "',";
                        }
                        else
                        {
                            whereNo += "'" + phone + "',";
                        }
                    }

                    rowNum++;

                    ibatch = rowNum / 1000;
                    int iremainder = rowNum % 1000;

                    if (ibatch > 0 && iremainder == 0)
                    {
                        if (whereYes.EndsWith(","))
                        {
                            whereYes = whereYes.Substring(0, whereYes.Length - 1);
                        }

                        if (whereNo.EndsWith(","))
                        {
                            whereNo = whereNo.Substring(0, whereNo.Length - 1);
                        }

                        mydataYes.whereSql = whereYes;
                        mydataYes.IsReturnVisit = "是";
                        mydataNo.whereSql = whereNo;
                        mydataNo.IsReturnVisit = "否";

                        myDataList.Add(mydataYes);
                        myDataList.Add(mydataNo);

                        whereYes = "";
                        whereNo = "";
                    }
                }

                if (whereYes != "")
                {
                    mydataYes.whereSql = whereYes;
                    mydataYes.IsReturnVisit = "是";
                    myDataList.Add(mydataYes);
                }

                if (whereNo != "")
                {
                    mydataNo.whereSql = whereNo;
                    mydataNo.IsReturnVisit = "否";
                    myDataList.Add(mydataNo);
                }

                foreach (myData item in myDataList)
                {

                }

                while (idr.Read())
                {
                    rowNum++;
                    System.Diagnostics.Debug.WriteLine("[Handler]DealDataImported rowNum..." + rowNum);
                    System.Diagnostics.Debug.WriteLine("[Handler]DealDataImported 电话号码..." + idr[0].ToString().Trim());


                    if (!string.IsNullOrEmpty(idr[0].ToString().Trim()))
                    {
                        phone = idr[0].ToString().Trim();
                    }

                    if (!string.IsNullOrEmpty(idr[1].ToString().Trim()))
                    {
                        IsReturnVisit = idr[1].ToString().Trim();
                    }


                    //先根据电话获取订单对应任务ID
                    if (phone != Constant.STRING_INVALID_VALUE && IsReturnVisit != Constant.STRING_INVALID_VALUE)
                    {
                        Entities.QueryGroupOrder query = new Entities.QueryGroupOrder();
                        query.CustomerTel = phone;
                        int total = 0;
                        DataTable dt = BLL.GroupOrder.Instance.GetGroupOrder(query, "", 1, 999999, out total);

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {

                                long taskid = Constant.INT_INVALID_VALUE;

                                if (!string.IsNullOrEmpty(row["TaskID"].ToString().Trim()))
                                {
                                    taskid = Convert.ToInt32(row["TaskID"].ToString().Trim());
                                }

                                if (taskid == Constant.INT_INVALID_VALUE)
                                {
                                    msg = "{\"Result\":false,\"Msg\":\"任务不能为空！\"}";
                                    continue;
                                }

                                switch (IsReturnVisit)
                                {
                                    case "是":
                                        helper.RequestIsReturnVisit = ((int)Entities.IsReturnVisit.Yes).ToString();
                                        break;
                                    case "否":
                                        helper.RequestIsReturnVisit = ((int)Entities.IsReturnVisit.No).ToString();
                                        break;
                                    default:
                                        helper.RequestIsReturnVisit = ((int)Entities.IsReturnVisit.UnKnown).ToString();
                                        break;

                                }

                                helper.RequestTaskID = row["TaskID"].ToString().Trim();
                                helper.DealOrder(2, out msg);

                                if (msg == "")
                                {

                                    if (IsReturnVisit == "是")
                                    {
                                        sucYesCount++;
                                    }
                                    else
                                    {
                                        sucNoCount++;
                                    }
                                }
                                else
                                {
                                    FailCount++;
                                    //失败任务，需生成对Excel文件
                                    //字段：电话号码、处理结果、任务ID
                                    //ExcelData data = new ExcelData();
                                    data.Phone = phone;
                                    data.TaskID = helper.RequestTaskID;
                                    data.Msg = msg;

                                    excelDataList.Add(data);
                                }
                            }
                        }
                        else
                        {
                            //未找到订单的 电话号码
                            noOrderCount++;
                        }
                    }
                }
                idr.Close();
                idr = null;
            }

            msg = "{\"Result\":true,\"Msg\":\"成功回写处理结果：是,任务" + sucYesCount + "条！\n成功回写处理结果：否,任务" + sucNoCount + "条！\n失败任务" + FailCount + "条！\n未找到订单电话记录" + noOrderCount + "条！}";

            if (FailCount > 0)
            {
                //ExcelHelper excelhelper = new ExcelHelper();
                //excelhelper.ExprotExcel(excelDataList);
            }

            return success;
        }

        public class myData
        {
            public string IsReturnVisit;
            public string whereSql;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}