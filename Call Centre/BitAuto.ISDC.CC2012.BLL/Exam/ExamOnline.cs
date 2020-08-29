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
    /// 业务逻辑类ExamOnline 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:16 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamOnline
    {
        #region Instance
        public static readonly ExamOnline Instance = new ExamOnline();
        #endregion

        #region Contructor
        protected ExamOnline()
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
        public DataTable GetExamOnline(QueryExamOnline query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamOnline.Instance.GetExamOnline(query, order, currentPage, pageSize, out totalCount);
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
        public DataTable GetExamScoreManage(ExamScoreManageQuery query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string userid = BLL.Util.GetLoginUserID().ToString();
            return Dal.ExamOnline.Instance.GetExamScoreManage(query, order, currentPage, pageSize, out totalCount, userid);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamOnline.Instance.GetExamOnline(new QueryExamOnline(), string.Empty, 1, 1000000, out totalCount);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamOnline GetExamOnline(long EOLID)
        {

            return Dal.ExamOnline.Instance.GetExamOnline(EOLID);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByEOLID(long EOLID)
        {
            QueryExamOnline query = new QueryExamOnline();
            query.EOLID = EOLID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamOnline(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Insert(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Insert(sqltran, model);
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Update(sqltran, model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long EOLID)
        {

            return Dal.ExamOnline.Instance.Delete(EOLID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long EOLID)
        {

            return Dal.ExamOnline.Instance.Delete(sqltran, EOLID);
        }
        #endregion

        #region 存储分组数据
        /// <summary>
        /// 存储分组数据
        /// 强斐
        /// 2014-10-11
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Entities.ExamOnline SaveBgid(Entities.ExamOnline model)
        {
            Entities.EmployeeAgent agent = EmployeeAgent.Instance.GetEmployeeAgentByUserID(model.ExamPersonID);
            if (agent != null)
            {
                model.BGID = agent.BGID;
            }
            return model;
        }
        #endregion
    }
}

