using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto
{
    public class ConfigEntity
    {
        public ConfigEntity()
        {
            BusinessType = MediaType.WeiXin;
        }

        public int CreateUserId { get; set; }
        public MediaType BusinessType { get; set; }
        public RoleEnum RoleTypeEnum { get; set; }
        public OperateType CureOperateType { get; set; }
        public UserTypeEnum UserType { get; set; }//企业，个人
        public SourceEnum SourceTypeEnum { get; set; }//自助、自营

        public string IdentityNo { get; set; }//身份证号
        public XYAuto.ITSC.Chitunion2017.Common.LoginUser LoginUser { get; set; }
    }
}