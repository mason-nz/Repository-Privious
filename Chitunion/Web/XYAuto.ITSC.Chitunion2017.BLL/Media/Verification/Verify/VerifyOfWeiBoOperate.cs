using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Enums;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify
{
    public class VerifyOfWeiBoOperate
    {
        /// <summary>
        /// 获取用户信息的规则方法
        /// </summary>
        /// <param name="entity">需要验证的实体</param>
        /// <param name="isDefaultRule">是否是默认的全部规则</param>
        /// <param name="funcDic">传入需要验证的规则</param>
        /// <param name="isOutParam">是否需要返回值</param>
        /// <returns></returns>
        public ReturnValue Verify(Entities.Media.MediaWeibo entity, bool isDefaultRule, Dictionary<int, Func<Entities.Media.MediaWeibo, ReturnValue>> funcDic,
            bool isOutParam = false)
        {
            //自定义规则
            if (!isDefaultRule) new Verifiction<Entities.Media.MediaWeibo>(funcDic).Verify(entity, isOutParam);
            //默认规则
            funcDic = new Dictionary<int, Func<Entities.Media.MediaWeibo, ReturnValue>>
            {
                {(int)  VerifyDataTypeEnum.VerifyOfWeiBoById, CheckWeiBoById},
                {(int)  VerifyDataTypeEnum.VerifyOfInteractionWeiBoByMediaId, CheckInteractionWeiBoByMediaId}
            };
            return new Verifiction<Entities.Media.MediaWeibo>(funcDic).Verify(entity, isOutParam);
        }

        public static ReturnValue CheckWeiBoById(Entities.Media.MediaWeibo entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10012", Message = string.Format("此媒体信息不存在Id:{0}", entity.MediaID) };
            var info = BLL.Media.MediaWeibo.Instance.GetEntity(entity.MediaID);
            if (info != null)
            {
                retValue.ReturnObject = info;
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        public static ReturnValue CheckWeiboNumber(Entities.Media.MediaWeibo entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10020", Message = string.Format("此微博帐号已存在") };
            var info = BLL.Media.MediaWeibo.Instance.GetEntity(entity.Number, entity.MediaID);
            if (info != null)
            {
                retValue.ReturnObject = info;
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;

            return retValue;
        }

        public static ReturnValue CheckWeiboName(Entities.Media.MediaWeibo entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10020", Message = string.Format("此微博帐号名称已存在") };
            var info = BLL.Media.MediaWeibo.Instance.GetEntityByName(entity.Name, entity.MediaID);
            if (info != null)
            {
                retValue.ReturnObject = info;
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;

            return retValue;
        }

        public static ReturnValue CheckInteractionWeiBoByMediaId(Entities.Media.MediaWeibo entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10013", Message = string.Format("此媒体互动参数信息不存在Id:{0}", entity.MediaID) };
            var info = BLL.Interaction.InteractionWeibo.Instance.GetEntity(entity.MediaID);
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
