using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CrmCustInfo
    {

        #region Instance
        public static readonly CrmCustInfo Instance = new CrmCustInfo();
        #endregion

        #region Select

        public DataTable GetCC_CrmCustInfo(QueryCrmCustInfo queryCustInfo, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CrmCustInfo.Instance.GetCC_CrmCustInfo(queryCustInfo, order, currentPage, pageSize, out totalCount);
        }
        #endregion

        public DataTable GetCC_CrmContactInfo(string tel)
        {
            return Dal.CrmCustInfo.Instance.GetCC_CrmContactInfo(tel);
        }

        /// <summary>
        /// 查询crm客户(同一客户只显示最后一个批次，最后处理状态)
        /// </summary>
        /// <param name="queryCustInfo"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCC_CrmCustInfoByAlone(QueryCrmCustInfo queryCustInfo, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CrmCustInfo.Instance.GetCC_CrmCustInfoByAlone(queryCustInfo, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetCC_CrmCustIDsByAlone(QueryCrmCustInfo queryCustInfo)
        {
            return Dal.CrmCustInfo.Instance.GetCC_CrmCustIDsByAlone(queryCustInfo);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryCustInfo"></param>
        /// <returns></returns>
        public DataTable GetCC_CrmCustInfoByAloneForExportCustID(QueryCrmCustInfo queryCustInfo)
        {
            return Dal.CrmCustInfo.Instance.GetCC_CrmCustInfoByAloneForExportCustID(queryCustInfo);
        }

        public void InitCustDepartMapping(string custID)
        {
            Dal.CrmCustInfo.Instance.InitCustDepartMapping(custID);
        }

        private string GenerateWhereStr(QueryCrmCustInfo query, out string joinWhere)
        {
            string where = string.Empty;
            joinWhere = string.Empty;

            //易湃会员主营品牌 add lxw 2013-1-8
            if (query.DMSMemberBrandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in ( select CustID from  Crm2009.dbo.DMSMember dms  where dms.CustID=ci.CustID AND dms.ID IN ( select MemberID From Crm2009.dbo.DMSMember_Brand Where BrandID IN (" + query.DMSMemberBrandID + ") AND dms.SyncStatus = 170002))";
            }

            if (query.BrandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in ( select CustID from  Crm2009.dbo.Cust_Brand  where BrandID in (" + query.BrandID + "))";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE && query.CityID != "null")
            {
                where += " And ci.CityID=" + query.CityID;
            }
            if (query.contactName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.ContactName like '%" + query.contactName + "%'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE && query.CountyID != "null")
            {
                where += " And ci.CountyID=" + query.CountyID;
            }
            if (query.Address != Constant.STRING_EMPTY_VALUE && query.Address != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.Address like '%" + query.Address + "%'";
            }
            if (query.TradeMarketID != Constant.STRING_EMPTY_VALUE && query.TradeMarketID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.TradeMarketID = " + query.TradeMarketID;
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustName like '%" + query.CustName + "%'";
            }
            if (query.AbbrName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.AbbrName like '%" + query.AbbrName + "%'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query.ProvinceID != Constant.STRING_INVALID_VALUE && query.ProvinceID != "null")
            {
                where += " And ci.ProvinceID=" + query.ProvinceID;
            }
            if (query.TrueName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ui.TrueName like '%" + query.TrueName + "%'";
            }
            if (query.Brandids != Constant.STRING_INVALID_VALUE)
            {
                string ids = query.Brandids.Trim(',');
                if (ids.Length > 0)
                {
                    where += string.Format(" and ci.custID in (select custid from Crm2009.dbo.cust_brand where brandid in ({0}))", BLL.Util.SqlFilterByInCondition(ids));
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
                where += " And ci.CarType in (" + BLL.Util.SqlFilterByInCondition(query.CarType) + ")";
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
            //where += " and tk.Status=0 "; 暂不需要
            if (query.CallRecordsCount != Constant.STRING_INVALID_VALUE)
            {
                where += " and (SELECT Count(*) FROM CallRecordInfo WHERE TaskTypeID=1 AND CustID=ci.CustID)=" + query.CallRecordsCount;
            }
            if (!string.IsNullOrEmpty(query.StatusIDs))
            {
                where += " and ci.Status IN (" + query.StatusIDs + ")";
            }
            else
            {
                where += " and (ci.Status=0 or ci.status=1)";
            }
            if (query.IsHaveMember)
            {
                where += " and  EXISTS (select CustID  from Crm2009.dbo.DMSMember   Where Crm2009.dbo.DMSMember.CustID = ci.CustID)  ";
            }
            if (query.IsHaveNoMember)
            {
                where += " and NOT  EXISTS (select CustID  from Crm2009.dbo.DMSMember   Where Crm2009.dbo.DMSMember.CustID = ci.CustID)  ";
            }
            if (!string.IsNullOrEmpty(query.CooperatedStatusIDs))
            {
                if (query.CooperatedStatusIDs == "1" || query.CooperatedStatusIDs.Contains("1"))//有排期
                {
                    where += " and (ci.CustID  IN (select CustID from Crm2009.dbo.DMSMember Where Status=0 And SyncStatus=170002 And Cooperated=1 ";
                    where += " And MemberCode IN (select distinct membercode from mj2009.dbo.CYTMember Where Status in (1003,1007)";
                    if (!string.IsNullOrEmpty(query.MemberCooperateStatus))
                    {
                        where += " And MemberType IN (" + query.MemberCooperateStatus + ") ";
                    }
                    if (!string.IsNullOrEmpty(query.BeginMemberCooperatedTime) &&
                        !string.IsNullOrEmpty(query.EndMemberCooperatedTime))//有排期时间范围
                    {
                        where += " And begintime<='" + query.EndMemberCooperatedTime + " 23:59:59' And endtime >='" + query.BeginMemberCooperatedTime + " 0:0:0' ";
                    }
                    where += " ) ";
                    if (!string.IsNullOrEmpty(query.BeginNoMemberCooperatedTime) &&
                       !string.IsNullOrEmpty(query.EndNoMemberCooperatedTime))//无排期时间范围
                    {
                        where += @" And Crm2009.dbo.DMSMember.MemberCode IN (SELECT DISTINCT temp2.memberCode FROM (
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
                    where += ")) ";
                }
                if (query.CooperatedStatusIDs == "0" || query.CooperatedStatusIDs.Contains("0"))//无排期
                {
                    where += " and (ci.CustID IN (select CustID from Crm2009.dbo.DMSMember Where Status=0 And (Cooperated=0 OR Cooperated IS NULL) AND SyncStatus!=170008)) ";
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
                where += " and ci.TypeID in (" + query.TypeID + ")";
            }
            if (query.TID != Constant.INT_INVALID_VALUE)
            {
                where += " and tk.PTID=" + query.TID;
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
                            where += temp + @"(ci.CountyID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=2 And Value=1) OR (
                                              (ci.CountyID=-1 or ci.CountyID is null or ci.CountyID='') And ci.CityID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=1 And Value=1)))";

                            break;
                        case "2"://163郊区
                            where += temp + @"ci.CountyID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=2 And Value=2)
                                             And ci.CityID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=1 And Value=1)";
                            break;
                        case "3"://178无人城城区
                            where += temp + "ci.CityID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=1 And Value=2) ";
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
            //if (query.IsMagazineReturn != Constant.INT_INVALID_VALUE)
            //{
            //    if (query.IsMagazineReturn == 0)
            //    {
            //        joinWhere += "LEFT JOIN (select distinct custID from  CC_MagazineReturn) AS cmr ON ci.CustID=cmr.CustID";
            //        where += " AND cmr.CustID is null";
            //    }
            //    else if (query.IsMagazineReturn == 1)
            //    {
            //        joinWhere += "LEFT JOIN (select distinct custID from  CC_MagazineReturn where Title='" + Utils.StringHelper.SqlFilter(query.ExecCycle) + "') AS cmr ON ci.CustID=cmr.CustID";
            //        where += " AND cmr.custId is not null";
            //    }
            //}

            return where;
        }

        public string GetCustUserMappingNames(string custId)
        {
            return Dal.CrmCustInfo.Instance.GetCustUserMappingNames(custId);
        }

        public string GetCustUserMappingNamesJustCC(string custId)
        {
            return Dal.CrmCustInfo.Instance.GetCustUserMappingNamesJustCC(custId);
        }
    }
}
