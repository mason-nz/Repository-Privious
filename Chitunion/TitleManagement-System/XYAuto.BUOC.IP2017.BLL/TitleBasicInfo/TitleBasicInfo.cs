using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.TitleBasicInfo
{
    public class TitleBasicInfo
    {
        public readonly static TitleBasicInfo Instance = new TitleBasicInfo();
        public Entities.TitleBasicInfo.TitleBasicInfo GetModelByTypeAndName(int type, string name)
        {
            return Dal.TitleBasicInfo.TitleBasicInfo.Instance.GetModelByTypeAndName(type, name);
        }
        public List<Entities.TitleBasicInfo.TitleBasicInfo> SelectAll()
        {
            return Dal.TitleBasicInfo.TitleBasicInfo.Instance.SelectAll();
        }
    }
}
