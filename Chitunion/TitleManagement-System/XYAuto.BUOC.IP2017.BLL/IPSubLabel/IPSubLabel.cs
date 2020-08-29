using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.IPSubLabel
{
    public class IPSubLabel
    {
        public static readonly IPSubLabel Instance = new IPSubLabel();

        public int Insert(Entities.IPSubLabel.IPSubLabel entity)
        {
            return Dal.IPSubLabel.IPSubLabel.Instance.Insert(entity);
        }
    }
}
