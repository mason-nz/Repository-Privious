/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 15:34:10
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Materiel.Dto
{
    public class RequestDistributeDto
    {
        /// <summary>
        /// 操作类型：1：分配 2：收回
        /// </summary>
        [Necessary(MtName = "OperateType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OperateType { get; set; }

        [Necessary(MtName = "GroupIds")]
        public string GroupIds { get; set; }

        /// <summary>
        /// OperateType=1 分配操作下，必填
        /// </summary>

        //[Necessary(MtName = "UserId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入媒体Id")]
        public int UserId { get; set; }
    }
}