/********************************************************
*创建人：lixiong
*创建时间：2017/6/3 11:57:14
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.AdTemplate
{
    public class AppAdTemplate
    {
        public AppAdTemplate()
        {
            this.AuditStatus = (int)AppTemplateEnum.待审核;
        }

        public int RecID { get; set; }
        public string TrueName { get; set; }

        //基表app媒体Id
        public int BaseMediaID { get; set; }

        public string BaseMediaName { get; set; }
        public string BaseMediaLogoUrl { get; set; }

        //审核通过主表id
        public int BaseAdID { get; set; }

        public string BaseAdName { get; set; }

        //广告名称(广告模板名称)
        public string AdTemplateName { get; set; }

        //刊例原件
        public string OriginalFile { get; set; }

        //广告形式
        public int AdForm { get; set; }

        public string AdFormName { get; set; }

        //轮播数
        public int CarouselCount { get; set; }

        //售卖平台(与的方式存储)
        public int SellingPlatform { get; set; }

        //售卖方式
        public int SellingMode { get; set; }

        //广告图例(最多选3张，参考存一个字段)
        public string AdLegendURL { get; set; }

        //广告展示逻辑
        public string AdDisplay { get; set; }

        //广告说明、描述
        public string AdDescription { get; set; }

        //广告备注
        public string Remarks { get; set; }

        //起投天数/次
        public int AdDisplayLength { get; set; }

        //审核状态（待审核、驳回、通过）
        public int AuditStatus { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        public DateTime CreateTime { get; set; }

        //数据状态
        public int Status { get; set; }

        public string AdStyleStr { get; set; }
        public string AdSaleAreaGroupStr { get; set; }
        public string CreateUserName { get; set; }
        public bool IsAuthAE { get; set; }
    }
}