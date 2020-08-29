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
	/// ҵ���߼���GroupOrderTask ��ժҪ˵����
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
		/// ���ղ�ѯ������ѯ
		/// </summary>
		/// <param name="query">��ѯ����</param>
		/// <param name="order">����</param>
		/// <param name="currentPage">ҳ��,-1����ҳ</param>
		/// <param name="pageSize">ÿҳ��¼��</param>
		/// <param name="totalCount">������</param>
		/// <returns>����</returns>
		public DataTable GetGroupOrderTask(QueryGroupOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.GroupOrderTask.Instance.GetGroupOrderTask(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.GroupOrderTask.Instance.GetGroupOrderTask(new QueryGroupOrderTask(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.GroupOrderTask GetGroupOrderTask(long TaskID)
		{
			return Dal.GroupOrderTask.Instance.GetGroupOrderTask(TaskID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
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
		/// ����һ������
		/// </summary>
		public int  Insert(Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public long Insert(SqlTransaction sqltran, Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Insert(sqltran, model);
		}


        /// <summary>
        /// �����Ź�������Ϣ����������
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="model">�Ź�������Ϣ</param>
        /// <returns>��������ID</returns>
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
		/// ����һ������
		/// </summary>
		public int Update(Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GroupOrderTask model)
		{
			return Dal.GroupOrderTask.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long TaskID)
		{
			
			return Dal.GroupOrderTask.Instance.Delete(TaskID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long TaskID)
		{
			
			return Dal.GroupOrderTask.Instance.Delete(sqltran, TaskID);
		}

		#endregion

        /// <summary>
        /// ȡ����ͬ������
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiffassignuserid()
        {
            return Dal.GroupOrderTask.Instance.GetDiffassignuserid();
        }

        /// <summary>
        /// ȡ���в�ͬ������
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

