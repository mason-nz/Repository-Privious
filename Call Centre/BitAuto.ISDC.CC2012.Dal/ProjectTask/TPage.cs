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
    /// 数据访问类TPage。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TPage : DataBase
    {
        #region Instance
        public static readonly TPage Instance = new TPage();
        #endregion

        #region const
        private const string P_TPAGE_SELECT = "p_TPage_Select";
        private const string P_TPAGE_INSERT = "p_TPage_Insert";
        private const string P_TPAGE_UPDATE = "p_TPage_Update";
        private const string P_TPAGE_DELETE = "p_TPage_Delete";
        #endregion

        #region Contructor
        protected TPage()
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
        public DataTable GetTPage(QueryTPage query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;


            //分组权限判断
            //if ((query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty) || (query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty))
            //{
            //    if (query.LoginID != Constant.INT_INVALID_VALUE)
            //    {
            //        where += " AND (";

            //        if (query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty)
            //        {
            //            //筛选登陆人管理的所属业务组权限是 本组 的信息
            //            where += " TPage.BGID IN ( " + query.OwnGroup + ") ";
            //        }

            //        if (query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty && query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty)
            //        {
            //            where += " OR ";
            //        }

            //        if (query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty)
            //        {
            //            //筛选登陆人管理的所属业务组权限是 本人 的信息 
            //            where += " (TPage.BGID IN (" + query.OneSelf + ") AND TPage.CreateUserID=" + query.LoginID + ")";
            //        }

            //        where += ")";
            //    }
            //}
            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("TPage", "BGID", "CreateUserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID = " + query.RecID + "";
            }

            if (query.TPName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TPName like '%" + StringHelper.SqlFilter(query.TPName) + "%'";
            }

            if (query.TTName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TTName ='" + StringHelper.SqlFilter(query.TTName) + "'";
            }

            if (query.TTCode != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TTCode ='" + StringHelper.SqlFilter(query.TTCode) + "'";
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND BGID=" + query.BGID;
            }

            if (query.IsUsed != Constant.INT_INVALID_VALUE)
            {
                where += " AND IsUsed=" + query.IsUsed;
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SCID=" + query.SCID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND CreateUserID=" + query.CreateUserID;
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE && query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND Createtime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:00:000' AND Createtime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:29'";
            }

            //0- 未完成  ；1-已完成；2-已使用（TPage表中除了 二手车客户导入模版 和 新车客户导入模板 的status=2，其他的已使用需要去TTable根据TTCode查询字段TTIsData是否为1，如果为1则表示已使用；为其他的则表示未使用）

            if (query.Statuss != Constant.STRING_INVALID_VALUE)
            {
                string statusWhere = query.Statuss;

                if (statusWhere.Contains("0") && statusWhere.Contains("1") && statusWhere.Contains("2"))
                {

                }
                else if (statusWhere.Contains("0") && statusWhere.Contains("1"))
                {
                    where += " and (status =0 or (status=1 and TTCode in (select TTCode from TTable where TTable.TTCode=Tpage.TTCode and TTIsData<>1)))";
                }
                else if (statusWhere.Contains("0") && statusWhere.Contains("2"))
                {
                    where += " and (status=1 and TTCode in (select TTCode from TTable where TTable.TTCode=Tpage.TTCode and TTIsData=1) or status in (0,2))";
                }
                else if (statusWhere.Contains("1") && statusWhere.Contains("2"))
                {
                    where += " and  status in (1,2)";
                }
                else if (statusWhere.Contains("0"))
                {
                    where += " and  status=0";
                }
                else if (statusWhere.Contains("1"))
                {
                    where += " and  status=1 and TTCode in (select TTCode from TTable where TTable.TTCode=Tpage.TTCode and TTIsData<>1)";
                }
                else if (statusWhere.Contains("2"))
                {
                    where += " and(  (status=1 and TTCode in (select TTCode from TTable where TTable.TTCode=Tpage.TTCode and TTIsData=1) ) or status=2)";
                }

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TPAGE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.TPage GetTPage(int RecID)
        {
            QueryTPage query = new QueryTPage();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTPage(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleTPage(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.TPage LoadSingleTPage(DataRow row)
        {
            Entities.TPage model = new Entities.TPage();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.TPName = row["TPName"].ToString();
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.TPRef = row["TPRef"].ToString();
            model.TTCode = row["TTCode"].ToString();
            model.GenTempletPath = row["GenTempletPath"].ToString();
            model.TPContent = row["TPContent"].ToString();
            model.TTName = row["TTName"].ToString();
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
            if (row["IsShowBtn"].ToString() != "")
            {
                model.IsShowBtn = int.Parse(row["IsShowBtn"].ToString());
            }
            if (row["IsShowWorkOrderBtn"].ToString() != "")
            {
                model.IsShowWorkOrderBtn = int.Parse(row["IsShowWorkOrderBtn"].ToString());
            }
            if (row["IsShowSendMsgBtn"].ToString() != "")
            {
                model.IsShowSendMsgBtn = int.Parse(row["IsShowSendMsgBtn"].ToString());
            }
            if (row["IsUsed"].ToString() != "")
            {
                model.IsUsed = int.Parse(row["IsUsed"].ToString());
            }
            if (row["IsShowQiCheTong"].ToString() != "")
            {
                model.IsShowQiCheTong = int.Parse(row["IsShowQiCheTong"].ToString());
            }
            if (row["IsShowSubmitOrder"].ToString() != "")
            {
                model.IsShowSubmitOrder = int.Parse(row["IsShowSubmitOrder"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.TPage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TPName", SqlDbType.NVarChar,200),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@TPRef", SqlDbType.Text),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TPContent", SqlDbType.NText),
					new SqlParameter("@GenTempletPath", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
                    new SqlParameter("@IsShowBtn", SqlDbType.Int,4)   ,
              new SqlParameter("@IsShowWorkOrderBtn", SqlDbType.Int,4),
              new SqlParameter("@IsShowSendMsgBtn", SqlDbType.Int,4)   ,
                    new SqlParameter("@IsUsed", SqlDbType.Int,4)  ,
              new SqlParameter("@IsShowQiCheTong", SqlDbType.Int,4),
              new SqlParameter("@IsShowSubmitOrder", SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TPName;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.TPRef;
            parameters[5].Value = model.TTCode;
            parameters[6].Value = model.TPContent;
            parameters[7].Value = model.GenTempletPath;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.TTName;
            parameters[13].Value = model.IsShowBtn;
            parameters[14].Value = model.IsShowWorkOrderBtn;
            parameters[15].Value = model.IsShowSendMsgBtn;
            parameters[16].Value = model.IsUsed;
            parameters[17].Value = model.IsShowQiCheTong;
            parameters[18].Value = model.IsShowSubmitOrder;


            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TPAGE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TPage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TPName", SqlDbType.NVarChar,200),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@TPRef", SqlDbType.Text),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TPContent", SqlDbType.NText),
					new SqlParameter("@GenTempletPath", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
                    new SqlParameter("@IsShowBtn", SqlDbType.Int,4),
                                         new SqlParameter("@IsShowWorkOrderBtn", SqlDbType.Int,4)  ,
              new SqlParameter("@IsShowSendMsgBtn", SqlDbType.Int,4)   ,
                    new SqlParameter("@IsUsed", SqlDbType.Int,4)    ,
              new SqlParameter("@IsShowQiCheTong", SqlDbType.Int,4),
              new SqlParameter("@IsShowSubmitOrder", SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TPName;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.TPRef;
            parameters[5].Value = model.TTCode;
            parameters[6].Value = model.TPContent;
            parameters[7].Value = model.GenTempletPath;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.TTName;
            parameters[13].Value = model.IsShowBtn;
            parameters[14].Value = model.IsShowWorkOrderBtn;
            parameters[15].Value = model.IsShowSendMsgBtn;
            parameters[16].Value = model.IsUsed;
            parameters[17].Value = model.IsShowQiCheTong;
            parameters[18].Value = model.IsShowSubmitOrder;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TPAGE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.TPage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TPName", SqlDbType.NVarChar,200),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@TPRef", SqlDbType.Text),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TPContent", SqlDbType.NText),
					new SqlParameter("@GenTempletPath", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
                    new SqlParameter("@IsShowBtn", SqlDbType.Int,4),
                   new SqlParameter("@IsShowWorkOrderBtn", SqlDbType.Int,4),
              new SqlParameter("@IsShowSendMsgBtn", SqlDbType.Int,4) ,
                    new SqlParameter("@IsUsed", SqlDbType.Int,4) ,
              new SqlParameter("@IsShowQiCheTong", SqlDbType.Int,4),
              new SqlParameter("@IsShowSubmitOrder", SqlDbType.Int,4)       };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TPName;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.TPRef;
            parameters[5].Value = model.TTCode;
            parameters[6].Value = model.TPContent;
            parameters[7].Value = model.GenTempletPath;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.TTName;
            parameters[13].Value = model.IsShowBtn;
            parameters[14].Value = model.IsShowWorkOrderBtn;
            parameters[15].Value = model.IsShowSendMsgBtn;
            parameters[16].Value = model.IsUsed;
            parameters[17].Value = model.IsShowQiCheTong;
            parameters[18].Value = model.IsShowSubmitOrder;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TPAGE_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TPage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TPName", SqlDbType.NVarChar,200),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@TPRef", SqlDbType.Text),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TPContent", SqlDbType.NText),
					new SqlParameter("@GenTempletPath", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
                    new SqlParameter("@IsShowBtn", SqlDbType.Int,4) ,
                                         new SqlParameter("@IsShowWorkOrderBtn", SqlDbType.Int,4),
              new SqlParameter("@IsShowSendMsgBtn", SqlDbType.Int,4),
                    new SqlParameter("@IsUsed", SqlDbType.Int,4) ,
              new SqlParameter("@IsShowQiCheTong", SqlDbType.Int,4),
              new SqlParameter("@IsShowSubmitOrder", SqlDbType.Int,4)     };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TPName;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.TPRef;
            parameters[5].Value = model.TTCode;
            parameters[6].Value = model.TPContent;
            parameters[7].Value = model.GenTempletPath;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.TTName;
            parameters[13].Value = model.IsShowBtn;
            parameters[14].Value = model.IsShowWorkOrderBtn;
            parameters[15].Value = model.IsShowSendMsgBtn;
            parameters[16].Value = model.IsUsed;
            parameters[17].Value = model.IsShowQiCheTong;
            parameters[18].Value = model.IsShowSubmitOrder;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TPAGE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TPAGE_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TPAGE_DELETE, parameters);
        }
        #endregion


        public DataTable GetAllCreateUserID()
        {
            string where = string.Empty;

            where = "select distinct CreateUserID from TPage";

            DataSet ds;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, where, null);
            return ds.Tables[0];
        }

        /// <summary>
        /// 判断recID的生成模板表是否存在数据
        /// </summary>
        /// <param name="recID">TPage主键</param>
        /// <returns></returns>
        public int isHasDataByTTName(int recID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@Count", SqlDbType.Int), 
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = recID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_TPage_isHasDataByTTName", parameters);
            return (int)parameters[0].Value;
        }


    }
}

