using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CC_UserCustDataRigth : DataBase
    {
        #region Instance
        public static readonly CC_UserCustDataRigth Instance = new CC_UserCustDataRigth();
        #endregion

        private const string P_CUSTUSERMAPPING_SELECT_BY_USERID = "P_CUSTUSERMAPPING_SELECT_BY_USERID";

        #region Contructor
        protected CC_UserCustDataRigth()
        {
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="custUserMappingQuery"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustUserMappingByUserID(QueryCC_CustUserMapping custUserMappingQuery, string order, int currentPage, int pageSize, out int totalCount, int currentUserID)
        {
            string where = "";
            //客户
            if (custUserMappingQuery.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustName like '%" + SqlFilter(custUserMappingQuery.CustName) + "%'";
            }
            //分组
            if (custUserMappingQuery.BGIDStr == Constant.STRING_INVALID_VALUE)
            {
                if (custUserMappingQuery.UserID != Constant.INT_INVALID_VALUE)
                {
                    where += " and a.custid in (select custid from  CRM2009.dbo.CustUserMapping where userid  =" + custUserMappingQuery.UserID + " )";
                }
            }
            //品牌
            if (custUserMappingQuery.Brandids != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustID in  ( select CustID from  CRM2009.dbo.Cust_Brand where BrandID in (" + Dal.Util.SqlFilterByInCondition(custUserMappingQuery.Brandids) + ") ) ";
            }
            //省
            if (custUserMappingQuery.ProvinceID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.ProvinceID='" + SqlFilter(custUserMappingQuery.ProvinceID) + "'";
            }
            //城市
            if (custUserMappingQuery.CityID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CityID='" + SqlFilter(custUserMappingQuery.CityID) + "'";
            }
            //区县
            if (custUserMappingQuery.CountyID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CountyID='" + SqlFilter(custUserMappingQuery.CountyID) + "'";
            }
            //add by qizq 2012-5-30 添加经营范围和客户类型
            if (!string.IsNullOrEmpty(custUserMappingQuery.CarType))
            {
                where += " And a.CarType IN (" + Dal.Util.SqlFilterByInCondition(custUserMappingQuery.CarType) + ") ";
            }

            if (!string.IsNullOrEmpty(custUserMappingQuery.TypeID))
            {
                where += " and a.TypeID in (" + Dal.Util.SqlFilterByInCondition(custUserMappingQuery.TypeID) + ")";
            }
            #region 回访日期改为工单日期
            switch (custUserMappingQuery.Contact)
            {
                case -2:
                    if (custUserMappingQuery.StartTime != Constant.DATE_INVALID_VALUE)
                    {
                        where += " and woif.CreateTime>='" + custUserMappingQuery.StartTime.ToString("yyyy-MM-dd") + " 0:00:00'";
                    }
                    if (custUserMappingQuery.EndTime != Constant.DATE_INVALID_VALUE)
                    {
                        where += " and woif.CreateTime<='" + custUserMappingQuery.EndTime.ToString("yyyy-MM-dd") + " 23:59:59'";
                    }
                    break;
                case 1:
                    if (custUserMappingQuery.StartTime != Constant.DATE_INVALID_VALUE && custUserMappingQuery.EndTime != Constant.DATE_INVALID_VALUE)
                    {
                        where += " and (woif.CreateTime<='" + custUserMappingQuery.StartTime.ToString("yyyy-MM-dd") + " 0:00:00' or woif.CreateTime>='" + custUserMappingQuery.EndTime.ToString("yyyy-MM-dd") + " 23:59:59' or woif.CreateTime is null)";
                    }
                    //where += " and (LastTime='' or LastTime is null)";
                    else
                    {
                        where += " and woif.CreateTime is null";
                    }

                    break;
                default:
                    break;
            }

            #endregion
            if (custUserMappingQuery.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustID in (select custid from MJ2009.dbo.OrderInfo where projectName='" + SqlFilter(custUserMappingQuery.ProjectName) + "' and status>=0) ";
            }
            if (custUserMappingQuery.ReqeustCCProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustID in (select pds.RelationID from ProjectInfo pj join dbo.ProjectDataSoure pds on pj.projectid=pds.projectid where pds.source=3 and pj.name like '%" + SqlFilter(custUserMappingQuery.ReqeustCCProjectName) + "%' and pj.status=1 and pds.status=1) ";
            }
            if (custUserMappingQuery.radioTaoche != Constant.STRING_INVALID_VALUE)
            {
                if (custUserMappingQuery.radioTaoche == "1")
                {
                    where += " and a.CustID IN (SELECT  CustID FROM    CRM2009.dbo.DMSMember WHERE   MemberCode IN " +
                             " (SELECT  * FROM MJ2009.dbo.f_GetDMSMemberByTaoCheTong(CONVERT(VARCHAR(10),GETDATE(),120)) ) ) ";
                }
                else if (custUserMappingQuery.radioTaoche == "2")
                {
                    where += " and a.CustID NOT IN (SELECT  CustID FROM    CRM2009.dbo.DMSMember WHERE   MemberCode IN " +
                                 " (SELECT  * FROM  MJ2009.dbo.f_GetDMSMemberByTaoCheTong(CONVERT(VARCHAR(10),GETDATE(),120)) ) ) ";
                }
            }

            //强斐 优化代码 2014-11-24
            string select = "";
            if (custUserMappingQuery.NoResponser != -2)
            {
                //无坐席时，易车网电话呼叫中心部门没有坐席时
                where += @" and ( select count(1) cnum from crm2009.dbo.custusermapping as map left join crm2009.dbo.v_userinfo as ui on map.userid=ui.userid 
                                    where custid=a.custid and ui.departid in (
                                    select  id
                                    from    sysrightsmanager.dbo.f_cid('DP00323')
                                    union
                                    select  id
                                    from    sysrightsmanager.dbo.f_cid('DP00805')
                                    ))=0";
                //负责坐席为空
                select = "''";
            }
            else
            {
                //有坐席时，按照坐席名称查询
                if (custUserMappingQuery.UserName != Constant.STRING_INVALID_VALUE)
                {
                    where += " and a.custid in (select custid from  CRM2009.dbo.CustUserMapping where userid  ='" + SqlFilter(custUserMappingQuery.UserName) + "')";
                }
                //负责坐席
                select = @"isnull(stuff((select ',' + cast(truename as varchar(50)) 
                                from crm2009.dbo.custusermapping as map left join crm2009.dbo.v_userinfo as ui on map.userid=ui.userid
                                where map.custid=a.custid and ui.departid in (
                                select  id
                                from    sysrightsmanager.dbo.f_cid('DP00323')
                                union
                                select  id
                                from    sysrightsmanager.dbo.f_cid('DP00805')
                                )
                                for xml path('')),1,1,''),'')";
            }
            if(custUserMappingQuery.TagID != Constant.STRING_INVALID_VALUE && custUserMappingQuery.TagID != "0")
            {
                where += " AND EXISTS(SELECT TagID FROM CRM2009.dbo.CustTagMapping WHERE CustID= a.CustID AND TagID=" + StringHelper.SqlFilter(custUserMappingQuery.TagID) + " )";
            }

            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@select", SqlDbType.VarChar,8000),
                    new SqlParameter("@where", SqlDbType.VarChar,8000),
			        new SqlParameter("@order", SqlDbType.NVarChar,100),
			        new SqlParameter("@pagesize", SqlDbType.Int,4),
			        new SqlParameter("@page", SqlDbType.Int,4),
			        new SqlParameter("@totalRecorder", SqlDbType.Int,4),
                    new SqlParameter("@CurrentUserID", SqlDbType.Int,4)
             };
            parameters[0].Value = select;
            parameters[1].Value = where;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[6].Value = currentUserID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTUSERMAPPING_SELECT_BY_USERID, parameters);

            totalCount = int.Parse(parameters[5].Value.ToString());

            return ds.Tables[0];
        }

        public DataTable GetCustUserMappingTagStatisticsByUserID(QueryCC_CustUserMapping custUserMappingQuery, int currentUserID)
        {
            string where = "";
            //客户
            if (custUserMappingQuery.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustName like '%" + SqlFilter(custUserMappingQuery.CustName) + "%'";
            }
            //分组
            if (custUserMappingQuery.BGIDStr == Constant.STRING_INVALID_VALUE)
            {
                if (custUserMappingQuery.UserID != Constant.INT_INVALID_VALUE)
                {
                    where += " and a.custid in (select custid from  CRM2009.dbo.CustUserMapping where userid  =" + custUserMappingQuery.UserID + " )";
                }
            }
            //品牌
            if (custUserMappingQuery.Brandids != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustID in  ( select CustID from  CRM2009.dbo.Cust_Brand where BrandID in (" + Dal.Util.SqlFilterByInCondition(custUserMappingQuery.Brandids) + ") ) ";
            }
            //省
            if (custUserMappingQuery.ProvinceID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.ProvinceID='" + SqlFilter(custUserMappingQuery.ProvinceID) + "'";
            }
            //城市
            if (custUserMappingQuery.CityID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CityID='" + SqlFilter(custUserMappingQuery.CityID) + "'";
            }
            //区县
            if (custUserMappingQuery.CountyID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CountyID='" + SqlFilter(custUserMappingQuery.CountyID) + "'";
            }
            //add by qizq 2012-5-30 添加经营范围和客户类型
            if (!string.IsNullOrEmpty(custUserMappingQuery.CarType))
            {
                where += " And a.CarType IN (" + Dal.Util.SqlFilterByInCondition(custUserMappingQuery.CarType) + ") ";
            }

            if (!string.IsNullOrEmpty(custUserMappingQuery.TypeID))
            {
                where += " and a.TypeID in (" + Dal.Util.SqlFilterByInCondition(custUserMappingQuery.TypeID) + ")";
            }
            #region 回访日期改为工单日期
            switch (custUserMappingQuery.Contact)
            {
                case -2:
                    if (custUserMappingQuery.StartTime != Constant.DATE_INVALID_VALUE)
                    {
                        where += " and woif.CreateTime>='" + custUserMappingQuery.StartTime.ToString("yyyy-MM-dd") + " 0:00:00'";
                    }
                    if (custUserMappingQuery.EndTime != Constant.DATE_INVALID_VALUE)
                    {
                        where += " and woif.CreateTime<='" + custUserMappingQuery.EndTime.ToString("yyyy-MM-dd") + " 23:59:59'";
                    }
                    break;
                case 1:
                    if (custUserMappingQuery.StartTime != Constant.DATE_INVALID_VALUE && custUserMappingQuery.EndTime != Constant.DATE_INVALID_VALUE)
                    {
                        where += " and (woif.CreateTime<='" + custUserMappingQuery.StartTime.ToString("yyyy-MM-dd") + " 0:00:00' or woif.CreateTime>='" + custUserMappingQuery.EndTime.ToString("yyyy-MM-dd") + " 23:59:59' or woif.CreateTime is null)";
                    }
                    //where += " and (LastTime='' or LastTime is null)";
                    else
                    {
                        where += " and woif.CreateTime is null";
                    }

                    break;
                default:
                    break;
            }

            #endregion
            if (custUserMappingQuery.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustID in (select custid from MJ2009.dbo.OrderInfo where projectName='" + SqlFilter(custUserMappingQuery.ProjectName) + "' and status>=0) ";
            }
            if (custUserMappingQuery.ReqeustCCProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.CustID in (select pds.RelationID from ProjectInfo pj join dbo.ProjectDataSoure pds on pj.projectid=pds.projectid where pds.source=3 and pj.name like '%" + SqlFilter(custUserMappingQuery.ReqeustCCProjectName) + "%' and pj.status=1 and pds.status=1) ";
            }
            if (custUserMappingQuery.radioTaoche != Constant.STRING_INVALID_VALUE)
            {
                if (custUserMappingQuery.radioTaoche == "1")
                {
                    where += " and a.CustID IN (SELECT  CustID FROM    CRM2009.dbo.DMSMember WHERE   MemberCode IN " +
                             " (SELECT  * FROM MJ2009.dbo.f_GetDMSMemberByTaoCheTong(CONVERT(VARCHAR(10),GETDATE(),120)) ) ) ";
                }
                else if (custUserMappingQuery.radioTaoche == "2")
                {
                    where += " and a.CustID NOT IN (SELECT  CustID FROM    CRM2009.dbo.DMSMember WHERE   MemberCode IN " +
                                 " (SELECT  * FROM  MJ2009.dbo.f_GetDMSMemberByTaoCheTong(CONVERT(VARCHAR(10),GETDATE(),120)) ) ) ";
                }
            }

            //强斐 优化代码 2014-11-24
           
            if (custUserMappingQuery.NoResponser != -2)
            {
                //无坐席时，易车网电话呼叫中心部门没有坐席时
                where += @" and ( select count(1) cnum from crm2009.dbo.custusermapping as map left join crm2009.dbo.v_userinfo as ui on map.userid=ui.userid 
                                    where custid=a.custid and ui.departid in (
                                    select  id
                                    from    sysrightsmanager.dbo.f_cid('DP00323')
                                    union
                                    select  id
                                    from    sysrightsmanager.dbo.f_cid('DP00805')
                                    ))=0";
            }
            else
            {
                //有坐席时，按照坐席名称查询
                if (custUserMappingQuery.UserName != Constant.STRING_INVALID_VALUE)
                {
                    where += " and a.custid in (select custid from  CRM2009.dbo.CustUserMapping where userid  ='" + SqlFilter(custUserMappingQuery.UserName) + "')";
                }
            }
            if (custUserMappingQuery.TagID != Constant.STRING_INVALID_VALUE && custUserMappingQuery.TagID != "0")
            {
                //where += " AND EXISTS(SELECT TagID FROM CRM2009.dbo.CustTagMapping WHERE CustID= a.CustID AND TagID=" + custUserMappingQuery.TagID + " )";
                where += " AND EXISTS(SELECT TagID FROM CRM2009.dbo.CustTagMapping WHERE CustID= a.CustID AND TagID in (" + Dal.Util.SqlFilterByInCondition(custUserMappingQuery.TagID) + ") )";
            }
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@where", SqlDbType.VarChar,8000),
                    new SqlParameter("@CurrentUserID", SqlDbType.Int,4)
             };
            parameters[0].Value = where;
            parameters[1].Value = currentUserID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CustUserMapping_Statistics_by_userTag", parameters);

            return (ds == null || ds.Tables.Count<1) ? null : ds.Tables[0];
        }
    
    }
}
