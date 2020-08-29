using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Constants;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Extend.LE;
using XYAuto.ChiTu2018.Entities.Query;
using XYAuto.ChiTu2018.Infrastructure;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Infrastructure.Exceptions;
using XYAuto.ChiTu2018.Infrastructure.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo.VerifyEntity;
using XYAuto.ChiTu2018.Service.App.Query.Dto.Request;
using XYAuto.ChiTu2018.Service.App.Query.Dto.Response;

namespace XYAuto.ChiTu2018.Service.App.AppInfo
{
    /// <summary>
    /// 注释：LeWithdrawalsDetailService
    /// 作者：lix
    /// 日期：2018/5/23 17:54:13
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeWithdrawalsDetailService : VerifyOperateBase
    {
        #region 单例

        private LeWithdrawalsDetailService() { }
        private static readonly Lazy<LeWithdrawalsDetailService> Linstance = new Lazy<LeWithdrawalsDetailService>(() => { return new LeWithdrawalsDetailService(); });

        public static LeWithdrawalsDetailService Instance => Linstance.Value;

        #endregion

        public LE_WithdrawalsDetail GetById(int recId)
        {
            return new LeWithdrawalsDetailBO().GetById(recId);
        }

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

        #region 分页

        public Query.Dto.BaseResponseEntity<RespWithdrawalsDto> GetIncomeWithdrawalsQuery(ReqInComeDto query)
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

            var resp = new Query.Dto.BaseResponseEntity<RespWithdrawalsDto>() { List = queryDo.DataList.MapToList<LeWithdrawalsIncomeDo, RespWithdrawalsDto>() };
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

        public ReturnValue PostWithdrawasTest()
        {
            try
            {
                var m = 9;
                var a = m / 0;
            }
            catch (Exception exception)
            {
                throw new PostWithdrawasException($"PostWithdrawas 提现申请操作异常错误.{exception.Message}" +
                                               $"{exception.StackTrace ?? string.Empty}");

            }
            return new ReturnValue();
        }

        public Tuple<int, bool> PostWithdrawas(LE_WithdrawalsDetail entity)
        {

            return new LeWithdrawalsDetailBO().PostWithdrawas(entity);
        }

    }
}
