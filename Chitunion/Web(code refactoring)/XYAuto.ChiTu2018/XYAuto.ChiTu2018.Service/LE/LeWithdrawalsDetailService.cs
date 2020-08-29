/**
*
*创建人：lixiong
*创建时间：2018/5/9 10:28:00
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Constants;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Extend.LE;
using XYAuto.ChiTu2018.Entities.Query;
using XYAuto.ChiTu2018.Infrastructure;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Infrastructure.LeTask;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Response.Withdrawals;
using XYAuto.ChiTu2018.Service.LE.Query.Dto;
using XYAuto.ChiTu2018.Service.LE.Query.Dto.Request;
using XYAuto.ChiTu2018.Service.LE.Query.Dto.Response;
using XYAuto.ChiTu2018.Service.LE.VerifyEntity;
using static System.Data.Entity.Core.Objects.EntityFunctions;

namespace XYAuto.ChiTu2018.Service.LE
{
    public class LeWithdrawalsDetailService
    {
        #region 单例

        private LeWithdrawalsDetailService() { }
        private static readonly Lazy<LeWithdrawalsDetailService> Linstance = new Lazy<LeWithdrawalsDetailService>(() => { return new LeWithdrawalsDetailService(); });

        public static LeWithdrawalsDetailService Instance => Linstance.Value;

        #endregion

        #region 基础测试

        //public List<LE_WithdrawalsDetail> GetJoinQuerys()
        //{
        //    return new LeWithdrawalsDetailBO().GetQuerys();
        //}

        public LE_WithdrawalsDetail GetById(int recId)
        {
            return new LeWithdrawalsDetailBO().GetById(recId);
        }

        public void UpdateStatus(int recId)
        {
            new LeWithdrawalsDetailBO().UpdateStatus(recId);
        }

        //public List<LE_WithdrawalsDetail> GetPageList(QueryPageBase<LE_WithdrawalsDetail> query)
        //{
        //    return new LeWithdrawalsDetailBO().GetPageList(query);
        //}

        #endregion

        /// <summary>
        /// 提现详情
        /// </summary>
        /// <param name="withdrawalsId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetWithdrawalsInfo(int withdrawalsId, int userId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var info =
                new LeWithdrawalsDetailBO().GetWithdrawalsDetailInfo(withdrawalsId, userId)
                    .MapTo<RespWithdrawalsInfoDto>();
            dic.Add("WithdrawalsInfo", SetReason(info));
            return dic;
        }

        /// <summary>
        /// 处理早期支付失败的错误消息（现在前台用户看见的是json）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private RespWithdrawalsInfoDto SetReason(RespWithdrawalsInfoDto info)
        {
            if (info != null)
            {
                info.Reason = new KrErrorMessageProvider().GetKrBaseDto(info.Reason).ErrorMessage;
                return info;
            }
            return null;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<RespWithdrawalsDto> GetList(ReqInComeDto query)
        {
            var queryDo = new GetPageBase<LE_WithdrawalsDetail, int>()
            {
                Expression = GetIncomeWithdrawalsQueryExtendExpression(query),
                Order = s => s.RecID,
                SortOrder = SortOrder.Descending,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize
            };
            var list = new LeWithdrawalsDetailBO().GetList(queryDo);
            return list.MapToList<RespWithdrawalsDto>();
        }


        /// <summary>
        /// 获取结算的总金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="statusEnum"></param>
        /// <returns></returns>
        public decimal GetSettlement(string startDate, string endDate, WithdrawalsStatusEnum statusEnum)
        {
            return new LeWithdrawalsDetailBO().GetSettlement(startDate, endDate, statusEnum);
        }

        #region 分页

        public BaseResponseEntity<RespWithdrawalsDto> GetIncomeWithdrawalsQuery(ReqInComeDto query)
        {
            var leBo = new LeWithdrawalsDetailBO();
            var queryDo = new GetPageBase<LeWithdrawalsIncomeDo, int>()
            {
                Expression = GetIncomeWithdrawalsQueryExpression(query),
                Order = s => s.Id,
                SortOrder = SortOrder.Descending,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize
            };
            leBo.GetIncomeWithdrawalsQuery(queryDo);

            var resp = new BaseResponseEntity<RespWithdrawalsDto>() { List = queryDo.DataList.MapToList<LeWithdrawalsIncomeDo, RespWithdrawalsDto>() };
            if (!queryDo.DataList.Any())
            {
                resp.Extend = new { TotalMoney = 0m };
                return resp;
            }

            resp.Extend = leBo.GetTotalAmount(GetIncomeWithdrawalsQueryExtendExpression(query));
            return resp;
        }

        /// <summary>
        /// GetIncomeWithdrawalsQuery Expression
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private Expression<Func<LeWithdrawalsIncomeDo, bool>> GetIncomeWithdrawalsQueryExpression(ReqInComeDto query)
        {
            Expression<Func<LeWithdrawalsIncomeDo, bool>> expression = PredicateBuilder.True<LeWithdrawalsIncomeDo>();
            if (query.UserId != Constant.INT_INVALID_VALUE)
            {
                expression = expression.And(s => s.PayeeId == query.UserId);
            }
            if (query.PayType != Constant.INT_INVALID_VALUE)
            {
                expression = expression.And(s => s.PayStatus == query.PayType);
            }
            if (!string.IsNullOrWhiteSpace(query.StartDate))
            {
                var endTime = Convert.ToDateTime(Convert.ToDateTime(query.EndDate).AddDays(1).ToString("yyyy-MM-dd"));
                var startDate = Convert.ToDateTime(query.StartDate);
                expression = expression.And(s => s.ApplicationDate >= startDate).And(s => s.ApplicationDate < endTime);
            }

            return expression;
        }

        /// <summary>
        /// GetIncomeWithdrawalsQueryExtend TotalMoney Expression
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private Expression<Func<LE_WithdrawalsDetail, bool>> GetIncomeWithdrawalsQueryExtendExpression(ReqInComeDto query)
        {
            Expression<Func<LE_WithdrawalsDetail, bool>> expression = PredicateBuilder.True<LE_WithdrawalsDetail>();

            if (query.UserId != Constant.INT_INVALID_VALUE)
            {
                expression = expression.And(s => s.PayeeID == query.UserId);
            }
            if (query.PayType != Constant.INT_INVALID_VALUE)
            {
                expression = expression.And(s => s.Status == query.PayType);
            }
            if (!string.IsNullOrWhiteSpace(query.StartDate))
            {
                var endTime = Convert.ToDateTime(Convert.ToDateTime(query.EndDate).AddDays(1).ToString("yyyy-MM-dd"));
                var startDate = Convert.ToDateTime(query.StartDate);
                expression = expression.And(s => s.ApplicationDate >= startDate).And(s => s.ApplicationDate < endTime);
            }
            if (!string.IsNullOrWhiteSpace(query.PayStartDate))
            {
                var startDate = Convert.ToDateTime(query.PayStartDate);
                expression = expression.And(s => s.PayDate >= startDate);
            }
            if (!string.IsNullOrWhiteSpace(query.PayEndDate))
            {
                var endTime = Convert.ToDateTime(Convert.ToDateTime(query.PayEndDate).AddDays(1).ToString("yyyy-MM-dd"));
                expression = expression.And(s => s.PayDate < endTime);
            }

            return expression;
        }

        #endregion
    }
}
