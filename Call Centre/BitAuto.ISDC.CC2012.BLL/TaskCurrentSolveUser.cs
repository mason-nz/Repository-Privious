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
    /// ҵ���߼���TaskCurrentSolveUser ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TaskCurrentSolveUser
    {
        #region Instance
        public static readonly TaskCurrentSolveUser Instance = new TaskCurrentSolveUser();
        #endregion

        #region Contructor
        protected TaskCurrentSolveUser()
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
        public DataTable GetTaskCurrentSolveUser(QueryTaskCurrentSolveUser query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(new QueryTaskCurrentSolveUser(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.TaskCurrentSolveUser GetTaskCurrentSolveUser(int RecID)
        {

            return Dal.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryTaskCurrentSolveUser query = new QueryTaskCurrentSolveUser();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTaskCurrentSolveUser(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.TaskCurrentSolveUser model)
        {
            return Dal.TaskCurrentSolveUser.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.TaskCurrentSolveUser model)
        {
            return Dal.TaskCurrentSolveUser.Instance.Update(model);
        }
        public int UpdateByTaskID(string taskID)
        {
            return Dal.TaskCurrentSolveUser.Instance.UpdateByTaskID(taskID);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.TaskCurrentSolveUser.Instance.Delete(RecID);
        }

        #endregion

    }
}

