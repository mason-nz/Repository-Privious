using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto
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
        public int TotalCount { get; set; }
    }
}
