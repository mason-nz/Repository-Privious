using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter
{
    public enum EnumData
    {
        #region 封装

        [Description("封装_头部标题+饼图"), Browsable(true)]
        fz_head_pie,
        [Description("封装_头部柱状图")]
        fz_head_bar,
        [Description("封装_物料在场景上的分布")]
        fz_scenc,
        [Description("封装_物料在账号分值上的分布")]
        fz_account,
        [Description("封装_物料在头部文章分值上的分布")]
        fz_essay,
        [Description("封装_物料在物料状态上的分布")]
        fz_condition,

        #endregion

        #region 分发

        [Description("分发_头部标题+柱状图")]
        fenf_head,
        [Description("分发_物料在物料类型上的分布")]
        fenf_materia,
        [Description("分发_物料在场景上的分布")]
        fenf_scenc,
        [Description("分发_物料在账号分值上的分布")]
        fenf_account,
        [Description("分发_物料在头部文章分值上的分布")]
        fenf_essay,

        #endregion

        #region 转发

        [Description("转发_头部标题+饼图")]
        zf_head_pie,
        [Description("转发_头部柱状图")]
        zf_head_bar,
        [Description("转发_物料的转发在物料类型上的分布")]
        zf_materia,
        [Description("转发_物料的转发在场景上的分布")]
        zf_scenc,
        [Description("转发_物料的转发在账号分值上的分布")]
        zf_account,
        [Description("转发_物料的转发在头部文章分值上的分布")]
        zf_essay,
        #endregion

        #region 线索
        [Description("线索_头部标题+饼图")]
        xs_head_pie,
        [Description("线索_头部柱状图")]
        xs_head_bar,
        [Description("线索_产生的线索在物料类型上的分布")]
        xs_materia,
        [Description("线索_产生的线索在场景上的分布")]
        xs_scenc,
        [Description("线索_产生的线索在账号分值上的分布")]
        xs_account,
        [Description("线索_产生的线索在头部文章分值上的分布")]
        xs_essay
        #endregion


    }
    public enum ExcelEnum
    {
        [Description("封装日汇总数据")]
        fz,

        [Description("分发日汇总数据")]
        ff,

        [Description("转发日汇总数据")]
        zf,

        [Description("线索日汇总数据")]
        xs,
        [Description("封装明细数据")]
        fz_detail,

        [Description("分发明细数据")]
        ff_detail,

        [Description("转发明细数据")]
        zf_detail,

        [Description("线索明细数据")]
        xs_detail
    }

    public enum WorkEnum
    {
        [Description("79001")]
        初筛 = 1002,

        [Description("79002,79003")]
        清洗 = 1003,

        [Description("79004")]
        封装 = 1004

    }
}
