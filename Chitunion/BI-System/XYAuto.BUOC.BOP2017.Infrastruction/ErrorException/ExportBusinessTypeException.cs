/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 14:10:23
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;

namespace XYAuto.BUOC.BOP2017.Infrastruction.ErrorException
{
    /// <summary>
    /// 物料分发-导出数据业务类型错误
    /// </summary>
    public class ExportBusinessTypeException : Exception
    {
        public ExportBusinessTypeException(string message = ErrorCodeMap.ExportBusinessTypeExceptionMsg)
            : base(message)
        {
        }
    }
}