/********************************************************
*创建人：lixiong
*创建时间：2017/6/8 14:40:18
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestTemplateInfoDto : CreatePublishQueryBase
    {
        public RequestTemplateInfoDto()
        {
            this.AdTempId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.AdBaseTempId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.BaseMediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.MediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int AdTempId { get; set; }
        public int AdBaseTempId { get; set; }
        public string AdTempName { get; set; }
        public int BaseMediaId { get; set; }
        public int MediaId { get; set; }
        public string AdTempIdList { get; set; }

        public int OperateType { get; set; }
        public int CreateUserId { get; set; }
    }
}