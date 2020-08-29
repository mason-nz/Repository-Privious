using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Interaction;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business
{
    public class WeiXinOperate : OperateBase<RequestMediaWeiXinDto>//,IMediaOperate
    {
        private readonly RequestMediaPublicParam _requestMediaPublicParam;
        private readonly RequestMediaWeiXinDto _requestMediaWeiXinDto;
        private Entities.Media.MediaWeixin _itemEntity;
        private Entities.Interaction.InteractionWeixin _itemInteractionWeixinEntity;

        public WeiXinOperate(RequestMediaPublicParam publicParam, RequestMediaWeiXinDto requestMediaWeiXinDto)
            : base(publicParam, requestMediaWeiXinDto)
        {
            _requestMediaPublicParam = publicParam;
            _requestMediaWeiXinDto = requestMediaWeiXinDto;
            SetEntity();
        }

        public override ReturnValue Create()
        {
            var retValue = base.Create();
            if (retValue.HasError)
            {
                return retValue;
            }
            var mediaId = BLL.Media.MediaWeixin.Instance.Insert(_itemEntity);
            if (mediaId <= 0)
            {
                retValue.ErrorCode = "1001";
                retValue.HasError = true;
                retValue.Message = "微信公众号添加失败";
                return retValue;
            }
            _requestMediaPublicParam.MediaID = _itemInteractionWeixinEntity.MediaID = mediaId;
            if (BLL.Interaction.InteractionWeixin.Instance.Insert(_itemInteractionWeixinEntity) <= 0)
            {
                retValue.ErrorCode = "1002";
                retValue.HasError = true;
                retValue.Message = "微信公众号-互动参数添加失败";
                return retValue;
            }
            BLL.Media.MediaAreaMapping.Instance.BusinessCure(_requestMediaPublicParam);
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
            if (BLL.Media.MediaWeixin.Instance.Update(_itemEntity) <= 0)
            {
                retValue.ErrorCode = "1021";
                retValue.HasError = true;
                retValue.Message = "微信公众号-编辑失败";
                return retValue;
            }
            retValue = OperateInteraction();
            if (retValue.HasError)
            {
                return retValue;
            }

            //覆盖区域
            Task.Factory.StartNew(() => BLL.Media.MediaAreaMapping.Instance.BusinessCure(_requestMediaPublicParam));

            //文件管理
            UploadFile(_requestMediaPublicParam.MediaID);

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
                _itemEntity.TwoCodeURL
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, _requestMediaPublicParam.CreateUserID,
                   UploadFileEnum.MediaManage, mediaId, "Media_Weixin");
        }

        private void SetEntity()
        {
            _itemEntity = GetOperateEntity();
            _itemInteractionWeixinEntity = GetInteractionEntity(_requestMediaPublicParam.MediaID);
        }

        /// <summary>
        /// AE 运营 超级管理员，在一个范围内，只能有一个微信号存在，不能重复
        /// 媒体主角色 不同的用户可以添加多个相同的，但一个用户只能添加一个
        /// </summary>
        /// <returns></returns>
        protected override ReturnValue VerifyCreateBusiness()
        {
            var retValue = VerifyModuleRights(-1);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1056" });//校验微信名称
            return retValue;
            //var retValue = VerifyOfWeiXinOperate.CheckWeiXinNumber(_itemEntity);
            //return retValue.HasError
            //    ? retValue : VerifyModuleRights(-1);
        }

        private ReturnValue VerifyCreateBusiness(ReturnValue retValue, Entities.Media.MediaWeixin weixinEntity)
        {
            retValue = retValue ?? new ReturnValue();
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1056" });//校验微信名称
            return retValue;
        }

        private ReturnValue VerifyUpdateNumberOrName(Entities.Media.MediaWeixin itemEntity, ReturnValue retValue, List<string> errorCodeList)
        {
            retValue = retValue ?? new ReturnValue();
            var role = RoleInfoMapping.GetUserRole(itemEntity.CreateUserID);
            var roleList = RoleInfoMapping.GetVerifyMediaRoleIdList(role);
            if (
                Dal.Media.MediaWeixin.Instance.GetRepeatCount(itemEntity.Number, string.Empty, roleList, "Media_Weixin",
                    itemEntity.MediaID) > 0)
            {
                //存在重复
                retValue.ErrorCode = errorCodeList[0];//1058
                retValue.HasError = true;
                retValue.Message = string.Format("错误:{1},当前角色范围只允许添加一个相同{0}", "帐号", retValue.ErrorCode);
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        protected override ReturnValue VerifyUpdateBusinessParams()
        {
            var retValue = VerifyOfWeiXinOperate.CheckWeiXinById(_itemEntity);
            if (retValue.HasError)
                return retValue;
            var mediaInfo = retValue.ReturnObject as Entities.Media.MediaWeixin;
            if (mediaInfo == null)
            {
                retValue.HasError = true;
                retValue.Message = "VerifyUpdateBusinessParams 数据转换失败";
                return retValue;
            }
            retValue = VerifyModuleRights(mediaInfo.CreateUserID);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1058" });//校验微信名称
            if (retValue.HasError)
            {
                return retValue;
            }
            //校验媒体的刊例已上架，不能修改
            retValue = VerifyUpdateMedia(retValue, _requestMediaPublicParam);
            return retValue.HasError ?
                retValue
                : VerifyOfWeiXinOperate.CheckInteractionWeiXinByMediaId(_itemEntity);
        }

        private ReturnValue VerifyNumberOrName(Entities.Media.MediaWeixin itemEntity)
        {
            var retValue = VerifyOfWeiXinOperate.CheckWeiXinNumber(itemEntity);

            return retValue;
            //if (retValue.HasError)
            //    return retValue;
            //return VerifyOfWeiXinOperate.CheckWeiXinName(itemEntity);
        }

        protected override ReturnValue VerifyModuleRights(int mediaInfoUserId)
        {
            var moduleId = new Dictionary<int, string>()
            {
                {1,"SYS001BUT400101"},{2,"SYS001BUT400102"}
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
            if (BLL.Interaction.InteractionWeixin.Instance.GetEntity(_itemEntity.MediaID) == null)
            {
                _itemInteractionWeixinEntity.MediaID = _itemEntity.MediaID;
                if (BLL.Interaction.InteractionWeixin.Instance.Insert(_itemInteractionWeixinEntity) <= 0)
                {
                    retValue.ErrorCode = "10031";
                    retValue.HasError = true;
                    retValue.Message = "weixin-互动参数添加失败";
                    return retValue;
                }
            }
            else
            {
                if (BLL.Interaction.InteractionWeixin.Instance.Update(_itemInteractionWeixinEntity) <= 0)
                {
                    retValue.ErrorCode = "10032";
                    retValue.HasError = true;
                    retValue.Message = "weixin-互动参数编辑失败";
                    return retValue;
                }
            }
            return retValue;
        }

        private Entities.Media.MediaWeixin GetOperateEntity()
        {
            return new Entities.Media.MediaWeixin()
            {
                MediaID = _requestMediaPublicParam.MediaID,
                AreaID = _requestMediaWeiXinDto.AreaID,
                CategoryID = _requestMediaPublicParam.CategoryID,
                CityID = _requestMediaPublicParam.CityID,
                FansCount = _requestMediaPublicParam.FansCount,
                FansCountURL = _requestMediaWeiXinDto.FansCountURL.ToAbsolutePath(true),
                FansFemalePer = _requestMediaWeiXinDto.FansFemalePer,
                FansMalePer = _requestMediaWeiXinDto.FansMalePer,
                HeadIconURL = _requestMediaPublicParam.HeadIconURL.ToAbsolutePath(true),
                Number = _requestMediaPublicParam.Number,
                Name = _requestMediaPublicParam.Name,
                Sign = RequestMedia.Sign,
                ProvinceID = _requestMediaPublicParam.ProvinceID,
                TwoCodeURL = RequestMedia.TwoCodeURL.ToAbsolutePath(true),
                LevelType = RequestMedia.LevelType,
                IsAuth = _requestMediaWeiXinDto.IsAuth,
                OrderRemark = _requestMediaWeiXinDto.OrderRemark,
                IsReserve = _requestMediaWeiXinDto.IsReserve,
                Source = RequestMediaPublicParam.Source,
                Status = 0,
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }

        private Entities.Interaction.InteractionWeixin GetInteractionEntity(int mediaId)
        {
            return new InteractionWeixin
            {
                MeidaType = _requestMediaPublicParam.BusinessType,
                AveragePointCount = _requestMediaWeiXinDto.AveragePointCount,
                MaxinumReading = _requestMediaWeiXinDto.MaxinumReading,
                MediaID = RequestMediaPublicParam.MediaID,
                MoreReadCount = _requestMediaWeiXinDto.MoreReadCount,
                OrigArticleCount = _requestMediaWeiXinDto.OrigArticleCount,
                ScreenShotURL = _requestMediaWeiXinDto.ScreenShotURL.ToAbsolutePath(true),
                UpdateCount = _requestMediaWeiXinDto.UpdateCount,
                ReferReadCount = _requestMediaWeiXinDto.ReferReadCount,
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }
    }
}