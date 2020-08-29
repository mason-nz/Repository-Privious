using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Withdrawals;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class ResourcesExportBll
    {
        /// <summary>
        /// 收入管理导出
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static MemoryStream ExportIncome(QueryWithdrawalsArgs query)
        {

            //封装收入管理表头
            Dictionary<string, string> encapsulateDic = new Dictionary<string, string> {
                { "UserID","用户ID"},
                { "UserName","用户名"},
                { "Mobile","手机号"},
                { "Nickname","微信昵称"},
                { "AccumulatedIncome","累计收益(单位：元)"},
                { "RemainingAmount","账户余额(单位：元)"},
                { "HaveWithdrawals","已提现金额(单位：元)"},
                { "WithdrawalsProcess","提现中金额(单位：元)"} };
            var ResultQuery = WithdrawalsBll.Instance.GetWithdrawalsStatisticsList(query);
            return ExcelHelper.ResourcesExport(ToDataTableTow((IList)ResultQuery.List), encapsulateDic);
        }
        /// <summary>
        /// 收入明细管理导出
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static MemoryStream ExportIncomeDetail(QueryWithdrawalsArgs query)
        {
            //封装明细列表表头
            Dictionary<string, string> encapsulateDic = new Dictionary<string, string> {
                { "IncomeTime","收入时间"},
                { "UserName","用户名"},
                { "Mobile","手机号"},
                { "Nickname","微信昵称"},
                { "CategoryName","收益类型"},
                { "DetailDescription","收益详情"},
                { "IncomePrice","金额"} };

            var ResultQuery = WithdrawalsBll.Instance.GetIncomeDetailModelList(query);
            return ExcelHelper.ResourcesExport(ToDataTableTow((IList)ResultQuery.List), encapsulateDic);
        }
        /// <summary>
        /// 媒体主、广告主
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static MemoryStream ExportMediaUser(UserQueryArgs query)
        {
            //封装媒体主列表表头
            Dictionary<string, string> encapsulateDic = new Dictionary<string, string> {
                { "UserID","用户ID"},
                { "UserName","用户名"},
                { "Nickname","昵称"},
                { "Mobile","手机号"},
                { "RegisterFromName","注册来源"},
                { "RegisterTypeName","注册方式"},
                { "CreateTime","注册日期"},
                { "StatusName","状态"},
                { "ApproveStatusName","认证状态"},
                { "AttentionName","关注状态"} };
            if (query.ApproveStatus == 1 || query.ApproveStatus == -2)
            {
                encapsulateDic.Add("ApplyTime", "申请认证日期");
            }
            if (query.ApproveStatus == 2)
            {
                encapsulateDic.Add("AuditTime", "审核日期");
            }
            if (query.ApproveStatus == 3)
            {
                encapsulateDic.Add("AuditTime", "审核日期");
                encapsulateDic.Add("Reason", "原因");
            }
            var ResultQuery = MediaUserBll.Instance.GetMediaUserList(query);
            return ExcelHelper.ResourcesExport(ToDataTableTow((IList)ResultQuery.List), encapsulateDic);
        }
        /// <summary>
        /// 提现管理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static MemoryStream ExportPresentManange(ReqWithdrawalsDto query)
        {
            Dictionary<string, string> encapsulateDic = null;
            if (query.AuditStatus == 197003)
            {
                encapsulateDic = new Dictionary<string, string> {
                { "Id","申请ID"},
                { "UserName","申请人"},
                { "UserTypeName","类型"},
                { "ApplicationDate","申请时间"},
                { "WithdrawalsPrice","申请金额"},
                { "IndividualTaxPeice","代扣个税"},
                { "PracticalPrice","实付"},
                { "PayeeAccount","提现账号"} };
            }
            if (query.AuditStatus == 197001)
            {
                encapsulateDic = new Dictionary<string, string> {
                { "Id","申请ID"},
                { "UserName","申请人"},
                { "TrueName","真是姓名/公司名称"},
                { "ApplicationDate","申请时间"},
                { "WithdrawalsPrice","申请金额"},
                { "PracticalPrice","实付"},
                { "PayStatusName","支付结果"},
                { "PayDate","实际支付时间"} };
            }
            if (query.AuditStatus == 197002)
            {
                encapsulateDic = new Dictionary<string, string> {
                { "Id","申请ID"},
                { "UserName","申请人"},
                { "TrueName","真是姓名/公司名称"},
                { "ApplicationDate","申请时间"},
                { "WithdrawalsPrice","申请金额"},
                { "PracticalPrice","实付"},
                { "PayStatusName","支付结果"},
                { "AuditTime","审核时间"},
                { "Reason","驳回原因"} };
            }
            var present = new WithdrawalsQuery(new ConfigEntity()).GetQueryList(query);
            return ExcelHelper.ResourcesExport(ToDataTableTow(present.List), encapsulateDic);
        }
        private static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name);
                }
                foreach (object t in list)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(t, null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }
}
