/********************************************************
*创建人：lixiong
*创建时间：2017/6/29 12:05:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.AdTemplate
{
    public static class AppAdTemplateExtend
    {
        /*
         --查处所有的
            --1.判断当前baseadId,或者审核状态，已通过则是公共模板
            --2.公共模板只需要IsPublic = 1
            --3.套用模板的需要公共模板+自己的IsPublic = 0
            --4.城市组过滤考虑：套用模板之后，在已有城市组加了城市，获取自己的那部分
            --用当前自己的模板的创建人去过滤
        */

        public static List<AdTempStyleDto> FitlerStyle(this List<AdTempStyleDto> source, RespAdTemplateItemDto reqItemDto)
        {
            if (source == null) return null;
            if (reqItemDto == null) return source;
            if (reqItemDto.AuditStatus == (int)Entities.Enum.AppTemplateEnum.已通过)
            {
                //查询的是已通过的IsPublic = 1
                return source.Where(s => s.IsPublic == 1).ToList();
            }
            else
            {
                //套用模板的需要公共模板+自己的IsPublic = 0
                return source.Where(s => s.IsPublic == 1 || s.CreateUserId == reqItemDto.CreateUserID).ToList();
            }
        }

        public static List<AdSaleAreaGroupDto> FilterSaleGroup(this List<AdSaleAreaGroupDto> source, RespAdTemplateItemDto reqItemDto)
        {
            if (source == null) return null;
            if (reqItemDto == null) return source;
            if (reqItemDto.AuditStatus == (int)Entities.Enum.AppTemplateEnum.已通过)
            {
                //查询的是已通过的IsPublic = 1

                var allCountry = source.FirstOrDefault(s => s.GroupType == (int)SaleAreaGroupTypeEnum.AllCountry);
                var other = source.FirstOrDefault(s => s.GroupType == (int)SaleAreaGroupTypeEnum.Other);

                var respListDto = new List<AdSaleAreaGroupDto>();
                foreach (var item in source)
                {
                    if (item == null) continue;
                    if (item.GroupType == (int)SaleAreaGroupTypeEnum.AllCountry ||
                        item.GroupType == (int)SaleAreaGroupTypeEnum.Other)
                    {
                        continue;
                    }
                    if (item.IsPublic == 1)
                    {
                        //公共的
                        if (item.DetailArea != null)
                        {
                            item.DetailArea = item.DetailArea.Where(s => s.IsPublic == 1).ToList();
                        }
                    }
                    respListDto.Add(item);
                }
                if (allCountry != null)
                    respListDto.Insert(0, allCountry);
                if (other != null)
                    respListDto.Add(other);
                return respListDto;
            }
            else
            {
                //城市组过滤考虑：套用模板之后，在已有城市组加了城市，获取自己的那部分
                //--用当前自己的模板的创建人去过滤
                var allCountry = source.FirstOrDefault(s => s.GroupType == (int)SaleAreaGroupTypeEnum.AllCountry);
                var other = source.FirstOrDefault(s => s.GroupType == (int)SaleAreaGroupTypeEnum.Other);

                var respListDto = new List<AdSaleAreaGroupDto>();

                foreach (var item in source)
                {
                    if (item == null) continue;
                    if (item.GroupType == (int)SaleAreaGroupTypeEnum.AllCountry ||
                        item.GroupType == (int)SaleAreaGroupTypeEnum.Other)
                    {
                        continue;
                    }
                    if (item.IsPublic == 1)
                    {
                        //公共的
                        if (item.DetailArea != null)
                        {
                            item.DetailArea = item.DetailArea.Where(s => s.IsPublic == 1
                                    || s.CreateUserId == reqItemDto.CreateUserID).ToList();
                        }
                    }
                    respListDto.Add(item);
                }
                if (allCountry != null)
                    respListDto.Insert(0, allCountry);
                if (other != null)
                    respListDto.Add(other);
                return respListDto;
            }
        }
    }
}