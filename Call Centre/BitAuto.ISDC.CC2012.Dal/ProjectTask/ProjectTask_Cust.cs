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
    /// 数据访问类ProjectTask_Cust。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:30 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_Cust : DataBase
    {
        #region Instance
        public static readonly ProjectTask_Cust Instance = new ProjectTask_Cust();
        #endregion

        #region const
        private const string P_PROJECTTASK_CUST_SELECT = "p_ProjectTask_Cust_Select";
        private const string P_PROJECTTASK_CUST_SELECT_BY_ID = "p_ProjectTask_Cust_select_by_id";
        private const string P_PROJECTTASK_CUST_INSERT = "p_ProjectTask_Cust_Insert";
        private const string P_PROJECTTASK_CUST_UPDATE = "p_ProjectTask_Cust_Update";
        private const string P_PROJECTTASK_CUST_DELETE = "p_ProjectTask_Cust_Delete";
        private const string P_PROJECTTASK_CUST_SYNCINFOFROMCRM = "p_ProjectTask_Cust_SyncInfoFromCrm";
        #endregion

        #region Contructor
        protected ProjectTask_Cust()
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
        public DataTable GetProjectTask_Cust(QueryProjectTask_Cust query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.CustName like '%" + SqlFilter(query.CustName) + "%'";
            }
            if (query.AbbrName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.AbbrName like '%" + SqlFilter(query.AbbrName) + "%'";
            }
            if (query.UserName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.lastUpdateUserID =(select userid from Crm2009.dbo.v_userinfo where truename='" + SqlFilter(query.UserName) + "')";
            }
            if (query.SubmitUserName != Constant.STRING_INVALID_VALUE)
            {
                where += " and (SELECT TOP 1 truename FROM Crm2009.dbo.v_userinfo WHERE USERID =(SELECT TOP 1 UserID FROM dbo.ProjectTaskInfo WHERE PTID=ProjectTask_Cust.PTID Order by CreateTime Desc))='" + SqlFilter(query.SubmitUserName) + "'";
            }
            if (query.Brandids != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.PTID in  ( select PTID from  ProjectTask_Cust_Brand where BrandID in (" + Dal.Util.SqlFilterByInCondition(query.Brandids) + ") ) ";
            }

            if (query.ProvinceID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.ProvinceID='" + SqlFilter(query.ProvinceID) + "'";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.CityID='" + SqlFilter(query.CityID) + "'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.CountyID='" + SqlFilter(query.CountyID) + "'";
            }
            //if (queryCC_Custs.SubmitTime != Constant.STRING_INVALID_VALUE)
            //{
            //    where += " and dbo.fn_DateToString(CC_Custs.lastUpdateTime,'yyyy-MM-dd')>= '" + SqlFilter(queryCC_Custs.SubmitTime) + "'";
            //}
            if (query.BeginSubmitTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and dbo.fn_DateToString((SELECT TOP 1 CreateTime FROM dbo.ProjectTaskLog WHERE PTID=ProjectTask_Cust.PTID Order by CreateTime Desc),'yyyy-MM-dd')>= '" + SqlFilter(query.BeginSubmitTime) + "'";
            }
            if (query.EndSubmitTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and dbo.fn_DateToString((SELECT TOP 1 CreateTime FROM dbo.ProjectTaskLog WHERE PTID=ProjectTask_Cust.PTID Order by CreateTime Desc),'yyyy-MM-dd')<= '" + SqlFilter(query.EndSubmitTime) + "'";
            }

            if (query.TaskStatus == null)
            {
                where += " and 1!=1 ";
            }
            else if (query.TaskStatus != null && query.TaskStatus.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Entities.EnumTaskStatus s in query.TaskStatus)
                {
                    sb.Append(((int)s) + ",");
                }
                where += string.Format(" and ProjectTaskInfo.TaskStatus in ({0})", sb.ToString().Trim(','));
            }

            if (query.TopOneTaskStatus.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Entities.EnumTaskStatus s in query.TopOneTaskStatus)
                {
                    sb.Append(((int)s) + ",");
                }
                where += string.Format(" and ctl.TaskStatus in ({0})", sb.ToString().Trim(','));
            }

            if (query.FailureReason != Constant.STRING_INVALID_VALUE)
            {
                if (query.FailureReason.Equals("其它异常"))
                {
                    where += " and (SELECT TOP 1 Description FROM dbo.ProjectTaskLog WHERE PTID=ProjectTask_Cust.PTID ORDER BY CreateTime DESC) Not IN('提交审核失败，与CRM系统中的客户名称重复','提交审核失败，与CRM系统中的会员简称重复') ";
                }
                else
                {
                    where += " and (SELECT TOP 1 Description FROM dbo.ProjectTaskLog WHERE PTID=ProjectTask_Cust.PTID ORDER BY CreateTime DESC) = '" + SqlFilter(query.FailureReason) + "' ";
                }
            }
            if (query.CarType != Constant.STRING_EMPTY_VALUE && query.CarType != null)
            {
                where += " and ProjectTask_Cust.CarType in (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ")";
            }
            if (query.TaskSource != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectTaskInfo.Source=" + query.TaskSource;
            }

            //状态为有效
            where += " and ProjectTaskInfo.Status= 0";

            DataSet ds;
            SqlParameter[] parameters = {
                     new SqlParameter("@where", SqlDbType.VarChar,8000),
			new SqlParameter("@order", SqlDbType.NVarChar,100),
			new SqlParameter("@pagesize", SqlDbType.Int,4),
			new SqlParameter("@page", SqlDbType.Int,4),
			new SqlParameter("@totalRecorder", SqlDbType.Int,4)
             };

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_Cust GetProjectTask_Cust(string PTID)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@PTid", SqlDbType.VarChar)
            };

            parameters[0].Value = PTID;
            //绑定存储过程参数

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleCC_Custs(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }
        private Entities.ProjectTask_Cust LoadSingleProjectTask_Cust(DataRow row)
        {
            Entities.ProjectTask_Cust model = new Entities.ProjectTask_Cust();
            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["OriginalCustID"] != DBNull.Value)
            {
                model.OriginalCustID = row["OriginalCustID"].ToString();
            }

            if (row["CustName"] != DBNull.Value)
            {
                model.CustName = row["CustName"].ToString();
            }

            if (row["AbbrName"] != DBNull.Value)
            {
                model.AbbrName = row["AbbrName"].ToString();
            }

            if (row["LevelID"] != DBNull.Value)
            {
                model.LevelID = row["LevelID"].ToString();
            }

            if (row["IndustryID"] != DBNull.Value)
            {
                model.IndustryID = row["IndustryID"].ToString();
            }

            if (row["TypeID"] != DBNull.Value)
            {
                model.TypeID = row["TypeID"].ToString();
            }

            if (row["CustPid"] != DBNull.Value)
            {
                model.CustPid = row["CustPid"].ToString();
            }

            if (row["Pid"] != DBNull.Value)
            {
                model.Pid = row["Pid"].ToString();
            }

            if (row["ShopLevel"] != DBNull.Value)
            {
                model.ShopLevel = row["ShopLevel"].ToString();
            }

            if (row["ProvinceID"] != DBNull.Value)
            {
                model.ProvinceID = row["ProvinceID"].ToString();
            }

            if (row["CityID"] != DBNull.Value)
            {
                model.CityID = row["CityID"].ToString();
            }

            if (row["CountyID"] != DBNull.Value)
            {
                model.CountyID = row["CountyID"].ToString();
            }

            if (row["Address"] != DBNull.Value)
            {
                model.Address = row["Address"].ToString();
            }

            if (row["Zipcode"] != DBNull.Value)
            {
                model.Zipcode = row["Zipcode"].ToString();
            }

            if (row["OfficeTel"] != DBNull.Value)
            {
                model.OfficeTel = row["OfficeTel"].ToString();
            }

            if (row["Fax"] != DBNull.Value)
            {
                model.Fax = row["Fax"].ToString();
            }

            if (row["Notes"] != DBNull.Value)
            {
                model.Notes = row["Notes"].ToString();
            }

            if (row["ContactName"] != DBNull.Value)
            {
                model.ContactName = row["ContactName"].ToString();
            }

            if (row["CreateUserID"] != DBNull.Value)
            {
                int i = -1;
                if (int.TryParse(row["CreateUserID"].ToString(), out i))
                {
                    model.CreateUserID = i;
                }

            }
            if (row["LastUpdateUserID"] != DBNull.Value)
            {
                int i = -1;
                if (int.TryParse(row["LastUpdateUserID"].ToString(), out i))
                {
                    model.LastUpdateUserID = i;
                }
            }
            if (row["CustStatus"] != DBNull.Value)
            {
                model.Status = int.Parse(row["CustStatus"].ToString());
            }
            if (row["Lock"] != DBNull.Value)
            {
                model.Lock = int.Parse(row["Lock"].ToString());
            }

            ////经营范围 
            //if (row["CarType"] != DBNull.Value)
            //{
            //    model.CarType = int.Parse(row["CarType"].ToString());
            //}
            ////二手车经营类型 
            //if (row["UsedCarBusinessType"] != DBNull.Value)
            //{
            //    model.UsedCarBusinessType = row["UsedCarBusinessType"].ToString();
            //}
            ////车商通会员ID 
            //if (row["CstMemberID"] != DBNull.Value)
            //{
            //    model.CstMemberID = row["CstMemberID"].ToString();
            //}

            ////交易市场
            //if (row["TradeMarketID"] != DBNull.Value)
            //{
            //    model.TradeMarketID = row["TradeMarketID"].ToString();
            //}
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.ProjectTask_Cust model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@OriginalCustID", SqlDbType.VarChar,50),
					new SqlParameter("@CustName", SqlDbType.VarChar,100),
					new SqlParameter("@AbbrName", SqlDbType.VarChar,50),
					new SqlParameter("@LevelID", SqlDbType.VarChar,20),
					new SqlParameter("@IndustryID", SqlDbType.VarChar,20),
					new SqlParameter("@TypeID", SqlDbType.VarChar,20),
					new SqlParameter("@CustPid ", SqlDbType.VarChar,50),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@ShopLevel", SqlDbType.VarChar,20),
					new SqlParameter("@ProvinceID", SqlDbType.VarChar,20),
					new SqlParameter("@CityID", SqlDbType.VarChar,20),
					new SqlParameter("@CountyID", SqlDbType.VarChar,20),
					new SqlParameter("@Address", SqlDbType.VarChar,400),
					new SqlParameter("@Zipcode", SqlDbType.VarChar,10),
					new SqlParameter("@OfficeTel", SqlDbType.VarChar,50),
					new SqlParameter("@Fax", SqlDbType.VarChar,50),
					new SqlParameter("@Notes", SqlDbType.VarChar,2000),
					new SqlParameter("@ContactName", SqlDbType.VarChar,50),
                    new SqlParameter("@TradeMarketID",SqlDbType.VarChar,50),
                    new SqlParameter("@CarType",SqlDbType.Int,4),
                    new SqlParameter("@FoursPid",SqlDbType.VarChar)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.OriginalCustID;
            parameters[2].Value = model.CustName;
            parameters[3].Value = model.AbbrName;
            parameters[4].Value = model.LevelID;
            parameters[5].Value = model.IndustryID;
            parameters[6].Value = model.TypeID;
            parameters[7].Value = model.CustPid;
            parameters[8].Value = model.Pid;
            parameters[9].Value = model.ShopLevel;
            parameters[10].Value = model.ProvinceID;
            parameters[11].Value = model.CityID;
            parameters[12].Value = model.CountyID;
            parameters[13].Value = model.Address;
            parameters[14].Value = model.Zipcode;
            parameters[15].Value = model.OfficeTel;
            parameters[16].Value = model.Fax;
            parameters[17].Value = model.Notes;
            parameters[18].Value = model.ContactName;
            parameters[19].Value = model.TradeMarketID;
            parameters[20].Value = model.CarType;
            parameters[21].Value = model.FoursPid;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_INSERT, parameters);

        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_Cust model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@OriginalCustID", SqlDbType.VarChar,50),
					new SqlParameter("@CustName", SqlDbType.VarChar,100),
					new SqlParameter("@AbbrName", SqlDbType.VarChar,50),
					new SqlParameter("@LevelID", SqlDbType.VarChar,20),
					new SqlParameter("@IndustryID", SqlDbType.VarChar,20),
					new SqlParameter("@TypeID", SqlDbType.VarChar,20),
					new SqlParameter("@CustPid ", SqlDbType.VarChar,50),
					new SqlParameter("@Pid", SqlDbType.VarChar,50),
					new SqlParameter("@ShopLevel", SqlDbType.VarChar,20),
					new SqlParameter("@ProvinceID", SqlDbType.VarChar,20),
					new SqlParameter("@CityID", SqlDbType.VarChar,20),
					new SqlParameter("@CountyID", SqlDbType.VarChar,20),
					new SqlParameter("@Address", SqlDbType.VarChar,400),
					new SqlParameter("@Zipcode", SqlDbType.VarChar,10),
					new SqlParameter("@OfficeTel", SqlDbType.VarChar,50),
					new SqlParameter("@Fax", SqlDbType.VarChar,50),
					new SqlParameter("@Notes", SqlDbType.VarChar,2000),
					new SqlParameter("@ContactName", SqlDbType.VarChar,50),
					new SqlParameter("@lastUpdateUserID", SqlDbType.Int),
                    new SqlParameter("@lastUpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@TradeMarketID", SqlDbType.VarChar,50),
                    new SqlParameter("@CarType", SqlDbType.Int),
                    new SqlParameter("@CstMemberID", SqlDbType.VarChar,50),
                    new SqlParameter("@UsedCarBusinessType", SqlDbType.VarChar,50),
                    new SqlParameter("@FoursPid", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.OriginalCustID;
            parameters[2].Value = model.CustName;
            parameters[3].Value = model.AbbrName;
            parameters[4].Value = model.LevelID;
            parameters[5].Value = model.IndustryID;
            parameters[6].Value = model.TypeID;
            parameters[7].Value = model.CustPid;
            parameters[8].Value = model.Pid;
            parameters[9].Value = model.ShopLevel;
            parameters[10].Value = model.ProvinceID;
            parameters[11].Value = model.CityID;
            parameters[12].Value = model.CountyID;
            parameters[13].Value = model.Address;
            parameters[14].Value = model.Zipcode;
            parameters[15].Value = model.OfficeTel;
            parameters[16].Value = model.Fax;
            parameters[17].Value = model.Notes;
            parameters[18].Value = model.ContactName;
            parameters[19].Value = model.LastUpdateUserID;
            parameters[20].Value = model.LastUpdateTime;

            parameters[21].Value = model.TradeMarketID;
            parameters[22].Value = model.CarType;
            parameters[23].Value = model.CstMemberID;
            parameters[24].Value = model.UsedCarBusinessType;
            parameters[25].Value = model.FoursPid;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_UPDATE, parameters);
        }
        #endregion

        #region SelectSingle
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="rid">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        public Entities.ProjectTask_Cust GetProjectTask_Cust(int tid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                                            new SqlParameter("@PTID", SqlDbType.VarChar)
            };

            parameters[0].Value = tid;
            //绑定存储过程参数

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleCC_Custs(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }

        private static Entities.ProjectTask_Cust LoadSingleCC_Custs(DataRow row)
        {
            Entities.ProjectTask_Cust model = new Entities.ProjectTask_Cust();
            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["OriginalCustID"] != DBNull.Value)
            {
                model.OriginalCustID = row["OriginalCustID"].ToString();
            }

            if (row["CustName"] != DBNull.Value)
            {
                model.CustName = row["CustName"].ToString();
            }

            if (row["AbbrName"] != DBNull.Value)
            {
                model.AbbrName = row["AbbrName"].ToString();
            }

            if (row["LevelID"] != DBNull.Value)
            {
                model.LevelID = row["LevelID"].ToString();
            }

            if (row["IndustryID"] != DBNull.Value)
            {
                model.IndustryID = row["IndustryID"].ToString();
            }

            if (row["TypeID"] != DBNull.Value)
            {
                model.TypeID = row["TypeID"].ToString();
            }

            if (row["CustPid"] != DBNull.Value)
            {
                model.CustPid = row["CustPid"].ToString();
            }

            if (row["Pid"] != DBNull.Value)
            {
                model.Pid = row["Pid"].ToString();
            }

            if (row["ShopLevel"] != DBNull.Value)
            {
                model.ShopLevel = row["ShopLevel"].ToString();
            }

            if (row["ProvinceID"] != DBNull.Value)
            {
                model.ProvinceID = row["ProvinceID"].ToString();
            }

            if (row["CityID"] != DBNull.Value)
            {
                model.CityID = row["CityID"].ToString();
            }

            if (row["CountyID"] != DBNull.Value)
            {
                model.CountyID = row["CountyID"].ToString();
            }

            if (row["Address"] != DBNull.Value)
            {
                model.Address = row["Address"].ToString();
            }

            if (row["Zipcode"] != DBNull.Value)
            {
                model.Zipcode = row["Zipcode"].ToString();
            }

            if (row["OfficeTel"] != DBNull.Value)
            {
                model.OfficeTel = row["OfficeTel"].ToString();
            }

            if (row["Fax"] != DBNull.Value)
            {
                model.Fax = row["Fax"].ToString();
            }

            if (row["Notes"] != DBNull.Value)
            {
                model.Notes = row["Notes"].ToString();
            }

            if (row["ContactName"] != DBNull.Value)
            {
                model.ContactName = row["ContactName"].ToString();
            }

            if (row["CreateUserID"] != DBNull.Value)
            {
                int i = -1;
                if (int.TryParse(row["CreateUserID"].ToString(), out i))
                {
                    model.CreateUserID = i;
                }

            }
            if (row["LastUpdateUserID"] != DBNull.Value)
            {
                int i = -1;
                if (int.TryParse(row["LastUpdateUserID"].ToString(), out i))
                {
                    model.LastUpdateUserID = i;
                }
            }
            if (row["CustStatus"] != DBNull.Value)
            {
                model.Status = int.Parse(row["CustStatus"].ToString());
            }
            if (row["Lock"] != DBNull.Value)
            {
                model.Lock = int.Parse(row["Lock"].ToString());
            }

            //经营范围 
            if (row["CarType"] != DBNull.Value)
            {
                model.CarType = int.Parse(row["CarType"].ToString());
            }
            //二手车经营类型 
            if (row["UsedCarBusinessType"] != DBNull.Value)
            {
                model.UsedCarBusinessType = row["UsedCarBusinessType"].ToString();
            }
            //车商通会员ID 
            if (row["CstMemberID"] != DBNull.Value)
            {
                model.CstMemberID = row["CstMemberID"].ToString();
            }

            //交易市场
            if (row["TradeMarketID"] != DBNull.Value)
            {
                model.TradeMarketID = row["TradeMarketID"].ToString();
            }

            //所属4S
            if (row["FoursPid"] != DBNull.Value)
            {
                model.FoursPid = row["FoursPid"].ToString();
            }
            return model;
        }
        #endregion

        #region IsExist
        public bool Exists(string tid)
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, string.Format("SELECT TOP 1 PTID FROM dbo.ProjectTask_Cust WHERE PTID='{0}'", StringHelper.SqlFilter(tid)));

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) { return true; }
            else { return false; }
        }
        #endregion

        public void SyncInfoFromCrm(Entities.ProjectTaskInfo task, int createUserID, DateTime createTime)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@CrmCustID", SqlDbType.VarChar, 50),
					new SqlParameter("@CreateUserID", SqlDbType.Int),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)
            };
            parameters[0].Value = task.PTID;
            parameters[1].Value = task.RelationID;
            parameters[2].Value = createUserID;
            parameters[3].Value = createTime;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_SYNCINFOFROMCRM, parameters);
        }

        public void EvalQueryCusts(QueryProjectTask_Cust query, string where)
        {
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.CustName like '%" + SqlFilter(query.CustName) + "%'";
            }
            if (query.AbbrName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.AbbrName like '%" + SqlFilter(query.AbbrName) + "%'";
            }
            if (query.UserName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.lastUpdateUserID =(select userid from Crm2009.dbo.v_userinfo where truename='" + SqlFilter(query.UserName) + "')";
            }
            if (query.Brandids != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.PTID in  ( select PTID from  ProjectTask_Cust_Brand where BrandID in (" + Dal.Util.SqlFilterByInCondition(query.Brandids) + ") ) ";
            }

            if (query.ProvinceID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.ProvinceID='" + SqlFilter(query.ProvinceID) + "'";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.CityID='" + SqlFilter(query.CityID) + "'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust.CountyID='" + SqlFilter(query.CountyID) + "'";
            }
            if (query.SubmitTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and dbo.fn_DateToString(ProjectTask_Cust.lastUpdateTime,'yyyy-MM-dd')>= '" + SqlFilter(query.SubmitTime) + "'";
            }
            if (query.BeginSubmitTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and dbo.fn_DateToString(ProjectTask_Cust.lastUpdateTime,'yyyy-MM-dd')>= '" + SqlFilter(query.BeginSubmitTime) + "'";
            }
            if (query.EndSubmitTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and dbo.fn_DateToString(ProjectTask_Cust.lastUpdateTime,'yyyy-MM-dd')<= '" + SqlFilter(query.EndSubmitTime) + "'";
            }

            if (query.TaskStatus.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Entities.EnumTaskStatus s in query.TaskStatus)
                {
                    sb.Append(((int)s) + ",");
                }
                where += string.Format(" and ProjectTaskInfo.TaskStatus in ({0})", sb.ToString().Trim(','));
            }


            //状态为有效
            where += " and ProjectTaskInfo.Status= 0";

        }


        public void DeleteByTID(string tid)
        {
            string sql = string.Format("Delete FROM ProjectTask_Cust Where PTID='{0}'", StringHelper.SqlFilter(tid));
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public DataTable GetProjectTask_CustByCRMCustName(string custName)
        {
            DataSet ds = null;
            string sql = string.Format(@"SELECT cc.*
                                         FROM dbo.ProjectTask_Cust AS cc
                                         INNER JOIN dbo.ProjectTaskInfo ct ON ct.PTID=cc.PTID
                                         WHERE ct.Source=1 AND cc.CustName='{0}'", StringHelper.SqlFilter(custName));
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 根据任务ID得到客户信息实体 刘学文
        /// </summary>
        /// <param name="ptid">任务ID</param>
        /// <returns></returns>
        public Entities.ProjectTask_Cust GetCustInfoModelByPTID(string ptid)
        {
            DataSet ds = null;
            string sqlStr = string.Format(@"SELECT  ptc.* ,
                                            ci.Status AS CustStatus,
                                            ci.Lock
                                    FROM    ProjectTask_Cust ptc
                                            LEFT JOIN CRM2009.dbo.CustInfo ci ON ci.CustID = ptc.OriginalCustID
                                    where PTID='" + StringHelper.SqlFilter(ptid) + "'");
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            DataTable dt = ds.Tables[0];
            if (ds != null && dt.Rows.Count > 0)
            {
                return LoadSingleProjectTask_Cust(dt.Rows[0]);
            }

            return null;
        }

        /// <summary>
        /// 通过任务ID得到客户信息和会员信息串 刘学文 13.2.25
        /// </summary>
        /// <param name="ptid">任务ID</param>
        /// <returns>Table</returns>
        public DataTable GetCustInfoByPTID(string ptid)
        {
            DataSet ds = null;
            string sqlStr = string.Format(@"SELECT  ptc.* ,
                                            STUFF(( SELECT  ',' + CAST(dms.MemberCode AS VARCHAR(50))
                                                    FROM    dbo.ProjectTask_DMSMember ptm
                                                            LEFT JOIN CRM2009.dbo.DMSMember dms ON ptm.OriginalDMSMemberID = CAST(dms.ID AS VARCHAR(50))
                                                    WHERE   ptm.PTID = ptc.PTID AND ptm.Status=0
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, '') AS MemberID ,
                                            STUFF(( SELECT  ',' + Name
                                                    FROM    dbo.ProjectTask_DMSMember ptm
                                                    WHERE   ptm.PTID = ptc.PTID AND ptm.Status=0
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, '') AS DMSName
                                    FROM    dbo.ProjectTask_Cust AS ptc
                            WHERE   PTID ='" + StringHelper.SqlFilter(ptid) + "'");
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }


        /// <summary>
        /// 通过项目ID得到客户信息和会员信息串 刘学文 13.3.14
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns>Table</returns>
        public DataTable GetCustInfoByReturnProjectID(string ProjectID)
        {
            DataSet ds = null;
            string sqlStr = string.Format(@"SELECT  ci.* ,
                                            STUFF(( SELECT  ',' + CAST(dms.MemberCode AS VARCHAR(50))
                                                    FROM    CRM2009.dbo.DMSMember dms
                                                    WHERE   ci.CustID = dms.CustID
                                                            AND dms.Status = 0
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, '') AS MemberID ,
                                            STUFF(( SELECT  ',' + Name
                                                    FROM    CRM2009.dbo.DMSMember dms
                                                    WHERE   dms.Status = 0
                                                            AND ci.CustID = dms.CustID
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, '') AS DMSName
                                    FROM    CRM2009.dbo.CustInfo AS ci
                            WHERE   CustID IN ( SELECT   ReturnVisitCustID
                       FROM     ProjectTask_SurveyAnswer
                       WHERE    ProjectID = " + StringHelper.SqlFilter(ProjectID) + " )");
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }


        /// <summary>
        /// 通过项目ID得到客户信息和会员信息串 刘学文 13.4.7
        /// </summary>
        /// <param name="ProjectID">项目ID</param>
        /// <param name="siid">问卷ID</param>
        /// <returns>Table</returns>
        public DataTable GetCustInfoByProjectID(int ProjectID, int siid)
        {
            DataSet ds = null;
            string sqlStr = string.Format(@"SELECT  ptc.* ,
                                            STUFF(( SELECT  ',' + CAST(dms.MemberCode AS VARCHAR(50))
                                                    FROM    dbo.ProjectTask_DMSMember ptm
                                                            LEFT JOIN CRM2009.dbo.DMSMember dms ON ptm.OriginalDMSMemberID = CAST(dms.ID AS VARCHAR(50))
                                                    WHERE   ptm.PTID = ptc.PTID AND ptm.Status=0
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, '') AS MemberID ,
                                            STUFF(( SELECT  ',' + Name
                                                    FROM    dbo.ProjectTask_DMSMember ptm
                                                    WHERE   ptm.PTID = ptc.PTID AND ptm.Status=0
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, '') AS DMSName
                                    FROM    dbo.ProjectTask_Cust AS ptc
                            WHERE   PTID in (select PTID From ProjectTask_SurveyAnswer where  ProjectTask_SurveyAnswer.ProjectID=" + ProjectID + " and SIID=" + siid + ")");
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

    }
}

