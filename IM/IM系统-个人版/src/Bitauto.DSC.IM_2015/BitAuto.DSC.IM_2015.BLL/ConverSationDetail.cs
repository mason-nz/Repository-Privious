using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类ConverSationDetail 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ConverSationDetail
    {
        #region Instance
        public static readonly ConverSationDetail Instance = new ConverSationDetail();
        #endregion

        #region Contructor
        protected ConverSationDetail()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 获取会话明细的表名
        /// </summary>
        /// <returns>返回表名</returns>
        public string GetSationDetailName()
        {
            return Dal.ConverSationDetail.Instance.GetSationDetailName();
        }

        /// <summary>
        ///  根据会话ID查询明细
        /// </summary>
        /// <param name="CSID">会员ID</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public DataTable GetDetailByCSID(int CSID, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ConverSationDetail.Instance.GetDetailByCSID(CSID, order, currentPage, pageSize, out totalCount);
        }


        /// <summary>
        /// 根据会话ID查询所有明细(不推荐使用)
        /// </summary>
        /// <param name="CSID">会话ID</param>
        /// <returns></returns>
        public DataTable GetDetailByCSID(int CSID)
        {
            int totalCount = 0;
            return Dal.ConverSationDetail.Instance.GetDetailByCSID(CSID, "", 1, 100000, out totalCount);
        }


        /// <summary>
        ///  查询当前月明细
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public DataTable GetConverSationDetail(QueryConverSationDetail CSID, string order, int currentPage, int pageSize, out int totalCount, string TableName = "")
        {
            return Dal.ConverSationDetail.Instance.GetConverSationDetail(CSID, order, currentPage, pageSize, out totalCount, TableName);
        }

        /// <summary>
        ///  模糊查询会话历史记录
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="content">内容</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public DataTable GetDetailByContent(DateTime date, string content, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ConverSationDetail.Instance.GetDetailByContent(date, content, order, currentPage, pageSize, out totalCount);
        }

        #endregion

        public object GetSourceTypeValue(string VisitID)
        {
            return Dal.ConverSationDetail.Instance.GetSourceTypeValue(VisitID);
        }

    }
}

