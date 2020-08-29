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
	/// 业务逻辑类GroupOrderTask 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:14 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class GroupOrderTask
	{
		#region Instance
		public static readonly GroupOrderTask Instance = new GroupOrderTask();
		#endregion

		#region Contructor
		protected GroupOrderTask()
		{}
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
		public DataTable GetGroupOrderTask(QueryGroupOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.GroupOrderTask.Instance.GetGroupOrderTask(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.GroupOrderTask.Instance.GetGroupOrderTask(new QueryGroupOrderTask(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.GroupOrderTask GetGroupOrderTask(long TaskID)
		{
			return Dal.GroupOrderTask.Instance.GetGroupOrderTask(TaskID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByTaskID(long TaskID)
		{
			QueryGroupOrderTask query = new QueryGroupOrderTask();
			query.TaskID = TaskID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetGroupOrderTask(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Insert(SqlTransaction sqltran, Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Insert(sqltran, model);
		}


        /// <summary>
        /// 根据团购订单信息，生成任务
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="model">团购订单信息</param>
        /// <returns>返回任务ID</returns>
        public long InsertByOrder(SqlTransaction tran, Entities.GroupOrderOrg model)
        {
            Entities.GroupOrderTask otModel = new Entities.GroupOrderTask();
            otModel.TaskStatus = (int)Entities.GroupTaskStatus.NoAllocation;
            otModel.OrderID = model.OrderID;
            otModel.BGID = int.Parse(ConfigurationUtil.GetAppSettingValue("GenTaskDefaultGroupID"));
            otModel.SCID = int.Parse(ConfigurationUtil.GetAppSettingValue("GenTaskDefaultCategoryID"));
            otModel.AssignUserID = null;
            otModel.AssignTime = null;
            otModel.SubmitTime = null;
            otModel.CreateTime = DateTime.Now;
            otModel.CreateUserID = null;
            otModel.LastUpdateTime = null;
            otModel.LastUpdateUserID = null;

            return Insert(tran, otModel);
        }
		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long TaskID)
		{
			
			return Dal.GroupOrderTask.Instance.Delete(TaskID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long TaskID)
		{
			
			return Dal.GroupOrderTask.Instance.Delete(sqltran, TaskID);
		}

		#endregion

        /// <summary>
        /// 取任务不同处理人
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiffassignuserid()
        {
            return Dal.GroupOrderTask.Instance.GetDiffassignuserid();
        }

        /// <summary>
        /// 取所有不同处理人
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
    }
}

