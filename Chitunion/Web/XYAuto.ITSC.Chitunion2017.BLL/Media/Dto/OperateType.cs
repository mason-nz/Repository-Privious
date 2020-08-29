using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto
{
    public enum OperateType
    {
        [Description("添加操作")]
        Insert = 1,

        [Description("编辑操作")]
        Edit = 2,

        [Description("测试操作")]
        Test = 0
    }
}