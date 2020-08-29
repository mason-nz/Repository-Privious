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
    /// 业务逻辑类TPage 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TPage
    {
        #region Instance
        public static readonly TPage Instance = new TPage();
        #endregion

        #region Contructor
        protected TPage()
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
        public DataTable GetTPage(QueryTPage query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.TPage.Instance.GetTPage(query, order, currentPage, pageSize, out totalCount);
        }

        //获取状态   add lxw 13.3.22
        //未完成-保存但未生成模板-status=0；已完成-已保存且生成模板但新建的新表没数据-status=1；已使用：已生成模板并新建的新表存在数据-status=1
        //在为已完成时，需要根据TTable表查询该TTcode下的TTIsData是否为1，如果为1则为已使用，status赋值为2
        public int getStatus(string recID, string status)
        {
            int _status;
            int _recID;
            if (int.TryParse(status, out _status) && int.TryParse(recID, out _recID))
            {
                if (_status == 1)
                {
                    //判断该生成的模板表是否存在数据
                    int count = BLL.TPage.Instance.isHasDataByTTName(_recID);
                    if (count > 0)//不等于0，表示模板表有数据，状态修改为已使用
                    {
                        _status = 2;
                    }
                }

            }
            return _status;
        }

        //获取状态   add lxw 13.3.22
        //未完成-保存但未生成模板-status=0；已完成-已保存且生成模板但新建的新表没数据-status=1；已使用：已生成模板并新建的新表存在数据-status=1
        //在为已完成时，需要根据TTable表查询该TTcode下的TTIsData是否为1，如果为1则为已使用，status赋值为2
        public int getStatusByTTCode(string status, string ttCode)
        {
            int _status;
            if (int.TryParse(status, out _status) && ttCode != string.Empty)
            {
                if (_status == 1)
                {
                    //判断该生成的模板表是否存在数据
                    Entities.TTable model = BLL.TTable.Instance.GetTTableByTTCode(ttCode);
                    if (model != null && model.TTIsData == 1)//等于1，表示模板表有数据，状态修改为已使用
                    {
                        _status = 2;
                    }
                }

            }
            return _status;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.TPage.Instance.GetTPage(new QueryTPage(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.TPage GetTPage(int RecID)
        {

            return Dal.TPage.Instance.GetTPage(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryTPage query = new QueryTPage();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTPage(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.TPage model)
        {
            return Dal.TPage.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TPage model)
        {
            return Dal.TPage.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.TPage model)
        {
            return Dal.TPage.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TPage model)
        {
            return Dal.TPage.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.TPage.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.TPage.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public DataTable GetAllCreateUserID()
        {
            return Dal.TPage.Instance.GetAllCreateUserID();
        }

        /// <summary>
        /// 判断recID的生成模板表是否存在数据
        /// </summary>
        /// <param name="recID">TPage主键</param>
        /// <returns></returns>
        public int isHasDataByTTName(int recID)
        {
            return Dal.TPage.Instance.isHasDataByTTName(recID);
        }

        /// <summary>
        /// 根据TTCode获取实体
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Entities.TPage GetTPageByTTCode(string ttcode)
        {
            Entities.TPage tpageModel = new Entities.TPage();
            Entities.QueryTPage query = new QueryTPage();
            query.TTCode = ttcode;
            int totalCount = 0;
            DataTable dt = BLL.TPage.Instance.GetTPage(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                tpageModel = Dal.TPage.Instance.GetTPage(int.Parse(dt.Rows[0]["RecID"].ToString()));
            }
            return tpageModel;
        }
    }
}

