using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Dal.ExamineLabel;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.Result;
using XYAuto.Utils.Data;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.Dal.Result
{
    public class ResultLabel : DataBase
    {
        public static readonly ResultLabel Instance = new ResultLabel();
        /// <summary>
        /// zlb 2017-10-24
        /// 根据最终结果ID查询媒体信息
        /// </summary>
        /// <param name="MediaResultID">结果ID</param>
        /// <returns></returns>
        public DataTable QueryResultMediaInfo(int MediaResultID)
        {
            string strSql = $@"SELECT  HomeUrl,BM.MediaType,BM.MediaName AS Name,BM.MediaNumber AS Number,HeadImg,U1.UserName AS SubmitMan,BM.LastUpdateTime AS SubmitTime FROM MediaLabelResult BM 	
		                        LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON BM.LastUpdateUserID=U1.UserID WHERE BM.MediaResultID={MediaResultID}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 查询最终结果品牌信息
        /// </summary>
        /// <param name="MediaResultID">结果ID</param>
        /// <returns></returns>
        public DataTable QueryResultBrandInfo(int MediaResultID)
        {
            string strSql = $@"SELECT  BMA.BrandID ,
        BMA.SerialID ,
        CB.Name AS BrandName ,
        CS.Name AS SerialName ,
        U1.UserName AS SubmitMan ,
        BMA.LastUpdateTime AS SubmitTime
FROM    dbo.MediaLabelResult BMA
        LEFT JOIN BaseData2017.dbo.CarBrand CB ON BMA.BrandID = CB.BrandID
        LEFT JOIN BaseData2017.dbo.CarSerial CS ON BMA.SerialID = CS.SerialID
        LEFT JOIN BaseData2017.dbo.CarMasterBrand CMB ON CB.MasterId = CMB.MasterID
        LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON BMA.LastUpdateUserID = U1.UserID
WHERE   BMA.MediaResultID = {MediaResultID}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 根据审核批次ID获取结果ID
        /// </summary>
        /// <param name="BatchAuditID">批次ID</param>
        /// <param name="TaskType">任务类型</param>
        /// <returns></returns>
        public int GetResultID(int BatchAuditID, int TaskType)
        {
            string strSql = $"SELECT MediaResultID FROM MediaLabelResult WHERE TaskType={TaskType} AND Status=0";
            if (TaskType == (int)EnumTaskType.媒体)
            {
                DataTable dt = ExmaineMediaLabel.Instance.QueryMediaInfo(BatchAuditID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int mediaType = Convert.ToInt32(dt.Rows[0]["MediaType"]);
                    if (mediaType == (int)EnumMediaType.微信)
                    {
                        strSql += $" AND MediaNumber='{dt.Rows[0]["Number"].ToString()}' AND  MediaType={mediaType}";
                    }
                    else if (mediaType == (int)EnumMediaType.头条 || mediaType == (int)EnumMediaType.搜狐)
                    {
                        strSql += $" AND HomeUrl='{dt.Rows[0]["HomeUrl"].ToString()}' AND  MediaType={mediaType}";
                    }
                    else
                    {
                        strSql += $" AND MediaName='{dt.Rows[0]["Name"].ToString()}' AND  MediaType={mediaType}";
                    }
                }
                else
                {
                    strSql += " AND 1=2";
                }
            }
            else
            {
                DataTable dt = ExmaineBrandLabel.Instance.QueryBrandInfo(BatchAuditID);
                if (TaskType == (int)EnumTaskType.子品牌)
                {
                    strSql += $" AND BrandID={Convert.ToInt32(dt.Rows[0]["BrandID"])}";
                }
                else
                {
                    strSql += $" AND SerialID={Convert.ToInt32(dt.Rows[0]["SerialID"])}";
                }
            }
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 插入最终审核结果表
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <param name="UserId">操作人</param>
        /// <returns></returns>
        public int InsertMediaLabelResult(int BatchAuditID, int UserId)
        {
            string strSql = $@"INSERT INTO MediaLabelResult (TaskType,MediaType,MediaName,MediaNumber,HeadImg,IsSelfDo,HomeUrl,BrandID,SerialID,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID) 
		                       SELECT TaskType,MediaType,MediaName,MediaNumber,HeadImg,IsSelfDo,HomeUrl,BrandID,SerialID,GETDATE(),{UserId},GETDATE(),{UserId} FROM BatchMediaAudit WHERE BatchAuditID={BatchAuditID};SELECT @@Identity ";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 增加审核普通标签
        /// </summary>
        /// <param name="labelOperate">标签</param>
        /// <param name="MediaResultID">结果ID</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        public void InsertResultLabel(AuditLabel labelOperate, int MediaResultID, int UserId, bool IsUpdate)
        {
            StringBuilder sb = new StringBuilder();
            DateTime dtNow = DateTime.Now;
            if (IsUpdate)
            {
                sb.AppendFormat($"DELETE FROM ResultLabel WHERE MediaResultID={MediaResultID};DELETE FROM MediaResultIPSubLabe WHERE MediaResultID = {MediaResultID};DELETE FROM MediaResultSonIPLabel WHERE MediaResultID ={MediaResultID} ");
            }
            string strInsert = "";
            if (labelOperate.TaskType == (int)EnumTaskType.媒体)
            {
                sb.AppendFormat(" INSERT INTO dbo.ResultLabel (MediaResultID,TitleID,Type,IsCustom,Name,CreateTime,CreateUserID) VALUES");
                if (labelOperate.Category != null)
                {
                    foreach (var item in labelOperate.Category)
                    {
                        strInsert += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}),", MediaResultID, item.DictId, (int)EnumLabelType.分类, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                    }
                }
                if (labelOperate.MarketScene != null)
                {
                    foreach (var item in labelOperate.MarketScene)
                    {
                        strInsert += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}),", MediaResultID, item.DictId, (int)EnumLabelType.市场场景, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                    }
                }
                if (labelOperate.DistributeScene != null)
                {
                    foreach (var item in labelOperate.DistributeScene)
                    {
                        strInsert += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}),", MediaResultID, item.DictId, (int)EnumLabelType.分发场景, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                    }
                }
            }
            string strSql = sb.ToString();
            if (strInsert != "" || strSql.Contains("DELETE"))
            {
                if (!string.IsNullOrEmpty(strSql))
                {
                    strSql = strSql + strInsert;
                    if (strSql.Contains(","))
                    {
                        strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
                    }
                    SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
                }
            }
            InserResultLableIpInfo(labelOperate.IPLabel, MediaResultID, UserId);
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 增加审核IP标签
        /// </summary>
        /// <param name="IpLabel">标签</param>
        /// <param name="MediaResultID">结果ID</param>
        /// <param name="UserId">操作人</param>
        public void InserResultLableIpInfo(List<IpLabel> IpLabel, int MediaResultID, int UserId)
        {
            if (IpLabel != null)
            {
                DateTime dtNow = DateTime.Now;
                foreach (var item in IpLabel)
                {
                    int lableID = 0;
                    if (item.PIPID <= 0)
                    {
                        string strSqlIP = "INSERT INTO dbo.ResultLabel (MediaResultID,TitleID,Type,IsCustom,Name,CreateTime,CreateUserID) VALUES ";
                        strSqlIP += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}); SELECT @@Identity", MediaResultID, item.DictId, (int)EnumLabelType.IP, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                        object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlIP);
                        lableID = obj == null ? 0 : Convert.ToInt32(obj);
                    }
                    else
                    {
                        lableID = item.PIPID;
                    }
                    if (lableID > 0)
                    {
                        if (item.SubIP != null)
                        {
                            foreach (var itemSonIP in item.SubIP)
                            {
                                int sonIpID = 0;
                                if (itemSonIP.SubIPID <= 0)
                                {
                                    string strSqlSonIp = "INSERT INTO dbo.MediaResultIPSubLabe (MediaResultID,ResultLabelID ,TitleID , CreateTime ,CreateUserID) VALUES ";
                                    strSqlSonIp += string.Format("({0},{1},{2},'{3}',{4});SELECT @@Identity", MediaResultID, lableID, itemSonIP.DictId, dtNow, UserId);
                                    object objSonIp = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlSonIp);
                                    sonIpID = objSonIp == null ? 0 : Convert.ToInt32(objSonIp);
                                }
                                else
                                {
                                    sonIpID = itemSonIP.SubIPID;
                                }
                                if (sonIpID > 0)
                                {
                                    if (itemSonIP.Label != null)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        foreach (var itemLabel in itemSonIP.Label)
                                        {
                                            sb.Append("INSERT INTO dbo.MediaResultSonIPLabel (MediaResultID,ResultSubIPID, Name,TitleID, CreateTime,CreateUserID) VALUES ");
                                            sb.AppendFormat("({0},{1},'{2}',{3},'{4}',{5});", MediaResultID, sonIpID, SqlFilter(itemLabel.DictName), 0, dtNow, UserId);
                                        }
                                        SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 插最终结果操作日志
        /// </summary>
        /// <param name="MediaResultID">结果ID</param>
        /// <param name="TaskType">任务类型</param>
        /// <param name="OptContent">内容</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public int InsertReslutOperateInfo(int MediaResultID, int TaskType, string OptContent, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OptContent", SqlDbType.Text)};
            parameters[0].Value = OptContent;
            string strSql = $@"INSERT  INTO dbo.ReslutOperateInfo
        ( MediaResultID ,
          TaskType ,
          OptContent ,
          CreateTime ,
          CreateUserID
        )
VALUES  ( {MediaResultID}, -- MediaResultID - int
          {TaskType} , -- TaskType - int
         @OptContent , -- OptContent - text
          GETDATE() , -- CreateTime - datetime
          {UserID}  -- CreateUserID - int
        );";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 更新最终标签结果表最后更新信息
        /// </summary>
        /// <param name="MediaResultID">结果ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public int UpdateResultLastInfo(int MediaResultID, int UserID)
        {
            string strSql = $"Update MediaLabelResult set LastUpdateTime=GETDATE(),LastUpdateUserID={UserID} where MediaResultID={MediaResultID}";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 根据结果ID查询操作记录内容
        /// </summary>
        /// <param name="MediaResultID">结果ID</param>
        /// <returns></returns>
        public string SelectOptContent(int MediaResultID)
        {
            string strSql = $"SELECT TOP 1 OptContent FROM (SELECT TOP 2 RecID,OptContent FROM  ReslutOperateInfo WHERE MediaResultID=(SELECT TOP 1 MediaResultID FROM ReslutOperateInfo  WHERE MediaResultID={MediaResultID}  GROUP BY MediaResultID HAVING COUNT(1)>1) ORDER BY  CreateTime DESC) A ORDER BY A.RecID";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? "" : obj.ToString();
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询结果媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public DataTable QueryResultMediaList(ReqsAuditMediaDto ReqDto)
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
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QueryResultMediaList", parameters);
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
        /// 查询结果品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public DataTable QueryResultBrandList(ReqsAuditBrandDto ReqDto)
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
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QueryResultBrandList", parameters);
            int totalCount = (int)(parameters[6].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-11-06
        /// 插入最终结果表
        /// </summary>
        /// <param name="TaskType"></param>
        /// <param name="BrandID"></param>
        /// <param name="SerialID"></param>
        /// <param name="MediaType"></param>
        /// <param name="MediaName"></param>
        /// <param name="HeadImg"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int InsertResult(int TaskType, int BrandID, int SerialID, int MediaType, string Number, string MediaName, string HeadImg, string HomeUrl, int UserId)
        {
            string strSql = string.Format(@"INSERT INTO MediaLabelResult (TaskType,IsSelfDo,BrandID,SerialID,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,MediaType,MediaName,MediaNumber,HeadImg,HomeUrl) 
                                                                 Values ({0},{1},{2},{3},'{4}',{5},'{4}',{5},{6},'{7}','{9}','{8}','{10}') ;SELECT @@Identity ", TaskType, 0, BrandID, SerialID, DateTime.Now, UserId, MediaType, MediaName, HeadImg, Number, HomeUrl);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-11-06
        /// 查询品牌车型或媒体是否已打标签并返回已打结果主键
        /// </summary>
        /// <param name="SerialID">品牌车型ID</param>
        /// <param name="TaskType">任务类型</param>
        /// <param name="MediaInfo">媒体信息</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns></returns>
        public int GetResultID(int TaskType, int SerialID, string MediaInfo, int MediaType)
        {
            string strSql = $"SELECT MediaResultID FROM MediaLabelResult WHERE TaskType={TaskType}";
            if (TaskType == (int)EnumTaskType.媒体)
            {

                if (MediaType == (int)EnumMediaType.微信)
                {
                    strSql += $" AND MediaNumber='{MediaInfo}' AND  MediaType={MediaType}";
                }
                else if (MediaType == (int)EnumMediaType.头条 || MediaType == (int)EnumMediaType.搜狐)
                {
                    strSql += $" AND HomeUrl='{MediaInfo}' AND  MediaType={MediaType}";
                }
                else
                {
                    strSql += $" AND MediaName='{MediaInfo}' AND  MediaType={MediaType}";
                }
            }
            else
            { 

                if (TaskType == (int)EnumTaskType.子品牌)
                {
                    strSql += $" AND BrandID={SerialID}";
                }
                else
                {
                    strSql += $" AND SerialID={SerialID}";
                }
            }
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// zlb 2017-11-27
        /// 查询结果媒体车型信息
        /// </summary>
        /// <param name="MediaResultID"></param>
        /// <returns></returns>
        public DataTable QueryResultInfo(int MediaResultID)
        {
            string strSql = $@"SELECT  MR.TaskType ,
                MR.HomeUrl ,
                MR.MediaType ,
                MR.MediaName AS Name ,
                MR.MediaNumber AS Number ,
                MR.HeadImg ,
                MR.BrandID ,
                MR.SerialID
        FROM    dbo.MediaLabelResult MR
        WHERE   MR.MediaResultID ={MediaResultID}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-11-27
        /// 修改结果标签状态
        /// </summary>
        /// <param name="MediaResultID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int UpdateResultStatus(int MediaResultID, int Status)
        {
            string strSql = $"Update MediaLabelResult set Status={Status} WHERE MediaResultID ={MediaResultID}";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2018-1-4
        /// 删除结果标签
        /// </summary>
        /// <param name="MediaResultID"></param>
        /// <returns></returns>
        public int DeleteResultLabel(int MediaResultID)
        {
            string strSql = $@"DELETE FROM ResultLabel WHERE MediaResultID={MediaResultID} AND CONVERT(VARCHAR,CreateTime,23)< CONVERT(VARCHAR,GETDATE(),23)
                              DELETE FROM MediaResultIPSubLabe WHERE MediaResultID ={MediaResultID} AND CONVERT(VARCHAR,CreateTime,23)< CONVERT(VARCHAR,GETDATE(),23)
                              DELETE FROM MediaResultSonIPLabel WHERE MediaResultID = {MediaResultID} AND CONVERT(VARCHAR,CreateTime,23)< CONVERT(VARCHAR,GETDATE(),23)";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// 根据车型名称查询车型ID
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int SelectSerialId(string Name)
        {
            string strSql = $@" SELECT SerialID FROM BaseData2017.dbo.CarSerial WHERE Name='{Name}'";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }




    }
}
