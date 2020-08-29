using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.SessionState;
namespace BitAuto.ISDC.CC2012.Web.DataImport
{
    public partial class UploadDeal : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region Common Properties

        //private string action;
        // <summary>
        // 小写
        // </summary>
        //public string Action
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(this.action) ?
        //            (this.action = HttpUtility.UrlDecode(this.Request["Action"] + "").Trim().ToLower())
        //            : this.action;
        //    }
        //    set { this.action = value; }
        //}

        //导入类型
        //public int CarType
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(this.Request["CarType"]) ? -1 : int.Parse(this.Request["CarType"]);
        //    }
        //}

        //private int currentPage = -1;
        //public int CurrentPage
        //{
        //    get
        //    {
        //        if (currentPage > 0) { return this.currentPage; }
        //        currentPage = PagerHelper.GetCurrentPage();
        //        return currentPage;
        //    }
        //    set { currentPage = value; }
        //}

        //private int pageSize = -1;
        //public int PageSize
        //{
        //    get
        //    {
        //        if (pageSize > 0) { return this.pageSize; }
        //        pageSize = PagerHelper.GetPageSize();
        //        return pageSize;
        //    }
        //    set { pageSize = value; }
        //}

        //public HttpRequest Request
        //{
        //    get { return HttpContext.Current.Request; }
        //}

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
                //throw new Exception("上传文件应为xls或者xlsx格式的文件");
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
                string firstSheetName = dtSheetName.Rows[0]["TABLE_NAME"].ToString();
                //读取第一个sheet填充数据sheetConfig.Name.Replace(".", "#") + "$"
                OleDbCommand command = new OleDbCommand("select * from [" + firstSheetName.Replace(".", "#") + "]", conn);
                IDataReader idr = command.ExecuteReader();
                int rowNum = 1;
                int startReadRow = 0;


                DataTable ExportDataTable = new DataTable();
                ExportDataTable.Columns.Add("username", typeof(System.String));
                ExportDataTable.Columns.Add("sex", typeof(System.String));
                ExportDataTable.Columns.Add("tel1", typeof(System.String));
                ExportDataTable.Columns.Add("custcategory", typeof(System.String));
                ExportDataTable.Columns.Add("tel2", typeof(System.String));
                ExportDataTable.Columns.Add("email", typeof(System.String));
                ExportDataTable.Columns.Add("province", typeof(System.String));
                ExportDataTable.Columns.Add("city", typeof(System.String));
                ExportDataTable.Columns.Add("county", typeof(System.String));
                ExportDataTable.Columns.Add("address", typeof(System.String));
                ExportDataTable.Columns.Add("datasource", typeof(System.String));
                ExportDataTable.Columns.Add("EnquiryType", typeof(System.String));
                ExportDataTable.Columns.Add("brand", typeof(System.String));
                ExportDataTable.Columns.Add("carmodel", typeof(System.String));
                ExportDataTable.Columns.Add("Dealer", typeof(System.String));
                ExportDataTable.Columns.Add("callrecord", typeof(System.String));
                ExportDataTable.Columns.Add("failinfo", typeof(System.String));



                msg += "{root:[";
                while (idr.Read())
                {
                    ++rowNum;
                    if (idr[0].ToString().Trim().Contains("姓名*"))
                    {
                        startReadRow = rowNum;
                    }
                    StringBuilder ex = new StringBuilder();
                    if (rowNum > startReadRow)
                    {
                        success = this.Valid(rowNum, idr, out ex, userid, ExportDataTable);
                        if (success)
                        {
                            successNum++;
                        }
                        else
                        {
                            FailureNum++;
                        }
                    }
                    if (!string.IsNullOrEmpty(ex.ToString()))
                    {
                        msg += "{'information':'" + ex.ToString() + "'},";
                    }
                }
                if (FailureNum == 0)
                {
                    msg += "{'information':''},";
                }

