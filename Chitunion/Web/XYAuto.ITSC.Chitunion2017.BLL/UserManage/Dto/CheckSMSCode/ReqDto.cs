using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto.CheckSMSCode
{
    public class ReqDto
    {
        public string Mobile { get; set; } = string.Empty;
        public string ValidateCode { get; set; } = string.Empty;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (string.IsNullOrEmpty(Mobile))
                sb.Append($"请填写手机号码!");

            if (string.IsNullOrEmpty(ValidateCode))
                sb.Append($"请填写验证码!");
            else if (BLL.Util.GetMobileCheckCodeByCache(Mobile) != ValidateCode)
                sb.Append($"验证码错误!");

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
