using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Android;

namespace XYAuto.ChiTu2018.BO.Android
{
    /// <summary>
    /// 注释：AppPushMsgSwitchLogBO
    /// 作者：lix
    /// 日期：2018/6/5 10:30:45
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppPushMsgSwitchLogBO
    {
        private readonly IAppPushMsgSwitchLog _appPushMsgSwitchLog;
        public AppPushMsgSwitchLogBO()
        {
            _appPushMsgSwitchLog = IocMannager.Instance.Resolve<IAppPushMsgSwitchLog>();
        }

        public bool IsExist(string deviceId, int days)
        {
            return _appPushMsgSwitchLog.IsExist(deviceId, days);

            //return _appPushMsgSwitchLog.Queryable().AsNoTracking().Count(s => s.DeviceId == deviceId &&
            //                                                                       DbFunctions.DiffDays(s.PushShowTime,
            //                                                                           DateTime.Now) >= 0
            //                                                                       &&
            //                                                                       DbFunctions.DiffDays(s.PushShowTime,
            //                                                                           DateTime.Now) <= days) > 0;
        }

        public int InsertFisrt(Entities.Chitunion2017.AppPushMsgSwitchLog entity)
        {
            var info = _appPushMsgSwitchLog.Retrieve(s => s.DeviceId.Equals(entity.DeviceId));
            if (info != null)
            {
                return 0;
            }
            var actionEntity = _appPushMsgSwitchLog.Add(entity);
            return actionEntity != null ? actionEntity.RecId : 0;
        }

        public bool IsClosed(string deviceId, int days)
        {
            var info = _appPushMsgSwitchLog.Queryable().AsNoTracking().Where(s => s.DeviceId == deviceId &&
                                DbFunctions.DiffDays(s.PushShowTime, DateTime.Now) >= 0
                                && DbFunctions.DiffDays(s.PushShowTime, DateTime.Now) <= days).OrderByDescending(s => s.RecId).FirstOrDefault();
            return info != null && info.IsClosed;
        }

        public void UpdateClosed(string deviceId)
        {
            var info = _appPushMsgSwitchLog.Queryable().AsNoTracking().Where(s => s.DeviceId == deviceId).OrderByDescending(s => s.RecId).FirstOrDefault();
            if (info != null)
            {
                _appPushMsgSwitchLog.UpdateClosed(info.RecId, true);
            }
        }
    }
}
