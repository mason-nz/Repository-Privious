/********************************************************
*创建人：lixiong
*创建时间：2017/6/3 11:57:55
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.AdTemplate
{
    public class AppAdTemplateStyle
    {
        public int RecID { get; set; }

        //基表媒体id
        public int BaseMediaID { get; set; }

        //广告模板id
        public int AdTemplateID { get; set; }

        //广告样式
        public string AdStyle { get; set; }

        public bool IsPublic { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class AppAdTemplateStyleTable
    {
        public AppAdTemplateStyleTable()
        {
            this.CreateTime = DateTime.Now;
        }

        public int RecID { get; set; }

        //基表媒体id
        public int BaseMediaID { get; set; }

        //广告模板id
        public int AdTemplateID { get; set; }

        //广告样式
        public string AdStyle { get; set; }

        public int IsPublic { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        public DateTime CreateTime { get; set; }
    }
}