/********************************************************
*创建人：lixiong
*创建时间：2017/6/6 15:12:10
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1
{
    public class RequestMediaAppQueryDto : RequestAdQueryDto
    {
        public RequestMediaAppQueryDto()
        {
            this.MediaRelations = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.OperatingType = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int MediaRelations { get; set; }//媒体关系(50001:代理 50002:自有 50003:自营)
        public int OperatingType { get; set; }//运营者类型(1001:企业 1002:个人)
        public string AuditStatus { get; set; }//审核状态（43001：待审核 43002：已通过 43003：驳回）
        public string SubmitUserRole { get; set; }//添加人角色
    }
}