using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Extend.User;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.User;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Response.User;

namespace XYAuto.ChiTu2018.Service.App.PublicService
{
    /// <summary>
    /// 注释：PsWxUserAuthService
    /// 作者：lix
    /// 日期：2018/6/8 12:01:41
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class PsWxUserAuthService : VerifyOperateBase
    {
        #region 单例

        private PsWxUserAuthService() { }
        private static readonly Lazy<PsWxUserAuthService> Linstance = new Lazy<PsWxUserAuthService>(() => { return new PsWxUserAuthService(); });

        public static PsWxUserAuthService Instance => Linstance.Value;

        #endregion

        public ReturnValue WeiXinUserOperation(PsReqPostWxUserOperationDto entity)
        {
            Entities.Extend.User.WeiXinUserOperateDo wxUser = null;
            var retValue = VerifyOfNecessaryParameters(entity);
            if (retValue.HasError)
                return retValue;
            try
            {
                wxUser = CreateWeiXinUserOperateDo(entity);
                var retTp = new LEWeiXinUserBO().WeiXinUserOperation(wxUser);
                if (!retTp.Item1)
                {
                    return CreateFailMessage(retValue, "1001", "操作失败");
                }
                retValue.ReturnObject = new PsRespUserOpeationDto { UserId = retTp.Item2, OpenId = entity.openid };
            }
            catch (Exception exception)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error($"WeiXinUserOperation 操作失败，参数:{JsonConvert.SerializeObject(wxUser)}" +
                                                              $"错误信息:{exception.Message}");
            }

            return retValue;
        }

        private Entities.Extend.User.WeiXinUserOperateDo CreateWeiXinUserOperateDo(PsReqPostWxUserOperationDto entity)
        {
            return AutoMapper.Mapper.Map<PsReqPostWxUserOperationDto, Entities.Extend.User.WeiXinUserOperateDo>(entity);
        }
    }
}
