using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Android;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.Android
{
    /// <summary>
    /// 注释：AppDeviceImpl
    /// 作者：lix
    /// 日期：2018/5/21 19:02:21
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppDeviceImpl : RepositoryImpl<App_Device>, IAppDevice
    {
        public bool UpdateIsAllowMsgNotice(int recId, string emei, bool isAllowMsgNotice)
        {
            //var info = context.App_Device.Where(s => s.EMEI.Equals(emei)).OrderByDescending(s => s.RecID).FirstOrDefault();
            //if (info != null)
            //{
            //    var entity = new App_Device() { RecID = info.RecID, EMEI = emei, IsAllowMsgNotice = isAllowMsgNotice };
            //    context.App_Device.Attach(entity);
            //    var entry = context.Entry(entity);
            //    entry.Property(e => e.IsAllowMsgNotice).IsModified = true;
            //    return context.SaveChanges() > 0;
            //}

            var entity = new App_Device() { RecID = recId, EMEI = emei, IsAllowMsgNotice = isAllowMsgNotice };
            context.App_Device.Attach(entity);
            var entry = context.Entry(entity);
            entry.Property(e => e.IsAllowMsgNotice).IsModified = true;
            return context.SaveChanges() > 0;
        }
    }
}
