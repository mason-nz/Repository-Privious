/********************************************************
*创建人：lixiong
*创建时间：2017/6/5 10:25:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base
{
    public interface IMediaOperate
    {
        ReturnValue Excute();

        ReturnValue VerifyCreateBusiness(ReturnValue retValue);

        ReturnValue VerifyUpdateBusiness(ReturnValue retValue);

        /// <summary>
        /// 校验操作权限
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        ReturnValue VerifyOfRoleModule(ReturnValue retValue);
    }

    public static class InterfaceMediaOperateExtend
    {
        //public static ReturnValue VerifyOfRoleModule(this IMediaOperate mediaOperate, ReturnValue retValue)
        //{
        //    return mediaOperate.VerifyOfRoleModule(retValue);
        //}
    }
}