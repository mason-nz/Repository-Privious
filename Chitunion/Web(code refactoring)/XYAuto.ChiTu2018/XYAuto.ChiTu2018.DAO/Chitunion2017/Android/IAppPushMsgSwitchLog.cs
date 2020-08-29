using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Android
{
    /// <summary>
    /// 注释：IAppPushMsgSwitchLog
    /// 作者：lix
    /// 日期：2018/6/5 10:27:23
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public interface IAppPushMsgSwitchLog : Repository<AppPushMsgSwitchLog>
    {
        bool IsExist(string deviceId, int days);

        bool UpdateClosed(int recId, bool isClosed);
    }
}
