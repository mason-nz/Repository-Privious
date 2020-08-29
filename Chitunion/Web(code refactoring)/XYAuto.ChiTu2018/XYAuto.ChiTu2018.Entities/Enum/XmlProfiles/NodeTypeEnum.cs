/********************************
* 项目名称 ：XYAuto.ChiTu2018.Entities.Enum.XmlProfiles
* 类 名 称 ：NodeTypeEnum
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 14:52:35
********************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Enum.XmlProfiles
{
    public enum NodeTypeEnum
    {
        [Description("WelcomeSettings")]
        pz_hytc = 0,
        [Description("DaySignSettings")]
        pz_qdtc = 1,
        [Description("WithdrawSettings")]
        pz_tx = 2,
        [Description("FlauntSettings")]
        pz_xytc = 3,
        [Description("RestsSettings")]
        pz_qt = 4
    }
}
