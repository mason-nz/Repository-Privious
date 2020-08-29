using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserPasswordInfo
{
    public class ReqDto
    {
        public int UserID { get; set; } = -2;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (string.IsNullOrEmpty(OldPassword))
                sb.Append($"请填写旧密码!");

            if (string.IsNullOrEmpty(NewPassword))
                sb.Append($"请填写新密码!");

            if (string.IsNullOrEmpty(ConfirmPassword))
                sb.Append($"请填写确认密码!");

            if (NewPassword != ConfirmPassword)
                sb.Append($"两次输入的密码不一致!");

            if (OldPassword.Length > 20 || NewPassword.Length > 20 || ConfirmPassword.Length > 20)
                sb.Append($"密码最多20个字符!");

            if (UserID <= 0)
                sb.Append($"参数UserID：{UserID}错误!");
            else
            {
                var userInfo = BLL.UserManage.UserManage.Instance.GetModel(UserID);
                if (userInfo == null)
                    sb.Append($"参数UserID：{UserID}不存在!");
                else if (!string.IsNullOrEmpty(OldPassword))
                {
                    OldPassword = XYAuto.Utils.Security.DESEncryptor.MD5Hash(OldPassword + userInfo.Category.ToString() + Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey"), System.Text.Encoding.UTF8);
                    if (OldPassword != userInfo.Pwd)
                        sb.Append($"旧密码输入错误!");
                }
            }
            
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
