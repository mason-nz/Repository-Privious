using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.ExcelOperation
{
    public partial class ExportResources : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string ExportType = Request["Export"] + string.Empty;

                MemoryStream memorystream = new MemoryStream();
                string FileName = string.Empty;
                switch (ExportType.ToLower())
                {
                    case "mediauser":
                        memorystream = ExportMediaUserInfo();
                        FileName = "媒体主数据";
                        break;
                    case "advertisementuser":
                        memorystream = ExportMediaUserInfo();
                        FileName = "广告主数据";
                        break;
                    case "income":
                        memorystream = ExportIncomeInfo();
                        FileName = "收入管理数据";
                        break;
                    case "incomedetail":
                        memorystream = ExportIncomeDetail();
                        FileName = "收入明细数据";
                        break;
                    case "presentmanage":
                        memorystream = ExportPresentManage();
                        FileName = "提现管理数据";
                        break;
                }
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8)));
                Response.BinaryWrite(memorystream.ToArray());
                
                memorystream.Close();
                memorystream.Dispose();
                Response.End();
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("导出报错", ex);
            }

        }
        /// <summary>
        /// 导出媒体主、广告主
        /// </summary>
        /// <returns></returns>
        private MemoryStream ExportMediaUserInfo()
        {
            string userName = Request["UserName"] + string.Empty; //用户名
            string listType = Request["ListType"] + string.Empty;//用户类型
            string attentionStatus = Request["AttentionStatus"] + string.Empty;//关注状态
            string mobile = Request["Mobile"] + string.Empty;//手机号
            string registerFrom = Request["RegisterFrom"] + string.Empty;// 账号来源
            string registerType = Request["RegisterType"] + string.Empty;//账号注册方式
            string status = Request["Status"] + string.Empty;//状态
            string approveStatus = Request["ApproveStatus"] + string.Empty;//认证状态
            string beginTime = Request["BeginTime"] + string.Empty;//开始时间
            string endTime = Request["EndTime"] + string.Empty;//结束时间
            string level1 = Request["Level1"] + string.Empty;//一级渠道
            return ResourcesExportBll.ExportMediaUser(new UserQueryArgs
            {
                PageIndex = 0,
                PageSize = 0,
                BeginTime = beginTime,
                EndTime = endTime,
                UserName = userName,
                ListType = listType,
                AttentionStatus = ConvertHelper.ToInt32(attentionStatus),
                Mobile = mobile,
                RegisterFrom = ConvertHelper.ToInt32(registerFrom),
                RegisterType = ConvertHelper.ToInt32(registerType),
                Status = ConvertHelper.ToInt32(status),
                ApproveStatus = ConvertHelper.ToInt32(approveStatus),
                Level1= ConvertHelper.ToLong(level1)
            });
        }
        /// <summary>
        /// 导出收入管理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private MemoryStream ExportIncomeInfo()
        {
            string keyword = Request["Keyword"] + string.Empty; //关键字
            string orderBy = Request["OrderBy"] + string.Empty;//排序类型
            return ResourcesExportBll.ExportIncome(new QueryWithdrawalsArgs
            {
                PageIndex = 0,
                PageSize = 0,
                OrderBy = ConvertHelper.ToInt32(orderBy),
                Keyword = keyword
            });
        }

        /// <summary>
        /// 导出收入管理明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private MemoryStream ExportIncomeDetail()
        {
            string keyword = Request["Keyword"] + string.Empty; //关键字
            string userID = Request["UserID"] + string.Empty;//用户ID
            string incomeBeginTime = Request["IncomeBeginTime"] + string.Empty;//用户开始时间
            string incomeEndTime = Request["IncomeEndTime"] + string.Empty;//用户结束时间
            string categoryID = Request["CategoryID"] + string.Empty;//收益类型

            return ResourcesExportBll.ExportIncomeDetail(new QueryWithdrawalsArgs
            {
                PageIndex = 0,
                PageSize = 0,
                UserID = ConvertHelper.ToInt32(userID),
                IncomeBeginTime = incomeBeginTime,
                IncomeEndTime = incomeEndTime,
                CategoryID = ConvertHelper.ToInt32(categoryID),
                Keyword = keyword
            });
        }

        /// <summary>
        /// 导出提现管理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private MemoryStream ExportPresentManage()
        {
            string username = Request["UserName"] + string.Empty; //用户名
            string auditstatus = Request["AuditStatus"] + string.Empty;//审核状态
            string beginpaydate = Request["BeginPayDate"] + string.Empty;//支付开始时间
            string endpaydate = Request["EndPayDate"] + string.Empty;//支付结束时间
            string orderstatus = Request["OrderStatus"] + string.Empty;//订单状态
            string startdate = Request["StartDate"] + string.Empty;//申请开始时间
            string enddate = Request["EndDate"] + string.Empty;//申请结束时间

            return ResourcesExportBll.ExportPresentManange(new ReqWithdrawalsDto
            {
                PageIndex = 0,
                PageSize = 0,
                UserName = username,
                AuditStatus = ConvertHelper.ToInt32(auditstatus),
                BeginPayDate = beginpaydate,
                EndPayDate = endpaydate,
                StartDate = startdate,
                EndDate = endpaydate,
                OrderStatus = ConvertHelper.ToInt32(orderstatus)

            });
        }
    }
}