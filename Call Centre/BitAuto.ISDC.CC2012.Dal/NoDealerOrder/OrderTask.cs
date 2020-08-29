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
    /// 数据访问类OrderTask。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderTask : DataBase
    {
        #region Instance
        public static readonly OrderTask Instance = new OrderTask();
        #endregion

        #region const
        private const string P_ORDERTASK_SELECT = "p_OrderTask_Select";
        private const string P_ORDERTASK_INSERT = "p_OrderTask_Insert";
        private const string P_ORDERTASK_UPDATE = "p_OrderTask_Update";
        private const string P_ORDERTASK_DELETE = "p_OrderTask_Delete";
        //add by qizq 2012-9-25 无主订单任务列表
        private const string P_ORDERTASK_MANAGE = "p_OrderTaskManage";

        #endregion

        #region Contructor
        protected OrderTask()
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
        public DataTable GetOrderTask(QueryOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 条件

            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID=" + query.TaskID.ToString();
            }
            if (query.Source != Constant.INT_INVALID_VALUE)
            {
                where += " AND Source=" + query.Source.ToString();
            }
            if (query.TaskStatus != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskStatus=" + query.TaskStatus.ToString();
            }
            if (query.RelationID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RelationID=" + query.RelationID.ToString();
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND BGID=" + query.BGID.ToString();
            }
            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND AssignUserID=" + query.AssignUserID.ToString();
            }
            if (query.UserName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND UserName like '%" + StringHelper.SqlFilter(query.UserName) + "%'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND Status=" + query.Status.ToString();
            }

            #endregion

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASK_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 取任务不同处理人
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiffassignuserid()
        {
            DataSet ds = null;
            string sqlstr = "select distinct assignuserid from OrderTask WITH (NOLOCK)";
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }

        //add by qizq 2012-9-25 无主订单任务列表
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetOrderTaskList(QueryOrderTask query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            string where = string.Empty;

            #region 条件

            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.TaskID=" + StringHelper.SqlFilter(query.TaskID.ToString());
            }
            if (query.Source != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.Source=" + StringHelper.SqlFilter(query.Source.ToString());
            }
            if (query.TaskStatus != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.TaskStatus=" + StringHelper.SqlFilter(query.TaskStatus.ToString());
            }
            if (query.RelationID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.RelationID=" + StringHelper.SqlFilter(query.RelationID.ToString());
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.BGID=" + StringHelper.SqlFilter(query.BGID.ToString());
            }
            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.AssignUserID=" + StringHelper.SqlFilter(query.AssignUserID.ToString());
            }
            if (query.UserName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.UserName like '%" + StringHelper.SqlFilter(query.UserName) + "%'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.Status=" + StringHelper.SqlFilter(query.Status.ToString());
            }
            if (query.YpOrderID != Constant.INT_INVALID_VALUE)
            {
                where += " and ((a.source=2 and b.yporderid=" + query.YpOrderID + ") or (a.source=1 and c.yporderid=" + query.YpOrderID + ") or (a.source=3 and c.yporderid=" + query.YpOrderID + "))";
            }

            if (!string.IsNullOrEmpty(query.Area))
            {
                where += " and ((a.source=2 and b.AreaID='" + query.Area + "') or (a.source=1 and c.AreaID='" + query.Area + "') or (a.source=3 and c.AreaID='" + query.Area + "'))";
            }

            if (query.ProvinceID != Constant.INT_INVALID_VALUE)
            {
                where += " and ((a.source=2 and b.ProvinceID='" + query.ProvinceID + "') or (a.source=1 and c.ProvinceID='" + query.ProvinceID + "') or (a.source=3 and c.ProvinceID='" + query.ProvinceID + "'))";
            }
            if (query.CityID != Constant.INT_INVALID_VALUE)
            {
                where += " and ((a.source=2 and b.CityID='" + query.CityID + "') or (a.source=1 and c.CityID='" + query.CityID + "') or (a.source=3 and c.CityID='" + query.CityID + "'))";
            }
            //由于OrderCreateTime 是smallDatetime类型的所以如果查某天数据列如 2月1号的要写成 >='2013-2-1 00:00:000' and <'2013-2-2 00:00:000'
            if (query.CreateTimeBegin != Constant.DATE_INVALID_VALUE && query.CreateTimeEnd != Constant.DATE_INVALID_VALUE)
            {
                DateTime endtime = Convert.ToDateTime(query.CreateTimeEnd);
                if (endtime.Hour == 23 && endtime.Minute == 59)
                {
                    endtime = endtime.AddHours(-23);
                    endtime = endtime.AddMinutes(-59);
                    endtime = endtime.AddSeconds(-endtime.Second);
                    endtime = endtime.AddDays(1);
                }

                where += " and ((a.source=2 and (b.OrderCreateTime>='" + query.CreateTimeBegin + "' and b.OrderCreateTime<'" + endtime + "')) or (a.source=1 and (c.OrderCreateTime>='" + query.CreateTimeBegin + "' and c.OrderCreateTime<'" + endtime + "')) or (a.source=3 and (c.OrderCreateTime>='" + query.CreateTimeBegin + "' and c.OrderCreateTime<'" + endtime + "')))";
            }
            if (query.SubmitTimeBegin != Constant.DATE_INVALID_VALUE && query.SubmitTimeEnd != Constant.DATE_INVALID_VALUE)
            {
                where += " and (a.submittime>='" + Convert.ToDateTime(query.SubmitTimeBegin).ToShortDateString() + " 0:00:000' and a.submittime<='" + Convert.ToDateTime(query.SubmitTimeEnd).ToShortDateString() + " 23:59:59')";
            }


            if (query.TypeStr != Constant.STRING_INVALID_VALUE)
            {

                if (query.TypeStr.IndexOf(',') > 0)
                {
                    where += " and (";
                    for (int i = 0; i < Util.SqlFilterByInCondition(query.TypeStr).Split(',').Length; i++)
                    {
                        where += " a.Source='" + Util.SqlFilterByInCondition(query.TypeStr).Split(',')[i] + "' or";
                    }
                    where = where.Substring(0, where.Length - 3);
                    where += ")";
                }
                else
                {
                    where += " and a.source='" + query.TypeStr + "'";
                }

            }
            //add by qizq 2013-7-18加任务类型，0是无主订单，不等于0是免费订单
            if (query.TaskType != Constant.STRING_INVALID_VALUE)
            {
                if (query.TaskType.IndexOf(',') > 0)
                {
                }
                else
                {
                    if (query.TaskType == "0")
                    {
                        where += " and a.DealerID=0";
                    }
                    else
                    {
                        where += " and a.DealerID!=0";
                    }
                }
            }
            //

            if (query.IsSelectdMsmemberstr != Constant.STRING_INVALID_VALUE)
            {

                if (query.IsSelectdMsmemberstr.IndexOf(',') > 0)
                {
                    where += " and ( a.isselectDMSMember='" + Util.SqlFilterByInCondition(query.IsSelectdMsmemberstr).Split(',')[0] + "' or (a.isselectDMSMember='" + query.IsSelectdMsmemberstr.Split(',')[1] + "'";

                    if (query.NoDealerReasonID != Constant.INT_INVALID_VALUE)
                    {
                        where += " and a.NoDealerReasonID=" + query.NoDealerReasonID + "";
                    }
                    where += "))";
                }
                else
                {
                    where += " and a.isselectDMSMember='" + Util.SqlFilterByInCondition(query.IsSelectdMsmemberstr) + "'";
                    if (query.NoDealerReasonID != Constant.INT_INVALID_VALUE)
                    {
                        where += " and a.NoDealerReasonID=" + query.NoDealerReasonID + "";
                    }
                }

            }
            if (query.StatuStr != Constant.STRING_INVALID_VALUE)
            {

                if (query.StatuStr.IndexOf(',') > 0)
                {
                    where += " and (";
                    for (int i = 0; i < Util.SqlFilterByInCondition(query.StatuStr).Split(',').Length; i++)
                    {
                        where += " a.TaskStatus='" + Util.SqlFilterByInCondition(query.StatuStr).Split(',')[i] + "' or";
                    }
                    where = where.Substring(0, where.Length - 3);
                    where += ")";
                }
                else
                {
                    where += " and a.TaskStatus='" + StringHelper.SqlFilter(query.StatuStr) + "'";
                }

            }


            //判断当前人是否有全部数据权限
            int RightType = (int)Dal.UserDataRigth.Instance.GetUserDataRigth(userid).RightType;
            //如果没有
            if (RightType != 2)
            {
                //取当前人所对应的数据权限组
                Entities.QueryUserGroupDataRigth QueryUserGroupDataRigth = new Entities.QueryUserGroupDataRigth();
                QueryUserGroupDataRigth.UserID = userid;
                int totcount = 0;
                DataTable dtUserGroupDataRigth = Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(QueryUserGroupDataRigth, "", 1, 100000, out totcount);
                string Rolename = string.Empty;
                if (dtUserGroupDataRigth != null && dtUserGroupDataRigth.Rows.Count > 0)
                {
                    where += "  and (";
                    for (int i = 0; i < dtUserGroupDataRigth.Rows.Count; i++)
                    {
                        //本人
                        if (dtUserGroupDataRigth.Rows[i]["RightType"].ToString() == "1")
                        {
                            where += "(a.BGID='" + dtUserGroupDataRigth.Rows[i]["BGID"].ToString() + "' and a.assignuserid='" + userid + "') or";
                        }
                        //本组
                        else
                        {
                            where += "(a.BGID='" + dtUserGroupDataRigth.Rows[i]["BGID"].ToString() + "') or";
                        }

                    }
                    where = where.Substring(0, where.Length - 3);
                    where += ")";
                }
            }



            #endregion

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASK_MANAGE, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderTask GetOrderTask(long TaskID)
        {
            QueryOrderTask query = new QueryOrderTask();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderTask(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOrderTask(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.OrderTask LoadSingleOrderTask(DataRow row)
        {
            Entities.OrderTask model = new Entities.OrderTask();

            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = long.Parse(row["TaskID"].ToString());
            }
            if (row["Source"].ToString() != "")
            {
                model.Source = int.Parse(row["Source"].ToString());
            }
            if (row["TaskStatus"].ToString() != "")
            {
                model.TaskStatus = int.Parse(row["TaskStatus"].ToString());
            }
            if (row["RelationID"].ToString() != "")
            {
                model.RelationID = long.Parse(row["RelationID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["AssignUserID"].ToString() != "")
            {
                model.AssignUserID = int.Parse(row["AssignUserID"].ToString());
            }
            if (row["AssignTime"].ToString() != "")
            {
                model.AssignTime = DateTime.Parse(row["AssignTime"].ToString());
            }
            model.UserName = row["UserName"].ToString();
            if (row["IsSelectDMSMember"].ToString() != "")
            {
                if ((row["IsSelectDMSMember"].ToString() == "1") || (row["IsSelectDMSMember"].ToString().ToLower() == "true"))
                {
                    model.IsSelectDMSMember = true;
                }
                else
                {
                    model.IsSelectDMSMember = false;
                }
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["SubmitTime"].ToString() != "")
            {
                model.SubmitTime = DateTime.Parse(row["SubmitTime"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["NoDealerReasonID"].ToString() != "")
            {
                model.NoDealerReasonID = int.Parse(row["NoDealerReasonID"].ToString());
            }
            if (row["NoDealerReason"].ToString() != "")
            {
                model.NoDealerReason = row["NoDealerReason"].ToString();
            }
            if (row["DealerID"].ToString() != "")
            {
                model.DealerID = int.Parse(row["DealerID"].ToString());
            }

            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.OrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@IsSelectDMSMember", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                	new SqlParameter("@NoDealerReasonID", SqlDbType.Int,4),
					new SqlParameter("@NoDealerReason", SqlDbType.VarChar,1000),                        
                     new SqlParameter("@DealerID", SqlDbType.Int,4) 
                                        
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Source;
            parameters[2].Value = model.TaskStatus;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.BGID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.UserName;
            parameters[8].Value = model.IsSelectDMSMember;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.SubmitTime;
            parameters[11].Value = model.CreateTime;
            parameters[12].Value = model.CreateUserID;
            parameters[13].Value = model.NoDealerReasonID;
            parameters[14].Value = model.NoDealerReason;
            parameters[15].Value = model.DealerID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASK_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@IsSelectDMSMember", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
	
                                        	new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                	new SqlParameter("@NoDealerReasonID", SqlDbType.Int,4),
					new SqlParameter("@NoDealerReason", SqlDbType.VarChar,1000),
                    new SqlParameter("@DealerID", SqlDbType.Int,4)                   
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Source;
            parameters[2].Value = model.TaskStatus;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.BGID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.UserName;
            parameters[8].Value = model.IsSelectDMSMember;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.SubmitTime;
            parameters[11].Value = model.CreateTime;
            parameters[12].Value = model.CreateUserID;
            parameters[13].Value = model.NoDealerReasonID;
            parameters[14].Value = model.NoDealerReason;
            parameters[15].Value = model.DealerID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERTASK_INSERT, parameters);
            return (long)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.OrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@IsSelectDMSMember", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                	new SqlParameter("@NoDealerReasonID", SqlDbType.Int,4),
					new SqlParameter("@NoDealerReason", SqlDbType.VarChar,1000) 
                                        
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.Source;
            parameters[2].Value = model.TaskStatus;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.BGID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.UserName;
            parameters[8].Value = model.IsSelectDMSMember;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.SubmitTime;
            parameters[11].Value = model.CreateTime;
            parameters[12].Value = model.CreateUserID;
            parameters[13].Value = model.NoDealerReasonID;
            parameters[14].Value = model.NoDealerReason;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASK_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@IsSelectDMSMember", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                	new SqlParameter("@NoDealerReasonID", SqlDbType.Int,4),
					new SqlParameter("@NoDealerReason", SqlDbType.VarChar,1000) 
                                        
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.Source;
            parameters[2].Value = model.TaskStatus;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.BGID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.UserName;
            parameters[8].Value = model.IsSelectDMSMember;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.SubmitTime;
            parameters[11].Value = model.CreateTime;
            parameters[12].Value = model.CreateUserID;
            parameters[13].Value = model.NoDealerReasonID;
            parameters[14].Value = model.NoDealerReason;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERTASK_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASK_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASK_DELETE, parameters);
        }
        #endregion




    }
}

