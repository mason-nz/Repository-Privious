using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Materiel
{
    //MaterielExtend
    public class MaterielExtend : DataBase
    {
        public static readonly MaterielExtend Instance = new MaterielExtend();

        public Entities.Materiel.MaterielExtend GetEntity(int materielId)
        {
            var sql = @"SELECT [MaterielID]
                              ,[ThirdID]
                              ,[Name]
                              ,[Name] AS MaterielName
                              ,[ArticleID]
                              ,[ArticleFrom]
                              ,[HeadContentURL]
                              ,[HeadContentType]
                              ,[BodyContentType]
                              ,[FootContentUrl]
                              ,[ContractNumber]
                              ,[SerialID]
                              ,[Tag]
                              ,[Category]
                              ,[CreateUserID]
                              ,[CreateTime]
                              ,[LastUpdateTime]
                        FROM DBO.MaterielExtend AS MET WITH(NOLOCK) WHERE MaterielID = @MaterielID";
            var parameters = new List<SqlParameter> { new SqlParameter("@MaterielID", materielId) };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToEntity<Entities.Materiel.MaterielExtend>(data.Tables[0]);
        }

        public Tuple<Entities.Materiel.MaterielExtend,
            List<Entities.Materiel.MaterielChannel>> GetInfo(int materielId, bool isGetChannelInfo = true)
        {
            var sql = @"
                    CREATE TABLE #TempDic(DictId INT,DictName VARCHAR(100) )

                    INSERT INTO #TempDic
                            ( DictId, DictName )
                    SELECT DictId,DictName FROM DBO.DictInfo

                    SELECT  MET.HeadContentUrl ,
                            MET.FootContentURL ,
                            MET.CreateTime ,
                            MET.Name AS MaterielName ,
                            MET.Tag ,
                            MET.Category ,
                            CS.SerialID ,
                            CS.ShowName AS SerialName,
                            CB.Name AS BrandName,
                            MET.MaterielID ,
                            MET.ThirdID ,
                            MET.ArticleFrom ,
                            DC.DictName AS ArticleFromName ,
                            MET.ContractNumber ,
                            DC1.DictName AS HeadContentTypeName ,
                            DC2.DictName AS BodyContentTypeName ,
                            MET.Tag AS HeadContentTag ,
                            MET.Category AS HeadContentClass ,
                            MEF.FootContentUrl ,
                            MEF.FootContentType ,
                            DC3.DictName AS FootContentTypeName
                    FROM    dbo.MaterielExtend AS MET WITH ( NOLOCK )
                            LEFT JOIN BaseData2017.dbo.CarSerial AS CS WITH ( NOLOCK ) ON CS.SerialID = MET.SerialID
                            LEFT JOIN BaseData2017.dbo.CarBrand AS CB WITH ( NOLOCK ) ON CB.BrandID = CS.BrandID
                            LEFT JOIN #TempDic AS DC WITH ( NOLOCK ) ON DC.DictId = MET.ArticleFrom
                            LEFT JOIN #TempDic AS DC1 WITH ( NOLOCK ) ON DC1.DictId = MET.HeadContentType
                            LEFT JOIN #TempDic AS DC2 WITH ( NOLOCK ) ON DC2.DictId = MET.BodyContentType
                            LEFT JOIN dbo.MaterielExtendFoot AS MEF WITH ( NOLOCK ) ON MEF.MaterielID = MET.MaterielID
                            LEFT JOIN #TempDic AS DC3 WITH ( NOLOCK ) ON DC3.DictId = MEF.FootContentType
                        WHERE   MET.MaterielID = @MaterielID
                        ";
            var parameters = new List<SqlParameter>();
            var channelSql = $"{ System.Environment.NewLine}";

            parameters.Add(new SqlParameter("@MaterielID", materielId));

            if (isGetChannelInfo)
            {
                channelSql += @"SELECT  MCL.* ,
                                        DC.DictName AS ChannelTypeName ,
                                        DC1.DictName AS PayTypeName ,
                                        DC2.DictName AS PayModeName
                                FROM    dbo.MaterielChannel AS MCL WITH ( NOLOCK )
                                        LEFT JOIN #TempDic AS DC WITH ( NOLOCK ) ON DC.DictId = MCL.ChannelType
                                        LEFT JOIN #TempDic AS DC1 WITH ( NOLOCK ) ON DC1.DictId = MCL.PayType
                                        LEFT JOIN #TempDic AS DC2 WITH ( NOLOCK ) ON DC2.DictId = MCL.PayMode
                                WHERE MCL.MaterielID = @MaterielID1";
                parameters.Add(new SqlParameter("@MaterielID1", materielId));
            }

            sql += channelSql;

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());

            return new Tuple<Entities.Materiel.MaterielExtend, List<Entities.Materiel.MaterielChannel>>(
                DataTableToEntity<Entities.Materiel.MaterielExtend>(data.Tables[0]),
                isGetChannelInfo ? DataTableToList<Entities.Materiel.MaterielChannel>(data.Tables[1])
                : new List<Entities.Materiel.MaterielChannel>());
        }

        public List<Entities.Materiel.MaterielChannel> GetChannelList(string ids)
        {
            var sql = @"SELECT MCL.* FROM DBO.MaterielChannel AS MCL WITH(NOLOCK)
                                WHERE MCL.ChannelID IN ({0})";

            sql = string.Format(sql, ids);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Materiel.MaterielChannel>(data.Tables[0]);
        }

        public bool UpdateContractNumber(int materielID, string contractNumber)
        {
            string sql = "UPDATE dbo.MaterielExtend SET ContractNumber = @ContractNumber WHERE MaterielID = @MaterielID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaterielID", materielID),
                new SqlParameter("@ContractNumber", contractNumber),
            };
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return rowcount > 0;
        }

        public List<Entities.Materiel.DTO.RestDivideUserDTO> GetDivideUser()
        {
            string sqlstr = @"
                            SELECT  DISTINCT
                                    VUI.SysName UserName ,
                                    VUI.UserID
                            FROM    dbo.v_UserInfo VUI
                                    JOIN dbo.UserRole UR ON UR.UserID = VUI.UserID
                                                            AND UR.RoleID IN ( 'SYS001RL00013' )";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            return DataTableToList<Entities.Materiel.DTO.RestDivideUserDTO>(ds.Tables[0]);
        }

        public Entities.Materiel.DTO.ResGetArticleInfoDTO GetArticleInfo(int ArticleId)
        {
            string sqlstr = @"
                            SELECT  RecID ArticleId ,
                                    Title ,
                                    Content ,
                                    Abstract ,
                                    HeadImg
                            FROM    BaseData2017.dbo.ArticleInfo
                            WHERE   RecID = @ArticleId";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ArticleId", ArticleId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return DataTableToEntity<Entities.Materiel.DTO.ResGetArticleInfoDTO>(ds.Tables[0]);
        }
    }
}