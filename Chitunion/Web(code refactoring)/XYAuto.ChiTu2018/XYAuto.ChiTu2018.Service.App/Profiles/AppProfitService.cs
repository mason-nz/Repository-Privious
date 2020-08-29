/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.Profiles
* 类 名 称 ：ProfitService
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/12 10:03:32
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.Profit;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Enum.Profit;
using XYAuto.ChiTu2018.Entities.Extend.Profit;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Service.App.Profiles.Dto;

namespace XYAuto.ChiTu2018.Service.App.Profiles
{
    public class AppProfitService
    {
        #region 初始化
        private AppProfitService() { }
        private static readonly Lazy<AppProfitService> Linstance = new Lazy<AppProfitService>(() => new AppProfitService());

        public static AppProfitService Instance { get { return Linstance.Value; } }

        #endregion

        #region 收益列表
        /// <summary>
        /// 返回分页列表
        /// </summary>
        /// <param name="queryArgs">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> GetProfitList(AppProfitQueryDto queryArgs)
        {
            //查询收益列表
            var proFitTuple = new ProfitBO().GetProFitList(queryArgs.MapTo<ProfitQuery>());

            var listDic = new List<Dictionary<string, object>>();

            //拼装数据结构
            if (proFitTuple.Item1 != null)
            {
                var profitDtoList = proFitTuple.Item1.MapToList<ProfitDo, AppProfitModelDto>();

                var listDate = profitDtoList.GroupBy(t => new { t.ProfitDate }).ToList();
                listDic.AddRange(listDate.Select(item => new Dictionary<string, object>
                {
                    {"ProfitDate", item.Key.ProfitDate}, {"DateList", item}
                }));
            }

            return new Dictionary<string, object>() { { "ProfitList", listDic }, { "TotalCount", new LEADOrderInfoBO().GetUserOrderCount(queryArgs.UserID) + proFitTuple.Item2 } };
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
