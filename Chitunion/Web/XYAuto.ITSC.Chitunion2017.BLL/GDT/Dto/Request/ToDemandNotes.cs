/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 15:52:10
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;

namespace XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request
{
    public class ToNotesPostBase
    {
        /// <summary>
        /// 机构id
        /// </summary>
        [Necessary(MtName = "机构OrganizeId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OrganizeId { get; set; }

        /// <summary>
        /// 智慧云需求单号(接口参数OrganizeAdsID)
        /// </summary>
        [Necessary(MtName = "智慧云需求单号DemandBillNo", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int DemandBillNo { get; set; }
    }

    public class ToDemandNotes : ToNotesPostBase
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        public DemandAuditStatusEnum AuditStatus { get; set; }

        /// <summary>
        /// 驳回原因
        /// </summary>
        public string Reject { get; set; } = "无";
    }
}