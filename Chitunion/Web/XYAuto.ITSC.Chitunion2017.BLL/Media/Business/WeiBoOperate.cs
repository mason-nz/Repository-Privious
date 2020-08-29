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
    public class WeiBoOperate : OperateBase<RequestMediaWeiBoDto>
    {
        private Entities.Media.MediaWeibo _itemEntity;
        private Entities.Interaction.InteractionWeibo _itemInteractionEntity;

        public WeiBoOperate(RequestMediaPublicParam publicParam, RequestMediaWeiBoDto requestMediaDto)
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
            var mediaId = BLL.Media.MediaWeibo.Instance.Insert(_itemEntity);
            if (mediaId <= 0)
            {
                retValue.ErrorCode = "1008";
                retValue.HasError = true;
                retValue.Message = "微博添加失败";
                return retValue;
            }
            RequestMediaPublicParam.MediaID = _itemInteractionEntity.MediaID = mediaId;
            if (BLL.Interaction.InteractionWeibo.Instance.Insert(_itemInteractionEntity) <= 0)
            {
                retValue.ErrorCode = "1009";
                retValue.HasError = true;
                retValue.Message = "微博-互动参数添加失败";
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

        private void SetEntity()
        {
            _itemEntity = GetOperateEntity();
            _itemInteractionEntity = GetInteractionEntity(RequestMediaPublicParam.MediaID);
        }

        private void UploadFile(int mediaId)
        {
            var urlList = new List<string>()
            {
                _itemEntity.FansCountURL,
                _itemEntity.HeadIconURL,
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, RequestMediaPublicParam.CreateUserID,
                   UploadFileEnum.MediaManage, mediaId, "Media_Weibo");
        }

        public override ReturnValue Update()
        {
            var retValue = base.Update();
            if (retValue.HasError) return retValue;

            if (BLL.Media.MediaWeibo.Instance.Update(_itemEntity) <= 0)
            {
                retValue.ErrorCode = "1023";
                retValue.HasError = true;
                retValue.Message = "微博-编辑失败";
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

        protected override ReturnValue VerifyCreateBusiness()
        {
            var retValue = VerifyModuleRights(-1);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1065", "1066" });//校验名称
            return retValue;
            //var retValue = VerifyOfWeiBoOperate.CheckWeiboNumber(_itemEntity);
            //return retValue.HasError
            //    ? retValue : VerifyModuleRights(-1);
        }

        protected override ReturnValue VerifyUpdateBusinessParams()
        {
            var retValue = new ReturnValue() { HasError = true, Message = "当前媒体weibo信息不存在 id：" + _itemEntity.MediaID };
            var info = BLL.Media.MediaWeibo.Instance.GetEntity(_itemEntity.MediaID);
            if (info == null)
                return retValue;

            retValue = VerifyModuleRights(info.CreateUserID);
            if (retValue.HasError) return retValue;

            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1077", "1078" });//校验名称
            if (retValue.HasError) return retValue;

            //校验媒体的刊例已上架，不能修改
            retValue = VerifyUpdateMedia(retValue, RequestMediaPublicParam);
            if (retValue.HasError) return retValue;
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue VerifyUpdateNumberOrName(Entities.Media.MediaWeibo itemEntity, ReturnValue retValue, List<string> errorCodeList)
        {
            retValue = retValue ?? new ReturnValue();
            var role = RoleInfoMapping.GetUserRole(itemEntity.CreateUserID);
            var roleList = RoleInfoMapping.GetVerifyMediaRoleIdList(role);
            if (
                Dal.Media.MediaWeixin.Instance.GetRepeatCount(itemEntity.Number, string.Empty, roleList, "Media_Weibo",
                    itemEntity.MediaID) > 0)
            {
                //存在重复
                retValue.ErrorCode = errorCodeList[0];//"1077";
                retValue.HasError = true;
                retValue.Message = string.Format("错误:{1},当前角色范围只允许添加一个相同{0}", "帐号", retValue.ErrorCode);
                return retValue;
            }
            if (
               Dal.Media.MediaWeixin.Instance.GetRepeatCount(string.Empty, itemEntity.Name, roleList, "Media_Weibo",
                   itemEntity.MediaID) > 0)
            {
                //存在重复
                retValue.ErrorCode = errorCodeList[1];//"1078";
                retValue.HasError = true;
                retValue.Message = string.Format("错误:{1},当前角色范围只允许添加一个相同{0}", "名称", retValue.ErrorCode);
                return retValue;
            }

            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue VerifyNumberOrName(Entities.Media.MediaWeibo itemEntity)
        {
            var retValue = VerifyOfWeiBoOperate.CheckWeiboNumber(itemEntity);
            if (retValue.HasError)
                return retValue;
            return VerifyOfWeiBoOperate.CheckWeiboName(itemEntity);
        }

        protected override ReturnValue VerifyModuleRights(int mediaInfoUserId)
        {
            var moduleId = new Dictionary<int, string>()
            {
                {1,"SYS001BUT400201"},{2,"SYS001BUT400202"}
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
            if (BLL.Interaction.InteractionWeibo.Instance.GetEntity(_itemEntity.MediaID) == null)
            {
                _itemInteractionEntity.MediaID = _itemEntity.MediaID;
                if (BLL.Interaction.InteractionWeibo.Instance.Insert(_itemInteractionEntity) <= 0)
                {
                    retValue.ErrorCode = "10030";
                    retValue.HasError = true;
                    retValue.Message = "weibo-互动参数添加失败";
                    return retValue;
                }
            }
            else
            {
                if (BLL.Interaction.InteractionWeibo.Instance.Update(_itemInteractionEntity) <= 0)
                {
                    retValue.ErrorCode = "10031";
                    retValue.HasError = true;
                    retValue.Message = "weibo-互动参数编辑失败";
                    return retValue;
                }
            }
            return retValue;
        }

        public Entities.Media.MediaWeibo GetOperateEntity()
        {
            return new Entities.Media.MediaWeibo()
            {
                MediaID = RequestMediaPublicParam.MediaID,
                Number = RequestMediaPublicParam.Number,
                Name = RequestMediaPublicParam.Name,
                HeadIconURL = RequestMediaPublicParam.HeadIconURL.ToAbsolutePath(true),
                CategoryID = RequestMediaPublicParam.CategoryID,
                CityID = RequestMediaPublicParam.CityID,
                ProvinceID = RequestMediaPublicParam.ProvinceID,
                FansCount = RequestMediaPublicParam.FansCount,
                Profession = RequestMedia.Profession,
                AreaID = RequestMedia.AreaID,
                Sex = RequestMedia.Sex,
                LevelType = RequestMedia.LevelType,
                FansSex = RequestMedia.FansSex,
                AuthType = RequestMedia.AuthType,
                OrderRemark = RequestMedia.OrderRemark,
                IsReserve = RequestMedia.IsReserve,
                Source = RequestMediaPublicParam.Source,
                Sign = RequestMedia.Sign,
                Status = 0,
                FansCountURL = RequestMedia.FansCountURL,
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }

        private Entities.Interaction.InteractionWeibo GetInteractionEntity(int mediaId)
        {
            return new Entities.Interaction.InteractionWeibo
            {
                MeidaType = RequestMediaPublicParam.BusinessType,
                AveragePointCount = RequestMedia.AveragePointCount,
                MediaID = RequestMediaPublicParam.MediaID,
                AverageForwardCount = RequestMedia.AverageForwardCount,
                AverageCommentCount = RequestMedia.AverageCommentCount,
                ScreenShotURL = RequestMedia.ScreenShotURL.ToAbsolutePath(true),
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }
    }
}