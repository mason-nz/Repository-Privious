/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 17:41:11
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

namespace XYAuto.BUOC.ChiTuData2017.Dal.Statistics
{
    public class StatDataProfiling : DataBase
    {
        public static readonly StatDataProfiling Instance = new StatDataProfiling();

        public Entities.Statistics.StatDataProfiling GetInfo(string date)
        {
            var sql = $@"

                        SELECT SDP.RecID ,
                               SDP.HeadArticle ,
                               SDP.HeadArticleAccount ,
                               SDP.HeadAutoClean ,
                               SDP.HeadAutoCleanAccount ,
                               SDP.WaistArticle ,
                               SDP.WaistArticleClean ,
                               SDP.WaistArticleMatched ,
                               SDP.WaistArticleUnmatched ,
                               SDP.MaterialPackaged ,
                               SDP.MaterialDistribute ,
                               SDP.MaterialForward ,
                               SDP.Clues ,
                               SDP.Status ,
                               SDP.Date ,
                               SDP.CreateTime
	                            FROM dbo.Stat_DataProfiling AS SDP WITH(NOLOCK)
                        WHERE SDP.Status=0
                        AND SDP.Date = '{date}'
                    ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.Statistics.StatDataProfiling>(data.Tables[0]);
        }
    }
}