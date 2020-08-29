using System.ComponentModel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Enum
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