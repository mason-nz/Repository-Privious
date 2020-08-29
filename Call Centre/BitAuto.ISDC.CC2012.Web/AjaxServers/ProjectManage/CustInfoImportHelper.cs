using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Web;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;
using Newtonsoft.Json;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public class CustInfoImportHelper
    {
        /// <summary>
        /// 小写
        /// </summary>
        public string Action
        {
            get
            {
                return HttpUtility.UrlDecode(this.Request["Action"] + "").Trim().ToLower();
            }
        }
        public string ProjectID
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["ProjectID"]) ? "" : this.Request["ProjectID"];
            }
        }
        //导入类型
        public string CarType
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["cartype"]) ? "-1" : this.Request["cartype"];
            }
        }
        public string TTCode
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["ttcode"]) ? "" : this.Request["ttcode"];
            }
        }
        //为1：表示是客户回访追加数据
        public string AddType
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["AddType"]) ? "" : this.Request["AddType"];
            }
        }
        public int UserID
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["UserID"]) ? 0 : int.Parse(this.Request["UserID"]);
            }
        }

        /// 是否进行黑名单验证
        /// <summary>
        /// 是否进行黑名单验证
        /// </summary>
        public bool IsBlacklistCheck
        {
            get
            {
                return (string.IsNullOrEmpty(this.Request["IsBlacklistCheck"]) ? "" : this.Request["IsBlacklistCheck"].ToString()) == "1";
            }
        }
        /// 验证方式
        /// <summary>
        /// 验证方式
        /// </summary>
        public BlackListCheckType BlackListCheckType
        {
            get
            {
                return (BlackListCheckType)CommonFunction.ObjectToInteger(this.Request["BlackListCheckType"]);
            }
        }

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// 总入口
        /// <summary>
        /// 总入口
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="tipinfo"></param>
        /// <returns></returns>
        public bool BatchImport(out string msg, out string tipinfo)
        {
            BLL.Loger.Log4Net.Info("开始保存文件");
            tipinfo = "";
            //保存文件
            string fileName = this.SaveFile();
            bool isSucees = false;

            if (CarType == "1" || CarType == "2")
            {
                // 新车、二手车模板导入
                isSucees = DealDataImported(fileName, out msg);
            }
            else
            {
                try
                {
                    string returnIDs = "";
                    //其他模板数据导入
                    isSucees = OtherDataImported(fileName, out msg, out returnIDs, out tipinfo);
                    if (isSucees)
                    {
                        msg = returnIDs;
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message.ToString();
                }
            }
            //清除文件
            ClearFiles(fileName);
            return isSucees;
        }

        #region 废弃：新车、二手车模板导入
        /// 新车、二手车模板导入
        /// <summary>
        /// 新车、二手车模板导入
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
            List<CustInfoImportExceptionInfo> exceptionInfoList = new List<CustInfoImportExceptionInfo>();
            List<Entities.ExcelCustInfo> excelInfoList = new List<Entities.ExcelCustInfo>();
            bool isConform = false;
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
                    if (idr[0].ToString().Trim().Contains("客户名称"))
                    {
                        startReadRow = rowNum;
                        isConform = true;
                    }
                    if (rowNum > startReadRow)
                    {
                        success = this.Valid(rowNum, idr, exceptionInfoList, excelInfoList);
                        if (!success)
                        {
                            break;
                        }
                    }
                }
                idr.Close();
                idr = null;
            }
            if (!isConform)
            {
                CustInfoImportExceptionInfo ex = new CustInfoImportExceptionInfo();
                ex.Info.Append("导入模版有误！");
                exceptionInfoList.Add(ex);
                msg = JavaScriptConvert.SerializeObject(exceptionInfoList);
                return false;
            }
            //(3)如果校验通过，插入数据
            if (success == false)
            {
                msg = JavaScriptConvert.SerializeObject(exceptionInfoList);
            }
            else
            {
                //插入数据
                BLL.ExcelCustInfo.Instance.InsertExcelCustInfo_ForBitch(excelInfoList);

                foreach (Entities.ExcelCustInfo item in excelInfoList)
                {
                    msg += item.ID.ToString() + ",";
                }
                if (msg != "")
                {
                    msg = msg.Substring(0, msg.Length - 1);
                }
            }
            return success;
        }
        /// <summary>
        /// 校验
        /// </summary>
        private bool Valid(int rowNum, IDataReader reader, List<CustInfoImportExceptionInfo> exceptionInfoList, List<Entities.ExcelCustInfo> execlInfoList)
        {
            bool currentSuccess = true;
            CustInfoImportExceptionInfo ex = new CustInfoImportExceptionInfo();

            string custName = string.Empty;
            string typeName = string.Empty;
            string provinceName = string.Empty;
            string cityName = string.Empty;
            string countyName = string.Empty;
            string brandName = string.Empty;
            string officeTel = string.Empty;
            string fax = string.Empty;
            string zipCode = string.Empty;
            string address = string.Empty;

            string monthStock = string.Empty;
            string monthSales = string.Empty;
            string monthTrade = string.Empty;
            string tradeMarketName = string.Empty;
            string contactName = string.Empty;

            custName = reader[0].ToString().Trim();
            typeName = reader[1].ToString().Trim();
            provinceName = reader[2].ToString().Trim();
            cityName = reader[3].ToString().Trim();
            countyName = reader[4].ToString().Trim();

            #region validate
            if (CarType == "1")
            {
                brandName = reader[5].ToString().Trim();
                officeTel = reader[6].ToString().Trim();
                fax = reader[7].ToString().Trim();
                zipCode = reader[8].ToString().Trim();
                address = reader[9].ToString().Trim();

                if (string.IsNullOrEmpty(custName) && string.IsNullOrEmpty(typeName) && string.IsNullOrEmpty(provinceName) && string.IsNullOrEmpty(cityName) && string.IsNullOrEmpty(countyName) && string.IsNullOrEmpty(zipCode) && string.IsNullOrEmpty(brandName) && string.IsNullOrEmpty(officeTel))
                {
                    currentSuccess = false;//此行为空行
                }
            }
            else if (CarType == "2")
            {
                monthStock = reader[5].ToString().Trim();
                monthSales = reader[6].ToString().Trim();
                monthTrade = reader[7].ToString().Trim();
                tradeMarketName = reader[8].ToString().Trim();
                contactName = reader[9].ToString().Trim();

                officeTel = reader[10].ToString().Trim();
                fax = reader[11].ToString().Trim();
                zipCode = reader[12].ToString().Trim();
                address = reader[13].ToString().Trim();

                if (string.IsNullOrEmpty(custName) && string.IsNullOrEmpty(typeName) && string.IsNullOrEmpty(provinceName) && string.IsNullOrEmpty(cityName)
                    && string.IsNullOrEmpty(countyName) && string.IsNullOrEmpty(zipCode) && string.IsNullOrEmpty(brandName) && string.IsNullOrEmpty(officeTel)
                    && string.IsNullOrEmpty(monthStock) && string.IsNullOrEmpty(monthSales) && string.IsNullOrEmpty(tradeMarketName) && string.IsNullOrEmpty(contactName))
                {
                    return currentSuccess;//此行为空行
                }
            }

            //客户名称验证
            if (string.IsNullOrEmpty(custName))
            {
                ex.Info.Append("第" + rowNum + "行[客户名称]不可为空。");
            }
            else if (custName.Length > 100)
            {
                ex.Info.Append("第" + rowNum + "行[客户名称]长度过长。");
            }
            int stock = -1;
            int sales = -1;
            int trade = -1;
            int tradeMarketID = -1;
            if (CarType == "2")
            {
                if (!string.IsNullOrEmpty(monthStock))
                {
                    bool isExist = false;
                    foreach (BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthStock c in (BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthStock[])Enum.GetValues(typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthStock)))
                    {
                        if (Utils.EnumHelper.GetEnumTextValue(c) == monthStock)
                        {
                            isExist = true;
                            stock = (int)c;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        ex.Info.Append("第" + rowNum + "行[月库存量]不在规定的范围内！");
                    }
                }
                if (!string.IsNullOrEmpty(monthSales))
                {
                    bool isExist = false;
                    foreach (BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthSales c in (BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthSales[])Enum.GetValues(typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthSales)))
                    {
                        if (Utils.EnumHelper.GetEnumTextValue(c) == monthSales)
                        {
                            isExist = true;
                            sales = (int)c;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        ex.Info.Append("第" + rowNum + "行[月置换量]不在规定的范围内！");
                    }
                }
                if (!string.IsNullOrEmpty(monthTrade))
                {
                    bool isExist = false;
                    foreach (BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthTrade c in (BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthTrade[])Enum.GetValues(typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthTrade)))
                    {
                        if (Utils.EnumHelper.GetEnumTextValue(c) == monthTrade)
                        {
                            isExist = true;
                            trade = (int)c;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        ex.Info.Append("第" + rowNum + "行[月交易量]不在规定的范围内！");
                    }
                }
                if (!string.IsNullOrEmpty(tradeMarketName))
                {
                    BitAuto.YanFa.Crm2009.Entities.QueryCustInfo query = new BitAuto.YanFa.Crm2009.Entities.QueryCustInfo();
                    query.TypeID = ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.TradeMarket).ToString();
                    query.ExistCustName = tradeMarketName.Trim();
                    int total = 0;
                    DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, "", 1, 1, out total);
                    if (dt.Rows.Count == 0)
                    {
                        ex.Info.Append("第" + rowNum + "行[所在交易市场名称]不在规定的范围内！");
                    }
                    else
                    {
                        if (dt.Rows[0]["CustID"].ToString() != string.Empty)
                        {
                            tradeMarketID = int.Parse(dt.Rows[0]["CustID"].ToString());
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(officeTel))
            {
                ex.Info.Append("第" + rowNum + "行[客户电话]不可为空。");
            }
            else
            {
                if (!BLL.Util.IsTelephone(officeTel))
                {
                    ex.Info.Append("第" + rowNum + "行[客户电话]格式不正确！");
                }
            }
            if (ex.Info.Length > 0)
            {
                currentSuccess = false;
                exceptionInfoList.Add(ex);
            }
            #endregion
            //如通过则插入数据
            if (currentSuccess)
            {
                Entities.ExcelCustInfo info = new Entities.ExcelCustInfo();
                info.BrandName = brandName;
                info.CityName = cityName;
                info.Fax = fax;
                info.CountyName = countyName;
                info.CustName = custName;
                info.OfficeTel = officeTel;
                info.Address = address;
                info.ProvinceName = provinceName;
                info.TypeName = typeName;
                info.Zipcode = zipCode;

                info.CarType = int.Parse(CarType);
                info.MonthSales = sales;
                info.MonthStock = stock;
                info.MonthTrade = trade;
                info.TradeMarketID = tradeMarketID;
                info.ContactName = contactName;
                // info.CreateUserID = BLL.Util.GetLoginUserID();
                info.CreateUserID = UserID;
                info.Createtime = DateTime.Now;

                execlInfoList.Add(info);
            }

            return currentSuccess;
        }
        #endregion

        #region 其他模板数据导入
        /// 其他模板数据导入
        /// <summary>
        /// 其他模板数据导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool OtherDataImported(string fileName, out string msg, out  string returnIDs, out string tipinfo)
        {
            bool isSuccess = false;
            tipinfo = "";

            msg = "";
            returnIDs = "";
            List<ColumnInfo> columninfo = null;
            DataTable dt = new DataTable();
            try
            {
                BLL.Loger.Log4Net.Info("从Excel读取数据—1:开始读取数据：");
                #region 读取数据
                if (AddType == "1")
                {
                    if (AddData_ReturnVisitCust(fileName, ref msg, ref dt) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    dt = GetImportData_Other(fileName, out msg);
                }

                if (dt.Rows.Count == 0)
                {
                    msg = "Excel文件中没有数据";
                    return false;
                }

                if (msg != "")
                {
                    return false;
                }
                #endregion

                if (AddType == "1")
                {
                    BLL.Loger.Log4Net.Info("从Excel获取relationID数据开始");
                    List<string> ids = new List<string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        ids.Add(row[0].ToString());
                    }
                    //去重校验
                    List<string> notexists = GetNotRepartCustID(ids);
                    tipinfo = string.Format("共选择{0}条数据，{1}条数据新增成功！", ids.Count, notexists.Count);

                    BLL.Loger.Log4Net.Info("从Excel获取relationID数据结束");
                    returnIDs = string.Join(",", notexists.ToArray());
                }
                else
                {
                    List<Entities.TField> fList = BLL.TField.Instance.GetTFieldListByTTCode(TTCode);
                    Entities.TPage tpageModel = BLL.TPage.Instance.GetTPageByTTCode(TTCode);

                    //验证
                    columninfo = VaileData(out msg, dt, fList);

                    BLL.Loger.Log4Net.Info("从Excel读取数据—4:验证已执行完成：");

                    if (msg == "")
                    {
                        #region 导入数据库
                        returnIDs = ImportDataToDB(dt, out msg, fList, tpageModel, columninfo, out tipinfo);

                        BLL.Loger.Log4Net.Info("从Excel读取数据—5:插入数据完成：" + returnIDs);

                        Entities.TTable ttable = BLL.TTable.Instance.GetTTableByTTCode(TTCode);
                        if (ttable != null)
                        {
                            ttable.TTIsData = 1;
                            BLL.TTable.Instance.Update(ttable);
                        }
                        else
                        {
                            msg = "没有找到数据表";
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BLL.Loger.Log4Net.Error("导入出错—3:出错：" + ex.Message.ToString(), ex);
            }
            if (msg == "")
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        /// 验证导入的数据
        /// <summary>
        /// 验证导入的数据
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="dt"></param>
        private List<ColumnInfo> VaileData(out string msg, DataTable dt, List<Entities.TField> fList)
        {
            msg = "";
            List<ColumnInfo> columnInfo = new List<ColumnInfo>();

            #region 去除空行
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString().Trim() != "")
                    {
                        rowdataisnull = false;
                        break;
                    }
                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }
            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }

            if (dt.Rows.Count > 5000)
            {
                msg += "每次导入数据不能超过5000行";
                return columnInfo;
            }
            #endregion

            #region 一个项目中总数不能超过2W条
            int SumCount = 0;
            if (ProjectID == "")
            {
                //新增
                SumCount = dt.Rows.Count;
            }
            else
            {
                //编辑
                string DataCount = "";
                BLL.ProjectDataSoure.Instance.GetProjectDataSoureID(long.Parse(ProjectID), out DataCount, false);
                SumCount = dt.Rows.Count + int.Parse(DataCount);
            }
            if (SumCount > 20000)
            {
                msg += "一个项目中导入数据总数不能超过20000条";
                return columnInfo;
            }

            #endregion

            #region 验证列、处理列、增加需要的列
            ColumnInfo column = new ColumnInfo();
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                column = new ColumnInfo();
                column.ExcelColumnName = dt.Columns[i].ColumnName;

                //去除EXCEL列名中特定的字符
                column.FieldName = dt.Columns[i].ColumnName.Replace("（起）", "")
                                                                            .Replace("（止）", "")
                                                                            .Replace("（省）", "")
                                                                            .Replace("（市）", "")
                                                                            .Replace("（县）", "")
                                                                            .Replace("（品牌）", "")
                                                                            .Replace("（车型）", "");

                //字段表中的物理字段的名称、显示方式
                string strtest = "";
                Entities.TField f = fList.Find(delegate(Entities.TField o) { strtest = column.FieldName; return o.TFDesName == column.FieldName; });
                if (f != null)
                {
                    //显示方式
                    column.ShowCode = f.TFShowCode;
                    column.TFValue = f.TFValue;
                    column.TFInportIsNull = f.TFInportIsNull.ToString();
                    #region 列名
                    switch (column.ShowCode)
                    {
                        case "100009":  //日期段
                            if (dt.Columns[i].ColumnName.IndexOf("（起）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_startdata";
                            }
                            else if (dt.Columns[i].ColumnName.IndexOf("（止）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_enddata";
                            }
                            else
                            {
                                msg += "[" + dt.Columns[i].ColumnName + "]列名格式不正确！<br>";
                            }
                            break;
                        case "100011":  //时间段
                            if (dt.Columns[i].ColumnName.IndexOf("（起）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_starttime";
                            }
                            else if (dt.Columns[i].ColumnName.IndexOf("（止）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_endtime";
                            }
                            else
                            {
                                msg += "[" + dt.Columns[i].ColumnName + "]列名格式不正确！<br>";
                            }
                            break;
                        case "100012"://二级省市
                        case "100013"://三级省市
                            if (dt.Columns[i].ColumnName.IndexOf("（省）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_Province_name";

                                dt.Columns.Add(f.TFName + "_Province").SetOrdinal(i);//添加一个省份ID列
                                column.TabelColumnName2ID = f.TFName + "_Province";
                            }
                            else if (dt.Columns[i].ColumnName.IndexOf("（市）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_City_name";

                                dt.Columns.Add(f.TFName + "_City").SetOrdinal(i);//添加一个城市ID列
                                column.TabelColumnName2ID = f.TFName + "_City";
                            }
                            else if (dt.Columns[i].ColumnName.IndexOf("（县）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_Country_name";

                                dt.Columns.Add(f.TFName + "_Country").SetOrdinal(i);//添加一个县ID列
                                column.TabelColumnName2ID = f.TFName + "_Country";
                            }
                            else
                            {
                                msg += "[" + dt.Columns[i].ColumnName + "]列名格式不正确！<br>";
                            }
                            break;
                        case "100003"://单选
                            column.TabelColumnName = f.TFName + "_radioid_name";
                            dt.Columns.Add(f.TFName + "_radioid").SetOrdinal(i);//单选ID
                            column.TabelColumnName2ID = f.TFName + "_radioid";
                            break;
                        case "100004"://复选
                            column.TabelColumnName = f.TFName + "_checkid_name";
                            dt.Columns.Add(f.TFName + "_checkid").SetOrdinal(i);//单选ID
                            column.TabelColumnName2ID = f.TFName + "_checkid";
                            break;
                        case "100005"://下拉
                            column.TabelColumnName = f.TFName + "_selectid_name";
                            dt.Columns.Add(f.TFName + "_selectid").SetOrdinal(i);//单选ID
                            column.TabelColumnName2ID = f.TFName + "_selectid";
                            break;
                        case "100014"://客户ID
                            column.TabelColumnName = f.TFName + "_crmcustid_name";
                            break;
                        case "100015"://个人客户
                            if (f.TFDesName == "姓名" || f.TFDesName == "电话")
                            {
                                column.TabelColumnName = f.TFName;
                            }
                            else if (f.TFDesName == "性别")
                            {
                                column.TabelColumnName = f.TFName + "_radioid_name";
                                dt.Columns.Add(f.TFName + "_radioid").SetOrdinal(i);//单选ID
                                column.TabelColumnName2ID = f.TFName + "_radioid";
                            }
                            break;
                        case "100016"://下单车型
                            if (dt.Columns[i].ColumnName.IndexOf("（品牌）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_XDBrand_Name";
                                dt.Columns.Add(f.TFName + "_XDBrand").SetOrdinal(i);//添加一个品牌列
                                column.TabelColumnName2ID = f.TFName + "_XDBrand";
                            }
                            else if (dt.Columns[i].ColumnName.IndexOf("（车型）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_XDSerial_Name";
                                dt.Columns.Add(f.TFName + "_XDSerial").SetOrdinal(i);//添加一个车型列
                                column.TabelColumnName2ID = f.TFName + "_XDSerial";
                                column.TFInportIsNull = "1";
                            }
                            else
                            {
                                msg += "[" + dt.Columns[i].ColumnName + "]列名格式不正确！<br>";
                            }
                            break;
                        case "100017"://意向车型
                            if (dt.Columns[i].ColumnName.IndexOf("（品牌）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_YXBrand_Name";
                                dt.Columns.Add(f.TFName + "_YXBrand").SetOrdinal(i);//添加一个品牌列
                                column.TabelColumnName2ID = f.TFName + "_YXBrand";
                            }
                            else if (dt.Columns[i].ColumnName.IndexOf("（车型）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_YXSerial_Name";
                                dt.Columns.Add(f.TFName + "_YXSerial").SetOrdinal(i);//添加一个车型列
                                column.TabelColumnName2ID = f.TFName + "_YXSerial";
                                column.TFInportIsNull = "1";
                            }
                            else
                            {
                                msg += "[" + dt.Columns[i].ColumnName + "]列名格式不正确！<br>";
                            }
                            break;
                        case "100018"://出售车型
                            if (dt.Columns[i].ColumnName.IndexOf("（品牌）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_CSBrand_Name";
                                dt.Columns.Add(f.TFName + "_CSBrand").SetOrdinal(i);//添加一个品牌列
                                column.TabelColumnName2ID = f.TFName + "_CSBrand";
                            }
                            else if (dt.Columns[i].ColumnName.IndexOf("（车型）") != -1)
                            {
                                column.TabelColumnName = f.TFName + "_CSSerial_Name";
                                dt.Columns.Add(f.TFName + "_CSSerial").SetOrdinal(i);//添加一个车型列
                                column.TabelColumnName2ID = f.TFName + "_CSSerial";
                                column.TFInportIsNull = "1";
                            }
                            else
                            {
                                msg += "[" + dt.Columns[i].ColumnName + "]列名格式不正确！<br>";
                            }
                            break;
                        default:
                            column.TabelColumnName = f.TFName;
                            break;
                    }
                    #endregion
                    columnInfo.Add(column);
                }
                else
                {
                    msg += "模板不正确！【" + column.FieldName + "】列名不正确";
                }
            }
            #endregion

            if (msg != "")
            {
                return null;
            }

            #region 验证数据
            //数据行数
            int RowCount = dt.Rows.Count;

            StringBuilder custIDs = new StringBuilder();
            string[] list = null;
            int flog = 0;
            DateTime datetimeVal;
            int ColIndex = 0;

            foreach (DataColumn col in dt.Columns)
            {
                DateTime oldTime = DateTime.Now;
                ColumnInfo curCol = columnInfo.Find(delegate(ColumnInfo o) { return o.ExcelColumnName == col.ColumnName; });
                if (curCol != null)
                {
                    #region 验证CRM 客户ID
                    if (curCol.ShowCode == "100014")
                    {
                        custIDs = new StringBuilder();
                        for (int r = 0; r <= RowCount - 1; r++)
                        {
                            //追加待检测数据
                            custIDs.Append(dt.Rows[r][col.ColumnName].ToString() + ",");
                            //攒够1000条，检测一次
                            if (r % 1000 == 0 && r != 0)
                            {
                                CheckNoExistsCustID(ref msg, custIDs, col.ColumnName);
                                //检测完成后，清空
                                custIDs = new StringBuilder();
                                if (msg != "")
                                {
                                    return columnInfo;
                                }
                            }
                        }
                        //循环结束后，最后一次检测
                        if (custIDs.Length > 0)
                        {
                            CheckNoExistsCustID(ref msg, custIDs, col.ColumnName);
                            //检测完成后，清空
                            custIDs = new StringBuilder();
                            if (msg != "")
                            {
                                return columnInfo;
                            }
                        }
                    }
                    #endregion

                    for (int r = 0; r <= RowCount - 1; r++)
                    {
                        //判断不能为空
                        if (curCol.TFInportIsNull == "0" && dt.Rows[r][col.ColumnName].ToString() == "")
                        {
                            msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行不能为空！";
                        }
                        //其他判断
                        if (dt.Rows[r][col.ColumnName].ToString().Trim() != "")
                        {
                            #region 判断格式
                            list = null;
                            flog = 0;
                            switch (curCol.ShowCode)
                            {
                                case "100003"://单选
                                case "100005"://下拉
                                    #region 验证单选和下拉
                                    list = curCol.TFValue.Split(';');
                                    flog = 0;
                                    foreach (string s in list)
                                    {
                                        if (s.Split('|')[1] == dt.Rows[r][col.ColumnName].ToString())
                                        {
                                            flog = 1;
                                            dt.Rows[r][curCol.TabelColumnName2ID] = s.Split('|')[0];//存入对应ID
                                            break;
                                        }
                                    }
                                    if (flog == 0)
                                    {
                                        //没找到
                                        msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行（" + dt.Rows[r][col.ColumnName].ToString() + "）格式不正确！";
                                    }
                                    #endregion
                                    break;
                                case "100004"://复选
                                    #region 验证复选
                                    list = curCol.TFValue.Split(';');
                                    string ids = "";
                                    string[] valList = dt.Rows[r][col.ColumnName].ToString().Split(',');
                                    foreach (string val in valList)
                                    {
                                        flog = 0;
                                        foreach (string s in list)
                                        {
                                            if (s.Split('|')[1] == val)
                                            {
                                                flog = 1;
                                                ids += s.Split('|')[0] + ",";//存入对应ID
                                                break;
                                            }
                                        }
                                        if (flog == 0)
                                        {
                                            //没找到
                                            msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行（" + dt.Rows[r][col.ColumnName].ToString() + "）格式不正确！";
                                            break;
                                        }
                                    }
                                    if (ids != "")
                                    {
                                        ids = ids.Substring(0, ids.Length - 1);
                                    }
                                    dt.Rows[r][curCol.TabelColumnName2ID] = ids;
                                    #endregion
                                    break;
                                case "100008"://日期点
                                case "100009"://日期段
                                case "100010"://时间点
                                case "100011"://时间段
                                    if (!DateTime.TryParse(dt.Rows[r][col.ColumnName].ToString(), out datetimeVal))
                                    {
                                        msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行（" + dt.Rows[r][col.ColumnName].ToString() + "）日期时间格式不正确！";
                                    }
                                    break;
                                case "100006"://电话号码
                                    string telVal = dt.Rows[r][col.ColumnName].ToString();
                                    if (!BLL.Util.IsTelephone(telVal))
                                    {
                                        msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行（" + dt.Rows[r][col.ColumnName].ToString() + "）电话格式不正确！";
                                    }
                                    break;
                                case "100007"://邮箱
                                    if (!BLL.Util.IsEmail(dt.Rows[r][col.ColumnName].ToString()))
                                    {
                                        msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行（" + dt.Rows[r][col.ColumnName].ToString() + "）邮箱格式不正确！";
                                    }
                                    break;
                                case "100012"://二级省市
                                case "100013"://三级省市
                                    string name = dt.Rows[r][col.ColumnName].ToString();
                                    AreaInfo areaInfo;
                                    int Pid = 0;
                                    string pidstr = "";
                                    string afterStr = name.Substring(name.Length - 1, 1);
                                    if (afterStr == "省" || afterStr == "市" || afterStr == "县" || afterStr == "区")
                                    {
                                        name = name.Substring(0, name.Length - 1);
                                    }
                                    if (curCol.TabelColumnName.IndexOf("_Province_name") != -1)
                                    {
                                        Pid = 0;
                                        #region 省份
                                        areaInfo = AreaHelper.Instance.GetAreaInfoByLikeNameFromCache(name, AreaType.Province, Pid);
                                        if (areaInfo == null)
                                        {
                                            msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行省份名称(" + name + ")不正确！";
                                        }
                                        else
                                        {
                                            dt.Rows[r][curCol.TabelColumnName2ID] = int.Parse(areaInfo.AreaID);
                                            dt.Rows[r][col.ColumnName] = areaInfo.AreaName;
                                        }
                                        #endregion
                                    }
                                    else if (curCol.TabelColumnName.IndexOf("_City_name") != -1)
                                    {
                                        #region  城市
                                        if (dt.Columns.Contains(curCol.TabelColumnName.Split('_')[0] + "_Province"))
                                        {
                                            pidstr = dt.Rows[r][curCol.TabelColumnName.Split('_')[0] + "_Province"].ToString();//对应省份ID
                                            if (pidstr == "" || !int.TryParse(pidstr, out Pid))
                                            {
                                                msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行城市名称(" + name + ")没有对应的上级省份！";
                                            }
                                            else
                                            {
                                                areaInfo = AreaHelper.Instance.GetAreaInfoByLikeNameFromCache(name, AreaType.City, Pid);
                                                if (areaInfo == null)
                                                {
                                                    msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行城市名称(" + name + ")不正确！";
                                                }
                                                else
                                                {
                                                    dt.Rows[r][curCol.TabelColumnName2ID] = areaInfo.AreaID;
                                                    dt.Rows[r][col.ColumnName] = areaInfo.AreaName;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            msg += "省份列应该在城市列之前！";
                                        }
                                        #endregion
                                    }
                                    else if (curCol.TabelColumnName.IndexOf("_Country_name") != -1)
                                    {
                                        #region 县
                                        if (dt.Columns.Contains(curCol.TabelColumnName.Split('_')[0] + "_City"))
                                        {
                                            pidstr = dt.Rows[r][curCol.TabelColumnName.Split('_')[0] + "_City"].ToString();//对应城市ID
                                            if (pidstr == "" || !int.TryParse(pidstr, out Pid))
                                            {
                                                msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行县名称(" + name + ")没有对应的上级城市！";
                                            }
                                            else
                                            {
                                                areaInfo = AreaHelper.Instance.GetAreaInfoByLikeNameFromCache(name, AreaType.Country, Pid);
                                                if (areaInfo == null)
                                                {
                                                    msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行县名称(" + name + ")不正确！";
                                                }
                                                else
                                                {
                                                    dt.Rows[r][curCol.TabelColumnName2ID] = areaInfo.AreaID;
                                                    dt.Rows[r][col.ColumnName] = areaInfo.AreaName;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            msg += "城市列应该在县之前！";
                                        }
                                        #endregion
                                    }
                                    break;
                                case "100016"://下单车型
                                case "100017"://意向车型
                                case "100018"://出售车型
                                    string PreStr = curCol.ShowCode == "100016" ? "_XD" : curCol.ShowCode == "100017" ? "_YX" : "_CS"; //前缀
                                    string NameText = dt.Rows[r][col.ColumnName].ToString().Trim();
                                    int BrandID = 0;
                                    int MasterBrandID = 0;
                                    if (curCol.TabelColumnName.IndexOf("Brand_Name") != -1)
                                    {
                                        Pid = 0;
                                        //从品牌表中找，返回主品牌ID和品牌ID
                                        GetBrandID(NameText, out BrandID, out MasterBrandID);
                                        if (MasterBrandID == -1)
                                        {
                                            msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行品牌名称(" + NameText + ")不正确！";
                                        }
                                        else
                                        {
                                            dt.Rows[r][curCol.TabelColumnName2ID] = MasterBrandID;
                                            dt.Rows[r][col.ColumnName] = NameText;
                                        }
                                    }
                                    else if (curCol.TabelColumnName.IndexOf("Serial_Name") != -1)
                                    {
                                        #region 车型
                                        if (dt.Columns.Contains(curCol.TabelColumnName.Split('_')[0] + PreStr + "Brand"))
                                        {
                                            string brandName = dt.Rows[r][ColIndex - 2].ToString();// 前一列的品牌名称
                                            GetBrandID(brandName, out BrandID, out MasterBrandID);
                                            if (MasterBrandID == -1)
                                            {
                                                msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行车型名称(" + NameText + ")没有对应的上级品牌！";
                                            }
                                            else
                                            {
                                                if (BrandID != -1)
                                                {
                                                    Pid = BrandID;
                                                }
                                                else
                                                {
                                                    Pid = MasterBrandID;
                                                }
                                                int SerialID = BLL.CarTypeAPIFromCC.Instance.GetSerilIDByNameFormCache(NameText, Pid);
                                                if (SerialID == -1)
                                                {
                                                    msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行车型名称(" + NameText + ")不正确！";
                                                }
                                                else
                                                {
                                                    dt.Rows[r][curCol.TabelColumnName2ID] = SerialID;
                                                    dt.Rows[r][col.ColumnName] = NameText;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            msg += "品牌列应该在车型列之前！";
                                        }
                                        #endregion
                                    }
                                    break;
                                case "100015"://个人用户
                                    if (curCol.FieldName == "电话")
                                    {
                                        string telVal2 = dt.Rows[r][col.ColumnName].ToString();
                                        if (!BLL.Util.IsTelephone(telVal2))
                                        {
                                            msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行（" + dt.Rows[r][col.ColumnName].ToString() + "）电话格式不正确！";
                                        }
                                    }
                                    else if (curCol.FieldName == "性别")
                                    {
                                        #region 验证单选和下拉
                                        list = curCol.TFValue.Split(';');
                                        flog = 0;
                                        foreach (string s in list)
                                        {
                                            if (s.Split('|')[1] == dt.Rows[r][col.ColumnName].ToString())
                                            {
                                                flog = 1;
                                                dt.Rows[r][curCol.TabelColumnName2ID] = s.Split('|')[0];//存入对应ID
                                                break;
                                            }
                                        }
                                        if (flog == 0)
                                        {
                                            //没找到
                                            msg += "【" + col.ColumnName + "】列第" + (r + 3) + "行（" + dt.Rows[r][col.ColumnName].ToString() + "）格式不正确！";
                                        }
                                        #endregion
                                    }
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            switch (curCol.ShowCode)
                            {
                                case "100008"://日期点
                                case "100009"://日期段
                                case "100010"://时间点
                                case "100011"://时间段
                                    dt.Rows[r][col.ColumnName] = DBNull.Value;
                                    break;
                            }
                        }
                        if (msg != "")
                        {
                            return null;
                        }
                    }
                }

                BLL.Loger.Log4Net.Info(col.ColumnName + ":【" + TTCode + "】时间\t" + (DateTime.Now - oldTime).TotalMilliseconds);

                ColIndex++;
            }
            #endregion

            return columnInfo;
        }
        /// 根据名称查找品牌
        /// <summary>
        /// 根据名称查找品牌
        /// </summary>
        /// <param name="NameText"></param>
        /// <param name="BrandID"></param>
        /// <param name="MasterBrandID"></param>
        private static void GetBrandID(string NameText, out  int BrandID, out int MasterBrandID)
        {
            BrandID = -1;
            MasterBrandID = -1;
            //从品牌表中找，返回主品牌ID和品牌ID
            BLL.CarTypeAPIFromCC.Instance.GetBrandIDByNameFormCache(NameText, out BrandID, out MasterBrandID);
            if (BrandID == -1)
            {
                //在品牌表中没有找到,去之品牌里找
                MasterBrandID = BLL.CarTypeAPIFromCC.Instance.GetMasterBrandIDByNameFormCache(NameText);
            }
        }

        /// 导入数据到数据库
        /// <summary>
        /// 导入数据到数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg"></param>
        /// <param name="fList"></param>
        /// <param name="tpageModel"></param>
        /// <returns>插入数据的RecID</returns>
        private string ImportDataToDB(DataTable dt, out string msg, List<Entities.TField> fList, Entities.TPage tpageModel, List<ColumnInfo> columninfo, out string tipinfo)
        {
            tipinfo = "";
            msg = "";
            //导入的全部数据
            List<string> allIds = new List<string>();
            #region 修改列名
            foreach (ColumnInfo col in columninfo)
            {
                if (col.TabelColumnName != null && col.TabelColumnName != "")
                {
                    dt.Columns[col.ExcelColumnName].ColumnName = col.TabelColumnName;
                }
                if (col.TabelColumnName2ID != null && col.TabelColumnName2ID != "")
                {
                    dt.Columns[col.TabelColumnName2ID].ColumnName = col.TabelColumnName2ID;
                }
            }
            #endregion

            #region 构建结构化的DataTable
            #region 建立表
            dt.Columns.Add("RecID", typeof(int));
            dt.Columns.Add("Status", typeof(int));
            dt.Columns.Add("CreateTime", typeof(DateTime));
            dt.Columns.Add("CreateUserID", typeof(int));

            dt.Columns["RecID"].SetOrdinal(0);
            dt.Columns["Status"].SetOrdinal(1);
            dt.Columns["CreateTime"].SetOrdinal(2);
            dt.Columns["CreateUserID"].SetOrdinal(3);

            DataTable newDt = dt.Clone();
            ColumnInfo telColumnInfo = null;

            foreach (ColumnInfo col in columninfo)
            {
                switch (col.ShowCode)
                {
                    case "100006":
                        //电话
                        if (telColumnInfo == null)
                            telColumnInfo = col;
                        break;
                    case "100008":
                    case "100009":
                    case "100010":
                    case "100011": //日期
                        newDt.Columns[col.TabelColumnName].DataType = typeof(DateTime);
                        break;
                    case "100012":
                        newDt.Columns[col.TabelColumnName2ID].DataType = typeof(int);
                        break;
                    case "100013":
                        newDt.Columns[col.TabelColumnName2ID].DataType = typeof(int);
                        break;
                    case "100003":
                        newDt.Columns[col.TabelColumnName2ID].DataType = typeof(int);
                        break;
                    case "100004":
                        newDt.Columns[col.TabelColumnName2ID].DataType = typeof(int);
                        break;
                    case "100005":
                        newDt.Columns[col.TabelColumnName2ID].DataType = typeof(int);
                        break;
                    case "100015"://个人用户
                        if (col.FieldName == "电话")
                        {
                            //电话
                            if (telColumnInfo == null)
                                telColumnInfo = col;
                        }
                        else if (col.FieldName == "性别")
                        {
                            newDt.Columns[col.TabelColumnName2ID].DataType = typeof(int);
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion

            #region 创建数据
            foreach (DataRow dr in dt.Rows)
            {
                object[] obj = new object[dt.Columns.Count];
                dr.ItemArray.CopyTo(obj, 0);
                obj[1] = 0;
                obj[2] = DateTime.Now;
                obj[3] = UserID;
                newDt.Rows.Add(obj);
            }
            #endregion

            #endregion
            try
            {
                //导入数据库
                lock (this)
                {
                    //最大主键值
                    int maxRecid = BLL.TTable.Instance.GetMaxRecIdByTTName(tpageModel.TTName);
                    foreach (DataRow dr in newDt.Rows)
                    {
                        ++maxRecid;
                        dr["RecID"] = maxRecid;
                        allIds.Add((maxRecid).ToString());
                    }
                    BLL.Util.BulkCopyToDB(newDt, ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), tpageModel.TTName, out msg);
                }
                //校验电话是否重复 在数据库中校验效率要高点 强斐 2015-9-23
                //如果为空，则不校验
                if (telColumnInfo != null && allIds.Count > 0)
                {
                    tipinfo = CheckPhone(tpageModel, ref allIds, telColumnInfo);
                }
            }
            catch (Exception ex)
            {
                msg += ex.Message.ToString();
                BLL.Loger.Log4Net.Error("导出报错：", ex);
            }
            return string.Join(",", allIds.ToArray());
        }
        /// 电话校验
        /// <summary>
        /// 电话校验
        /// </summary>
        /// <param name="tpageModel"></param>
        /// <param name="tipinfo"></param>
        /// <param name="allIds"></param>
        /// <param name="telColumnInfo"></param>
        /// <returns></returns>
        private string CheckPhone(Entities.TPage tpageModel, ref List<string> allIds, ColumnInfo telColumnInfo)
        {
            string tipinfo = "";
            string[] rangid = new string[2];
            rangid[0] = allIds[0];
            rangid[1] = allIds[allIds.Count - 1];
            long projectID = CommonFunction.ObjectToLong(ProjectID);
            //查询所有不符合规则的数据
            Dictionary<string, List<string>> dic = BLL.OtherTaskInfo.Instance.DelSameTelForImportOtherData(projectID, tpageModel.TTName, telColumnInfo.TabelColumnName, rangid, BlackListCheckType);

            //重复的电话数据
            List<string> other_list = dic["重复数据"];
            //最原始的总数
            int total_count = allIds.Count;
            //成功总数
            int success_count = 0;
            //删除数
            int del_rep_count = 0;
            //删除数
            int del_cc_count = 0;
            //删除数
            int del_crm_count = 0;

            BLL.Util.SubtractionList(allIds, other_list, out success_count, out del_rep_count);

            //CC黑名单数据
            if (dic.ContainsKey("CC黑名单"))
            {
                other_list = dic["CC黑名单"];
                BLL.Util.SubtractionList(allIds, other_list, out success_count, out del_cc_count);
            }

            //CRM黑名单数据
            if (dic.ContainsKey("CRM黑名单"))
            {
                other_list = dic["CRM黑名单"];
                BLL.Util.SubtractionList(allIds, other_list, out success_count, out del_crm_count);
            }

            //提示信息
            tipinfo = string.Format("共选择{0}条数据，{1}条数据新增成功！", total_count, success_count);
            if (del_rep_count + del_cc_count + del_crm_count > 0)
            {
                tipinfo += "</br>（其中";

                if (del_rep_count > 0)
                {
                    tipinfo += string.Format("{0}条电话号码重复，", del_rep_count);
                }
                if (del_cc_count > 0)
                {
                    tipinfo += string.Format("{0}条属于C端免打扰，", del_cc_count);
                }
                if (del_crm_count > 0)
                {
                    tipinfo += string.Format("{0}条属于经销商，", del_crm_count);
                }
                tipinfo = tipinfo.Substring(0, tipinfo.Length - 1) + "）";
            }
            return tipinfo;
        }
        #endregion

        #region 客户回访
        /// 客户回访读取数据
        /// <summary>
        /// 客户回访读取数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool AddData_ReturnVisitCust(string fileName, ref string msg, ref DataTable dt)
        {
            string strids = "";
            bool isok = true;
            dt = GetImportData_CustID(fileName, out msg, out strids);

            //验证客户id是否存在
            DataTable custDt = BLL.ProjectInfo.Instance.p_GerNoExistsCustID(strids);
            if (custDt.Rows.Count > 0)
            {
                StringBuilder noExistsCustIDs = new StringBuilder();
                foreach (DataRow dr in custDt.Rows)
                {
                    noExistsCustIDs.Append(dr[0].ToString() + ",");
                }
                msg = "Excel中有的客户ID不存在：" + noExistsCustIDs.ToString();
                isok = false;
            }
            return isok;
        }
        /// 查找不存在的CRM客户ID
        /// <summary>
        /// 查找不存在的CRM客户ID
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="custIDs"></param>
        /// <param name="ColumnName"></param>
        private void CheckNoExistsCustID(ref string msg, StringBuilder custIDs, string ColumnName)
        {
            DataTable custDt = BLL.ProjectInfo.Instance.p_GerNoExistsCustID(custIDs.ToString().Substring(0, custIDs.Length - 1));
            if (custDt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in custDt.Rows)
                {
                    sb.Append(dr[0].ToString() + ",");
                }
                msg += "【" + ColumnName + "】列有的CRM客户ID不正确，没有找到对应的CRM客户！<br/>" + sb.ToString();
            }
        }
        /// 获取不在项目中存在的数据
        /// <summary>
        /// 获取不在项目中存在的数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private List<string> GetNotRepartCustID(List<string> ids)
        {
            if (ProjectID != "")
            {
                return BLL.ProjectDataSoure.Instance.GetNotExistsDataByProjectID(CommonFunction.ObjectToLong(ProjectID), string.Join(",", ids.ToArray()));
            }
            return ids;
        }
        #endregion

        #region 公用方法
        /// 从Excel文件中读取数据
        /// <summary>
        /// 从Excel文件中读取数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private DataTable GetImportData_Other(string fileName, out string msg)
        {
            msg = "";
            DataTable dt = new DataTable();
            BLL.Loger.Log4Net.Info("从Excel读取数据—1");
            //(1)连接EXCEL文件
            string ext = Path.GetExtension(fileName);
            string connStr = string.Empty;
            if (ext.ToLower() == ".xls")
            {
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                msg = "上传文件应为xls格式的文件";
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
                int rowNum = 0;
                while (idr.Read())
                {
                    if (rowNum == 0)
                    {
                        //列名
                        DataColumn dc;
                        for (int i = 0; i <= idr.FieldCount - 1; i++)
                        {
                            dc = new DataColumn(idr[i].ToString().Trim());
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        //数据
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i <= idr.FieldCount - 1; i++)
                        {
                            dr[i] = idr[i].ToString().Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                    rowNum++;
                }
            }
            BLL.Loger.Log4Net.Info("从Excel读取数据—2：读取条数：" + dt.Rows.Count);
            return dt;
        }
        /// 从Excel文件中读取数据——运营客服适用
        /// <summary>
        /// 从Excel文件中读取数据——运营客服适用
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private DataTable GetImportData_CustID(string fileName, out string msg, out string strids)
        {
            msg = "";
            strids = "";
            DataTable dt = new DataTable();
            BLL.Loger.Log4Net.Info("从Excel读取数据—1");
            //(1)连接EXCEL文件
            string ext = Path.GetExtension(fileName);
            string connStr = string.Empty;
            if (ext.ToLower() == ".xls")
            {
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                msg = "上传文件应为xls格式的文件";
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
                int rowNum = 0;
                //列名
                DataColumn dc = new DataColumn("CustIDs");
                dt.Columns.Add(dc);
                while (idr.Read())
                {
                    //数据
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i <= idr.FieldCount - 1; i++)
                    {
                        dr[i] = idr[i].ToString().Trim();
                    }
                    dt.Rows.Add(dr);
                    strids += "," + idr[0];
                    rowNum++;
                }
            }
            BLL.Loger.Log4Net.Info("从Excel读取数据—2：读取条数：" + dt.Rows.Count);
            if (strids != "")
            {
                strids = strids.Substring(1);
            }
            return dt;
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
        #endregion
    }

    /// 批量导入时，校验EXCEL的异常信息
    /// <summary>
    /// 批量导入时，校验EXCEL的异常信息
    /// </summary>
    public class CustInfoImportExceptionInfo
    {
        private StringBuilder info = new StringBuilder();
        /// <summary>
        /// 附加信息
        /// </summary>
        [JsonIgnore]
        public StringBuilder Info { get { return info; } set { info = value; } }

        public string Infomation { get { return info.ToString(); } }

        public CustInfoImportExceptionInfo()
        {
        }
    }

    public class ColumnInfo
    {
        /// <summary>
        /// Excel原来的列名
        /// </summary>
        public string ExcelColumnName { get; set; }

        /// <summary>
        /// 字段表里存储的列名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 物理表里的列名
        /// </summary>
        public string TabelColumnName { get; set; }

        /// <summary>
        /// 物理表里的ID对应列名
        /// </summary>
        public string TabelColumnName2ID { get; set; }

        /// <summary>
        /// 字段显示方式
        /// </summary>
        public string ShowCode { get; set; }

        /// <summary>
        /// 导入时是否允许为空 1为空 0不能为空
        /// </summary>
        public string TFInportIsNull { get; set; }

        /// <summary>
        /// 字段的默认值或数据源
        /// </summary>
        public string TFValue { get; set; }
    }
}