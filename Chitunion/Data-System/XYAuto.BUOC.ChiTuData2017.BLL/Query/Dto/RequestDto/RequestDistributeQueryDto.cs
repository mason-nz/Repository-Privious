/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 15:57:05
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto
{
    public class RequestDistributeQueryDto : CreatePublishQueryBase
    {
        [Necessary(MtName = "DistributeType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int DistributeType { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        //[Necessary(MtName = "DataType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public DistributeDataTypeEnum DataType { get; set; }

        public int DistributeId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int MaterielId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string StartDate { get; set; } = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        public string EndDate { get; set; } = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

        //public int Source { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string AssembleUser { get; set; }//组装操作人

        //public int AssembleUserType { get; set; }
        public string DistributeUser { get; set; }//分发操作人

        //public int DistributeUserType { get; set; }
        public string MaterielName { get; set; }//标题/URL模糊搜索

        public int CarSerialId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int BrandId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string CarSerialName { get; set; }//车型模糊搜索
        public int ChannelId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int ChildIp { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int Ip { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public bool IsGetResult { get; set; } = true;

        /* 导出报表冗余字段 */

        public string ExpChannelName { get; set; }
        public string ExpCarSerialName { get; set; }
        public string ExpIpName { get; set; }
        public string ExpChildIpName { get; set; }
    }
}