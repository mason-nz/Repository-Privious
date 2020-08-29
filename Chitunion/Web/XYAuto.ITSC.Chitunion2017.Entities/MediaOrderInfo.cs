using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class MediaOrderInfo
    {
        public int RecID { get; set; } = Constant.INT_INVALID_VALUE;
        public string OrderID { get; set; } = Constant.STRING_EMPTY_VALUE;
        public int MediaType { get; set; } = Constant.INT_INVALID_VALUE;
        public int MediaID { get; set; } = Constant.INT_INVALID_VALUE;
        public string Note { get; set; } = Constant.STRING_EMPTY_VALUE;
        public string UploadFileURL { get; set; } = Constant.STRING_EMPTY_VALUE;
        public DateTime CreateTime { get; set; } = Constant.DATE_INVALID_VALUE;
        public int CreateUserID { get; set; } = Constant.INT_INVALID_VALUE;

    }
}
