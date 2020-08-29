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
    public class ExmaineMediaLabel : DataBase
    {
        public static readonly ExmaineMediaLabel Instance = new ExmaineMediaLabel();
        /// <summary>
        /// zlb 2017-10-19
        /// 查询待审媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public DataTable QueryPendingAuditMediaList(ReqsAuditMediaDto ReqDto)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@DictId", SqlDbType.Int),
                    new SqlParameter("@StartDate", SqlDbType.VarChar,10),
                    new SqlParameter("@EndDate", SqlDbType.VarChar,10),
                    new SqlParameter("@Name", SqlDbType.VarChar,50),
                    new SqlParameter("@SelfDoBusiness", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = ReqDto.MediaType;
            parameters[1].Value = ReqDto.DictId;
            parameters[2].Value = ReqDto.StartDate;
            parameters[3].Value = ReqDto.EndDate;
            parameters[4].Value = ReqDto.Name;
            parameters[5].Value = ReqDto.SelfDoBusiness;
            parameters[6].Value = ReqDto.PageSize;
            parameters[7].Value = ReqDto.PageIndex;
            parameters[8].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QueryPendingAuditMediaList", parameters);
            int totalCount = (int)(parameters[8].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询已审媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public DataTable QueryAuditedMediaList(ReqsAuditMediaDto ReqDto)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@DictId", SqlDbType.Int),
                    new SqlParameter("@StartDate", SqlDbType.VarChar,10),
                    new SqlParameter("@EndDate", SqlDbType.VarChar,10),
                    new SqlParameter("@Name", SqlDbType.VarChar,50),
                    new SqlParameter("@SelfDoBusiness", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = ReqDto.MediaType;
            parameters[1].Value = ReqDto.DictId;
            parameters[2].Value = ReqDto.StartDate;
            parameters[3].Value = ReqDto.EndDate;
            parameters[4].Value = ReqDto.Name;
            parameters[5].Value = ReqDto.SelfDoBusiness;
            parameters[6].Value = ReqDto.PageSize;
            parameters[7].Value = ReqDto.PageIndex;
            parameters[8].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QueryAuditedMediaList", parameters);
            int totalCount = (int)(parameters[8].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-10-23
        /// 根据审核批次ID查询媒体信息
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        public DataTable QueryMediaInfo(int BatchAuditID)
        {
            string strSql = $@"SELECT HomeUrl,BM.MediaType,BM.MediaName AS Name,BM.MediaNumber AS Number,HeadImg,U1.UserName AS SubmitMan,BM.CreateTime AS SubmitTime,U2.UserName AS ExamineMan,BM.AuditTime AS ExamineTime FROM BatchMediaAudit BM  LEFT JOIN Chitunion2017.dbo.UserInfo U1
			                   ON BM.CreateUserID=U1.UserID LEFT JOIN Chitunion2017.dbo.UserInfo U2 ON BM.AuditUserID=U2.UserID WHERE BM.BatchAuditID={BatchAuditID}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-10-27
        /// 根据批次ID查询媒体批次下的所有文章ID
        /// </summary>
        /// <param name="BatchAuditID"></param>
        /// <returns></returns>
        public string QueryMediaArticleIdList(int BatchAuditID)
        {
            string strSql = $@"SELECT  STUFF(( SELECT  ',' + CAST(BMA.ArticleID AS VARCHAR(50))
                FROM    ( SELECT  DISTINCT
                                    BMA.ArticleID
                          FROM      dbo.BatchMediaArticle BMA
                          WHERE     BMA.BatchMediaID IN ( SELECT
                                                              BatchMediaID
                                                          FROM
                                                              BatchMedia
                                                          WHERE
                                                              BatchAuditID = {BatchAuditID} )
                        ) BMA
              FOR
                XML PATH('')
              ), 1, 1, '')";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? "" : obj.ToString();
        }
    }
}
