using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;


namespace BitAuto.ISDC.CC2012.Web.TaskManager
{
    public partial class TastExcelExport : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        /// <summary>
        /// 要导出的字段
        /// </summary>
        public string Fields
        {
            get
            {
                if (HttpContext.Current.Request["field"] != null)
                {
                    return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request["field"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string Where
        {
            get
            {
                if (HttpContext.Current.Request["where"] != null)
                {
                    return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request["where"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #region 属性
        private string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        private string RequestCustName  //客户姓名
        {
            get { return HttpContext.Current.Request["CustName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString()); }
        }
        private string RequestConsultID //咨询类型
        {
            get { return String.IsNullOrEmpty(HttpContext.Current.Request["ConsultID"]) ? "0" : HttpUtility.UrlDecode(HttpContext.Current.Request["ConsultID"].ToString()); }
        }
        private string RequestQuestionType  //问题类别
        {
            get { return HttpContext.Current.Request["QuestionType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QuestionType"].ToString()); }
        }
        private string RequestQuestionQuality    //问题性质
        {
            get { return HttpContext.Current.Request["QuestionQuality"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QuestionQuality"].ToString()); }
        }
        private string RequestIsComplaint   //是否投诉
        {
            get { return HttpContext.Current.Request["IsComplaint"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsComplaint"].ToString()); }
        }
        private string RequestProcessStatus //任务状态
        {
            get { return HttpContext.Current.Request["ProcessStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProcessStatus"].ToString()); }
        }
        private string RequestStatus    //处理状态
        {
            get { return HttpContext.Current.Request["Status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Status"].ToString()); }
        }
        private string RequestBeginTime //来电时间 （开始时间）
        {
            get { return HttpContext.Current.Request["BeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BeginTime"].ToString()); }
        }
        private string RequestEndTime    //来电时间 （结束时间）
        {
            get { return HttpContext.Current.Request["EndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EndTime"].ToString()); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            string fields = Fields;

            #region 替换字段

            fields = fields.Replace("dii.BrandID", "dbo.GetBrandListByCustID( chi.CustID)  as BrandID");
            fields = fields.Replace("cbi.Phone", "dbo.GetTelByCustID(cbi.CustID)  as Phone ");
            fields = fields.Replace("cbi.Email", "dbo.GetEmailByCustID(cbi.CustID) AS Email");
            fields = fields.Replace("dii.Remark1", "dii.Remark AS Remark1");

            fields = fields.Replace("cnc.CarBrandId1", "case  cnc.CarBrandId when 0 Then '' else CONVERT(VARCHAR(50),  cnc.CarBrandId)  end CarBrandId1,case  cnc.CarSerialId when 0 Then '' else CONVERT(VARCHAR(50),  cnc.CarSerialId)  end CarSerialId1");
            fields = fields.Replace("csc.CarBrandId2", "case  csc.CarBrandId when 0 Then '' else CONVERT(VARCHAR(50),  csc.CarBrandId)  end CarBrandId2,case  csc.CarSerialId when 0 Then '' else CONVERT(VARCHAR(50),  csc.CarSerialId)  end CarSerialId2");

            fields = fields.Replace("csc.CallRecord1", "csc.CallRecord AS CallRecord1");

            //替换
            fields = fields.Replace("cbi.Sex", "CASE cbi.Sex WHEN 1 THEN '男' WHEN 2 THEN '女' ELSE '未知' END Sex");
            fields = fields.Replace("cbi.ProvinceID", "(SELECT AreaName FROM CRM2009.dbo.AreaInfo WHERE AreaID=cbi.ProvinceID AND Level=1) AS ProvinceID");
            fields = fields.Replace("cbi.CityID", "(SELECT AreaName FROM CRM2009.dbo.AreaInfo WHERE AreaID=cbi.CityID AND Level=2) AS CityID");
            fields = fields.Replace("cbi.CountyID", "(SELECT AreaName FROM CRM2009.dbo.AreaInfo WHERE AreaID=cbi.CountyID AND Level=3) AS CountyID");
            //fields = fields.Replace("cbi.AreaID", "CASE cbi.AreaID WHEN 170001 THEN '北京大区'   WHEN 170002 THEN '北方大区'  WHEN 170003 THEN '南方大区'  WHEN 170004 THEN '华东大区'  WHEN 170005 THEN '西部大区'  ELSE '' END AreaID");
            //fields = fields.Replace("cbi.DataSource", "CASE cbi.DataSource WHEN 180001 THEN '呼叫中心' WHEN 180002 THEN '在线'  WHEN 180003 THEN '汽车通'   WHEN 180004 THEN '车易通'    ELSE '未知' END DataSource");
            fields = fields.Replace("cbi.AreaID", "case cbi.AreaID when -2 Then '' else CONVERT(VARCHAR(40),cbi.AreaID)  end AreaID");
            fields = fields.Replace("cbi.DataSource", " case cbi.DataSource when -2 Then '' else CONVERT(VARCHAR(50),cbi.DataSource)  end   DataSource");



            fields = fields.Replace("cbi.CustCategoryID", "CASE cbi.CustCategoryID WHEN 1 THEN '已购车' WHEN 2 THEN '未购车' WHEN 3 THEN '经销商'  ELSE '未知' END CustCategoryID");

            //
            fields = fields.Replace("bci.Age", "CASE bci.Age WHEN -2 THEN '' Else CONVERT(VARCHAR(10), bci.Age) END Age");
            fields = fields.Replace("bci.DriveAge", "CASE bci.DriveAge WHEN -2 THEN '' Else CONVERT(VARCHAR(10),bci.DriveAge) END DriveAge");
            // fields = fields.Replace("bci.Vocation", "CASE bci.Vocation WHEN 130001 THEN '一般职业'  WHEN 130002 THEN '农牧业'  WHEN 130003 THEN '渔业'  WHEN 130004 THEN '木材森林业'  WHEN 130005 THEN '矿业采掘业'  WHEN 130006 THEN '交通运输业'  WHEN 130007 THEN '餐饮旅游业'  WHEN 130008 THEN '建筑工程'  WHEN 130009 THEN '制造加工维修业'  WHEN 1300010 THEN '出版广告业'  WHEN 1300011 THEN '医药卫生保健'  WHEN 1300012 THEN '娱乐业'  WHEN 1300013 THEN '文教机构'  WHEN 1300014 THEN '宗教机构'  WHEN 1300015 THEN '邮政通信电力自来水'  WHEN 1300016 THEN '零售批发业'  WHEN 1300014 THEN '金融保险证券'  WHEN 1300018 THEN '家庭管理'  WHEN 1300019 THEN '公检法等执法检查机关'  WHEN 1300020 THEN '军人'   WHEN 1300021 THEN 'IT业（软硬件开发制作）'   WHEN 1300022 THEN '职业运动'   WHEN 1300023 THEN '无业人员'   WHEN 1300024 THEN '其他'  Else ''  END Vocation");
            fields = fields.Replace("bci.Vocation", "case bci.Vocation when -2 Then '' else CONVERT(VARCHAR(50),bci.Vocation)  end Vocation");

            fields = fields.Replace("bci.Marriage", "CASE bci.Marriage WHEN 0 THEN '未婚' WHEN 1 THEN '已婚' Else '' END Marriage");
            //fields = fields.Replace("bci.Income", "CASE bci.Income WHEN 140001 THEN '1000元/月以下'  WHEN 140002 THEN '1000-2000元/月'  WHEN 140003 THEN '2001-4000元/月'  WHEN 140004 THEN '4001-6000元/月'  WHEN 140005 THEN '6001-8000元/月'  WHEN 140006 THEN '8001-10000元/月'  WHEN 140007 THEN '10001-15000元/月'  WHEN 140008 THEN '15001-25000元/月'   WHEN 140009 THEN '25000元/月以上'   WHEN 140009 THEN '保密'   ELSE '' END Income");
            fields = fields.Replace("bci.Income", "case bci.Income when -2 Then '' else CONVERT(VARCHAR(50),bci.Income)  end  Income");


            fields = fields.Replace("bci.CarBrandId", "case  bci.CarBrandId when 0 Then '' else CONVERT(VARCHAR(50),  bci.CarBrandId)  end    CarBrandId");
            fields = fields.Replace("bci.CarSerialId", "case  bci.CarSerialId when 0 Then '' else CONVERT(VARCHAR(50),  bci.CarSerialId)  end  CarSerialId");

            fields = fields.Replace("bci.IsAttestation", "CASE bci.IsAttestation WHEN 0 THEN '否' WHEN 1 THEN '是'  END IsAttestation");
            fields = fields.Replace("dii.CityScope", "CASE dii.CityScope WHEN 10001 THEN '111城区' WHEN 10002 THEN '111郊区' WHEN 10003 THEN '6全城' WHEN 10004 THEN '224全城'   END CityScope");
            fields = fields.Replace("dii.MemberType", "CASE dii.MemberType WHEN 20004 THEN '4s' WHEN 20005 THEN '特许经销商' WHEN 20006 THEN '综合店'    END MemberType");
            fields = fields.Replace("dii.CarType", "CASE dii.CarType WHEN 30001 THEN '新车'  WHEN 30002 THEN '二手车'  WHEN 30003 THEN '新车/二手车'  END CarType");
            fields = fields.Replace("dii.MemberStatus", "CASE dii.MemberStatus WHEN 40001 THEN '会员页' WHEN 40002 THEN '旺店页' WHEN 40003 THEN '待创建'  END MemberStatus");
            //1 fields = fields.Replace("cnc.CarBrandId AS CarBrandId1", "(SELECT  top 1 Name FROM CRM2009.dbo.car_brand WHERE Brandid=cnc.CarBrandId) AS CarBrandId1");
            fields = fields.Replace("cnc.BuyCarTime", "CASE cnc.BuyCarTime WHEN 50001 THEN '一周内'  WHEN 50002 THEN '一月内'  WHEN 50003 THEN '半年内'  WHEN 50004 THEN '无计划'  END BuyCarTime");
            fields = fields.Replace("cnc.BuyOrDisplace", "CASE cnc.BuyOrDisplace WHEN 1 THEN '新购' WHEN 2 THEN '置换'  END BuyOrDisplace");
            fields = fields.Replace("cnc.AcceptTel", "CASE cnc.AcceptTel WHEN 1 THEN '接受' WHEN 0 THEN '不接受'  END AcceptTel");
            fields = fields.Replace("csc.QuestionType", "CASE csc.QuestionType WHEN 70001 THEN '买车' WHEN 70002 THEN '卖车' WHEN 70003 THEN '删除'  END QuestionType");

            //1 fields = fields.Replace("csc.CarBrandId AS CarBrandId2", "(SELECT top 1 Name FROM CRM2009.dbo.car_brand WHERE Brandid=csc.CarBrandId) AS CarBrandId2");
            //1 fields = fields.Replace("csc.SaleCarBrandId", "(SELECT top 1 Name FROM CRM2009.dbo.car_brand WHERE Brandid=csc.SaleCarBrandId) AS SaleCarBrandId");
            fields = fields.Replace("csc.SaleCarBrandId", "case  csc.SaleCarBrandId when 0 Then '' else CONVERT(VARCHAR(50),  csc.SaleCarBrandId)  end SaleCarBrandId,case  csc.SaleCarSerialId when 0 Then '' else CONVERT(VARCHAR(50),  csc.SaleCarSerialId)  end SaleCarSerialId");

            fields = fields.Replace("chi.CreateUserID", "(SELECT trueName FROM SysRightsManager.dbo.UserInfo WHERE UserID=chi.CreateUserID) AS CreateUserID");
            fields = fields.Replace("chi.CallTime", "case cbi.CallTime when -2 Then 0 else CONVERT(VARCHAR(10),cbi.CallTime) end CallTime ");

            #endregion

            int RecordCount = 0;
            QueryCustHistoryInfo query = BLL.CustHistoryInfo.Instance.GetQueryModel(RequestTaskID, RequestCustName, RequestBeginTime, RequestEndTime,
                                        RequestConsultID, RequestQuestionType, RequestQuestionQuality, RequestIsComplaint, RequestProcessStatus, RequestStatus, "");
            DataTable dt = BLL.CustHistoryInfo.Instance.GetCustHistoryInfoForExport(query, fields);

            DataSet ds = dt.DataSet;

            //foreach (DataColumn col in ds.Tables[0].Columns)
            //{
            //    // ds.Tables[0].Columns[0].DataType = System.Type.GetType("System.String");
            //    col.DataType = System.Type.GetType("System.String");
            //}
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                //string carMaster = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(model.CarMasterID.ToString()));
                //string carSerial = BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(model.CarSerialID.ToString()));
                //string carType = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(int.Parse(model.CarTypeID.ToString()));


                #region 替换成汉字



                int intVal = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dt.Columns.Contains("AreaID") && dr["AreaID"].ToString() != (-2).ToString() && int.TryParse(dr["AreaID"].ToString(), out intVal))
                    {
                        dr["AreaID"] = BLL.Util.GetEnumOptText(typeof(EnumArea), int.Parse(dr["AreaID"].ToString()));
                    }
                    if (dt.Columns.Contains("DataSource") && dr["DataSource"].ToString() != (-2).ToString() && int.TryParse(dr["DataSource"].ToString(), out intVal))
                    {
                        dr["DataSource"] = BLL.Util.GetEnumOptText(typeof(EnumDataSource), int.Parse(dr["DataSource"].ToString()));
                    }
                    if (dt.Columns.Contains("Vocation") && dr["Vocation"].ToString() != (-2).ToString() && int.TryParse(dr["Vocation"].ToString(), out intVal))
                    {
                        dr["Vocation"] = BLL.Util.GetEnumOptText(typeof(CustVocation), int.Parse(dr["Vocation"].ToString()));
                    }
                    if (dt.Columns.Contains("Income") && dr["Income"].ToString() != (-2).ToString() && int.TryParse(dr["Income"].ToString(), out intVal))
                    {
                        dr["Income"] = BLL.Util.GetEnumOptText(typeof(CustInCome), int.Parse(dr["Income"].ToString()));
                    }

                    #region 车型车款转成汉字

                    if (dt.Columns.Contains("CarBrandId1") && dr["CarBrandId1"].ToString() != "0" && int.TryParse(dr["CarBrandId1"].ToString(), out intVal))
                    {
                        dr["CarBrandId1"] = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(dr["CarBrandId1"].ToString()));

                        if (dt.Columns.Contains("CarSerialId1") && dr["CarSerialId1"].ToString() != "0" && int.TryParse(dr["CarSerialId1"].ToString(), out intVal))
                        {
                            dr["CarBrandId1"] += "-" + BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(dr["CarSerialId1"].ToString()));
                        }
                    }
                    if (dt.Columns.Contains("CarBrandId2") && dr["CarBrandId2"].ToString() != "0" && int.TryParse(dr["CarBrandId2"].ToString(), out intVal))
                    {
                        dr["CarBrandId2"] = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(dr["CarBrandId2"].ToString()));

                        if (dt.Columns.Contains("CarSerialId2") && dr["CarSerialId2"].ToString() != "0" && int.TryParse(dr["CarSerialId2"].ToString(), out intVal))
                        {
                            dr["CarBrandId2"] += "-" + BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(dr["CarSerialId2"].ToString())) ;
                        }
                    }

                    if (dt.Columns.Contains("CarBrandId") && dr["CarBrandId"].ToString() != "0" && int.TryParse(dr["CarBrandId"].ToString(), out intVal))
                    {
                        dr["CarBrandId"] = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(dr["CarBrandId"].ToString()));
                    }
                    if (dt.Columns.Contains("CarSerialId") && dr["CarSerialId"].ToString() != "0" && int.TryParse(dr["CarSerialId"].ToString(), out intVal))
                    {
                        dr["CarSerialId"] = BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(dr["CarSerialId"].ToString()));
                    }
                    if (dt.Columns.Contains("SaleCarBrandId") && dr["SaleCarBrandId"].ToString() != "0" && int.TryParse(dr["SaleCarBrandId"].ToString(), out intVal))
                    {
                        dr["SaleCarBrandId"] = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(dr["SaleCarBrandId"].ToString()));
                        //SaleCarSerialId
                        if (dt.Columns.Contains("SaleCarSerialId") && dr["SaleCarSerialId"].ToString() != "0" && int.TryParse(dr["SaleCarSerialId"].ToString(), out intVal))
                        {
                            dr["SaleCarBrandId"] += "-" + BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(dr["SaleCarSerialId"].ToString()));
                        }
                    }


                    #endregion


                }

                #endregion

                #region 替换字段名

                DataSet configDs = new DataSet();
                configDs.ReadXml(Server.MapPath("~/TaskManager/TaskFields.xml"));

                if (configDs.Tables["item"] != null)
                {
                    foreach (DataRow dr in configDs.Tables["item"].Rows)
                    {

                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            if (col.ColumnName == dr["name"].ToString())
                            {
                                col.ColumnName = dr["des"].ToString();
                            }
                        }
                    }
                }

                #endregion

                #region 删除不需要的列

                if (ds.Tables[0].Columns.Contains("RowNumber"))
                {
                    ds.Tables[0].Columns.Remove("RowNumber");
                }

                if (ds.Tables[0].Columns.Contains("CarSerialId1"))
                {
                    ds.Tables[0].Columns.Remove("CarSerialId1");
                }

                if (ds.Tables[0].Columns.Contains("CarSerialId2"))
                {
                    ds.Tables[0].Columns.Remove("CarSerialId2");
                }
                if (ds.Tables[0].Columns.Contains("SaleCarSerialId"))
                {
                    ds.Tables[0].Columns.Remove("SaleCarSerialId");
                }
                #endregion

                BLL.Util.InsertUserLog("导出了任务信息(共计：" + ds.Tables[0].Rows.Count.ToString() + "行)");

               // ExcelInOut.CreateEXCEL(ds, "Task" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));
                BLL.Util.ExportToCSV("个人客户任务" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), ds.Tables[0]);
            }
        }
    }
}