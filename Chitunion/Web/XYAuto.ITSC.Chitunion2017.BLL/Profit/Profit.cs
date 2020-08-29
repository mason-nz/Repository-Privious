using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Profit;

namespace XYAuto.ITSC.Chitunion2017.BLL.Profit
{
    public class Profit
    {
        public static readonly Profit Instance = new Profit();
        /// <summary>
        /// 获取收益信息列表
        /// </summary>
        /// <param name="TopCount">查询数量</param>
        /// <param name="RowNum">排序ID</param>
        /// <param name="isGetAll">是否查询全部</param>
        /// <returns></returns>
        public Dictionary<string, object> GetProfitList(int TopCount, int RowNum, bool isGetAll)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            //int totalCount = Dal.Profit.Profit.Instance.GetProfitMaxId(userId) + 1;
            int startRowNum = RowNum + 1;
            int endRowNum = RowNum + TopCount;
            List<ProfitInfo> list = Util.DataTableToList<ProfitInfo>(Dal.Profit.Profit.Instance.GetProfitList(userId, startRowNum, endRowNum, isGetAll));
            List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
            if (list != null)
            {
                var listDate = list.GroupBy(t => new { t.ProfitDate }).ToList();
                foreach (var item in listDate)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ProfitDate", item.Key.ProfitDate);
                    dic.Add("DateList", item);
                    listDic.Add(dic);
                }
            }
            dicAll.Add("ProfitList", listDic);
            dicAll.Add("TotalCount", Dal.LETask.LeAdOrderInfo.Instance.GetUserOrderCount(userId) + Dal.Profit.Profit.Instance.GetProfitCount(userId, isGetAll));
            return dicAll;
        }

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
            string errorMsg = Dal.Profit.Profit.Instance.AddProfit(userId, (int)profitType, detailDescription, incomPrice, dtDate, insertCount);
            if (string.IsNullOrEmpty(errorMsg))
                return true;
            else
            {
                Loger.Log4Net.Error("AddProfit:" + errorMsg);
            }
            return false;
        }
    }
}
