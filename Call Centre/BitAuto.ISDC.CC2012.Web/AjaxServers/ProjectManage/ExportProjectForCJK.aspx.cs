using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public partial class ExportProjectForCJK : PageBase
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }
        private string tasksubstart
        {
            get
            {

                return HttpContext.Current.Request["tasksubstart"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["tasksubstart"].ToString());
            }
        }
        private string tasksubend
        {
            get
            {

                return HttpContext.Current.Request["tasksubend"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["tasksubend"].ToString());
            }
        }
        private string taskcreatestart
        {
            get
            {

                return HttpContext.Current.Request["taskcreatestart"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["taskcreatestart"].ToString());
            }
        }
        private string taskcreateend
        {
            get
            {

                return HttpContext.Current.Request["taskcreateend"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["taskcreateend"].ToString());
            }
        }
        string msg = "";//错误信息
        int userId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024BUT500605"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                int projectID;
                if (!int.TryParse(ProjectID, out projectID))
                {
                    msg += "项目ID错误，导出失败！";
                    return;
                }
                Entities.QueryLeadsTask query = new QueryLeadsTask();
                query.LoginID = userId;
                query.ProjectID = projectID;
                int totalCount = 0;
                //modify by qizq 2014-11-24 给导出加任务创建，任务提交等过滤
                DataTable dt = BLL.LeadsTask.Instance.GetLeadsTaskForExport(query, "", 1, -1, out totalCount, taskcreatestart, taskcreateend, tasksubstart, tasksubend);
                if (dt != null)
                {
                    Export(dt);
                }
            }
        }
        private void Export(DataTable dt)
        {
            //项目名称、姓名、性别、电话、地区、下单品牌、下单车型、下单日期、匹配车型、是否接通、是否成功、不成功原因、考虑车型、备注、操作时间、所属坐席、任务状态、订单ID
            string exportName = string.Empty;
            if (dt != null)
            {
                dt.Columns.Add("是否接通", typeof(string));
                dt.Columns.Add("是否成功", typeof(string));
                //dt.Columns.Add("不成功原因", typeof(string));
                //edit add two field
                dt.Columns.Add("未接通原因", typeof(string));
                dt.Columns.Add("失败原因", typeof(string));

                dt.Columns.Add("性别", typeof(string));
                //dt.Columns.Add("地区", typeof(string));
                dt.Columns.Add("状态", typeof(string));
                dt.Columns.Add("下单日期", typeof(string));

                dt.Columns.Add("是否购车", typeof(string));
                dt.Columns.Add("已购车型", typeof(string));
                dt.Columns.Add("购车时间", typeof(string));
                dt.Columns.Add("购车经销商", typeof(string));

                dt.Columns.Add("购车计划", typeof(string));
                dt.Columns.Add("意向车型", typeof(string));

                dt.Columns.Add("是否关注该品牌", typeof(string));
                dt.Columns.Add("是否有经销商联系", typeof(string));
                dt.Columns.Add("经销商服务是否满意", typeof(string));
       
                if (dt.Rows.Count > 0)
                {
                    exportName = dt.Rows[0]["ProjectName"].ToString();
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string isjt = string.Empty;
                    if (dt.Rows[i]["IsJT"].ToString() == "1")
                    {
                        isjt = "是";
                    }
                    else if (dt.Rows[i]["IsJT"].ToString() == "0")
                    {
                        isjt = "否";
                    }
                    dt.Rows[i]["是否接通"] = isjt;

                    string issucess = string.Empty;
                    if (dt.Rows[i]["IsSuccess"].ToString() == "1")
                    {
                        issucess = "成功";
                    }
                    else if (dt.Rows[i]["IsSuccess"].ToString() == "0")
                    {
                        issucess = "失败";
                    }
                    dt.Rows[i]["是否成功"] = issucess;

                    string notEstablishReasonStr = string.Empty;
                    int _notEstablishReason = -1;

                    if (dt.Rows[i]["IsJT"].ToString() == "0")
                    {
                        //未接通 才有原因
                        if (dt.Rows[i]["NotEstablishReason"] != DBNull.Value)
                        {
                            int.TryParse(dt.Rows[i]["NotEstablishReason"].ToString(), out _notEstablishReason);
                        }
                    }

                    notEstablishReasonStr = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.NotEstablishReason), _notEstablishReason);
                    dt.Rows[i]["未接通原因"] = notEstablishReasonStr;

                    string notSuccessReasonStr = string.Empty;
                    int _notSuccessReason = -1;
                    if (dt.Rows[i]["NotSuccessReason"] != DBNull.Value)
                    {
                        int.TryParse(dt.Rows[i]["NotSuccessReason"].ToString(), out _notSuccessReason);
                    }
                    notSuccessReasonStr = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.NotSuccessReason), _notSuccessReason);
                    dt.Rows[i]["失败原因"] = notSuccessReasonStr;

                    int _sex = 1;
                    if (dt.Rows[i]["sex"] != DBNull.Value)
                    {
                        int.TryParse(dt.Rows[i]["sex"].ToString(), out _sex);
                    }
                    if (_sex == 1)
                    {
                        dt.Rows[i]["性别"] = "男";
                    }
                    else if (_sex == 2)
                    {
                        dt.Rows[i]["性别"] = "女";
                    }
                    //if (dt.Rows[i]["provincename"] != DBNull.Value)
                    //{
                    //    dt.Rows[i]["地区"] = dt.Rows[i]["provincename"].ToString();
                    //}
                    //if (dt.Rows[i]["cityname"] != DBNull.Value)
                    //{
                    //    dt.Rows[i]["地区"] = dt.Rows[i]["地区"] + " " + dt.Rows[i]["cityname"].ToString();
                    //}
                    if (dt.Rows[i]["Status"] != DBNull.Value)
                    {
                        int _status = -2;
                        int.TryParse(dt.Rows[i]["Status"].ToString(), out _status);
                        dt.Rows[i]["状态"] = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.LeadsTaskStatus), _status);
                    }
                    if (dt.Rows[i]["OrderCreateTime"] != DBNull.Value)
                    {
                        DateTime _ordercreatetime;
                        DateTime.TryParse(dt.Rows[i]["OrderCreateTime"].ToString(), out _ordercreatetime);

                        dt.Rows[i]["下单日期"] = _ordercreatetime.ToString("yyyy-MM-dd");
                    }


                    int _isboughtcar = -1;
                    if (dt.Rows[i]["IsBoughtCar"] != DBNull.Value)
                    {
                        int.TryParse(dt.Rows[i]["IsBoughtCar"].ToString(), out _isboughtcar);
                    }
                    if (_isboughtcar == 1)
                    {
                        dt.Rows[i]["是否购车"] = "已购车";
                    }
                    else if (_isboughtcar == 0)
                    {
                        dt.Rows[i]["是否购车"] = "未购车";
                    }
                    else if (_isboughtcar == 2)
                    {
                        dt.Rows[i]["是否购车"] = "未知";
                    }
                    dt.Rows[i]["已购车型"] = dt.Rows[i]["BoughtCarMaster"] + "  " + dt.Rows[i]["BoughtCarSerial"];
                    dt.Rows[i]["购车时间"] = dt.Rows[i]["BoughtCarYearMonth"] == null ? "" : dt.Rows[i]["BoughtCarYearMonth"].ToString().Replace(",", "").Replace("-1", "");
                    dt.Rows[i]["购车经销商"] = dt.Rows[i]["BoughtCarDealerName"];
                    int _hasbuycarplan = -1;
                    if (dt.Rows[i]["HasBuyCarPlan"] != DBNull.Value)
                    {
                        int.TryParse(dt.Rows[i]["HasBuyCarPlan"].ToString(), out _hasbuycarplan);
                    }
                    if (_hasbuycarplan == 1)
                    {
                        dt.Rows[i]["购车计划"] = "有";
                    }
                    else if (_hasbuycarplan == 0)
                    {
                        dt.Rows[i]["购车计划"] = "无";
                    }
                    dt.Rows[i]["意向车型"] = dt.Rows[i]["IntentionCarMaster"] + "  " + dt.Rows[i]["IntentionCarSerial"];

                 
                    string strIsAttention = string.Empty;
                    if (dt.Rows[i]["IsAttention"] != DBNull.Value && dt.Rows[i]["IsAttention"].ToString().ToLower() == "true")
                    {
                        strIsAttention = "是";
                    }
                    else if (dt.Rows[i]["IsAttention"] != DBNull.Value && dt.Rows[i]["IsAttention"].ToString().ToLower() == "false")
                    {
                        strIsAttention = "否";
                    }
                    dt.Rows[i]["是否关注该品牌"] = strIsAttention;

                    string strIsContactedDealer = string.Empty;
                    if (dt.Rows[i]["IsContactedDealer"] != DBNull.Value && dt.Rows[i]["IsContactedDealer"].ToString().ToLower() == "true")
                    {
                        strIsContactedDealer = "是";
                    }
                    else if (dt.Rows[i]["IsContactedDealer"] != DBNull.Value && dt.Rows[i]["IsContactedDealer"].ToString().ToLower() == "false")
                    {
                        strIsContactedDealer = "否";
                    }
                    dt.Rows[i]["是否有经销商联系"] = strIsContactedDealer;

                    string strIsSatisfiedService = string.Empty;
                    if (dt.Rows[i]["IsSatisfiedService"] != DBNull.Value && dt.Rows[i]["IsSatisfiedService"].ToString().ToLower() == "true")
                    {
                        strIsSatisfiedService = "是";
                    }
                    else if (dt.Rows[i]["IsSatisfiedService"] != DBNull.Value && dt.Rows[i]["IsSatisfiedService"].ToString().ToLower() == "false")
                    {
                        strIsSatisfiedService = "否";
                    }
                    dt.Rows[i]["经销商服务是否满意"] = strIsSatisfiedService;
                     
                }




                Dictionary<string, string> exportColums = new Dictionary<string, string>();
                //列名与导出名对应
                exportColums.Add("ProjectName", "项目名称");
                exportColums.Add("UserName", "姓名");
                exportColums.Add("性别", "性别");
                exportColums.Add("Tel", "电话");
                exportColums.Add("LDProvName", "目标下单省份");
                exportColums.Add("LDCityName", "目标下单城市");
                exportColums.Add("DealerName", "下单经销商");
                exportColums.Add("PBuyCarTime2", "预计购车时间");
                exportColums.Add("OrderCarMaster", "下单品牌");
                exportColums.Add("OrderCarSerial", "下单车型");
                exportColums.Add("下单日期", "下单日期");
                exportColums.Add("DCarMaster", "目标品牌");

                exportColums.Add("DCarSerial", "目标车型");
                //***********************
                exportColums.Add("是否购车", "是否购车");
                exportColums.Add("已购车型", "已购车型");
                exportColums.Add("购车时间", "购车时间");
                exportColums.Add("购车经销商", "购车经销商");

                //exportColums.Add("购车计划", "购车计划");                
                exportColums.Add("意向车型", "意向车型");
                exportColums.Add("是否关注该品牌", "是否关注该品牌");
                exportColums.Add("是否有经销商联系", "是否有经销商联系");
                exportColums.Add("经销商服务是否满意", "经销商服务是否满意");
                exportColums.Add("ContactedWhichDealer", "哪家经销商联系");
                //***********************
                exportColums.Add("是否接通", "是否接通");
                exportColums.Add("是否成功", "是否成功");

                exportColums.Add("未接通原因", "未接通原因");
                exportColums.Add("失败原因", "失败原因");

                //exportColums.Add("不成功原因", "失败原因");
                exportColums.Add("ThinkCar", "其他考虑车型");
                exportColums.Add("Remark", "备注");
                exportColums.Add("OrderSource", "订单渠道");
                exportColums.Add("LastUpdateTime", "操作时间");
                exportColums.Add("AssignName", "所属坐席");
                exportColums.Add("状态", "任务状态");
                exportColums.Add("OrderID", "订单ID");
                 
                //设定顺序
                dt.Columns["ProjectName"].SetOrdinal(0);
                dt.Columns["UserName"].SetOrdinal(1);
                dt.Columns["性别"].SetOrdinal(2);
                dt.Columns["Tel"].SetOrdinal(3);
                dt.Columns["LDProvName"].SetOrdinal(4);
                dt.Columns["LDCityName"].SetOrdinal(5);
                dt.Columns["DealerName"].SetOrdinal(6);
                dt.Columns["PBuyCarTime2"].SetOrdinal(7);
                dt.Columns["OrderCarMaster"].SetOrdinal(8);
                dt.Columns["OrderCarSerial"].SetOrdinal(9);
                dt.Columns["下单日期"].SetOrdinal(10);
                dt.Columns["DCarMaster"].SetOrdinal(11);

                dt.Columns["DCarSerial"].SetOrdinal(12);
                //***********************
                dt.Columns["是否购车"].SetOrdinal(13);
                dt.Columns["已购车型"].SetOrdinal(14);
                dt.Columns["购车时间"].SetOrdinal(15);
                dt.Columns["购车经销商"].SetOrdinal(16);

                //dt.Columns["购车计划"].SetOrdinal(17);
                dt.Columns["意向车型"].SetOrdinal(17);

                dt.Columns["是否关注该品牌"].SetOrdinal(18);
                dt.Columns["是否有经销商联系"].SetOrdinal(19);
                dt.Columns["经销商服务是否满意"].SetOrdinal(20);
                dt.Columns["ContactedWhichDealer"].SetOrdinal(21);
                //***********************
                dt.Columns["是否接通"].SetOrdinal(22);
                dt.Columns["是否成功"].SetOrdinal(23);

                dt.Columns["未接通原因"].SetOrdinal(24);
                dt.Columns["失败原因"].SetOrdinal(25);

                //dt.Columns["不成功原因"].SetOrdinal(21);
                dt.Columns["ThinkCar"].SetOrdinal(26);
                dt.Columns["Remark"].SetOrdinal(27);
                dt.Columns["OrderSource"].SetOrdinal(28);
                dt.Columns["OrderID"].SetOrdinal(29);
                dt.Columns["LastUpdateTime"].SetOrdinal(30);
                dt.Columns["AssignName"].SetOrdinal(31);
                dt.Columns["状态"].SetOrdinal(32);




                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (exportColums.ContainsKey(dt.Columns[i].ColumnName))
                    {
                        //字段时要导出的字段，改名
                        dt.Columns[i].ColumnName = exportColums[dt.Columns[i].ColumnName];
                    }
                    else
                    {
                        //不是要导出的字段，删除
                        dt.Columns.RemoveAt(i);
                        i--;
                    }
                }
                #region 导出

                BLL.Util.ExportToCSV(exportName + "的任务" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);

                #endregion
            }
        }
    }
}