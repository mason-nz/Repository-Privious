using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CRMCSTMember : DataBase
    {
        #region Instance
        public static readonly CRMCSTMember Instance = new CRMCSTMember();
        #endregion

        #region const
        private const string P_CC_CSTMEMBER_ALONE_SELECT = "p_CC_CTSMember_Alone_Select";
        private const string P_CRM_DMSMEMBER_ALONE_SELECT = "p_CRM_DMSMember_Alone_Select";
        //add by lihf 2013-7-22 根据会员号查询车商通会员ID信息
        private const string p_CC_CTSMemberCustIDByMemberCode_Select = "p_CC_CTSMemberCustIDByMemberCode_Select";
        private const string CCDepartID = "DP00323";//呼叫中心部门ID
        ////add by qizhiqiang 批量查询签约结果查询2012-4-11
        //private const string p_CC_MemberSignResult_Select = "p_CC_MemberSignResult_Select";
        #endregion

        #region Select
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
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, p_CC_CTSMemberCustIDByMemberCode_Select, parameters);

            return ds.Tables[0];
        }

        public DataTable GetCC_CRMCSTMemberInfo(QueryCRMCSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where = GetWhere(query, where);


            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 8000),
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
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CC_CSTMEMBER_ALONE_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }

        private static string GetWhere(QueryCRMCSTMember query, string where)
        {
            //if (query.Status != Constant.INT_INVALID_VALUE)
            //{
            //    where += " and DMSMember.Status = " + query.Status;
            //}
            where += " and cstmember.status >=0";

            if (query.CSTMemberCode != Constant.STRING_INVALID_VALUE && query.CSTMemberCode.Length > 0)
            {
                where += " and CSTMember.CSTMemberID = '" + SqlFilter(query.CSTMemberCode) + "'";
            }
            if (query.CSTMemberName != Constant.STRING_INVALID_VALUE && query.CSTMemberName.Length > 0)
            {
                where += " and CSTMember.fullname Like '%" + SqlFilter(query.CSTMemberName) + "%'";
            }
            if (query.ProvinceID != Constant.STRING_INVALID_VALUE && query.ProvinceID.Length > 0 &&
                query.ProvinceID != "-1")
            {
                where += " and CSTMember.ProvinceID='" + SqlFilter(query.ProvinceID) + "'";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE && query.CityID.Length > 0 &&
                query.CityID != "-1")
            {
                where += " and CSTMember.CityID='" + SqlFilter(query.CityID) + "'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE && query.CountyID.Length > 0 &&
                query.CountyID != "-1")
            {
                where += " and CSTMember.CountyID='" + SqlFilter(query.CountyID) + "'";
            }
            if (query.TFCustPids != null && query.TFCustPids.Length > 0)
            {
                where += " and ci.trademarketid in (" + Dal.Util.SqlFilterByInCondition(query.TFCustPids) + ")";
            }
            if (query.MemberTypeIDs != null && query.MemberTypeIDs.Length > 0)
            {
                where += " and CSTMember.VendorClass in (" + Dal.Util.SqlFilterByInCondition(query.MemberTypeIDs) + ")";
            }
            else
            {
                query.MemberTypeIDs = "3,2";
                where += " and CSTMember.VendorClass in (" + Dal.Util.SqlFilterByInCondition(query.MemberTypeIDs) + ")";
            }
            //累计充值车商币
            if (query.LoggingAmountStarts != null && query.LoggingAmountStarts.Length > 0)
            {
                where += " and ub.UBTotalAmount>=" + int.Parse(query.LoggingAmountStarts) + " and ub.UBTotalAmount<=" + int.Parse(query.LoggingAmountEnds);
            }

            //车商币余额
            if (query.RemainAmountStarts != null && query.RemainAmountStarts.Length > 0)
            {
                where += " and ub.ucount>=" + int.Parse(query.RemainAmountStarts) + " and ub.ucount<=" + int.Parse(query.RemainAmountEnds);
            }

            //车商币有效期
            if (query.AvailabilityTimeStarts != null && query.AvailabilityTimeStarts.Length > 0)
            {
                where += " and ub.ActiveTime>= '" + StringHelper.SqlFilter(query.AvailabilityTimeStarts) + " 0:0:0' and ub.ActiveTime<= '" + StringHelper.SqlFilter(query.AvailabilityTimeEnds) + " 23:59:59'";
            }
            //累计消费车商币
            if (query.UserdAmountStarts != null && query.UserdAmountStarts.Length > 0)
            {
                where += " and ub.UBTotalExpend>=" + int.Parse(query.UserdAmountStarts) + " and ub.UBTotalExpend<=" + int.Parse(query.UserdAmountEnds);
            }
            if (!string.IsNullOrEmpty(query.MemberSyncStatus))
            {
                where += " And CSTMember.SyncStatus in (" + Dal.Util.SqlFilterByInCondition(query.MemberSyncStatus) + ")";
            }
            return where;

            //            if (query.AreaTypeWhereStr != Constant.STRING_INVALID_VALUE)
            //            {
            //                where += query.AreaTypeWhereStr;
            //            }
            //            if (query.IsCCCreate != null)
            //            {
            //                //where += " and DMSMember.CreateUserID IN (SELECT UserID FROM v_userinfo WHERE DepartID" + (query.IsCCCreate.Value ? "" : "!") + "='" + CCDepartID + "')";
            //                where += " and DMSMember.CreateSource=" + (query.IsCCCreate.Value ? "1" : "0");
            //            }
            //            if (query.IsCCUserMapping != null)
            //            {
            //                where += " and DMSMember.CustID IN (SELECT CustID FROM CustUserMapping WHERE UserID IN " +
            //                         "(SELECT UserID FROM v_userinfo WHERE DepartID" + (query.IsCCUserMapping.Value ? "" : "!") + "='" + CCDepartID + "'))";
            //            }
            //            if (query.IsCCReturnVisit != null)
            //            {
            //                where += " and DMSMember.CustID IN (SELECT CustID FROM ReturnVisit WHERE Status=0 And CreateuserDepart " + (query.IsCCReturnVisit.Value ? "" : "!") + "='" + CCDepartID + "')";
            //            }
            //            //if (!string.IsNullOrEmpty(query.MemberCooperateStatus))
            //            //{
            //            //    where += " and DMSMember.CooperateStatus IN (" + query.MemberCooperateStatus + ") ";
            //            //}
            //            if (query.MemberCreateTimeStart != Constant.STRING_INVALID_VALUE)
            //            {
            //                where += " and DMSMember.CreateTime >= '" + StringHelper.SqlFilter(query.MemberCreateTimeStart) + " 0:0:0'";
            //            }
            //            if (query.MemberCreateTimeEnd != Constant.STRING_INVALID_VALUE)
            //            {
            //                where += " and DMSMember.CreateTime <= '" + StringHelper.SqlFilter(query.MemberCreateTimeEnd) + " 23:59:59'";
            //            }

            //            if (query.ReturnVisitTimeStart != Constant.STRING_INVALID_VALUE)
            //            {
            //                where += " and DMSMember.CustID IN (SELECT CustID FROM ReturnVisit WHERE Status=0 And CreateTime >='" + StringHelper.SqlFilter(query.ReturnVisitTimeStart) + " 0:0:0') ";
            //            }
            //            if (query.ReturnVisitTimeEnd != Constant.STRING_INVALID_VALUE)
            //            {
            //                where += " and DMSMember.CustID IN (SELECT CustID FROM ReturnVisit WHERE Status=0 And CreateTime <='" + StringHelper.SqlFilter(query.ReturnVisitTimeEnd) + " 23:59:59') ";
            //            }
            //            if (!string.IsNullOrEmpty(query.AreaTypeIDs))
            //            {
            //                string[] typeids = query.AreaTypeIDs.Split(',');
            //                for (int i = 0; i < typeids.Length; i++)
            //                {
            //                    string temp = " or ";
            //                    if (i == 0)
            //                    {
            //                        temp = " and (";
            //                    }
            //                    switch (typeids[i])
            //                    {
            //                        case "1"://163城区
            //                            where += temp + @"(DMSMember.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=1) OR (
            //                                              (DMSMember.CountyID=-1 or DMSMember.CountyID is null OR DMSMember.CountyID='') And DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=1)))";
            //                            break;
            //                        case "2"://163郊区
            //                            where += temp + @"DMSMember.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=2) 
            //                                              And DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=1)";
            //                            break;
            //                        case "3"://178无人城
            //                            where += temp + "DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=2) ";
            //                            break;
            //                        //case "4"://178无人城郊区
            //                        //    where += temp + "DMSMember.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=2) ";
            //                        //    break;
            //                        default:
            //                            break;
            //                    }
            //                    if (i == typeids.Length - 1)
            //                    {
            //                        where += " ) ";
            //                    }
            //                }
            //            }
            //            if (!string.IsNullOrEmpty(query.BrandIDs))
            //            {
            //                where += " and DMSMember.ID IN (SELECT MemberID FROM DMSMember_Brand WHERE BrandID IN (" + (query.BrandIDs) + "))";
            //            }
            //            if (!string.IsNullOrEmpty(query.CooperatedStatusIDs))
            //            {
            //                if (query.CooperatedStatusIDs == "1" || query.CooperatedStatusIDs.Contains("1"))//有排期
            //                {
            //                    where += " And DMSMember.Status=0 And DMSMember.Cooperated=1 ";
            //                    where += " And DMSMember.MemberCode IN (select distinct membercode from mj2009.dbo.CYTMember Where Status in (1003,1007)";
            //                    if (!string.IsNullOrEmpty(query.MemberCooperateStatus))
            //                    {
            //                        where += " And MemberType IN (" + query.MemberCooperateStatus + ") ";
            //                    }
            //                    if (!string.IsNullOrEmpty(query.BeginMemberCooperatedTime) &&
            //                        !string.IsNullOrEmpty(query.EndMemberCooperatedTime))//有排期时间范围
            //                    {
            //                        where += " And begintime<='" + query.EndMemberCooperatedTime + " 23:59:59' And endtime >='" + query.BeginMemberCooperatedTime + " 0:0:0' ";
            //                    }
            //                    where += " ) ";
            //                    if (!string.IsNullOrEmpty(query.BeginNoMemberCooperatedTime) &&
            //                       !string.IsNullOrEmpty(query.EndNoMemberCooperatedTime))//无排期时间范围
            //                    {
            //                        where += @" And DMSMember.MemberCode IN (SELECT DISTINCT temp2.memberCode FROM (
            //	                                                                SELECT temp.memberCode,SUM(temp.NotCooperated) AS SUMNotCooperated,COUNT(*) CountRecord FROM (
            //			                                                            select membercode,
            //			                                                            (CASE WHEN begintime>'" + query.EndNoMemberCooperatedTime + @" 23:59:59' OR endtime <'" + query.BeginNoMemberCooperatedTime + @" 0:0:0'  THEN 1 
            //				                                                             ELSE 0 END) AS NotCooperated
            //			                                                            from mj2009.dbo.CYTMember 
            //			                                                            Where Status in (1003,1007)
            //		                                                            ) AS temp
            //		                                                            GROUP BY temp.memberCode
            //	                                                        ) AS temp2
            //                                                       WHERE temp2.SUMNotCooperated=temp2.CountRecord ) ";
            //                    }
            //                }
            //                if (query.CooperatedStatusIDs == "0" || query.CooperatedStatusIDs.Contains("0"))//无排期
            //                {
            //                    where += " and DMSMember.Status=0 And (DMSMember.Cooperated=0 OR DMSMember.Cooperated IS NULL) ";
            //                }
            //            }
            //            if (!string.IsNullOrEmpty(query.MemberType))
            //            {
            //                where += " and DMSMember.MemberType IN (" + query.MemberType + ")";
            //            }


        }


        public DataTable GetCRMCSTMemberInfo(QueryCRMCSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            string innerwhere = string.Empty;//客户联系人条件

            string joinwhere = string.Empty;
            where = GetWhere(query, where);

            //            if (query.CustContactOfficeTypeCode < 0)
            //            {
            //                //                innerwhere = @" LEFT JOIN  (
            //                //	                                SELECT DISTINCT z.CustID,
            //                //	                                CName=(SELECT TOP 1 CName FROM dbo.ContactInfo WHERE CustID=z.CustID AND OfficeTypeCode=z.OfficeTypeID2 ORDER BY CreateTime DESC),
            //                //	                                Title=(SELECT TOP 1 Title FROM dbo.ContactInfo WHERE CustID=z.CustID AND OfficeTypeCode=z.OfficeTypeID2 ORDER BY CreateTime DESC),
            //                //	                                OfficeTypeCode=(SELECT TOP 1 OfficeTypeCode FROM dbo.ContactInfo WHERE CustID=z.CustID AND OfficeTypeCode=z.OfficeTypeID2 ORDER BY CreateTime DESC)
            //                //	                                FROM 
            //                //                                    (
            //                //		                                SELECT v.*,
            //                //		                                OfficeTypeID2=(CASE v.OfficeTypeID WHEN 1 THEN '160002'
            //                //									                                WHEN 2 THEN '160002'
            //                //									                                WHEN 3 THEN '160004'
            //                //									                                WHEN 4 THEN '160005'
            //                //									                                WHEN 5 THEN '160000'
            //                //									                                ELSE '000000' END)
            //                //		                                FROM (
            //                //			                                SELECT y.CustID,MIN(y.OfficeTypeID) AS OfficeTypeID FROM (
            //                //				                                SELECT ci.CustID,cti.STATUS,
            //                //				                                OfficeTypeID=(CASE OfficeTypeCode WHEN 160002 THEN 1
            //                //									                                WHEN 160003 THEN 2
            //                //									                                WHEN 160004 THEN 3
            //                //									                                WHEN 160005 THEN 4
            //                //									                                WHEN 160000 THEN 5
            //                //									                                ELSE 9999 END)
            //                //				                                FROM dbo.CustInfo  AS ci 
            //                //				                                LEFT JOIN dbo.ContactInfo AS cti ON ci.CustID=cti.CustID AND ci.Status=0
            //                //			                                ) AS y
            //                //			                                GROUP BY y.CustID
            //                //		                                ) AS v
            //                //	                                ) AS z 
            //                //                                ) AS m ON m.CustID=DMSMember.CustID";
            //            }
            //            else
            //            {
            //                innerwhere = @" LEFT JOIN (SELECT ci.CustID,CName,Title,OfficeTypeCode 
            //					            FROM ContactInfo AS ci
            //					            INNER JOIN (
            //						            SELECT CustID,MAX(ID) AS ID
            //						            FROM ContactInfo
            //						            WHERE Status=0 AND OfficeTypeCode=" + query.CustContactOfficeTypeCode + @"
            //						            GROUP BY CustID
            //					            ) AS a ON a.ID=ci.ID ) AS m ON m.CustID=DMSMember.CustID ";
            //                where += " AND m.OfficeTypeCode=" + query.CustContactOfficeTypeCode;
            //            }
            //            if (query.IsMagazineReturn != Constant.INT_INVALID_VALUE)
            //            {
            //                if (query.IsMagazineReturn == 0)
            //                {
            //                    joinwhere += "LEFT JOIN (select distinct DMSMemberID from  CC_MagazineReturn) AS cmr ON DMSMember.ID=cmr.DMSMemberID";
            //                    where += " AND cmr.DMSMemberID is null";
            //                }
            //                else if (query.IsMagazineReturn == 1)
            //                {
            //                    joinwhere += "LEFT JOIN (select distinct DMSMemberID from  CC_MagazineReturn where Title='" + Utils.StringHelper.SqlFilter(query.ExecCycle) + "') AS cmr ON DMSMember.ID=cmr.DMSMemberID";
            //                    where += " AND cmr.DMSMemberID is not null";
            //                }
            //            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 8000),
                    new SqlParameter("@innerwhere", SqlDbType.VarChar, 8000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@joinwhere",SqlDbType.VarChar,8000)
					};

            parameters[0].Value = where;
            parameters[1].Value = innerwhere;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[6].Value = joinwhere;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CRM_DMSMEMBER_ALONE_SELECT, parameters);

            totalCount = int.Parse(parameters[5].Value.ToString());

            return ds.Tables[0];
        }

        //add by qizhiqiang 批量查询会员签约结果2012-4-11
        /// <summary>
        /// 会员签约结果
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        //public DataTable GetMemberSignResult(string where)
        //{
        //    DataSet ds;
        //    SqlParameter[] parameters = {
        //        new SqlParameter("@where", SqlDbType.NVarChar,4000), 
        //    };
        //    parameters[0].Value = where;
        //    ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, p_CC_MemberSignResult_Select, parameters);

        //    return ds.Tables[0];
        //}

        /// <summary>
        /// add by qizhiqiang 2012-4-20
        /// </summary>
        /// <param name="CustID">客户id</param>
        /// <returns>会员列表</returns>
        public DataTable SelectByCustID(string CustID)
        {
            StringBuilder strSql = new StringBuilder();
            //查询车商通会员
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM  CstMember ");
            strSql.Append(" WHERE   status >=0 and CustID='" + SqlFilter(CustID) + "' ");

            return SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, strSql.ToString()).Tables[0];
        }

        #endregion
    }
}
