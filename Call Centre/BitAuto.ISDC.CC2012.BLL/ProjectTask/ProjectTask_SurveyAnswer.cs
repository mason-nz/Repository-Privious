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
    /// 业务逻辑类ProjectTask_SurveyAnswer 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_SurveyAnswer
    {
        #region Instance
        public static readonly ProjectTask_SurveyAnswer Instance = new ProjectTask_SurveyAnswer();
        #endregion

        #region Contructor
        protected ProjectTask_SurveyAnswer()
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
        public DataTable GetProjectTask_SurveyAnswer(QueryProjectTask_SurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(query, order, currentPage, pageSize, out totalCount);
        }
        public int GetProjectTask_SurveyAnswer_Count(QueryProjectTask_SurveyAnswer query)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer_Count(query);
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
        public DataTable GetProjectTask_SurveyAnswer(SqlTransaction tran, QueryProjectTask_SurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(tran, query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(new QueryProjectTask_SurveyAnswer(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_SurveyAnswer GetProjectTask_SurveyAnswer(long RecID)
        {

            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(RecID);
        }

        /// <summary>
        /// 根据项目ID获取该项目的答题信息
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByProjectID(int projectID, int siid)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetAnswerDetailByProjectID(projectID, siid);
        }

        /// <summary>
        /// 根据条件，查询记录条数
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="query">条件对象</param>
        /// <returns>有记录返回True，否则返回False</returns>
        public Entities.ProjectTask_SurveyAnswer GetProjectTask_SurveyAnswerByQuery(SqlTransaction tran, QueryProjectTask_SurveyAnswer query)
        {
            DataTable dt = Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswerByQuery(tran, query);
            if (dt!=null&&!string.IsNullOrEmpty(dt.Rows[0]["RecID"].ToString()))
            {
                return Dal.ProjectTask_SurveyAnswer.Instance.LoadSingleProjectTask_SurveyAnswer(dt.Rows[0]);
            }
            return null;
        }
        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryProjectTask_SurveyAnswer query = new QueryProjectTask_SurveyAnswer();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_SurveyAnswer(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Update(sqltran, model);
        }

        public int UpdateCreateTimeAndStatus(SqlTransaction tran, Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.UpdateCreateTimeAndStatus(tran, model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.ProjectTask_SurveyAnswer.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.ProjectTask_SurveyAnswer.Instance.Delete(sqltran, RecID);
        }

        #endregion


    }
}

