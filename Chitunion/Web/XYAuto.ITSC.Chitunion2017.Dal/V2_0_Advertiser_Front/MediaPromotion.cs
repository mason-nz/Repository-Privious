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
    public class MediaPromotion : DataBase
    {
        public static readonly MediaPromotion Instance = new MediaPromotion();
        public DataTable GetMediaPromotionList(int UserID, string Name, int Status, int PageIndex, int PageSize)
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
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetMediaPromotionList", parameters);
            int totalCount = (int)(parameters[4].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public DataSet GetMediaPromotionInfo(int RecID, int UserID)
        {
            string strSql = $@"SELECT  CD.RecID ,
                            CD.Name ,
                            CD.MaterialUrl ,
                            CONVERT(VARCHAR(10),CD.BeginTime,23) BeginTime, 
                            CONVERT(VARCHAR(10),CD.EndTime,23) EndTime,
                            CD.Remark ,
                            CD.BudgetPrice ,
                            CONVERT(VARCHAR(10),CD.CreateTime,23) CreateTime,
                            D2.DictName AS StatusName
                    FROM    LE_MediaPromotion CD  WITH ( NOLOCK )
                            LEFT JOIN dbo.DictInfo D2  WITH ( NOLOCK ) ON CD.Status = D2.DictId
                    WHERE   CD.Status > 0
                            AND CD.RecID = {RecID} AND CD.USERID={UserID}; 
                    SELECT  CB.Name + '-' + CS.ShowName AS CarStyleText
                    FROM    LE_Car_Promotion CP  WITH ( NOLOCK )
                            LEFT JOIN BaseData2017.dbo.CarSerial CS WITH ( NOLOCK ) ON CP.ModelID = CS.SerialID
                            LEFT JOIN BaseData2017.dbo.CarBrand CB WITH ( NOLOCK ) ON CB.BrandID = CS.BrandID
                    WHERE   CP.MediaID = {RecID} 
                    SELECT  ( CASE WHEN AI1.AreaName = AI2.AreaName THEN AI1.AreaName
                                   ELSE AI1.AreaName + '-' + AI2.AreaName
                              END ) AreaText
                    FROM    LE_Area_Promotion AP WITH ( NOLOCK )
                            LEFT JOIN dbo.AreaInfo AI1 WITH ( NOLOCK ) ON AP.CityID = AI1.AreaID
                            LEFT JOIN dbo.AreaInfo AI2 WITH ( NOLOCK ) ON AI1.PID = AI2.AreaID
                    WHERE   AP.MediaID = {RecID}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);

        }
        public int AddMediaPromotionInfo(ReqPromotionDto Dto, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,50),
                    new SqlParameter("@MaterialUrl", SqlDbType.VarChar,200),
                    new SqlParameter("@Remark", SqlDbType.VarChar,4000),
                    new SqlParameter("@BeginTime", SqlDbType.VarChar,10),
                    new SqlParameter("@EndTime", SqlDbType.VarChar,10),
                    new SqlParameter("@BudgetPrice", SqlDbType.Decimal,18),
                    };
            parameters[0].Value = Dto.Name.Trim();
            parameters[1].Value = Dto.MaterialUrl;
            parameters[2].Value = Dto.Remark != null ? Dto.Remark.Trim() : "";
            parameters[3].Value = Dto.BeginTime.Trim();
            parameters[4].Value = Dto.EndTime.Trim();
            parameters[5].Value = Dto.BudgetPrice;
            string strSql = $@"INSERT INTO dbo.LE_MediaPromotion
                            ( Name ,
                              MaterialUrl ,
                              Remark ,
                              BudgetPrice ,
                              BeginTime ,
                              EndTime ,
                              UserID ,
                              CreateTime ,
                              Status
                            )
                    VALUES  ( @Name, -- Name - varchar(50)
                              @MaterialUrl , -- MaterialUrl - varchar(100)
                              @Remark , -- Remark - varchar(4000)
                              @BudgetPrice, -- BudgetPrice - decimal
                              @BeginTime , -- BeginTime - date
                              @EndTime , -- EndTime - date
                              {UserID} , -- UserID - int
                              GETDATE() , -- CreateTime - datetime
                              {(int)ExtensionStatusEnum.待审核} --Status - int
                            );SELECT @@IDENTITY";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        public int AddCarPromotion(List<PromotionCar> list, int MediaId)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat($@"INSERT  INTO dbo.LE_Car_Promotion
                                (MakeID, ModelID, MediaID, CreateTime )
                        VALUES  ({item.MakeID},
                                 {item.ModelID}, 
                                 {MediaId}, 
                                 GETDATE() );");
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
        public int AddAreaPromotion(List<PromotionArea> list, int MediaId)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat($@"INSERT  INTO dbo.LE_Area_Promotion
                                ( ProvinceID ,
                                  CityID ,
                                  MediaID ,
                                  CreateTime
	                            )
                        VALUES  ( {item.ProvinceID} , 
                                  {item.CityID} ,
                                  {MediaId} , 
                                  GETDATE()
	                            );");
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
        public bool IsAvailableByName(string Name)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,50),

                    };
            parameters[0].Value = Name.Trim();
            string strSql = $"select count(1) from LE_MediaPromotion where Name =@Name AND  Status >0";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? true : (Convert.ToInt32(obj) > 0 ? false : true);
        }
    }
}
