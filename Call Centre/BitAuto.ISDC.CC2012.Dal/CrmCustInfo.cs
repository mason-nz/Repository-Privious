using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CrmCustInfo : DataBase
    {
        #region Instance
        public static readonly CrmCustInfo Instance = new CrmCustInfo();
        #endregion

        #region const
        private const string P_CC_Crm_SELECT_Manage = "p_CC_Crm_Select_Manage";
        private const string P_CC_CRM_SELECT_BY_ALONE_MANAGE = "P_CC_Crm_SELECT_By_Alone_Manage";
        private const string P_CC_Crm_SELECT_IDs_By_Alone = "P_CC_Crm_SELECT_IDs_By_Alone";
        private const string P_CC_Crm_Select_TaskStatus = "p_CC_Crm_Select_TaskStatus";
        private const string P_CC_Crm_Select_TaskStatus_By_Contion = "p_CC_Crm_Select_TaskStatus_By_Contion";
        private const string P_CC_Custs_Delete = "p_CC_Custs_Delete";
        private const string P_CC_Custs_Delete_By_Contion = "p_CC_Custs_Delete_By_Contion";
        private const string P_CC_Crm_Principal_Select = "p_CC_Crm_Principal_Select";
        private const string P_CustUserMappingNames_Select_by_CustID = "p_CustUserMappingNames_Select_by_CustID";
        private const string P_CustUserMappingNamesJustCC_Select_by_CustID = "p_CustUserMappingNamesJustCC_Select_by_CustID";

        private const string P_CustUserMapping_Select_by_CustID = "p_CustUserMapping_Select_by_CustID";
        private const string P_CUSTDEPARTMAPPING_INSERT_INIT = "p_CustDepartMapping_insert_Init";
        private const string P_CC_CRM_SELECT_BY_ALONE_MANAGE_FOREXPORT = "P_CC_Crm_SELECT_By_Alone_Manage_ForExport";
        #endregion

        public DataTable GetCC_CrmContactInfo(string tel)
        {
            string sql = "SELECT CustID,CName AS CustName,Phone,REPLACE(OfficeTel,'-','') OfficeTel from CRM2009.dbo.ContactInfo " +
                         "WHERE Phone ='" + StringHelper.SqlFilter(tel) + "' OR REPLACE(OfficeTel,'-','')='" + StringHelper.SqlFilter(tel) + "' " +
                         "ORDER BY ModifyTime DESC";
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sql);
            return ds.Tables[0];
        }

        public DataTable GetCC_CrmCustInfo(QueryCrmCustInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string joinwhere = string.Empty;
            string where = GenerateWhereStr(query, out joinwhere);
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@joinwhere",SqlDbType.NVarChar,4000)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = joinwhere;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CC_Crm_SELECT_Manage, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }

        private string GenerateWhereStr(QueryCrmCustInfo query, out string joinWhere)
        {
            string where = string.Empty;
            joinWhere = string.Empty;

            if (query.Officetel != Constant.STRING_INVALID_VALUE)
            {
                where += " AND ci.Officetel='" + StringHelper.SqlFilter(query.Officetel) + "' ";
            }

            //易湃会员主营品牌 add lxw 2013-1-8
            if (query.DMSMemberBrandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in ( select CustID from CRM2009.dbo.DMSMember dms  where dms.CustID=ci.CustID AND dms.ID IN ( select MemberID From CRM2009.dbo.DMSMember_Brand Where BrandID IN (" + Dal.Util.SqlFilterByInCondition(query.DMSMemberBrandID) + ") AND dms.SyncStatus = 170002))";
            }

            if (query.BrandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in ( select CustID from  CRM2009.dbo.Cust_Brand  where BrandID in (" + Dal.Util.SqlFilterByInCondition(query.BrandID) + "))";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE && query.CityID != "null")
            {
                where += " And ci.CityID=" + StringHelper.SqlFilter(query.CityID);
            }
            if (query.contactName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.ContactName like '%" + StringHelper.SqlFilter(query.contactName) + "%'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE && query.CountyID != "null")
            {
                where += " And ci.CountyID=" + StringHelper.SqlFilter(query.CountyID);
            }
            if (query.Address != Constant.STRING_EMPTY_VALUE && query.Address != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.Address like '%" + StringHelper.SqlFilter(query.Address) + "%'";
            }
            if (query.TradeMarketID != Constant.STRING_EMPTY_VALUE && query.TradeMarketID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.TradeMarketID = " + StringHelper.SqlFilter(query.TradeMarketID);
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.AbbrName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.AbbrName like '%" + StringHelper.SqlFilter(query.AbbrName) + "%'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query.ProvinceID != Constant.STRING_INVALID_VALUE && query.ProvinceID != "null")
            {
                where += " And ci.ProvinceID=" + StringHelper.SqlFilter(query.ProvinceID);
            }
            if (query.TrueName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ui.TrueName like '%" + StringHelper.SqlFilter(query.TrueName) + "%'";
            }
            if (query.Brandids != Constant.STRING_INVALID_VALUE)
            {
                string ids = query.Brandids.Trim(',');
                if (ids.Length > 0)
                {
                    where += string.Format(" and ci.custID in (select custid from CRM2009.dbo.cust_brand where brandid in ({0}))", Dal.Util.SqlFilterByInCondition(ids));
                }
            }

            if (query.LastUpdateTime_StartTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime>='" + query.LastUpdateTime_StartTime.ToString("yyyy-MM-dd") + "'";
            }
            if (query.LastUpdateTime_EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime<'" + query.LastUpdateTime_EndTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (query.Lock != Constant.INT_INVALID_VALUE)
            {
                where += " And ci.lock=" + query.Lock;
            }
            if (query.CarType != Constant.STRING_EMPTY_VALUE && query.CarType != null)
            {
                where += " And ci.CarType in (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ")";
            }
            if (query.StatusNoManage || query.StatusManaging || query.StatusManageFinsh || query.StatusNoAssign)
            {
                where += " And ( 1=-1";
                if (query.StatusNoManage)
                {
                    if (query.LastUpdateTime_StartTime == Constant.DATE_INVALID_VALUE && query.LastUpdateTime_EndTime == Constant.DATE_INVALID_VALUE)
                    {
                        where += " or tk.TaskStatus=180000";
                    }
                }
                if (query.StatusManaging)
                {
                    string sqlT = " or ((tk.TaskStatus=180001 or tk.TaskStatus=180002 or tk.TaskStatus=180009)";
                    StringBuilder sbT = new StringBuilder();
                    if (string.IsNullOrEmpty(query.AdditionalStatus) == false)
                    {
                        sqlT = sqlT + "and ({0}))";
                        foreach (string s in query.AdditionalStatus.Split(','))
                        {
                            string ss = s.Trim();
                            if (string.IsNullOrEmpty(ss) == false)
                            {
                                if (sbT.Length > 0) { sbT.Append(" or "); }
                                //if (ss.ToLower() == "as_a")
                                //{
                                //    sbT.Append(string.Format(" tas.AdditionalStatus='{0}' or tas.AdditionalStatus is null ", ss));
                                //}
                                //else
                                //{
                                sbT.Append(string.Format(" tas.AdditionalStatus='{0}' ", ss));
                                //}
                            }
                        }
                    }
                    else
                    {
                        sqlT = sqlT + "or ({0}))";
                        sbT.Append(" 1=-1 ");
                    }

                    where += string.Format(sqlT, sbT.ToString());
                }
                if (query.StatusManageFinsh)
                {
                    where += " or (tk.TaskStatus between 180003 and 180008) or tk.TaskStatus=180010 or tk.TaskStatus=180011";
                }
                if (query.StatusNoAssign)
                {
                    where += " or (tk.taskStatus is null)";
                }
                where += ")";
            }
            else
            {
                if (query.LastUpdateTime_StartTime != Constant.DATE_INVALID_VALUE || query.LastUpdateTime_EndTime != Constant.DATE_INVALID_VALUE)
                {
                    where += "and tk.taskStatus>180000";
                }
            }
            //分配员工
            if (query.UserIDAssigned != Constant.INT_INVALID_VALUE)
            {
                if (query.UserIDAssigned == 0)
                {
                    where += " And ccte.UserID IS NOT NULL";
                }
                else
                {
                    where += " And ccte.UserID IS NOT NULL and ccte.UserID=" + query.UserIDAssigned;
                }
            }

            if (query.TaskType != Constant.INT_INVALID_VALUE)
            {
                if (query.TaskType == 0)
                {
                    where += string.Format(" AND ci.CreateSource=0");
                }
                else if (query.TaskType == 1)
                {
                    where += string.Format(" And ci.CreateSource=1 ");
                }
            }
            if (!string.IsNullOrEmpty(query.StatusIDs))
            {
                where += " and ci.Status IN (" + Dal.Util.SqlFilterByInCondition(query.StatusIDs) + ")";
            }
            else
            {
                where += " and (ci.Status=0 or ci.status=1)";
            }
            if ((query.IsHaveMember && query.IsHaveNoMember) || (!query.IsHaveMember && !query.IsHaveNoMember))
            {

            }
            else
            {
                if (query.IsHaveMember)
                {
                    where += " and  EXISTS (select CustID  from CRM2009.dbo.DMSMember  Where DMSMember.CustID = ci.CustID and status>=0)  ";
                }
                if (query.IsHaveNoMember)
                {
                    where += " and NOT  EXISTS (select CustID  from CRM2009.dbo.DMSMember  Where DMSMember.CustID = ci.CustID and status>=0)  ";
                }
            }
            if (!string.IsNullOrEmpty(query.CooperatedStatusIDs))
            {
                if (query.CooperatedStatusIDs == "1" || query.CooperatedStatusIDs.Contains("1"))//有排期
                {
                    where += " and (ci.CustID  IN (select CustID from CRM2009.dbo.DMSMember Where Status=0 And SyncStatus=170002 And Cooperated=1 ";
                    where += " And MemberCode IN (select distinct membercode from mj2009.dbo.CYTMember Where Status in (1003,1007)";
                    if (!string.IsNullOrEmpty(query.MemberCooperateStatus))
                    {
                        where += " And MemberType IN (" + Dal.Util.SqlFilterByInCondition(query.MemberCooperateStatus) + ") ";
                    }
                    if (!string.IsNullOrEmpty(query.BeginMemberCooperatedTime) &&
                        !string.IsNullOrEmpty(query.EndMemberCooperatedTime))//有排期时间范围
                    {
                        where += " And begintime<='" + StringHelper.SqlFilter(query.EndMemberCooperatedTime) + " 23:59:59' And endtime >='" + StringHelper.SqlFilter(query.BeginMemberCooperatedTime) + " 0:0:0' ";
                    }
                    where += " AND CYTMember.membercode=DMSMember.MemberCode) ";
                    if (!string.IsNullOrEmpty(query.BeginNoMemberCooperatedTime) &&
                       !string.IsNullOrEmpty(query.EndNoMemberCooperatedTime))//无排期时间范围
                    {
                        where += @" And DMSMember.MemberCode IN (SELECT DISTINCT temp2.memberCode FROM (
	                                                                SELECT temp.memberCode,SUM(temp.NotCooperated) AS SUMNotCooperated,COUNT(*) CountRecord FROM (
			                                                            select membercode,
			                                                            (CASE WHEN begintime>'" + StringHelper.SqlFilter(query.EndNoMemberCooperatedTime) + @" 23:59:59' OR endtime <'" + StringHelper.SqlFilter(query.BeginNoMemberCooperatedTime) + @" 0:0:0'  THEN 1 
				                                                             ELSE 0 END) AS NotCooperated
			                                                            from mj2009.dbo.CYTMember 
			                                                            Where Status in (1003,1007)
		                                                            ) AS temp
		                                                            GROUP BY temp.memberCode
	                                                        ) AS temp2
                                                       WHERE temp2.SUMNotCooperated=temp2.CountRecord ) ";
                    }
                    where += ")) ";
                }
                if (!string.IsNullOrEmpty(query.StartMemberCooperatedBeginTime) ||
                       !string.IsNullOrEmpty(query.EndMemberCooperatedBeginTime))
                {
                    where += " and (ci.CustID  IN (select CustID from CRM2009.dbo.DMSMember Where Status=0 And SyncStatus=170002 And Cooperated=1 ";
                    where += " And MemberCode IN (select distinct membercode from mj2009.dbo.CYTMember Where Status in (1003,1007)";
                    if (!string.IsNullOrEmpty(query.StartMemberCooperatedBeginTime))
                    {
                        where += " and begintime >= '" + StringHelper.SqlFilter(query.StartMemberCooperatedBeginTime) + "'";
                    }
                    if (!string.IsNullOrEmpty(query.EndMemberCooperatedBeginTime))
                    {
                        where += " and begintime <= '" + StringHelper.SqlFilter(query.EndMemberCooperatedBeginTime) + "'";
                    }
                    where += " AND CYTMember.membercode=DMSMember.MemberCode))) ";
                }
                if (query.CooperatedStatusIDs == "0" || query.CooperatedStatusIDs.Contains("0"))//无排期
                {
                    where += " and (ci.CustID IN (select CustID from CRM2009.dbo.DMSMember Where Status=0 And (Cooperated=0 OR Cooperated IS NULL) AND SyncStatus!=170008)) ";
                }
            }

            #region 之前“有排期”、“无排期”逻辑
            //if ((!string.IsNullOrEmpty(query.CooperatedStatusIDs) || !string.IsNullOrEmpty(query.CooperateStatusIDs)) &&
            //    !(query.CooperatedStatusIDs == "1,0" && query.CooperateStatusIDs == "1"))
            //{
            //    ArrayList alWhereOR = new ArrayList();
            //    if (!string.IsNullOrEmpty(query.CooperateStatusIDs))
            //    {
            //        alWhereOR.Add("ci.CooperateStatus IN (" + query.CooperateStatusIDs + ")");
            //    }
            //    if (query.CooperatedStatusIDs == "1")
            //    {
            //        alWhereOR.Add("(ci.CustID  IN (select CustID from DMSMember Where Status=0 And Cooperated IN (" + query.CooperatedStatusIDs + ") And CooperateStatus=0))");
            //    }
            //    if (query.CooperatedStatusIDs == "0")
            //    {
            //        alWhereOR.Add("(ci.CustID  IN (select CustID from DMSMember Where Status=0 And (Cooperated IN (" + query.CooperatedStatusIDs + ") OR Cooperated IS NULL)))");
            //    }
            //    string temp = string.Join(" OR ", (string[])alWhereOR.ToArray(typeof(string)));
            //    if (temp != string.Empty)
            //    {
            //        where += " And (" + temp + ")";
            //    }
            //}
            #endregion

            if (!string.IsNullOrEmpty(query.TypeID))
            {
                where += " and ci.TypeID in (" + Dal.Util.SqlFilterByInCondition(query.TypeID) + ")";
            }
            if (query.Batch != Constant.INT_INVALID_VALUE)
            {
                if (query.Batch == -1)
                {
                    where += " and (tk.tid in (a.TID) or tk.Batch is null)";
                }
                else if (query.Batch == 0)
                {
                    where += " and tk.Batch is null";
                }
                else
                {
                    where += " and tk.Batch=" + query.Batch;
                }
            }
            if (query.TID != Constant.INT_INVALID_VALUE)
            {
                where += " and tk.TID=" + query.TID;
            }
            if (query.TaskSource != Constant.INT_INVALID_VALUE)
            {
                where += " and tk.Source=" + query.TaskSource;
            }
            if (!string.IsNullOrEmpty(query.CreateTimeStart))
            {
                where += " and ci.CreateTime >= '" + StringHelper.SqlFilter(query.CreateTimeStart) + " 0:0:0' ";
            }
            if (!string.IsNullOrEmpty(query.CreateTimeEnd))
            {
                where += " and ci.CreateTime <= '" + StringHelper.SqlFilter(query.CreateTimeEnd) + " 23:59:59' ";
            }
            if (!string.IsNullOrEmpty(query.DistrictName))
            {
                where += " AND ISNULL(area3.DistinctName,ISNULL(area2.DistinctName,'')) = '" + StringHelper.SqlFilter(query.DistrictName.Trim()) + "'";
            }
            if (!string.IsNullOrEmpty(query.AreaTypeIDs))
            {
                string[] typeids = query.AreaTypeIDs.Split(',');
                for (int i = 0; i < typeids.Length; i++)
                {
                    string temp = " or ";
                    if (i == 0)
                    {
                        temp = " and (";
                    }
                    switch (typeids[i])
                    {
                        case "1"://163城区
                            //where += temp + "ci.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=1) ";
                            where += temp + @"(ci.CountyID IN (SELECT AreaID FROM CRM2009.dbo.AreaType WHERE Type=2 And Value=1) OR (
                                              (ci.CountyID=-1 or ci.CountyID is null or ci.CountyID='') And ci.CityID IN (SELECT AreaID FROM CRM2009.dbo.AreaType WHERE Type=1 And Value=1)))";

                            break;
                        case "2"://163郊区
                            where += temp + @"ci.CountyID IN (SELECT AreaID FROM CRM2009.dbo.AreaType WHERE Type=2 And Value=2)
                                             And ci.CityID IN (SELECT AreaID FROM CRM2009.dbo.AreaType WHERE Type=1 And Value=1)";
                            break;
                        case "3"://178无人城城区
                            where += temp + "ci.CityID IN (SELECT AreaID FROM CRM2009.dbo.AreaType WHERE Type=1 And Value=2) ";
                            break;
                        //case "4"://178无人城郊区
                        //    where += temp + "ci.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=2) ";
                        //    break;
                        default:
                            break;
                    }
                    if (i == typeids.Length - 1)
                    {
                        where += " ) ";
                    }
                }
            }
            if (query.IsMagazineReturn != Constant.INT_INVALID_VALUE)
            {
                if (query.IsMagazineReturn == 0)
                {
                    joinWhere += "LEFT JOIN (select distinct custID from  CC_MagazineReturn) AS cmr ON ci.CustID=cmr.CustID";
                    where += " AND cmr.CustID is null";
                }
                else if (query.IsMagazineReturn == 1)
                {
                    joinWhere += "LEFT JOIN (select distinct custID from  CC_MagazineReturn where Title='" + Utils.StringHelper.SqlFilter(query.ExecCycle) + "') AS cmr ON ci.CustID=cmr.CustID";
                    where += " AND cmr.custId is not null";
                }
            }

            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ci.CustID in (select custid from MJ2009.dbo.OrderInfo where projectName='" + SqlFilter(query.ProjectName) + "' and status>=0) ";
            }

            return where;
        }

        public DataTable GetCC_CrmCustInfoByAlone(QueryCrmCustInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string joinwhere = string.Empty;
            string where = GenerateWhereStr(query, out joinwhere);

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
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CC_CRM_SELECT_BY_ALONE_MANAGE, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }

        public DataTable GetCC_CrmCustInfoByAloneForExportCustID(QueryCrmCustInfo query)
        {
            string joinwhere = string.Empty;
            string where = GenerateWhereStr(query, out joinwhere);

            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000)
					};

            parameters[0].Value = where;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CC_CRM_SELECT_BY_ALONE_MANAGE_FOREXPORT, parameters);

            return ds.Tables[0];
        }
        /// <summary>
        /// IDs
        /// </summary>
        /// <param name="queryCustInfo"></param>
        /// <returns></returns>
        public DataTable GetCC_CrmCustIDsByAlone(QueryCrmCustInfo query)
        {
            string joinwhere = string.Empty;
            string where = GenerateWhereStr(query, out joinwhere);

            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000)
					};

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CC_Crm_SELECT_IDs_By_Alone, parameters);

            return ds.Tables[0];

        }

        public void InitCustDepartMapping(string custID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar, 50)};

            parameters[0].Value = custID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTDEPARTMAPPING_INSERT_INIT, parameters);
        }
        public string GetCustUserMappingNames(string custId)
        {
            string result = string.Empty;
            SqlParameter[] parameters = {
			new SqlParameter("@custId", SqlDbType.VarChar,50)
             };

            parameters[0].Value = custId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CustUserMappingNames_Select_by_CustID, parameters))
            {
                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(reader[0].ToString()))
                    {
                        result = reader[0].ToString();
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// 根据客户ID查询负责员工（员工仅限呼叫中心）
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public string GetCustUserMappingNamesJustCC(string custId)
        {
            string result = string.Empty;
            SqlParameter[] parameters = {
			new SqlParameter("@custId", SqlDbType.VarChar,50)
             };

            parameters[0].Value = custId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CustUserMappingNamesJustCC_Select_by_CustID, parameters))
            {
                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(reader[0].ToString()))
                    {
                        result = reader[0].ToString();
                    }
                }
            }

            return result;
        }
    }
}
