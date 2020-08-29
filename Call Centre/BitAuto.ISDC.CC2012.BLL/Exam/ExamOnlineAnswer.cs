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
    /// ҵ���߼���ExamOnlineAnswer ��ժҪ˵����
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
    public class ExamOnlineAnswer
    {
        #region Instance
        public static readonly ExamOnlineAnswer Instance = new ExamOnlineAnswer();
        #endregion

        #region Contructor
        protected ExamOnlineAnswer()
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
        public DataTable GetExamOnlineAnswer(QueryExamOnlineAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamOnlineAnswer.Instance.GetExamOnlineAnswer(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamOnlineAnswer.Instance.GetExamOnlineAnswer(new QueryExamOnlineAnswer(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ExamOnlineAnswer GetExamOnlineAnswer(long RecID)
        {

            return Dal.ExamOnlineAnswer.Instance.GetExamOnlineAnswer(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryExamOnlineAnswer query = new QueryExamOnlineAnswer();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamOnlineAnswer(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.ExamOnlineAnswer.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.ExamOnlineAnswer.Instance.Delete(sqltran, RecID);
        }

        #endregion



        #region add by qizq
        /// <summary>
        /// ȡѡ���ѡ��
        /// </summary>
        /// <returns></returns>
        public string GetSelected(string EIID, string Type, string Personid, string BQID, string KLQID)
        {
            string sqlstr = "";
            DataTable dt = Dal.ExamOnlineAnswer.Instance.GetSelected(EIID, Type, Personid, BQID, KLQID);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sqlstr += dt.Rows[i]["ExamAnswer"].ToString() + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);
            }
            return sqlstr;
        }

        public string GetSelected(string EOLID, string BQID, string KLQID)
        {
            string sqlstr = "";
            DataTable dt = Dal.ExamOnlineAnswer.Instance.GetSelected(EOLID, BQID, KLQID);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sqlstr += dt.Rows[i]["ExamAnswer"].ToString() + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);
            }
            return sqlstr;
        }

        /// <summary>
        /// ȡС��÷�
        /// </summary>
        /// <returns></returns>
        public string Getfenshu(string EIID, string Type, string Personid, string BQID, string KLQID)
        {
            string sqlstr = "";
            DataTable dt = Dal.ExamOnlineAnswer.Instance.Getfenshu(EIID, Type, Personid, BQID, KLQID);
            if (dt != null && dt.Rows.Count > 0)
            {
                sqlstr = dt.Rows[0]["Score"].ToString();
            }
            return sqlstr;
        }

        /// <summary>
        /// ȡĳ���ܷ�
        /// </summary>
        /// <param name="EIID"></param>
        /// <param name="Type"></param>
        /// <param name="Personid"></param>
        /// <returns></returns>
        public string GetSumScore(string EIID, string Type, string Personid, out string Marking)
        {
            string sqlstr = "";
            DataTable dt = Dal.ExamOnlineAnswer.Instance.GetSumScore(EIID, Type, Personid);
            if (dt != null && dt.Rows.Count > 0)
            {
                sqlstr = dt.Rows[0]["SumScore"].ToString();
                Marking = dt.Rows[0]["IsMarking"].ToString();
            }
            else
            {
                Marking = "";
            }
            return sqlstr;
        }
        /// <summary>
        /// ȡ����id
        /// </summary>
        /// <param name="EIID"></param>
        /// <param name="Type"></param>
        /// <param name="Personid"></param>
        /// <returns></returns>
        public string GetEOLID(string EIID, string Type, string Personid)
        {
            string sqlstr = "";
            DataTable dt = Dal.ExamOnlineAnswer.Instance.GetSumScore(EIID, Type, Personid);
            if (dt != null && dt.Rows.Count > 0)
            {
                sqlstr = dt.Rows[0]["EOLID"].ToString();
            }
            return sqlstr;
        }
        #endregion
    }
}

