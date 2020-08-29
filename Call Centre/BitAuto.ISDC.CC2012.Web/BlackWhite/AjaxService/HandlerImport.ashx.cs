using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Web.SessionState;
using System.IO;
using BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder.ImportExcelWriteBack;
using System.Data.OleDb;
using System.Text;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.BlackWhite.AjaxService
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
        public string Type
        {
            get
            {
                return HttpContext.Current.Request["Type"] == null ? string.Empty : HttpContext.Current.Request["Type"].ToString();
            }
        }
        private string BusinessTypeJSON
        {
            get
            {
                return HttpContext.Current.Request["BusinessTypeJSON"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["BusinessTypeJSON"].ToString());
            }
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
         //   BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
            {
                System.Diagnostics.Debug.WriteLine("[Handler]ProcessRequest begin...");
                context.Response.ContentType = "text/plain";
                if (!BLL.Util.CheckButtonRight("SYS024BUT40110101"))
                {
                    AJAXHelper.WrapJsonResponse(false, "", "对不起，您还没有导入操作的权限");
                    return;
                }
                try
                {
                    switch (Action)
                    {
                        case "batchimport":
                            string msg = "";
                            //AJAXHelper.WrapJsonResponse(BatchImport(out msg), "", msg);
                            if (BatchImport(out msg))
                            {
                                if (msg.Length > 0)
                                {
                                    msg = "{success:'false',result:'yes',msg:'部分操作失败：<br/>" + msg + "'}";
                                }
                                else
                                {
                                    msg = "{success:'true',result:'yes',msg:'保存成功'}";
                                }
                            }
                            else
                            {
                                msg = "{success:'false',result:'no',msg:'操作失败：<br/>" + msg + "'}";
                            }
                            context.Response.Write(msg);
                            break;
                        default:
                            AJAXHelper.WrapJsonResponse(false, "", "没有对应的操作");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    AJAXHelper.WrapJsonResponse(false, "", ex.Message);
                }
            }
            else
            {

                context.Response.Write("{success:'false',result:'no',msg:'操作失败：<br/>}");
            }
        }

        /// <summary>
        /// 上传文件根目录 /UploadFiles/BackMoney/
        /// </summary>
        //private const string UpladFilesPath = "/Statistics/MemberIDImport/UpLoad/";

        internal bool BatchImport(out string msg)
        {
            BLL.Loger.Log4Net.Info("[HandlerImport]BatchImport begin...");
            try
            {
                //保存文件
                string fileName = this.SaveFile();

                //校验 插入
                if (Type.Trim() == "0")
                {
                     return DealDataImported(fileName, out msg);
                }
                else{
                   return DealDataImportedForBlack(fileName, out msg);
                }
               
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
            //ClearFiles(DateTime.Now.AddDays(-1));

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
            string strDir = BLL.Util.GetUploadTemp();
            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }
            return strDir + Path.GetFileName(fileName);
        }

        /// <summary>
        /// 处理导入的数据
        /// </summary>
        private bool DealDataImported(string fileName, out string msg)
        {
            System.Diagnostics.Debug.WriteLine("[HandlerImport]DealDataImported begin...");
            BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported begin...");
            bool success = false;
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
                //throw new Exception("上传文件应为xls或者xlsx格式的文件");
                throw new Exception("上传文件应为xls格式的文件");
            }
            List<BusinessTypeMod> modeBusinessType = new List<BusinessTypeMod>();
            modeBusinessType = (List<BusinessTypeMod>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(BusinessTypeJSON, typeof(List<BusinessTypeMod>));

            int userid = BLL.Util.GetLoginUserID();
            Entities.EmployeeAgent employeeagent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
 

            DataTable tbNoDisturbReason = BLL.Util.GetEnumDataTable(typeof(Entities.NoDisturbReason));
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

                // string phone = Constant.STRING_INVALID_VALUE;
                //string IsReturnVisit = Constant.STRING_INVALID_VALUE;

                List<Entities.BlackWhiteList> myDataList = new List<Entities.BlackWhiteList>();
                int rowIndex = 0;
                while (idr.Read())
                {
                    rowIndex++;
                    Entities.BlackWhiteList model = new Entities.BlackWhiteList();
                    int backVal;
                    string innerMsg = "";
                    try
                    { 
                        //1.电话号码验证
                        if (!string.IsNullOrEmpty(idr[0].ToString().Trim()))
                        {
                            if (!BLL.Util.IsNumber(idr[0].ToString().Trim().Replace("-","")))
                            {
                                innerMsg += "第" + rowIndex + "行数据：" + "“电话号码”格式不正确，只能由数字组成<br/>";
                                msg += innerMsg; 
                                continue;
                            }
                            backVal = BLL.BlackWhiteList.Instance.PhoneNumIsNoDisturb(idr[0].ToString().Trim().Replace("-", ""));

                            if (backVal == 0 || backVal == 1) //没有考虑到已删除数据的处理
                            {
                                innerMsg += "第" + rowIndex + "行数据：" + "“电话号码”（" + idr[0] + "）已添加过，请核对数据<br/>";
                                msg += innerMsg; 
                                continue;
                            }
                            model.PhoneNum = idr[0].ToString().Trim().Replace("-", "");
                        }
                        else
                        {
                            innerMsg += "第" + rowIndex + "行数据：" + "“电话号码”不能为空<br/>";
                            msg += innerMsg; 
                            continue;
                        }
                        //2.话务ID验证（可空）
                        if (!string.IsNullOrEmpty(idr[1].ToString().Trim()))
                        {
                            if (!BLL.Util.IsNumber(idr[1].ToString().Trim()))
                            {
                                innerMsg += "第" + rowIndex + "行数据：" + "“话务ID”格式不正确，只能由数字组成<br/>";
                                msg += innerMsg; 
                                continue;
                            }
                            model.CallID = Convert.ToInt64(idr[1].ToString().Trim());
                        } 
                        //3.生效时间
                        if (!string.IsNullOrEmpty(idr[2].ToString().Trim()))
                        {
                            DateTime datetime;
                            if (DateTime.TryParse(idr[2].ToString().Trim(), out datetime))
                            {
                                model.EffectiveDate = datetime;
                            }
                            else
                            {
                                innerMsg += "第" + rowIndex + "行数据：" + "“生效时间”不是正确的日期时间格式<br/>";
                                msg += innerMsg; 
                                continue;
                            }
                        }
                        else
                        {
                            innerMsg += "第" + rowIndex + "行数据：" + "“生效时间”不能为空<br/>";
                            msg += innerMsg; 
                            continue;
                        }
                        //4.过期时间
                        if (!string.IsNullOrEmpty(idr[3].ToString().Trim()))
                        {
                            DateTime datetime;
                            if (DateTime.TryParse(idr[3].ToString().Trim(), out datetime))
                            {
                                model.ExpiryDate = datetime;
                            }
                            else
                            {
                                innerMsg += "第" + rowIndex + "行数据：" + "“过期时间”不是正确的日期时间格式<br/>";
                                msg += innerMsg; 
                                continue;
                            }
                        }
                        else
                        {
                            innerMsg += "第" + rowIndex + "行数据：" + "“过期时间”不能为空<br/>";
                            msg += innerMsg; 
                            continue;
                        }
                        //5.；类型
                        if (!string.IsNullOrEmpty(idr[4].ToString().Trim()))
                        {
                            int calltype = 0;
                            if (idr[4].ToString().Trim().Contains("呼入"))
                            {
                                calltype = 1;
                                //6.对应业务
                                if (!string.IsNullOrEmpty(idr[5].ToString().Trim()))
                                {
                                    string cidsReal = idr[5].ToString().Replace("，", ",").Trim();
                                    string[] businessnameArr = cidsReal.Split(',');
                                    int cdids = 0;
                                    for (int i = 0; i < businessnameArr.Length; i++)
                                    {
                                        for (int j = 0; j < modeBusinessType.Count; j++)
                                        {
                                            if (modeBusinessType[j].BusinessName == businessnameArr[i])
                                            {
                                                cdids += int.Parse(modeBusinessType[j].BusinessRightValue);
                                            }
                                        }
                                    }
                                    if (cdids == 0)
                                    {
                                        innerMsg += "第" + rowIndex + "行数据：" + "“对应业务”数据异常，请检查数据<br/>";
                                        msg += innerMsg; 
                                        continue;
                                    }
                                    else
                                    {
                                        model.CDIDS = cdids;
                                    }
                                }
                                else
                                {
                                    innerMsg += "第" + rowIndex + "行数据：" + "“对应业务”数据不能为空<br/>";
                                    msg += innerMsg; 
                                    continue;
                                }
                            }
                            if (idr[4].ToString().Trim().Contains("呼出"))
                            {
                                calltype += 2;
                                //7.原因
                                if (!string.IsNullOrEmpty(idr[6].ToString().Trim()))
                                {
                                    bool hasVal = false;
                                    foreach (DataRow row in tbNoDisturbReason.Rows)
                                    {
                                        if (row["name"].ToString() == idr[6].ToString().Trim())
                                        {
                                            model.CallOutNDType = int.Parse(row["value"].ToString());
                                            hasVal = true;
                                            continue;
                                        }
                                    }
                                    if (!hasVal)
                                    {
                                        innerMsg += "第" + rowIndex + "行数据：" + "“原因”数据不正确，没有相匹配的原因数据<br/>";
                                        msg += innerMsg; 
                                        continue;
                                    }
                                }
                                else
                                {
                                    innerMsg += "第" + rowIndex + "行数据：" + "“原因”数据不能为空<br/>";
                                    msg += innerMsg; 
                                    continue;
                                }
                            }

                            if (calltype == 0)
                            {
                                innerMsg += "第" + rowIndex + "行数据：" + "“类型”数据异常，请检查数据<br/>";
                                msg += innerMsg; 
                                continue;
                            }
                            else
                            {
                                model.CallType = calltype;
                            }
                        }
                        else
                        {
                            innerMsg += "第" + rowIndex + "行数据：" + "“类型”不能为空<br/>";
                            msg += innerMsg; 
                            continue;
                        }
                       
                        
                        //8.备注（可空）
                        if (!string.IsNullOrEmpty(idr[7].ToString().Trim()))
                        {
                            model.Reason = idr[7].ToString().Trim();
                        }
                    }
                    catch (Exception eex)
                    {
                        innerMsg += "第" + rowIndex + "行数据：" + "部分数据异常，请检查数据<br/>";
                        msg += innerMsg; 
                        continue;
                    }
                    model.Type = int.Parse(Type);
                    model.SynchrodataStatus = 0;
                    model.CreateUserId = userid ;
                    model.CreateDate = DateTime.Now;
                    model.UpdateUserId = Constant.INT_INVALID_VALUE;
                    model.UpdateDate = Constant.DATE_INVALID_VALUE;
                    model.Status = 0;
             
                    if (employeeagent != null && employeeagent.BGID.HasValue)
                    {
                        model.BGID = employeeagent.BGID.Value;
                    } 
                    if (innerMsg == "")
                    {
                        if (backVal == -1)
                        {
                            int backRecID = BLL.BlackWhiteList.Instance.GetRecIDByPhoneNumberAndType(idr[0].ToString().Trim().Replace("-", ""), 0);
                            if (backRecID > 0)
                            {
                                model.RecId = backRecID;
                                model.SynchrodataStatus = 1;
                                model.UpdateDate = DateTime.Now;
                                model.UpdateUserId = userid;
                                bool retVal = BLL.BlackWhiteList.Instance.UpdateNoDisturbData(model);
                                if (!retVal)
                                {
                                    msg += "号码" + idr[0].ToString() + "更新失败<br/>";
                                }
                                else
                                {
                                    success = true;
                                }
                            }
                        }
                        else if (backVal == 2)
                        {
                            if (BLL.BlackWhiteList.Instance.AddNoDisturbData(model) <= 0)
                            {
                                msg += "号码" + idr[0].ToString() + "入库失败<br/>";
                            }
                            else
                            {
                                success = true;
                            }
                        }
                    }
                    else
                    {
                        innerMsg = "";
                    }
                    //myDataList.Add(model);
                   
                }
              //  DataTable dtBW = BLL.Util.ListToDataTable(myDataList);
              //  BLL.BlackWhiteList.Instance.ImportData(dtBW);
  
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
            ClearFiles(fileName);

            return success;
        }

        /// <summary>
        /// 处理导入的数据
        /// </summary>
        private bool DealDataImportedForBlack(string fileName, out string msg)
        {
            System.Diagnostics.Debug.WriteLine("[HandlerImport]DealDataImported begin...");
            BLL.Loger.Log4Net.Info("[HandlerImport]DealDataImported begin...");
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
                //throw new Exception("上传文件应为xls或者xlsx格式的文件");
                //throw new Exception("上传文件应为xls格式的文件");
                msg = "传文件应为xls格式的文件";
                return false;
            }
            List<BusinessTypeMod> modeBusinessType = new List<BusinessTypeMod>();
            modeBusinessType = (List<BusinessTypeMod>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(BusinessTypeJSON, typeof(List<BusinessTypeMod>));

            int userid = BLL.Util.GetLoginUserID();
            Entities.EmployeeAgent employeeagent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
 
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

                // string phone = Constant.STRING_INVALID_VALUE;
                //string IsReturnVisit = Constant.STRING_INVALID_VALUE;

                List<Entities.BlackWhiteList> myDataList = new List<Entities.BlackWhiteList>();

                while (idr.Read())
                {
                    Entities.BlackWhiteList model = new Entities.BlackWhiteList(); 
                    try
                    {
                        model.Type = int.Parse(Type);
                        if (!string.IsNullOrEmpty(idr[0].ToString().Trim()))
                        {
                            model.PhoneNum = idr[0].ToString().Trim();
                        }
                        else
                        {
                            msg = "模板数据不能为空";
                            return false;
                        }

                        if (!string.IsNullOrEmpty(idr[1].ToString().Trim()))
                        {
                            model.EffectiveDate = DateTime.Parse(idr[1].ToString().Trim());
                        }
                        else
                        {
                            msg = "模板数据不能为空";
                            return false;
                        }

                        if (!string.IsNullOrEmpty(idr[2].ToString().Trim()))
                        {
                            model.ExpiryDate = DateTime.Parse(idr[2].ToString().Trim());
                        }
                        else
                        {
                            msg = "模板数据不能为空";
                            return false;
                        }

                        if (!string.IsNullOrEmpty(idr[3].ToString().Trim()))
                        {
                            int calltype = 0;
                            if (idr[3].ToString().Trim().Contains("呼入"))
                            {
                                calltype = 1;
                            }
                            if (idr[3].ToString().Trim().Contains("呼出"))
                            {
                                calltype += 2;
                            }

                            if (calltype == 0)
                            {
                                msg = "模板数据异常，请检查模板数据";
                                return false;
                            }
                            else
                            {
                                model.CallType = calltype;

                            }
                        }
                        else
                        {
                            msg = "模板数据不能为空";
                            return false;
                        }

                        if (!string.IsNullOrEmpty(idr[4].ToString().Trim()))
                        {
                            string cidsReal = idr[4].ToString().Replace("，", ",").Trim();
                            string[] businessnameArr = cidsReal.Split(',');
                            int cdids = 0;
                            for (int i = 0; i < businessnameArr.Length; i++)
                            {
                                for (int j = 0; j < modeBusinessType.Count; j++)
                                {
                                    if (modeBusinessType[j].BusinessName == businessnameArr[i])
                                    {
                                        if (BLL.BlackWhiteList.Instance.IsPhoneNumberCDIDExist(model.PhoneNum, int.Parse(modeBusinessType[j].BusinessRightValue)))
                                        {
                                            msg = "电话号码已添加过，请核对数据";
                                            return false;

                                        }

                                        cdids += int.Parse(modeBusinessType[j].BusinessRightValue);



                                    }
                                } 
                            }
                            if (cdids == 0)
                            {
                                msg = "模板数据异常，请检查模板数据";
                                return false;
                            }
                            else
                            {
                                model.CDIDS = cdids;
                            }
                        }
                        else
                        {
                            msg = "模板数据不能为空";
                            return false;
                        }
                        if (!string.IsNullOrEmpty(idr[5].ToString().Trim()))
                        {
                            model.Reason = idr[5].ToString().Trim();
                        }
                        else
                        {
                            msg = "模板数据不能为空";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "模板数据异常，请检查模板数据";
                        return false;

                    }
                    model.SynchrodataStatus = 0;
                    model.CreateUserId = userid;
                    model.CreateDate = DateTime.Now;
                    model.UpdateUserId = Constant.INT_INVALID_VALUE;
                    model.UpdateDate = Constant.DATE_INVALID_VALUE;
                    model.Status = 0;

                    if (employeeagent != null && employeeagent.BGID.HasValue)
                    {
                        model.BGID = employeeagent.BGID.Value;
                    }

                    myDataList.Add(model);
                }
                DataTable dtBW = BLL.Util.ListToDataTable(myDataList);
                BLL.BlackWhiteList.Instance.ImportData(dtBW);

                //return success;
            }

            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
 
                ClearFiles(fileName);
       
            
            return success;
        }

        private void ClearFiles(string fullName)
        {
            string path = Path.GetDirectoryName(fullName);
            Directory.Delete(path, true);
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
    public class BusinessTypeMod
    {
        public string BusinessName;
        public string BusinessRightValue;
    }
}
