using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserBasicInfo
{
    public class ReqDto
    {
        public int UserID { get; set; } = -2;
        public string UserName { get; set; } = string.Empty;
        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public string Address { get; set; } = string.Empty;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (UserID <= 0)
                sb.Append($"参数UserID：{UserID}错误!");
            else
            {
                if (BLL.UserManage.UserManage.Instance.GetModel(UserID) == null)
                    sb.Append($"参数UserID：{UserID}不存在!");
            }

            if (ProvinceID > 0 && CityID == -2)
                sb.Append($"城市为必填项!");
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
