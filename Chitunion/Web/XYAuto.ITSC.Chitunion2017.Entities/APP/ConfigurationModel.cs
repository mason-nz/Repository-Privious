using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.APP
{
    public class ConfigurationModel
    {
        public int UserID { get; set; }

        public string NodeType { get; set; }
    }

    public enum NodeTypeEnum
    {
        [Description("WelcomeSettings")]
        pz_hytc=0,
        [Description("DaySignSettings")]
        pz_qdtc=1,
        [Description("WithdrawSettings")]
        pz_tx=2,
        [Description("FlauntSettings")]
        pz_xytc=3,
        [Description("RestsSettings")]
        pz_qt=4
    }
}
