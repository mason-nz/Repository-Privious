using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ���ݷ�����ComSentence��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ComSentence : DataBase
    {
        #region Instance
        public static readonly ComSentence Instance = new ComSentence();
        #endregion

        #region const
        private const string P_COMSENTENCE_SELECT = "p_ComSentence_Select";
        private const string P_COMSENTENCE_SELECTLIST = "p_ComSentence_SelectList";
        private const string P_COMSENTENCE_INSERT = "p_ComSentence_Insert";
        private const string P_COMSENTENCE_UPDATE = "p_ComSentence_Update";
        private const string P_COMSENTENCE_DELETE = "p_ComSentence_Delete";
        #endregion

        #region Contructor
        protected ComSentence()
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
        public DataTable GetComSentence(QueryComSentence query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.LTID != Constant.INT_INVALID_VALUE)
            {
                where += " and LTID=" + query.LTID;
            }

            if (query.CSID != Constant.INT_INVALID_VALUE)
            {
                where += " and CSID=" + query.CSID;
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " and Name='" + StringHelper.SqlFilter(query.Name) + "'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_COMSENTENCE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public DataTable GetComSentenceList(QueryComSentence query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            //����Ȩ�ޣ���������
            if (!string.IsNullOrEmpty(query.DataRight))
            {
                where += " and lt.AreaType=" + query.DataRight;
            }
          
            if (query.LTID != Constant.INT_INVALID_VALUE)
            {
                where += " and cs.LTID=" + query.LTID;
            }

            if (query.CSID != Constant.INT_INVALID_VALUE)
            {
                where += " and cs.CSID=" + query.CSID;
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " and cs.Name LIKE '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }

            if (query.LTName != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.Name LIKE '%" + StringHelper.SqlFilter(query.LTName) + "%'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_COMSENTENCE_SELECTLIST, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public DataTable GetAllLableWithCM(int bgid)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@bgid", SqlDbType.Int, 4)
					};

            parameters[0].Value = bgid;


            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "GetAllLableWithCM", parameters);

            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ComSentence GetComSentence(int CSID)
        {
            QueryComSentence query = new QueryComSentence();
            query.CSID = CSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetComSentence(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleComSentence(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ComSentence LoadSingleComSentence(DataRow row)
        {
            Entities.ComSentence model = new Entities.ComSentence();

            if (row["CSID"].ToString() != "")
            {
                model.CSID = int.Parse(row["CSID"].ToString());
            }
            if (row["LTID"].ToString() != "")
            {
                model.LTID = int.Parse(row["LTID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["SortNum"].ToString() != "")
            {
                model.SortNum = int.Parse(row["SortNum"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.ComSentence model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.LTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.SortNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_COMSENTENCE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ComSentence model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.LTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.SortNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_COMSENTENCE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.ComSentence model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.LTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.SortNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_COMSENTENCE_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ComSentence model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.LTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.SortNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_COMSENTENCE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int CSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)};
            parameters[0].Value = CSID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_COMSENTENCE_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int CSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)};
            parameters[0].Value = CSID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_COMSENTENCE_DELETE, parameters);
        }
        #endregion


        /// �����ƶ�����
        /// <summary>
        /// �����ƶ�����
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="sortnum"></param>
        /// <param name="type">1��-1��</param>
        /// <returns></returns>
        public bool MoveUpOrDown(Entities.QueryComSentence query, int sortnum, int type)
        {
            int recid = query.CSID;
            int next_recid = 0, next_sortnum = 0;
            string sql = "";
            string where = "";
            //����Ȩ�ޣ���������
            if (!string.IsNullOrEmpty(query.DataRight))
            {
                where += " and lt.AreaType=" + query.DataRight;
            }
            if (query.CSName != Constant.STRING_INVALID_VALUE)
            {
                where += " and cs.Name LIKE '%"+ StringHelper.SqlFilter(query.CSName) + "%' ";
            }

            if (query.LTName != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.Name LIKE '%" + StringHelper.SqlFilter(query.LTName) + "%' ";
            }

            //����
            if (type > 0)
            {
                //��ȡǰһλ����
                //sql = "select top 1 csid,sortnum from dbo.ComSentence where status=0 and sortnum<" + sortnum + " order by sortnum desc";
                sql = "select top 1 cs.csid,cs.sortnum from dbo.ComSentence cs INNER JOIN dbo.LabelTable lt ON cs.LTID = lt.LTID where cs.status=0 and cs.sortnum<" + sortnum + " "+ where + " order by cs.sortnum desc";
            }
            //����
            else if (type < 0)
            {
                //��ȡ��һλ����
                //sql = "select top 1 csid,sortnum from dbo.ComSentence where status=0 and sortnum>" + sortnum + " order by sortnum asc";
                sql = "select top 1 cs.csid,cs.sortnum from dbo.ComSentence cs INNER JOIN dbo.LabelTable lt ON cs.LTID = lt.LTID where cs.status=0 and cs.sortnum>" + sortnum + " " + where + " order by cs.sortnum asc";
            }
            else return false;
            //��ѯ����
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                next_recid = CommonFunc.ObjectToInteger(dt.Rows[0]["csid"]);
                next_sortnum = CommonFunc.ObjectToInteger(dt.Rows[0]["sortnum"]);
            }
            //��������
            string sql1 = "update dbo.ComSentence set sortnum=" + next_sortnum + " where csid=" + recid;
            string sql2 = "update dbo.ComSentence set sortnum=" + sortnum + " where csid=" + next_recid;
            int i = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql1);
            i += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql2);
            return i == 2;
        }

    }
}

