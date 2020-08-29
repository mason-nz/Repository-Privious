/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.Wechat.Dto
* 类 名 称 ：RespLeDaySignDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/11 14:58:21
********************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.Wechat.Dto
{
    public class RespLeDaySignDto
    {
      
        public decimal Amount { get; set; }
        [JsonIgnore]

        public string Message { get; set; }

        public bool isLuckDraw { get; set; }

        public int AlreadyOrderCount { get; set; }

        public int SignOrderCount { get; set; }

        public object List { get; set; }
    }
}
