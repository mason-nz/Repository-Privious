using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base
{
    public abstract class OperateBase<T>
        where T : CreateBusinessBaseDto, new()
    {
        protected readonly T RequestMedia;
        protected readonly RequestMediaPublicParam RequestMediaPublicParam;

        protected OperateBase(RequestMediaPublicParam publicParam, T requestMediaDto)
        {
            RequestMedia = requestMediaDto;
            RequestMediaPublicParam = publicParam;
        }

        /// <summary>
        /// 媒体添加方法
        /// </summary>
        /// <returns></returns>
        public virtual ReturnValue Create()
        {
            var retValue = VerifyOfNecessaryParameters();//基础参数校验
            if (retValue.HasError)
                return retValue;
            retValue = VerifyCreateBusiness();
            if (retValue.HasError)
                return retValue;
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        /// <summary>
        /// 媒体编辑方法
        /// </summary>
        /// <returns></returns>
        public virtual ReturnValue Update()
        {
            var retValue = VerifyOfNecessaryParameters();//基础参数校验
            if (retValue.HasError)
                return retValue;
            retValue = VerifyUpdateBusinessParams();
            if (retValue.HasError)
                return retValue;
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        /// <summary>
        /// 校验Create逻辑
        /// </summary>
        /// <returns></returns>
        protected abstract ReturnValue VerifyCreateBusiness();

        /// <summary>
        /// 校验update逻辑
        /// </summary>
        /// <returns></returns>
        protected abstract ReturnValue VerifyUpdateBusinessParams();

        /// <summary>
        /// 校验角色
        /// </summary>
        /// <param name="mediaInfoUserId">模块入库时的用户Id</param>
        /// <returns></returns>
        protected abstract ReturnValue VerifyModuleRights(int mediaInfoUserId);

        protected ReturnValue VerifyOfNecessaryParameters()
        {
            var retValue = new VerifyOfNecessaryParameters<RequestMediaPublicParam>().VerifyNecessaryParameters(RequestMediaPublicParam);
            if (retValue.HasError)
                return retValue;
            retValue = new VerifyOfNecessaryParameters<T>().VerifyNecessaryParameters(RequestMedia);
            if (retValue.HasError)
                return retValue;
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        /// <summary>
        /// 校验媒体的刊例已上架，不能修改
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="requestMediaPublicParam"></param>
        /// <returns></returns>
        protected ReturnValue VerifyUpdateMedia(ReturnValue returnValue,
            RequestMediaPublicParam requestMediaPublicParam)
        {
            returnValue = returnValue ?? new ReturnValue() { };
            var publishBaseInfo = BLL.Publish.PublishInfoQuery.Instance.GetPublishBasicInfo(new PublishQuery<PublishBasicInfo>()
            {
                Media_Id = requestMediaPublicParam.MediaID,
                MediaType = requestMediaPublicParam.BusinessType
            });
            if (publishBaseInfo == null)
                return returnValue;
            if (publishBaseInfo.Status == AuditStatusEnum.已通过)
            {
                var role = RoleInfoMapping.GetUserRole();
                if (role == RoleEnum.MediaOwner)
                {
                    returnValue.HasError = true;
                    returnValue.Message = "此媒体的刊例审核已通过，媒体主不能修改";
                    return returnValue;
                }
            }
            returnValue.HasError = false;
            returnValue.Message = string.Empty;
            return returnValue;
        }
    }
}