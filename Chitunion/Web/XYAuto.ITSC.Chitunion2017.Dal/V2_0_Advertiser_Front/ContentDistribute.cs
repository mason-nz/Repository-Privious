using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V_2_0;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.V2_0_Advertiser_Front
{
    public class ContentDistribute : DataBase
    {
        public static readonly ContentDistribute Instance = new ContentDistribute();
        public DataTable GetContentDistributeList(int UserID, string Name, int Status, int PageIndex, int PageSize)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Status", SqlDbType.Int),
                    new SqlParameter("@Name", SqlDbType.VarChar,50),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    };
            parameters[0].Value = Status;
            parameters[1].Value = Name;
            parameters[2].Value = PageIndex;
            parameters[3].Value = PageSize;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = UserID;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetContentDistributeList", parameters);
            int totalCount = (int)(parameters[4].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        public int AddContentDistributeInfo(ReqDistributeDto Dto, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,50),
                    new SqlParameter("@Link", SqlDbType.VarChar,255),
                    new SqlParameter("@Synopsis", SqlDbType.VarChar,4000),
                    new SqlParameter("@Platform", SqlDbType.Int),
                    new SqlParameter("@ImgUrl", SqlDbType.VarChar,200),
                    new SqlParameter("@BillingMode", SqlDbType.Int),
                    new SqlParameter("@BeginTime", SqlDbType.VarChar,10),
                    new SqlParameter("@EndTime", SqlDbType.VarChar,10),
                    new SqlParameter("@BudgetPrice", SqlDbType.Decimal,18),
                    };
            parameters[0].Value = Dto.Name.Trim();
            parameters[1].Value = Dto.Link.Trim();
            parameters[2].Value = Dto.Synopsis.Trim();
            parameters[3].Value = Dto.Platform;
            parameters[4].Value = Dto.ImgUrl.Trim();
            parameters[5].Value = Dto.BillingMode;
            parameters[6].Value = Dto.BeginTime.Trim();
            parameters[7].Value = Dto.EndTime.Trim();
            parameters[8].Value = Dto.BudgetPrice;
            string strSql = $@"INSERT INTO dbo.LE_ContentDistribute
                            ( Name,
                              Link,
                              Synopsis,
                              ImgUrl,
                              Platform,
                              BillingMode,
                              BudgetPrice,
                              BeginTime,
                              EndTime,
                              UserID,
                              CreateTime,
                              Status
                            )
                    VALUES(   @Name, --Name - varchar(50)
                              @Link, --Link - varchar(255)
                              @Synopsis, --Synopsis - varchar(4000)
                              @ImgUrl, --ImgUrl - varchar(200)
                              @Platform, --Platform - int
                              @BillingMode, --BillingMode - int
                              @BudgetPrice, --BudgetPrice - decimal
                              @BeginTime, --BeginTime - date
                              @EndTime, --EndTime - date
                             {UserID}, --UserID - int
                              GETDATE(), --CreateTime - datetime
                              {(int)ExtensionStatusEnum.待审核} --Status - int
                            )";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }

        public DataTable GetContentDistributeDetailInfo(int RecID,int UserID)
        {
            string strSql = $@"SELECT  CD.RecID,CD.Name,CD.Link,CD.Synopsis,CD.ImgUrl,CD.Platform, CONVERT(VARCHAR(10),CD.BeginTime,23) BeginTime,  CONVERT(VARCHAR(10),CD.EndTime,23) EndTime,CD.BudgetPrice,D1.DictName AS BillingModeName,CONVERT(VARCHAR(10),CD.CreateTime,23) CreateTime,D2.DictName AS StatusName FROM LE_ContentDistribute CD 
                               LEFT JOIN  dbo.DictInfo D1 ON   CD.BillingMode=D1.DictId
                               LEFT JOIN dbo.DictInfo D2 ON CD.Status=D2.DictId WHERE CD.Status>0 AND CD.RecID={RecID} AND CD.USERID={UserID};";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];

        }
        public int UpdateDistributeStatus(int RecID, int Status, int BeforeStatus)
        {
            string strSql = $"UPDATE LE_ContentDistribute SET Status = {Status} WHERE RecID = {RecID} AND  Status = {BeforeStatus}";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        public bool IsAvailableByName(string Name)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,50),

                    };
            parameters[0].Value = Name.Trim();
            string strSql = $"select count(1) from LE_ContentDistribute where Name =@Name AND  Status >0";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? true : (Convert.ToInt32(obj) > 0 ? false : true);
        }


    }
}
