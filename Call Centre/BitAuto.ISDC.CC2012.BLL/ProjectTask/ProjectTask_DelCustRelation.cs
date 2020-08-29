using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类ProjectTask_DelCustRelation 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_DelCustRelation
	{
		#region Instance
        public static readonly ProjectTask_DelCustRelation Instance = new ProjectTask_DelCustRelation();
        #endregion

        #region Contructor
        protected ProjectTask_DelCustRelation()
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
        public DataTable GetProjectTask_DelCustRelation(QueryProjectTask_DelCustRelation query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelation(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelation(new QueryProjectTask_DelCustRelation(), string.Empty, 1, 2147483647, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_DelCustRelation GetProjectTask_DelCustRelation(int RecID)
        {

            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelation(RecID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_DelCustRelation GetProjectTask_DelCustRelationByTID(string tid)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelationByTID(tid);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByTID(string tid)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.PTID = tid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_DelCustRelation model)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_DelCustRelation model)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.Update(model);
        }
        /// <summary>
        /// 根据TID，更新CustID
        /// </summary>
        /// <param name="tid">TID</param>
        /// <param name="custid">CustID</param>
        public void UpdateCustIDByTID(string tid, string custid)
        {
            Entities.ProjectTask_DelCustRelation model = GetProjectTask_DelCustRelationByTID(tid);
            if (model != null)
            {
                model.CustID = custid;
                Update(model);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.Delete(RecID);
        }

        /// <summary>
        /// 根据客户ID，删除多条数据
        /// </summary>
        /// <param name="custID">客户ID</param>
        /// <returns></returns>
        public int DeleteByCustID(string custID)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.DeleteByCustID(custID);
        }
        /// <summary>
        /// 根据任务ID，删除多条数据
        /// </summary>
        /// <param name="TID">任务ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.DeleteByTID(tid);
        }
        #endregion

        /// <summary>
        /// 根据任务ID，返回备注信息
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <returns>备注信息</returns>
        public string GetRemarkByTID(string tid)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.PTID = tid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return dt.Rows[0]["Remark"].ToString().Trim();
            }
            return string.Empty;
        }
	}
}

