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
    public class PublishAuditInfo
    {
        #region Instance
        public static readonly PublishAuditInfo Instance = new PublishAuditInfo();
        #endregion

        #region Contructor
        protected PublishAuditInfo()
        { }
        #endregion

        #region 修改刊例上下架状态
        /// <summary>
        /// 修改刊例状态
        /// </summary>
        /// <param name="pubid">刊例ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public int UpdateStatusByPubID_PublishBasic(int pubid, int status)
        {
            return Dal.PublishAuditInfo.Instance.UpdateStatusByPubID_PublishBasic(pubid, status);
        }
        #endregion

        #region 根据刊例ID获取状态
        /// <summary>
        /// 根据刊例ID获取状态
        /// </summary>
        /// <param name="pubid"></param>
        /// <returns></returns>
        public int GetStatus_PublishID(int pubid)
        {
            return Dal.PublishAuditInfo.Instance.GetStatus_PublishID(pubid);
        }
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
        public DataTable GetPublishAuditInfo(QueryPublishAuditInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.PublishAuditInfo.Instance.GetPublishAuditInfo(query, order, currentPage, pageSize, out totalCount);
        }              
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.PublishAuditInfo.Instance.GetPublishAuditInfo(new QueryPublishAuditInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.PublishAuditInfo GetPublishAuditInfo(int recid)
        {

            return Dal.PublishAuditInfo.Instance.GetPublishAuditInfo(recid);
        }

        #endregion      

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Insert(Entities.PublishAuditInfo model)
        {           
            return Dal.PublishAuditInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertPublishAuditInfo(Entities.PublishAuditInfo model)
        {
            return Dal.PublishAuditInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.PublishAuditInfo model)
        {
            return Dal.PublishAuditInfo.Instance.Update(model);
        }      

        #endregion
        

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int recid)
        {

            return Dal.PublishAuditInfo.Instance.Delete(recid);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int recid)
        {

            return Dal.PublishAuditInfo.Instance.Delete(sqltran, recid);
        }

        #endregion
    }
}
