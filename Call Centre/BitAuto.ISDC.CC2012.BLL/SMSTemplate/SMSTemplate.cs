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
    /// ҵ���߼���SMSTemplate ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-12-23 06:17:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SMSTemplate
    {
        #region Instance
        public static readonly SMSTemplate Instance = new SMSTemplate();
        #endregion

        #region Contructor
        protected SMSTemplate()
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
        public DataTable GetSMSTemplate(QuerySMSTemplate query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSTemplate.Instance.GetSMSTemplate(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SMSTemplate.Instance.GetSMSTemplate(new QuerySMSTemplate(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ���ݵ�ǰ��ȡ��ǰ������Ȩ���µ�ģ�崴����
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetCreateUserID(int userid)
        {
            return Dal.SMSTemplate.Instance.GetCreateUserID(userid);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SMSTemplate GetSMSTemplate(int RecID)
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            return Dal.SMSTemplate.Instance.GetSMSTemplate(RecID);
        }

        #endregion

        #region IsExists

        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(Entities.SMSTemplate model)
        {
            Dal.SMSTemplate.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.SMSTemplate model)
        {
            Dal.SMSTemplate.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.SMSTemplate model)
        {
            return Dal.SMSTemplate.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SMSTemplate model)
        {
            return Dal.SMSTemplate.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            return Dal.SMSTemplate.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            return Dal.SMSTemplate.Instance.Delete(sqltran, RecID);
        }

        #endregion

    }
}

