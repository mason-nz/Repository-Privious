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
    /// 业务逻辑类SurveyAnswer 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:19 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyAnswer
    {
        #region Instance
        public static readonly SurveyAnswer Instance = new SurveyAnswer();
        #endregion

        #region Contructor
        protected SurveyAnswer()
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
        public DataTable GetSurveyAnswer(QuerySurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyAnswer.Instance.GetSurveyAnswer(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetSurveyAnswerByTextDetail(QuerySurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyAnswer.Instance.GetSurveyAnswerByTextDetail(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SurveyAnswer.Instance.GetSurveyAnswer(new QuerySurveyAnswer(), string.Empty, 1, 1000000, out totalCount);
        }
        /// <summary>
        /// 获取参加试题的人员数
        /// </summary>
        /// <param name="SIID">试题ID</param>
        /// <returns></returns>
        public int GetAnswerUserCountBySPIID(int SPIID)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerUserCountBySPIID(SPIID);
        }
        /// <summary>
        /// 获取参加试题的人员数
        /// </summary>
        /// <param name="SQID">试题ID</param>
        /// <returns></returns>
        public int GetAnswerUserCountBySQID(int SQID, int SPIID)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerUserCountBySQID(SQID, SPIID);
        }

        /// <summary>
        /// 获取此次调查答题详细信息
        /// </summary>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailBySPIID(int SPIID)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerDetailBySPIID(SPIID);
        }

        /// <summary>
        /// 根据PTID任务ID获取此次调查答题详细信息  lxw
        /// </summary>
        /// <param name="PTID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByPTID(string PTID, int projectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerDetailByPTID(PTID, projectID, siid);
        }

        /// <summary>
        /// 根据ReturnVisitCRMCustID回访客户ID获取此次调查答题详细信息  lxw
        /// </summary>
        /// <param name="PTID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByReturnCustID(string ReturnCustID, int projectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerDetailByReturnCustID(ReturnCustID, projectID, siid);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyAnswer GetSurveyAnswer(int RecID)
        {

            return Dal.SurveyAnswer.Instance.GetSurveyAnswer(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsBySPIIDAndSIIDAndSQIDAndCreateUserID(int SPIID, int SIID, int SQID, int CreateUserID)
        {
            QuerySurveyAnswer query = new QuerySurveyAnswer();
            query.SPIID = SPIID;
            query.SIID = SIID;
            query.SQID = SQID;
            query.CreateUserID = CreateUserID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyAnswer(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.SurveyAnswer model)
        {
            Dal.SurveyAnswer.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.SurveyAnswer model)
        {
            Dal.SurveyAnswer.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.SurveyAnswer model)
        {
            return Dal.SurveyAnswer.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyAnswer model)
        {
            return Dal.SurveyAnswer.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.SurveyAnswer.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.SurveyAnswer.Instance.Delete(sqltran, RecID);
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SIID, string PTID)
        {

            return Dal.SurveyAnswer.Instance.Delete(sqltran, SIID, PTID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int DeleteByCustID(SqlTransaction sqltran, int SIID, string CustID)
        {

            return Dal.SurveyAnswer.Instance.DeleteByCustID(sqltran, SIID, CustID);
        }
        #endregion

        /// <summary>
        /// 通过Where得到AnswerContent
        /// </summary> 
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public string getAnswerBySQID(string Where)
        {
            return Dal.SurveyAnswer.Instance.getAnswerBySQID(Where);
        }
        /// <summary>
        /// 通过ProjectID和问卷ID得到任务ID
        /// </summary> 
        /// <param name="typeID">类型：1-数据清洗；2-其他任务</param>
        /// <param name="ProjectID">项目ID</param>
        /// <param name="siid">问卷ID</param>
        /// <returns></returns>
        public DataTable getPTIDByProject(int typeID, int ProjectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.getPTIDByProject(typeID,ProjectID, siid);
        }

        /// <summary>
        /// 通过ProjectID和问卷ID得到任务ID,add by qizq 2014-11-24,通过加任务提交时间区间过滤任务id，只适用于其他任务，数据清洗流程任务提交并不是任务处理完成，可能会有结束，所以找不到任务提交时间，这次需求产品刘萍没有考虑数据清洗业务，所以此方法对数据清洗不起作用
        /// </summary> 
        /// <param name="typeID">类型：1-数据清洗；2-其他任务</param>
        /// <param name="ProjectID">项目ID</param>
        /// <param name="siid">问卷ID</param>
        /// <returns></returns>
        public DataTable getPTIDByProject(int typeID, int ProjectID, int siid, string tasksubstart, string tasksubend)
        {
            return Dal.SurveyAnswer.Instance.getPTIDByProject(typeID, ProjectID, siid, tasksubstart, tasksubend);
        }

        /// <summary>
        /// 通过ProjectID得到客户ID
        /// </summary> 
        /// <param name="ProjectID">项目ID</param> 
        /// <param name="siid">问卷ID</param> 
        /// <returns></returns>
        public DataTable getCustIDByProject(int ProjectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.getCustIDByProject(ProjectID, siid);
        }

        /// <summary>
        /// 通过ProjectID得到客户ID,加文件提交时间过滤 by qizq 2014-11-25
        /// </summary> 
        /// <param name="ProjectID">项目ID</param> 
        /// <param name="siid">问卷ID</param> 
        /// <returns></returns>
        public DataTable getCustIDByProject(int ProjectID, int siid, string substart, string subend)
        {
            return Dal.SurveyAnswer.Instance.getCustIDByProject(ProjectID, siid,substart,subend);

        }
    }
}

