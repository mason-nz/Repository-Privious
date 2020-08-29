using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ���ݷ�����KnowledgeCategory��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KnowledgeCategory : DataBase
    {
        #region Instance
        public static readonly KnowledgeCategory Instance = new KnowledgeCategory();
        #endregion

        #region const
        private const string P_KNOWLEDGECATEGORY_SELECT = "p_KnowledgeCategory_Select";
        private const string P_KNOWLEDGECATEGORY_INSERT = "p_KnowledgeCategory_Insert";
        private const string P_KNOWLEDGECATEGORY_UPDATE = "p_KnowledgeCategory_Update";
        private const string P_KNOWLEDGECATEGORY_DELETE = "p_KnowledgeCategory_Delete";
        private const string P_KNOWLEDGECATEGORY_SELECTEXCEPTDEL = "p_KnowledgeCategory_SelectaExceptDel";
        #endregion

        #region Contructor
        protected KnowledgeCategory()
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
        public DataTable GetKnowledgeCategory(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.KCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KCID=" + query.KCID;
            }
            if (query.Pid != Constant.INT_INVALID_VALUE)
            {
                where += " AND Pid=" + query.Pid;
            }
            if (query.Level != Constant.INT_INVALID_VALUE)
            {
                where += " AND Level=" + query.Level;
            }
            //if (query.Regionid != Constant.INT_INVALID_VALUE)
            //{
            //    where += " AND regionid =" + query.Regionid;
            //}
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// ��������Ĳ�ѯ
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKnowledgeCategoryWithRegion(QueryKnowledgeCategory query, int nUserID, string order, int currentPage, int pageSize, out int totalCount)
        {
          //  string where = string.Format(" and RegionID IN(SELECT RegionID FROM dbo.EmployeeAgent WHERE UserID={0})", nUserID);
            string where = string.Empty;
            if (query.KCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KCID=" + query.KCID;
            }
            if (query.Pid != Constant.INT_INVALID_VALUE)
            {
                where += " AND Pid=" + query.Pid;
            }
            if (query.Level != Constant.INT_INVALID_VALUE)
            {
                where += " AND Level=" + query.Level;
            }
            //if (query.Regionid != Constant.INT_INVALID_VALUE)
            //{
            //    where += " AND regionid =" + query.Regionid;
            //}
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        public DataTable GetKnowledgeCategoryForSearch(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            if (query.KCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KCID=" + query.KCID;
            }
            if (query.Pid != Constant.INT_INVALID_VALUE)
            {
                where += " AND Pid=" + query.Pid;
            }
            if (query.Level != Constant.INT_INVALID_VALUE)
            {
                where += " AND Level=" + query.Level;
            }
            //if (query.Regionid != Constant.INT_INVALID_VALUE)
            //{
            //    where += " AND regionid =" + query.Regionid;
            //}

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_SELECTEXCEPTDEL, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.KnowledgeCategory GetKnowledgeCategory(int KCID)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.KCID = KCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleKnowledgeCategory(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.KnowledgeCategory LoadSingleKnowledgeCategory(DataRow row)
        {
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();

            if (row["KCID"].ToString() != "")
            {
                model.KCID = int.Parse(row["KCID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["Level"].ToString() != "")
            {
                model.Level = int.Parse(row["Level"].ToString());
            }
            if (row["Pid"].ToString() != "")
            {
                model.Pid = int.Parse(row["Pid"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.KnowledgeCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Level;
            parameters[3].Value = model.Pid;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KnowledgeCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Level;
            parameters[3].Value = model.Pid;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.KnowledgeCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.KCID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Level;
            parameters[3].Value = model.Pid;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_UPDATE, parameters);
        }
        /// <summary>
        /// �޸�֪ʶ�����
        /// </summary>
        /// <param name="kcid"></param>
        /// <param name="klid"></param>
        /// <returns></returns>
        public int Update(string kcid, string klid)
        {
            string sqlstr = "update knowledgelib set kcid=@kcid where klid=@klid";
            SqlParameter[] parameters = {
					new SqlParameter("@kcid", SqlDbType.Int,4),
					new SqlParameter("@klid", SqlDbType.Int,4)};
            parameters[0].Value = Convert.ToInt32(kcid);
            parameters[1].Value = Convert.ToInt32(klid);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }

        /// <summary>
        /// ȡ�ڵ����ӽڵ��Լ��ýڵ�����
        /// </summary>
        /// <param name="KCID"></param>
        /// <param name="Parentname"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPID(int KCID, string Parentname)
        {
            string sqstr = "select *,@Parentname as 'ParentName' from KnowledgeCategory where Pid=@KCID";
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Parentname", SqlDbType.VarChar)};
            parameters[0].Value = KCID;
            parameters[1].Value = Parentname;
            DataTable dt = null;
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqstr, parameters).Tables[0];
            return dt;
        }
        /// <summary>
        /// ȡ�ڵ����ӽڵ㣬���ӽڵ㲻�����ӽڵ�
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPIDNotSon(int KCID)
        {
            string sqstr = "select distinct a.kcid,a.name from dbo.KnowledgeCategory a left join KnowledgeCategory b on a.kcid=b.pid where a.pid=@KCID and b.pid is null";
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4)};
            parameters[0].Value = KCID;
            DataTable dt = null;
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqstr, parameters).Tables[0];
            return dt;
        }

        /// <summary>
        /// ȡ�ڵ����ӽڵ㣬���ӽڵ�����ӽڵ�
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPIDHaveSon(int KCID)
        {
            string sqstr = "select distinct a.kcid,a.name from dbo.KnowledgeCategory a left join KnowledgeCategory b on a.kcid=b.pid where a.pid=@KCID and b.pid is not null";
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4)};
            parameters[0].Value = KCID;
            DataTable dt = null;
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqstr, parameters).Tables[0];
            return dt;
        }

        /// <summary>
        /// ���ݸ����� ȡ���ӽڵ��б�
        /// </summary>
        /// <param name="KCID"></param>
        /// <param name="Parentname"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPName(string Parentname, int RegionID)
        {
            //string sqstr = "SELECT * FROM dbo.KnowledgeCategory t1 WHERE t1.Status=0 AND t1.Pid = (SELECT TOP 1 KCID FROM dbo.KnowledgeCategory t2 WHERE t2.Name='" + StringHelper.SqlFilter(Parentname) + "' AND t2.RegionID=" + RegionID + " AND t2.Status=0)";
            string sqstr = "SELECT * FROM dbo.KnowledgeCategory t1 WHERE t1.Status=0 AND t1.Pid = (SELECT TOP 1 KCID FROM dbo.KnowledgeCategory t2 WHERE t2.Name='" + StringHelper.SqlFilter(Parentname) + "' AND t2.Status=-3)";

            DataTable dt = null;
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqstr).Tables[0];
            return dt;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KnowledgeCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.KCID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Level;
            parameters[3].Value = model.Pid;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int KCID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4)};
            parameters[0].Value = KCID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int KCID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4)};
            parameters[0].Value = KCID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_DELETE, parameters);
        }
        #endregion


        public void DeleteKnowledgeCategory(Entities.KnowledgeCategory model, out string msg)
        {
            int count = 0;
            string strSelect = "update KnowledgeCategory set Status=-1  WHERE kcid='" + model.KCID + "' ";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteNonQuery();
            }
            if (count > 0)
            {
                msg = "{msg:'�����ɹ���'}";
            }
            else
            {
                msg = "{msg:'����ʧ�ܣ�'}";
            }
        }
        public int GetCountTheKnowledgeCategoryUsed(Entities.KnowledgeCategory model)
        {
            object count = 0;
            string strSelect = "SELECT COUNT(1) FROM KnowledgeLib WHERE kcid='" + model.KCID + "' ";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteScalar();
            }
            if (count != null)
            {
                return Convert.ToInt32(count);
            }
            else
            {
                return 0;
            }
        }

        public void InsertKnowledgeCategory(Entities.KnowledgeCategory model, out string msg)
        {
            int count = 0;
            string strSelect = "INSERT INTO KnowledgeCategory (Name,Level,Pid,RegionID) VALUES ('" + StringHelper.SqlFilter(model.Name) + "','" + model.Level + "','" + model.Pid + "','" + model.Regionid + "')";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteNonQuery();
            }
            if (count > 0)
            {
                msg = "{msg:'�����ɹ���'}";
            }
            else
            {
                msg = "{msg:'����ʧ�ܣ�'}";
            }
        }

        public void UpdateKnowledgeCategory(Entities.KnowledgeCategory model, out string msg)
        {
            int count = 0;
            string strSelect = "update KnowledgeCategory set name = '" + StringHelper.SqlFilter(model.Name) + "'  WHERE kcid='" + model.KCID + "' ";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteNonQuery();
            }
            if (count > 0)
            {
                msg = "{msg:'�����ɹ���'}";
            }
            else
            {
                msg = "{msg:'����ʧ�ܣ�'}";
            }
        }
        public DataTable BindKnowledgeCategory(Entities.QueryKnowledgeCategory query)
        {
            DataTable dt = new DataTable();
            //ͬһ�����ڵ��µĺ��ӽڵ�����
            string strSelect = "SELECT * FROM KnowledgeCategory WHERE Status<>-1 AND pid='"
                + query.Pid + "' AND Level='"
                + query.Level + "'  " +
                "ORDER BY ISNULL(SortNum,99999),KCID";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(strSelect, conn);
                adp.Fill(dt);
            }
            return dt;
        }

        public DataTable BindChildrenCategoryInfo(Entities.QueryKnowledgeCategory query)
        {
            DataTable dt = new DataTable();
            //ͬһ�����ڵ��µĺ��ӽڵ�����
            string strSelect =
                "SELECT  parentName =  ISNULL((SELECT name FROM KnowledgeCategory WHERE KCID=aa.Pid),'') " +
                ",aa.Name,aa.Status,aa.KCID,Pid,ISNULL(aa.SortNum,-1) AS SortNum " +
                "FROM    KnowledgeCategory aa " +
                "WHERE  aa.Status<>-1 AND aa.pid='"
                + query.Pid + "' AND aa.Level='"
                + query.Level + "'";

            if (!string.IsNullOrEmpty(query.Name))
            {
                strSelect += " and aa.name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            strSelect += " ORDER BY ISNULL(aa.SortNum,99999),aa.KCID";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(strSelect, conn);
                adp.Fill(dt);
            }
            return dt;
        }

        public int UpdateKnowledgeCategoryStatus(Entities.KnowledgeCategory model)
        {
            int count = 0;
            string strSelect = "update KnowledgeCategory set Status='" + model.Status + "'  WHERE kcid='" + model.KCID + "' ";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteNonQuery();
            }
            return count;
        }

        public int GetKnowledgeCategoryStatusByKCID(int KCID)
        {
            object count = 0;
            string strSelect = "SELECT STATUS FROM KnowledgeCategory WHERE kcid='" + KCID + "' ";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteScalar();
            }
            if (count != null)
            {
                return Convert.ToInt32(count);
            }
            else
            {
                return -5;
            }
        }

        public DataTable GetAllKnowledgeCategory()
        {
            DataTable dt = new DataTable(); ;
            string strSelect = "SELECT KCID,Pid FROM KnowledgeCategory WHERE Status<>-1";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(strSelect, conn);
                adp.Fill(dt);
            }
            return dt;
        }

        public int GetNotDelStatusNum(string KCIDS)
        {

            object count = 0;
            string strSelect = "SELECT COUNT(1) FROM KnowledgeLib WHERE KCID IN (" + Dal.Util.SqlFilterByInCondition(KCIDS) + ") AND Status<>4";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteScalar();
            }
            if (count != null)
            {
                return Convert.ToInt32(count);
            }
            else
            {
                return 0;
            }
        }

        public void DeleteKnowledgeCategoryAndChildren(string kcids, out string msg)
        {
            int count = 0;
            string strSelect = "update KnowledgeCategory set Status=-1  WHERE kcid in (" + Dal.Util.SqlFilterByInCondition(kcids) + ")";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(strSelect, conn);
                count = comm.ExecuteNonQuery();
            }
            if (count > 0)
            {
                msg = "{msg:'�����ɹ���'}";
            }
            else
            {
                msg = "{msg:'����ʧ�ܣ�'}";
            }
        }

        public void SortNumUpOrDown(string sortid, string type, string info, out string msg)
        {
            msg = "";
            try
            {
                string[] infos = info.Split('@');
                Dictionary<int, int> SortInofList = new Dictionary<int, int>();
                //�����µ�˳����Ϣ��˳��id-����id��
                string[] array = infos[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in array)
                {
                    string[] items = item.Split(':');
                    SortInofList.Add(CommonFunction.ObjectToInteger(items[0]), CommonFunction.ObjectToInteger(items[1]));
                }
                //�������ݿⱣ���˳����Ϣ������id-˳��id��
                array = infos[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, int> KcidSort = new Dictionary<int, int>();
                foreach (string item in array)
                {
                    string[] items = item.Split(':');
                    KcidSort.Add(CommonFunction.ObjectToInteger(items[0]), CommonFunction.ObjectToInteger(items[1]));
                }
                //�ƶ�����
                int SortId = CommonFunction.ObjectToInteger(sortid);
                int nextid = 0;
                //����
                if (type == "up")
                {
                    nextid = SortId - 1;
                }
                //����
                else if (type == "down")
                {
                    nextid = SortId + 1;
                }
                if (SortInofList.ContainsKey(nextid))
                {
                    //����Ԫ��
                    int x = SortInofList[nextid];
                    SortInofList[nextid] = SortInofList[SortId];
                    SortInofList[SortId] = x;
                }
                else
                {
                    msg = "��������";
                    return;
                }
                //���ݸ���
                foreach (int sid in SortInofList.Keys)
                {
                    int kcid = SortInofList[sid];
                    //�¼����˳��id���������ݿ����е�id���������ݿ�
                    if (sid != KcidSort[kcid])
                    {
                        UpdateSortNum(kcid, sid);
                    }
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void UpdateSortNum(int kcid, int sortid)
        {
            string sql = @"UPDATE KnowledgeCategory
                            SET SortNum=" + sortid + @"
                            WHERE KCID=" + kcid;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}

