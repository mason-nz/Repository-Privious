using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���EmployeeAgent ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:02 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class EmployeeAgent
    {
        #region Instance
        public static readonly EmployeeAgent Instance = new EmployeeAgent();
        #endregion

        #region Contructor
        protected EmployeeAgent()
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
        public DataTable GetEmployeeAgent(QueryEmployeeAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(query, order, currentPage, pageSize, out totalCount);
        }
        public string GetAgentNum(string strUserId)
        {
            return Dal.EmployeeAgent.Instance.GetAgentNum(strUserId);
        }
        /// <summary>
        /// ���ݿͻ�UserID��ѯ����Ŷ������Ự��
        /// </summary>
        /// <param name="userid">�ͷ�UserID</param>
        /// <param name="MaxQueue">�ͷ����ͬʱ�Ŷ���</param>
        /// <param name="MaxDialogueN">�ͷ����ͬʱ�Ự��</param>
        /// <returns>true:�ͷ������ò�����false:ȱʡ����</returns>
        public bool GetMaxQueueDialogue(int userid, out int MaxQueue, out int MaxDialogueN)
        {
            MaxQueue = 0;
            MaxDialogueN = 0;
            string sMaxQueue = "", sMaxDialogueN = "";
            sMaxQueue = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("MaxQueue");
            sMaxDialogueN = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("MaxDialogueN");

            MaxQueue = Convert.ToInt32(sMaxQueue);
            MaxDialogueN = Convert.ToInt32(sMaxDialogueN);
            QueryEmployeeAgent model = new QueryEmployeeAgent();
            model.UserID = userid;
            int itemp = 0;
            DataTable dt = Dal.EmployeeAgent.Instance.GetEmployeeAgent(model, "", 1, 100, out itemp);
            if (dt == null)
            {
                return false;
            }
            if (int.TryParse(dt.Rows[0]["MaxQueue"].ToString(), out itemp))
            {
                MaxQueue = itemp;
            }

            if (int.TryParse(dt.Rows[0]["MaxDialogueN"].ToString(), out itemp))
            {
                MaxDialogueN = itemp;
            }

            return true;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(new QueryEmployeeAgent(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgent(int RecID)
        {

            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryEmployeeAgent query = new QueryEmployeeAgent();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetEmployeeAgent(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.EmployeeAgent.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.EmployeeAgent.Instance.Delete(sqltran, RecID);
        }

        #endregion

    }
}

