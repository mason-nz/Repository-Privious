using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类OrderTask 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderTask
    {
        #region Instance
        public static readonly OrderTask Instance = new OrderTask();
        #endregion

        #region Contructor
        protected OrderTask()
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
        public DataTable GetOrderTask(QueryOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderTask.Instance.GetOrderTask(query, order, currentPage, pageSize, out totalCount);
        }


        //add by qizq 2012-9-25 无主订单任务列表
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetOrderTaskList(QueryOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = BLL.Util.GetLoginUserID();
            return Dal.OrderTask.Instance.GetOrderTaskList(query, order, currentPage, pageSize, out totalCount, userid);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OrderTask.Instance.GetOrderTask(new QueryOrderTask(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 取任务不同处理人 add by qizq 2013-2-21
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiffassignuserid()
        {
            return Dal.OrderTask.Instance.GetDiffassignuserid();
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderTask GetOrderTask(long TaskID)
        {

            return Dal.OrderTask.Instance.GetOrderTask(TaskID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByTaskID(long TaskID)
        {
            QueryOrderTask query = new QueryOrderTask();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderTask(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long TaskID)
        {

            return Dal.OrderTask.Instance.Delete(TaskID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {

            return Dal.OrderTask.Instance.Delete(sqltran, TaskID);
        }

        #endregion

        /// <summary>
        /// 取所有不同处理人 by qizq
        /// </summary>
        /// <returns></returns>
        public DataTable GetDealPerson()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("UserID", typeof(string));
            List<string> useridlist = new List<string>();
            DataTable dtuserid = GetDiffassignuserid();
            if (dtuserid != null && dtuserid.Rows.Count > 0)
            {
                for (int i = 0; i < dtuserid.Rows.Count; i++)
                {

                    if (dtuserid.Rows[i]["AssignUserID"] != DBNull.Value && !string.IsNullOrEmpty(dtuserid.Rows[i]["AssignUserID"].ToString()) && dtuserid.Rows[i]["AssignUserID"].ToString() != "-2")
                    {
                        if (!useridlist.Contains(dtuserid.Rows[i]["AssignUserID"].ToString()))
                        {
                            DataRow row = dt.NewRow();
                            useridlist.Add(dtuserid.Rows[i]["AssignUserID"].ToString());
                            row["UserID"] = dtuserid.Rows[i]["AssignUserID"].ToString();
                            row["Name"] = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(dtuserid.Rows[i]["AssignUserID"].ToString()));
                            dt.Rows.Add(row);
                        }
                    }
                }
            }
            return dt;

        }

        /// <summary>
        /// 根据新车订单信息，生成任务
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="model">新车订单信息</param>
        /// <returns>返回任务ID</returns>
        public long InsertByOrder(SqlTransaction tran, Entities.OrderNewCarLog model)
        {
            Entities.OrderTask otModel = new Entities.OrderTask();
            if (model.OrderType == 1)
            {
                otModel.Source = 3;
            }
            else
            {
                otModel.Source = 1;//1新车，2置换，3试驾
            }
            otModel.TaskStatus = (int)TaskStatus.NoAllocation;
            otModel.RelationID = model.RecID;
            otModel.BGID = int.Parse(ConfigurationUtil.GetAppSettingValue("GenTaskDefaultGroupID"));
            otModel.AssignUserID = null;
            otModel.AssignTime = null;
            otModel.UserName = model.UserName;
            otModel.IsSelectDMSMember = null;
            otModel.Status = 0;
            otModel.SubmitTime = null;
            otModel.CreateTime = DateTime.Now;
            otModel.CreateUserID = null;
            otModel.NoDealerReasonID = -2;
            otModel.NoDealerReason = "";
            otModel.DealerID = model.DealerID;

            return Insert(tran, otModel);
        }

        /// <summary>
        /// 根据置换订单信息，生成任务
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="model">置换订单信息</param>
        /// <returns>返回任务ID</returns>
        public long InsertByOrder(SqlTransaction tran, Entities.OrderRelpaceCarLog model)
        {
            Entities.OrderTask otModel = new Entities.OrderTask();
            otModel.Source = 2;//1新车，2置换
            otModel.TaskStatus = (int)TaskStatus.NoAllocation;
            otModel.RelationID = model.RecID;
            otModel.BGID = int.Parse(ConfigurationUtil.GetAppSettingValue("GenTaskDefaultGroupID"));
            otModel.AssignUserID = null;
            otModel.AssignTime = null;
            otModel.UserName = model.UserName;
            otModel.IsSelectDMSMember = null;
            otModel.Status = 0;
            otModel.SubmitTime = null;
            otModel.CreateTime = DateTime.Now;
            otModel.CreateUserID = null;
            otModel.NoDealerReasonID = -2;
            otModel.NoDealerReason = "";
            otModel.DealerID = model.DealerID;

            return Insert(tran, otModel);
        }
    }
}

