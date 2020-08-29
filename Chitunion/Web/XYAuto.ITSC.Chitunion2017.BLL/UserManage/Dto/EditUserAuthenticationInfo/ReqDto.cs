using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserAuthenticationInfo
{
    public class ReqDto
    {
        public int UserID { get; set; } = -2;
        public int Type { get; set; } = -2;
        public string TrueName { get; set; } = string.Empty;
        public string BLicenceURL { get; set; } = string.Empty;
        public string IdentityNo { get; set; } = string.Empty;
        public string IDCardFrontURL { get; set; } = string.Empty;
        public string IDCardBackURL { get; set; } = string.Empty;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            Entities.UserManage.UserInfoAll userInfo = null;
            if (UserID <= 0)
                sb.Append($"参数UserID：{UserID}错误!");
            else
            {
                userInfo = BLL.UserManage.UserManage.Instance.GetModel(UserID);
                if (userInfo == null)
                    sb.Append($"参数UserID：{UserID}不存在!");
                else
                {
                    if (userInfo.UDStatus == (int)Entities.UserManage.UserDetailStatus.已认证)
                    {
                        sb.Append($"认证通过的信息不可再次进行编辑!");
                        return false;
                    }
                }
            }

            if (!Enum.IsDefined(typeof(Entities.UserManage.Type), Type))
                sb.Append($"参数Type：{Type}不存在!");

            if ((int)Entities.UserManage.Type.企业 == Type)
            {
                if (string.IsNullOrEmpty(TrueName))
                    sb.Append($"公司名称为必填项!");

                if (string.IsNullOrEmpty(BLicenceURL))
                    sb.Append($"公司营业执照为必填项!");
            }
            else if ((int)Entities.UserManage.Type.个人 == Type)
            {
                if (string.IsNullOrEmpty(TrueName))
                    sb.Append($"真实姓名为必填项!");

                if (string.IsNullOrEmpty(IdentityNo))
                    sb.Append($"身份证号为必填项!");
                else
                {
                    if (Dal.UserManage.UserDetailInfo.Instance.IsExistsIdentityNo(UserID, userInfo.Category, IdentityNo))
                        sb.Append($"该身份证号已被他人使用，请重新输入！");
                }

                if (string.IsNullOrEmpty(IDCardFrontURL))
                    sb.Append($"身份证号图片URL为必填项!");
            }

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
