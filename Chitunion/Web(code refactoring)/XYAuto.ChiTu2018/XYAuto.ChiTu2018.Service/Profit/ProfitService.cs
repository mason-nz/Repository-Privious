/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.Profit
* 类 名 称 ：ProfitService
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 14:02:55
********************************/

using System;
using System.Collections.Generic;
using System.Linq;
using XYAuto.ChiTu2018.BO.Profit;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Extend.Profit;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Service.Profit.Dto;
using XYAuto.CTUtils.Log;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.Service.Profit
{
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

            return new Dictionary<string, object>() { { "ProfitList", listDic }, { "TotalCount", new LEADOrderInfoBO().GetUserOrderCount(queryArgs.UserID) + proFitTuple.Item2 } };
        }
        #endregion

    }
}
