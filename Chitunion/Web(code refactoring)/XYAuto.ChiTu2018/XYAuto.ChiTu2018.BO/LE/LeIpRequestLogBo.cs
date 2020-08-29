/********************************
* 项目名称 ：XYAuto.ChiTu2018.BO.LE
* 类 名 称 ：LeIpRequestLogBo
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/5 11:03:08
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.BO.LE
{
    public class LeIpRequestLogBo
    {
        public LE_IP_RequestLog AddIpRequestLogInfo(LE_IP_RequestLog entity)
        {
            return IocMannager.Instance.Resolve<ILeIpRequestLog>().Add(entity);
        }
    }
}
