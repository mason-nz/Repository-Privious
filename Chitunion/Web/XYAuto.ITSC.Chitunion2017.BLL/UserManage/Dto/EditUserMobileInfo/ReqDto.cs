using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserMobileInfo
{
    public class ReqDto
    {
        public int UserID { get; set; } = -2;
        public string Mobile { get; set; } = string.Empty;
        public string ValidateCode { get; set; } = string.Empty;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            var userInfo = BLL.UserManage.UserManage.Instance.GetModel(UserID);
            if (string.IsNullOrEmpty(Mobile))
                sb.Append($"请填写手机号码!");

            if (string.IsNullOrEmpty(ValidateCode))
                sb.Append($"请填写验证码!");
            //else if (BLL.Util.GetMobileCheckCodeByCache(userInfo.Mobile) != ValidateCode)
            //    sb.Append($"验证码错误!");

            if (UserID <= 0)
                sb.Append($"参数UserID：{UserID}错误!");
            else
            {                
                if (userInfo == null)
                    sb.Append($"参数UserID：{UserID}不存在!");
                else
                {
                    if (Dal.UserManage.UserInfo.Instance.IsExistsMobile(UserID, userInfo.Category, Mobile))
                        sb.Append($"手机号已经注册，请更换其它手机号!");
                }
            }

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
