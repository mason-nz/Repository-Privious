using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Web;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类LeadsTask。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-05-19 11:30:49 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class LeadsTask : DataBase
    {
        #region Instance
        public static readonly LeadsTask Instance = new LeadsTask();
        DateTime absoluteExpiration = DateTime.Now.AddDays(int.Parse(ConfigurationUtil.GetAppSettingValue("CarTypeCacheDays")));//缓存天数
        #endregion

        #region const
        private const string P_LEADSTASK_SELECT = "p_LeadsTask_Select";
        private const string P_LEADSTASK_INSERT = "p_LeadsTask_Insert";
        private const string P_LEADSTASK_UPDATE = "p_LeadsTask_Update";
        private const string P_LEADSTASK_DELETE = "p_LeadsTask_Delete";
        #endregion

        #region Contructor
        protected LeadsTask()
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
        public DataTable GetLeadsTask(QueryLeadsTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("pf", "lt", "BGID", "AssignUserID", query.LoginID);
            }
            #endregion

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.Status=" + query.Status.ToString();
            }
            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.AssignUserID=" + query.AssignUserID.ToString();
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and pf.Name like '%" + StringHelper.SqlFilter(query.ProjectName) + "%'";
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.ProjectID=" + query.ProjectID.ToString();
            }
            if (query.IsSuccess != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.IsSuccess=" + query.IsSuccess.ToString();
            }
            if (query.BeginDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime>='" + StringHelper.SqlFilter(query.BeginDealTime) + " 0:0:0'";
            }
            if (query.EndDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime<='" + StringHelper.SqlFilter(query.EndDealTime) + " 23:59:59'";
            }

            if (query.TaskCBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime>='" + StringHelper.SqlFilter(query.TaskCBeginTime) + " 0:0:0'";
            }
            if (query.TaskCEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime<='" + StringHelper.SqlFilter(query.TaskCEndTime) + " 23:59:59'";
            }

            if (query.ProvinceID != Constant.INT_INVALID_VALUE)
            {
                where += " AND lt.ProvinceID=" + query.ProvinceID.ToString();
            }
            if (query.CityID != Constant.INT_INVALID_VALUE)
            {
                where += " AND lt.CityID=" + query.CityID.ToString();
            }
            if (query.Tel != Constant.STRING_INVALID_VALUE)
            {
                where += " AND lt.Tel='" + StringHelper.SqlFilter(query.Tel) + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASK_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
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
        public DataTable GetLeadsTaskForExport(QueryLeadsTask query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("pf", "lt", "BGID", "AssignUserID", query.LoginID);
            }
            #endregion

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.Status=" + query.Status.ToString();
            }
            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.AssignUserID=" + query.AssignUserID.ToString();
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and pf.Name like '%" + StringHelper.SqlFilter(query.ProjectName) + "%'";
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.ProjectID=" + query.ProjectID.ToString();
            }
            if (query.IsSuccess != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.IsSuccess=" + query.IsSuccess.ToString();
            }
            if (query.BeginDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime>='" + StringHelper.SqlFilter(query.BeginDealTime) + " 0:0:0'";
            }
            if (query.EndDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime<='" + StringHelper.SqlFilter(query.EndDealTime) + " 0:0:0'";
            }

            if (query.TaskCBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime>='" + StringHelper.SqlFilter(query.TaskCBeginTime) + " 0:0:0'";
            }
            if (query.TaskCEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime<='" + StringHelper.SqlFilter(query.TaskCEndTime) + " 23:59:59'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LeadsTask_ForExport", parameters);
            totalCount = (int)(parameters[4].Value);
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
        public DataTable GetLeadsTaskForExport(QueryLeadsTask query, string order, int currentPage, int pageSize, out int totalCount, string taskcreatestart, string taskcreateend, string tasksubstart, string tasksubend)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("pf", "lt", "BGID", "AssignUserID", query.LoginID);
            }
            #endregion

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.Status=" + query.Status.ToString();
            }
            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.AssignUserID=" + query.AssignUserID.ToString();
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and pf.Name like '%" + StringHelper.SqlFilter(query.ProjectName) + "%'";
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.ProjectID=" + query.ProjectID.ToString();
            }
            if (query.IsSuccess != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.IsSuccess=" + query.IsSuccess.ToString();
            }
            if (query.BeginDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime>='" + StringHelper.SqlFilter(query.BeginDealTime) + " 0:0:0'";
            }
            if (query.EndDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime<='" + StringHelper.SqlFilter(query.EndDealTime) + " 0:0:0'";
            }

            if (query.TaskCBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime>='" + StringHelper.SqlFilter(query.TaskCBeginTime) + " 0:0:0'";
            }
            if (query.TaskCEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime<='" + StringHelper.SqlFilter(query.TaskCEndTime) + " 23:59:59'";
            }

            if (!string.IsNullOrEmpty(taskcreatestart) && !string.IsNullOrEmpty(taskcreateend))
            {
                where += " and lt.createtime>='" + StringHelper.SqlFilter(taskcreatestart.Trim()) + " 0:0:0' and lt.createtime<='" + StringHelper.SqlFilter(taskcreateend.Trim()) + " 23:59:59'";
            }
            if (!string.IsNullOrEmpty(tasksubstart) && !string.IsNullOrEmpty(tasksubend))
            {
                where += " and lt.lastupdatetime>='" + StringHelper.SqlFilter(tasksubstart.Trim()) + " 0:0:0' and lt.lastupdatetime<='" + StringHelper.SqlFilter(tasksubend.Trim()) + " 23:59:59' and lt.status=4";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LeadsTask_ForExport", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.LeadsTask GetLeadsTask(string taskId)
        {
            QueryLeadsTask query = new QueryLeadsTask();
            query.TaskID = taskId;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetLeadsTask(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleLeadsTask(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.LeadsTask LoadSingleLeadsTask(DataRow row)
        {
            Entities.LeadsTask model = new Entities.LeadsTask();

            model.TaskID = row["TaskID"].ToString();
            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = int.Parse(row["ProjectID"].ToString());
            }
            model.DemandID = row["DemandID"].ToString();
            //model.RelationID=row["RelationID"].ToString();
            model.RelationID = (Guid)row["RelationID"];
            model.UserName = row["UserName"].ToString();
            model.Tel = row["Tel"].ToString();
            if (row["Sex"].ToString() != "")
            {
                model.Sex = int.Parse(row["Sex"].ToString());
            }
            if (row["ProvinceID"].ToString() != "")
            {
                model.ProvinceID = int.Parse(row["ProvinceID"].ToString());
            }
            if (row["CityID"].ToString() != "")
            {
                model.CityID = int.Parse(row["CityID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["OrderCarMasterID"].ToString() != "")
            {
                model.OrderCarMasterID = int.Parse(row["OrderCarMasterID"].ToString());
            }
            model.OrderCarMaster = row["OrderCarMaster"].ToString();
            if (row["OrderCarSerialID"].ToString() != "")
            {
                model.OrderCarSerialID = int.Parse(row["OrderCarSerialID"].ToString());
            }
            model.OrderCarSerial = row["OrderCarSerial"].ToString();
            if (row["OrderCarID"].ToString() != "")
            {
                model.OrderCarID = int.Parse(row["OrderCarID"].ToString());
            }
            model.OrderCar = row["OrderCar"].ToString();
            if (row["DealerID"].ToString() != "")
            {
                model.DealerID = row["DealerID"].ToString();
            }
            model.DealerName = row["DealerName"].ToString();
            if (row["OrderCreateTime"].ToString() != "")
            {
                model.OrderCreateTime = DateTime.Parse(row["OrderCreateTime"].ToString());
            }
            if (row["DCarMasterID"].ToString() != "")
            {
                model.DCarMasterID = int.Parse(row["DCarMasterID"].ToString());
            }
            model.DCarMaster = row["DCarMaster"].ToString();
            if (row["DCarSerialID"].ToString() != "")
            {
                model.DCarSerialID = int.Parse(row["DCarSerialID"].ToString());
            }
            model.DCarSerial = row["DCarSerial"].ToString();
            if (row["DCarID"].ToString() != "")
            {
                model.DCarID = int.Parse(row["DCarID"].ToString());
            }
            model.DCarName = row["DCarName"].ToString();
            if (row["IsSuccess"].ToString() != "")
            {
                model.IsSuccess = int.Parse(row["IsSuccess"].ToString());
            }
            if (row["FailReason"].ToString() != "")
            {
                model.FailReason = int.Parse(row["FailReason"].ToString());
            }
            if (row["NotEstablishReason"].ToString() != "")
            {
                model.NotEstablishReason = int.Parse(row["NotEstablishReason"].ToString());
            }
            if (row["NotSuccessReason"].ToString() != "")
            {
                model.NotSuccessReason = int.Parse(row["NotSuccessReason"].ToString());
            }
            model.Remark = row["Remark"].ToString();
            if (row["AssignUserID"].ToString() != "")
            {
                model.AssignUserID = int.Parse(row["AssignUserID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["LastUpdateTime"].ToString() != "")
            {
                model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
            }
            if (row["LastUpdateUserID"].ToString() != "")
            {
                model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
            }
            if (row["LastDealTime"].ToString() != "")
            {
                model.LastDealTime = DateTime.Parse(row["LastDealTime"].ToString());
            }
            if (row["DemandVersion"].ToString() != "")
            {
                model.DemandVersion = int.Parse(row["DemandVersion"].ToString());
            }

            if (row["IsJT"].ToString() != "")
            {
                model.IsJT = int.Parse(row["IsJT"].ToString());
            }
            if (row["PBuyCarTime"].ToString() != "")
            {
                model.PBuyCarTime = int.Parse(row["PBuyCarTime"].ToString());
            }
            if (row["ThinkCar"].ToString() != "")
            {
                model.ThinkCar = row["ThinkCar"].ToString();
            }
            if (row["OrderNum"].ToString() != "")
            {
                model.OrderNum = int.Parse(row["OrderNum"].ToString());
            }
            if (row["TargetCityID"] != DBNull.Value && row["TargetCityID"].ToString() != "")
            {
                model.TargetCityID = int.Parse(row["TargetCityID"].ToString());
            }
            if (row["TargetProvinceID"] != DBNull.Value && row["TargetProvinceID"].ToString() != "")
            {
                model.TargetProvinceID = int.Parse(row["TargetProvinceID"].ToString());
            }
            //***************************
            if (row["IsBoughtCar"] != null && row["IsBoughtCar"].ToString() != "")
            {
                model.IsBoughtCar = int.Parse(row["IsBoughtCar"].ToString());
            }
            if (row["BoughtCarMasterID"] != null && row["BoughtCarMasterID"].ToString() != "")
            {
                model.BoughtCarMasterID = int.Parse(row["BoughtCarMasterID"].ToString());
            }
            if (row["BoughtCarMaster"] != null)
            {
                model.BoughtCarMaster = row["BoughtCarMaster"].ToString();
            }
            if (row["BoughtCarSerialID"] != null && row["BoughtCarSerialID"].ToString() != "")
            {
                model.BoughtCarSerialID = int.Parse(row["BoughtCarSerialID"].ToString());
            }
            if (row["BoughtCarSerial"] != null)
            {
                model.BoughtCarSerial = row["BoughtCarSerial"].ToString();
            }
            if (row["BoughtCarYearMonth"] != null)
            {
                model.BoughtCarYearMonth = row["BoughtCarYearMonth"].ToString();
            }
            if (row["BoughtCarDealerID"] != null)
            {
                model.BoughtCarDealerID = row["BoughtCarDealerID"].ToString();
            }
            if (row["BoughtCarDealerName"] != null)
            {
                model.BoughtCarDealerName = row["BoughtCarDealerName"].ToString();
            }
            if (row["HasBuyCarPlan"] != null && row["HasBuyCarPlan"].ToString() != "")
            {
                model.HasBuyCarPlan = int.Parse(row["HasBuyCarPlan"].ToString());
            }


            //if (row["IsAttention"] != null && !string.IsNullOrEmpty(row["IsAttention"].ToString()))
            //{
            //    model.IsAttention = int.Parse(row["IsAttention"].ToString());
            //}
            //if (row["IsContactedDealer"] != null && !string.IsNullOrEmpty(row["IsContactedDealer"].ToString()))
            //{
            //    model.IsContactedDealer = int.Parse(row["IsContactedDealer"].ToString());
            //}
            //if (row["IsSatisfiedService"] != null && !string.IsNullOrEmpty(row["IsSatisfiedService"].ToString()))
            //{
            //    model.IsSatisfiedService = int.Parse(row["IsSatisfiedService"].ToString());
            //}
            //if (row["ContactedWhichDealer"] != null)
            //{
            //    model.ContactedWhichDealer = row["ContactedWhichDealer"].ToString();
            //}

            if (row["IsAttention"] != null && row["IsAttention"].ToString() != "")
            {
                if ((row["IsAttention"].ToString() == "1") || (row["IsAttention"].ToString().ToLower() == "true"))
                {
                    model.IsAttention = 1;
                }
                else
                {
                    model.IsAttention = 0;
                }
            }
            if (row["IsContactedDealer"] != null && row["IsContactedDealer"].ToString() != "")
            {
                if ((row["IsContactedDealer"].ToString() == "1") || (row["IsContactedDealer"].ToString().ToLower() == "true"))
                {
                    model.IsContactedDealer = 1;
                }
                else
                {
                    model.IsContactedDealer = 0;
                }
            }
            if (row["IsSatisfiedService"] != null && row["IsSatisfiedService"].ToString() != "")
            {
                if ((row["IsSatisfiedService"].ToString() == "1") || (row["IsSatisfiedService"].ToString().ToLower() == "true"))
                {
                    model.IsSatisfiedService = 1;
                }
                else
                {
                    model.IsSatisfiedService = 0;
                }
            }
            if (row["ContactedWhichDealer"] != null)
            {
                model.ContactedWhichDealer = row["ContactedWhichDealer"].ToString();
            }


            if (row["IntentionCarMasterID"] != null && row["IntentionCarMasterID"].ToString() != "")
            {
                model.IntentionCarMasterID = int.Parse(row["IntentionCarMasterID"].ToString());
            }
            if (row["IntentionCarMaster"] != null)
            {
                model.IntentionCarMaster = row["IntentionCarMaster"].ToString();
            }
            if (row["IntentionCarSerialID"] != null && row["IntentionCarSerialID"].ToString() != "")
            {
                model.IntentionCarSerialID = int.Parse(row["IntentionCarSerialID"].ToString());
            }
            if (row["IntentionCarSerial"] != null)
            {
                model.IntentionCarSerial = row["IntentionCarSerial"].ToString();
            }

            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.LeadsTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@ProjectID", SqlDbType.Int,4),
					new SqlParameter("@DemandID", SqlDbType.VarChar,50),
					new SqlParameter("@RelationID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarID", SqlDbType.Int,4),
					new SqlParameter("@OrderCar", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.VarChar,50),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@DCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@DCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@DCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarID", SqlDbType.Int,4),
					new SqlParameter("@DCarName", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSuccess", SqlDbType.Int,4),
					new SqlParameter("@FailReason", SqlDbType.Int,4),

                    //增加 未接通原因和接通后失败原因
                    new SqlParameter("@NotEstablishReason", SqlDbType.Int,4),
                    new SqlParameter("@NotSuccessReason", SqlDbType.Int,4),

					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastDealTime", SqlDbType.DateTime),
                    new SqlParameter("@DemandVersion", SqlDbType.Int,4),
                    new SqlParameter("@IsJT", SqlDbType.Int,4),
                    new SqlParameter("@PBuyCarTime", SqlDbType.Int,4),
                    new SqlParameter("@ThinkCar", SqlDbType.VarChar,100),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),

                    new SqlParameter("@IsBoughtCar", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@BoughtCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@BoughtCarYearMonth", SqlDbType.VarChar,20),
					new SqlParameter("@BoughtCarDealerID", SqlDbType.VarChar,50),
					new SqlParameter("@BoughtCarDealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@HasBuyCarPlan", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@IntentionCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarSerial", SqlDbType.NVarChar,100)};

            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.DemandID;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Tel;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.OrderCarMasterID;
            parameters[11].Value = model.OrderCarMaster;
            parameters[12].Value = model.OrderCarSerialID;
            parameters[13].Value = model.OrderCarSerial;
            parameters[14].Value = model.OrderCarID;
            parameters[15].Value = model.OrderCar;
            parameters[16].Value = model.DealerID;
            parameters[17].Value = model.DealerName;
            parameters[18].Value = model.OrderCreateTime;
            parameters[19].Value = model.DCarMasterID;
            parameters[20].Value = model.DCarMaster;
            parameters[21].Value = model.DCarSerialID;
            parameters[22].Value = model.DCarSerial;
            parameters[23].Value = model.DCarID;
            parameters[24].Value = model.DCarName;
            parameters[25].Value = model.IsSuccess;
            parameters[26].Value = model.FailReason;

            parameters[27].Value = model.NotEstablishReason;
            parameters[28].Value = model.NotSuccessReason;

            parameters[29].Value = model.Remark;
            parameters[30].Value = model.AssignUserID;
            parameters[31].Value = model.CreateTime;
            parameters[32].Value = model.CreateUserID;
            parameters[33].Value = model.LastUpdateTime;
            parameters[34].Value = model.LastUpdateUserID;
            parameters[35].Value = model.LastDealTime;
            parameters[36].Value = model.DemandVersion;
            parameters[37].Value = model.IsJT;
            parameters[38].Value = model.PBuyCarTime;
            parameters[39].Value = model.ThinkCar;
            parameters[40].Value = model.OrderNum;

            parameters[41].Value = model.IsBoughtCar;
            parameters[42].Value = model.BoughtCarMasterID;
            parameters[43].Value = model.BoughtCarMaster;
            parameters[44].Value = model.BoughtCarSerialID;
            parameters[45].Value = model.BoughtCarSerial;
            parameters[46].Value = model.BoughtCarYearMonth;
            parameters[47].Value = model.BoughtCarDealerID;
            parameters[48].Value = model.BoughtCarDealerName;
            parameters[49].Value = model.HasBuyCarPlan;
            parameters[50].Value = model.IntentionCarMasterID;
            parameters[51].Value = model.IntentionCarMaster;
            parameters[52].Value = model.IntentionCarSerialID;
            parameters[53].Value = model.IntentionCarSerial;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASK_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.LeadsTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@ProjectID", SqlDbType.Int,4),
					new SqlParameter("@DemandID", SqlDbType.VarChar,50),
					new SqlParameter("@RelationID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarID", SqlDbType.Int,4),
					new SqlParameter("@OrderCar", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.VarChar,50),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@DCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@DCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@DCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarID", SqlDbType.Int,4),
					new SqlParameter("@DCarName", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSuccess", SqlDbType.Int,4),
					new SqlParameter("@FailReason", SqlDbType.Int,4),

                    //增加 未接通原因和接通后失败原因
                    new SqlParameter("@NotEstablishReason", SqlDbType.Int,4),
                    new SqlParameter("@NotSuccessReason", SqlDbType.Int,4),

					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastDealTime", SqlDbType.DateTime),
                    new SqlParameter("@DemandVersion", SqlDbType.Int,4),
                    new SqlParameter("@IsJT", SqlDbType.Int,4),
                    new SqlParameter("@PBuyCarTime", SqlDbType.Int,4),
                    new SqlParameter("@ThinkCar", SqlDbType.VarChar,100),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),
                    new SqlParameter("@TargetProvinceID", SqlDbType.Int,4),
                    new SqlParameter("@TargetCityID", SqlDbType.Int,4),

                    new SqlParameter("@IsBoughtCar", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@BoughtCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@BoughtCarYearMonth", SqlDbType.VarChar,20),
					new SqlParameter("@BoughtCarDealerID", SqlDbType.VarChar,50),
					new SqlParameter("@BoughtCarDealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@HasBuyCarPlan", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@IntentionCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarSerial", SqlDbType.NVarChar,100)};

            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.DemandID;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Tel;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.OrderCarMasterID;
            parameters[11].Value = model.OrderCarMaster;
            parameters[12].Value = model.OrderCarSerialID;
            parameters[13].Value = model.OrderCarSerial;
            parameters[14].Value = model.OrderCarID;
            parameters[15].Value = model.OrderCar;
            parameters[16].Value = model.DealerID;
            parameters[17].Value = model.DealerName;
            parameters[18].Value = model.OrderCreateTime;
            parameters[19].Value = model.DCarMasterID;
            parameters[20].Value = model.DCarMaster;
            parameters[21].Value = model.DCarSerialID;
            parameters[22].Value = model.DCarSerial;
            parameters[23].Value = model.DCarID;
            parameters[24].Value = model.DCarName;
            parameters[25].Value = model.IsSuccess;
            parameters[26].Value = model.FailReason;

            parameters[27].Value = model.NotEstablishReason;
            parameters[28].Value = model.NotSuccessReason;

            parameters[29].Value = model.Remark;
            parameters[30].Value = model.AssignUserID;
            parameters[31].Value = model.CreateTime;
            parameters[32].Value = model.CreateUserID;
            parameters[33].Value = model.LastUpdateTime;
            parameters[34].Value = model.LastUpdateUserID;
            parameters[35].Value = model.LastDealTime;
            parameters[36].Value = model.DemandVersion;

            parameters[37].Value = model.IsJT;
            parameters[38].Value = model.PBuyCarTime;
            parameters[39].Value = model.ThinkCar;
            parameters[40].Value = model.OrderNum;
            parameters[41].Value = model.TargetProvinceID;
            parameters[42].Value = model.TargetCityID;

            parameters[43].Value = model.IsBoughtCar;
            parameters[44].Value = model.BoughtCarMasterID;
            parameters[45].Value = model.BoughtCarMaster;
            parameters[46].Value = model.BoughtCarSerialID;
            parameters[47].Value = model.BoughtCarSerial;
            parameters[48].Value = model.BoughtCarYearMonth;
            parameters[49].Value = model.BoughtCarDealerID;
            parameters[50].Value = model.BoughtCarDealerName;
            parameters[51].Value = model.HasBuyCarPlan;
            parameters[52].Value = model.IntentionCarMasterID;
            parameters[53].Value = model.IntentionCarMaster;
            parameters[54].Value = model.IntentionCarSerialID;
            parameters[55].Value = model.IntentionCarSerial;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LEADSTASK_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.LeadsTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@ProjectID", SqlDbType.Int,4),
					new SqlParameter("@DemandID", SqlDbType.VarChar,50),
					new SqlParameter("@RelationID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarID", SqlDbType.Int,4),
					new SqlParameter("@OrderCar", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.VarChar,50),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@DCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@DCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@DCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarID", SqlDbType.Int,4),
					new SqlParameter("@DCarName", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSuccess", SqlDbType.Int,4),
					new SqlParameter("@FailReason", SqlDbType.Int,4),

                     //增加 未接通原因和接通后失败原因
                    new SqlParameter("@NotEstablishReason", SqlDbType.Int,4),
                    new SqlParameter("@NotSuccessReason", SqlDbType.Int,4),

					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastDealTime", SqlDbType.DateTime),
                    new SqlParameter("@DemandVersion", SqlDbType.Int,4),
                    new SqlParameter("@IsJT", SqlDbType.Int,4),
                    new SqlParameter("@PBuyCarTime", SqlDbType.Int,4),
                    new SqlParameter("@ThinkCar", SqlDbType.VarChar,100),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@TargetProvinceID", SqlDbType.Int,4),
					new SqlParameter("@TargetCityID", SqlDbType.Int,4),

                    new SqlParameter("@IsBoughtCar", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@BoughtCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@BoughtCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@BoughtCarYearMonth", SqlDbType.VarChar,20),
					new SqlParameter("@BoughtCarDealerID", SqlDbType.VarChar,50),
					new SqlParameter("@BoughtCarDealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@HasBuyCarPlan", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@IntentionCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@IntentionCarSerial", SqlDbType.NVarChar,100),

                    new SqlParameter("@IsAttention", SqlDbType.Bit,1),
					new SqlParameter("@IsContactedDealer", SqlDbType.Bit,1),
					new SqlParameter("@IsSatisfiedService", SqlDbType.Bit,1),
					new SqlParameter("@ContactedWhichDealer", SqlDbType.NVarChar,10),
                   };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.DemandID;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Tel;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.OrderCarMasterID;
            parameters[11].Value = model.OrderCarMaster;
            parameters[12].Value = model.OrderCarSerialID;
            parameters[13].Value = model.OrderCarSerial;
            parameters[14].Value = model.OrderCarID;
            parameters[15].Value = model.OrderCar;
            parameters[16].Value = model.DealerID;
            parameters[17].Value = model.DealerName;
            parameters[18].Value = model.OrderCreateTime;
            parameters[19].Value = model.DCarMasterID;
            parameters[20].Value = model.DCarMaster;
            parameters[21].Value = model.DCarSerialID;
            parameters[22].Value = model.DCarSerial;
            parameters[23].Value = model.DCarID;
            parameters[24].Value = model.DCarName;
            parameters[25].Value = model.IsSuccess;
            parameters[26].Value = model.FailReason;

            parameters[27].Value = model.NotEstablishReason;
            parameters[28].Value = model.NotSuccessReason;

            parameters[29].Value = model.Remark;
            parameters[30].Value = model.AssignUserID;
            parameters[31].Value = model.CreateTime;
            parameters[32].Value = model.CreateUserID;
            parameters[33].Value = model.LastUpdateTime;
            parameters[34].Value = model.LastUpdateUserID;
            parameters[35].Value = model.LastDealTime;
            parameters[36].Value = model.DemandVersion;
            parameters[37].Value = model.IsJT;
            parameters[38].Value = model.PBuyCarTime;
            parameters[39].Value = model.ThinkCar;
            parameters[40].Value = model.OrderNum;
            parameters[41].Value = model.TargetProvinceID;
            parameters[42].Value = model.TargetCityID;

            parameters[43].Value = model.IsBoughtCar;
            parameters[44].Value = model.BoughtCarMasterID;
            parameters[45].Value = model.BoughtCarMaster;
            parameters[46].Value = model.BoughtCarSerialID;
            parameters[47].Value = model.BoughtCarSerial;
            parameters[48].Value = model.BoughtCarYearMonth;
            parameters[49].Value = model.BoughtCarDealerID;
            parameters[50].Value = model.BoughtCarDealerName;
            parameters[51].Value = model.HasBuyCarPlan;
            parameters[52].Value = model.IntentionCarMasterID;
            parameters[53].Value = model.IntentionCarMaster;
            parameters[54].Value = model.IntentionCarSerialID;
            parameters[55].Value = model.IntentionCarSerial;

            parameters[56].Value = model.IsAttention.HasValue && model.IsAttention.Value != -2 ? model.IsAttention : null;
            parameters[57].Value = model.IsContactedDealer.HasValue && model.IsContactedDealer.Value != -2 ? model.IsContactedDealer : null;
            parameters[58].Value = model.IsSatisfiedService.HasValue && model.IsSatisfiedService.Value != -2 ? model.IsSatisfiedService : null;  
            parameters[59].Value = string.IsNullOrEmpty(model.ContactedWhichDealer) ? null : model.ContactedWhichDealer;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASK_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.LeadsTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@ProjectID", SqlDbType.Int,4),
					new SqlParameter("@DemandID", SqlDbType.VarChar,50),
					new SqlParameter("@RelationID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@OrderCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCarID", SqlDbType.Int,4),
					new SqlParameter("@OrderCar", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.VarChar,50),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@DCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@DCarMaster", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@DCarSerial", SqlDbType.NVarChar,100),
					new SqlParameter("@DCarID", SqlDbType.Int,4),
					new SqlParameter("@DCarName", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSuccess", SqlDbType.Int,4),
					new SqlParameter("@FailReason", SqlDbType.Int,4),

                     //增加 未接通原因和接通后失败原因
                    new SqlParameter("@NotEstablishReason", SqlDbType.Int,4),
                    new SqlParameter("@NotSuccessReason", SqlDbType.Int,4),

					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastDealTime", SqlDbType.DateTime),
                    new SqlParameter("@DemandVersion", SqlDbType.Int,4),
                    new SqlParameter("@IsJT", SqlDbType.Int,4),
                    new SqlParameter("@PBuyCarTime", SqlDbType.Int,4),
                    new SqlParameter("@ThinkCar", SqlDbType.VarChar,100),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.DemandID;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Tel;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.OrderCarMasterID;
            parameters[11].Value = model.OrderCarMaster;
            parameters[12].Value = model.OrderCarSerialID;
            parameters[13].Value = model.OrderCarSerial;
            parameters[14].Value = model.OrderCarID;
            parameters[15].Value = model.OrderCar;
            parameters[16].Value = model.DealerID;
            parameters[17].Value = model.DealerName;
            parameters[18].Value = model.OrderCreateTime;
            parameters[19].Value = model.DCarMasterID;
            parameters[20].Value = model.DCarMaster;
            parameters[21].Value = model.DCarSerialID;
            parameters[22].Value = model.DCarSerial;
            parameters[23].Value = model.DCarID;
            parameters[24].Value = model.DCarName;
            parameters[25].Value = model.IsSuccess;
            parameters[26].Value = model.FailReason;

            parameters[27].Value = model.NotEstablishReason;
            parameters[28].Value = model.NotSuccessReason;

            parameters[27].Value = model.Remark;
            parameters[28].Value = model.AssignUserID;
            parameters[29].Value = model.CreateTime;
            parameters[30].Value = model.CreateUserID;
            parameters[31].Value = model.LastUpdateTime;
            parameters[32].Value = model.LastUpdateUserID;
            parameters[33].Value = model.LastDealTime;
            parameters[34].Value = model.DemandVersion;

            parameters[35].Value = model.IsJT;
            parameters[36].Value = model.PBuyCarTime;
            parameters[37].Value = model.ThinkCar;
            parameters[38].Value = model.OrderNum;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LEADSTASK_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string taskId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.VarChar,50)};
            parameters[0].Value = taskId;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASK_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string taskId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.VarChar,50)};
            parameters[0].Value = taskId;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LEADSTASK_DELETE, parameters);
        }
        #endregion

        //根据车型ID取车款列表
        /// <summary>
        /// 根据车型ID取车款列表
        /// </summary>
        /// <param name="serialId">车型ＩＤ</param>
        /// <returns></returns>
        public DataSet GetCarListByCarSerialID(int serialId)
        {
            DataSet dtCarList = null;
            try
            {
                dtCarList = (DataSet)HttpRuntime.Cache.Get("CarListByCarSerialID_" + serialId);
                if (dtCarList == null || (dtCarList != null && dtCarList.Tables.Count == 0))
                {
                    string sqlstr = "SELECT CarID,CarName,CarYearType FROM [Car_Car] where csid=" + serialId + " and status=0  order by caryeartype asc;SELECT distinct CarYearType FROM [Car_Car] where csid=" + serialId + " and status=0 order by caryeartype asc";
                    dtCarList = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null);
                    HttpRuntime.Cache.Insert("CarListByCarSerialID_" + serialId, dtCarList, null, absoluteExpiration, TimeSpan.Zero);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtCarList;
        }
        /// <summary>
        /// 根据品牌id,取所有在销车型
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public DataTable GetSerialByBrandID(int brandId)
        {
            DataTable dtSerialList = null;
            try
            {
                string sqlstr = "select serialID,Name from Car_Serial where BrandID=" + brandId + " and cssalestate='在销'";
                dtSerialList = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtSerialList;
        }

        //

        //根据任务ID串获取任务信息
        public DataTable GetTaskInfoListByIDs(string taskIds)
        {
            string sqlStr = "SELECT * FROM dbo.LeadsTask WHERE TaskID IN (" + Dal.Util.SqlFilterByInCondition(taskIds) + ")";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }


        //根据status分组，获取各状态下数量
        public DataTable GetStatusNum(Entities.QueryLeadsTask query)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("pf", "lt", "BGID", "AssignUserID", query.LoginID);
            }
            #endregion

            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and AssignUserID=" + query.AssignUserID;
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and  pf.Name LIKE '%" + StringHelper.SqlFilter(query.ProjectName.ToString()) + "%'";
            }
            if (query.BeginDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime>='" + StringHelper.SqlFilter(query.BeginDealTime) + " 0:0:0'";
            }
            if (query.EndDealTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.LastDealTime<='" + StringHelper.SqlFilter(query.EndDealTime) + " 23:59:59'";
            }

            if (query.TaskCBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime>='" + StringHelper.SqlFilter(query.TaskCBeginTime) + " 0:0:0'";
            }
            if (query.TaskCEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.CreateTime<='" + StringHelper.SqlFilter(query.TaskCEndTime) + " 23:59:59'";
            }
            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }

            if (query.ProvinceID != Constant.INT_INVALID_VALUE)
            {
                where += " AND lt.ProvinceID=" + query.ProvinceID;
            }
            if (query.CityID != Constant.INT_INVALID_VALUE)
            {
                where += " AND lt.CityID=" + query.CityID;
            }
            if (query.Tel != Constant.STRING_INVALID_VALUE)
            {
                where += " AND lt.Tel='" + StringHelper.SqlFilter(query.Tel) + "'";
            }

            string sqlStr = @"SELECT  
                                        ISNULL(SUM(CASE WHEN lt.Status = 1 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '待分配' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 2 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '待处理' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 3 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '处理中' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 4 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '已处理' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 5 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '已撤销' ,
                                        ISNULL(SUM(CASE WHEN lt.status = 4 AND lt.IsSuccess = 1 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '成功' ,
                                        ISNULL(SUM(CASE WHEN lt.status = 4 AND lt.IsSuccess = 0 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '失败'
                                        FROM    dbo.LeadsTask lt
                                                LEFT JOIN dbo.ProjectInfo pf ON lt.ProjectID = pf.ProjectID
                                        WHERE   1 = 1 " + where;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public int GetMaxIDFromTaskID()
        {
            int intval = 0;
            string sqlStr = "select max(CAST (SUBSTRING(TaskID,4,len(TaskID)-3) as int)) from LeadsTask";

            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            int.TryParse(o.ToString(), out intval);

            return intval;
        }
        /// <summary>
        /// 根据项目，取项目下成功集客数
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public int GetSumSuccess(int projectid)
        {
            int intval = 0;
            string sqlStr = "select count(*) from LeadsTask where IsSuccess=1 and Status=4 and ProjectID=" + projectid;

            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            int.TryParse(o.ToString(), out intval);

            return intval;
        }
        /// <summary>
        /// 批量验证电话是否是黑名单，返回是黑名单的
        /// </summary>
        /// <param name="tels"></param>
        /// <returns></returns>
        public DataTable CheckTelBatch(string tels)
        {
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@TelS", SqlDbType.VarChar)
					};
            parameters[0].Value = tels;
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CheckTelBlackWhiteBatch", parameters).Tables[0];
            return dt;
        }

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        public DataTable GetCrm()
        {
            string sql = "SELECT  Detail.RecID ,Detail.Name AS UserName ,Detail.Mobile AS UserMobile ,Detail.Sex , Detail.ProvinceID ,Detail.CityID ,Detail.BrandID ,Detail.SerialID ,Detail.OrderTime, Detail.DealerName,Detail.TargetProvinceID ,Detail.TargetCityID,Detail.OrderSource,Detail.OrderID  FROM CJKLeadsImportDetail AS Detail LEFT JOIN dbo.CJKLeadsImportLog AS Record ON Detail.ImportID = Record.ImportID WHERE      Detail.Status= 0 AND Record.NeedCall= 1 AND Record.OrderCode ='CJK2016032100001' AND Record.Batch= 1";
            return SqlHelper.ExecuteDataset(ConnectionStrings_CRM,CommandType.Text,sql,null).Tables[0];
        }


    }
}

