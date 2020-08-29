using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish
{
    public class RoleInfoMapping
    {
        public const string SupperAdmin = "SYS001RL00001";
        public const string Advertiser = "SYS001RL00002";
        public const string MediaOwner = "SYS001RL00003";//SYS001RL00003
        public const string YunYingOperate = "SYS001RL00004";
        public const string AE = "SYS001RL00005";
        public const string Plan = "SYS001RL00006";
        public const string GroupLeader = "SYS001RL00012";//清洗主管
        public const string ArticleCleaner = "SYS001RL00013";//文章清洗员

        public static string GetAllowRoleInfo()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}", SupperAdmin, Advertiser, MediaOwner, YunYingOperate, AE, Plan);
        }

        /// <summary>
        /// 获取当前登录人是属于 自营、自助
        /// </summary>
        /// <returns></returns>
        public static SourceEnum GetUserSource()
        {
            var roleIds = Common.UserInfo.GetLoginUserRoleIDs();
            if (roleIds.Split(',').Any(role => role.Equals(Advertiser, StringComparison.OrdinalIgnoreCase)
                                               || role.Equals(MediaOwner, StringComparison.OrdinalIgnoreCase)))
            {
                //媒体主,广告主
                return SourceEnum.自助;
            }

            return SourceEnum.自营;
        }

        public static string GetUserRoleId(int userId)
        {
            var dt = Common.Dal.UserInfo.Instance.GetLoginUserInfo(userId, Common.UserInfo.SYSID);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["roleIDs"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 验证当前用户与指定的角色是否匹配
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleCodesList"></param>
        /// <returns></returns>
        private static bool VerifyOfUserRole(int userId, List<string> roleCodesList)
        {
            var isVerify = false;
            var roleIds = userId == 0 ? Common.UserInfo.GetLoginUserRoleIDs()
              : GetUserRoleId(userId);
            if (string.IsNullOrWhiteSpace(roleIds))
                return false;

            roleCodesList.ForEach(s =>
            {
                if (roleIds.Contains(s))
                {
                    isVerify = true;
                    return;
                }
            });

            return isVerify;
        }

        /// <summary>
        /// 为了验证编辑媒体信息，是否存在重复的帐号或者名称，获取到媒体创建人的角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static List<string> GetVerifyMediaRoleIdList(RoleEnum role)
        {
            var list = new List<string>();
            switch (role)
            {
                case RoleEnum.AE:
                case RoleEnum.SupperAdmin:
                case RoleEnum.YunYingOperate:
                    list.Add(SupperAdmin);
                    list.Add(AE);
                    list.Add(YunYingOperate);
                    return list;

                case RoleEnum.MediaOwner:
                    list.Add(MediaOwner);
                    return list;
            }
            return list;
        }

        /// <summary>
        /// 返回角色枚举
        /// </summary>
        /// <returns></returns>
        public static RoleEnum GetUserRole(int userId = 0)
        {
            var roleIds = userId == 0 ? Common.UserInfo.GetLoginUserRoleIDs()
                : GetUserRoleId(userId);
            if (string.IsNullOrWhiteSpace(roleIds))
                return RoleEnum.None;
            var spRoleIds = roleIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            switch (spRoleIds[0].Trim())
            {
                case SupperAdmin:
                    return RoleEnum.SupperAdmin;

                case Advertiser:
                    return RoleEnum.Advertiser;

                case MediaOwner:
                    return RoleEnum.MediaOwner;

                case YunYingOperate:
                    return RoleEnum.YunYingOperate;

                case AE:
                    return RoleEnum.AE;

                case GroupLeader:
                    return RoleEnum.GroupLeader;

                case ArticleCleaner:
                    return RoleEnum.ArticleCleaner;

                case Plan:
                    return RoleEnum.Plan;

                default:
                    return RoleEnum.None;
            }
        }

        /// <summary>
        /// 后台查询-需注意角色,0:无权限访问，-1:超级管理员，大于0:正常用户AE，媒体主
        /// </summary>
        /// <returns></returns>
        public static int GetBackQueryUserIdByRole()
        {
            var roleMeum = GetUserRole();
            if (roleMeum == RoleEnum.YunYingOperate || roleMeum == RoleEnum.SupperAdmin)
            {
                return -1;
            }
            if (roleMeum == RoleEnum.AE || roleMeum == RoleEnum.MediaOwner)//媒体主的刊例列表与AE的刊例列表相同，都是只能看到自己添加的
            {
                return GetCurrentUserId(); //获取用户id
            }
            if (roleMeum == RoleEnum.Advertiser)
            {
                return 0;
            }
            return 0;
        }

        public static int GetCurrentUserId()
        {
            var userId = -1;
            try
            {
                userId = Chitunion2017.Common.UserInfo.GetLoginUserID(); //获取用户id
            }
            catch
            {
                throw new Exception("暂无用户登录信息");
            }
            return userId;
        }
    }

    [Description("此枚举只能用于识别，不能用于比较")]
    public enum RoleEnum
    {
        None,
        SupperAdmin,
        Advertiser,
        MediaOwner,
        YunYingOperate,
        AE,
        Plan,
        GroupLeader,
        ArticleCleaner
    }

    public enum SourceEnum
    {
        自营 = 3001,
        自助 = 3002
    }

    public enum SaleMode
    {
        CPD = 11001,
        CPM = 11002
    }

    public enum SysSysPlatform
    {
        Android = 12001,
        IOS = 12002
    }

    public enum ThrMonitor
    {
        曝光监测 = 13001,
        点击监测
    }

    public enum AdPositionMapping
    {
        单图文 = 6001,
        多图文头条 = 6002,
        多图文第二条 = 6003,
        多图文3N条 = 6004,
    }

    public enum AdTypeMapping
    {
        硬广 = 7001,
        软广 = 7002
    }

    public enum AdFormality3
    {
        发布 = 8001,
        原创发布 = 8002
    }

    public enum AdFormality4
    {
        直发 = 9001,
        原创发布 = 9002,
        转发 = 9003
    }

    public enum AdFormality5
    {
        活动现场直播 = 10001,
        直播广告植入 = 10002
    }

    public enum TerminalType
    {
        App = 28001,
        WapPc = 28002
    }
}