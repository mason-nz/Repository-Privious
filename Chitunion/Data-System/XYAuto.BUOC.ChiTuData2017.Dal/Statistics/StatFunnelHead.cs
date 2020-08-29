/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 16:48:14
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
    public class StatFunnelHead : DataBase
    {
        public static readonly StatFunnelHead Instance = new StatFunnelHead();

        public List<Entities.Statistics.StatFunnelHead> GetStatFunnelHead(string beginTime, string endTime)
        {
            var sql = $@"

                    SELECT SFH.RecID ,
                           SFH.SceneId ,
                           SFH.SceneName ,
                           SFH.ChannelID ,
                           SFH.ChannelName ,
                           SFH.AAScoreType ,
                           SFH.SpiderArticleCount ,
                           SFH.SpiderAccountCount ,
                           SFH.AutoArticleCount ,
                           SFH.AutoAccountCount ,
                           SFH.PrimaryArticleCount ,
                           SFH.PrimaryAccountCount ,
                           SFH.ArtificialArticleCount ,
                           SFH.ArtificialAccountCount ,
                           SFH.EncapsulateArticleCount ,
                           SFH.EncapsulateAccountCount ,
                           SFH.BeginTime ,
                           SFH.EndTime ,
                           SFH.Status ,
                           SFH.CreateTime
	                       FROM dbo.Stat_Funnel_Head AS SFH WITH(NOLOCK)
                    WHERE SFH.Status=0
                    AND SFH.SceneId = 0
                    AND SFH.ChannelID = 0
                    AND SFH.AAScoreType = 0
                    AND SFH.BeginTime = '{beginTime}'
                    AND SFH.EndTime = '{endTime}'
                    ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatFunnelHead>(data.Tables[0]);
        }

        public List<Entities.Statistics.StatFunnelWaist> GetStatFunnelWaists(string beginTime, string endTime)
        {
            var sql = $@"
                        SELECT  SFW.RecID ,
                                SFW.ChannelID ,
                                SFW.ChannelName ,
                                SFW.Category ,
                                SFW.SpiderCount ,
                                SFW.AutoCleanCount ,
                                SFW.MatchedCount ,
                                SFW.ArtificialCount ,
                                SFW.EncapsulateCount ,
                                SFW.BeginTime ,
                                SFW.EndTime ,
                                SFW.Status ,
                                SFW.CreateTime
                        FROM    dbo.Stat_Funnel_Waist AS SFW
                        WHERE   SFW.Status = 0
                                AND SFW.ChannelID = 0
                                AND SFW.BeginTime = '{beginTime}'
	                            AND SFW.EndTime = '{endTime}'
                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatFunnelWaist>(data.Tables[0]);
        }

        public List<Entities.Statistics.StatFunnelMaterial> GetStatFunnelMaterials(string beginTime, string endTime)
        {
            var sql = $@"

                            SELECT SFM.RecID ,
                                   SFM.ChannelID ,
                                   SFM.ChannelName ,
                                   SFM.SceneId ,
                                   SFM.SceneName ,
                                   SFM.AAScoreType ,
                                   SFM.Encapsulate ,
                                   SFM.Distribute ,
                                   SFM.Forward ,
                                   SFM.Clue ,
                                   SFM.BeginTime ,
                                   SFM.EndTime ,
                                   SFM.Status ,
                                   SFM.CreateTime
	                               FROM dbo.Stat_Funnel_Material AS SFM WITH(NOLOCK)
	                               WHERE SFM.Status = 0
	                               AND SFM.SceneId = 0
	                               AND SFM.ChannelID = 0
	                               AND SFM.AAScoreType = 0
	                               AND SFM.BeginTime = '{beginTime}'
	                               AND SFM.EndTime = '{endTime}'
                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatFunnelMaterial>(data.Tables[0]);
        }
    }
}