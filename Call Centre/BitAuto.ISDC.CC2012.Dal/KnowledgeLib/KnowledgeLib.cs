using System;
using System.Data;
using System.Text;
using System.Linq;
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
    /// 数据访问类KnowledgeLib。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:10 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KnowledgeLib : DataBase
    {
        #region Instance
        public static readonly KnowledgeLib Instance = new KnowledgeLib();
        #endregion

        #region const
        private const string P_KNOWLEDGELIB_SELECT = "p_KnowledgeLib_Select";
        private const string P_KNOWLEDGELIB_INSERT = "p_KnowledgeLib_Insert";
        private const string P_KNOWLEDGELIB_UPDATE = "p_KnowledgeLib_Update";
        private const string P_KNOWLEDGELIB_DELETE = "p_KnowledgeLib_Delete";
        #endregion

        #region Contructor
        protected KnowledgeLib()
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
        public DataTable GetKnowledgeLib(QueryKnowledgeLib query, string order, int currentPage, int pageSize, out int totalCount, string wherePlus = "")
        {
            string where = wherePlus;

            where += getCommonWhere(1, query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGELIB_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /*
        //在知识点管理里支持区域，暂时未实现。
        public DataTable GetKnowledgeLibManage(QueryKnowledgeLib query, string order, int currentPage, int pageSize, out int totalCount, string wherePlus = "")
        {
            string where = wherePlus;


            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.Title like '%" + StringHelper.SqlFilter(query.Title) + "%'";
            }
            if (query.UnRead != Constant.STRING_INVALID_VALUE)  //  未读
            {
                if (query.UnRead == string.Empty && query.UserID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND KLID NOT IN ( SELECT KLReadTag.KLID FROM KLReadTag WHERE KLReadTag.KLID=KnowledgeLib.KLID AND ReadTag=1 AND UserID=" + query.UserID + ")";
                }
            }


            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:00:00'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            if (query.MBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.LastModifyTime>='" + StringHelper.SqlFilter(query.MBeginTime) + " 0:00:00'";
            }
            if (query.MEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.LastModifyTime<='" + StringHelper.SqlFilter(query.MEndTime) + " 23:59:59'";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.CreateUserID=" + query.CreateUserID;
            }
            if (query.LastModifyUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.LastModifyUserID=" + query.LastModifyUserID;
            }
            if (query.StatusS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.Status IN (" + query.StatusS + ")";
            }

            if (query.Property != Constant.STRING_INVALID_VALUE)    // 属性 1：有附件 2：有FAQ 3：有试题
            {
                string[] property = query.Property.Split(',');
                for (int i = 0; i < property.Length; i++)
                {
                    switch (property[i])
                    {
                        case "1": where += " AND KnowledgeLib.UploadFileCount != 0";
                            break;
                        case "2": where += " AND KnowledgeLib.FAQCount != 0";
                            break;
                        case "3": where += " AND KnowledgeLib.QuestionCount != 0";
                            break;
                    }
                }
            }



            where += " AND KnowledgeLib.Status!=4 ";



            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@kcid", SqlDbType.Int, 4),
					new SqlParameter("@userid", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Value = query.KCID;
            parameters[5].Value = query.UserID;
            parameters[6].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGELIB_SELECT, parameters);
            totalCount = (int)(parameters[6].Value);
            return ds.Tables[0];
        }
        */
        /// <summary>
        /// 根据不同的表格选择条件
        /// </summary>
        /// <param name="n">1：知识点；2：FAQ；3：试题</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public string getCommonWhere(int n, QueryKnowledgeLib query)
        {
            string where = string.Empty;

            if (query.Keywords != Constant.STRING_INVALID_VALUE)
            {
                where += " AND (Title like '%" + StringHelper.SqlFilter(query.Keywords) + "%' OR Abstract like '%" + StringHelper.SqlFilter(query.Keywords) + "%')";
            }
            if (query.UnRead != Constant.STRING_INVALID_VALUE)  //  未读
            {
                if (query.UnRead == string.Empty && query.UserID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND KLID NOT IN ( SELECT KLReadTag.KLID FROM KLReadTag WHERE KLReadTag.KLID=KnowledgeLib.KLID AND ReadTag=1 AND UserID=" + query.UserID + ")";
                }
            }
            if (query.Content != Constant.STRING_INVALID_VALUE)
            {
                if (query.Content == string.Empty)
                {
                    where += " AND isnull(datalength(Content),0)  <> 0  ";
                }
            }

            switch (n)
            {
                case 1:
                case 4: if (query.Category != Constant.STRING_INVALID_VALUE)    // 题型
                    {
                        string[] category = Util.SqlFilterByInCondition(query.Category).Split(',');
                        where += " AND KLID IN (SELECT a.KLID FROM KLQuestion AS a WHERE a.KLID=KnowledgeLib.KLID AND (";
                        for (int i = 0; i < category.Length; i++)
                        {
                            where += " a.AskCategory=" + category[i] + " OR";
                        }
                        where = where.TrimEnd('O', 'R') + ") AND a.Status=0)";
                    };
                    break;
                case 2: if (query.Category != Constant.STRING_INVALID_VALUE)    // FAQ
                    {
                        string[] category = Util.SqlFilterByInCondition(query.Category).Split(',');
                        where += " AND KLFAQ.KLID IN (SELECT a.KLID FROM KLQuestion AS a WHERE a.KLID=KLFAQ.KLID AND (";
                        for (int i = 0; i < category.Length; i++)
                        {
                            where += " a.AskCategory=" + category[i] + " OR";
                        }
                        where = where.TrimEnd('O', 'R') + ") AND a.Status=0)";
                    };
                    break;
                case 3: if (query.Category != Constant.STRING_INVALID_VALUE)    // 试题
                    {
                        string[] category = Util.SqlFilterByInCondition(query.Category).Split(',');
                        where += " AND (";
                        for (int i = 0; i < category.Length; i++)
                        {
                            where += " KLQuestion.AskCategory=" + category[i] + " OR";
                        }
                        where = where.TrimEnd('O', 'R') + ") AND KLQuestion.Status=0 ";
                    };
                    break;
            }
            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.Title like '%" + StringHelper.SqlFilter(query.Title) + "%'";
            }
            if (query.Property != Constant.STRING_INVALID_VALUE)    // 属性 1：有附件 2：有FAQ 3：有试题
            {
                string[] property = query.Property.Split(',');
                for (int i = 0; i < property.Length; i++)
                {
                    switch (property[i])
                    {
                        case "1": where += " AND KnowledgeLib.UploadFileCount != 0";
                            break;
                        case "2": where += " AND KnowledgeLib.FAQCount != 0";
                            break;
                        case "3": where += " AND KnowledgeLib.QuestionCount != 0";
                            break;
                    }
                }
            }
            if (query.KLFAQID != Constant.INT_INVALID_VALUE)
            {
                where += " and KLFAQ.KLFAQID=" + query.KLFAQID;
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:00:00'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            if (query.MBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.LastModifyTime>='" + StringHelper.SqlFilter(query.MBeginTime) + " 0:00:00'";
            }
            if (query.MEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.LastModifyTime<='" + StringHelper.SqlFilter(query.MEndTime) + " 23:59:59'";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.CreateUserID=" + query.CreateUserID;
            }
            if (query.LastModifyUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.LastModifyUserID=" + query.LastModifyUserID;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.Status IN (" + query.Status + ")";
            }
            if (query.StatusS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.Status IN (" + Dal.Util.SqlFilterByInCondition(query.StatusS) + ")";
            }
            if (query.KCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.KCID IN (" + Dal.Util.SqlFilterByInCondition(query.KCID.ToString()) + ")";
            }
            if (query.KCIDS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowLedgeLib.KCID IN (SELECT ID from f_Cid(" + Dal.Util.SqlFilterByInCondition(query.KCIDS) + "))";
            }
            if (query.KLID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.KLID=" + query.KLID.ToString();
            }
            //除了统计，其余列表的状态不能是已删除(统计需要统计状态为已删除的总数)
            if (n != 4)
            {
                where += " AND KnowledgeLib.Status!=4 ";
            }
            return where;
        }
        /// <summary>
        /// 统计（知识库管理列表页 刘学文）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetKnowledgeLibCount(QueryKnowledgeLib query, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;
            string where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", query.UserID);
            if (query.MBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.CreateTime>='" + StringHelper.SqlFilter(query.MBeginTime) + " 0:00:00'";
            }
            if (query.MEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.CreateTime<='" + StringHelper.SqlFilter(query.MEndTime) + " 23:59:59'";
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@kcid", SqlDbType.Int, 4),
					new SqlParameter("@pageSize", SqlDbType.Int, 4),
					new SqlParameter("@pageIndex", SqlDbType.Int, 4),
					new SqlParameter("@TotalCount", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = query.KCID;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_KnowledgeLib_Count_new", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        //获取所有的知识点总数（状态为审核通过，内容不为空）
        public int GetKLIDAllCount(QueryKnowledgeLib query, out int totalCount)
        {
            string where = string.Empty;
            where += getCommonWhere(1, query);
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000) 
					};
            parameters[0].Value = where;
            totalCount = int.Parse(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_KnowledgeLib_KLIDAllCount", parameters).ToString());
            return totalCount;
        }

        /// <summary>
        /// 知识库页面使用，根据关键字，类别取得数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="unReadCount"></param>
        /// <param name="where"></param>
        /// <param name="isUnRead"></param>
        /// <returns></returns>
        public DataSet GetKnowledgeReport(int userId, int currentPage, int pageSize, out int unReadCount, int kcpid, int kcid, string kw, string Oreder, string asds,bool isUnRead)
        {
            unReadCount = 0;
            #region 拼凑Where


            StringBuilder sb = new StringBuilder();

            //sb.Append(BLL.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", BLL.Util.GetLoginUserID()));

            sb.Append(" AND ((a.FileUrl IS NOT NULL ) OR (a.FileUrl IS null AND a.Content is NOT null)) ");
            if (kcpid != -1)
            {
                sb.Append(" and b.ppid=");
                sb.Append(kcpid.ToString());
            }
            if (kcid != -1)
            {
                sb.Append(" and b.kcid=");
                sb.Append(kcid.ToString());
            }
            if (isUnRead)
            {
                sb.Append(
                    string.Format(
                        " AND NOT EXISTS(SELECT 1 FROM KLReadTag WHERE KLReadTag.KLID=a.KLID AND ReadTag=1 AND UserID={0}) ",
                        userId));
            }
            if (!string.IsNullOrEmpty(kw))
            {
                var keyWorsArray = kw.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();

                if (keyWorsArray.Length > 0)
                {
                    sb.Append(" and (");
                    foreach (string s in keyWorsArray)
                    {
                        sb.Append(string.Format(" Title like '%{0}%' OR Abstract like '%{0}%' or", BitAuto.Utils.StringHelper.SqlFilter(s)));
                    }

                    sb.Remove(sb.Length - 3, 3);

                    sb.Append(") ");

                }
            }
            var orderQuery = Oreder;
            if (string.IsNullOrEmpty(orderQuery))
            {
                orderQuery = "CreateTime desc";
            }
            else
            {
                if (asds == "0")
                {
                    orderQuery += " desc ";
                }
                else
                {
                    orderQuery += " asc ";
                }
            }
            #endregion
            SqlParameter[] parameters = {
					new SqlParameter("@userid", userId),					
					new SqlParameter("@pagesize", pageSize),
					new SqlParameter("@pageIndex",  currentPage),
					new SqlParameter("@where",  sb.ToString()),
					new SqlParameter("@isUnRead", isUnRead),
                    new SqlParameter("@order",  orderQuery),
					new SqlParameter("@unReadCount",  unReadCount)
					};
            parameters[6].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_GetKnowledgeReport", parameters);
            unReadCount = (int)(parameters[6].Value);
            return ds;
        }

        public DataTable GetClassifyReport(int userid, int pid, string dtWhere, string RegionId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@userId", userid),					
					new SqlParameter("@pid", pid),
					new SqlParameter("@dtWhere",  dtWhere),
					new SqlParameter("@RegionId",  RegionId)
					};
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_GetClassifyReport", parameters).Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KnowledgeLib GetKnowledgeLib(long KLID)
        {
            string sqlStr = @"SELECT  KLID ,
                                                KLNum ,
                                                Title ,
                                                KCID ,
                                                Abstract ,
                                                Status ,
                                                CreateTime ,
                                                CreateUserID ,
                                                LastModifyTime ,
                                                LastModifyUserID ,
                                                IsHistory ,
                                                RejectReason ,
                                                UploadFileCount ,
                                                FAQCount ,
                                                QuestionCount,
                                                FileUrl,
                                                DownLoadCount,clickcount
                                        FROM    KnowledgeLib WHERE KLID=@KLID";
            SqlParameter parameter = new SqlParameter("@KLID", KLID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleKnowledgeLib(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public string GetKnowledgeHtml(long KLID)
        {
            string sqlStr = @"SELECT   Content
                                        FROM    KnowledgeLib WHERE KLID=@KLID";
            SqlParameter parameter = new SqlParameter("@KLID", KLID);

            Object htmlobj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);

            if (htmlobj != null)
            {
                return htmlobj.ToString();
            }
            else
            {
                return "";
            }
        }

        private Entities.KnowledgeLib LoadSingleKnowledgeLib(DataRow row)
        {
            Entities.KnowledgeLib model = new Entities.KnowledgeLib();

            if (row["KLID"].ToString() != "")
            {
                model.KLID = long.Parse(row["KLID"].ToString());
            }
            model.KLNum = row["KLNum"].ToString();
            model.Title = row["Title"].ToString();
            if (row["KCID"].ToString() != "")
            {
                model.KCID = int.Parse(row["KCID"].ToString());
            }
            // model.Content = row["Content"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            model.Abstract = row["Abstract"].ToString();

            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            if (row["LastModifyUserID"].ToString() != "")
            {
                model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
            }
            if (row["IsHistory"].ToString() != "")
            {
                model.IsHistory = int.Parse(row["IsHistory"].ToString());
            }
            model.RejectReason = row["RejectReason"].ToString();
            if (row["UploadFileCount"].ToString() != "")
            {
                model.UploadFileCount = int.Parse(row["UploadFileCount"].ToString());
            }
            if (row["FAQCount"].ToString() != "")
            {
                model.FAQCount = int.Parse(row["FAQCount"].ToString());
            }
            if (row["QuestionCount"].ToString() != "")
            {
                model.QuestionCount = int.Parse(row["QuestionCount"].ToString());
            }
            if (row["DownLoadCount"].ToString() != "")
            {
                model.DownLoadCount = int.Parse(row["DownLoadCount"].ToString());
            }
            if (row["FileUrl"].ToString() != "")
            {
                model.FileUrl = row["FileUrl"].ToString();
            }
            if (row["clickcount"].ToString() != "")
            {
                model.ClickCount = Convert.ToInt32(row["clickcount"]);
            }

            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(Entities.KnowledgeLib model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@KLNum", SqlDbType.VarChar,20),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@IsHistory", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,200),
					new SqlParameter("@UploadFileCount", SqlDbType.Int,4),
					new SqlParameter("@FAQCount", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
                       new SqlParameter("@Abstract", SqlDbType.NVarChar,1000),
                       new SqlParameter("@FileUrl", SqlDbType.NVarChar,1000)               
               
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLNum;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.KCID;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.LastModifyTime;
            parameters[9].Value = model.LastModifyUserID;
            parameters[10].Value = model.IsHistory;
            parameters[11].Value = model.RejectReason;
            parameters[12].Value = model.UploadFileCount;
            parameters[13].Value = model.FAQCount;
            parameters[14].Value = model.QuestionCount;
            parameters[15].Value = model.Abstract;
            parameters[16].Value = model.FileUrl;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGELIB_INSERT, parameters);
            long retval = 0;
            if (long.TryParse(parameters[0].Value.ToString(), out retval))
            {
            }
            return retval;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KnowledgeLib model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.Int,8),
					new SqlParameter("@KLNum", SqlDbType.VarChar,20),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@IsHistory", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,200),
					new SqlParameter("@UploadFileCount", SqlDbType.Int,4),
					new SqlParameter("@FAQCount", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
                       new SqlParameter("@Abstract", SqlDbType.NVarChar,1000),
                       new SqlParameter("@FileUrl", SqlDbType.NVarChar,1000)               
               
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLNum;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.KCID;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.LastModifyTime;
            parameters[9].Value = model.LastModifyUserID;
            parameters[10].Value = model.IsHistory;
            parameters[11].Value = model.RejectReason;
            parameters[12].Value = model.UploadFileCount;
            parameters[13].Value = model.FAQCount;
            parameters[14].Value = model.QuestionCount;
            parameters[15].Value = model.Abstract;
            parameters[16].Value = model.FileUrl;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KNOWLEDGELIB_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.KnowledgeLib model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@KLNum", SqlDbType.VarChar,20),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@KCID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@IsHistory", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,200),
					new SqlParameter("@UploadFileCount", SqlDbType.Int,4),
					new SqlParameter("@FAQCount", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
                       new SqlParameter("@Abstract", SqlDbType.NVarChar,1000),
                       new SqlParameter("@FileUrl", SqlDbType.NVarChar,256)};
            parameters[0].Value = model.KLID;
            parameters[1].Value = model.KLNum;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.KCID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.IsHistory;
            parameters[10].Value = model.RejectReason;
            parameters[11].Value = model.UploadFileCount;
            parameters[12].Value = model.FAQCount;
            parameters[13].Value = model.QuestionCount;
            parameters[14].Value = model.Abstract;
            parameters[15].Value = model.FileUrl;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGELIB_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KnowledgeLib model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@KLNum", SqlDbType.VarChar,20),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@KCID", SqlDbType.Int,4),
					
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@IsHistory", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,200),
					new SqlParameter("@UploadFileCount", SqlDbType.Int,4),
					new SqlParameter("@FAQCount", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
                       new SqlParameter("@Abstract", SqlDbType.NVarChar,1000),
                       new SqlParameter("@FileUrl", SqlDbType.NVarChar,256)};
            parameters[0].Value = model.KLID;
            parameters[1].Value = model.KLNum;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.KCID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.IsHistory;
            parameters[10].Value = model.RejectReason;
            parameters[11].Value = model.UploadFileCount;
            parameters[12].Value = model.FAQCount;
            parameters[13].Value = model.QuestionCount;
            parameters[14].Value = model.Abstract;
            parameters[15].Value = model.FileUrl;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KNOWLEDGELIB_UPDATE, parameters);
        }

        public void AddClickAndDownloadCounts(int type, int klid)
        {
            SqlParameter[] parameters = new SqlParameter[]
             {
                 new SqlParameter("@type",type),
                 new SqlParameter("@klid",klid), 
             };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_AddClickAndDownloadCounts", parameters);
        }

        public object UpdateHtml(long KLID, string HtmlContext)
        {



            string sqlStr = string.Empty;
            if (string.IsNullOrEmpty(HtmlContext))
            {
                sqlStr = string.Format("update KnowledgeLib set Content=null where KLID={0}", KLID);
                return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            }
            else
            {
                SqlParameter[] parameters = {
					new SqlParameter("@Content", SqlDbType.NText),
                    new SqlParameter("@KLID", SqlDbType.BigInt,8)};

                parameters[0].Value = HtmlContext;
                parameters[1].Value = KLID;

                sqlStr = "update KnowledgeLib set Content=@Content where KLID=@KLID";
                return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long KLID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.BigInt)};
            parameters[0].Value = KLID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGELIB_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.BigInt)};
            parameters[0].Value = KLID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KNOWLEDGELIB_DELETE, parameters);
        }
        #endregion

        #region IsExist
        /// <summary>
        /// 知识点下是否有试题
        /// </summary>
        /// <param name="KLID"></param>
        /// <returns></returns>
        public bool IsExistQuestion(int KLID)
        {
            bool isExist = false;
            string sqlStr = "Select * From KLQuestion Where KLID=@KLID And Status=0";
            SqlParameter parameter = new SqlParameter("@KLID", KLID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                isExist = true;
            }

            return isExist;

        }
        #endregion

        #region 最大值

        /// <summary>
        /// 取当前最大值
        /// </summary>
        /// <returns></returns>
        public int GetCurrMaxID()
        {
            int maxid = 0;

            string sqlStr = "select max([KLID]) FROM [KnowledgeLib]";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                int intVal = 0;
                if (int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out intVal))
                {
                    maxid = intVal;
                }
                else
                {
                    maxid = 0;
                }
            }
            else
            {
                maxid = 0;
            }

            return maxid;
        }

        #endregion

        #region 新增、保存

        public void AddSave()
        {

        }

        /// <summary>
        /// 添加时的保存
        /// </summary>
        /// <param name="knowModel"></param>
        /// <param name="faqModel"></param>
        /// <param name="questionModel"></param>
        /// <param name="answerModel"></param>
        /// <param name="optionModel"></param>
        /// <param name="errMsg"></param>
        public void AddSave(Entities.KnowledgeLib knowModel, Entities.KLFAQ faqModel, Entities.KLQuestion questionModel, Entities.KLQAnswer answerModel, Entities.KLAnswerOption optionModel, out string errMsg)
        {
            errMsg = "";

            SqlConnection connection = new SqlConnection(CONNECTIONSTRINGS);
            connection.Open();
            SqlTransaction sqlTran = connection.BeginTransaction("SampleTransaction");
            try
            {
                int kid = Dal.KnowledgeLib.Instance.Insert(sqlTran, knowModel);
                int faqId = Dal.KLFAQ.Instance.Insert(sqlTran, faqModel);
                int questionId = Dal.KLQuestion.Instance.Insert(sqlTran, questionModel);
                Dal.KLQAnswer.Instance.Insert(sqlTran, answerModel);
                int optionId = Dal.KLAnswerOption.Instance.Insert(sqlTran, optionModel);

            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                errMsg = ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region 获取所有创建人
        public DataTable getCreateUser()
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT CreateUserID FROM dbo.KnowledgeLib");
            return ds.Tables[0];
        }
        public DataTable getModifyUser()
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT LastModifyUserID FROM dbo.KnowledgeLib");
            return ds.Tables[0];
        }
        #endregion



    }
}

