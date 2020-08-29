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
    /// ҵ���߼���ExamOnline ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:16 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamOnline
    {
        #region Instance
        public static readonly ExamOnline Instance = new ExamOnline();
        #endregion

        #region Contructor
        protected ExamOnline()
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
        public DataTable GetExamOnline(QueryExamOnline query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamOnline.Instance.GetExamOnline(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetExamScoreManage(ExamScoreManageQuery query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string userid = BLL.Util.GetLoginUserID().ToString();
            return Dal.ExamOnline.Instance.GetExamScoreManage(query, order, currentPage, pageSize, out totalCount, userid);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamOnline.Instance.GetExamOnline(new QueryExamOnline(), string.Empty, 1, 1000000, out totalCount);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ExamOnline GetExamOnline(long EOLID)
        {

            return Dal.ExamOnline.Instance.GetExamOnline(EOLID);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByEOLID(long EOLID)
        {
            QueryExamOnline query = new QueryExamOnline();
            query.EOLID = EOLID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamOnline(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Insert(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Insert(sqltran, model);
        }
        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Update(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            model = SaveBgid(model);
            return Dal.ExamOnline.Instance.Update(sqltran, model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long EOLID)
        {

            return Dal.ExamOnline.Instance.Delete(EOLID);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long EOLID)
        {

            return Dal.ExamOnline.Instance.Delete(sqltran, EOLID);
        }
        #endregion

        #region �洢��������
        /// <summary>
        /// �洢��������
        /// ǿ�
        /// 2014-10-11
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Entities.ExamOnline SaveBgid(Entities.ExamOnline model)
        {
            Entities.EmployeeAgent agent = EmployeeAgent.Instance.GetEmployeeAgentByUserID(model.ExamPersonID);
            if (agent != null)
            {
                model.BGID = agent.BGID;
            }
            return model;
        }
        #endregion
    }
}

