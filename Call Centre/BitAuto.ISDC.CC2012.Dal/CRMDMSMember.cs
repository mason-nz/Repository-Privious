using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CRMDMSMember : DataBase
    {
        #region Instance
        public static readonly CRMDMSMember Instance = new CRMDMSMember();
        #endregion

        #region const
        private const string P_CC_DMSMEMBER_ALONE_SELECT = "p_CC_DMSMember_Alone_Select";
        private const string P_CRM_DMSMEMBER_ALONE_SELECT = "p_CRM_DMSMember_Alone_Select";
        private const string CCDepartID = "DP00323";//呼叫中心部门ID
        //add by qizhiqiang 批量查询签约结果查询2012-4-11
        private const string p_CC_MemberSignResult_Select = "p_CC_MemberSignResult_Select";
        //add by lihf 根据会员号批量查询客户ID
        private const string p_CC_CustIDByMemberCode_Select = "p_CC_CustIDByMemberCode_Select";
        #endregion

        public DataTable GetCC_CRMDMSMemberInfo(QueryCRMDMSMember query, string order, int currentPage, int pageSize,string strSelectDpid, out int totalCount)
        {
            string where = string.Empty;

            where = GetWhere(query, where);


            DataSet ds;
            SqlParameter[] parameters = {
					
					new SqlParameter("@where", SqlDbType.VarChar, 8000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@DeptID", SqlDbType.NVarChar, 200)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = strSelectDpid;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CC_DMSMEMBER_ALONE_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }

        private static string GetWhere(QueryCRMDMSMember query, string where)
        {
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and DMSMember.Status = " + query.Status;
            }
            if (query.DMSMemberCode != Constant.STRING_INVALID_VALUE)
            {
                where += " and DMSMember.MemberCode = '" + SqlFilter(query.DMSMemberCode) + "'";
            }
            if (query.DMSMemberName != Constant.STRING_INVALID_VALUE)
            {
                where += " and DMSMember.Name Like '%" + SqlFilter(query.DMSMemberName) + "%'";
            }
            if (query.ProvinceID != Constant.STRING_INVALID_VALUE &&
                query.ProvinceID != "-1")
            {
                where += " and DMSMember.ProvinceID='" + SqlFilter(query.ProvinceID) + "'";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE &&
                query.CityID != "-1")
            {
                where += " and DMSMember.CityID='" + SqlFilter(query.CityID) + "'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE &&
                query.CountyID != "-1")
            {
                where += " and DMSMember.CountyID='" + SqlFilter(query.CountyID) + "'";
            }
            if (query.AreaTypeWhereStr != Constant.STRING_INVALID_VALUE)
            {
                where += query.AreaTypeWhereStr;
            }
            if (query.IsCCCreate != null)
            {
                //where += " and DMSMember.CreateUserID IN (SELECT UserID FROM v_userinfo WHERE DepartID" + (query.IsCCCreate.Value ? "" : "!") + "='" + CCDepartID + "')";
                where += " and DMSMember.CreateSource=" + (query.IsCCCreate.Value ? "1" : "0");
            }
            if (query.IsCCUserMapping != null)
            {
                where += " and DMSMember.CustID IN (SELECT CustID FROM CustUserMapping WHERE UserID IN " +
                         "(SELECT UserID FROM v_userinfo WHERE DepartID" + (query.IsCCUserMapping.Value ? "" : "!") + "='" + CCDepartID + "'))";
            }
            if (query.IsCCReturnVisit != null)
            {
                where += " and DMSMember.CustID IN (SELECT CustID FROM ReturnVisit WHERE Status=0 And CreateuserDepart " + (query.IsCCReturnVisit.Value ? "" : "!") + "='" + CCDepartID + "')";
            }
            //if (!string.IsNullOrEmpty(query.MemberCooperateStatus))
            //{
            //    where += " and DMSMember.CooperateStatus IN (" + query.MemberCooperateStatus + ") ";
            //}
            if (query.MemberCreateTimeStart != Constant.STRING_INVALID_VALUE)
            {
                where += " and DMSMember.CreateTime >= '" + StringHelper.SqlFilter(query.MemberCreateTimeStart) + " 0:0:0'";
            }
            if (query.MemberCreateTimeEnd != Constant.STRING_INVALID_VALUE)
            {
                where += " and DMSMember.CreateTime <= '" + StringHelper.SqlFilter(query.MemberCreateTimeEnd) + " 23:59:59'";
            }

            if (query.ReturnVisitTimeStart != Constant.STRING_INVALID_VALUE)
            {
                where += " and DMSMember.CustID IN (SELECT CustID FROM ReturnVisit WHERE Status=0 And CreateTime >='" + StringHelper.SqlFilter(query.ReturnVisitTimeStart) + " 0:0:0') ";
            }
            if (query.ReturnVisitTimeEnd != Constant.STRING_INVALID_VALUE)
            {
                where += " and DMSMember.CustID IN (SELECT CustID FROM ReturnVisit WHERE Status=0 And CreateTime <='" + StringHelper.SqlFilter(query.ReturnVisitTimeEnd) + " 23:59:59') ";
            }
            if (query.StrDeptS == "1")
            {
                var TelSaleDeptIDs = ConfigurationManager.AppSettings["TelSaleDeptIDs"];
                var tIds = TelSaleDeptIDs.Split(',');
                TelSaleDeptIDs = "";
                for (int i = 0; i < tIds.Length; i++)
                {
                    TelSaleDeptIDs += ",'" + tIds[i] + "'";
                }
                if (TelSaleDeptIDs.Length > 1)
                {
                    TelSaleDeptIDs = TelSaleDeptIDs.Substring(1);
                }
                where += " and EXISTS(SELECT 1 FROM CustDepartMapping cm WHERE DMSMember.CustID=cm.CustID AND cm.DepartID in( " + TelSaleDeptIDs + "))";
            }
            else if (query.StrDeptS == "0")
            {
                var TelSaleDeptIDs = ConfigurationManager.AppSettings["TelSaleDeptIDs"];
                var tIds = TelSaleDeptIDs.Split(',');
                TelSaleDeptIDs = "";
                for (int i = 0; i < tIds.Length; i++)
                {
                    TelSaleDeptIDs += ",'" + tIds[i] + "'";
                }
                if (TelSaleDeptIDs.Length > 1)
                {
                    TelSaleDeptIDs = TelSaleDeptIDs.Substring(1);
                }
                where += " and not EXISTS(SELECT 1 FROM CustDepartMapping cm WHERE DMSMember.CustID=cm.CustID AND cm.DepartID in( " + TelSaleDeptIDs + "))";
            }


            /*
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
                            where += temp + @"(DMSMember.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=1) OR (
                                              (DMSMember.CountyID=-1 or DMSMember.CountyID is null OR DMSMember.CountyID='') And DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=1)))";
                            break;
                        case "2"://163郊区
                            where += temp + @"DMSMember.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=2) 
                                              And DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=1)";
                            break;
                        case "3"://178无人城
                            where += temp + "DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=2) ";
                            break;
                        //case "4"://178无人城郊区
                        //    where += temp + "DMSMember.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=2) ";
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
            */
            if (!string.IsNullOrEmpty(query.BrandIDs))
            {
                where += " and DMSMember.ID IN (SELECT MemberID FROM DMSMember_Brand WHERE BrandID IN (" + Dal.Util.SqlFilterByInCondition(query.BrandIDs) + "))";
            }
            if (!string.IsNullOrEmpty(query.CooperatedStatusIDs))
            {
                if (query.CooperatedStatusIDs == "1" || query.CooperatedStatusIDs.Contains("1"))//有排期
                {
                    where += " And DMSMember.Status=0 And DMSMember.Cooperated=1 ";
                    where += " And DMSMember.MemberCode IN (select distinct membercode from mj2009.dbo.CYTMember Where Status in (1003,1007)";
                    if (!string.IsNullOrEmpty(query.MemberCooperateStatus))
                    {
                        where += " And MemberType IN (" + Dal.Util.SqlFilterByInCondition(query.MemberCooperateStatus) + ") ";
                    }
                    if (!string.IsNullOrEmpty(query.BeginMemberCooperatedTime) &&
                        !string.IsNullOrEmpty(query.EndMemberCooperatedTime))//有排期时间范围
                    {
                        where += " And begintime<='" + query.EndMemberCooperatedTime + " 23:59:59' And endtime >='" + query.BeginMemberCooperatedTime + " 0:0:0' ";
                    }
                    where += " AND CYTMember.membercode=DMSMember.MemberCode) ";
                    if (!string.IsNullOrEmpty(query.BeginNoMemberCooperatedTime) &&
                       !string.IsNullOrEmpty(query.EndNoMemberCooperatedTime))//无排期时间范围
                    {
                        where += @" And DMSMember.MemberCode IN (SELECT DISTINCT temp2.memberCode FROM (
	                                                                SELECT temp.memberCode,SUM(temp.NotCooperated) AS SUMNotCooperated,COUNT(*) CountRecord FROM (
			                                                            select membercode,
			                                                            (CASE WHEN begintime>'" + query.EndNoMemberCooperatedTime + @" 23:59:59' OR endtime <'" + query.BeginNoMemberCooperatedTime + @" 0:0:0'  THEN 1 
				                                                             ELSE 0 END) AS NotCooperated
			                                                            from mj2009.dbo.CYTMember 
			                                                            Where Status in (1003,1007)
		                                                            ) AS temp
		                                                            GROUP BY temp.memberCode
	                                                        ) AS temp2
                                                       WHERE temp2.SUMNotCooperated=temp2.CountRecord ) ";
                    }
                }
                //有排期开始时间范围 add=masj 2013-07-04
                if (!string.IsNullOrEmpty(query.StartMemberCooperatedBeginTime) ||
                       !string.IsNullOrEmpty(query.EndMemberCooperatedBeginTime))
                {
                    where += " And DMSMember.Status=0 And DMSMember.Cooperated=1 ";
                    where += " And DMSMember.MemberCode IN (select distinct membercode from mj2009.dbo.CYTMember Where Status in (1003,1007)";
                    if (!string.IsNullOrEmpty(query.StartMemberCooperatedBeginTime))
                    {
                        where += " and begintime >= '" + StringHelper.SqlFilter(query.StartMemberCooperatedBeginTime) + "'";
                    }
                    if (!string.IsNullOrEmpty(query.EndMemberCooperatedBeginTime))
                    {
                        where += " and begintime <= '" + StringHelper.SqlFilter(query.EndMemberCooperatedBeginTime) + "'";
                    }
                    where += " AND CYTMember.membercode=DMSMember.MemberCode) ";
                }
                if (query.CooperatedStatusIDs == "0" || query.CooperatedStatusIDs.Contains("0"))//无排期
                {
                    where += " and DMSMember.Status=0 And (DMSMember.Cooperated=0 OR DMSMember.Cooperated IS NULL) AND DMSMember.SyncStatus!=170008 ";
                }
            }
            if (!string.IsNullOrEmpty(query.MemberType))
            {
                where += " and DMSMember.MemberType IN (" + Dal.Util.SqlFilterByInCondition(query.MemberType) + ")";
            }
            //add by qizq 2012-8-2 排期确认时间
            if (!string.IsNullOrEmpty(query.ConfirmDateEnd) && !string.IsNullOrEmpty(query.ConfirmDateStart))
            {
                where += " and DMSMember.MemberCode in (select distinct membercode from mj2009.dbo.CYTMember where confirmtime is not null and confirmtime between '" + query.ConfirmDateStart + " 0:00:00' and  '" + query.ConfirmDateEnd + " 23:59:59' and membercode!='' and status in ('1003','1007') and begintime<=endtime)";
            }


            //add by masj 2013-06-28 
            if (query.IsMagazineReturn != Constant.INT_INVALID_VALUE)
            {
                if (query.IsMagazineReturn == 0)
                {
                    where += " and DMSMember.ID not in (select distinct DMSMemberID from  CRM2009.dbo.CC_MagazineReturn)";
                }
                else if (query.IsMagazineReturn == 1)
                {
                    where += " and DMSMember.ID in (select distinct DMSMemberID from  CRM2009.dbo.CC_MagazineReturn where Title='" + Utils.StringHelper.SqlFilter(query.ExecCycle) + "')";
                }
            }
            if (query.CustContactOfficeTypeCode > 0)
            {
                where += @" and DMSMember.CustID in (SELECT CustID FROM (
                                SELECT CustID,MAX(ID) AS ID
                                FROM CRM2009.dbo.ContactInfo
                                WHERE Status=0 AND OfficeTypeCode=" + query.CustContactOfficeTypeCode + @"
                                GROUP BY CustID
                            ) AS a)";
            }
            if (query.MemberSyncStatus != Constant.STRING_INVALID_VALUE)
            {
                where += " And SyncStatus in (" + Dal.Util.SqlFilterByInCondition(query.MemberSyncStatus) + ")";
            }
            return where;
        }


        public DataTable GetCRMDMSMemberInfo(QueryCRMDMSMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            string innerwhere = string.Empty;//客户联系人条件

            string joinwhere = string.Empty;
            where = GetWhere(query, where);

            if (query.CustContactOfficeTypeCode > 0)
            {
                innerwhere = @" LEFT JOIN (SELECT ci.CustID,CName,Title,OfficeTypeCode 
					            FROM ContactInfo AS ci
					            INNER JOIN (
						            SELECT CustID,MAX(ID) AS ID
						            FROM ContactInfo
						            WHERE Status=0 AND OfficeTypeCode=" + query.CustContactOfficeTypeCode + @"
						            GROUP BY CustID
					            ) AS a ON a.ID=ci.ID ) AS m ON m.CustID=DMSMember.CustID ";
                where += " AND m.OfficeTypeCode=" + query.CustContactOfficeTypeCode;
            }
            else
            {
                //联系人导出：如果未选择客户联系人职级，需要过滤联系人为空 add lxw 13.7.29
                where += " AND m.CName != '' ";
            }
            if (query.IsMagazineReturn != Constant.INT_INVALID_VALUE)
            {
                if (query.IsMagazineReturn == 0)
                {
                    joinwhere += "LEFT JOIN (select distinct DMSMemberID from  CC_MagazineReturn) AS cmr ON DMSMember.ID=cmr.DMSMemberID";
                    where += " AND cmr.DMSMemberID is null";
                }
                else if (query.IsMagazineReturn == 1)
                {
                    joinwhere += "LEFT JOIN (select distinct DMSMemberID from  CC_MagazineReturn where Title='" + Utils.StringHelper.SqlFilter(query.ExecCycle) + "') AS cmr ON DMSMember.ID=cmr.DMSMemberID";
                    where += " AND cmr.DMSMemberID is not null";
                }
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 8000),
                    new SqlParameter("@innerwhere", SqlDbType.VarChar, 8000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@joinwhere",SqlDbType.VarChar,8000),
                    new SqlParameter("@DeptID",SqlDbType.NVarChar,200)
					};

            parameters[0].Value = where;
            parameters[1].Value = innerwhere;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[6].Value = joinwhere;
            parameters[7].Value = query.SelectDeptID;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CRM_DMSMEMBER_ALONE_SELECT, parameters);

            totalCount = int.Parse(parameters[5].Value.ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 根据会员号查出客户ID
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCustIDByMemberCode(string where)
        {
            DataSet ds;
            SqlParameter[] parameters = {
				new SqlParameter("@where", SqlDbType.NVarChar,100000), 
            };
            parameters[0].Value = where;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, p_CC_CustIDByMemberCode_Select, parameters);

            return ds.Tables[0];
        }

        //add by qizhiqiang 批量查询会员签约结果2012-4-11
        /// <summary>
        /// 会员签约结果
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetMemberSignResult(string where)
        {
            DataSet ds;
            SqlParameter[] parameters = {
				new SqlParameter("@where", SqlDbType.NVarChar,100000), 
            };
            parameters[0].Value = where;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, p_CC_MemberSignResult_Select, parameters);

            return ds.Tables[0];
        }
        //add by qizhiqiang 批量查询会员签约结果2012-4-11
        /// <summary>
        /// 会员签约结果
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetMemberSignResult(string name, string where)
        {
            DataTable dt = null;
            dt = Dal.CRMDMSMember.Instance.GetMemberSignResult(where);
            if (dt != null && dt.Rows.Count > 0)
            {
                //Loger.Log4Net.Info(name + " 在 " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm").Replace("-", "/") + " 进行了导出会员操作，导出" + dt.Rows.Count + "条记录");
            }
            return dt;
        }

        public DataTable GetDMSMemberByCodeStr(string codeStr)
        {
            string sqlStr = "SELECT  * FROM DMSMember WHERE MemberCode IN (" + Dal.Util.SqlFilterByInCondition(codeStr) + ")";
            DataSet ds;

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlStr);

            return ds.Tables[0];
        }
    }
}
