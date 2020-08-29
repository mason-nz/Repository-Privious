/********************************************************
*创建人：lixiong
*创建时间：2017/5/12 18:42:05
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class MediaQualification : CurrentOperateBase
    {
        #region Instance

        public static readonly MediaQualification Instance = new MediaQualification();

        #endregion Instance

        #region Contructor

        protected MediaQualification()
        { }

        #endregion Contructor

        public int Insert(Entities.Media.MediaQualification entity)
        {
            return Dal.Media.MediaQualification.Instance.Insert(entity);
        }

        public Entities.Media.MediaQualification GetInfo(int userId)
        {
            var info = Dal.Media.MediaQualification.Instance.GetInfo(userId);

            if (info == null)
            {
                return Dal.Media.MediaQualification.Instance.GetUserDetailInfo(userId);
            }
            return info;
        }

        /// <summary>
        /// 查询资质表信息
        /// </summary>
        /// <param name="mediaId">媒体Id</param>
        /// <param name="userId">用户id</param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public Entities.Media.MediaQualification GetEntity(int mediaId, int userId = 0, int mediaType = (int)MediaType.WeiXin)
        {
            return Dal.Media.MediaQualification.Instance.GetEntity(mediaId, userId, mediaType);
        }

        /// <summary>
        /// auth:lixiong
        /// 资质信息编辑
        /// 1.维护MediaQualification表信息
        /// 2.更新用户详情营业执照信息（和用户详情表关联用户Id、是媒体的关联人）
        /// 3.维护UploadFileInfo表信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public ReturnValue Update(Entities.Media.MediaQualification entity, MediaType mediaType)
        {
            var retValue = new ReturnValue()
            {
                HasError = true,
                ErrorCode = "60002",
                Message = string.Format("媒体信息不存在.Id={0}", entity.MediaID)
            };

            retValue = VerifyOfMediaInfo(entity.MediaID, mediaType);
            if (retValue.HasError)
                return retValue;

            var mediaInfo = GetEntity(mediaType, retValue);
            if (mediaInfo == null)
            {
                return retValue;
            }

            if (string.IsNullOrWhiteSpace(entity.EnterpriseName))
            {
                retValue.HasError = true;
                retValue.ErrorCode = "60002";
                retValue.Message = "EnterpriseName 为null";
                return retValue;
            }
            return UpdateVerify(retValue, entity, mediaInfo.CreateUserID);
        }

        public dynamic GetEntity(MediaType mediaType, ReturnValue returnValue)
        {
            switch (mediaType)
            {
                case MediaType.WeiXin:
                    return returnValue.ReturnObject as Entities.Media.MediaWeixin;

                case MediaType.Video:
                    return returnValue.ReturnObject as Entities.Media.MediaVideo;

                case MediaType.APP:
                    return returnValue.ReturnObject as Entities.Media.MediaPcApp;

                case MediaType.Broadcast:
                    return returnValue.ReturnObject as Entities.Media.MediaBroadcast;

                case MediaType.WeiBo:
                    return returnValue.ReturnObject as Entities.Media.MediaWeibo;

                default:
                    returnValue.HasError = true;
                    returnValue.ErrorCode = "30012";
                    returnValue.Message = "拆箱失败";
                    return null;
            }
        }

        public ReturnValue VerifyOfMediaInfo(int mediaId, MediaType mediaType)
        {
            var returnValue = new ReturnValue()
            {
                HasError = true,
                ErrorCode = "30001",
                Message = string.Format("当前媒体信息不存在，BusinessType={0}&MediaId={1}",
                   (int)mediaType, mediaId)
            };
            var mediaDic = new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s => Dal.Media.MediaWeixin.Instance.GetEntity(mediaId)},
                {(int) MediaType.WeiBo, s => Dal.Media.MediaWeibo.Instance.GetEntity(mediaId)},
                {(int) MediaType.Video, s => Dal.Media.MediaVideo.Instance.GetEntity(mediaId)},
                {(int) MediaType.Broadcast, s => Dal.Media.MediaBroadcast.Instance.GetEntity(mediaId)},
                {(int) MediaType.APP, s => Dal.Media.MediaPCAPP.Instance.GetEntity(mediaId)}
            };
            if (mediaDic.ContainsKey((int)mediaType))
            {
                var mediaInfo = mediaDic[(int)mediaType].Invoke(string.Empty);
                if (mediaInfo == null)
                {
                    return returnValue;
                }
                returnValue.HasError = false;
                returnValue.Message = string.Empty;
                returnValue.ReturnObject = mediaInfo;
                return returnValue;
            }
            else
            {
                returnValue.HasError = true;
                returnValue.ErrorCode = "30000";
                returnValue.Message = "请输入合法的mediaType";
                return returnValue;
            }
        }

        private ReturnValue UpdateVerify(ReturnValue retValue, Entities.Media.MediaQualification entity,
            int mediaUserId, MediaType mediaType = MediaType.WeiXin)
        {
            VerifyOfMediaQualification(entity);
            var recId = 0;
            var info = Dal.Media.MediaQualification.Instance.GetEntity(entity.MediaID, 0, entity.MediaType);
            if (info == null)
            {
                recId = Dal.Media.MediaQualification.Instance.Insert(entity);
                if (recId <= 0)
                {
                    retValue.HasError = true;
                    retValue.Message = string.Format("添加数据到MediaQualification失败！参数：{0}",
                       JsonConvert.SerializeObject(entity));
                    retValue.ErrorCode = "60001";
                    Loger.Log4Net.ErrorFormat(retValue.Message);
                    return retValue;
                }
                //更新用户详情营业执照信息
                //retValue = UpdateUserDetail(retValue, mediaUserId, entity);
            }
            else
            {
                //修改
                Dal.Media.MediaQualification.Instance.Update(entity, mediaUserId);
                recId = info.RecID;
                //更新用户详情营业执照信息
                //retValue = UpdateUserDetail(retValue, mediaUserId, entity);
            }

            retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(new List<string>()
                    { entity.BusinessLicense, entity.QualificationOne,
                        entity.QualificationTwo,entity.IDCardBackURL,entity.IDCardFrontURL,
                        entity.AgentContractBackURL,entity.AgentContractFrontURL}, entity.CreateUserID,
                UploadFileEnum.QualificationManage, recId, "Media_Qualification");

            return retValue;
        }

        /// <summary>
        /// 去除url域名
        /// </summary>
        /// <param name="entity"></param>
        private void VerifyOfMediaQualification(Entities.Media.MediaQualification entity)
        {
            entity.AgentContractBackURL.ToAbsolutePath(true);
            entity.AgentContractFrontURL.ToAbsolutePath(true);
            entity.IDCardBackURL.ToAbsolutePath(true);
            entity.IDCardFrontURL.ToAbsolutePath(true);
        }

        /// <summary>
        /// 更新用户详情营业执照信息
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="mediaUserId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ReturnValue UpdateUserDetail(ReturnValue retValue, int mediaUserId, Entities.Media.MediaQualification entity)
        {
            //更新用户详情营业执照信息
            if (Dal.Media.MediaQualification.Instance.UpdateUserDetail(entity, mediaUserId) == 0)
            {
                retValue.HasError = true;
                retValue.Message = string.Format("更新用户详情营业执照信息失败！参数：{0}&mediaUserId={1}",
                    JsonConvert.SerializeObject(entity), mediaUserId);
                retValue.ErrorCode = "60003";
                Loger.Log4Net.ErrorFormat(retValue.Message);
                return retValue;
            }
            return retValue;
        }
    }
}