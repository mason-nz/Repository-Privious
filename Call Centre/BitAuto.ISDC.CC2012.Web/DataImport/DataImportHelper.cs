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
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.DataImport
{
    public class DataImportHelper
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
        private const string UpladFilesPath = "/DataImport/UpLoad/";

        internal bool BatchImport(out string msg, int userid)
        {
            //保存文件
            string fileName = this.SaveFile();

            //校验 插入
            return DealDataImported(fileName, out msg, userid);

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
        private bool DealDataImported(string fileName, out string msg, int userid)
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
               // throw new Exception("上传文件应为xls或者xlsx格式的文件");
                throw new Exception("上传文件应为xls格式的文件");
            }
            //(2)读取校验数据
            List<DataImportExceptionInfo> exceptionInfoList = new List<DataImportExceptionInfo>();
            string MemberStr = string.Empty;
            //成功个数
            int successNum = 0;
            //失败个数
            int FailureNum = 0;
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();

                //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等    
                DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                //包含excel中表名的字符串数组
                //string firstSheetName = dtSheetName.Rows[0]["TABLE_NAME"].ToString();
                string firstSheetName = BLL.Util.GetExcelSheetName(fileName);
                //读取第一个sheet填充数据sheetConfig.Name.Replace(".", "#") + "$"
                OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName.Replace(".", "#") + "]", conn);
                IDataReader idr = command.ExecuteReader();
                int rowNum = 1;
                int startReadRow = 0;

                string msgExport = "";

                msg += "{root:[";
                msgExport += "ExportData:[";
                while (idr.Read())
                {
                    ++rowNum;
                    if (idr[0].ToString().Trim().Contains("姓名*"))
                    {
                        startReadRow = rowNum;
                    }
                    StringBuilder ex = new StringBuilder();
                    StringBuilder msgExportstr = new StringBuilder();
                    if (rowNum > startReadRow)
                    {
                        string UserName = string.Empty;
                        UserName = idr[0].ToString().Trim();

                        string Sex = string.Empty;
                        Sex = idr[1].ToString().Trim();

                        string Tel1 = string.Empty;
                        Tel1 = idr[2].ToString().Trim();

                        string CustCategory = string.Empty;
                        CustCategory = idr[3].ToString().Trim();
                        if (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Sex) && string.IsNullOrEmpty(Tel1) && string.IsNullOrEmpty(CustCategory))
                        {
                        }
                        else
                        {
                            success = this.Valid(rowNum, idr, out ex, userid, out msgExportstr);
                            if (success)
                            {
                                successNum++;
                            }
                            else
                            {
                                FailureNum++;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(ex.ToString()))
                    {
                        string Message = BLL.Util.EscapeString(ex.ToString());
                        msg += "{'information':'" + Message + "'},";
                        msgExport += "{'information':'" + msgExportstr.ToString() + "'},";
                    }
                }
                if (FailureNum == 0)
                {
                    msg += "{'information':'" + BLL.Util.EscapeString("") + "'},";
                    msgExport += "{'information':''},";
                }
                msg = msg.Substring(0, msg.Length - 1);
                msgExport = msgExport.Substring(0, msgExport.Length - 1) + "],";
                msg += "]," + msgExport + "success:[{'information':'导入成功" + successNum + "条，导入失败" + FailureNum + "条'}]}";
                idr.Close();
                idr = null;

            }
            return success;
        }

        /// <summary>
        /// 校验
        /// </summary>
        private bool Valid(int rowNum, IDataReader reader, out StringBuilder ex, int userid, out StringBuilder exportstr)
        {
            ex = new StringBuilder();
            exportstr = new StringBuilder();

            string UserName = string.Empty;
            UserName = reader[0].ToString().Trim();

            string Sex = string.Empty;
            Sex = reader[1].ToString().Trim();

            string Tel1 = string.Empty;
            Tel1 = reader[2].ToString().Trim();

            string CustCategory = string.Empty;
            CustCategory = reader[3].ToString().Trim();

            string Tel2 = string.Empty;
            Tel2 = reader[4].ToString().Trim();

            string Email = string.Empty;
            Email = reader[5].ToString().Trim();

            string Province = string.Empty;
            Province = reader[6].ToString().Trim();

            string City = string.Empty;
            City = reader[7].ToString().Trim();

            string County = string.Empty;
            County = reader[8].ToString().Trim();

            string Address = string.Empty;
            Address = reader[9].ToString().Trim();

            string DataSource = string.Empty;
            DataSource = reader[10].ToString().Trim();
            //咨询类型
            string EnquiryType = string.Empty;
            EnquiryType = reader[11].ToString().Trim();


            string Brand = string.Empty;
            Brand = reader[12].ToString().Trim();

            string Models = string.Empty;
            Models = reader[13].ToString().Trim();

            //推荐经销商名称
            string DealerName = string.Empty;
            DealerName = reader[14].ToString().Trim();

            //来电记录
            string PhoneRecord = string.Empty;
            PhoneRecord = reader[15].ToString().Trim();

            bool insertflag = true;




            //会员ID号验证
            if (string.IsNullOrEmpty(UserName) || UserName.Length == 0)
            {
                ex.Append("[姓名]不可为空。");
                insertflag = false;
            }
            else
            {
                if (BLL.Util.GetLength(UserName) > 50)
                {
                    ex.Append("[姓名]超长。");
                    insertflag = false;
                }
            }
            if (string.IsNullOrEmpty(Sex) || Sex.Length == 0)
            {
                ex.Append("[性别]不可为空。");
                insertflag = false;
            }
            else
            {
                if (Sex != "男" && Sex != "女")
                {
                    ex.Append("[性别]输入错误。");
                    insertflag = false;
                }
            }
            if (string.IsNullOrEmpty(Tel1) || Tel1.Length == 0)
            {
                ex.Append("[电话1]不可为空。");
                insertflag = false;
            }
            else
            {
                if (BLL.Util.IsTelephone(Tel1) == false && BLL.Util.IsHandset(Tel1) == false)
                {
                    ex.Append("[电话]输入错误。");
                    insertflag = false;
                }
            }

            if (string.IsNullOrEmpty(CustCategory) || CustCategory.Length == 0)
            {
                ex.Append("[客户分类]不可为空。");
                insertflag = false;
            }
            else
            {
                if (CustCategory != "已购车" && CustCategory != "未购车" && CustCategory != "经销商")
                {
                    ex.Append("[客户分类]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(Tel2))
            {
                if (BLL.Util.IsTelephone(Tel2) == false && BLL.Util.IsHandset(Tel2) == false)
                {
                    ex.Append("[电话2]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(Email))
            {
                if (BLL.Util.IsEmail(Email) == false)
                {
                    ex.Append("[邮箱]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(Address))
            {
                if (BLL.Util.GetLength(Address) > 200)
                {
                    ex.Append("[地址]超长。");
                    insertflag = false;
                }
            }

            if (!string.IsNullOrEmpty(DataSource))
            {
                if (DataSource != "呼叫中心" && DataSource != "在线" && DataSource != "汽车通" && DataSource != "车易通")
                {
                    ex.Append("[数据来源]输入错误。");
                    insertflag = false;
                }
            }

            if (!string.IsNullOrEmpty(EnquiryType))
            {
                if (EnquiryType != "新车" && EnquiryType != "二手车" && EnquiryType != "个人反馈" && EnquiryType != "活动" && EnquiryType != "个人用车" && EnquiryType != "个人其他" && EnquiryType != "经销商合作" && EnquiryType != "经销商反馈" && EnquiryType != "经销商其他")
                {
                    ex.Append("[咨询类型]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(DealerName))
            {
                if (BLL.Util.GetLength(DealerName) > 200)
                {
                    ex.Append("[推荐经销商]超长。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(PhoneRecord))
            {
                if (BLL.Util.GetLength(PhoneRecord) > 2000)
                {
                    ex.Append("[来电记录]超长。");
                    insertflag = false;
                }
            }
            string provinceID = "0";
            //验证省份城市
            if (!string.IsNullOrEmpty(Province))
            {
                provinceID = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaIdByName(Province, 1);
                if (provinceID == "0")
                {
                    ex.Append("[地区（省）]输入错误。");
                    insertflag = false;
                }
            }
            string CityID = "0";
            //验证省份城市
            if (!string.IsNullOrEmpty(City))
            {
                CityID = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaIdByName(City, 2);
                if (CityID == "0")
                {
                    ex.Append("[地区（市）]输入错误。");
                    insertflag = false;
                }
            }
            string CountryID = "0";
            //验证区县
            if (!string.IsNullOrEmpty(County))
            {
                CountryID = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaIdByName(County, 3);
                if (CountryID == "0")
                {
                    ex.Append("[地区（区县）]输入错误。");
                    insertflag = false;
                }
            }
            //验证车型
            string BrandID = "0";
            if (!string.IsNullOrEmpty(Brand))
            {
                DataTable dt = BLL.BuyCarInfo.Instance.GetCarBrandByName(Brand);
                if (dt != null && dt.Rows.Count > 0)
                {
                    BrandID = dt.Rows[0]["Brandid"].ToString();
                }
                else
                {
                    ex.Append("[品牌]输入错误。");
                    insertflag = false;
                }
            }

            //验证车系
            string CarModelsID = "0";
            if (!string.IsNullOrEmpty(Models))
            {
                DataTable dt = BLL.BuyCarInfo.Instance.GetCarSerialByName(Models);
                if (dt != null && dt.Rows.Count > 0)
                {
                    CarModelsID = dt.Rows[0]["serialid"].ToString();
                }
                else
                {
                    ex.Append("[车系]输入错误。");
                    insertflag = false;
                }
            }

            //如果验证通过，插入
            if (insertflag)
            {
                bool flag = false;
                flag = BLL.CustBasicInfo.Instance.IsExistsByCustNameAndTel(UserName, Tel1);

                if (flag)
                {
                    ex.Append("记录[姓名],[电话1]已存在。");
                    insertflag = false;
                }
                else
                {

                    Entities.CustBasicInfo CustBasicInfoModel = new Entities.CustBasicInfo();
                    CustBasicInfoModel.Status = 0;
                    CustBasicInfoModel.CustName = UserName;
                    CustBasicInfoModel.Sex = (Sex == "男" ? 1 : 2);
                    CustBasicInfoModel.CreateTime = System.DateTime.Now;
                    CustBasicInfoModel.CreateUserID = userid;
                    if (CustCategory == "已购车")
                    {
                        CustBasicInfoModel.CustCategoryID = 1;
                    }
                    else if (CustCategory == "未购车")
                    {
                        CustBasicInfoModel.CustCategoryID = 2;
                    }
                    else
                    {
                        CustBasicInfoModel.CustCategoryID = 3;
                    }
                    if (provinceID != "0")
                    {
                        CustBasicInfoModel.ProvinceID = Convert.ToInt32(provinceID);
                    }
                    if (CityID != "0")
                    {
                        CustBasicInfoModel.CityID = Convert.ToInt32(CityID);
                    }
                    if (CountryID != "0")
                    {
                        CustBasicInfoModel.CountyID = Convert.ToInt32(CountryID);
                    }

                    if (!string.IsNullOrEmpty(Address))
                    {
                        CustBasicInfoModel.Address = Address;
                    }

                    if (!string.IsNullOrEmpty(DataSource))
                    {
                        if (DataSource == "呼叫中心")
                        {
                            CustBasicInfoModel.DataSource = 180001;
                        }
                        else if (DataSource == "在线")
                        {
                            CustBasicInfoModel.DataSource = 180002;
                        }
                        else if (DataSource == "汽车通")
                        {
                            CustBasicInfoModel.DataSource = 180003;
                        }
                        else if (DataSource == "车易通")
                        {
                            CustBasicInfoModel.DataSource = 180004;
                        }
                    }
                    string CustID = string.Empty;
                    int rerVal = 0;
                    //客户历史记录信息主键
                    int custhistoryVal = 0;
                    try
                    {
                        //功能废弃
                        CustID = null;// BLL.CustBasicInfo.Instance.Insert(CustBasicInfoModel);
                        Entities.CustTel custTel1 = new Entities.CustTel();
                        //电话去掉‘-’

                        if (Tel1.IndexOf('-') > 0)
                        {
                            custTel1.Tel = Tel1.Remove(Tel1.IndexOf('-'), 1);
                        }
                        else
                        {
                            custTel1.Tel = Tel1;
                        }


                        custTel1.CustID = CustID;
                        custTel1.CreateTime = System.DateTime.Now;
                        custTel1.CreateUserID = userid;
                        BLL.CustTel.Instance.Insert(custTel1);
                        Entities.CustTel custTel2 = null;
                        if (!string.IsNullOrEmpty(Tel2))
                        {
                            custTel2 = new Entities.CustTel();
                            if (Tel2.IndexOf('-') > 0)
                            {
                                custTel2.Tel = Tel2.Remove(Tel2.IndexOf('-'), 1);
                            }
                            else
                            {
                                custTel2.Tel = Tel2;
                            }
                            custTel2.CustID = CustID;
                            custTel2.CreateTime = System.DateTime.Now;
                            custTel2.CreateUserID = userid;
                            BLL.CustTel.Instance.Insert(custTel2);
                        }
                        Entities.CustEmail custemail = null;
                        if (!string.IsNullOrEmpty(Email))
                        {
                            custemail = new Entities.CustEmail();
                            custemail.Email = Email;
                            custemail.CustID = CustID;
                            custemail.CreateTime = System.DateTime.Now;
                            custemail.CreateUserID = userid;
                            BLL.CustEmail.Instance.Insert(custemail);
                        }

                        //if (EnquiryType != "新车" && EnquiryType != "二手车" && EnquiryType != "个人反馈" && EnquiryType != "活动" && EnquiryType != "个人用车" && EnquiryType != "个人其他" && EnquiryType != "经销商合作" && EnquiryType != "经销商反馈" && EnquiryType != "经销商其他")

                        Entities.CustHistoryInfo CustHistoryInfoModel = null;

                        if (EnquiryType == "新车")
                        {
                            Entities.ConsultNewCar ConsultNewCarModel = new Entities.ConsultNewCar();
                            ConsultNewCarModel.CustID = CustID;
                            if (BrandID != "0")
                            {
                                ConsultNewCarModel.CarBrandId = Convert.ToInt32(BrandID);
                            }
                            if (CarModelsID != "0")
                            {
                                ConsultNewCarModel.CarSerialId = Convert.ToInt32(CarModelsID);
                            }
                            if (!string.IsNullOrEmpty(DealerName))
                            {
                                ConsultNewCarModel.DealerName = DealerName;
                            }
                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultNewCarModel.CallRecord = PhoneRecord;
                            }
                            ConsultNewCarModel.CreateTime = System.DateTime.Now;
                            ConsultNewCarModel.CreateUserID = userid;
                            rerVal = BLL.ConsultNewCar.Instance.Insert(ConsultNewCarModel);

                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.NewCar, rerVal, userid);

                        }
                        else if (EnquiryType == "二手车")
                        {
                            Entities.ConsultSecondCar ConsultSecondCarModel = new Entities.ConsultSecondCar();
                            ConsultSecondCarModel.CustID = CustID;
                            if (BrandID != "0")
                            {
                                ConsultSecondCarModel.CarBrandId = Convert.ToInt32(BrandID);

                            }
                            if (CarModelsID != "0")
                            {
                                ConsultSecondCarModel.CarSerialId = Convert.ToInt32(CarModelsID);
                            }

                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultSecondCarModel.CallRecord = PhoneRecord;
                            }
                            ConsultSecondCarModel.CreateTime = System.DateTime.Now;
                            ConsultSecondCarModel.CreateUserID = userid;
                            rerVal = BLL.ConsultSecondCar.Instance.Insert(ConsultSecondCarModel);

                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.SecondCar, rerVal, userid);
                        }
                        else if (EnquiryType == "个人反馈")
                        {
                            Entities.ConsultPFeedback ConsultPFeedbackModel = new Entities.ConsultPFeedback();
                            ConsultPFeedbackModel.CustID = CustID;


                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultPFeedbackModel.CallRecord = PhoneRecord;
                            }
                            ConsultPFeedbackModel.CreateTime = System.DateTime.Now;
                            ConsultPFeedbackModel.CreateUserID = userid;
                            rerVal = BLL.ConsultPFeedback.Instance.Insert(ConsultPFeedbackModel);

                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.PFeedback, rerVal, userid);
                        }
                        //else if (EnquiryType == "活动")
                        //{
                        //    Entities.ConsultActivity ConsultActivityModel = new Entities.ConsultActivity();
                        //    ConsultActivityModel.CustID = CustID;

                        //    if (!string.IsNullOrEmpty(Brand))
                        //    {
                        //        ConsultActivityModel.BrandName = Brand;
                        //    }

                        //    if (!string.IsNullOrEmpty(PhoneRecord))
                        //    {
                        //        ConsultActivityModel.CallRecord = PhoneRecord;
                        //    }
                        //    ConsultActivityModel.CreateTime = System.DateTime.Now;
                        //    ConsultActivityModel.CreateUserID = userid;
                        //    rerVal = BLL.ConsultActivity.Instance.Insert(ConsultActivityModel);

                        //    CustHistoryInfoModel = new CustHistoryInfo();

                        //    GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.Activity, rerVal, userid);
                        //}
                        else if (EnquiryType == "个人其他")
                        {
                            Entities.ConsultPOther ConsultPOtherModel = new Entities.ConsultPOther();
                            ConsultPOtherModel.CustID = CustID;

                            ConsultPOtherModel.CreateTime = System.DateTime.Now;
                            ConsultPOtherModel.CreateUserID = userid;

                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultPOtherModel.CallRecord = PhoneRecord;
                            }
                            rerVal = BLL.ConsultPOther.Instance.Insert(ConsultPOtherModel);


                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.POther, rerVal, userid);
                        }

                        else if (EnquiryType == "个人用车")
                        {
                            Entities.ConsultPUseCar ConsultPUseCarModel = new Entities.ConsultPUseCar();
                            ConsultPUseCarModel.CustID = CustID;

                            ConsultPUseCarModel.CreateTime = System.DateTime.Now;
                            ConsultPUseCarModel.CreateUserID = userid;

                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultPUseCarModel.CallRecord = PhoneRecord;
                            }
                            rerVal = BLL.ConsultPUseCar.Instance.Insert(ConsultPUseCarModel);


                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.PUseCar, rerVal, userid);
                        }

                        else if (EnquiryType == "经销商合作")
                        {
                            Entities.ConsultDCoop ConsultDCoopCarModel = new Entities.ConsultDCoop();
                            ConsultDCoopCarModel.CustID = CustID;
                            ConsultDCoopCarModel.Type = 1;
                            ConsultDCoopCarModel.CreateTime = System.DateTime.Now;
                            ConsultDCoopCarModel.CreateUserID = userid;

                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultDCoopCarModel.CallRecord = PhoneRecord;
                            }
                            rerVal = BLL.ConsultDCoop.Instance.Insert(ConsultDCoopCarModel);

                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.DCoop, rerVal, userid);
                        }
                        else if (EnquiryType == "经销商反馈")
                        {
                            Entities.ConsultDCoop ConsultDCoopCarModel = new Entities.ConsultDCoop();
                            ConsultDCoopCarModel.CustID = CustID;
                            ConsultDCoopCarModel.Type = 2;
                            ConsultDCoopCarModel.CreateTime = System.DateTime.Now;
                            ConsultDCoopCarModel.CreateUserID = userid;

                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultDCoopCarModel.CallRecord = PhoneRecord;
                            }
                            rerVal = BLL.ConsultDCoop.Instance.Insert(ConsultDCoopCarModel);

                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.DCoopFeedback, rerVal, userid);
                        }
                        else if (EnquiryType == "经销商其他")
                        {
                            Entities.ConsultDCoop ConsultDCoopCarModel = new Entities.ConsultDCoop();
                            ConsultDCoopCarModel.CustID = CustID;
                            ConsultDCoopCarModel.Type = 3;
                            ConsultDCoopCarModel.CreateTime = System.DateTime.Now;
                            ConsultDCoopCarModel.CreateUserID = userid;

                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultDCoopCarModel.CallRecord = PhoneRecord;
                            }
                            rerVal = BLL.ConsultDCoop.Instance.Insert(ConsultDCoopCarModel);

                            CustHistoryInfoModel = new CustHistoryInfo();

                            GetCustHistoryInfoModel(CustHistoryInfoModel, CustID, (int)ConsultType.DCoopOther, rerVal, userid);
                        }
                        //插入客户历史记录
                        if (CustHistoryInfoModel != null)
                        {
                            //功能废弃
                            custhistoryVal = BLL.CustHistoryInfo.Instance.Insert(CustHistoryInfoModel);
                        }


                    }
                    catch (Exception ex1)
                    {
                        ex.Append("数据插入报错。");
                        insertflag = false;
                        if (CustID != string.Empty)
                        {
                            //回滚数据
                            BLL.CustBasicInfo.Instance.Delete(CustID);
                            BLL.CustTel.Instance.Delete(CustID);
                            BLL.CustEmail.Instance.Delete(CustID);
                            if (EnquiryType == "新车")
                            {

                                BLL.ConsultNewCar.Instance.Delete(rerVal);
                            }
                            else if (EnquiryType == "二手车")
                            {

                                BLL.ConsultSecondCar.Instance.Delete(rerVal);
                            }
                            else if (EnquiryType == "个人反馈")
                            {
                                BLL.ConsultPFeedback.Instance.Delete(rerVal);
                            }
                            else if (EnquiryType == "活动")
                            {
                                BLL.ConsultActivity.Instance.Delete(rerVal);
                            }
                            else if (EnquiryType == "个人其他")
                            {
                                BLL.ConsultPOther.Instance.Delete(rerVal);
                            }

                            else if (EnquiryType == "个人用车")
                            {
                                BLL.ConsultPUseCar.Instance.Delete(rerVal);
                            }

                            else if (EnquiryType == "经销商合作")
                            {

                                BLL.ConsultDCoop.Instance.Delete(rerVal);
                            }
                            else if (EnquiryType == "经销商反馈")
                            {
                                BLL.ConsultDCoop.Instance.Delete(rerVal);
                            }
                            else if (EnquiryType == "经销商其他")
                            {
                                BLL.ConsultDCoop.Instance.Delete(rerVal);
                            }
                            if (custhistoryVal != 0)
                            {
                                BLL.CustHistoryInfo.Instance.Delete(custhistoryVal);
                            }
                        }
                    }
                }
            }
            if (insertflag == false)
            {
                //导出内容
                exportstr.Append(UserName + "|");
                exportstr.Append(Sex + "|");
                exportstr.Append(Tel1 + "|");
                exportstr.Append(CustCategory + "|");
                exportstr.Append(Tel2 + "|");
                exportstr.Append(Email + "|");

                exportstr.Append(Province + "|");

                exportstr.Append(City + "|");
                exportstr.Append(County + "|");

                exportstr.Append(Address + "|");
                exportstr.Append(DataSource + "|");

                exportstr.Append(EnquiryType + "|");
                exportstr.Append(Brand + "|");

                exportstr.Append(Models + "|");
                exportstr.Append(DealerName + "|");

                exportstr.Append(PhoneRecord + "|");
                exportstr.Append(ex.ToString());
            }
            return insertflag;
        }
        /// <summary>
        /// 批量导入时，校验EXCEL的异常信息
        /// </summary>
        public class DataImportExceptionInfo
        {

            private StringBuilder info = new StringBuilder();
            /// <summary>
            /// 附加信息
            /// </summary>
            [JsonIgnore]
            public StringBuilder Info { get { return info; } set { info = value; } }

            public string Infomation { get { return info.ToString(); } }

            public DataImportExceptionInfo()
            {
            }
        }

        /// <summary>
        /// 取客户历史记录信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CustID"></param>
        /// <param name="ConsultID"></param>
        /// <param name="racid"></param>
        /// <param name="userid"></param>
        public void GetCustHistoryInfoModel(Entities.CustHistoryInfo model, string CustID, int ConsultID, int racid, int userid)
        {
            model.TaskID = "TK" + BLL.ConsultNewCar.Instance.GetCurrMaxID().ToString();
            //int intVal = 0;
            //if (CALLID != "" && int.TryParse(CALLID, out intVal))
            //{
            //    model.CallRecordID = int.Parse(CALLID);
            //}
            model.CustID = CustID;
            model.RecordType = 1;
            model.ConsultID = int.Parse(ConsultID.ToString());
            model.ConsultDataID = int.Parse(racid.ToString());
            model.QuestionQuality = (int)QuestionNature.NatureCommon;//默认普通
            model.ProcessStatus = (int)EnumTaskStatus.TaskStatusOver;
            model.CreateTime = DateTime.Now;
            model.CreateUserID = userid;
        }
    }
}