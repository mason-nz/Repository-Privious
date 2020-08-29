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
    /// ҵ���߼���OrderTask ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetOrderTask(QueryOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderTask.Instance.GetOrderTask(query, order, currentPage, pageSize, out totalCount);
        }


        //add by qizq 2012-9-25 �������������б�
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetOrderTaskList(QueryOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = BLL.Util.GetLoginUserID();
            return Dal.OrderTask.Instance.GetOrderTaskList(query, order, currentPage, pageSize, out totalCount, userid);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OrderTask.Instance.GetOrderTask(new QueryOrderTask(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ȡ����ͬ������ add by qizq 2013-2-21
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiffassignuserid()
        {
            return Dal.OrderTask.Instance.GetDiffassignuserid();
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.OrderTask GetOrderTask(long TaskID)
        {

            return Dal.OrderTask.Instance.GetOrderTask(TaskID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
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
        /// ����һ������
        /// </summary>
        public int Insert(Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderTask model)
        {
            return Dal.OrderTask.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long TaskID)
        {

            return Dal.OrderTask.Instance.Delete(TaskID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {

            return Dal.OrderTask.Instance.Delete(sqltran, TaskID);
        }

        #endregion

        /// <summary>
        /// ȡ���в�ͬ������ by qizq
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
        /// �����³�������Ϣ����������
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="model">�³�������Ϣ</param>
        /// <returns>��������ID</returns>
        public long InsertByOrder(SqlTransaction tran, Entities.OrderNewCarLog model)
        {
            Entities.OrderTask otModel = new Entities.OrderTask();
            if (model.OrderType == 1)
            {
                otModel.Source = 3;
            }
            else
            {
                otModel.Source = 1;//1�³���2�û���3�Լ�
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
        /// �����û�������Ϣ����������
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="model">�û�������Ϣ</param>
        /// <returns>��������ID</returns>
        public long InsertByOrder(SqlTransaction tran, Entities.OrderRelpaceCarLog model)
        {
            Entities.OrderTask otModel = new Entities.OrderTask();
            otModel.Source = 2;//1�³���2�û�
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

