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
    /// 数据访问类KnowledgeCategory。
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
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
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
        /// 区分区域的查询
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
        /// 得到一个对象实体
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
        ///  增加一条数据
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
        ///  增加一条数据
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
        ///  更新一条数据
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
        /// 修改知识点分类
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
        /// 取节点下子节点以及该节点名称
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
        /// 取节点下子节点，且子节点不存在子节点
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
        /// 取节点下子节点，且子节点存在子节点
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
        /// 根据父名称 取得子节点列表
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
        ///  更新一条数据
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
        /// 删除一条数据
        /// </summary>
        public int Delete(int KCID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KCID", SqlDbType.Int,4)};
            parameters[0].Value = KCID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGECATEGORY_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
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
                msg = "{msg:'操作成功！'}";
            }
            else
            {
                msg = "{msg:'操作失败！'}";
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
                msg = "{msg:'操作成功！'}";
            }
            else
            {
                msg = "{msg:'操作失败！'}";
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
                msg = "{msg:'操作成功！'}";
            }
            else
            {
                msg = "{msg:'操作失败！'}";
            }
        }
        public DataTable BindKnowledgeCategory(Entities.QueryKnowledgeCategory query)
        {
            DataTable dt = new DataTable();
            //同一个父节点下的孩子节点排序
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
            //同一个父节点下的孩子节点排序
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
                msg = "{msg:'操作成功！'}";
            }
            else
            {
                msg = "{msg:'操作失败！'}";
            }
        }

        public void SortNumUpOrDown(string sortid, string type, string info, out string msg)
        {
            msg = "";
            try
            {
                string[] infos = info.Split('@');
                Dictionary<int, int> SortInofList = new Dictionary<int, int>();
                //解析新的顺序信息（顺序id-数据id）
                string[] array = infos[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in array)
                {
                    string[] items = item.Split(':');
                    SortInofList.Add(CommonFunction.ObjectToInteger(items[0]), CommonFunction.ObjectToInteger(items[1]));
                }
                //解析数据库保存的顺序信息（数据id-顺序id）
                array = infos[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, int> KcidSort = new Dictionary<int, int>();
                foreach (string item in array)
                {
                    string[] items = item.Split(':');
                    KcidSort.Add(CommonFunction.ObjectToInteger(items[0]), CommonFunction.ObjectToInteger(items[1]));
                }
                //移动数据
                int SortId = CommonFunction.ObjectToInteger(sortid);
                int nextid = 0;
                //上移
                if (type == "up")
                {
                    nextid = SortId - 1;
                }
                //下移
                else if (type == "down")
                {
                    nextid = SortId + 1;
                }
                if (SortInofList.ContainsKey(nextid))
                {
                    //交换元素
                    int x = SortInofList[nextid];
                    SortInofList[nextid] = SortInofList[SortId];
                    SortInofList[SortId] = x;
                }
                else
                {
                    msg = "参数错误";
                    return;
                }
                //数据更新
                foreach (int sid in SortInofList.Keys)
                {
                    int kcid = SortInofList[sid];
                    //新计算的顺序id不等于数据库已有的id，更新数据库
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

