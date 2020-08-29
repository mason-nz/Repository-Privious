/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 10:17:45
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Request
{
    public class DistributeExportDto : RequestDistributeQueryDto
    {
        /// <summary>
        /// 操作类型：1：分发列表导出 2：分发明细导出 3: 物料详情明细导出
        /// </summary>
        public ExportBusinessType BusinessType { get; set; } = ExportBusinessType.None;

        //public RequestDistributeQueryDto Distribute { get; set; }

        //public DistributeDetailsDto DistributeDetails { get; set; }

        //public MaterielDetailsDto MaterielDetails { get; set; }

        /// <summary>
        /// 导出类型（赤兔类型的才有渠道数据，其他类型没有则传2）：1：渠道数据 2：物料日数据
        /// </summary>
        public ExportTypeEnum ExportType { get; set; }
    }

    /// <summary>
    /// 2：分发明细导出
    /// </summary>
    public class DistributeDetailsDto : DistributeDetailsBaseDto
    {
        /// <summary>
        /// 导出类型（赤兔类型的才有渠道数据，其他类型没有则传2）：1：渠道数据 2：物料日数据
        /// </summary>
        public int ExportType { get; set; }
    }

    /// <summary>
    /// 3: 物料详情明细导出
    /// </summary>
    public class MaterielDetailsDto : DistributeDetailsBaseDto
    {
    }

    public class DistributeDetailsBaseDto
    {
        public int MaterielId { get; set; }
        public int Source { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    /// <summary>
    /// 操作类型：1：分发列表导出 2：分发明细导出 3: 物料详情明细导出
    /// </summary>
    public enum ExportBusinessType
    {
        None = -2,

        [Description("分发列表导出")]
        Distribute = 1,

        [Description("分发明细导出")]
        DistributeDetails = 2,

        [Description("物料详情明细导出")]
        MaterielDetails = 3,
    }

    public enum ExportTypeEnum
    {
        [Description("渠道数据")]
        Channel = 1,

        [Description("物料日数据")]
        Daily = 2
    }
}