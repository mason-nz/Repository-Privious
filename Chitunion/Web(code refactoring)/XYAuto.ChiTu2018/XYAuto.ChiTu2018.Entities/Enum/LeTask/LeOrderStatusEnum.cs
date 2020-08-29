using System.ComponentModel;

namespace XYAuto.ChiTu2018.Entities.Enum.LeTask
{
	public enum LeOrderStatusEnum
	{
		[Description("无")]
		None = -2,
		[Description("进行中")]
		Ing = 193001,

		[Description("已结束")]
		Finished = 193002,
	}					
}
