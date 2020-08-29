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
    public class ADDetailInfo
    {
        #region Instance
        public static readonly ADDetailInfo Instance = new ADDetailInfo();
        #endregion

        #region Contructor
        protected ADDetailInfo()
        { }
        #endregion

        #region Select        
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetADDetailInfo(QueryADDetailInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ADDetailInfo.Instance.GetADDetailInfo(query, order, currentPage, pageSize, out totalCount);
        }              
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ADDetailInfo.Instance.GetADDetailInfo(new QueryADDetailInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 根据主键Recid得到一个对象实体
        /// </summary>
        public Entities.ADDetailInfo GetADDetailInfo(int recid)
        {

            return Dal.ADDetailInfo.Instance.GetADDetailInfo(recid);
        }

        #endregion      

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Insert(Entities.ADDetailInfo model)
        {           
            return Dal.ADDetailInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertADDetailInfo(Entities.ADDetailInfo model)
        {
            return Dal.ADDetailInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ADDetailInfo model)
        {
            return Dal.ADDetailInfo.Instance.Update(model);
        }      

        #endregion
        
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int recid)
        {
            return Dal.ADDetailInfo.Instance.Delete(recid);
        }
        #endregion

        #region 根据主订单号删除广告位
        public int DeleteByOrderID(string orderid)
        {
            return Dal.ADDetailInfo.Instance.DeleteByOrderID(orderid);
        }
        #endregion

        #region 将DataRow转成实体
        /// <summary>
        /// 将DataRow转成实体
        /// </summary>
        public Entities.ADDetailInfo DataRowToModel(DataRow row)
        {
            return Dal.ADDetailInfo.Instance.DataRowToModel(row);
        }
        #endregion        

        public DataTable GetADDetailInfoBySubOrderID(string subOrderID)
        {
            return Dal.ADDetailInfo.Instance.GetADDetailInfoBySubOrderID(subOrderID);
        }
        #region 根据广告位ID获取广告位信息
        public Entities.ADDetailInfo p_GetPubDetailInfo_SelectV1_1(int mediaType, int pubDetailID)
        {
            return Dal.ADDetailInfo.Instance.p_GetPubDetailInfo_SelectV1_1(mediaType, pubDetailID);
        }
        #endregion

        #region 根据订单号查询广告位
        public List<T> QueryADDetailBySubOrderID<T>(int mediaType, string subOrderID)
        {
            return Util.DataTableToList<T>(Dal.ADDetailInfo.Instance.QueryADDetailBySubOrderID(mediaType, subOrderID));
        }
        #endregion
        #region 根据广告位ID查询排期
        public List<T> QueryADScheduleInfoByADDetailID<T>(int adDetailID)
        {
            DataTable dtADSchedule = null;
            dtADSchedule = Dal.ADDetailInfo.Instance.QueryADScheduleInfoByADDetailID(adDetailID);
            //if (dtADSchedule?.Rows.Count > 0)
            //    return Util.DataTableToList<T>(dtADSchedule);

            return Util.DataTableToList<T>(dtADSchedule);
        }
        #endregion
    }
}
