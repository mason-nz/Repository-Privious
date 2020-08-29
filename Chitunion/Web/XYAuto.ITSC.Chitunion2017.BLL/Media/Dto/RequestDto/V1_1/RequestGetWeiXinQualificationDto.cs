/********************************************************
*创建人：lixiong
*创建时间：2017/7/5 12:15:18
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestGetWeiXinQualificationDto
    {
        public int MediaId { get; set; }

        //[Necessary(MtName = "OperateType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OperateType { get; set; }//1：添加（是从授权之后调整到页面、查询是基础表信息） 2：编辑（从后台媒体列表跳转到页面、查询是副表信息）

        public int CreateUserId { get; set; }

        public bool IsInsert { get; set; }
    }
}