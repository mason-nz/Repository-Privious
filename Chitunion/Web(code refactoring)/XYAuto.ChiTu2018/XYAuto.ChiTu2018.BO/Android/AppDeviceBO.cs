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
    /// 注释：AppDeviceBO
    /// 作者：lix
    /// 日期：2018/5/21 19:04:54
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppDeviceBO
    {
        private readonly IAppDevice _appDevice;

        public AppDeviceBO()
        {
            _appDevice = IocMannager.Instance.Resolve<IAppDevice>();
        }

        public int Insert(Entities.Chitunion2017.App_Device appDevice)
        {
            var entity = _appDevice.Add(appDevice);
            return entity?.RecID ?? 0;
        }

        public Entities.Chitunion2017.App_Device IsExistIsAllowMsgNotice(string emei)
        {
            return _appDevice.Queryable().AsNoTracking().Where(s => s.EMEI == emei).OrderByDescending(s => s.RecID).FirstOrDefault();
        }

        public bool UpdateIsAllowMsgNotice(string emei, bool isOpend)
        {
            var info = IsExistIsAllowMsgNotice(emei);
            if (info != null)
            {
                return _appDevice.UpdateIsAllowMsgNotice(info.RecID, emei, isOpend);
            }
            return false;
        }

        public bool IsExist(int userId)
        {
            return _appDevice.Queryable().AsNoTracking().Count(s => s.UserID == userId) > 0;
        }
    }
}
