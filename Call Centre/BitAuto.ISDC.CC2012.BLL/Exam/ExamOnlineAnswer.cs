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
    /// 业务逻辑类ExamOnlineAnswer 的摘要说明。
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
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetExamOnlineAnswer(QueryExamOnlineAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamOnlineAnswer.Instance.GetExamOnlineAnswer(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamOnlineAnswer.Instance.GetExamOnlineAnswer(new QueryExamOnlineAnswer(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamOnlineAnswer GetExamOnlineAnswer(long RecID)
        {

            return Dal.ExamOnlineAnswer.Instance.GetExamOnlineAnswer(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
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
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamOnlineAnswer model)
        {
            return Dal.ExamOnlineAnswer.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.ExamOnlineAnswer.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.ExamOnlineAnswer.Instance.Delete(sqltran, RecID);
        }

        #endregion



        #region add by qizq
        /// <summary>
        /// 取选择的选项
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
        /// 取小题得分
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
        /// 取某人总分
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
        /// 取考试id
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

