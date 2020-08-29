using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.BLL.QueryPage.Entity
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