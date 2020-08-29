/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 17:54:21
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.OpenXmlFormats.Dml;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.AutoResolver;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile
{
    public class DistributeProfile : AutoMapper.Profile
    {
        public DistributeProfile()
        {
            CreateMap<MaterielChannelDetailed, RespDistributeDto>()
                .ForMember(desc => desc.BrowsePageAvg,
                    map => map.MapFrom(s => DistributeProfile.GetBrowsePageAvg(s.PV, s.UV, s.BrowsePageAvg)))
                .ForMember(desc => desc.OnLineAvgTimeFormt,
                    map => map.MapFrom(s => DistributeProfile.GetOnLineAvgTimeFormt(s.OnLineAvgTime)));

            CreateMap<MaterielDetailedStatistics, StatisticsDto>();

            //导出数据相关

            CreateMap<RespDistributeListDto, ExportDistributeDto>()
                .ForMember(desc => desc.BussinessType,
                map => map.MapFrom(s => s.DistributeTypeName))
                    .ForMember(desc => desc.JumpProportion,
                    map => map.MapFrom(s => GetExportJumpProportion(s.JumpProportion)))
                .ForMember(desc => desc.BrowsePageAvg,
                    map => map.MapFrom(s => DistributeProfile.GetBrowsePageAvg(s.PV, s.UV, s.BrowsePageAvg)))
                .ForMember(desc => desc.OnLineAvgTimeFormt,
                    map => map.MapFrom(s => DistributeProfile.GetOnLineAvgTimeFormt(s.OnLineAvgTime)));
            //分发日结
            CreateMap<MaterielDistributeDetailed, ExportDistributeDetailedDto>()
                .ForMember(desc => desc.Date,
                    map => map.MapFrom(s => s.Date.ToString("yyyy-MM-dd")))
                .ForMember(desc => desc.SessionNumber,
                    map => map.MapFrom(s => GetExportReplaceText(s.SessionNumber)))
                .ForMember(desc => desc.TelConnectNumber,
                    map => map.MapFrom(s => GetExportReplaceText(s.TelConnectNumber)))
                .ForMember(desc => desc.InquiryNumber,
                    map => map.MapFrom(s => GetExportReplaceText(s.InquiryNumber)))
                .ForMember(desc => desc.BrowsePageAvg,
                    map => map.MapFrom(s => GetExportReplaceText(s.BrowsePageAvg)))
                .ForMember(desc => desc.JumpProportion,
                    map => map.MapFrom(s => GetExportJumpProportion(s.JumpProportion)))
                .ForMember(desc => desc.OnLineAvgTimeFormt,
                    map => map.MapFrom(s => DistributeProfile.GetOnLineAvgTimeFormt(s.OnLineAvgTime)));
            //渠道
            CreateMap<MaterielChannelDetailed, ExportDistributeChannelDto>()
                .ForMember(desc => desc.Date,
                    map => map.MapFrom(s => s.Date.ToString("yyyy-MM-dd")))
               .ForMember(desc => desc.JumpProportion,
                    map => map.MapFrom(s => GetExportJumpProportion(s.JumpProportion)))
                .ForMember(desc => desc.BrowsePageAvg,
                    map => map.MapFrom(s => DistributeProfile.GetBrowsePageAvg(s.PV, s.UV, s.BrowsePageAvg)))
                .ForMember(desc => desc.OnLineAvgTimeFormt,
                    map => map.MapFrom(s => DistributeProfile.GetOnLineAvgTimeFormt(s.OnLineAvgTime)));
            //日结详情统计（赤兔）
            CreateMap<MaterielDetailedStatistics, ExportStatisticsChiTuDto>()
                .ForMember(desc => desc.Title,
                    map => map.MapFrom(s => DetailedStatisticsRsolver.GetExportTitle(s)))
                .ForMember(desc => desc.Date,
                    map => map.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));
            //日结详情统计（青鸟经纪人）
            CreateMap<MaterielDetailedStatistics, ExportStatisticsQingNiaoAgentDto>()
                .ForMember(desc => desc.LikeNumber,
                    map => map.MapFrom(s => GetExportReplaceText(s.LikeNumber)))
                .ForMember(desc => desc.ForwardNumber,
                    map => map.MapFrom(s => GetExportReplaceText(s.ForwardNumber)))
                .ForMember(desc => desc.ReadNumber,
                    map => map.MapFrom(s => GetExportReplaceText(s.ReadNumber)))
                .ForMember(desc => desc.Title,
                    map => map.MapFrom(s => DetailedStatisticsRsolver.GetExportTitle(s)))
                .ForMember(desc => desc.Date,
                    map => map.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));
            //物料详情
            CreateMap<Entities.Distribute.MaterielInfo, RespMaterielInfoDto>()
                .ForMember(desc => desc.Scene,
                    map => map.MapFrom(s => s.SceneName))
                .ForMember(desc => desc.Url,
                    map => map.MapFrom(s => s.DistributeUrl))
                .ForMember(desc => desc.Ip,
                    map => map.MapFrom(s => GetIpInfo(s.IpList, LableTypeEnum.Ip)))
                .ForMember(desc => desc.ChildIp,
                    map => map.MapFrom(s => GetIpInfo(s.IpList, LableTypeEnum.ChildIp)));

            CreateMap<RespChituChannelDto, Entities.Distribute.MaterielChannelDetailed>()
                .ForMember(desc => desc.BrowsePageAvg,
                    map => map.MapFrom(s => DistributeProfile.GetBrowsePageAvg(s.Pv, s.Uv, -1)))
                .ForMember(desc => desc.Date,
                    map => map.MapFrom(s => s.Dt))
                .ForMember(desc => desc.OnLineAvgTime,
                    map => map.MapFrom(s => s.Avg_dur))
                .ForMember(desc => desc.TelConnectNumber,
                    map => map.MapFrom(s => s.Orders))
                .ForMember(desc => desc.ChannelName,
                    map => map.MapFrom(s => s.Channel));

            CreateMap<Entities.QingNiao.ChituClickStat, Entities.Distribute.MaterielDetailedStatistics>()
                .ForMember(desc => desc.MaterielId,
                    map => map.MapFrom(s => s.Material_Id))
                .ForMember(desc => desc.ReadNumber,
                    map => map.MapFrom(s => s.ReadNum))
                .ForMember(desc => desc.ConentType,
                    map => map.MapFrom(s => DistributeProfile.GetContentType(s.Click_Name, s.Click_Site, s.Click_Val).Item1))
                .ForMember(desc => desc.ArticleType,
                    map => map.MapFrom(s => DistributeProfile.GetContentType(s.Click_Name, s.Click_Site, s.Click_Val).Item2))
                .ForMember(desc => desc.ArticleId,
                    map => map.MapFrom(s => DistributeProfile.GetContentType(s.Click_Name, s.Click_Site, s.Click_Val).Item3));
        }

        public static string GetExportReplaceText(int number)
        {
            if (number < 0)
                return "--";
            return number.ToString();
        }

        public static string GetExportReplaceText(long number)
        {
            if (number < 0)
                return "--";
            return number.ToString();
        }

        public static string GetExportJumpProportion(decimal jumpProportion)
        {
            if (jumpProportion < 0)
            {
                return "--";
            }
            return jumpProportion.ToString("p");
        }

        /// <summary>
        /// 人均浏览页面数：物料包当天总pv/物料包当天总uv
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="uv"></param>
        /// <param name="browsePageAvg"></param>
        /// <returns></returns>
        public static int GetBrowsePageAvg(long pv, long uv, int browsePageAvg)
        {
            //因为渠道表（赤兔）入库的时候是计算了入库的，这里排除掉已计算过的值
            if (browsePageAvg > 0)
                return browsePageAvg;
            if (uv <= 0)
                return 0;
            var avg = Math.Round(pv * 1.0 / uv, 0);
            return Convert.ToInt32(avg);
        }

        public static int GetAvg(int num, int total)
        {
            if (num == 0 || total == 0)
            {
                return 0;
            }
            if (num < 0)
                return -1;
            if (total < 0)
                return -1;
            var avg = Math.Round(num * 1.0 / total, 0);
            return Convert.ToInt32(avg);
        }

        public static int GetAvg(double num, int total)
        {
            if (num == 0 || total == 0)
            {
                return 0;
            }
            if (num < 0)
                return -1;
            if (total < 0)
                return -1;
            var avg = Math.Round(num * 1.0 / total, 0);
            return Convert.ToInt32(avg);
        }

        public static decimal GetJumpProportionAvg(decimal num, int total)
        {
            if (num == 0 || total == 0)
            {
                return 0;
            }
            if (num < 0)
                return -1;
            if (total < 0)
                return -1;
            var avg = Math.Round(num * 1.0m / total, 3);
            return avg;
        }

        /// <summary>
        /// 将秒转换成日期格式
        /// </summary>
        /// <param name="onLineAvgTime">s 秒</param>
        /// <returns></returns>
        public static string GetOnLineAvgTimeFormt(double onLineAvgTime)
        {
            if (onLineAvgTime <= 0)
                return $"00:00:00";

            var initDate = new DateTime(1970, 01, 01, 00, 00, 00);

            var ts = (initDate.AddSeconds(onLineAvgTime) - initDate);

            return $@"{GetDateFormat(ts.Hours)}:{GetDateFormat(ts.Minutes)}:{GetDateFormat(ts.Seconds)}";
        }

        private static string GetDateFormat(int num)
        {
            if (num <= 0)
            {
                return $"00";
            }
            else if (num < 10)
            {
                return $"0{num}";
            }
            else
            {
                return num.ToString();
            }
        }

        public static Tuple<string, DateTime> GetDistributeUser(Entities.Distribute.MaterielInfo entity, int distributeType)
        {
            if (entity == null)
                return new Tuple<string, DateTime>(string.Empty, DateTime.MinValue);
            if (distributeType == (int)DistributeTypeEnum.QuanWangYu)
            {
                return new Tuple<string, DateTime>(entity.DistributeUserQwy, entity.DistributeDateQWY);
            }
            else if (distributeType == (int)DistributeTypeEnum.QingNiaoAgent)
            {
                entity.DistributeUserAgent = DistributeTypeEnum.QingNiaoAgent.GetEnumDesc();
                return new Tuple<string, DateTime>(entity.DistributeUserAgent, entity.DistributeDateAgent);
            }
            return new Tuple<string, DateTime>("未知", DateTime.MinValue);
        }

        public static string GetIpInfo(string ipList, LableTypeEnum lableType)
        {
            var list = ipList.Split('|');
            var dtoList = new List<IpDto>();
            for (int i = 0; i < list.Length; i++)
            {
                var item = list[i];
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                var ip = item.Split(',');
                var key = CurrentOperateBase.GetAppContent(ip, 0);
                if (!string.IsNullOrWhiteSpace(key))
                {
                    dtoList.Add(new IpDto()
                    {
                        Type = key.ToInt(),
                        IpName = CurrentOperateBase.GetAppContent(ip, 1)
                    });
                }
            }
            var lastList = dtoList.Where(s => s.Type.Equals((int)lableType)).Select(s => s.IpName).ToList();
            return string.Join(",", lastList);
        }

        public static Tuple<int, int, int> GetContentType(string clickName, string clickSite, string clickVal)
        {
            /*
                xyh5_chitu_wlff_middle:0:2132
                xyh5_chitu_wlff_bottom:xunjia（现在直接定义dicId）:2132
                这里面，“xyh5_chitu_wlff_middle”、“xyh5_chitu_wlff_bottom”就是点击名称
                "0"、“xunjia”就是点击位置
                2132就是点击扩展值
            */
            var clickContentTypeKey = "xyh5_chitu_wlff_middle";
            var clickArticleTypeKey = "xyh5_chitu_wlff_bottom";
            int contentType = 0;//内容类型（头，腰，尾）
            int articleType = 0;//文章类型（图文，商务专题）
            int articleId = clickVal.ToInt(-1);//文章id
            if (clickName.Equals(clickContentTypeKey, StringComparison.OrdinalIgnoreCase))
            {
                //是腰部文章类型点击,clickVal 是文章id
                articleType = (int)HeadContentTypeEnum.图文;
                //contentType = (int)MaterielConentTypeEnum.Head;
                contentType = (int)MaterielConentTypeEnum.Body;
            }
            else if (clickName.Equals(clickArticleTypeKey, StringComparison.OrdinalIgnoreCase))
            {
                //腿部点击类型（询价，问答入口等等）
                articleType = clickSite.ToInt();//dicId
                //contentType = (int)MaterielConentTypeEnum.Body;
                contentType = (int)MaterielConentTypeEnum.Foot;
            }
            else
            {
                //头部
                articleType = (int)HeadContentTypeEnum.图文;
                //contentType = (int)MaterielConentTypeEnum.Foot;
                contentType = (int)MaterielConentTypeEnum.Head;
            }

            return new Tuple<int, int, int>(contentType, articleType, articleId);
        }
    }

    public class IpDto
    {
        public int Id { get; set; }
        public string IpName { get; set; }
        public int Type { get; set; }
    }
}