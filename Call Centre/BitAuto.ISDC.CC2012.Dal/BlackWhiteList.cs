using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Configuration;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class BlackWhiteList : DataBase
    {
        public string Conn
        {
            get
            {
                return CONNECTIONSTRINGS;
            }
        }
        public string ConnCCBlackWhiteSynch
        {
            get
            {
                return ConnectionStrings_Holly_Business;
            }
        }

        #region Instance
        public static readonly BlackWhiteList Instance = new BlackWhiteList();
        #endregion

        #region   CC数据库==黑白名单公用方法
        /// <summary>
        /// 获取需要同步的新增的黑、白名单数据（黑名单和白名单的数据一起同步）
        /// </summary>
        /// <returns></returns>
        public DataTable GetSynchrodata_BlackWhiteData_Insert()
        {
            string sqlStr = @"SELECT [RecId],[Type],[PhoneNum],[EffectiveDate],[ExpiryDate],[CallType],[CDIDS],[Reason],[SynchrodataStatus],[CreateUserId],
                              [CreateDate],[UpdateUserId],[UpdateDate],[Status],[CallID] ,[CallOutNDType],[BGID],[TIMESTAMP] FROM dbo.BlackWhiteList
                              WHERE SynchrodataStatus = 0 AND [Status] = 0 AND CallType&1=1";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取需要同步的更新了的黑、白名单数据（黑名单和白名单的数据一起同步）
        /// </summary>
        /// <returns></returns>
        public DataTable GetSynchrodata_BlackWhiteData_Update()
        {
            string sqlStr = @"SELECT [RecId],[Type],[PhoneNum],[EffectiveDate],[ExpiryDate],[CallType],[CDIDS],[Reason],[SynchrodataStatus],[CreateUserId],
                              [CreateDate],[UpdateUserId],[UpdateDate],[Status],[CallID] ,[CallOutNDType],[BGID],[TIMESTAMP] FROM dbo.BlackWhiteList
                              WHERE SynchrodataStatus = 1 AND CallType&1=1";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 将指定的RecIDS(多个RecID之间用“，”隔开)的SynchrodataStatus值改为2
        /// </summary>
        /// <param name="RecIDS"></param>
        /// <returns></returns>
        public int UpdateSuccessSynchrodateStatus(string RecIDS)
        {
            string sqlStr = @"UPDATE dbo.BlackWhiteList SET SynchrodataStatus = 2 WHERE RecId IN(" + Dal.Util.SqlFilterByInCondition(RecIDS) + ")";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }
        /// <summary>
        /// 逻辑删除数据
        /// </summary>
        /// <param name="RecIDS"></param>
        /// <returns></returns>
        public int DeleteByChangeStatus(int RecID, int UserId)
        {
            string sqlStr = @"UPDATE dbo.BlackWhiteList SET Status = -1, SynchrodataStatus=1,UpdateDate=GETDATE(),UpdateUserId=" + UserId + " WHERE RecId = " + RecID
                + @";INSERT INTO [dbo].[BlackWhiteListOperLog]
			   ([BWRecID]
			   ,[BWType]
			   ,[PhoneNum]
			   ,[CallID]
			   ,[OperType]
			   ,[OperUserID]
			   ,[OperTime]) 
               SELECT  RecId ,[Type],PhoneNum,CallID,3," + UserId + @",GETDATE()
               FROM dbo.BlackWhiteList
               WHERE RecId =" + RecID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }
        /// <summary>
        /// 根据指定条件查询黑、白名单数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBlackWhiteData(QueryBlackWhite query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.Type != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.[Type] = '" + query.Type + "'";
            }
            if (query.CallType != Constant.INT_INVALID_VALUE)
            {
                if (query.Type == 0)
                {
                    where += " AND (a.CallType = " + query.CallType + ")";
                }
                else
                {
                    where += " AND (a.CallType = " + query.CallType + " or a.CallType = 3)";
                }
            }
            if (query.QueryCallTypes != Constant.STRING_INVALID_VALUE && query.QueryCallTypes != "")
            {
                if (query.Type == 0)
                {
                    where += " AND a.CallType in (" + Dal.Util.SqlFilterByInCondition(query.QueryCallTypes) + ")";
                }
                else
                {
                    where += " AND (a.CallType in (" + Dal.Util.SqlFilterByInCondition(query.QueryCallTypes) + ") or a.CallType = 3)";
                }
            }
            if (query.CDIDS != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CDIDS = " + query.CDIDS;
            }
            if (query.QueryCDIDs != Constant.STRING_INVALID_VALUE && query.QueryCDIDs != "")
            {
                where += " AND (";
                string[] arrCDID = Util.SqlFilterByInCondition(query.QueryCDIDs).Split(',');
                string strcdidq = "";
                for (int i = 0; i < arrCDID.Length; i++)
                {
                    strcdidq += "or (a.CDIDS & " + arrCDID[i] + ") = " + arrCDID[i];
                }
                where += strcdidq.Substring(2) + ")";
            }
            if (query.CreateUserId != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CreateUserId = '" + query.CreateUserId + "'";
            }
            if (query.PhoneNum != Constant.STRING_INVALID_VALUE && query.PhoneNum != "")
            {
                where += " AND a.PhoneNum = '" + SqlFilter(query.PhoneNum) + "'";
            }
            if (query.QueryCreateStartDate != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.CreateDate >= '" + query.QueryCreateStartDate + "'";
            }
            if (query.QueryCreateEndDate != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.CreateDate < '" + query.QueryCreateEndDate.AddDays(1) + "'";
            }
            if (query.EffectiveDate != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.EffectiveDate >= '" + query.EffectiveDate + "'";
            }
            if (query.ExpiryDate != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.ExpiryDate < '" + query.ExpiryDate.AddDays(1) + "'";
            }
            if (query.QueryLoginUserId != Constant.INT_INVALID_VALUE)
            {
                //数据权限控制：当前登录人的所属分组和管辖分组
                //                where += @" and a.BGID in(select BGID from UserGroupDataRigth where UserID=" + query.QueryLoginUserId + @"
                //                union all SELECT EmployeeAgent.BGID FROM    EmployeeAgent INNER JOIN dbo.BusinessGroup ON EmployeeAgent.BGID = BusinessGroup.BGID 
                //                                    WHERE BusinessGroup.Status=0 AND EmployeeAgent.UserID=" + query.QueryLoginUserId + @")";
                //数据权限控制：创建人+管辖分组
                where += Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserId", query.QueryLoginUserId);
            }
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_BlackWhiteList_Select", parameters);

            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        public int IsPhoneNumExist(string PhoneNumber, int CallDisplayId)
        {
            string sqlStr = @"SELECT count(1) FROM dbo.BlackWhiteList
                              WHERE Status=0 AND ExpiryDate>='" + DateTime.Now + "' AND EffectiveDate<='" + DateTime.Now + "' and PhoneNum='" + StringHelper.SqlFilter(PhoneNumber) + "' AND (CDIDs & " + CallDisplayId + ") = " + CallDisplayId;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return (int)obj;
            }
        }
        public int IsPhoneNumExist(string PhoneNumber, int CallDisplayId, int RecID)
        {
            string sqlStr = @"SELECT count(1) FROM dbo.BlackWhiteList
                              WHERE RecID !=" + RecID + " and Status=0 AND ExpiryDate>='" + DateTime.Now + "' AND EffectiveDate<='" + DateTime.Now + "' and PhoneNum='" + PhoneNumber + "' AND (CDIDs & " + CallDisplayId + ") = " + CallDisplayId;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return (int)obj;
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Entities.BlackWhiteList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO BlackWhiteList(");
            strSql.Append("Type,PhoneNum,EffectiveDate,ExpiryDate,CallType,CDIDS,Reason,SynchrodataStatus,CreateUserId,CreateDate,UpdateUserId,UpdateDate,Status,CallID,CallOutNDType)");
            strSql.Append(" VALUES (");
            strSql.Append("@Type,@PhoneNum,@EffectiveDate,@ExpiryDate,@CallType,@CDIDS,@Reason,@SynchrodataStatus,@CreateUserId,@CreateDate,@UpdateUserId,@UpdateDate,@Status,@CallID,@CallOutNDType)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,20),
					new SqlParameter("@EffectiveDate", SqlDbType.DateTime),
					new SqlParameter("@ExpiryDate", SqlDbType.DateTime),
					new SqlParameter("@CallType", SqlDbType.Int,4),
					new SqlParameter("@CDIDS", SqlDbType.Int,4),
					new SqlParameter("@Reason", SqlDbType.NVarChar,500),
					new SqlParameter("@SynchrodataStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@UpdateUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CallID", SqlDbType.BigInt),
                    new SqlParameter("@CallOutNDType", SqlDbType.Int,4)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.PhoneNum;
            parameters[2].Value = model.EffectiveDate;
            parameters[3].Value = model.ExpiryDate;
            parameters[4].Value = model.CallType;
            parameters[5].Value = model.CDIDS;
            parameters[6].Value = model.Reason;
            parameters[7].Value = model.SynchrodataStatus;
            parameters[8].Value = model.CreateUserId;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.UpdateUserId;
            parameters[11].Value = model.UpdateDate;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.CallID;
            parameters[14].Value = model.CallOutNDType;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Entities.BlackWhiteList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE BlackWhiteList set ");
            strSql.Append("Type=@Type,");
            strSql.Append("PhoneNum=@PhoneNum,");
            strSql.Append("EffectiveDate=@EffectiveDate,");
            strSql.Append("ExpiryDate=@ExpiryDate,");
            strSql.Append("CallType=@CallType,");
            strSql.Append("CDIDS=@CDIDS,");
            strSql.Append("Reason=@Reason,");
            strSql.Append("SynchrodataStatus=@SynchrodataStatus,");
            strSql.Append("CreateUserId=@CreateUserId,");
            strSql.Append("CreateDate=@CreateDate,");
            strSql.Append("UpdateUserId=@UpdateUserId,");
            strSql.Append("UpdateDate=@UpdateDate,");
            strSql.Append("Status=@Status,");
            strSql.Append("CallID=@CallID,");
            strSql.Append("CallOutNDType=@CallOutNDType");

            strSql.Append(" WHERE RecId=@RecId");
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,20),
					new SqlParameter("@EffectiveDate", SqlDbType.DateTime),
					new SqlParameter("@ExpiryDate", SqlDbType.DateTime),
					new SqlParameter("@CallType", SqlDbType.Int,4),
					new SqlParameter("@CDIDS", SqlDbType.Int,4),
					new SqlParameter("@Reason", SqlDbType.NVarChar,500),
					new SqlParameter("@SynchrodataStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@UpdateUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@RecId", SqlDbType.Int,4),
                    new SqlParameter("@CallID", SqlDbType.BigInt),
                    new SqlParameter("@CallOutNDType", SqlDbType.Int,4)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.PhoneNum;
            parameters[2].Value = model.EffectiveDate;
            parameters[3].Value = model.ExpiryDate;
            parameters[4].Value = model.CallType;
            parameters[5].Value = model.CDIDS;
            parameters[6].Value = model.Reason;
            parameters[7].Value = model.SynchrodataStatus;
            parameters[8].Value = model.CreateUserId;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.UpdateUserId;
            parameters[11].Value = model.UpdateDate;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.RecId;
            parameters[14].Value = model.CallID;
            parameters[15].Value = model.CallOutNDType;

            int rows = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.BlackWhiteList GetModel(int RecId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  TOP 1 RecId,Type,PhoneNum,EffectiveDate,ExpiryDate,CallType,CDIDS,Reason,SynchrodataStatus,CreateUserId,CreateDate,UpdateUserId,UpdateDate,Status,CallID,CallOutNDType");
            strSql.Append(" FROM BlackWhiteList ");
            strSql.Append(" WHERE RecId=@RecId");
            SqlParameter[] parameters = {
					new SqlParameter("@RecId", SqlDbType.Int,4)
			};
            parameters[0].Value = RecId;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.BlackWhiteList DataRowToModel(DataRow row)
        {
            Entities.BlackWhiteList model = new Entities.BlackWhiteList();
            if (row != null)
            {
                if (row["RecId"] != null && row["RecId"].ToString() != "")
                {
                    model.RecId = int.Parse(row["RecId"].ToString());
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["PhoneNum"] != null)
                {
                    model.PhoneNum = row["PhoneNum"].ToString();
                }
                if (row["EffectiveDate"] != null && row["EffectiveDate"].ToString() != "")
                {
                    model.EffectiveDate = DateTime.Parse(row["EffectiveDate"].ToString());
                }
                if (row["ExpiryDate"] != null && row["ExpiryDate"].ToString() != "")
                {
                    model.ExpiryDate = DateTime.Parse(row["ExpiryDate"].ToString());
                }
                if (row["CallType"] != null && row["CallType"].ToString() != "")
                {
                    model.CallType = int.Parse(row["CallType"].ToString());
                }
                if (row["CDIDS"] != null && row["CDIDS"].ToString() != "")
                {
                    model.CDIDS = int.Parse(row["CDIDS"].ToString());
                }
                if (row["Reason"] != null)
                {
                    model.Reason = row["Reason"].ToString();
                }
                if (row["SynchrodataStatus"] != null && row["SynchrodataStatus"].ToString() != "")
                {
                    model.SynchrodataStatus = int.Parse(row["SynchrodataStatus"].ToString());
                }
                if (row["CreateUserId"] != null && row["CreateUserId"].ToString() != "")
                {
                    model.CreateUserId = int.Parse(row["CreateUserId"].ToString());
                }
                if (row["CreateDate"] != null && row["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                }
                if (row["UpdateUserId"] != null && row["UpdateUserId"].ToString() != "")
                {
                    model.UpdateUserId = int.Parse(row["UpdateUserId"].ToString());
                }
                if (row["UpdateDate"] != null && row["UpdateDate"].ToString() != "")
                {
                    model.UpdateDate = DateTime.Parse(row["UpdateDate"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["CallID"] != null && row["CallID"].ToString() != "")
                {
                    model.CallID = long.Parse(row["CallID"].ToString());
                }
                if (row["CallOutNDType"] != null && row["CallOutNDType"].ToString() != "")
                {
                    model.CallOutNDType = int.Parse(row["CallOutNDType"].ToString());
                }
            }
            return model;
        }
        #endregion

        #region Holly数据库
        /// 判断电话号码是什么类型 0：黑名单；1：白名单；2：一般用户
        /// <summary>
        /// 判断电话号码是什么类型 0：黑名单；1：白名单；2：一般用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <param name="CallDisplayId"></param>
        /// <returns></returns>
        public int GetPhoneNumberType(string PhoneNumber, int CallDisplayId)
        {
            string sqlStr = @"SELECT top 1 [TYPE] FROM dbo.BlackWhiteList
                              WHERE Status=0 AND ExpiryDate>='" + DateTime.Now + "' AND EffectiveDate<='" + DateTime.Now + "' and PhoneNum='" + PhoneNumber + "' AND (CDIDs & " + CallDisplayId + ") = " + CallDisplayId;
            object obj = SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sqlStr);
            if (obj == null)
            {
                return 2;
            }
            else
            {
                return (int)obj;
            }
        }
        public int IsRecIdExistForSynch(int RecId)
        {
            string sqlStr = @"SELECT count(1) FROM dbo.BlackWhiteList WHERE RecId = " + RecId;
            object obj = SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sqlStr);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return (int)obj;
            }
        }
        /// 增加一条数据
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddForSynch(Entities.BlackWhiteList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into  dbo.BlackWhiteList (");
            strSql.Append("RecId,Type,PhoneNum,EffectiveDate,ExpiryDate,CallType,CDIDS,Reason,SynchrodataStatus,CreateUserId,CreateDate,UpdateUserId,UpdateDate,Status)");
            strSql.Append(" values (");
            strSql.Append("@RecId,@Type,@PhoneNum,@EffectiveDate,@ExpiryDate,@CallType,@CDIDS,@Reason,@SynchrodataStatus,@CreateUserId,@CreateDate,@UpdateUserId,@UpdateDate,@Status)");
            SqlParameter[] parameters = {
                    new SqlParameter("@RecId", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,20),
					new SqlParameter("@EffectiveDate", SqlDbType.DateTime),
					new SqlParameter("@ExpiryDate", SqlDbType.DateTime),
					new SqlParameter("@CallType", SqlDbType.Int,4),
					new SqlParameter("@CDIDS", SqlDbType.Int,4),
					new SqlParameter("@Reason", SqlDbType.NVarChar,500),
					new SqlParameter("@SynchrodataStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@UpdateUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.RecId;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.PhoneNum;
            parameters[3].Value = model.EffectiveDate;
            parameters[4].Value = model.ExpiryDate;
            parameters[5].Value = model.CallType;
            parameters[6].Value = model.CDIDS;
            parameters[7].Value = model.Reason;
            parameters[8].Value = model.SynchrodataStatus;
            parameters[9].Value = model.CreateUserId;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.UpdateUserId;
            parameters[12].Value = model.UpdateDate;
            parameters[13].Value = model.Status;

            return SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, strSql.ToString(), parameters);

        }
        /// 更新一条数据
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateForSynch(Entities.BlackWhiteList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  dbo.BlackWhiteList set ");
            strSql.Append("Type=@Type,");
            strSql.Append("PhoneNum=@PhoneNum,");
            strSql.Append("EffectiveDate=@EffectiveDate,");
            strSql.Append("ExpiryDate=@ExpiryDate,");
            strSql.Append("CallType=@CallType,");
            strSql.Append("CDIDS=@CDIDS,");
            strSql.Append("Reason=@Reason,");
            strSql.Append("SynchrodataStatus=@SynchrodataStatus,");
            strSql.Append("CreateUserId=@CreateUserId,");
            strSql.Append("CreateDate=@CreateDate,");
            strSql.Append("UpdateUserId=@UpdateUserId,");
            strSql.Append("UpdateDate=@UpdateDate,");
            strSql.Append("Status=@Status");
            strSql.Append(" where RecId=@RecId");
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,20),
					new SqlParameter("@EffectiveDate", SqlDbType.DateTime),
					new SqlParameter("@ExpiryDate", SqlDbType.DateTime),
					new SqlParameter("@CallType", SqlDbType.Int,4),
					new SqlParameter("@CDIDS", SqlDbType.Int,4),
					new SqlParameter("@Reason", SqlDbType.NVarChar,500),
					new SqlParameter("@SynchrodataStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@UpdateUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@RecId", SqlDbType.Int,4)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.PhoneNum;
            parameters[2].Value = model.EffectiveDate;
            parameters[3].Value = model.ExpiryDate;
            parameters[4].Value = model.CallType;
            parameters[5].Value = model.CDIDS;
            parameters[6].Value = model.Reason;
            parameters[7].Value = model.SynchrodataStatus;
            parameters[8].Value = model.CreateUserId;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.UpdateUserId;
            parameters[11].Value = model.UpdateDate;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.RecId;

            int rows = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// 批量删除数据
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteListForSynch(string RecIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from  dbo.BlackWhiteList ");
            strSql.Append(" where RecId in (" + Dal.Util.SqlFilterByInCondition(RecIdlist) + ")  ");
            int rows = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// 获取多选ID
        /// <summary>
        /// 获取多选ID
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public string GetCallDisplayMutilIDHolly(string tel)
        {
            string sql = "SELECT MutilID FROM dbo.CallDisplay WHERE TelMainNum='" + StringHelper.SqlFilter(tel) + "'";
            return CommonFunction.ObjectToString(SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sql));
        }
        #endregion

        #region 黑名单验证
        /// 验证是否黑名单
        /// <summary>
        /// 验证是否黑名单
        /// </summary>
        /// <param name="blackListCheckType"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool CheckPhoneAndTelIsInBlackList(BlackListCheckType blackListCheckType, string phone)
        {
            phone = SqlFilter(phone);
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                //先验证CC端
                if ((blackListCheckType & BlackListCheckType.BT2_CC) == BlackListCheckType.BT2_CC)
                {
                    string sql = "SELECT COUNT(*) FROM dbo.BlackWhiteList WHERE Status=0 AND Type=0 AND PhoneNum='" + phone + "'";
                    int num = (int)SqlHelper.ExecuteScalar(conn, CommandType.Text, sql);
                    if (num > 0)
                    {
                        //是CC的黑名单
                        return true;
                    }
                }
                //再验证CRM端
                if ((blackListCheckType & BlackListCheckType.BT1_CRM) == BlackListCheckType.BT1_CRM)
                {
                    //验证联系人表 会员表 客户表
                    string sql = @"SELECT COUNT(*) FROM (
                                            SELECT 0 AS typeid
                                            FROM CRM2009.dbo.ContactInfo
                                            WHERE Status=0
                                            AND (REPLACE(OfficeTel,'-','')='" + phone + @"' OR REPLACE(Phone,'-','')='" + phone + @"')
                                            UNION
                                            SELECT 1 AS typeid
                                            FROM CRM2009.dbo.DMSMember 
                                            WHERE Status=0 AND REPLACE(Phone,'-','')='" + phone + @"'
                                            UNION
                                            SELECT 2 AS typeid
                                            FROM CRM2009.dbo.CustInfo 
                                            WHERE Status=0 AND REPLACE(OfficeTel,'-','')='" + phone + @"'
                                            ) tmp";
                    int num = (int)SqlHelper.ExecuteScalar(conn, CommandType.Text, sql);
                    if (num > 0)
                    {
                        //是CRM的黑名单
                        return true;
                    }
                }
            }
            //不是黑名单
            return false;
        }
        /// 判断此号码是否为免打扰号码：-1：是已删除的免打扰号码；0：是免打扰号码；1：是已过期的免打扰号码；2：不是免打扰号码
        /// <summary>
        /// 判断此号码是否为免打扰号码：-1：是已删除的免打扰号码；0：是免打扰号码；1：是已过期的免打扰号码；2：不是免打扰号码
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public int PhoneNumIsNoDisturb(string PhoneNumber)
        {
            string sqlStr = @"SELECT top 1 [Status] FROM dbo.BlackWhiteList
                              WHERE Type=0 AND PhoneNum='" + SqlFilter(PhoneNumber) + "'";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (obj == null)
            {
                return 2;
            }
            else
            {
                int intVal;
                if (int.TryParse(obj.ToString(), out intVal))
                {
                    return intVal;
                }
                else
                {
                    return 2;
                }
            }
        }
        /// 更新一条免打扰数据
        /// <summary>
        /// 更新一条免打扰数据
        /// </summary>
        public bool UpdateNoDisturbData(Entities.BlackWhiteList model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecId", SqlDbType.Int,4),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,20),
                    new SqlParameter("@CallType", SqlDbType.Int,4),
                    new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ExpiryDate", SqlDbType.DateTime),					
					new SqlParameter("@CDIDS", SqlDbType.Int,4),
					new SqlParameter("@Reason", SqlDbType.NVarChar,500),
                    new SqlParameter("@UpdateUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@SynchrodataStatus", SqlDbType.Int,4),
                    new SqlParameter("@CallID", SqlDbType.BigInt),
                    new SqlParameter("@CallOutNDType", SqlDbType.Int,4),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserId", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4),
                    new SqlParameter("@backvalue", SqlDbType.Int,4)};
            parameters[0].Value = model.RecId;
            parameters[1].Value = model.PhoneNum;
            parameters[2].Value = model.CallType;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.ExpiryDate.ToString("yyyy-MM-dd 0:0:0");
            parameters[5].Value = model.CDIDS;
            parameters[6].Value = model.Reason;
            parameters[7].Value = model.UpdateUserId;
            parameters[8].Value = model.UpdateDate;
            parameters[9].Value = model.SynchrodataStatus;
            parameters[10].Value = model.CallID;
            parameters[11].Value = model.CallOutNDType;
            parameters[12].Value = model.CreateDate.ToString("yyyy-MM-dd 0:0:0");
            parameters[13].Value = model.CreateUserId;
            parameters[14].Value = model.BGID;
            parameters[15].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_NoDisturbData_Update", parameters);
            return (int)(parameters[15].Value) == 1 ? true : false;
        }
        /// 增加一条免打扰数据
        /// <summary>
        /// 增加一条免打扰数据
        /// </summary>
        public int AddNoDisturbData(Entities.BlackWhiteList model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,20),
					new SqlParameter("@EffectiveDate", SqlDbType.DateTime),
					new SqlParameter("@ExpiryDate", SqlDbType.DateTime),
					new SqlParameter("@CallType", SqlDbType.Int,4),
					new SqlParameter("@CDIDS", SqlDbType.Int,4),
					new SqlParameter("@Reason", SqlDbType.NVarChar,500),
					new SqlParameter("@SynchrodataStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CallID", SqlDbType.BigInt),
                    new SqlParameter("@CallOutNDType", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4),
                    new SqlParameter("@backvalue", SqlDbType.Int,4)};
            parameters[0].Value = model.Type;
            parameters[1].Value = model.PhoneNum;
            parameters[2].Value = model.EffectiveDate.ToString("yyyy-MM-dd 0:0:0");
            parameters[3].Value = model.ExpiryDate.ToString("yyyy-MM-dd 0:0:0");
            parameters[4].Value = model.CallType;
            parameters[5].Value = model.CDIDS;
            parameters[6].Value = model.Reason;
            parameters[7].Value = model.SynchrodataStatus;
            parameters[8].Value = model.CreateUserId;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.CallID;
            parameters[12].Value = model.CallOutNDType;
            parameters[13].Value = model.BGID;
            parameters[14].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_NoDisturbData_Insert", parameters);
            return (int)(parameters[14].Value);
        }
        /// 根据电话号码和类型，获取RecID
        /// <summary>
        /// 根据电话号码和类型，获取RecID
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetRecIDByPhoneNumberAndType(string phoneNumber, int type)
        {
            string strSql = "SELECT TOP 1 RecId FROM dbo.BlackWhiteList WHERE PhoneNum='" + phoneNumber + "' AND Type=" + type;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                int backVal;
                if (int.TryParse(obj.ToString(), out backVal))
                {
                    return backVal;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// 获取有效和失效的外呼黑名单
        /// <summary>
        /// 获取有效和失效的外呼黑名单
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="maxRow"></param>
        /// <returns></returns>
        public DataTable GetBlackListDataForOutCall(long timestamp, int maxRow)
        {
            string sql = @"SELECT TOP " + maxRow + @"
                                    a.PhoneNum AS Phone,
                                    a.ExpiryDate AS EndTime,
                                    CASE a.CallOutNDType 
                                    WHEN 1 THEN '客户主动要求不再联系'
                                    WHEN 2 THEN '疑似经销商/经纪人'
                                    WHEN 3 THEN '疑似调研咨询公司'
                                    WHEN 4 THEN '疑似公司内部员工测试'
                                    WHEN 5 THEN '其他'
                                    ELSE '' END AS Reason,
                                    a.Reason AS Remark,
                                    a.Status,
                                    a.CreateUserID,
                                    b.TrueName AS CreateUserName,
                                    a.CreateDate AS CreateTime,
                                    CAST(a.[TIMESTAMP] AS BIGINT) AS [TIMESTAMP]
                                    FROM dbo.BlackWhiteList a
                                    LEFT JOIN dbo.v_userinfo b ON a.CreateUserId=b.UserID
                                    WHERE 
                                    --黑名单
                                    a.Type=0 
                                    --外呼
                                    AND a.CallType&2=2
                                    --正常和删除
                                    AND a.Status IN (0,-1) 
                                    --时间戳
                                    AND a.[TIMESTAMP]>" + timestamp + @" 
                                    ORDER BY a.[TIMESTAMP] ASC";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        #endregion
    }
}
