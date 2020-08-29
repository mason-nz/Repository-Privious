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
    /// 注释：IAppDevice
    /// 作者：lix
    /// 日期：2018/5/21 19:01:42
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public interface IAppDevice : Repository<App_Device>
    {
        /// <summary>
        /// 更改AppDevice 里面的开关，毕竟是根源
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="emei"></param>
        /// <param name="isAllowMsgNotice"></param>
        /// <returns></returns>
        bool UpdateIsAllowMsgNotice(int recId, string emei, bool isAllowMsgNotice);
    }
}
