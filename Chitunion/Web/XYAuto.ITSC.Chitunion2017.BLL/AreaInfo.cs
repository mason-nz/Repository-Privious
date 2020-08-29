using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class AreaInfo
    {
        public static readonly BLL.AreaInfo Instance = new BLL.AreaInfo();
        protected AreaInfo()
        { }

        public DataTable GetAreaByPid(int pid)
        {
            return Dal.AreaInfo.Instance.GetAreaByPid(pid);
        }


        public DataTable GetAreaByID(int id)
        {
            return Dal.AreaInfo.Instance.GetAreaByID(id);
        }


    }
}
