/********************************************************
*创建人：lixiong
*创建时间：2017/9/18 15:05:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;

namespace XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.AutoResolver
{
    //public class DetailedStatisticsRsolver : ValueResolver<List<MaterielDetailedStatistics>,
    //         List<FootStatisticsDto>>
    //{
    //}

    public class DetailedStatisticsRsolver
    {
        public static string GetDistributeTime(string distributeUser)
        {
            if (string.IsNullOrWhiteSpace(distributeUser))
            {
                return string.Empty;
            }
            return distributeUser.Split('|')[1];
        }

        public static string GetExportTitle(MaterielDetailedStatistics item)
        {
            if (item.ConentType == (int)MaterielConentTypeEnum.Foot)
            {
                return item.ConentTypeName + ":" + item.ArticleTypeName;
            }
            return item.ConentTypeName + ":" + item.Title;
        }

        public static string GetExportTitle(int conentType, string conentTypeName, string articleTypeName, string title)
        {
            if (conentType == (int)MaterielConentTypeEnum.Foot)
            {
                return conentTypeName + ":" + articleTypeName;
            }
            return conentTypeName + ":" + title;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="statisticsList">所有分发下面的详情统计</param>
        /// <param name="statisticsDto">头，腰，</param>
        /// <returns></returns>
        public static void GetFootStatistics(
            IEnumerable<Entities.Distribute.MaterielDetailedStatistics> statisticsList, List<StatisticsDto> statisticsDto)
        {
            //头，腰，尾
            var list = statisticsList.OrderBy(s => s.ConentType);

            statisticsDto.ForEach(t =>
            {
                t.FootStatistics = new List<FootStatisticsDto>();
                if (t.ConentType == (int)MaterielConentTypeEnum.Head)
                {
                    var headFoot = list.Where(s => s.DistributeId == t.DistributeId && s.ArticleId == t.ArticleId);
                    t.FootStatistics.AddRange(headFoot.Select(s => new FootStatisticsDto()
                    {
                        ArticleId = s.ArticleId,
                        ArticleTypeName = s.ArticleTypeName,
                        ClickPV = s.ClickPV,
                        ClickUV = s.ClickUV,
                        ConentTypeName = s.ConentTypeName,
                        ReadNumber = -1
                    }));
                }
                else if (t.ConentType == (int)MaterielConentTypeEnum.Body)
                {
                    var headFoot = list.Where(s => s.DistributeId == t.DistributeId && s.ArticleId == t.ArticleId);
                    t.FootStatistics.AddRange(headFoot.Select(s => new FootStatisticsDto()
                    {
                        ArticleId = s.ArticleId,
                        ArticleTypeName = s.ArticleTypeName,
                        ClickPV = s.ClickPV,
                        ClickUV = s.ClickUV,
                        ConentTypeName = s.ConentTypeName,
                        ReadNumber = -1
                    }));
                }
            });
        }
    }
}