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
    /// ҵ���߼���AutoCall_ProjectInfo ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-09-14 09:57:58 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AutoCall_ProjectInfo
    {
        #region Instance
        public static readonly AutoCall_ProjectInfo Instance = new AutoCall_ProjectInfo();
        #endregion

        #region Contructor
        protected AutoCall_ProjectInfo()
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
        public DataTable GetAutoCall_ProjectInfo(QueryAutoCall_ProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AutoCall_ProjectInfo.Instance.GetAutoCall_ProjectInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.AutoCall_ProjectInfo.Instance.GetAutoCall_ProjectInfo(new QueryAutoCall_ProjectInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        public DataTable GetAutoCallProjectInfo(string strName, string BGID, string SCID, string strStatus, string strACStatus, int userid, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AutoCall_ProjectInfo.Instance.GetAutoCall_ProjectInfo(strName, BGID, SCID, strStatus, strACStatus, userid, currentPage, pageSize, out totalCount);
        }

        public DataSet GetOutCall400Number()
        {
            return Dal.AutoCall_ProjectInfo.Instance.GetOutCall400Number();
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.AutoCall_ProjectInfo GetAutoCall_ProjectInfo(long ProjectID)
        {

            return Dal.AutoCall_ProjectInfo.Instance.GetAutoCall_ProjectInfo(ProjectID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByProjectID(long ProjectID)
        {
            QueryAutoCall_ProjectInfo query = new QueryAutoCall_ProjectInfo();
            query.ProjectID = ProjectID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAutoCall_ProjectInfo(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.AutoCall_ProjectInfo model)
        {
            Dal.AutoCall_ProjectInfo.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.AutoCall_ProjectInfo model)
        {
            Dal.AutoCall_ProjectInfo.Instance.Insert(sqltran, model);
        }

        public void InsertAutoProBatch(string strProjectIDs, string skillGroupID, string CallID, int UserId, out string errorMsg)
        {
            Dal.AutoCall_ProjectInfo.Instance.InsertAutoProBatch(strProjectIDs, skillGroupID, CallID, UserId, out errorMsg);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.AutoCall_ProjectInfo model)
        {
            return Dal.AutoCall_ProjectInfo.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.AutoCall_ProjectInfo model)
        {
            return Dal.AutoCall_ProjectInfo.Instance.Update(sqltran, model);
        }

        public int Update(string strProjectID, string strSkillID, string strCallNum)
        {
            return Dal.AutoCall_ProjectInfo.Instance.Update(strProjectID, strSkillID, strCallNum);
        }

        public int UpdateAutoProjectStatus(string strProjectID, ProjectACStatus status)
        {
            return Dal.AutoCall_ProjectInfo.Instance.UpdateAutoProjectStatus(strProjectID, status);
        }

        public int InportAutoCallTask(string strProjectID)
        {
            int userid = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
            return Dal.AutoCall_ProjectInfo.Instance.InportAutoCallTask(strProjectID, userid);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long ProjectID)
        {

            return Dal.AutoCall_ProjectInfo.Instance.Delete(ProjectID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long ProjectID)
        {

            return Dal.AutoCall_ProjectInfo.Instance.Delete(sqltran, ProjectID);
        }

        #endregion

    }
}

