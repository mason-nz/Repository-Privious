/********************************************************
*创建人：lixiong
*创建时间：2017/9/12 19:24:43
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Lable
{
    public class TitleBasicInfo : DataBase
    {
        public static readonly TitleBasicInfo Instance = new TitleBasicInfo();

        /// <summary>
        /// 获取父类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Entities.Lable.TitleBasicInfo> GeTitleBasicInfos(LableTypeEnum type)
        {
            var sql = @"
                    SELECT TB.TitleID ,
                           TB.Name ,
                           TB.Type
	                       FROM Chitunion_OP2017.dbo.TitleBasicInfo AS TB WITH(NOLOCK)
                    WHERE TB.Type = @Type AND TB.Status = 0
                    ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Type",(int)type)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunionOp2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Lable.TitleBasicInfo>(data.Tables[0]);
        }

        /// <summary>
        /// 获取子IP
        /// </summary>
        /// <param name="ipId"></param>
        /// <returns></returns>
        public List<Entities.Lable.TitleBasicInfo> GetChildrenLable(int ipId)
        {
            var sql = @"
                    SELECT  TB1.TitleID ,
                            TB1.Name,
		                    TB1.Type
                    FROM    Chitunion_OP2017.dbo.IPTitleInfo AS TI WITH ( NOLOCK )
                            LEFT JOIN Chitunion_OP2017.dbo.TitleBasicInfo AS TB1 WITH ( NOLOCK ) ON TB1.TitleID = TI.SubIP
                    WHERE   TI.PIP = @PIP
                    GROUP BY TB1.TitleID ,
                            TB1.Name,TB1.Type
                    ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@PIP",ipId)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunionOp2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Lable.TitleBasicInfo>(data.Tables[0]);
        }
    }
}