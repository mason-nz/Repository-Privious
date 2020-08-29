using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Enum;

namespace XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity
{
    public class ConfigEntity
    {
        public ConfigEntity()
        {
        }

        public int CreateUserId { get; set; }

        //public RoleEnum RoleTypeEnum { get; set; }
        public OperateType CureOperateType { get; set; }

        //public UserTypeEnum UserType { get; set; }//企业，个人
        //public SourceEnum SourceTypeEnum { get; set; }//自助、自营
        public QueryTypeEnum QueryTypeEnum { get; set; }
    }
}