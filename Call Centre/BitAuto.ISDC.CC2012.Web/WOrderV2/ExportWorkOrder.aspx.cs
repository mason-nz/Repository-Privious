using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2
{
    public partial class ExportWorkOrder : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region
        private string RequestBrowser
        {
            get { return BLL.Util.GetCurrentRequestStr("Browser"); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUT102102A"))
                {
                    WorkOrderExport();
                }
                else
                {
                    Response.Redirect(BLL.Util.GetNotAccessMsgPage("您没有导出权限"));
                    Response.End();
                }
            }
        }

        private void WorkOrderExport()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            QueryWOrderV2DataInfo query = BLL.Util.BindQuery<QueryWOrderV2DataInfo>(this.Context);
            query.CC_LoginID = BLL.Util.GetLoginUserID();
            string orderstring = " a.WorkOrderStatus ASC,a.CreateTime Desc ";
            DataTable dt = BLL.WOrderInfo.Instance.GetExportWorkOrderList(query, orderstring);
            BLL.WOrderInfo.LogToLog4("[工单导出] 查询工单累计耗时：" + sw.Elapsed.ToString());
            //获取CRM的访问分类数据
            Dictionary<int, string> dictCrmVistType = BLL.WOrderInfo.Instance.GetCrmVistType();
            BLL.WOrderInfo.LogToLog4("[工单导出] 查询CRM的访问分类数据累计耗时：" + sw.Elapsed.ToString());
            //循环处理          
            foreach (DataRow dr in dt.Rows)
            {
                dr["用户地区"] = GetAddress(CommonFunction.ObjectToString(dr["ProvinceID"]), CommonFunction.ObjectToString(dr["CityID"]), CommonFunction.ObjectToString(dr["CountyID"]));
                dr["工单状态"] = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WorkOrderStatus), CommonFunction.ObjectToInteger(dr["WorkOrderStatus"], -1));
                dr["工单来源"] = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WorkOrderDataSource), CommonFunction.ObjectToInteger(dr["DataSource"], -1));
                dr["工单类型"] = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum), CommonFunction.ObjectToInteger(dr["CategoryID"], -1));
                dr["投诉级别"] = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.ComplaintLevelEnum), CommonFunction.ObjectToInteger(dr["ComplaintLevel"], -1));
                int visitType = CommonFunction.ObjectToInteger(dr["VisitType"]);
                if (dictCrmVistType.ContainsKey(visitType))
                {
                    dr["访问分类"] = dictCrmVistType[visitType];
                }
                else
                {
                    dr["访问分类"] = "--";
                }
                int isEstablish = CommonFunction.ObjectToInteger(dr["IsEstablish"], -1);
                if (isEstablish == 1)
                {
                    dr["是否接通"] = "是";
                }
                else if (isEstablish == 0)
                {
                    dr["是否接通"] = "否";
                }
                else
                {
                    dr["是否接通"] = "--";
                }
                dr["未接通原因"] = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.NotEstablishReason), CommonFunction.ObjectToInteger(dr["NotEstablishReason"], -1));

                if (CommonFunction.ObjectToInteger(dr["P2RecID"]) == CommonFunction.ObjectToInteger(dr["P3RecID"]))
                {
                    //当只有一条处理记录时
                    dr["最后处理人"] = "";
                    dr["最后处理记录"] = "";
                    dr["最后处理时间"] = DBNull.Value;
                }

                int isReturnVisit = CommonFunction.ObjectToInteger(dr["IsReturnVisit"], -1);
                if (isReturnVisit == 0)
                {
                    dr["是否回访"] = "否";
                }
                else if (isReturnVisit == 1)
                {
                    dr["是否回访"] = "是";
                }
                else
                {
                    dr["是否回访"] = "--";
                }
            }
            //删除id列
            dt.Columns.Remove("ProvinceID");
            dt.Columns.Remove("CityID");
            dt.Columns.Remove("CountyID");
            dt.Columns.Remove("WorkOrderStatus");
            dt.Columns.Remove("DataSource");
            dt.Columns.Remove("CategoryID");
            dt.Columns.Remove("ComplaintLevel");
            dt.Columns.Remove("VisitType");
            dt.Columns.Remove("IsEstablish");
            dt.Columns.Remove("NotEstablishReason");
            dt.Columns.Remove("P3RecID");
            dt.Columns.Remove("P2RecID");
            dt.Columns.Remove("IsReturnVisit");
            BLL.WOrderInfo.LogToLog4("[工单导出] 循环处理累计耗时：" + sw.Elapsed.ToString());
            //导出
            BLL.Util.ExportToCSV("工单记录_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), dt);
            sw.Stop();
            BLL.WOrderInfo.LogToLog4("[工单导出] 导出完成累计耗时：" + sw.Elapsed.ToString());
        }

        private string GetAddress(string proviceID, string cityID, string countryID)
        {
            string returnData = "";

            if (DictionaryDataCache.Instance.AreaInfo_Province.ContainsKey(proviceID))
            {
                returnData += CommonFunction.ObjectToString(DictionaryDataCache.Instance.AreaInfo_Province[proviceID]["AreaName"]);
            }
            else
            {
                return "--";
            }
            if (DictionaryDataCache.Instance.AreaInfo_City.ContainsKey(cityID))
            {
                returnData += "-" + CommonFunction.ObjectToString(DictionaryDataCache.Instance.AreaInfo_City[cityID]["AreaName"]);
            }
            else
            {
                return returnData;
            }
            if (DictionaryDataCache.Instance.AreaInfo_County.ContainsKey(countryID))
            {
                returnData += "-" + CommonFunction.ObjectToString(DictionaryDataCache.Instance.AreaInfo_County[countryID]["AreaName"]);
            }
            else
            {
                return returnData;
            }

            return returnData;
        }
    }
}