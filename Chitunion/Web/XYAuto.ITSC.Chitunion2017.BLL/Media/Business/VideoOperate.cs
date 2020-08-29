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
    public class VideoOperate : OperateBase<RequestMediaVideoDto>
    {
        private Entities.Media.MediaVideo _itemEntity;
        private Entities.Interaction.InteractionVideo _itemInteractionEntity;

        public VideoOperate(RequestMediaPublicParam publicParam, RequestMediaVideoDto requestMediaDto)
            : base(publicParam, requestMediaDto)
        {
            SetEntity();
        }

        public override ReturnValue Create()
        {
            var retValue = base.Create();
            if (retValue.HasError)
            {
                return retValue;
            }
            var mediaId = BLL.Media.MediaVideo.Instance.Insert(_itemEntity);
            if (mediaId <= 0)
            {
                retValue.ErrorCode = "1006";
                retValue.HasError = true;
                retValue.Message = "视频添加失败";
                return retValue;
            }
            RequestMediaPublicParam.MediaID = _itemInteractionEntity.MediaID = mediaId;
            if (BLL.Interaction.InteractionVideo.Instance.Insert(_itemInteractionEntity) <= 0)
            {
                retValue.ErrorCode = "1007";
                retValue.HasError = true;
                retValue.Message = "视频-互动参数添加失败";
                return retValue;
            }

            //覆盖区域
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
            if (retValue.HasError)
            {
                return retValue;
            }

            if (BLL.Media.MediaVideo.Instance.Update(_itemEntity) <= 0)
            {
                retValue.ErrorCode = "1025";
                retValue.HasError = true;
                retValue.Message = "视频-编辑失败";
                return retValue;
            }
            retValue = OperateInteractionVideo();
            if (retValue.HasError)
            {
                return retValue;
            }

            //覆盖区域
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
                _itemEntity.FansCountURL,
                _itemEntity.HeadIconURL,
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, RequestMediaPublicParam.CreateUserID,
                   UploadFileEnum.MediaManage, mediaId, "Media_Video");
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
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1089" });//校验平台和帐号
            return retValue;

            //var retValue = VerifyOfVideoOperate.CheckVideoNumber(_itemEntity);
            //return retValue.HasError
            //    ? retValue : VerifyModuleRights(-1);
        }

        private ReturnValue VerifyUpdateNumberOrName(Entities.Media.MediaVideo itemEntity, ReturnValue retValue, List<string> errorCodeList)
        {
            retValue = retValue ?? new ReturnValue();
            var role = RoleInfoMapping.GetUserRole(itemEntity.CreateUserID);
            var roleList = RoleInfoMapping.GetVerifyMediaRoleIdList(role);
            if (
                Dal.Media.MediaBroadcast.Instance.GetRepeatCount(itemEntity.Number, string.Empty, itemEntity.Platform, roleList, "Media_Video",
                    itemEntity.MediaID) > 0)
            {
                //存在重复
                retValue.ErrorCode = errorCodeList[0];
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
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "20010", Message = string.Format("此视频媒体信息不存在Id:{0}", _itemEntity.MediaID) };
            var info = BLL.Media.MediaVideo.Instance.GetEntity(_itemEntity.MediaID);
            if (info == null)
                return retValue;
            retValue = VerifyModuleRights(info.CreateUserID);
            if (retValue.HasError) return retValue;

            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1088" });//校验平台和帐号
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
                {1,"SYS001BUT400301"},{2,"SYS001BUT400302"}
            }.FirstOrDefault(s => s.Key == RequestMediaPublicParam.OperateType).Value;
            return VerifyOfMediaOperateRole.CheckModuleRight(new VerifyModuleRight()
            {
                ModuleId = moduleId,//当前模块Id
                PublicParam = RequestMediaPublicParam,
                UserId = mediaInfoUserId//模块入库时的用户Id
            });
        }

        private ReturnValue OperateInteractionVideo()
        {
            var retValue = new ReturnValue();
            if (BLL.Interaction.InteractionVideo.Instance.GetEntity(_itemEntity.MediaID) == null)
            {
                _itemInteractionEntity.MediaID = _itemEntity.MediaID;
                if (BLL.Interaction.InteractionVideo.Instance.Insert(_itemInteractionEntity) <= 0)
                {
                    retValue.ErrorCode = "10022";
                    retValue.HasError = true;
                    retValue.Message = "视频-互动参数添加失败";
                    return retValue;
                }
            }
            else
            {
                if (BLL.Interaction.InteractionVideo.Instance.Update(_itemInteractionEntity) <= 0)
                {
                    retValue.ErrorCode = "10027";
                    retValue.HasError = true;
                    retValue.Message = "视频-互动参数编辑失败";
                    return retValue;
                }
            }
            return retValue;
        }

        private Entities.Media.MediaVideo GetOperateEntity()
        {
            return new Entities.Media.MediaVideo()
            {
                MediaID = RequestMediaPublicParam.MediaID,
                Number = RequestMediaPublicParam.Number,
                Name = RequestMediaPublicParam.Name,

                HeadIconURL = RequestMediaPublicParam.HeadIconURL.ToAbsolutePath(),
                CategoryID = RequestMediaPublicParam.CategoryID,
                CityID = RequestMediaPublicParam.CityID,
                ProvinceID = RequestMediaPublicParam.ProvinceID,
                FansCount = RequestMediaPublicParam.FansCount,
                Profession = RequestMedia.Profession,
                Platform = RequestMedia.Platform,
                Sex = RequestMedia.Sex,
                AuthType = RequestMedia.IsAuth ? 1 : 0,
                IsReserve = RequestMedia.IsReserve,
                Status = 0,
                LevelType = RequestMedia.LevelType,
                Source = RequestMediaPublicParam.Source,
                FansCountURL = RequestMedia.FansCountURL.ToAbsolutePath(),
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }

        private Entities.Interaction.InteractionVideo GetInteractionEntity(int mediaId)
        {
            return new Entities.Interaction.InteractionVideo
            {
                MeidaType = RequestMediaPublicParam.BusinessType,
                AveragePointCount = RequestMedia.AveragePointCount,
                MediaID = RequestMediaPublicParam.MediaID,
                AveragePlayCount = RequestMedia.AveragePlayCount,
                AverageCommentCount = RequestMedia.AverageCommentCount,
                AverageBarrageCount = RequestMedia.AverageBarrageCount,
                ScreenShotURL = RequestMedia.ScreenShotURL.ToAbsolutePath(),
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }
    }
}