using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using System.Collections.Generic;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CC_CSTMember : DataBase
    {
        #region Instance
        public static readonly CC_CSTMember Instance = new CC_CSTMember();
        #endregion

        #region const
        private const string P_CC_CSTMEMBER_SELECT = "p_CC_CSTMember_Select";
        private const string P_CC_CSTMEMBER_INSERT = "p_CC_CSTMember_Insert";
        private const string P_CC_CSTMEMBER_UPDATE = "p_CC_CSTMember_Update";
        private const string P_CC_CSTMEMBER_DELETE = "p_CC_CSTMember_Delete";
        private const string P_CC_CSTMember_SELECT_BY_ID = "p_CC_CSTMember_select_by_id";
        private const string P_CC_CSTMEMBER_SELECT_CREATESOURCEBYCC = "p_CC_CSTMember_Select_CreateSourceByCC";
        private const string P_CC_CSTMEMBER_SELECT_STATCREATESOURCEBYCC = "p_CC_CSTMember_Select_StatCreateSourceByCC";
        #endregion

        /// <summary>
        /// 获取要导出的排期信息
        /// </summary>
        /// <param name="MemberStr"></param>
        /// <returns></returns>
        public DataTable GetOrderInfo(string MemberStr)
        {
            string sqlStr = "SELECT "
                            + " [会员ID]=m.CstMemberID,"
                            + " [会员名称]=m.FullName,"
                            + " [会员省份]=m.ProvinceName,"
                            + " [会员城市]=m.CityName,"
                            + " [会员区县]=m.CountyName,"
                            + " [合作名称]=MemberCode+'('+AdDateCode+')',"
                            + " [销售类型]=CASE usestyle WHEN '4001' THEN '销售' ELSE '' END,"
                            + " [执行周期]=BeginTime+'至'+EndTime,"
                            + " [排期创建时间]=CONVERT(varchar(50),CreateTime)"
                            + " FROM MJ2009.dbo.orderinfo"
                            + " LEFT JOIN "
                            + " ("
                                + " SELECT DISTINCT me.CstMemberID,me.FullName,"
                                + " (SELECT AreaName FROM dbo.AreaInfo WHERE AreaID=me.ProvinceID) ProvinceName,"
                                + " (SELECT AreaName FROM dbo.AreaInfo WHERE AreaID=me.CityID) CityName,"
                                + " (SELECT AreaName FROM dbo.AreaInfo WHERE AreaID=me.CountyID) CountyName"
                                 + " FROM dbo.CstMember me  WHERE CstMemberID<>-2 "
                            + " ) m ON MJ2009.dbo.OrderInfo.MemberCode=m.CstMemberID"
                            + " WHERE ordertype =6003 AND Status IN (1003,1007) AND usestyle=4001 "
                            + " AND m.CstMemberID IN"
                            + " ("
                            + Dal.Util.SqlFilterByInCondition(MemberStr)
                            + " )";

            DataSet ds;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlStr);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据任务编号查询车商通会员信息
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public List<Entities.ProjectTask_CSTMember> GetCC_CSTMemberByTID(string TID)
        {
            List<Entities.ProjectTask_CSTMember> list = new List<Entities.ProjectTask_CSTMember>();
            string sqlStr = "SELECT * FROM ProjectTask_CSTMember WHERE status=0 and  PTID='" + Utils.StringHelper.SqlFilter(TID)+"'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                Entities.ProjectTask_CSTMember cstMember = new Entities.ProjectTask_CSTMember();
                cstMember = LoadSingleCC_CSTMember(dr);
                list.Add(cstMember);
            }

            return list;
        }
        private static Entities.ProjectTask_CSTMember LoadSingleCC_CSTMember(DataRow row)
        {
            Entities.ProjectTask_CSTMember model = new Entities.ProjectTask_CSTMember();


            if (row["ID"] != DBNull.Value)
            {
                model.ID = Convert.ToInt32(row["ID"].ToString());
            }

            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["OriginalCSTRecID"] != DBNull.Value)
            {
                model.OriginalCSTRecID = row["OriginalCSTRecID"].ToString();
            }

            if (row["VendorCode"] != DBNull.Value)
            {
                model.VendorCode = row["VendorCode"].ToString();
            }

            if (row["FullName"] != DBNull.Value)
            {
                model.FullName = row["FullName"].ToString();
            }

            if (row["ShortName"] != DBNull.Value)
            {
                model.ShortName = row["ShortName"].ToString();
            }

            if (row["VendorClass"] != DBNull.Value)
            {
                model.VendorClass = int.Parse(row["VendorClass"].ToString());
            }

            if (row["SuperId"] != DBNull.Value)
            {
                model.SuperId = int.Parse(row["SuperId"].ToString());
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

            if (row["PostCode"] != DBNull.Value)
            {
                model.PostCode = row["PostCode"].ToString();
            }

            if (row["TrafficInfo"] != DBNull.Value)
            {
                model.TrafficInfo = row["TrafficInfo"].ToString();
            }

            if (row["CreateTime"] != DBNull.Value)
            {
                model.CreateTime = Convert.ToDateTime(row["CreateTime"].ToString());
            }

            if (row["CreateUserID"] != DBNull.Value)
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }

            if (row["Status"] != DBNull.Value)
            {
                model.Status = Convert.ToInt32(row["Status"].ToString());
            }

            //if (row["SyncStatus"] != DBNull.Value)
            //{
            //    model.SyncStatus = Convert.ToInt32(row["SyncStatus"].ToString());
            //}

            return model;
        }
    }
}
