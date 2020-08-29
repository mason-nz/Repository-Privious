using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business
{
    public class PcAppOperate : OperateBase<RequestMediaPcAppDto>
    {
        private Entities.Media.MediaPcApp _itemEntity;

        public PcAppOperate(RequestMediaPublicParam publicParam, RequestMediaPcAppDto requestMediaDto)
            : base(publicParam, requestMediaDto)
        {
            SetEntity();
        }

        public override ReturnValue Create()
        {
            var retValue = VerifyCreateBusiness();//因为app页面涉及到的参数比较少，不能沿用之前的逻辑，校验公共参数
            if (retValue.HasError)
            {
                return retValue;
            }
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1098" });//校验名称重复
            if (retValue.HasError)
            {
                return retValue;
            }
            var entity = GetOperateEntity();
            var mediaId = BLL.Media.MediaPCAPP.Instance.Insert(entity);
            if (mediaId <= 0)
            {
                retValue.ErrorCode = "1005";
                retValue.HasError = true;
                retValue.Message = "PcApp添加失败";
                return retValue;
            }
            RequestMediaPublicParam.MediaID = mediaId;

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
            var retValue = VerifyUpdateBusinessParams();//base.Update();
            if (retValue.HasError)
            {
                return retValue;
            }
            if (BLL.Media.MediaPCAPP.Instance.Update(_itemEntity) <= 0)
            {
                retValue.ErrorCode = "1028";
                retValue.HasError = true;
                retValue.Message = "PcApp-编辑失败";
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
                _itemEntity.HeadIconURL
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, RequestMediaPublicParam.CreateUserID,
                   UploadFileEnum.MediaManage, mediaId, "Media_PCAPP");
        }

        private void SetEntity()
        {
            _itemEntity = GetOperateEntity();
        }

        protected override ReturnValue VerifyCreateBusiness()
        {
            var retValue = new VerifyOfNecessaryParameters<RequestMediaPcAppDto>().VerifyNecessaryParameters(RequestMedia);
            if (retValue.HasError)
                return retValue;

            #region 基本参数校验

            if (RequestMediaPublicParam.OperateType == (int)OperateType.Insert)
            {
                retValue = VerifyModuleRights(-1);
                if (retValue.HasError)
                    return retValue;
            }

            if (string.IsNullOrWhiteSpace(RequestMediaPublicParam.Name))
            {
                retValue.HasError = true;
                retValue.Message = "请输入参数name";
                return retValue;
            }
            if (string.IsNullOrWhiteSpace(RequestMediaPublicParam.HeadIconURL))
            {
                retValue.HasError = true;
                retValue.Message = "请输入参数HeadIconURL";
                return retValue;
            }
            if (string.IsNullOrWhiteSpace(RequestMediaPublicParam.CoverageArea))
            {
                retValue.HasError = true;
                retValue.Message = "请输入参数CoverageArea";
                return retValue;
            }

            if (string.IsNullOrWhiteSpace(RequestMedia.Terminal))
            {
                retValue.HasError = true;
                retValue.Message = "请输入参数广告终端Terminal";
                return retValue;
            }
            var spTerminal = RequestMedia.Terminal.Split(',');
            foreach (var item in spTerminal.Where(item => !string.IsNullOrWhiteSpace(item)))
            {
                if (!Enum.IsDefined(typeof(TerminalType), item.ToInt()))
                {
                    retValue.HasError = true;
                    retValue.Message = string.Format("Terminal字段其中的值为：{0}，不合法", item);
                    return retValue;
                }
                if (item.ToInt() == (int)TerminalType.App)
                {
                    if (RequestMedia.DailyLive <= -1 || RequestMedia.DailyLive > int.MaxValue)
                    {
                        retValue.HasError = true;
                        retValue.Message = "Terminal为app，请输入参数DailyLive";
                        return retValue;
                    }
                }
                if (item.ToInt() != (int)TerminalType.WapPc) continue;
                if (RequestMedia.DailyIP > -1 || RequestMedia.DailyIP < int.MaxValue) continue;
                retValue.HasError = true;
                retValue.Message = "Terminal为WapPc，请输入参数DailyIP";
                return retValue;
            }

            #endregion 基本参数校验

            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
            //return VerifyOfPcAppOperate.CheckPcAppNumber(_itemEntity);
        }

        protected override ReturnValue VerifyUpdateBusinessParams()
        {
            var retValue = new ReturnValue() { HasError = true, Message = "当前媒体pcapp信息不存在 id：" + _itemEntity.MediaID };
            var info = BLL.Media.MediaPCAPP.Instance.GetEntity(_itemEntity.MediaID);
            if (info == null)
                return retValue;
            retValue = VerifyModuleRights(info.CreateUserID);
            if (retValue.HasError)
            {
                return retValue;
            }
            retValue = VerifyCreateBusiness();
            if (retValue.HasError)
            {
                return retValue;
            }
            retValue = VerifyUpdateNumberOrName(_itemEntity, retValue, new List<string>() { "1099" });//校验名称重复
            if (retValue.HasError)
            {
                return retValue;
            }
            //校验媒体的刊例已上架，不能修改
            retValue = VerifyUpdateMedia(retValue, RequestMediaPublicParam);
            if (retValue.HasError)
            {
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue VerifyUpdateNumberOrName(Entities.Media.MediaPcApp itemEntity, ReturnValue retValue, List<string> errorCodeList)
        {
            retValue = retValue ?? new ReturnValue();
            var role = RoleInfoMapping.GetUserRole(itemEntity.CreateUserID);
            var roleList = RoleInfoMapping.GetVerifyMediaRoleIdList(role);
            if (
                Dal.Media.MediaBroadcast.Instance.GetRepeatCount(string.Empty, itemEntity.Name, -1, roleList, "Media_PCAPP",
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

        protected override ReturnValue VerifyModuleRights(int mediaInfoUserId)
        {
            var moduleId = new Dictionary<int, string>()
            {
                {1,"SYS001BUT400501"},{2,"SYS001BUT400502"}
            }.FirstOrDefault(s => s.Key == RequestMediaPublicParam.OperateType).Value;
            return VerifyOfMediaOperateRole.CheckModuleRight(new VerifyModuleRight()
            {
                ModuleId = moduleId,//当前模块Id
                PublicParam = RequestMediaPublicParam,
                UserId = mediaInfoUserId//模块入库时的用户Id
            });
        }

        private Entities.Media.MediaPcApp GetOperateEntity()
        {
            return new Entities.Media.MediaPcApp()
            {
                MediaID = RequestMediaPublicParam.MediaID,
                Name = RequestMediaPublicParam.Name,
                HeadIconURL = RequestMediaPublicParam.HeadIconURL.ToAbsolutePath(true),
                CategoryID = RequestMediaPublicParam.CategoryID,
                CityID = RequestMediaPublicParam.CityID,
                ProvinceID = RequestMediaPublicParam.ProvinceID,
                Terminal = RequestMedia.Terminal,
                DailyLive = Convert.ToInt32(RequestMedia.DailyLive),
                DailyIP = Convert.ToInt32(RequestMedia.DailyIP),
                WebSite = RequestMedia.WebSite,
                Remark = RequestMedia.Remark,
                Source = RequestMediaPublicParam.Source,
                CreateTime = RequestMediaPublicParam.CreateTime,
                CreateUserID = RequestMediaPublicParam.CreateUserID,
                LastUpdateTime = RequestMediaPublicParam.LastUpdateTime,
                LastUpdateUserID = RequestMediaPublicParam.LastUpdateUserID
            };
        }
    }
}