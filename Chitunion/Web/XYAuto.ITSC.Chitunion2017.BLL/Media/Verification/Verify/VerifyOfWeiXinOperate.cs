using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Enums;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify
{
    public class VerifyOfWeiXinOperate
    {
        /// <summary>
        /// 获取用户信息的规则方法
        /// </summary>
        /// <param name="entity">需要验证的实体</param>
        /// <param name="isDefaultRule">是否是默认的全部规则</param>
        /// <param name="funcDic">传入需要验证的规则</param>
        /// <param name="isOutParam">是否需要返回值</param>
        /// <returns></returns>
        public ReturnValue Verify(Entities.Media.MediaWeixin entity, bool isDefaultRule, Dictionary<int, Func<Entities.Media.MediaWeixin, ReturnValue>> funcDic,
            bool isOutParam = false)
        {
            //自定义规则
            if (!isDefaultRule) new Verifiction<Entities.Media.MediaWeixin>(funcDic).Verify(entity, isOutParam);
            //默认规则
            funcDic = new Dictionary<int, Func<Entities.Media.MediaWeixin, ReturnValue>>
            {
                {(int)  VerifyDataTypeEnum.VerifyOfWeiXinById, CheckWeiXinById},
                {(int)  VerifyDataTypeEnum.VerifyOfInteractionWeiXinByMediaId, CheckInteractionWeiXinByMediaId}
            };
            return new Verifiction<Entities.Media.MediaWeixin>(funcDic).Verify(entity, isOutParam);
        }

        public static ReturnValue CheckWeiXinNumber(Entities.Media.MediaWeixin entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10020", Message = string.Format("此微信号已存在") };
            var info = BLL.Media.MediaWeixin.Instance.GetEntity(entity.Number, entity.MediaID);
            if (info != null)
            {
                retValue.ReturnObject = info;
                return retValue;
            }

            retValue.HasError = false;
            retValue.Message = string.Empty;

            return retValue;
        }

        public static ReturnValue CheckWeiXinName(Entities.Media.MediaWeixin entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10040", Message = string.Format("此微信名称已经存在") };
            var info = BLL.Media.MediaWeixin.Instance.GetEntityByName(entity.Name, entity.MediaID);
            if (info != null)
            {
                retValue.ReturnObject = info;
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;

            return retValue;
        }

        public static ReturnValue CheckWeiXinById(Entities.Media.MediaWeixin entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10010", Message = string.Format("此媒体信息不存在Id:{0}", entity.MediaID) };
            var info = BLL.Media.MediaWeixin.Instance.GetEntity(entity.MediaID);
            if (info == null)
            {
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            retValue.ReturnObject = info;
            return retValue;
        }

        public static ReturnValue CheckInteractionWeiXinByMediaId(Entities.Media.MediaWeixin entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10011", Message = string.Format("此媒体互动参数信息不存在Id:{0}", entity.MediaID) };
            var info = BLL.Interaction.InteractionWeixin.Instance.GetEntity(entity.MediaID);
            if (info == null)
            {
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            retValue.ReturnObject = info;
            return retValue;
        }
    }
}
