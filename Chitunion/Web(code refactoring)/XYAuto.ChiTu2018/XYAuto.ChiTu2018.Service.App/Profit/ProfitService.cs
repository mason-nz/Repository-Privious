using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.Profit;
using XYAuto.ChiTu2018.Entities.Enum.Profit;
using XYAuto.ChiTu2018.Entities.Extend.Profit;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Service.App.Profit.Dto;

namespace XYAuto.ChiTu2018.Service.App.Profit
{
    /// <summary>
    /// 注释：ProfitService
    /// 作者：lix
    /// 日期：2018/6/11 18:22:24
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ProfitService
    {
        #region 初始化
        private ProfitService() { }
        private static readonly Lazy<ProfitService> Linstance = new Lazy<ProfitService>(() => new ProfitService());

        public static ProfitService Instance { get { return Linstance.Value; } }

        #endregion

        #region 收益列表
        /// <summary>
        /// 返回分页列表
        /// </summary>
        /// <param name="queryArgs">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> GetProfitList(ProfitQueryDto queryArgs)
        {
            //查询收益列表
            var proFitTuple = new ProfitBO().GetProFitList(queryArgs.MapTo<ProfitQuery>());

            var listDic = new List<Dictionary<string, object>>();

            //拼装数据结构
            if (proFitTuple.Item1 != null)
            {
                var profitDtoList = proFitTuple.Item1.MapToList<ProfitDo, ProfitModelDto>();

                var listDate = profitDtoList.GroupBy(t => new { t.ProfitDate }).ToList();
                listDic.AddRange(listDate.Select(item => new Dictionary<string, object>
                {
                    {"ProfitDate", item.Key.ProfitDate}, {"DateList", item}
                }));
            }

            return new Dictionary<string, object>() { { "ProfitList", listDic }, { "TotalCount", proFitTuple.Item2 } };
        }
        #endregion

        /// <summary>
        /// 添加用户收益
        /// </summary>
        /// <param name="profitType">收益类型</param>
        /// <param name="detailDescription">收益描述</param>
        /// <param name="incomPrice">收益金额</param>
        /// <param name="userId">用户Id</param>
        /// <param name="dtDate">插入时间</param>
        /// <param name="insertCount">插入个数</param>
        /// <returns></returns>
        public bool AddProfit(int userId, ProfitTypeEnum profitType, string detailDescription, decimal incomPrice, DateTime? dtDate, int insertCount)
        {
            string errorMsg = new ProfitBO().AddProfit(userId, profitType, detailDescription, incomPrice, dtDate, insertCount);
            if (string.IsNullOrEmpty(errorMsg))
                return true;
            else
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("AddProfit:" + errorMsg);
            }
            return false;
        }
    }
}
