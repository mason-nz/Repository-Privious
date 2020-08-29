using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.QingNiao
{
    //物料统计
    public partial class ChituMaterialStat : DataBase
    {
        public static readonly ChituMaterialStat Instance = new ChituMaterialStat();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.QingNiao.ChituMaterialStat entity, int source)
        {
            var strSql = new StringBuilder();

            strSql.AppendFormat($@"
IF (NOT EXISTS(SELECT 1 FROM dbo.Materiel_DistributeDetailed WITH(NOLOCK)
        WHERE Date ='{entity.Dt}' AND MaterielId={entity.Material_Id} AND Source = {source}))
BEGIN
    INSERT  INTO dbo.Materiel_DistributeDetailed
                        ( MaterielId ,
                          Date ,
                          PV ,
                          UV ,
                          OnLineAvgTime ,
                          BrowsePageAvg ,
                          JumpProportion ,
                          InquiryNumber ,
                          SessionNumber ,
                          TelConnectNumber ,
                          Source ,
                          Status ,
                          CreateTime ,
                          CreateUserId
                        ) values ({entity.Material_Id},'{entity.Dt}',{entity.Total_Pv},{entity.Total_Uv},{entity.Avg_Dur},-1,-1,-1,-1,-1,{source},0,getdate() ,{-2})
END
");
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 同步数据到物料分发明细表
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int CopyDataToMateriel(string date)
        {
            var sql = string.Format(@"
        DELETE FROM DBO.Materiel_DistributeDetailed
        WHERE Date ='{0}' AND Source = {1}

                INSERT  INTO dbo.Materiel_DistributeDetailed
                        ( MaterielId ,
                          Date ,
                          PV ,
                          UV ,
                          OnLineAvgTime ,
                          BrowsePageAvg ,
                          JumpProportion ,
                          InquiryNumber ,
                          SessionNumber ,
                          TelConnectNumber ,
                          Source ,
                          Status ,
                          CreateTime ,
                          CreateUserId,
                          DistributeUrl ,
                          DistributeDetailType
                        )
                        SELECT  CMS.material_id ,
                                CMS.dt ,
                                CMS.total_pv ,
                                CMS.total_uv ,
                                CMS.avg_dur ,
                                -1 ,
                                -1 ,
                                -1 ,
                                -1 ,
                                -1 ,
                                {1} ,
                                0 ,
                                GETDATE() ,
                                -2 ,
                                MaterielUrl,
                                0
                        FROM    dbo.chitu_material_stat AS CMS WITH ( NOLOCK )
                                LEFT JOIN Chitunion_OP2017.dbo.MaterielExtend AS ME WITH ( NOLOCK ) ON ME.MaterielID = CMS.material_id
                        WHERE   CMS.dt = '{0}'

", date, (int)DistributeTypeEnum.QuanWangYu
                );
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}