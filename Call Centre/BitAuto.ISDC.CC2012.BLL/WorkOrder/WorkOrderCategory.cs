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
    /// 业务逻辑类WorkOrderCategory 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:20 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderCategory
    {
        #region Instance
        public static readonly WorkOrderCategory Instance = new WorkOrderCategory();
        #endregion

        #region Contructor
        protected WorkOrderCategory()
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
        public DataTable GetWorkOrderCategory(QueryWorkOrderCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderCategory.Instance.GetWorkOrderCategory(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.WorkOrderCategory.Instance.GetWorkOrderCategory(new QueryWorkOrderCategory(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 根据分类ID，返回分类名称
        /// </summary>
        /// <param name="CategoryID">分类ID</param>
        /// <returns>分类名称</returns>
        public string GetCategoryNameByCategoryID(int CategoryID)
        {
            Entities.WorkOrderCategory model = GetWorkOrderCategory(CategoryID);
            if (model != null)
            {
                return model.Name;
            }
            return string.Empty;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderCategory GetWorkOrderCategory(int RecID)
        {

            return Dal.WorkOrderCategory.Instance.GetWorkOrderCategory(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryWorkOrderCategory query = new QueryWorkOrderCategory();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderCategory(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.WorkOrderCategory.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.WorkOrderCategory.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public DataTable GetCategoryFullByCategoryID(string CategoryID)
        {
            return Dal.WorkOrderCategory.Instance.GetCategoryFullName(CategoryID);
        }

        public string GetCategoryFullName(string CategoryID)
        {

            if (!string.IsNullOrEmpty(CategoryID))
            {
                string fullName = string.Empty;

                DataTable dt = Dal.WorkOrderCategory.Instance.GetCategoryFullName(CategoryID);
                for (int k = dt.Rows.Count - 1; k >= 0; k--)
                {
                    fullName += dt.Rows[k]["Name"].ToString() + "-";
                }

                return fullName.TrimEnd('-');
            }
            else
            {
                return "";
            }


        }

        public string GetWorkCategoryJsonBySql(string where)
        {
            string jsonData = string.Empty;

            DataTable dt = BLL.Util.GetTableInfoBySql("select * from WorkOrderCategory where status=0 " + where + " order by OrderNum asc");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                jsonData += i == 0 ? "[{ID:'" + dr["RecID"] + "',Name:'" + dr["Name"] + "'}" : ",{ID:'" + dr["RecID"] + "',Name:'" + dr["Name"] + "'}";

                if (i == dt.Rows.Count - 1)
                {
                    jsonData += "]";
                }

            }

            return jsonData;

        }

    }
}

