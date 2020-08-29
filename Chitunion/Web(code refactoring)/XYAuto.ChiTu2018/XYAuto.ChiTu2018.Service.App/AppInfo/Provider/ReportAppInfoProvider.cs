using XYAuto.ChiTu2018.BO.Android;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Provider
{
    /// <summary>
    /// 注释：ReportAppInfoProvider
    /// 作者：lix
    /// 日期：2018/5/21 18:48:04
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReportAppInfoProvider : VerifyOperateBase
    {
        private readonly ReqReportAppDto _reqReportApp;

        public ReportAppInfoProvider(ReqReportAppDto reqReportApp)
        {
            _reqReportApp = reqReportApp;
        }

        /// <summary>
        /// android app 设备上报
        /// </summary>
        /// <returns></returns>
        public ReturnValue Report()
        {
            var retValue = new ReturnValue();
            var entity = AutoMapper.Mapper.Map<ReqReportAppDto, Entities.Chitunion2017.App_Device>(_reqReportApp);

            var excuteId = new AppDeviceBO().Insert(entity);

            return excuteId <= 0 ? CreateFailMessage(retValue, "10001", "入库失败") : retValue;
        }
    }
}
