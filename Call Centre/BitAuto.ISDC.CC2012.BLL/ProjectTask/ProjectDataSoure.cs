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
    /// 业务逻辑类ProjectDataSoure 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:28 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectDataSoure
    {
        #region Instance
        public static readonly ProjectDataSoure Instance = new ProjectDataSoure();
        #endregion

        #region Contructor
        protected ProjectDataSoure()
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
        public DataTable GetProjectDataSoure(QueryProjectDataSoure query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectDataSoure.Instance.GetProjectDataSoure(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectDataSoure.Instance.GetProjectDataSoure(new QueryProjectDataSoure(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectDataSoure GetProjectDataSoure(long PDSID)
        {

            return Dal.ProjectDataSoure.Instance.GetProjectDataSoure(PDSID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByPDSID(long PDSID)
        {
            QueryProjectDataSoure query = new QueryProjectDataSoure();
            query.PDSID = PDSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectDataSoure(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long PDSID)
        {

            return Dal.ProjectDataSoure.Instance.Delete(PDSID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long PDSID)
        {

            return Dal.ProjectDataSoure.Instance.Delete(sqltran, PDSID);
        }

        #endregion


        public void DeleteByProjectID(SqlTransaction tran, int ProjectID)
        {
            Dal.ProjectDataSoure.Instance.DeleteByProjectID(tran, ProjectID);
        }

        public void UpdateStatusByProjectId(SqlTransaction tran, string status, int ProjectID)
        {
            Dal.ProjectDataSoure.Instance.UpdateStatusByProjectId(tran, status, ProjectID);
        }
        public void UpdateStatusByProjectId(SqlTransaction tran, string status, DataTable dt, int ProjectID)
        {
            Dal.ProjectDataSoure.Instance.UpdateStatusByProjectId(tran, status, dt, ProjectID);
        }

        /// <summary>
        /// 根据项目ID,获取关联数据的ID和个数
        /// </summary>
        /// <param name="p"></param>
        /// <param name="DataCount"></param>
        /// <returns></returns>
        public string GetProjectDataSoureID(long ProjectID, out string DataCount, bool isReturn)
        {
            return Dal.ProjectDataSoure.Instance.GetProjectDataSoureID(ProjectID, out DataCount, isReturn);
        }

        public int GetDataCountByProjectID(long projectID)
        {
            int totalCount = 0;
            Entities.QueryProjectDataSoure query = new QueryProjectDataSoure();
            query.ProjectID = projectID;
            DataTable dt = BLL.ProjectDataSoure.Instance.GetProjectDataSoure(query, "", 1, 1, out totalCount);
            return totalCount;
        }
        /// 获取不存在的数据
        /// <summary>
        /// 获取不存在的数据
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<string> GetNotExistsDataByProjectID(long projectID, string ids)
        {
            return Dal.ProjectDataSoure.Instance.GetNotExistsDataByProjectID(projectID, ids);
        }
    }
}

