/********************************************************
*创建人：lixiong
*创建时间：2017/6/6 13:59:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query.AdTemplate
{
    public class AdTemplateQuery<T> : QueryPageBase<T>
    {
        public AdTemplateQuery()
        {
            this.TemplateId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.BaseMediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.BaseAdId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.AuditStatus = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.FilterTemplateId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.IsPublic = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.GroupType = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.IsGetTrueName = false;
        }

        public int TemplateId { get; set; }
        public int BaseMediaId { get; set; }
        public int CreateUserId { get; set; }
        public int BaseAdId { get; set; }
        public string TemplateName { get; set; }
        public int AuditStatus { get; set; }
        public string AuditStatusStr { get; set; }
        public string AdTempIdList { get; set; }
        public int FilterTemplateId { get; set; }
        public int IsPublic { get; set; }
        public bool IsGetTrueName { get; set; }
        public int GroupType { get; set; }
    }
}