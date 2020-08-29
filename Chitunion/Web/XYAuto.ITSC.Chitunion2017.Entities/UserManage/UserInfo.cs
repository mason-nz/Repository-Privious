using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.UserManage
{
    public enum Type
    {
        企业= 1001,
        个人= 1002
    }
    public enum Category
    {
        广告主=29001,
        媒体主,
        内部员工
    }
    public enum Source
    {
        自营= 3001,
        自助,
        库存,
        智慧云
    }
    public class UserInfo
    {
        public int UserID { get; set; } = -2;
        public string UserName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Pwd { get; set; } = string.Empty;
        public int Type { get; set; } = -2;
        public int Category { get; set; } = -2;
        public int Source { get; set; } = -2;
        public bool IsAuthMTZ { get; set; } = false;
        public int AuthAEUserID { get; set; } = -2;
        public bool IsAuthAE { get; set; } = false;
        public int SysUserID { get; set; } = -2;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; } = -2;
        public DateTime LastUpdateTime { get; set; } = new DateTime(1900, 1, 1);
        public int LastUpdateUserID { get; set; } = -2;

        public int LockState { get; set; }
        public int SleepState { get; set; }
        public int LockType { get; set; }
        public int SleepStatus { get; set; }

    }
}
