using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto
{
    public class RequestPubDetailVertifyDto
    {
        public string OrderID { get; set; } = string.Empty;
        public List<RequestMediaDto> Media { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;

            foreach (var item in Media)
            {
                if (!Enum.IsDefined(typeof(Entities.EnumMediaType), item.MediaType))
                    sb.Append($"参数媒体类型错误!{item.MediaType}\n");
            }
        
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
    public class RequestMediaDto
    {
        public int MediaType { get; set; } = -2;
        public int CartID { get; set; } = -2;
        public int PublishDetailID { get; set; } = -2;
        public int SaleAreaID { get; set; } = -2;
    }
}
