using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Response
{
    public class RespBaseChituDto<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回状态值的说明
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回Json对象
        /// </summary>
        public T Result { get; set; }

    }
}
