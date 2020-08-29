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
    /// ҵ���߼���UserGroupDataRigth ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-14 11:25:36 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserGroupDataRigth
    {
        #region Instance
        public static readonly UserGroupDataRigth Instance = new UserGroupDataRigth();
        #endregion

        #region Contructor
        protected UserGroupDataRigth()
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
        public DataTable GetUserGroupDataRigth(QueryUserGroupDataRigth query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ��ѯ�û��µ��û���
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigthByUserID(int userId)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId, null);
        }
        /// <summary>
        /// ��ѯ�û��µ��û���
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigthByUserID(int userId, string where)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId, where);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(new QueryUserGroupDataRigth(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.UserGroupDataRigth GetUserGroupDataRigth(int RecID)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(RecID);
        }
        /// <summary>
        /// ��ȡ�û��������ַ���
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupNamesStr(int userId)
        {
            return Dal.UserGroupDataRigth.Instance.GetUserGroupNamesStr(userId);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryUserGroupDataRigth query = new QueryUserGroupDataRigth();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserGroupDataRigth(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.UserGroupDataRigth model)
        {
            return Dal.UserGroupDataRigth.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.UserGroupDataRigth.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.UserGroupDataRigth.Instance.Delete(sqltran, RecID);
        }
        /// <summary>
        /// ͨ���û�IDɾ������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteByUserID(int userId)
        {
            return Dal.UserGroupDataRigth.Instance.DeleteByUserID(userId);
        }
        #endregion

        /// <summary>
        /// ���ݱ����ƣ������ֶ����ƣ���ǰ���ֶ����ƣ���ǰ��¼��id��ƴ������Ȩ������
        /// </summary>
        /// <param name="tablename">�����ƣ�������</param>
        /// <param name="BgIDFileName">�����ֶ�����</param>
        /// <param name="UserIDFileName">����Ȩ���ֶ�</param>
        /// <param name="UserID">��ǰ��id</param>
        /// <returns></returns>
        public string GetSqlRightstr(string tablename, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return Dal.UserGroupDataRigth.Instance.GetSqlRightstr(tablename, BgIDFileName, UserIDFileName, UserID);
        }

        /// <summary>
        /// ƴ������Ȩ�ޣ���BGID��UserID��������ʱ
        /// </summary>
        /// <param name="tablenameBgID"></param>
        /// <param name="tablenameUserID"></param>
        /// <param name="BgIDFileName"></param>
        /// <param name="UserIDFileName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetSqlRightstr(string tablenameBgID, string tablenameUserID, string BgIDFileName, string UserIDFileName, int UserID)
        {
            return Dal.UserGroupDataRigth.Instance.GetSqlRightstr(tablenameBgID, tablenameUserID, BgIDFileName, UserIDFileName, UserID);
        }

        /// <summary>
        /// �����б�ƴ������Ȩ�ޣ�������Ȩ���жϣ����˵��ж��⻹��������������������紦����Ҳ���Բ鿴����һ���ַ��� add lxw 13.10.12
        /// </summary>
        /// <param name="tablenameBgID"></param>
        /// <param name="tablenameUserID"></param>
        /// <param name="BgIDFileName"></param>
        /// <param name="UserIDFileName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetSqlRightstrByOrderWhere(string tablename, string BgIDFileName, string UserIDFileName, int UserID, string whereStr)
        {
            return Dal.UserGroupDataRigth.Instance.GetSqlRightstrByOrderWhere(tablename, BgIDFileName, UserIDFileName, UserID, whereStr);
        }


        public string GetGroupStr(int userid)
        {
            return Dal.UserGroupDataRigth.Instance.GetGroupStr(userid);
        }

        /// ɾ����Ա�ͷ�������Բ��ϵĴ�������
        /// <summary>
        /// ɾ����Ա�ͷ�������Բ��ϵĴ�������
        /// </summary>
        /// <returns></returns>
        public int DeleteErrorData(int userid)
        {
            return Dal.UserGroupDataRigth.Instance.DeleteErrorData(userid);
        }
    }
}

