using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto
{
    public class RequestDeliveryShoppingDto
    {
        public string OrderID { get; set; } = string.Empty;
        public List<RequestDeliveryShoppingIDDto> IDs { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = "";            
            if (IDs == null || IDs.Count == 0)
            {
                sb.Append("媒体ID、广告位ID对象数组不能为空!");
            }
            else
            {
                if (IDs.Count > 50)
                {
                    sb.Append("媒体ID广告位ID对象数量不能超过50!");
                }
                else
                {
                    foreach (RequestDeliveryShoppingIDDto item in IDs)
                    {
                        if (item.MediaID == 0)
                            sb.Append("媒体ID不能为0!");

                        if (!Enum.IsDefined(typeof(Entities.EnumMediaType), item.MediaType))
                            sb.Append("媒体类型不存在!");

                        if (item.PublishDetailID == 0)
                            sb.Append("广告位ID不能为0!");

                        if (item.CartID == 0)
                            sb.Append("购物车ID不能为0!");

                    }
                }
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
    public class RequestDeliveryShoppingIDDto
    {
        public int CartID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public int PublishDetailID { get; set; } = -2;
        public int SaleAreaID { get; set; } = -2;
        public int ADLaunchDays { get; set; } = -2;
        public byte IsSelected { get; set; } = 0;
    }
}
