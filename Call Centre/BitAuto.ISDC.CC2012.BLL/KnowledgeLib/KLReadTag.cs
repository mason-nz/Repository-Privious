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
    /// 业务逻辑类KLReadTag 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:08 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KLReadTag
    {
        #region Instance
        public static readonly KLReadTag Instance = new KLReadTag();
        #endregion

        #region Contructor
        protected KLReadTag()
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
        public DataTable GetKLReadTag(QueryKLReadTag query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KLReadTag.Instance.GetKLReadTag(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KLReadTag.Instance.GetKLReadTag(new QueryKLReadTag(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KLReadTag GetKLReadTag(long RecID)
        {

            return Dal.KLReadTag.Instance.GetKLReadTag(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryKLReadTag query = new QueryKLReadTag();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKLReadTag(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Update(sqltran, model);
        }

        /// <summary>
        /// 根据知识点ID，更新阅读标记为"未读"
        /// </summary>
        /// <param name="sqltran"></param>
        /// <param name="KLID"></param>
        /// <param name="Tag">1 已读  0未读</param>
        /// <returns></returns>
        public int UpdateTagByKLID(SqlTransaction sqltran, long KLID, int Tag)
        {
            return Dal.KLReadTag.Instance.UpdateTagByKLID(sqltran, KLID, Tag);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.KLReadTag.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.KLReadTag.Instance.Delete(sqltran, RecID);
        }

        public int DeleteByUserID(SqlTransaction sqltran, int UserID, long klid)
        {
            return Dal.KLReadTag.Instance.DeleteByUserID(sqltran, UserID, klid);
        }
        #endregion

        /// <summary>
        /// 标记为已读
        /// </summary>
        /// <param name="kid">知识点ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="tag">1 已读  0未读</param>
        public int SetReadTag(int kid, int userID, int tag)
        {
            Entities.QueryKLReadTag query = new QueryKLReadTag();

            query.KLID = kid;
            query.UserID = userID;
            int totalCount = 0;
            DataTable dt = BLL.KLReadTag.Instance.GetKLReadTag(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                //有，就更改标志
                return Dal.KLReadTag.Instance.ModifyReadTag(kid, userID, tag);
            }
            else
            {
                //没有，就插入
                Entities.KLReadTag newModel = new Entities.KLReadTag();
                newModel.KLID = kid;
                newModel.UserID = userID;
                newModel.ReadTag = tag;
                newModel.CreateTime = DateTime.Now;
                newModel.CreateUserID = userID;

                return Dal.KLReadTag.Instance.Insert(newModel);
            }

        }
    }
}

