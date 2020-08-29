/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.Dal.APP
* 类 名 称 ：IpManageDal
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/25 10:38:04
********************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.APP;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.APP
{
    public class IpManageDal: DataBase
    {
        #region Instance

        public static readonly IpManageDal Instance = new IpManageDal();

        #endregion Instance
        public int AddIpRequestLog(IpRequestLogModel query)
        {
            string SQL = @" 
                        INSERT INTO dbo.LE_IP_RequestLog
                                ( IP ,
                                  Url ,
                                  ProvinceName ,
                                  CityName ,
                                  ProvinceID ,
                                  CityID ,
                                  CreateTime
                                )
                        VALUES  ( @IP ,
                                  @Url ,
                                  @ProvinceName ,
                                  @CityName ,
                                  @ProvinceID ,
                                  @CityID ,
                                  GETDATE()
                                )";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@IP",query.IP),
                new SqlParameter("@Url",query.Url),
                new SqlParameter("@ProvinceName",query.ProvinceName),
                new SqlParameter("@CityName",query.CityName),
                new SqlParameter("@ProvinceID",query.ProvinceID),
                new SqlParameter("@CityID",query.CityID)
            };

            return (int)SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
        }
    }
}
