using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.SonIPLabel
{
    public class SonIPLabel
    {
        public static readonly SonIPLabel Instance = new SonIPLabel();

        public int Insert(Entities.SonIPLabel.SonIPLabel entity)
        {
            return Dal.SonIPLabel.SonIPLabel.Instance.Insert(entity);
        }
    }
}
