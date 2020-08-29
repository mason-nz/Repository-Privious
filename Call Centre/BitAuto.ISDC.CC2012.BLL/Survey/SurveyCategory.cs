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
    /// 业务逻辑类SurveyCategory 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyCategory
    {
        #region Instance
        public static readonly SurveyCategory Instance = new SurveyCategory();
        #endregion

        #region Contructor
        protected SurveyCategory()
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
        public DataTable GetSurveyCategory(QuerySurveyCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyCategory.Instance.GetSurveyCategory(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SurveyCategory.Instance.GetSurveyCategory(new QuerySurveyCategory(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyCategory GetSurveyCategory(int SCID)
        {
            return Dal.SurveyCategory.Instance.GetSurveyCategory(SCID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsBySCID(int SCID)
        {
            QuerySurveyCategory query = new QuerySurveyCategory();
            query.SCID = SCID;

            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyCategory(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int SCID)
        {

            return Dal.SurveyCategory.Instance.Delete(SCID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SCID)
        {

            return Dal.SurveyCategory.Instance.Delete(sqltran, SCID);
        }

        #endregion

        public int GetSCIDByName(string categoryName)
        {
            return Dal.SurveyCategory.Instance.GetSCIDByName(categoryName);
        }

        /// <summary>
        /// 通过UserID获取所在业务组ID
        /// </summary>
        /// <returns>成功找到返回BGID，否则返回0</returns>
        public int GetSelfBGIDByUserID(int userID)
        {
            int bgid = 0;

            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userID);
            if (model != null && model.BGID != null)
            {
                bgid = (int)model.BGID;
            }

            return bgid;
        }

        /// <summary>
        /// 获取userID所在业务组下面的“工单分类”ID
        /// </summary>
        /// <returns>成功找到返回SCID，否则返回0</returns>
        public int GetSelfSCIDByUserID(int userID)
        {
            int scid = 0;

            int bgid = GetSelfBGIDByUserID(userID);
            if (bgid != 0)
            {
                Entities.QuerySurveyCategory query = new QuerySurveyCategory();
                query.BGID = bgid;
                query.Name = "工单分类";
                int count = 0;
                DataTable dt = BLL.SurveyCategory.Instance.GetSurveyCategory(query, "", 1, 1, out count);
                if (dt != null && dt.Rows.Count == 1 && int.TryParse(dt.Rows[0]["SCID"].ToString(), out scid))
                {

                }
            }

            return scid;
        }

        /// <summary>
        ///  分类名称是否重复
        /// </summary>
        public bool IsExistsCategoryName(string categoryName)
        {
            return Dal.SurveyCategory.Instance.IsExistsCategoryName(categoryName);
        }
    }
}

