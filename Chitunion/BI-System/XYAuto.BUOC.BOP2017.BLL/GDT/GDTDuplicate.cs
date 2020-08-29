using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.BLL.GDT
{
    public class GDTDuplicate
    {
        #region Instance
        public static readonly GDTDuplicate Instance = new GDTDuplicate();
        #endregion

        public int Insert(Entities.GDT.GDTDuplicate entity)
        {
            return Dal.GDT.GDTDuplicate.Instance.Insert(entity);
        }
    }
}
