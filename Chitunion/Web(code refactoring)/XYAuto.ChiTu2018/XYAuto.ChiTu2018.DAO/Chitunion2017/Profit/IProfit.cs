/********************************
* 项目名称 ：XYAuto.ChiTu2018.DAO.Chitunion2017.Profit
* 项目描述 ：
* 类 名 称 ：IProfit
* 类 描 述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 10:08:00
********************************/

using System;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Profit
{
    public interface IProfit : Repository<LE_IncomeDetail>
    {
        /// <summary>
        /// 添加用户收益
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="profitType">收益类型</param>
        /// <param name="detailDescription">收益描述</param>
        /// <param name="incomPrice">收益金额</param>
        /// <param name="dtDate">插入时间</param>
        /// <param name="insertCount">插入条数</param>
        /// <returns></returns>
        int InsertProfit(int userId, int profitType, string detailDescription, decimal incomPrice,
            DateTime? dtDate, int insertCount);
    }
}
