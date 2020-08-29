using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Web;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类KnowledgeLib 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:10 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KnowledgeLib
    {
        #region Instance
        public static readonly KnowledgeLib Instance = new KnowledgeLib();
        #endregion

        #region Contructor
        protected KnowledgeLib()
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
        public DataTable GetKnowledgeLib(QueryKnowledgeLib query, string order, int currentPage, int pageSize, out int totalCount, string wherePlus = "")
        {
            return Dal.KnowledgeLib.Instance.GetKnowledgeLib(query, order, currentPage, pageSize, out totalCount, wherePlus);
        }

        public DataSet GetKnowledgeReport(int userId, int currentPage, int pageSize, out int unReadCount,int kcpid, int kcid, string kw,string Oreder,string asds, bool isUnRead)
        {
            return Dal.KnowledgeLib.Instance.GetKnowledgeReport(userId, currentPage, pageSize, out unReadCount, kcpid, kcid, kw, Oreder,asds,isUnRead);
        }

        public DataTable GetClassifyReport(int userid, int pid, string dtWhere, string RegionId)
        {
            return Dal.KnowledgeLib.Instance.GetClassifyReport(userid, pid, dtWhere, RegionId);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KnowledgeLib.Instance.GetKnowledgeLib(new QueryKnowledgeLib(), string.Empty, 1, 1000000, out totalCount);
        }

        public DataTable GetKnowledgeLibCount(QueryKnowledgeLib query, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KnowledgeLib.Instance.GetKnowledgeLibCount(query, currentPage, pageSize, out totalCount);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KnowledgeLib GetKnowledgeLib(long KLID)
        {

            return Dal.KnowledgeLib.Instance.GetKnowledgeLib(KLID);
        }

        /// <summary>
        /// 获取知识点的HTML内容
        /// </summary>
        /// <param name="KLID"></param>
        /// <returns></returns>
        public string GetKnowledgeHtml(long KLID)
        {
            return Dal.KnowledgeLib.Instance.GetKnowledgeHtml(KLID);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByKLID(long KLID)
        {
            QueryKnowledgeLib query = new QueryKnowledgeLib();
            query.KLID = KLID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeLib(query, string.Empty, 1, 1, out count);
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
        public long Insert(Entities.KnowledgeLib model)
        {
            return Dal.KnowledgeLib.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KnowledgeLib model)
        {
            return Dal.KnowledgeLib.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.KnowledgeLib model)
        {
            return Dal.KnowledgeLib.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KnowledgeLib model)
        {
            return Dal.KnowledgeLib.Instance.Update(sqltran, model);
        }

        public int GetKLIDAllCount(QueryKnowledgeLib query, out int totalCount)
        {
            return Dal.KnowledgeLib.Instance.GetKLIDAllCount(query, out totalCount);
        }

        public void AddClickAndDownloadCounts(int type, int klid)
        {
            Dal.KnowledgeLib.Instance.AddClickAndDownloadCounts(type, klid);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long KLID)
        {

            return Dal.KnowledgeLib.Instance.Delete(KLID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLID)
        {

            return Dal.KnowledgeLib.Instance.Delete(sqltran, KLID);
        }

        #endregion

        #region
        /// <summary>
        /// 知识点下是否有试题
        /// </summary>
        /// <param name="KLID"></param>
        /// <returns></returns>
        public bool IsExistQuestion(int KLID)
        {
            return Dal.KnowledgeLib.Instance.IsExistQuestion(KLID);
        }
        #endregion

        #region 最大值

        /// <summary>
        /// 取当前最大值
        /// </summary>
        /// <returns></returns>
        public int GetCurrMaxID()
        {
            return Dal.KnowledgeLib.Instance.GetCurrMaxID();
        }

        #endregion

        public DataTable getCreateUser()
        {
            return Dal.KnowledgeLib.Instance.getCreateUser();
        }
        public DataTable getModifyUser()
        {
            return Dal.KnowledgeLib.Instance.getModifyUser();
        }

        public void UpdateHtml(long KLID, string HtmlContext)
        {
            Dal.KnowledgeLib.Instance.UpdateHtml(KLID, HtmlContext);
        }
    }


}

