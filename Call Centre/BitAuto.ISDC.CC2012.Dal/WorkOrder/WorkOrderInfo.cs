using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类WorkOrderInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderInfo : DataBase
    {
        #region Instance
        public static readonly WorkOrderInfo Instance = new WorkOrderInfo();
        #endregion

        #region const
        private const string P_WORKORDERINFO_SELECT = "p_WorkOrderInfo_Select";
        private const string P_WORKORDERINFO_INSERT = "p_WorkOrderInfo_Insert";
        private const string P_WORKORDERINFO_UPDATE = "p_WorkOrderInfo_Update";
        private const string P_WORKORDERINFO_DELETE = "p_WorkOrderInfo_Delete";
        private const string P_WORKORDERINFO_SELECTFOREXPORT = "p_WorkOrderInfo_SelectForExport";
        private const string P_WORKORDERINFO_SELECTCREATEUSER = "p_WorkOrderInfo_SelectCreateUser";
        private const string P_WORKORDERINFO_SELECTBYUSERID = "p_WorkOrderInfo_SelectByUserID";
        #endregion

        #region Contructor
        protected WorkOrderInfo()
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
        public DataTable GetWorkOrderInfo(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            string joinWhere = string.Empty;

            where = GetWhere(query, out joinWhere);

            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@joinwhere",SqlDbType.NVarChar,4000),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = joinWhere;
            parameters[1].Value = where;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_SELECT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetWorkOrderInfoForDemandInfo(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            string joinWhere = string.Empty;

            where = GetWhere(query, out joinWhere) + " and woi.WorkOrderStatus !=1 ";

            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@joinwhere",SqlDbType.NVarChar,4000),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = joinWhere;
            parameters[1].Value = where;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_SELECT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 按照查询条件查询(包含数据权限)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetWorkOrderInfoForList(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount, log4net.ILog log)
        {
            string where = string.Empty;
            string joinWhere = string.Empty;

            where = GetWhere(query, out joinWhere);

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstrByOrderWhere("woi", "BGID", "CreateUserID", query.LoginID, @" EXISTS ( SELECT  wor.OrderID
                            FROM    dbo.WorkOrderReceiver wor
                            WHERE   wor.OrderID = woi.OrderID
                                    AND wor.ReceiverUserID = " + query.LoginID + " ) ");

                where += whereDataRight;
            }
            #endregion

            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@joinwhere",SqlDbType.NVarChar,4000),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = joinWhere;
            parameters[1].Value = where;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            log.Info(string.Format("【调用脚本p_WorkOrderInfo_Report】,开始：条件：{0}", where));
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_WorkOrderInfo_Report", parameters);
            sw.Stop();
            totalCount = (int)(parameters[5].Value);
            log.Info(string.Format("【调用脚本p_WorkOrderInfo_Report】,耗时：{2}毫秒，条件：{0}，查询总数：{1}", where, totalCount, sw.ElapsedMilliseconds));

            return ds.Tables[0];
        }
        /// <summary>
        /// 查询员工负责的客户下的工单（用于Crm系统查询）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="userId"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoByUserID(QueryWorkOrderInfo query, int userId, string departmentId, string order, int currentPage, int pageSize, out int totalCount, log4net.ILog log)
        {
            string where = string.Empty;
            string joinWhere = string.Empty;

            where = GetWhere(query, out joinWhere);
            if (string.IsNullOrEmpty(query.CRMCustID))
            {
                where += " And workOrderStatus!=5";
            }
            //过滤无效沟通和其他类的工单
            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@joinwhere",SqlDbType.NVarChar,4000),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
                    new SqlParameter("@userid",SqlDbType.Int,4),
                    new SqlParameter("@departID",SqlDbType.VarChar,200),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = joinWhere;
            parameters[1].Value = where;
            parameters[2].Value = userId;
            parameters[3].Value = departmentId;
            parameters[4].Value = order;
            parameters[5].Value = pageSize;
            parameters[6].Value = currentPage;
            parameters[7].Direction = ParameterDirection.Output;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_SELECTBYUSERID, parameters);
            totalCount = (int)(parameters[7].Value);
            sw.Stop();
            log.Info(string.Format("【调用脚本p_WorkOrderInfo_SelectByUserID】,耗时：{2}毫秒，where条件：{0}，joinWhere条件：{3}，UserID：{4}，departID：{5}，order：{6}，查询总数：{1}",
                where, totalCount, sw.ElapsedMilliseconds, joinWhere, userId, departmentId, order));
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoForExport(QueryWorkOrderInfo query, int workCategory, int userId, string order)
        {
            string where = string.Empty;
            string joinWhere = string.Empty;

            where = GetWhere(query, out joinWhere);

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstrByOrderWhere("woi", "BGID", "CreateUserID", query.LoginID, @" EXISTS ( SELECT  wor.OrderID
                            FROM    dbo.WorkOrderReceiver wor
                            WHERE   wor.OrderID = woi.OrderID
                                    AND wor.ReceiverUserID = " + query.LoginID + " ) ");

                where += whereDataRight;
            }
            #endregion

            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@WorkCategory",SqlDbType.Int),
                    new SqlParameter("@joinwhere",SqlDbType.NVarChar,4000),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200)
					};
            parameters[0].Value = workCategory;
            parameters[1].Value = joinWhere;
            parameters[2].Value = where;
            parameters[3].Value = order;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_SELECTFOREXPORT, parameters);
            return ds.Tables[0];
        }


        private string GetWhere(QueryWorkOrderInfo query, out string joinWhere)
        {
            string where = string.Empty;
            joinWhere = string.Empty;

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " and woi.OrderID='" + SqlFilter(query.OrderID) + "'";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE && query.BGID > 0)
            {
                where += " and woi.BGID=" + query.BGID;
            }
            if (query.SCID != Constant.INT_INVALID_VALUE && query.SCID > 0)
            {
                where += " and woi.SCID=" + query.SCID;
            }
            if (query.ProvinceID != null && query.ProvinceID.Value != Constant.INT_INVALID_VALUE && query.ProvinceID.Value > 0)
            {
                where += " and woi.ProvinceID=" + query.ProvinceID.Value;
            }
            if (query.CityID != null && query.CityID.Value != Constant.INT_INVALID_VALUE && query.CityID.Value > 0)
            {
                where += " and woi.CityID=" + query.CityID.Value;
            }
            if (query.CountyID != null && query.CountyID.Value != Constant.INT_INVALID_VALUE && query.CountyID.Value > 0)
            {
                where += " and woi.CountyID=" + query.CountyID.Value;
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and woi.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.ContactTel != Constant.STRING_INVALID_VALUE)
            {
                where += " and woi.ContactTel = '" + StringHelper.SqlFilter(query.ContactTel) + "'";
            }
            if (query.CategoryID != Constant.INT_INVALID_VALUE && query.CategoryID > 0)
            {
                joinWhere += " Left Join WorkOrderCategory as woc ON woi.CategoryID=woc.RecID";
                joinWhere += " Left Join WorkOrderCategory as woca ON woc.PID=woca.RecID";
                where += " and (woi.CategoryID=" + query.CategoryID + " or woc.pid=" + query.CategoryID + " or woca.pid=" + query.CategoryID + ")";
            }
            DateTime beginTime;
            if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            {
                where += " and woi.CreateTime>='" + beginTime + "'";
            }
            DateTime endTime;
            if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            {
                where += " and woi.CreateTime<'" + endTime.AddDays(1) + "'";
            }
            if (query.WorkOrderStatus != Constant.INT_INVALID_VALUE && query.WorkOrderStatus > 0)
            {
                where += " and woi.WorkOrderStatus=" + query.WorkOrderStatus;
            }
            if (query.PriorityLevel != Constant.INT_INVALID_VALUE && query.PriorityLevel > 0)
            {
                where += " and woi.PriorityLevel=" + query.PriorityLevel;
            }
            if (query.IsRevertStr != Constant.STRING_INVALID_VALUE && !query.IsRevertStr.Contains(","))
            {
                where += " and woi.IsRevert=" + StringHelper.SqlFilter(query.IsRevertStr);
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE && query.CreateUserID > 0)
            {
                where += " And woi.CreateUserID=" + query.CreateUserID;
            }
            if (query.CRMCustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And woi.CRMCustID=" + StringHelper.SqlFilter(query.CRMCustID);
            }
            if (query.CreateUserName != Constant.STRING_INVALID_VALUE)
            {
                joinWhere += " JOIN Crm2009.dbo.v_userinfo as v On woi.CreateUserID=v.UserID";
                where += " And v.TrueName like '%" + SqlFilter(query.CreateUserName) + "%'";
            }
            if (query.ReceiverName != Constant.STRING_INVALID_VALUE)
            {
                joinWhere += " JOIN Crm2009.dbo.v_userinfo as v On woi.ReceiverID=v.UserID";
                where += " And v.TrueName like '%" + SqlFilter(query.ReceiverName) + "%'";
            }
            if (query.WorkCategory != Constant.INT_INVALID_VALUE && query.WorkCategory > 0)
            {
                where += " And woi.WorkCategory=" + query.WorkCategory;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE && query.BGID > 0)
            {
                where += " And woi.BGID=" + query.BGID;
            }
            if (query.IsReCheck != Constant.STRING_INVALID_VALUE && !query.IsReCheck.Contains(","))
            {
                //可复核-是
                if (query.IsReCheck == "1")
                {
                    //条件为：状态为 处理中 且 当前处理人=最后操作人
                    where += " And woi.WorkOrderStatus=" + (int)Entities.WorkOrderStatus.Processing + " and woi.ModifyUserID=woi.ReceiverID and woi.ReceiverID != -2 and woi.ReceiverID is not null";
                }
                //可复核-否
                else if (query.IsReCheck == "0")
                {
                    //条件为：
                    where += " and (";
                    //状态不为处理中
                    where += " (woi.WorkOrderStatus!=" + (int)Entities.WorkOrderStatus.Processing + ")";
                    where += " or ";
                    //或 （状态为处理中 且 当前处理人 != 最后操作人 且 当前处理人不能为空）
                    where += " ( woi.WorkOrderStatus=" + (int)Entities.WorkOrderStatus.Processing + " and woi.ModifyUserID!=woi.ReceiverID and woi.ReceiverID != -2 and woi.ReceiverID is not null)";
                    where += " or ";
                    //或 当前处理人为空
                    where += " ( woi.ReceiverID = -2 or woi.ReceiverID is null)";
                    where += " )";
                }
            }

            if (query.AreaID != Constant.STRING_INVALID_VALUE && query.AreaID != "-1")
            {
                //大区逻辑变化 bif qiangfei 2014-12-17
                where += " And CC2012.dbo.fn_GetDistrict(woi.ProvinceID,woi.CityID,woi.CountyID)='" + StringHelper.SqlFilter(query.AreaID) + "'";
            }
            if (query.DemandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And DemandID='" + Utils.StringHelper.SqlFilter(query.DemandID) + "'";
            }
            if (query.TagBGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND wtag.RecID IN ( SELECT RecID FROM dbo.WorkOrderTag WHERE status in (0,1) AND bgid=" + query.TagBGID + " ) ";
            }
            if (query.TagID != Constant.INT_INVALID_VALUE)
            {
                where += string.Format(" AND wtag.RecID IN (SELECT RecID FROM dbo.WorkOrderTag WHERE RecID = {0} OR PID = {0})", query.TagID);
            }
            //if (query.TagIds != Constant.STRING_INVALID_VALUE && query.TagIds != "-2")
            //{
            //    where += " AND wtag.RecID in (" + Dal.Util.SqlFilterByInCondition(query.TagIds) + ") ";
            //}
            return where;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderInfo GetWorkOrderInfo(string OrderID)
        {
            QueryWorkOrderInfo query = new QueryWorkOrderInfo();
            query.OrderID = OrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.WorkOrderInfo LoadSingleWorkOrderInfo(DataRow row)
        {
            Entities.WorkOrderInfo model = new Entities.WorkOrderInfo();

            model.OrderID = row["OrderID"].ToString();
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            if (row["CategoryID"].ToString() != "")
            {
                model.CategoryID = int.Parse(row["CategoryID"].ToString());
            }
            if (row["DataSource"].ToString() != "")
            {
                model.DataSource = int.Parse(row["DataSource"].ToString());
            }
            model.CustName = row["CustName"].ToString();
            model.CRMCustID = row["CRMCustID"].ToString();
            if (row["ProvinceID"].ToString() != "")
            {
                model.ProvinceID = int.Parse(row["ProvinceID"].ToString());
            }
            if (row["CityID"].ToString() != "")
            {
                model.CityID = int.Parse(row["CityID"].ToString());
            }
            if (row["CountyID"].ToString() != "")
            {
                model.CountyID = int.Parse(row["CountyID"].ToString());
            }
            model.Contact = row["Contact"].ToString();
            model.ContactTel = row["ContactTel"].ToString();
            if (row["PriorityLevel"].ToString() != "")
            {
                model.PriorityLevel = int.Parse(row["PriorityLevel"].ToString());
            }
            model.LastProcessDate = row["LastProcessDate"].ToString();
            if (row["IsComplaintType"].ToString() != "")
            {
                if ((row["IsComplaintType"].ToString() == "1") || (row["IsComplaintType"].ToString().ToLower() == "true"))
                {
                    model.IsComplaintType = true;
                }
                else
                {
                    model.IsComplaintType = false;
                }
            }
            model.Title = row["Title"].ToString();
            if (row["WorkOrderStatus"].ToString() != "")
            {
                model.WorkOrderStatus = int.Parse(row["WorkOrderStatus"].ToString());
            }
            model.Content = row["Content"].ToString();
            if (row["ReceiverID"].ToString() != "")
            {
                model.ReceiverID = int.Parse(row["ReceiverID"].ToString());
            }
            model.ReceiverName = row["ReceiverName"].ToString();
            model.ReceiverDepartName = row["ReceiverDepartName"].ToString();
            if (row["IsSales"].ToString() != "")
            {
                if ((row["IsSales"].ToString() == "1") || (row["IsSales"].ToString().ToLower() == "true"))
                {
                    model.IsSales = true;
                }
                else
                {
                    model.IsSales = false;
                }
            }
            if (row["AttentionCarBrandID"].ToString() != "")
            {
                model.AttentionCarBrandID = int.Parse(row["AttentionCarBrandID"].ToString());
            }
            if (row["AttentionCarSerialID"].ToString() != "")
            {
                model.AttentionCarSerialID = int.Parse(row["AttentionCarSerialID"].ToString());
            }
            if (row["AttentionCarTypeID"].ToString() != "")
            {
                model.AttentionCarTypeID = int.Parse(row["AttentionCarTypeID"].ToString());
            }
            model.AttentionCarBrandName = row["AttentionCarBrandName"].ToString();
            model.AttentionCarSerialName = row["AttentionCarSerialName"].ToString();
            model.AttentionCarTypeName = row["AttentionCarTypeName"].ToString();
            model.SelectDealerID = row["SelectDealerID"].ToString();
            model.SelectDealerName = row["SelectDealerName"].ToString();
            if (row["IsReturnVisit"].ToString() != "")
            {
                model.IsReturnVisit = int.Parse(row["IsReturnVisit"].ToString());
            }
            model.NominateActivity = row["NominateActivity"].ToString();
            if (row["SaleCarBrandID"].ToString() != "")
            {
                model.SaleCarBrandID = int.Parse(row["SaleCarBrandID"].ToString());
            }
            if (row["SaleCarSerialID"].ToString() != "")
            {
                model.SaleCarSerialID = int.Parse(row["SaleCarSerialID"].ToString());
            }
            if (row["SaleCarTypeID"].ToString() != "")
            {
                model.SaleCarTypeID = int.Parse(row["SaleCarTypeID"].ToString());
            }
            model.SaleCarBrandName = row["SaleCarBrandName"].ToString();
            model.SaleCarSerialName = row["SaleCarSerialName"].ToString();
            model.SaleCarTypeName = row["SaleCarTypeName"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["IsRevert"].ToString() != "")
            {
                if ((row["IsRevert"].ToString() == "1") || (row["IsRevert"].ToString().ToLower() == "true"))
                {
                    model.IsRevert = true;
                }
                else
                {
                    model.IsRevert = false;
                }
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
            if (row["WorkCategory"].ToString() != "" && row["WorkCategory"].ToString() != "-2")
            {
                model.WorkCategory = int.Parse(row["WorkCategory"].ToString());
            }
            if (row["GUID"].ToString() != "")
            {
                model.GUID = (Guid)row["GUID"];
            }
            if (row["DemandID"].ToString() != "")
            {
                model.DemandID = row["DemandID"].ToString();
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public string Insert(Entities.WorkOrderInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@DataSource", SqlDbType.Int,4),
					new SqlParameter("@CustName", SqlDbType.NVarChar,50),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@Contact", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactTel", SqlDbType.VarChar,50),
					new SqlParameter("@PriorityLevel", SqlDbType.Int,4),
					new SqlParameter("@LastProcessDate", SqlDbType.VarChar,10),
					new SqlParameter("@IsComplaintType", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@WorkOrderStatus", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@ReceiverID", SqlDbType.Int,4),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsSales", SqlDbType.Bit,1),
					new SqlParameter("@AttentionCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@SelectDealerID", SqlDbType.VarChar,50),
					new SqlParameter("@SelectDealerName", SqlDbType.VarChar,500),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsRevert", SqlDbType.Bit,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@WorkCategory", SqlDbType.Int),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
                    new SqlParameter("@DemandID",SqlDbType.NVarChar,50),
                    new SqlParameter("@CallID",SqlDbType.BigInt)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CategoryID;
            parameters[2].Value = model.DataSource;
            parameters[3].Value = model.CustName;
            parameters[4].Value = model.CRMCustID;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.CityID;
            parameters[7].Value = model.CountyID;
            parameters[8].Value = model.Contact;
            parameters[9].Value = model.ContactTel;
            parameters[10].Value = model.PriorityLevel;
            parameters[11].Value = model.LastProcessDate;
            parameters[12].Value = model.IsComplaintType;
            parameters[13].Value = model.Title;
            parameters[14].Value = model.WorkOrderStatus;
            parameters[15].Value = model.Content;
            parameters[16].Value = model.ReceiverID;
            parameters[17].Value = model.ReceiverName;
            parameters[18].Value = model.ReceiverDepartName;
            parameters[19].Value = model.IsSales;
            parameters[20].Value = model.AttentionCarBrandID;
            parameters[21].Value = model.AttentionCarSerialID;
            parameters[22].Value = model.AttentionCarTypeID;
            parameters[23].Value = model.AttentionCarBrandName;
            parameters[24].Value = model.AttentionCarSerialName;
            parameters[25].Value = model.AttentionCarTypeName;
            parameters[26].Value = model.SelectDealerID;
            parameters[27].Value = model.SelectDealerName;
            parameters[28].Value = model.IsReturnVisit;
            parameters[29].Value = model.NominateActivity;
            parameters[30].Value = model.SaleCarBrandID;
            parameters[31].Value = model.SaleCarSerialID;
            parameters[32].Value = model.SaleCarTypeID;
            parameters[33].Value = model.SaleCarBrandName;
            parameters[34].Value = model.SaleCarSerialName;
            parameters[35].Value = model.SaleCarTypeName;
            parameters[36].Value = model.Status;
            parameters[37].Value = model.IsRevert;
            parameters[38].Value = model.CreateTime;
            parameters[39].Value = model.CreateUserID;
            parameters[40].Value = model.ModifyTime;
            parameters[41].Value = model.ModifyUserID;
            parameters[42].Value = model.WorkCategory;
            parameters[43].Value = model.BGID;
            parameters[44].Value = model.SCID;
            parameters[45].Value = model.DemandID;
            parameters[46].Value = model.CallID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_INSERT, parameters);

            return parameters[0].Value.ToString();
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public string Insert(SqlTransaction sqltran, Entities.WorkOrderInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@DataSource", SqlDbType.Int,4),
					new SqlParameter("@CustName", SqlDbType.NVarChar,50),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@Contact", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactTel", SqlDbType.VarChar,50),
					new SqlParameter("@PriorityLevel", SqlDbType.Int,4),
					new SqlParameter("@LastProcessDate", SqlDbType.VarChar,10),
					new SqlParameter("@IsComplaintType", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@WorkOrderStatus", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@ReceiverID", SqlDbType.Int,4),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsSales", SqlDbType.Bit,1),
					new SqlParameter("@AttentionCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@SelectDealerID", SqlDbType.VarChar,50),
					new SqlParameter("@SelectDealerName", SqlDbType.VarChar,500),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsRevert", SqlDbType.Bit,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@WorkCategory", SqlDbType.Int),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
                    new SqlParameter("@DemandID",SqlDbType.NVarChar,50),
                    new SqlParameter("@CallID",SqlDbType.BigInt)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CategoryID;
            parameters[2].Value = model.DataSource;
            parameters[3].Value = model.CustName;
            parameters[4].Value = model.CRMCustID;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.CityID;
            parameters[7].Value = model.CountyID;
            parameters[8].Value = model.Contact;
            parameters[9].Value = model.ContactTel;
            parameters[10].Value = model.PriorityLevel;
            parameters[11].Value = model.LastProcessDate;
            parameters[12].Value = model.IsComplaintType;
            parameters[13].Value = model.Title;
            parameters[14].Value = model.WorkOrderStatus;
            parameters[15].Value = model.Content;
            parameters[16].Value = model.ReceiverID;
            parameters[17].Value = model.ReceiverName;
            parameters[18].Value = model.ReceiverDepartName;
            parameters[19].Value = model.IsSales;
            parameters[20].Value = model.AttentionCarBrandID;
            parameters[21].Value = model.AttentionCarSerialID;
            parameters[22].Value = model.AttentionCarTypeID;
            parameters[23].Value = model.AttentionCarBrandName;
            parameters[24].Value = model.AttentionCarSerialName;
            parameters[25].Value = model.AttentionCarTypeName;
            parameters[26].Value = model.SelectDealerID;
            parameters[27].Value = model.SelectDealerName;
            parameters[28].Value = model.IsReturnVisit;
            parameters[29].Value = model.NominateActivity;
            parameters[30].Value = model.SaleCarBrandID;
            parameters[31].Value = model.SaleCarSerialID;
            parameters[32].Value = model.SaleCarTypeID;
            parameters[33].Value = model.SaleCarBrandName;
            parameters[34].Value = model.SaleCarSerialName;
            parameters[35].Value = model.SaleCarTypeName;
            parameters[36].Value = model.Status;
            parameters[37].Value = model.IsRevert;
            parameters[38].Value = model.CreateTime;
            parameters[39].Value = model.CreateUserID;
            parameters[40].Value = model.ModifyTime;
            parameters[41].Value = model.ModifyUserID;
            parameters[42].Value = model.WorkCategory;
            parameters[43].Value = model.BGID;
            parameters[44].Value = model.SCID;
            parameters[45].Value = model.DemandID;
            parameters[46].Value = model.CallID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERINFO_INSERT, parameters);

            return parameters[0].Value.ToString();
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@DataSource", SqlDbType.Int,4),
					new SqlParameter("@CustName", SqlDbType.NVarChar,50),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@Contact", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactTel", SqlDbType.VarChar,50),
					new SqlParameter("@PriorityLevel", SqlDbType.Int,4),
					new SqlParameter("@LastProcessDate", SqlDbType.VarChar,10),
					new SqlParameter("@IsComplaintType", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@WorkOrderStatus", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@ReceiverID", SqlDbType.Int,4),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsSales", SqlDbType.Bit,1),
					new SqlParameter("@AttentionCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@SelectDealerID", SqlDbType.VarChar,50),
					new SqlParameter("@SelectDealerName", SqlDbType.VarChar,500),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsRevert", SqlDbType.Bit,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@WorkCategory", SqlDbType.Int),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
                    new SqlParameter("@DemandID",SqlDbType.NVarChar,50)};
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.CategoryID;
            parameters[2].Value = model.DataSource;
            parameters[3].Value = model.CustName;
            parameters[4].Value = model.CRMCustID;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.CityID;
            parameters[7].Value = model.CountyID;
            parameters[8].Value = model.Contact;
            parameters[9].Value = model.ContactTel;
            parameters[10].Value = model.PriorityLevel;
            parameters[11].Value = model.LastProcessDate;
            parameters[12].Value = model.IsComplaintType;
            parameters[13].Value = model.Title;
            parameters[14].Value = model.WorkOrderStatus;
            parameters[15].Value = model.Content;
            parameters[16].Value = model.ReceiverID;
            parameters[17].Value = model.ReceiverName;
            parameters[18].Value = model.ReceiverDepartName;
            parameters[19].Value = model.IsSales;
            parameters[20].Value = model.AttentionCarBrandID;
            parameters[21].Value = model.AttentionCarSerialID;
            parameters[22].Value = model.AttentionCarTypeID;
            parameters[23].Value = model.AttentionCarBrandName;
            parameters[24].Value = model.AttentionCarSerialName;
            parameters[25].Value = model.AttentionCarTypeName;
            parameters[26].Value = model.SelectDealerID;
            parameters[27].Value = model.SelectDealerName;
            parameters[28].Value = model.IsReturnVisit;
            parameters[29].Value = model.NominateActivity;
            parameters[30].Value = model.SaleCarBrandID;
            parameters[31].Value = model.SaleCarSerialID;
            parameters[32].Value = model.SaleCarTypeID;
            parameters[33].Value = model.SaleCarBrandName;
            parameters[34].Value = model.SaleCarSerialName;
            parameters[35].Value = model.SaleCarTypeName;
            parameters[36].Value = model.Status;
            parameters[37].Value = model.IsRevert;
            parameters[38].Value = model.CreateTime;
            parameters[39].Value = model.CreateUserID;
            parameters[40].Value = model.ModifyTime;
            parameters[41].Value = model.ModifyUserID;
            parameters[42].Value = model.WorkCategory;
            parameters[43].Value = model.BGID;
            parameters[44].Value = model.SCID;
            parameters[45].Value = model.DemandID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@DataSource", SqlDbType.Int,4),
					new SqlParameter("@CustName", SqlDbType.NVarChar,50),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@Contact", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactTel", SqlDbType.VarChar,50),
					new SqlParameter("@PriorityLevel", SqlDbType.Int,4),
					new SqlParameter("@LastProcessDate", SqlDbType.VarChar,10),
					new SqlParameter("@IsComplaintType", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@WorkOrderStatus", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@ReceiverID", SqlDbType.Int,4),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsSales", SqlDbType.Bit,1),
					new SqlParameter("@AttentionCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@SelectDealerID", SqlDbType.VarChar,50),
					new SqlParameter("@SelectDealerName", SqlDbType.VarChar,500),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleCarBrandID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarTypeID", SqlDbType.Int,4),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsRevert", SqlDbType.Bit,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@WorkCategory", SqlDbType.Int),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
                    new SqlParameter("@DemandID",SqlDbType.NVarChar,50)};
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.CategoryID;
            parameters[2].Value = model.DataSource;
            parameters[3].Value = model.CustName;
            parameters[4].Value = model.CRMCustID;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.CityID;
            parameters[7].Value = model.CountyID;
            parameters[8].Value = model.Contact;
            parameters[9].Value = model.ContactTel;
            parameters[10].Value = model.PriorityLevel;
            parameters[11].Value = model.LastProcessDate;
            parameters[12].Value = model.IsComplaintType;
            parameters[13].Value = model.Title;
            parameters[14].Value = model.WorkOrderStatus;
            parameters[15].Value = model.Content;
            parameters[16].Value = model.ReceiverID;
            parameters[17].Value = model.ReceiverName;
            parameters[18].Value = model.ReceiverDepartName;
            parameters[19].Value = model.IsSales;
            parameters[20].Value = model.AttentionCarBrandID;
            parameters[21].Value = model.AttentionCarSerialID;
            parameters[22].Value = model.AttentionCarTypeID;
            parameters[23].Value = model.AttentionCarBrandName;
            parameters[24].Value = model.AttentionCarSerialName;
            parameters[25].Value = model.AttentionCarTypeName;
            parameters[26].Value = model.SelectDealerID;
            parameters[27].Value = model.SelectDealerName;
            parameters[28].Value = model.IsReturnVisit;
            parameters[29].Value = model.NominateActivity;
            parameters[30].Value = model.SaleCarBrandID;
            parameters[31].Value = model.SaleCarSerialID;
            parameters[32].Value = model.SaleCarTypeID;
            parameters[33].Value = model.SaleCarBrandName;
            parameters[34].Value = model.SaleCarSerialName;
            parameters[35].Value = model.SaleCarTypeName;
            parameters[36].Value = model.Status;
            parameters[37].Value = model.IsRevert;
            parameters[38].Value = model.CreateTime;
            parameters[39].Value = model.CreateUserID;
            parameters[40].Value = model.ModifyTime;
            parameters[41].Value = model.ModifyUserID;
            parameters[42].Value = model.WorkCategory;
            parameters[43].Value = model.BGID;
            parameters[44].Value = model.SCID;
            parameters[45].Value = model.DemandID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,50)};
            parameters[0].Value = OrderID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,50)};
            parameters[0].Value = OrderID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERINFO_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 根据 表获取总数量
        /// </summary> 
        /// <returns></returns>
        public int GetMax()
        {
            int intval = 0;
            string sqlStr = "select max(CAST (SUBSTRING(OrderID,7,len(OrderID)-6) as int)) from WorkOrderInfo";

            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            int.TryParse(o.ToString(), out intval);

            return intval;
        }

        public DataTable GetProcessOrderUserID(string orderID)
        {
            string sql = @"
                        SELECT  createUserID
                        FROM    ( SELECT    wi.createUserID
                                  FROM      workorderinfo wi
                                  WHERE     wi.STATUS = 0
                                            AND orderID = '" + StringHelper.SqlFilter(orderID) + @"'
                                  UNION
                                  SELECT    wr.createUserID
                                  FROM      WorkOrderReceiver wr
                                  WHERE     1 = 1
                                            AND orderID = '" + StringHelper.SqlFilter(orderID) + @"'
                                  UNION
                                  SELECT    wi.ReceiverUserID
                                  FROM      WorkOrderReceiver wi
                                  WHERE     ReceiverUserID != -2
                                            AND ReceiverUserID IS NOT NULL
                                            AND orderID = '" + StringHelper.SqlFilter(orderID) + @"'
                                ) a
            ";

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取所有工单的创建人
        /// </summary>
        /// <returns></returns>
        public DataTable GetCreateUser(string where, string order, int currentPage, int pageSize, out int totalCount)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 200),
					new SqlParameter("@order", SqlDbType.NVarChar, 50),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERINFO_SELECTCREATEUSER, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 查询某部门及其子部门
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public DataTable GetChildDepartMent(string[] DepartIDs)
        {
            string sqlstr = "";
            foreach (string strID in DepartIDs)
            {
                sqlstr += "SELECT ID FROM SysRightsManager.dbo.f_Cid('" + strID + "')   UNION ALL ";
            }
            sqlstr = sqlstr.Remove(sqlstr.Length - 10);
            return SqlHelper.ExecuteDataset(ConnectionStrings_SYS, CommandType.Text, sqlstr, null).Tables[0];
        }
        /// <summary>
        /// 根据工单ID获取录音URL
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderRecordUrl_OrderID(string orderid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar, 20)
					};

            parameters[0].Value = orderid;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_GetWorkOrderRecordUrl_OrderID", parameters);

            return ds.Tables[0];
        }

        /// <summary>
        /// 惠买车工单数据
        /// </summary>
        /// <param name="query">WorkCategory(1:个人2:经销商)</param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable WorkOrderInfoExportHMC(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.WorkCategory != Constant.INT_INVALID_VALUE)
            {
                where += " AND woi.WorkCategory = " + query.WorkCategory;
            }

            DateTime beginTime;
            if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            {
                where += " and woi.CreateTime>='" + beginTime + "'";
            }
            DateTime endTime;
            if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            {
                where += " and woi.CreateTime<'" + endTime.AddDays(1) + "'";
            }
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 200),
					new SqlParameter("@order", SqlDbType.NVarChar, 50),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_WorkOrderInfo_Export_HMC", parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());
            return ds.Tables[0];
        }

        public bool HasConversation(string OrderID)
        {
            string strSql = "SELECT Count(*) FROM dbo.v_Conversations WHERE OrderID='" + SqlFilter(OrderID) + "'";

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);

            if (obj != null)
            {
                string objValue = obj.ToString();
                if (int.Parse(objValue) == 0)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCustIDByWorkOrderID(string orderid)
        {
            string sql = @"SELECT TOP 1 CustID from dbo.CustHistoryInfo where TaskID='" + orderid + @"' AND BusinessType=1
                                    ORDER BY CreateTime DESC ";
            return CommonFunction.ObjectToString(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }
    }
}

