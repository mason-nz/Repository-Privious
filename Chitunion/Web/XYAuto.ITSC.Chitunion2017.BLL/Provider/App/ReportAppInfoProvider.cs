using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request;

namespace XYAuto.ITSC.Chitunion2017.BLL.Provider.App
{
    public class ReportAppInfoProvider : VerifyOperateBase
    {
        private readonly ReqReportAppDto _reqReportApp;

        public ReportAppInfoProvider(ReqReportAppDto reqReportApp)
        {
            _reqReportApp = reqReportApp;
        }

        public ReturnValue Report()
        {
            var retValue = new ReturnValue();
            var entity = AutoMapper.Mapper.Map<ReqReportAppDto, Entities.LETask.AppDevice>(_reqReportApp);

            var excuteId = Dal.LETask.AppDevice.Instance.Insert(entity);

            if (excuteId <= 0)
            {
                return CreateFailMessage(retValue, "10001", "入库失败");
            }

            return retValue;
        }


    }
}
