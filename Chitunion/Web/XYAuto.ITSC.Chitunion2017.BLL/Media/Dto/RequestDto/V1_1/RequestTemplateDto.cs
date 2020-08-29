/********************************************************
*创建人：lixiong
*创建时间：2017/6/6 10:17:13
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestTemplateDto
    {
        public RequestTemplateDto()
        {
            this.AuditStatus = (int)Entities.Enum.AppTemplateEnum.待审核;
        }

        public int TemplateId { get; set; }

        public int BaseAdId { get; set; }//审核通过主表id

        //[Necessary(MtName = "BaseMediaId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int BaseMediaId { get; set; }        //基表app媒体Id

        //[Necessary(MtName = "MediaId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int MediaId { get; set; }

        [Necessary(MtName = "AdTemplateName")]
        public string AdTemplateName { get; set; }//广告名称(广告模板名称)

        //[Necessary(MtName = "OriginalFile")]
        public string OriginalFile { get; set; }//刊例原件

        [Necessary(MtName = "AdForm", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdForm { get; set; }//广告形式

        public string AdFormName { get; set; }

        [Necessary(MtName = "CarouselCount", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int CarouselCount { get; set; }    //轮播数

        [Necessary(MtName = "SellingPlatform", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int SellingPlatform { get; set; }//售卖平台(与的方式存储)

        [Necessary(MtName = "SellingMode", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int SellingMode { get; set; }//售卖方式

        public string AdLegendUrl { get; set; }//广告图例(最多选3张，参考存一个字段)AE添加模板时，示例图为非必填项

        //[Necessary(MtName = "AdDisplay")]
        public string AdDisplay { get; set; }   //广告展示逻辑

        //[Necessary(MtName = "AdDescription")]
        public string AdDescription { get; set; }  //广告说明、描述

        //[Necessary(MtName = "Remarks")]
        public string Remarks { get; set; }//广告备注

        //[Necessary(MtName = "AdDisplayLength", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdDisplayLength { get; set; } //起投天数/次

        public int CreateUserId { get; set; }//创建人

        public DateTime CreateTime { get; set; }

        public int AuditStatus { get; set; }//运营添加、编辑模板是直接通过

        public bool IsModified { get; set; }//修正，修订

        public List<AdTempStyleDto> AdTempStyle { get; set; }//广告样式
        public List<AdSaleAreaGroupDto> AdSaleAreaGroup { get; set; }//城市组
    }
}