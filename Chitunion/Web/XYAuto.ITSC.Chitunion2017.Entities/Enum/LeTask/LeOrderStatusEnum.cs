using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask
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
