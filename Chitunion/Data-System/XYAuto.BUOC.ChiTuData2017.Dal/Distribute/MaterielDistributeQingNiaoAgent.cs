/********************************************************
*创建人：lixiong
*创建时间：2017/10/16 16:56:56
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Distribute
{
    public class MaterielDistributeQingNiaoAgent : DataBase
    {
        public static readonly MaterielDistributeQingNiaoAgent Instance = new MaterielDistributeQingNiaoAgent();

        public void Insert(Entities.Distribute.MaterielDistributeQingNiaoAgent entity)
        {
            var filterDate = entity.DistributeDate.AddDays(1).ToString("yyyy-MM-dd");
            var sql = $@"
                    IF ( NOT EXISTS ( SELECT    1
                                      FROM      dbo.Materiel_DistributeQingNiaoAgent WITH ( NOLOCK )
                                      WHERE     MaterielId = {entity.MaterielId}
                                                AND [Type] = {entity.Type}
                                                AND ArticleId = {entity.ArticleId}
                                                AND DistributeDate >= '{entity.DistributeDate.ToString("yyyy-MM-dd")}'
                                                AND DistributeDate < '{filterDate}')
                       )
                        BEGIN
                            INSERT INTO DBO.Materiel_DistributeQingNiaoAgent
                                    ( MaterielId ,
                                      ArticleId ,
                                      Type ,
                                      DistributeDate ,
                                      Status ,
                                      CreateTime ,
                                      CreateUserId
                                    )
                            VALUES  ( {entity.MaterielId} , -- MaterielId - int
                                      {entity.ArticleId} , -- ArticleId - int
                                      {entity.Type} , -- Type - int
                                      '{entity.DistributeDate}' , -- DistributeDate - datetime
                                      0 , -- Status - int
                                      GETDATE() , -- CreateTime - datetime
                                      0  -- CreateUserId - int
                                    )
                        END
                    ";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// <summary>
        /// 获取某天分发的物料详情的最小时间
        /// </summary>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        public List<Entities.Distribute.MaterielDistributeQingNiaoAgent> GetPullDateMaterielIds(string queryDate)
        {
            var date = Convert.ToDateTime(queryDate).AddDays(1).ToString("yyyy-MM-dd");
            var sql = $@"

                    SELECT  MIN(DN1.DistributeDate) AS DistributeDate ,
                            DN1.MaterielId ,
                            DN1.ArticleId
                    FROM    ( SELECT DISTINCT
                                        DNAT.MaterielId ,
                                        DNAT.ArticleId
                              FROM      dbo.Materiel_DistributeQingNiaoAgent AS DNAT WITH ( NOLOCK )
                              WHERE     DNAT.DistributeDate >= '{queryDate}' AND DNAT.DistributeDate < '{date}'
                                        AND DNAT.Type = 1
                            ) AS T1
                            INNER JOIN dbo.Materiel_DistributeQingNiaoAgent AS DN1 WITH ( NOLOCK ) ON DN1.MaterielId = T1.MaterielId
					                    AND DN1.ArticleId = T1.ArticleId
                    GROUP BY DN1.MaterielId ,
                            DN1.ArticleId
                    ";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Distribute.MaterielDistributeQingNiaoAgent>(data.Tables[0]);
        }

        public int Insert(List<Entities.Distribute.MaterielDistributeQingNiaoAgent> list)
        {
            if (!list.Any())
            {
                return 1;
            }
            var sqlCode = new StringBuilder();
            sqlCode.AppendFormat(@"INSERT INTO [dbo].[Materiel_DistributeQingNiaoAgent]
                                           ([MaterielId]
                                           ,[ArticleId]
                                           ,[Type]
                                           ,[DistributeDate]
                                           ,[Status]
                                           ,[CreateTime]
                                           ,[CreateUserId])
                                     VALUES
                                          ");
            list.ForEach(s =>
            {
                sqlCode.AppendFormat(@" ({0},{1},{2},'{3}',0,getdate(),0),",
                    s.MaterielId, s.ArticleId, s.Type, s.DistributeDate);
            });

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlCode.ToString().Trim(','));
        }

        public int IsExist(DateTime queryDate)
        {
            var sql = $@"
                    SELECT COUNT(*)
                    FROM dbo.Materiel_DistributeQingNiaoAgent AS DQN WITH ( NOLOCK )
                    WHERE DQN.DistributeDate >= '{queryDate.ToString("yyyy-MM-dd")}'
                        AND DQN.DistributeDate < '{queryDate.AddDays(1).ToString("yyyy-MM-dd")}'
                    ";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }
    }
}