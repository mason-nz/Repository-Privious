using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    /// <summary>
    /// 媒体案例 2017-04-24 张立彬
    /// </summary>
    public class MediaCase
    {
        public static readonly MediaCase Instance = new MediaCase();
        public string InsertMediaCaseInfo(XYAuto.ITSC.Chitunion2017.Entities.Media.MediaCase CaseInfo)
        {
            string IsSuccess = VerificationInsert(CaseInfo);
            if (IsSuccess != "")
            {
                return IsSuccess;
            }
            int userId = Common.UserInfo.GetLoginUserID();
            int retsult = Dal.Media.MediaCase.Instance.InsertMediaCaseInfo(CaseInfo, userId);
            if (retsult <= 0)
            {
                return "操作失败，请重试";
            }
            return "";
        }
        /// <summary>
        /// 根据媒体类型和ID,状态查询媒体案例
        /// </summary>
        /// <param name="MediaID"></param>
        /// <param name="MediaType"></param>
        /// <param name="CaseStatus"></param>
        /// <returns></returns>
        public DataTable SelectMediaCaseInfo(int MediaID, int MediaType, int CaseStatus, out string Msg)
        {
            Msg = VerificationSelect(MediaType, CaseStatus);

            string mediaTableName = "Media_Weixin";
            switch (MediaType)
            {
                case (int)EnumMediaType.WeChat:
                   
                    mediaTableName = "Media_Weixin";
                    break;
                case (int)EnumMediaType.APP:
                    mediaTableName = "Media_PCAPP";
                    break;
            }
            return Dal.Media.MediaCase.Instance.SelectMediaCaseInfo(MediaID, MediaType, CaseStatus, mediaTableName);
        }
        /// <summary>
        /// 2017-06-05 zlb
        /// 根基媒体类型和ID删除案例
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns>大于0删除成功;反之删除失败</returns>
        public int DeleteMediaCaseByMidAndMtype(int MediaID, int MediaType, out string Msg)
        {
            Msg = VerificationSelect(MediaType, 0);
            if (Msg != "")
            {
                return -1;
            }
            int result = Dal.Media.MediaCase.Instance.DeleteMediaCaseByMidAndMtype(MediaID, MediaType);
            if (result <= 0)
            {
                Msg = "删除失败";
            }
            return result;
        }

        /// <summary>
        /// 验证插入参数
        /// </summary>
        /// <param name="CaseInfo">案例信息</param>
        /// <returns></returns>
        private string VerificationInsert(XYAuto.ITSC.Chitunion2017.Entities.Media.MediaCase CaseInfo)
        {
            if (CaseInfo != null)
            {
                if (CaseInfo.CaseContent == null || CaseInfo.CaseContent.Trim() == "")
                {
                    return "请输入案列内容";
                }
                if (!Enum.IsDefined(typeof(EnumMediaType), CaseInfo.MediaType))
                {
                    return "媒体类型错误";
                }
                if (CaseInfo.CaseStatus != 0 && CaseInfo.CaseStatus != 1)
                {
                    return "参数类型错误";
                }
                return "";
            }
            return "参数类型错误";
        }
        /// <summary>
        /// 验证查询参数
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="CaseStatus">媒体案例状态0预览 1正式 </param>
        /// <returns></returns>
        private string VerificationSelect(int MediaType, int CaseStatus)
        {
            if (!Enum.IsDefined(typeof(EnumMediaType), MediaType))
            {
                return "媒体类型错误";
            }
            if (CaseStatus != 0 && CaseStatus != 1)
            {
                return "参数类型错误";
            }
            return "";
        }

    }
}
