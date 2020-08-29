using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class ExportWorkOrder : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region
        private string RequestBrowser
        {
            get { return BLL.Util.GetCurrentRequestStr("Browser"); }
        }
        public string RequestOrderCreateTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("OrderCreateTime"); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //增加“任务列表--工单记录”导出功能验证逻辑
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUG100603"))
                {
                    WorkOrderBind();
                }
                else
                {
                    Response.Redirect(BLL.Util.GetNotAccessMsgPage("您没有导出权限"));
                    Response.End();
                }
            }
        }
        private void WorkOrderBind()
        {
            QueryWorkOrderInfo query = BLL.Util.BindQuery<QueryWorkOrderInfo>(this.Context);
            int userId = BLL.Util.GetLoginUserID();
            query.LoginID = userId;

            #region 数据权限
            ////判断当前人是否有全部数据权限
            //int RightType = (int)BLL.UserDataRigth.Instance.GetUserDataRigth(userId).RightType;
            ////如果没有
            //if (RightType != 2)
            //{
            //    DataTable dtBusiness = BLL.BusinessGroup.Instance.GetBusinessGroupByName("工单组");
            //    if (dtBusiness != null && dtBusiness.Rows.Count > 0)
            //    {
            //        //取当前人所对应的数据权限组
            //        Entities.QueryUserGroupDataRigth QueryUserGroupDataRigth = new Entities.QueryUserGroupDataRigth();
            //        QueryUserGroupDataRigth.UserID = userId;
            //        QueryUserGroupDataRigth.BGID = int.Parse(dtBusiness.Rows[0]["BGID"].ToString());
            //        int totcount = 0;
            //        DataTable dtUserGroupDataRigth = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigth(QueryUserGroupDataRigth, "", 1, 100000, out totcount);
            //        string Rolename = string.Empty;
            //        if (dtUserGroupDataRigth != null && dtUserGroupDataRigth.Rows.Count > 0)
            //        {
            //            if (dtUserGroupDataRigth.Rows[0]["RightType"].ToString() == "1") //本人权限
            //            {
            //                query.RightType = 1;
            //            }
            //            else //本组权限
            //            {
            //                query.RightType = 0;
            //            }
            //        }
            //        else  //如果没有数据权限
            //        {
            //            //-1表示没有权限
            //            query.RightType = -1;
            //        }
            //    }
            //    else
            //    {
            //        query.RightType = -1;
            //    }
            //}


            #endregion

            string orderstring = "";
            if (RequestOrderCreateTime == "1")
            {
                //orderstring = "Order By OrderNum ASC,woi.CreateTime Desc,woi.LastProcessDate ASC";
                orderstring = "Order By OrderNum ASC,CreateTime Desc,LastProcessDate ASC";
            }
            else
            {
                //orderstring = "Order By OrderNum ASC,woi.LastProcessDate ASC,woi.CreateTime Desc";
                orderstring = "Order By OrderNum ASC,LastProcessDate ASC,CreateTime Desc";
            }

            //DataTable dt = BLL.WorkOrderInfo.Instance.GetWorkOrderInfoForExport(query, (int)query.WorkCategory, userId, "Order By OrderNum ASC,LastProcessDate ASC");
            DataTable dt = BLL.WorkOrderInfo.Instance.GetWorkOrderInfoForExport(query, (int)query.WorkCategory, userId, orderstring);
            DataTable dtNew = new DataTable();

            DataColumn dcOrderID = new DataColumn("工单ID");
            dtNew.Columns.Add(dcOrderID);

            //工单类型：1个人，2经销商
            DataColumn dcWorkCategory = new DataColumn("工单类型");
            dtNew.Columns.Add(dcWorkCategory);

            DataColumn dcOrderCategory = new DataColumn("工单分类");
            dtNew.Columns.Add(dcOrderCategory);
            DataColumn dcDataSource = new DataColumn("数据来源");
            dtNew.Columns.Add(dcDataSource);
            DataColumn dcTitle = new DataColumn("工单标题");
            dtNew.Columns.Add(dcTitle);
            DataColumn dcCustName = new DataColumn("客户名称");
            DataColumn dcProvincel = new DataColumn("省份");
            DataColumn dcCity = new DataColumn("城市");
            DataColumn dcCounty = new DataColumn("区县");
            DataColumn dcAreaName = new DataColumn("大区");
            DataColumn dcContact = new DataColumn("联系人");
            DataColumn dcContactTel = new DataColumn("联系电话");

            //add 是否接通和未接通原因 by anyy 2015-11-20
            DataColumn dcIsEstablish = new DataColumn("是否接通");
            DataColumn dcNotEstablishReason = new DataColumn("未接通原因");

            DataColumn dcTag = new DataColumn("标签");
            DataColumn dcType = new DataColumn("是否投诉");
            DataColumn dcPriorityLevel = new DataColumn("优先级");
            DataColumn dcLastDate = new DataColumn("最晚处理时间");
            DataColumn dcWorkOrderStatus = new DataColumn("工单状态");
            DataColumn dcContent = new DataColumn("工单记录");
            DataColumn dcReceiver = new DataColumn("处理人");
            DataColumn dcReceiverDepartName = new DataColumn("处理人所属部门");

            DataColumn dcIsSales = new DataColumn("是否为营销顾问");
            DataColumn dcAttentionCarBrandName = new DataColumn("关注车型品牌名称");
            DataColumn dcAttentionCarSerialName = new DataColumn("关注车型系列名称");

            DataColumn dcAttentionCarTypeName = new DataColumn("关注车型车款名称");
            DataColumn dcSelectDealerName = new DataColumn("工单推荐经销商名称");
            DataColumn dcIsReturnVisit = new DataColumn("是否接受回访");
            DataColumn dcNominateActivity = new DataColumn("推荐活动");
            DataColumn dcSaleCarBrandName = new DataColumn("出售车型品牌名称");
            DataColumn dcSaleCarSerialName = new DataColumn("出售车型系列名称");
            DataColumn dcSaleCarTypeName = new DataColumn("出售车型车款名称");
            DataColumn dcIsRevert = new DataColumn("是否有回复");
            if (query.WorkCategory == 1)//个人工单
            {
                dtNew.Columns.Add(dcTag);
                dtNew.Columns.Add(dcContent);
                dtNew.Columns.Add(dcAttentionCarBrandName);
                dtNew.Columns.Add(dcAttentionCarSerialName);
                dtNew.Columns.Add(dcAttentionCarTypeName);
                dtNew.Columns.Add(dcSelectDealerName);
                dtNew.Columns.Add(dcIsReturnVisit);
                dtNew.Columns.Add(dcNominateActivity);
                dtNew.Columns.Add(dcSaleCarBrandName);
                dtNew.Columns.Add(dcSaleCarSerialName);
                dtNew.Columns.Add(dcSaleCarTypeName);
                dtNew.Columns.Add(dcProvincel);
                dtNew.Columns.Add(dcCity);
                dtNew.Columns.Add(dcAreaName);
            }
            else//经销商工单
            {
                dtNew.Columns.Add(dcCustName);
                dtNew.Columns.Add(dcProvincel);
                dtNew.Columns.Add(dcCity);
                dtNew.Columns.Add(dcCounty);
                dtNew.Columns.Add(dcAreaName);
                dtNew.Columns.Add(dcContact);
                dtNew.Columns.Add(dcContactTel);

                //客户回访的订单全是经销商的，有是否接通和未接通原因
                dtNew.Columns.Add(dcIsEstablish);
                dtNew.Columns.Add(dcNotEstablishReason);

                dtNew.Columns.Add(dcTag);
                dtNew.Columns.Add(dcType);
                dtNew.Columns.Add(dcPriorityLevel);
                dtNew.Columns.Add(dcLastDate);
                dtNew.Columns.Add(dcWorkOrderStatus);
                dtNew.Columns.Add(dcContent);
                dtNew.Columns.Add(dcReceiver);
                dtNew.Columns.Add(dcReceiverDepartName);
                dtNew.Columns.Add(dcIsSales);
                dtNew.Columns.Add(dcIsRevert);
            }
            DataColumn dcLastOptTime = new DataColumn("最后操作时间");
            dtNew.Columns.Add(dcLastOptTime);
            DataColumn dcCreateUser = new DataColumn("操作人");
            dtNew.Columns.Add(dcCreateUser);
            DataColumn dcCreateTime = new DataColumn("提交日期");
            dtNew.Columns.Add(dcCreateTime);

            foreach (DataRow dr in dt.Rows)
            {
                //dt.Columns[0].ColumnName
                DataRow drNew = dtNew.NewRow();

                //工单类型
                if (dr["WorkCategory"].ToString() == "1")
                {
                    drNew[dcWorkCategory] = "个人";
                }
                else if (dr["WorkCategory"].ToString() == "2")
                {
                    drNew[dcWorkCategory] = "经销商";
                }

                drNew[dcOrderID] = dr["OrderID"].ToString();
                drNew[dcOrderCategory] = dr["CategoryName"].ToString();
                drNew[dcDataSource] = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderDataSource), int.Parse(dr["DataSource"].ToString()));
                drNew[dcTitle] = dr["Title"].ToString();

                if (query.WorkCategory == 1)
                {
                    drNew[dcTag] = dr["TagName"].ToString();
                    drNew[dcContent] = dr["Content"].ToString();
                    drNew[dcAttentionCarBrandName] = dr["AttentionCarBrandName"].ToString();
                    drNew[dcAttentionCarSerialName] = dr["AttentionCarSerialName"].ToString();
                    drNew[dcAttentionCarTypeName] = dr["AttentionCarTypeName"].ToString();
                    drNew[dcSelectDealerName] = dr["SelectDealerName"].ToString();
                    drNew[dcIsReturnVisit] = dr["IsReturnVisit"].ToString() == "-2" ? "" : (dr["IsReturnVisit"].ToString() == "1" ? "是" : "否");
                    drNew[dcNominateActivity] = dr["NominateActivity"].ToString();
                    drNew[dcSaleCarBrandName] = dr["SaleCarBrandName"].ToString();
                    drNew[dcSaleCarSerialName] = dr["SaleCarSerialName"].ToString();
                    drNew[dcSaleCarTypeName] = dr["SaleCarTypeName"].ToString();
                    drNew[dcProvincel] = dr["CustProvince"].ToString();
                    drNew[dcCity] = dr["CustCity"].ToString();
                }
                else
                {
                    drNew[dcCustName] = dr["CustName"].ToString();
                    drNew[dcProvincel] = dr["Province"].ToString();
                    drNew[dcCity] = dr["City"].ToString();
                    drNew[dcCounty] = dr["Country"].ToString();
                    drNew[dcContact] = dr["Contact"].ToString();
                    drNew[dcContactTel] = dr["ContactTel"].ToString();

                    //增加是否接通和未接通原因

                    drNew[dcIsEstablish] = BLL.Util.GetIsNotStatus(dr["IsEstablish"].ToString());

                    string notEstablishReasonName = String.Empty;
                    string notEstablishReason = dr["NotEstablishReason"].ToString();
                    if ((!String.IsNullOrEmpty(notEstablishReason) || notEstablishReason.Trim() == "-1" || notEstablishReason.Trim() == "-2"))
                    {
                        notEstablishReasonName = BLL.Util.GetEnumOptText(typeof(Entities.NotEstablishReason), Int32.Parse(notEstablishReason));
                    }

                    drNew[dcNotEstablishReason] = notEstablishReasonName;


                    drNew[dcTag] = dr["TagName"].ToString();
                    drNew[dcType] = dr["IsComplaintType"].ToString() == "True" ? "是" : "否";
                    drNew[dcPriorityLevel] = dr["PriorityLevel"].ToString() == "1" ? "普通" : "紧急";
                    drNew[dcLastDate] = dr["LastProcessDate"].ToString();
                    drNew[dcWorkOrderStatus] = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderStatus), int.Parse(dr["WorkOrderStatus"].ToString()));
                    drNew[dcContent] = dr["Content"].ToString();
                    drNew[dcReceiver] = dr["ReceiverName"].ToString();
                    drNew[dcReceiverDepartName] = dr["ReceiverDepartName"].ToString();
                    drNew[dcIsSales] = dr["IsSales"].ToString() == "True" ? "是" : "否";
                    drNew[dcIsRevert] = dr["IsRevert"].ToString() == "True" ? "是" : "否";
                }

                //查询大区 强斐 2014-12-17
                BitAuto.YanFa.Crm2009.Entities.AreaInfo info = BLL.Util.GetAreaInfoByPCC(
                              BLL.Util.GetDataRowValue(drNew, dcProvincel.ColumnName),
                              BLL.Util.GetDataRowValue(drNew, dcCity.ColumnName),
                              BLL.Util.GetDataRowValue(drNew, dcCounty.ColumnName));

                drNew[dcAreaName] = info == null ? "" : info.DistinctName;

                drNew[dcLastOptTime] = dr["LastOptTime"].ToString();
                drNew[dcCreateUser] = dr["CreateUserName"].ToString();
                drNew[dcCreateTime] = dr["CreateTime"].ToString(); ;
                dtNew.Rows.Add(drNew);
            }

            //BitAuto.ISDC.CC2012.Web.AjaxServers.ExcelOperate.ExcelInOut.CreateEXCEL(dtNew, "工单记录结果表", RequestBrowser);
            BLL.Util.ExportToCSV("工单记录结果表", dtNew);

        }
    }
}