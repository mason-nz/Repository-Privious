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
    /// 业务逻辑类SMSTemplate 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-12-23 06:17:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SMSTemplate
    {
        #region Instance
        public static readonly SMSTemplate Instance = new SMSTemplate();
        #endregion

        #region Contructor
        protected SMSTemplate()
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
        public DataTable GetSMSTemplate(QuerySMSTemplate query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSTemplate.Instance.GetSMSTemplate(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SMSTemplate.Instance.GetSMSTemplate(new QuerySMSTemplate(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 根据当前人取当前人数据权限下的模板创建人
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetCreateUserID(int userid)
        {
            return Dal.SMSTemplate.Instance.GetCreateUserID(userid);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SMSTemplate GetSMSTemplate(int RecID)
        {
            //该表无主键信息，请自定义主键/条件字段
            return Dal.SMSTemplate.Instance.GetSMSTemplate(RecID);
        }

        #endregion

        #region IsExists

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.SMSTemplate model)
        {
            Dal.SMSTemplate.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.SMSTemplate model)
        {
            Dal.SMSTemplate.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.SMSTemplate model)
        {
            return Dal.SMSTemplate.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SMSTemplate model)
        {
            return Dal.SMSTemplate.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            //该表无主键信息，请自定义主键/条件字段
            return Dal.SMSTemplate.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            //该表无主键信息，请自定义主键/条件字段
            return Dal.SMSTemplate.Instance.Delete(sqltran, RecID);
        }

        #endregion

    }
}

