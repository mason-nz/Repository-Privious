using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify
{
    public class VerifyOfPcAppOperate
    {
        /// <summary>
        /// 获取用户信息的规则方法
        /// </summary>
        /// <param name="entity">需要验证的实体</param>
        /// <param name="isDefaultRule">是否是默认的全部规则</param>
        /// <param name="funcDic">传入需要验证的规则</param>
        /// <param name="isOutParam">是否需要返回值</param>
        /// <returns></returns>
        public ReturnValue Verify(Entities.Media.MediaPcApp entity, bool isDefaultRule, Dictionary<int, Func<Entities.Media.MediaPcApp, ReturnValue>> funcDic,
             bool isOutParam = false)
        {
            //自定义规则
            if (!isDefaultRule) new Verifiction<Entities.Media.MediaPcApp>(funcDic).Verify(entity, isOutParam);
            //默认规则
            funcDic = new Dictionary<int, Func<Entities.Media.MediaPcApp, ReturnValue>>
            {
                //{(int)  VerifyDataTypeEnum.VerifyOfWeiBoById, CheckWeiBoById},
                //{(int)  VerifyDataTypeEnum.VerifyOfInteractionWeiBoByMediaId, CheckInteractionWeiBoByMediaId}
            };
            return new Verifiction<Entities.Media.MediaPcApp>(funcDic).Verify(entity, isOutParam);
        }

        public static ReturnValue CheckPcAppNumber(Entities.Media.MediaPcApp entity)
        {
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "10020", Message = string.Format("此媒体名称已存在") };
            var info = BLL.Media.MediaPCAPP.Instance.GetEntity(entity.Name, entity.MediaID);
            if (info != null)
            {
                //if (info.CreateUserID != entity.CreateUserID)//此媒体是自己的，则不算，跳过
                retValue.ReturnObject = info;
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }
    }
}
