using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Threading;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类AutoCall_TaskInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-09-14 09:57:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AutoCall_TaskInfo
    {
        #region Instance
        public static readonly AutoCall_TaskInfo Instance = new AutoCall_TaskInfo();
        #endregion

        #region Contructor
        protected AutoCall_TaskInfo()
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
        public DataTable GetAutoCall_TaskInfo(QueryAutoCall_TaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AutoCall_TaskInfo.Instance.GetAutoCall_TaskInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.AutoCall_TaskInfo.Instance.GetAutoCall_TaskInfo(new QueryAutoCall_TaskInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.AutoCall_TaskInfo GetAutoCall_TaskInfo(int ACTID)
        {

            return Dal.AutoCall_TaskInfo.Instance.GetAutoCall_TaskInfo(ACTID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByACTID(int ACTID)
        {
            QueryAutoCall_TaskInfo query = new QueryAutoCall_TaskInfo();
            query.ACTID = ACTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAutoCall_TaskInfo(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.AutoCall_TaskInfo model)
        {
            return Dal.AutoCall_TaskInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.AutoCall_TaskInfo model)
        {
            return Dal.AutoCall_TaskInfo.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int ACTID)
        {

            return Dal.AutoCall_TaskInfo.Instance.Delete(ACTID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int ACTID)
        {

            return Dal.AutoCall_TaskInfo.Instance.Delete(sqltran, ACTID);
        }

        #endregion


        /// 根据项目导入任务到自动外呼任务表中
        /// <summary>
        /// 根据项目导入任务到自动外呼任务表中
        /// </summary>
        public void AutoCallTaskInfoUpdate(long projectid)
        {
            int userid = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
            Dal.AutoCall_TaskInfo.Instance.AutoCallTaskInfoUpdate(projectid, userid);
        }
        /// 根据项目导入任务到自动外呼任务表中
        /// <summary>
        /// 根据项目导入任务到自动外呼任务表中
        /// </summary>
        public void AutoCallTaskInfoUpdate_Async(long projectid)
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    BLL.Loger.Log4Net.Info("异步-根据项目导入任务到自动外呼任务表中" + obj.ToString());
                    int userid = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
                    Dal.AutoCall_TaskInfo.Instance.AutoCallTaskInfoUpdate(CommonFunction.ObjectToLong(obj), userid);
                    sw.Stop();
                    BLL.Loger.Log4Net.Info("异步-根据项目导入任务到自动外呼任务表中 耗时（ms）：" + sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("异步-根据项目导入任务到自动外呼任务表中" + obj.ToString(), ex);
                }
            }, projectid);
        }
        /// 根据手机号码查询当前时间正在通话的任务ID
        /// <summary>
        /// 根据手机号码查询当前时间正在通话的任务ID
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string GetCurrentTaskIDByPhone(string phone)
        {
            return Dal.AutoCall_TaskInfo.Instance.GetCurrentTaskIDByPhone(phone);
        }
    }
}