                msg = msg.Substring(0, msg.Length - 1);
                msg += "],success:[{'information':'导入成功" + successNum + "条，导入失败" + FailureNum + "条'}]}";
                idr.Close();
                idr = null;

            }
            return success;
        }

        /// <summary>
        /// 校验
        /// </summary>
        private bool Valid(int rowNum, IDataReader reader, out StringBuilder ex, int userid, DataTable dtExport)
        {
            ex = new StringBuilder();

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
                ex.Append("第" + rowNum + "行[姓名]不可为空。");
                insertflag = false;
            }
            else
            {
                if (BLL.Util.GetLength(UserName) > 50)
                {
                    ex.Append("第" + rowNum + "行[姓名]超长。");
                    insertflag = false;
                }
            }
            if (string.IsNullOrEmpty(Sex) || Sex.Length == 0)
            {
                ex.Append("第" + rowNum + "行[性别]不可为空。");
                insertflag = false;
            }
            else
            {
                if (Sex != "男" && Sex != "女")
                {
                    ex.Append("第" + rowNum + "行[性别]输入错误。");
                    insertflag = false;
                }
            }
            if (string.IsNullOrEmpty(Tel1) || Tel1.Length == 0)
            {
                ex.Append("第" + rowNum + "行[电话1]不可为空。");
                insertflag = false;
            }
            else
            {
                if (BLL.Util.IsTelephone(Tel1) == false && BLL.Util.IsHandset(Tel1) == false)
                {
                    ex.Append("第" + rowNum + "行[电话]输入错误。");
                    insertflag = false;
                }
            }

            if (string.IsNullOrEmpty(CustCategory) || CustCategory.Length == 0)
            {
                ex.Append("第" + rowNum + "行[客户分类]不可为空。");
                insertflag = false;
            }
            else
            {
                if (CustCategory != "已购车" && CustCategory != "未购车" && CustCategory != "经销商")
                {
                    ex.Append("第" + rowNum + "行[客户分类]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(Tel2))
            {
                if (BLL.Util.IsTelephone(Tel2) == false && BLL.Util.IsHandset(Tel2) == false)
                {
                    ex.Append("第" + rowNum + "行[电话2]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(Email))
            {
                if (BLL.Util.IsEmail(Email) == false)
                {
                    ex.Append("第" + rowNum + "行[邮箱]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(Address))
            {
                if (BLL.Util.GetLength(Address) > 200)
                {
                    ex.Append("第" + rowNum + "行[地址]超长。");
                    insertflag = false;
                }
            }

            if (!string.IsNullOrEmpty(DataSource))
            {
                if (DataSource != "呼叫中心" && DataSource != "在线" && DataSource != "汽车通" && DataSource != "车易通")
                {
                    ex.Append("第" + rowNum + "行[数据来源]输入错误。");
                    insertflag = false;
                }
            }

            if (!string.IsNullOrEmpty(EnquiryType))
            {
                if (EnquiryType != "新车" && EnquiryType != "二手车" && EnquiryType != "个人反馈" && EnquiryType != "活动" && EnquiryType != "个人用车" && EnquiryType != "个人其他" && EnquiryType != "经销商合作" && EnquiryType != "经销商反馈" && EnquiryType != "经销商其他")
                {
                    ex.Append("第" + rowNum + "行[咨询类型]输入错误。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(DealerName))
            {
                if (BLL.Util.GetLength(DealerName) > 200)
                {
                    ex.Append("第" + rowNum + "行[推荐经销商]超长。");
                    insertflag = false;
                }
            }
            if (!string.IsNullOrEmpty(PhoneRecord))
            {
                if (BLL.Util.GetLength(PhoneRecord) > 2000)
                {
                    ex.Append("第" + rowNum + "行[来电记录]超长。");
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
                    ex.Append("第" + rowNum + "行[地区（省）]输入错误。");
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
                    ex.Append("第" + rowNum + "行[地区（市）]输入错误。");
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
                    ex.Append("第" + rowNum + "行[地区（区县）]输入错误。");
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
                    ex.Append("第" + rowNum + "行[品牌]输入错误。");
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
                    ex.Append("第" + rowNum + "行[车系]输入错误。");
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
                    ex.Append("第" + rowNum + "行记录[姓名],[电话1]已存在。");
                    insertflag = false;
                }
                else
                {

                    Entities.CustBasicInfo CustBasicInfoModel = new Entities.CustBasicInfo();
                    CustBasicInfoModel.Status = 0;
                    CustBasicInfoModel.CustName = UserName;
                    CustBasicInfoModel.Sex = (Sex == "男" ? 1 : 2);
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
                    try
                    {
                        //功能废弃
                        CustID = null;// BLL.CustBasicInfo.Instance.Insert(CustBasicInfoModel);
                        Entities.CustTel custTel1 = new Entities.CustTel();
                        custTel1.Tel = Tel1;
                        custTel1.CustID = CustID;
                        BLL.CustTel.Instance.Insert(custTel1);
                        Entities.CustTel custTel2 = null;
                        if (!string.IsNullOrEmpty(Tel2))
                        {
                            custTel2 = new Entities.CustTel();
                            custTel2.Tel = Tel2;
                            custTel2.CustID = CustID;
                            BLL.CustTel.Instance.Insert(custTel2);
                        }
                        Entities.CustEmail custemail = null;
                        if (!string.IsNullOrEmpty(Email))
                        {
                            custemail = new Entities.CustEmail();
                            custemail.Email = Email;
                            custemail.CustID = CustID;
                            BLL.CustEmail.Instance.Insert(custemail);
                        }

                        //if (EnquiryType != "新车" && EnquiryType != "二手车" && EnquiryType != "个人反馈" && EnquiryType != "活动" && EnquiryType != "个人用车" && EnquiryType != "个人其他" && EnquiryType != "经销商合作" && EnquiryType != "经销商反馈" && EnquiryType != "经销商其他")
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
                        }
                        else if (EnquiryType == "活动")
                        {
                            Entities.ConsultActivity ConsultActivityModel = new Entities.ConsultActivity();
                            ConsultActivityModel.CustID = CustID;

                            if (!string.IsNullOrEmpty(Brand))
                            {
                                ConsultActivityModel.BrandName = Brand;
                            }

                            if (!string.IsNullOrEmpty(PhoneRecord))
                            {
                                ConsultActivityModel.CallRecord = PhoneRecord;
                            }
                            ConsultActivityModel.CreateTime = System.DateTime.Now;
                            ConsultActivityModel.CreateUserID = userid;
                            rerVal = BLL.ConsultActivity.Instance.Insert(ConsultActivityModel);
                        }
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
                        }


                    }
                    catch (Exception ex1)
                    {
                        ex.Append("第" + rowNum + "行数据插入报错。");
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
                        }
                    }
                }
            }
            if (insertflag == false)
            {

                DataRow newRow = dtExport.NewRow();
                newRow["username"] = UserName;
                newRow["sex"] = Sex;
                newRow["tel1"] = Tel1;
                newRow["custcategory"] = CustCategory;
                newRow["tel2"] = Tel2;
                newRow["email"] = Email;

                newRow["province"] = Province;
                newRow["city"] = City;
                newRow["county"] = County;

                newRow["address"] = Address;
                newRow["datasource"] = DataSource;

                newRow["EnquiryType"] = EnquiryType;
                newRow["brand"] = Brand;

                newRow["carmodel"] = Models;
                newRow["Dealer"] = DealerName;
                newRow["callrecord"] = PhoneRecord;
                newRow["failinfo"] = ex.ToString();
                dtExport.Rows.Add(newRow);
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



    }
}