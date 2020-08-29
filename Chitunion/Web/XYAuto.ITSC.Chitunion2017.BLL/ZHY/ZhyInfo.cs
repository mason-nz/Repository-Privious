using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2;
using XYAuto.ITSC.Chitunion2017.Entities.GDT;
using XYAuto.ITSC.Chitunion2017.Entities.ZHY;

namespace XYAuto.ITSC.Chitunion2017.BLL.ZHY
{
    public class ZhyInfo
    {
        public static readonly ZhyInfo Instance = new ZhyInfo();
        /// <summary>
        /// zlb 2017-08-18
        /// 查询广告主/子客列表
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectZhyAdvertiserList(string Mobile, int PageIndex, int PageSize)
        {
            if (PageSize > Util.PageSize)
            {
                PageSize = Util.PageSize;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);
            dic.Add("TotalCashBalance", 0);
            dic.Add("TotalVirtualBalance", 0);
            dic.Add("TotalDividedBalance", 0);
            dic.Add("TotalSilverCBalance", 0);
            dic.Add("TotalTodaySpend", 0);
            dic.Add("TotalNumber", 0);
            List<ZhyAdvertiserInfo> listInfo = new List<ZhyAdvertiserInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            int userId = 0;
            if (!roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00004"))
            {
                if (roleIdList.Contains("SYS001RL00011"))
                {
                    userId = Common.UserInfo.GetLoginUserID();
                }
                else
                {
                    dic.Add("AdvertiserInfo", listInfo);
                    return dic;
                }
            }
            //DataTable dtStatics = Dal.ZHY.ZhyInfo.Instance.ZhyAdvertiserStatistics(Mobile, userId);
            //if (dtStatics != null && dtStatics.Rows.Count > 0)
            //{
            //    DataRow dr = dtStatics.Rows[0];
            //    dic["TotalCashBalance"] = Convert.ToDecimal(dr["TotalCashBalance"].ToString() == "" ? 0 : dr["TotalCashBalance"]);
            //    dic["TotalVirtualBalance"] = Convert.ToDecimal(dr["TotalVirtualBalance"].ToString() == "" ? 0 : dr["TotalVirtualBalance"]);
            //    dic["TotalDividedBalance"] = Convert.ToDecimal(dr["TotalDividedBalance"].ToString() == "" ? 0 : dr["TotalDividedBalance"]);
            //    dic["TotalSilverCBalance"] = Convert.ToDecimal(dr["TotalSilverCBalance"].ToString() == "" ? 0 : dr["TotalSilverCBalance"]);
            //    dic["TotalTodaySpend"] = Convert.ToDecimal(dr["TotalTodaySpend"].ToString() == "" ? 0 : dr["TotalTodaySpend"]);
            //    dic["TotalNumber"] = Convert.ToInt32(dr["TotalNumber"].ToString() == "" ? 0 : dr["TotalNumber"]);
            //}
            DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectZhyAdvertiserList(Mobile, PageIndex, PageSize, userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                listInfo = Common.Util.DataTableToList<ZhyAdvertiserInfo>(dt);
                dic["TotalCashBalance"] = listInfo.Sum(T => T.CashBalance);
                dic["TotalVirtualBalance"] = listInfo.Sum(T => T.VirtualBalance);
                dic["TotalDividedBalance"] = listInfo.Sum(T => T.DividedBalance);
                dic["TotalSilverCBalance"] = listInfo.Sum(T => T.SilverCardBalance);
                dic["TotalTodaySpend"] = listInfo.Sum(T => T.TodaySpend);
                dic["TotalNumber"] = listInfo.Sum(T => T.Number);
            }
            dic.Add("AdvertiserInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-18
        /// 查询广告运营角色列表
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Mobile">手机号</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectZhyOperaterList(string UserName, string Mobile, int PageIndex, int PageSize)
        {
            if (PageSize > Util.PageSize)
            {
                PageSize = Util.PageSize;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);
            List<ZhyOperaterInfo> listInfo = new List<ZhyOperaterInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00004"))
            {
                DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectZhyOperaterList(UserName, Mobile, PageIndex, PageSize);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                    listInfo = Common.Util.DataTableToList<ZhyOperaterInfo>(dt);
                }
            }
            dic.Add("OperaterInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-18
        /// 查询未绑定子客的广告主和广告运营用户
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> SelectGdtUserAndOperaterList()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<ZhyOperaterInfo> listInfo = new List<ZhyOperaterInfo>();
            List<int> listAccountId = new List<int>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00004"))
            {
                DataSet ds = Dal.ZHY.ZhyInfo.Instance.SelectGdtUserAndOperaterList();
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listAccountId.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString()));
                    }
                }
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    listInfo = Common.Util.DataTableToList<ZhyOperaterInfo>(ds.Tables[1]);
                }
            }
            dic.Add("AccountInfo", listAccountId);
            dic.Add("OperaterInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-18
        /// 查询未绑定运营的广告主信息集合
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> SelectFreeAdvertiserList()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<ZhyOperaterInfo> listInfo = new List<ZhyOperaterInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00004"))
            {
                DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectFreeAdvertiserList();

                if (dt != null && dt.Rows.Count > 0)
                {
                    listInfo = Common.Util.DataTableToList<ZhyOperaterInfo>(dt);
                }
            }
            dic.Add("AdvertiserInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-22
        ///查询广告运营所管理的广告主
        /// </summary>
        /// <param name="OperaterId">广告运营ID</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectOptAdvertiserList(int OperaterId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<ZhyOperaterInfo> listInfo = new List<ZhyOperaterInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00004"))
            {
                DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectOptAdvertiserList(OperaterId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    listInfo = Common.Util.DataTableToList<ZhyOperaterInfo>(dt);
                }
            }
            dic.Add("AdvertiserInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-18
        ///子客和广告主绑定
        /// </summary>
        /// <param name="ReqDto"></param>
        /// <returns></returns>
        public string BingdingOptIdAndAccIdToAdvId(BindingIdReqDTO ReqDto)
        {
            if (ReqDto == null || ReqDto.AccountId <= 0 || ReqDto.AdvertiserId <= 0)
            {
                return "参数错误";
            }
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!(roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00004")))
            {
                return "您无此操作权限";
            }
            int AdvCount = Dal.ZHY.ZhyInfo.Instance.SelectBingdingAdvCount(ReqDto.AdvertiserId);
            if (AdvCount > 0)
            {
                return "广告主已被绑定";
            }
            int AccCount = Dal.ZHY.ZhyInfo.Instance.SelectBingdingAccountCount(ReqDto.AccountId);
            if (AccCount > 0)
            {
                return "子客账号已被绑定";
            }
            int userId = Common.UserInfo.GetLoginUserID();
            int result = Dal.ZHY.ZhyInfo.Instance.BingdingOptIdAndAccIdToAdvId(ReqDto, userId);
            if (result <= 0)
            {
                return "绑定失败，请重试";
            }
            GdtLogicProvider P = new GdtLogicProvider();
            P.DoPullGetFundsInfo(ReqDto.AccountId);
            return "";
        }
        /// <summary>
        /// zlb 2017-08-18
        /// 运营批量绑定广告主
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <returns></returns>
        public string BingdingOptIdToAdvsId(AdvIdToOptIdReqDTO ReqDto)
        {
            if (ReqDto == null || ReqDto.OperaterId <= 0 || (ReqDto.OperateType != 1 && ReqDto.OperateType != 2))
            {
                return "参数错误";
            }
            StringBuilder sb = new StringBuilder();
            if (ReqDto.AdvertiserIds == null)
            {
                ReqDto.AdvertiserIds = new List<int>();
            }
            foreach (var item in ReqDto.AdvertiserIds)
            {
                if (item <= 0)
                {
                    return "参数错误";
                }
                sb.Append(item + ",");
            }
            string strAdvIds = sb.ToString();
            if (strAdvIds != "")
            {
                strAdvIds = strAdvIds.Remove(strAdvIds.LastIndexOf(","), 1);
            }
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!(roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00004")))
            {
                return "您无此操作权限";
            }
            if (ReqDto.OperateType == 2 && strAdvIds != "")
            {
                int AdvCount = Dal.ZHY.ZhyInfo.Instance.SelectBingdingAdvCount(ReqDto.OperaterId, strAdvIds);
                if (AdvCount > 0)
                {
                    return "广告主已被绑定，请重新选择";
                }
            }
            int userId = Common.UserInfo.GetLoginUserID();
            int result = Dal.ZHY.ZhyInfo.Instance.BingdingOptIdToAdvsId(ReqDto, userId, strAdvIds);
            if (result <= 0 && ReqDto.OperateType == 2 && strAdvIds != "")
            {
                return "绑定失败，请重试";
            }
            return "";
        }
        /// <summary>
        /// zlb 2017-08-18
        /// 解除广告主与子客的绑定
        /// </summary>
        /// <param name="ReqDto">广告主ID</param>
        /// <returns></returns>
        public string DeleteAdvsiterByAdvId(AdvertiserIdReqDTO ReqDto)
        {
            if (ReqDto == null || ReqDto.AdvertiserId <= 0)
            {
                return "参数错误";
            }
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!(roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00004")))
            {
                return "您无此操作权限";
            }
            int DemandCount = Dal.ZHY.ZhyInfo.Instance.SelectDemandCount(ReqDto.AdvertiserId);
            if (DemandCount > 0)
            {
                return "您还有需求未处理";
            }
            int result = Dal.ZHY.ZhyInfo.Instance.DeleteAdvsiterByAdvId(ReqDto.AdvertiserId);
            if (result <= 0)
            {
                return "操作失败，请重试";
            }
            return "";
        }
        /// <summary>
        /// zlb 2017-08-21
        /// 查询流水明细
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="GdtNum"></param>
        /// <param name="AccountType"></param>
        /// <param name="TradeType"></param>
        /// <param name="StarDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectGdtFlowDetail(string UserName, string GdtNum, int AccountType, int TradeType, string StarDate, string EndDate, int PageIndex, int PageSize)
        {
            if (PageSize > Util.PageSize)
            {
                PageSize = Util.PageSize;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);
            List<GdtDetailedInfo> listInfo = new List<GdtDetailedInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            int userId = 0;
            if (!roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00004"))
            {
                if (roleIdList.Contains("SYS001RL00011"))
                {
                    userId = Common.UserInfo.GetLoginUserID();
                }
                else
                {
                    dic.Add("GdtDetailedInfo", listInfo);
                    return dic;
                }
            }
            DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectGdtFlowDetail(UserName, GdtNum, AccountType, TradeType, StarDate, EndDate, userId, PageIndex, PageSize);
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                listInfo = Common.Util.DataTableToList<GdtDetailedInfo>(dt);
            }
            dic.Add("GdtDetailedInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-21
        /// 查询日结汇总
        /// </summary>
        /// <param name="AccountType"></param>
        /// <param name="TradeType"></param>
        /// <param name="StarDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectGdtDateSummaryInfo(int AccountType, int TradeType, string StarDate, string EndDate, int PageIndex, int PageSize)
        {
            if (PageSize > Util.PageSize)
            {
                PageSize = Util.PageSize;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);
            List<GdtDetailedInfo> listInfo = new List<GdtDetailedInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            int userId = 0;
            if (!roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00004"))
            {
                if (roleIdList.Contains("SYS001RL00011"))
                {
                    userId = Common.UserInfo.GetLoginUserID();
                }
                else
                {
                    dic.Add("DateSummaryInfo", listInfo);
                    return dic;
                }
            }
            DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectGdtDateSummaryInfo(AccountType, TradeType, StarDate, EndDate, userId, PageIndex, PageSize);
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                listInfo = Common.Util.DataTableToList<GdtDetailedInfo>(dt);
            }
            dic.Add("DateSummaryInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-21
        /// 查询充值列表
        /// </summary>
        /// <param name="RechargeNumber"></param>
        /// <param name="DemandBillNo"></param>
        /// <param name="ExternalBillNo"></param>
        /// <param name="UserName"></param>
        /// <param name="RechargeStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectGdtRechargeInfo(string RechargeNumber, string DemandBillNo, string ExternalBillNo, string UserName, int RechargeStatus, int PageIndex, int PageSize)
        {
            if (PageSize > Util.PageSize)
            {
                PageSize = Util.PageSize;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);
            List<RechargeInfo> listInfo = new List<RechargeInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00004"))
            {
                dic.Add("GdtRechargeInfo", listInfo);
                return dic;
            }
            DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectGdtRechargeInfo(RechargeNumber, DemandBillNo, ExternalBillNo, UserName, RechargeStatus, PageIndex, PageSize);
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                listInfo = Common.Util.DataTableToList<RechargeInfo>(dt);
            }
            dic.Add("GdtRechargeInfo", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-08-21
        /// 查询未关联的GDT流水单列表
        /// </summary>
        /// <param name="TradeType">84001:充值，84003回划 </param>
        /// <returns></returns>
        public Dictionary<string, object> SelectGdtInfo(int TradeType, int DemandBillNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<GdtTradeInfo> listInfo = new List<GdtTradeInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00004"))
            {
                dic.Add("GdtInfo", listInfo);
                return dic;
            }
            DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectGdtInfo(TradeType, DemandBillNo);
            if (dt != null && dt.Rows.Count > 0)
            {
                listInfo = Common.Util.DataTableToList<GdtTradeInfo>(dt);
            }
            dic.Add("GdtInfo", listInfo);
            return dic;
        }
        /// <summary>
        ///  zlb 2017-08-21
        ///  充值对账或资金回划绑定GDT流水单 
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <returns></returns>
        public string BingdingTradeRelation(GdtTradeReqDTO ReqDTO)
        {
            if (ReqDTO == null || ReqDTO.GdtNumList == null || ReqDTO.GdtNumList.Count <= 0 || (ReqDTO.TradeType != 84001 && ReqDTO.TradeType != 84003) || ReqDTO.DemandBillNo <= 0)
            {
                return "参数错误";
            }
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00004"))
            {
                return "您无此操作权限";
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in ReqDTO.GdtNumList)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    return "参数错误";
                }
                sb.Append("'" + item + "',");
            }
            string ExternalBillNos = sb.ToString();
            ExternalBillNos = ExternalBillNos.Remove(ExternalBillNos.LastIndexOf(","), 1);

            decimal GdtRechargeAmount = Dal.ZHY.ZhyInfo.Instance.SelectGdtRechargeAmount(ExternalBillNos);

            decimal ZhyRechargeAmount = Dal.ZHY.ZhyInfo.Instance.SelectZhyRechargeAmount(ReqDTO.DemandBillNo);
            decimal Amount = 0;
            if (ReqDTO.TradeType == 84001)
            {
                Amount = ZhyRechargeAmount;
            }
            else
            {
                decimal ZhyConsumeAmount = Dal.ZHY.ZhyInfo.Instance.SelectZhyConsumeAmount(ReqDTO.DemandBillNo);
                Amount = ZhyRechargeAmount - ZhyConsumeAmount;
            }

            if (!(Amount > 0 && GdtRechargeAmount == Amount))
            {
                return "金额必须相等";
            }
            int TradeCount = Dal.ZHY.ZhyInfo.Instance.SelectBingdingTradeCount(ExternalBillNos);
            if (TradeCount > 0)
            {
                return "流水单已被绑定，请重新选择";
            }
            TradeCount = Dal.ZHY.ZhyInfo.Instance.SelectBingdingTradeCount(ReqDTO.DemandBillNo, ReqDTO.TradeType);
            if (TradeCount > 0)
            {
                return "需求单已被绑定，请重新选择";
            }
            int organizeId = Dal.ZHY.ZhyInfo.Instance.SelectOrganizeId(ReqDTO.DemandBillNo);
            if (organizeId <= 0)
            {
                return "需求单号错误";
            }
            DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectZhyTradeAmount(ExternalBillNos);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return "流水单号错误";
            }
            DataRow[] drArr = dt.Select($"ExternalBillNo in ({ExternalBillNos})");
            if (drArr.Count() != ReqDTO.GdtNumList.Count)
            {
                return "流水单号错误";
            }
            if (ReqDTO.TradeType == 84003)
            {
                LogicByZhyProvider lbzp = new LogicByZhyProvider();
                foreach (var item in ReqDTO.GdtNumList)
                {
                    ToAccountFundNotes tfnModel = new ToAccountFundNotes();
                    tfnModel.DemandBillNo = ReqDTO.DemandBillNo;
                    tfnModel.OrganizeId = organizeId;
                    DataRow[] drArrItem = dt.Select($"ExternalBillNo='{item}'");//查询
                    tfnModel.TradeNo = drArrItem[0]["ExternalBillNo"].ToString();
                    tfnModel.TradeMoney = Convert.ToDecimal(drArrItem[0]["Amount"]);
                    tfnModel.MoneyTpe = ZhyEnum.ZhyMoneyTpeEnum.现金;
                    tfnModel.TradeType = (ZhyEnum.ZhyTradeTypeEnum)(ReqDTO.TradeType == 84001 ? 1 : 3);
                    ReturnValue zhyResult = lbzp.AccountFundsNote(tfnModel);
                    if (zhyResult.HasError)
                    {
                        return zhyResult.Message;
                    }
                }
            }
            int userId = Common.UserInfo.GetLoginUserID();
            int result = Dal.ZHY.ZhyInfo.Instance.BingdingTradeRelation(ReqDTO, userId);
            if (result <= 0)
            {
                return "操作失败，请重试";
            }
            return "";

        }
        /// <summary>
        /// zlb 2017-08-21
        /// 查询资金回划列表
        /// </summary>
        /// <param name="RechargeNumber"></param>
        /// <param name="DemandBillNo"></param>
        /// <param name="ExternalBillNo"></param>
        /// <param name="UserName"></param>
        /// <param name="HandleStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectBackAmountInfo(string RechargeNumber, string DemandBillNo, string ExternalBillNo, string UserName, int HandleStatus, int PageIndex, int PageSize)
        {
            if (PageSize > Util.PageSize)
            {
                PageSize = Util.PageSize;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);
            List<BackAmountInfo> listInfo = new List<BackAmountInfo>();
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!roleIdList.Contains("SYS001RL00001") && !roleIdList.Contains("SYS001RL00004"))
            {
                dic.Add("BackAmountInfo", listInfo);
                return dic;
            }
            DataTable dt = Dal.ZHY.ZhyInfo.Instance.SelectBackAmountInfo(RechargeNumber, DemandBillNo, ExternalBillNo, UserName, HandleStatus, PageIndex, PageSize);
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                listInfo = Common.Util.DataTableToList<BackAmountInfo>(dt);
            }
            dic.Add("BackAmountInfo", listInfo);
            return dic;
        }
    }
}
