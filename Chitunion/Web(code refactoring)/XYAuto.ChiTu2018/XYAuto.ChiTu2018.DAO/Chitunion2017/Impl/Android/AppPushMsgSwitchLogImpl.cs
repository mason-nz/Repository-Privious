using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Android;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.Android
{
    /// <summary>
    /// 注释：AppPushMsgSwitchLogImpl
    /// 作者：lix
    /// 日期：2018/6/5 10:27:07
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public sealed class AppPushMsgSwitchLogImpl : RepositoryImpl<AppPushMsgSwitchLog>, IAppPushMsgSwitchLog
    {

        public bool IsExist(string deviceId, int days)
        {
            return context.AppPushMsgSwitchLog.Count(s => s.DeviceId == DbFunctions.AsNonUnicode(deviceId) &&
                                                                       DbFunctions.DiffDays(s.PushShowTime,
                                                                           DateTime.Now) >= 0
                                                                       &&
                                                                       DbFunctions.DiffDays(s.PushShowTime,
                                                                           DateTime.Now) <= days) > 0;
        }

        public bool UpdateClosed(int recId, bool isClosed)
        {
            var entity = new AppPushMsgSwitchLog() { RecId = recId, IsClosed = isClosed };
            context.AppPushMsgSwitchLog.Attach(entity);
            var entry = context.Entry(entity);
            entry.Property(e => e.IsClosed).IsModified = true;
            return context.SaveChanges() > 0;
        }
    }
}
