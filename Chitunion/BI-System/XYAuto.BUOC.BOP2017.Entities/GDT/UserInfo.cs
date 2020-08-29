/********************************************************
*创建人：lixiong
*创建时间：2017/8/16 11:48:45
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    public class UserInfo
    {
        //用户信息Commnet_RecID
        public int UserID { get; set; }

        //用户名，
        //一期，用户名=手机号码
        public string UserName { get; set; }

        //手机号
        public string Mobile { get; set; }

        //登陆用户密码
        public string Pwd { get; set; }

        //用户类型：1-企业；2-个人
        public int Type { get; set; }

        //注册来源：1-自助；2-自营；
        public int Source { get; set; }

        //是否允许广告主授权给媒体主
        public bool IsAuthMTZ { get; set; }

        //授权AE的UserID
        public int AuthAEUserID { get; set; }

        //是否授权给AE，
        //1——同意；
        //0——不同意；
        public bool IsAuthAE { get; set; }

        public int SysUserID { get; set; }

        //内部用户的员工编号
        public string EmployeeNumber { get; set; }

        //状态：
        //0——正常；
        //1——禁用；
        //-1——删除；
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public int CreateUserID { get; set; }

        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        public int LastUpdateUserID { get; set; }

        //用户分类（29001—广告主；29002—媒体主）
        public int Category { get; set; }

        //Email
        public string Email { get; set; }

        public int OrganizeId { get; set; }
    }
}