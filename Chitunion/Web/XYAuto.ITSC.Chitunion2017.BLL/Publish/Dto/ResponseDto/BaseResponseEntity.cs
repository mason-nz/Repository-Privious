using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto
{
    public class BaseResponseEntity<T> : BaseResponseEntity
    {
        public List<T> List { get; set; }
    }

    public class BaseResponseList<T> : BaseResponseEntity
    {
        public T List { get; set; }
    }

    public class BaseResponseEntity
    {
        public int TotleCount { get; set; }
        public dynamic Extend { get; set; }
    }
}
