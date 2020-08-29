using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    /// <summary>
    /// 媒体状态操作 2017-04-24 张立彬
    /// </summary>
    public class MediaStatusOperate
    {
        public static readonly MediaStatusOperate Instance = new MediaStatusOperate();
        /// <summary>
        /// 媒体删除 2017-04-24 张立彬
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <returns></returns>
        public string ToDeleteMedia(int MediaType, int MediaID)
        {
            string success = VerificationUpdate(MediaType);
            if (success != "")
            {
                return success;
            }
            int result = 0;
            int userId = Common.UserInfo.GetLoginUserID();
            string LogContent = "";
            LogContent = "媒体：" + (EnumMediaType)MediaType + " ID:" + MediaID + " 操作：删除";
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            string MediaTableName = "Media_Weixin";
            int WxStatus = (int)PublishBasicStatusEnum.上架;
            switch (MediaType)
            {
                case (int)EnumMediaType.WeChat:
                    WxStatus = (int)PublishBasicStatusEnum.上架;
                    MediaTableName = "Media_Weixin";
                    break;
                case (int)EnumMediaType.APP:
                    WxStatus = (int)AppPublishStatus.已上架;
                    MediaTableName = "Media_PCAPP";
                    break;
            }
            if (Dal.Media.MediaStatusOperate.Instance.SelectPulishCount(MediaType, MediaID, WxStatus) > 0)
            {
                return "媒体包含已上架的广告,无法删除";
            }
            if (roleIdList.Contains("SYS001RL00004") || roleIdList.Contains("SYS001RL00001"))
            {
                result = Dal.Media.MediaStatusOperate.Instance.UpdateSuperStatus(MediaType, MediaID);
                if (result > 0)
                {
                    Dal.Media.MediaStatusOperate.Instance.DeletePublishByBaseMID(MediaType, MediaID, WxStatus);
                    Dal.Media.MediaCase.Instance.DeleteCaseByBaseMidAndMtype(MediaID, MediaType);
                }

            }
            else
            {
                result = Dal.Media.MediaStatusOperate.Instance.UpdateAEorMediaRoleStatus(MediaTableName, MediaID, userId, roleIdList);
                if (result > 0)
                {
                    Dal.Media.MediaStatusOperate.Instance.DeletePublishByMediaId(MediaType, MediaID, WxStatus);
                    Dal.Media.MediaCase.Instance.DeleteMediaCaseByMidAndMtype(MediaID, MediaType);
                }
            }
            if (result <= 0)
            {
                return "操作失败，请重试";
            }

            Common.LogInfo.Instance.InsertLog(Common.LogInfo.LogModuleType.媒体管理, Common.LogInfo.ActionType.Modify, LogContent);
            return "";
        }
        private string VerificationUpdate(int MediaType)
        {
            if (!Enum.IsDefined(typeof(EnumMediaType), MediaType))
            {
                return "媒体类型错误";
            }
            return "";
        }
        /// <summary>
        /// 媒体的审核 2017-04-25 张立彬
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="RejectMsg">驳回原因</param>
        /// <param name="Status">43001待审核 43002 审核通过 43003 驳回</param>
        /// <returns></returns>
        public string ToExamineMedia(int MediaID, int MediaType, string RejectMsg, int Status, out int NextMediaID)
        {
            NextMediaID = 0;
            string IsSuccess = VerificationExamine(MediaType, Status);
            if (IsSuccess != "")
            {
                return IsSuccess;
            }
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            if (!(roleIdList.Contains("SYS001RL00004") || roleIdList.Contains("SYS001RL00001")))
            {
                IsSuccess = "您没有审核媒体的权限";
            }
            if (IsSuccess != "")
            {
                return IsSuccess;
            }

            string LogContent = "媒体：" + (EnumMediaType)MediaType + " ID:" + MediaID + " 操作：" + Util.GetEnumDescription((MediaAuditStatusEnum)Status);
            int userId = Common.UserInfo.GetLoginUserID();
            int result = 0;
            string mediaTableName = "Media_Weixin";
            switch (MediaType)
            {
                case (int)EnumMediaType.WeChat:
                    result = Dal.Media.MediaStatusOperate.Instance.ToExamineWxMedia(MediaID, MediaType, RejectMsg == null ? "" : RejectMsg, Status, userId);
                    mediaTableName = "Media_Weixin";
                    break;
                case (int)EnumMediaType.APP:
                    result = Dal.Media.MediaStatusOperate.Instance.ToExamineAppMedia(MediaID, MediaType, RejectMsg == null ? "" : RejectMsg, Status, userId);
                    mediaTableName = "Media_PCAPP";
                    break;
            }
            NextMediaID = Dal.Media.MediaStatusOperate.Instance.SelectNextMediaID((int)MediaAuditStatusEnum.PendingAudit, mediaTableName);
            Common.LogInfo.Instance.InsertLog(Common.LogInfo.LogModuleType.媒体管理, Common.LogInfo.ActionType.Modify, LogContent);
            return "";
        }
        private string VerificationExamine(int MediaType, int Status)
        {

            if (!Enum.IsDefined(typeof(EnumMediaType), MediaType))
            {
                return "媒体类型错误";
            }
            if (!Enum.IsDefined(typeof(MediaAuditStatusEnum), Status))
            {
                return "操作类型错误";
            }
            if (Status == (int)MediaAuditStatusEnum.PendingAudit)
            {
                return "操作类型错误";
            }
            Common.UserRole ur = Common.UserInfo.GetUserRole();
            if (!(ur.IsAdministrator || ur.IsYY))
            {
                return "无审核媒体权限";
            }
            return "";
        }

    }
}
