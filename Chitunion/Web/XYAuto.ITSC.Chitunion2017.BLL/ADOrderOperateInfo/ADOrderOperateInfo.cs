using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class ADOrderOperateInfo
    {
        #region Instance
        public static readonly ADOrderOperateInfo Instance = new ADOrderOperateInfo();
        #endregion

        #region Contructor
        protected ADOrderOperateInfo()
        { }
        #endregion

        #region Select
        #region 根据订单号查询得到一个对象实体
        public Entities.ADOrderOperateInfo GetADOrderOperateInfo_ByOrderID(string orderid)
        {
            return Dal.ADOrderOperateInfo.Instance.GetADOrderOperateInfo_ByOrderID(orderid);
        }
        #endregion

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetADOrderOperateInfo(QueryADOrderOperateInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ADOrderOperateInfo.Instance.GetADOrderOperateInfo(query, order, currentPage, pageSize, out totalCount);
        }              
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ADOrderOperateInfo.Instance.GetADOrderOperateInfo(new QueryADOrderOperateInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ADOrderOperateInfo GetADOrderOperateInfo(int recid)
        {

            return Dal.ADOrderOperateInfo.Instance.GetADOrderOperateInfo(recid);
        }

        #endregion      

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Insert(Entities.ADOrderOperateInfo model)
        {           
            return Dal.ADOrderOperateInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertADOrderOperateInfo(Entities.ADOrderOperateInfo model)
        {
            return Dal.ADOrderOperateInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ADOrderOperateInfo model)
        {
            return Dal.ADOrderOperateInfo.Instance.Update(model);
        }      

        #endregion
        

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID)
        {

            return Dal.ADOrderOperateInfo.Instance.Delete(OrderID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {

            return Dal.ADOrderOperateInfo.Instance.Delete(sqltran, OrderID);
        }

        #endregion
    }
}
