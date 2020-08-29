using System;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    /// <summary>
    /// auth:lixiong
    /// 媒体收藏夹、黑名单操作代理类
    /// </summary>
    public class CollectPullBackProxy : CurrentOperateBase
    {
        private readonly RemoveCollectPullBackDto _removeCollectPullBackDto;
        private readonly AddToCollectPullBackDto _addToCollectPullBackDto;

        public CollectPullBackProxy(AddToCollectPullBackDto addToCollectPullBackDto)
        {
            _addToCollectPullBackDto = addToCollectPullBackDto;
        }

        public CollectPullBackProxy(RemoveCollectPullBackDto removeCollectPullBackDto)
        {
            _removeCollectPullBackDto = removeCollectPullBackDto;
        }

        /// <summary>
        /// 添加到收藏夹/拉黑操作
        /// </summary>
        /// <returns>结果返回</returns>
        public ReturnValue AddToExcute()
        {
            var retValue = new ReturnValue();

            retValue = VerifyAddBusiness(retValue);
            if (retValue.HasError)
            {
                return retValue;
            }

            return Insert(retValue);
        }

        /// <summary>
        /// 移除收藏夹/拉黑操作
        /// </summary>
        /// <returns></returns>
        public ReturnValue RemoveExcute()
        {
            var retValue = VerifyRemoveBusiness(new ReturnValue());
            if (retValue.HasError)
            {
                return retValue;
            }
            if (Dal.Media.MediaCollectionBlacklist.Instance.Delete(_removeCollectPullBackDto.MediaId,
                (MediaType)_removeCollectPullBackDto.BusinessType, (CollectPullBackTypeEnum)_removeCollectPullBackDto.OperateType,
                _removeCollectPullBackDto.CreateUserId) == 0)
                return CreateFailMessage(retValue, "50028", "该记录不存在或已被移除");
            retValue.ErrorCode = string.Empty;
            retValue.Message = "操作成功";
            return retValue;
        }

        /// <summary>
        /// 添加收藏/拉黑到数据库具体方法
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue Insert(ReturnValue retValue)
        {
            if (Dal.Media.MediaCollectionBlacklist.Instance.Insert(new Entities.Media.MediaCollectionBlacklist
            {
                MediaID = _addToCollectPullBackDto.MediaId,
                CreateUserID = _addToCollectPullBackDto.CreateUserId,
                MediaType = _addToCollectPullBackDto.BusinessType,
                RelationType = _addToCollectPullBackDto.OperateType,
                Status = 0
            }) == 0)
            {
                return CreateFailMessage(retValue, "50025", "操作失败,错误码：50025");
            }

            return retValue;
        }

        /// <summary>
        /// 添加操作的参数、逻辑验证
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyAddBusiness(ReturnValue retValue)
        {
            if (!Enum.IsDefined(typeof(CollectPullBackTypeEnum), _addToCollectPullBackDto.OperateType))
            {
                return CreateFailMessage(retValue, "50023", "请输入合法的操作类型");
            }
            retValue = VerifyOfNecessaryParameters(_addToCollectPullBackDto);
            if (retValue.HasError)
            {
                return retValue;
            }

            var info = Dal.Media.MediaCollectionBlacklist.Instance.GetEntity(_addToCollectPullBackDto.MediaId,
                 _addToCollectPullBackDto.BusinessType, _addToCollectPullBackDto.OperateType,
                 _addToCollectPullBackDto.CreateUserId);
            if (info != null)
            {
                return CreateFailMessage(retValue, "50024", "请勿重复操作");
            }
            return retValue;
        }

        private ReturnValue VerifyRemoveBusiness(ReturnValue retValue)
        {
            if (!Enum.IsDefined(typeof(CollectPullBackTypeEnum), _removeCollectPullBackDto.OperateType))
            {
                return CreateFailMessage(retValue, "50023", "请输入合法的操作类型");
            }

            return VerifyOfNecessaryParameters(_removeCollectPullBackDto);
        }
    }
}