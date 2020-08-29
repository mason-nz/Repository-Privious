using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class ADScheduleInfo
    {
        #region Instance
        public static readonly ADScheduleInfo Instance = new ADScheduleInfo();
        #endregion

        #region Contructor
        protected ADScheduleInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 根据广告位detailid获取CPD排期信息
        /// </summary>
        /// <param name="addetailid"></param>
        /// <returns></returns>
        public DataTable GetADScheduleInfo_ByADDetailID(int addetailid)
        {
            return Dal.ADScheduleInfo.Instance.GetADScheduleInfo_ByADDetailID(addetailid);
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetADScheduleInfo(QueryADScheduleInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ADScheduleInfo.Instance.GetADScheduleInfo(query, order, currentPage, pageSize, out totalCount);
        }              
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ADScheduleInfo.Instance.GetADScheduleInfo(new QueryADScheduleInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ADScheduleInfo GetADScheduleInfo(int recid)
        {

            return Dal.ADScheduleInfo.Instance.GetADScheduleInfo(recid);
        }

        #endregion      

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Insert(Entities.ADScheduleInfo model)
        {           
            return Dal.ADScheduleInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertADScheduleInfo(Entities.ADScheduleInfo model)
        {
            return Dal.ADScheduleInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ADScheduleInfo model)
        {
            return Dal.ADScheduleInfo.Instance.Update(model);
        }      

        #endregion
        
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int recid)
        {

            return Dal.ADScheduleInfo.Instance.Delete(recid);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int recid)
        {

            return Dal.ADScheduleInfo.Instance.Delete(sqltran, recid);
        }

        #endregion

        #region 根据主订单号删除排期信息
        /// <summary>
        /// 根据主订单号删除排期信息
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <returns></returns>
        public int DeleteByOrderID(string orderid)
        {
            return Dal.ADScheduleInfo.Instance.DeleteByOrderID(orderid);
        }
        #endregion

        #region 根据子订单明细ID查询所属排期信息
        /// <summary>
        /// 根据子订单明细ID查询所属排期信息
        /// </summary>
        /// <param name="Detailid">子订单明细ID</param>
        /// <returns></returns>
        public DataTable GetADScheduleInfoByDetailID(int Detailid)
        {
            return Dal.ADScheduleInfo.Instance.GetADScheduleInfoByDetailID(Detailid);
        }
        #endregion

        #region 批量插入
        public void Insert_BulkCopyToDB(DataTable dt)
        {
            Dal.ADScheduleInfo.Instance.Insert_BulkCopyToDB(dt);
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ADScheduleInfo DataRowToModel(DataRow row)
        {
            return Dal.ADScheduleInfo.Instance.DataRowToModel(row);
        }

        #region 根据广告位ID查询
        public List<RequestADScheduleInfoDto> QueryByADDetailID(int detailID)
        {
            return Util.DataTableToList<RequestADScheduleInfoDto>(Dal.ADScheduleInfo.Instance.QueryByADDetailID(detailID));
        }
        #endregion
    }
}
