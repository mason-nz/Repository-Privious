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
    /// 业务逻辑类CarSerial 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-12-11 03:57:11 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CarSerial
    {
        #region Instance
        public static readonly CarSerial Instance = new CarSerial();
        #endregion

        #region Contructor
        protected CarSerial()
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
        public DataTable GetCarSerial(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CarSerial.Instance.GetCarSerial(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            DataTable dt = Dal.CarSerial.Instance.GetAllCarSerial();
            return dt;
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CarSerial GetCarSerial(int CSID)
        {
            return Dal.CarSerial.Instance.GetCarSerial(CSID);
        }
        public List<Entities.CarSerial> GetCarSerial(string scids)
        {
            return Dal.CarSerial.Instance.GetCarSerial(scids);
        }
        #endregion

        //#region IsExists
        ///// <summary>
        ///// 是否存在该记录
        ///// </summary>
        //public bool IsExistsByCSID(int CSID)
        //{
        //    QueryCarSerial query = new QueryCarSerial();
        //    query.CSID = CSID;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetCarSerial(query, string.Empty, 1, 1, out count);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //#endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.CarSerial model)
        {
            Dal.CarSerial.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CarSerial model)
        {
            Dal.CarSerial.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CarSerial model)
        {
            return Dal.CarSerial.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CarSerial model)
        {
            return Dal.CarSerial.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int CSID)
        {

            return Dal.CarSerial.Instance.Delete(CSID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int CSID)
        {

            return Dal.CarSerial.Instance.Delete(sqltran, CSID);
        }

        #endregion


        internal void DeleteTable()
        {
            Dal.CarSerial.Instance.DeleteTable();
        }

        internal DataTable GetAllListFromCrm2009()
        {
            return Dal.CarSerial.Instance.GetAllListFromCrm2009();
        }
    }
}

