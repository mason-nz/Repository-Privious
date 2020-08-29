using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.Utils.Data;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.Dal.ExamineLabel
{
    public class ExmaineBrandLabel : DataBase
    {
        public static readonly ExmaineBrandLabel Instance = new ExmaineBrandLabel();
        /// <summary>
        /// zlb 2017-10-20
        /// 查询待审品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public DataTable QueryPendingAuditBrandList(ReqsAuditBrandDto ReqDto)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@BrandType", SqlDbType.Int),
                    new SqlParameter("@BrandId", SqlDbType.Int),
                    new SqlParameter("@StartDate", SqlDbType.VarChar,10),
                    new SqlParameter("@EndDate", SqlDbType.VarChar,10),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = ReqDto.BrandType;
            parameters[1].Value = ReqDto.BrandId;
            parameters[2].Value = ReqDto.StartDate;
            parameters[3].Value = ReqDto.EndDate;
            parameters[4].Value = ReqDto.PageSize;
            parameters[5].Value = ReqDto.PageIndex;
            parameters[6].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QueryPendingAuditBrandList", parameters);
            int totalCount = (int)(parameters[6].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询已审品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public DataTable QueryAuditedBrandList(ReqsAuditBrandDto ReqDto)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@BrandType", SqlDbType.Int),
                    new SqlParameter("@BrandId", SqlDbType.Int),
                    new SqlParameter("@StartDate", SqlDbType.VarChar,10),
                    new SqlParameter("@EndDate", SqlDbType.VarChar,10),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = ReqDto.BrandType;
            parameters[1].Value = ReqDto.BrandId;
            parameters[2].Value = ReqDto.StartDate;
            parameters[3].Value = ReqDto.EndDate;
            parameters[4].Value = ReqDto.PageSize;
            parameters[5].Value = ReqDto.PageIndex;
            parameters[6].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QueryAuditedBrandList", parameters);
            int totalCount = (int)(parameters[6].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-10-23
        /// 根据审核批次ID查询品牌信息
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        public DataTable QueryBrandInfo(int BatchAuditID)
        {
            string strSql = $@"SELECT  BMA.BrandID ,
        BMA.SerialID ,
        CB.Name AS BrandName ,
        CS.Name AS SerialName ,
        U1.UserName AS SubmitMan ,
        BMA.CreateTime AS SubmitTime ,
        U2.UserName AS ExamineMan ,
        BMA.AuditTime AS ExamineTime
FROM    dbo.BatchMediaAudit BMA
        LEFT JOIN BaseData2017.dbo.CarBrand CB ON BMA.BrandID = CB.BrandID
        LEFT JOIN BaseData2017.dbo.CarSerial CS ON BMA.SerialID = CS.SerialID
        LEFT JOIN BaseData2017.dbo.CarMasterBrand CMB ON CB.MasterId = CMB.MasterID
        LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON BMA.CreateUserID = U1.UserID
        LEFT JOIN Chitunion2017.dbo.UserInfo U2 ON BMA.AuditUserID = U2.UserID			
WHERE   BMA.BatchAuditID = {BatchAuditID}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
    }
}
