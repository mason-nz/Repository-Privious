using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto
{
    public class ReqQueryUserBasicInfoDto
    {
        public int UserID { get; set; } = -2;
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
            
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
