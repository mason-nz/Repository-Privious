using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business
{
    public class BroadcastOperate : OperateBase<RequestMediaBroadcastDto>
    {
        private Entities.Media.MediaBroadcast _itemEntity;
        private Entities.Interaction.InteractionBroadcast _itemInteractionEntity;

        public BroadcastOperate(RequestMediaPublicParam publicParam, RequestMediaBroadcastDto requestMediaDto)
            : base(publicParam, requestMediaDto)
        {
            SetEntity();
        }

        public override ReturnValue Create()
        {
            var retValue = base.Create();
            if (retValue.HasError) return retValue;
            var mediaId = BLL.Media.MediaBroadcast.Instance.Insert(_itemEntity);
            if (mediaId <= 0)
            {
                retValue.ErrorCode = "1003";
                retValue.HasError = true;
                retValue.Message = "直播添加失败";
                return retValue;
            }
            RequestMediaPublicParam.MediaID = _itemInteractionEntity.MediaID = mediaId;
            if (BLL.Interaction.InteractionBroadcast.Instance.Insert(_itemInteractionEntity) <= 0)
            {
                retValue.ErrorCode = "1004";
                retValue.HasError = true;
                retValue.Message = "直播-互动参数添加失败";
                return retValue;
            }
            BLL.Media.MediaAreaMapping.Instance.BusinessCureForTask(RequestMediaPublicParam);
            //文件管理
            UploadFile(mediaId);
            retValue.Message = "添加成功";
            retValue.ReturnObject = mediaId;
            return retValue;
        }

        public override ReturnValue Update()
        {
            var retValue = base.Update();
            if (retValue.HasError) return retValue;

            if (BLL.Media.MediaBroadcast.Instance.Update(_itemEntity) <= 0)
            {
                retValue.ErrorCode = "1026";
                retValue.HasError = true;
                retValue.Message = "直播-编辑失败";
                return retValue;
            }
            retValue = OperateInteraction();
            if (retValue.HasError) return retValue;
            BLL.Media.MediaAreaMapping.Instance.BusinessCureForTask(RequestMediaPublicParam);

            //文件管理
            UploadFile(_itemEntity.MediaID);
            retValue.Message = "编辑成功";
            retValue.ReturnObject = RequestMediaPublicParam.MediaID;
            return retValue;
        }

        private void UploadFile(int mediaId)
        {
            var urlList = new List<string>()
            {
                _itemEntity.HeadIconURL
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, RequestMediaPublicParam.CreateUserID,
                   UploadFileEnum.MediaManage, mediaId, "Media_Broadcast");
        }

        private void SetEntity()
        {
            _itemEntity = GetOperateEntity();
            _itemInteractionEntity = GetInteractionEntity(RequestMediaPublicParam.MediaID);
        }

        protected override ReturnValue VerifyCreateBusiness()
        {
            var retValue = VerifyModuleRights(-1);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1101" });//校验重复
            return retValue;

            //var retValue = VerifyOfBroadcastOperate.CheckBroadcastNumber(_itemEntity);
            //return retValue.HasError
            //    ? retValue : VerifyModuleRights(-1);
        }

        private ReturnValue VerifyUpdateNumberOrName(Entities.Media.MediaBroadcast itemEntity, ReturnValue retValue, List<string> errorCodeList)
        {
            retValue = retValue ?? new ReturnValue();
            var role = RoleInfoMapping.GetUserRole(itemEntity.CreateUserID);
            var roleList = RoleInfoMapping.GetVerifyMediaRoleIdList(role);
            if (
                Dal.Media.MediaBroadcast.Instance.GetRepeatCount(itemEntity.Number, string.Empty, itemEntity.Platform, roleList, "Media_Broadcast",
                    itemEntity.MediaID) > 0)
            {
                //存在重复
                retValue.ErrorCode = "1100";
                retValue.HasError = true;
                retValue.Message = string.Format("错误:{1},当前角色范围只允许添加一个相同平台的{0}", "帐号", retValue.ErrorCode);
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        protected override ReturnValue VerifyUpdateBusinessParams()
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "20012", Message = string.Format("此直播媒体信息不存在Id:{0}", _itemEntity.MediaID) };
            var info = BLL.Media.MediaBroadcast.Instance.GetEntity(_itemEntity.MediaID);
            if (info == null)
                return retValue;
            retValue = VerifyModuleRights(info.CreateUserID);
            if (retValue.HasError) return retValue;
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1100" });//校验重复
            if (retValue.HasError) return retValue;
            //校验媒体的刊例已上架，不能修改
            retValue = VerifyUpdateMedia(retValue, RequestMediaPublicParam);
            if (retValue.HasError) return retValue;

            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        protected override ReturnValue VerifyModuleRights(int mediaInfoUserId)
        {
            var moduleId = new Dictionary<int, string>()
            {
                {1,"SYS001BUT400401"},{2,"SYS001BUT400402"}
            }.FirstOrDefault(s => s.Key == RequestMediaPublicParam.OperateType).Value;
            return VerifyOfMediaOperateRole.CheckModuleRight(new VerifyModuleRight()
            {
                ModuleId = moduleId,//当前模块Id
                PublicParam = RequestMediaPublicParam,
                UserId = mediaInfoUserId//模块入库时的用户Id
            });
        }

        private ReturnValue OperateInteraction()
        {
            var retValue = new ReturnValue();
            if (BLL.Interaction.InteractionBroadcast.Instance.GetEntity(_itemEntity.MediaID) == null)
            {
                _itemInteractionEntity.MediaID = _itemEntity.MediaID;
                if (BLL.Interaction.InteractionBroadcast.Instance.Insert(_itemInteractionEntity) <= 0)
                {
                    retValue.ErrorCode = "10028";
                    retValue.HasError = true;
                    retValue.Message = "直播-互动参数添加失败";
                    return retValue;
                }
            }
            else
            {
                if (BLL.Interaction.InteractionBroadcast.Instance.Update(_itemInteractionEntity) <= 0)
                {
                    retValue.ErrorCode = "10029";
                    retValue.HasError = true;
                    retValue.Message = "直播-互动参数编辑失败";
                    return retValue;
                }
            }
            return retValue;
        }

        private Entities.Media.MediaBroadcast GetOperateEntity()
        {
            return new Entities.Media.MediaBroadcast()
            {
                MediaID = RequestMediaPublicParam.MediaID,
                RoomID = RequestMedia.RoomID,
                Number = RequestMediaPublicParam.Number,
                Name = RequestMediaPublicParam.Name,

                HeadIconURL = RequestMediaPublicParam.HeadIconURL.ToAbsolutePath(true),
                CategoryID = RequestMediaPublicParam.CategoryID,
                CityID = RequestMediaPublicParam.CityID,
                FansCount = RequestMediaPublicParam.FansCount,
                ProvinceID = RequestMediaPublicParam.ProvinceID,
                Profession = RequestMedia.Profession,
                FansCountURL = RequestMedia.FansCountURL,
                Platform = RequestMedia.Platform,
                Sex = RequestMedia.Sex,
                LevelType = RequestMedia.LevelType,
                IsAuth = RequestMedia.IsAuth ? 1 : 0,
                IsReserve = RequestMedia.IsReserve,
                Status = 0,
                Source = RequestMediaPublicParam.Source,
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }

        private Entities.Interaction.InteractionBroadcast GetInteractionEntity(int mediaId)
        {
            return new Entities.Interaction.InteractionBroadcast
            {
                MeidaType = RequestMediaPublicParam.BusinessType,
                AudienceCount = RequestMedia.AudienceCount,
                MediaID = RequestMediaPublicParam.MediaID,
                MaximumAudience = RequestMedia.MaximumAudience,
                AverageAudience = RequestMedia.AverageAudience,
                ScreenShotURL = RequestMedia.ScreenShotURL.ToAbsolutePath(),
                CumulateReward = RequestMedia.CumulateReward,
                CumulateIncome = RequestMedia.CumulateIncome,
                CumulatePoints = RequestMedia.CumulatePoints,
                CumulateSendCount = RequestMedia.CumulateSendCount,
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }
    }
}