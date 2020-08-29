using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.BO
{
    /// <summary>
    /// 注释：DictInfoBO
    /// 作者：lix
    /// 日期：2018/5/18 13:46:31
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class DictInfoBO
    {
        private readonly IDictInfo _dictInfoContext;
        public DictInfoBO()
        {
            _dictInfoContext = IocMannager.Instance.Resolve<IDictInfo>();
        }

        public List<DictInfo> GetList()
        {
            return _dictInfoContext.Queryable().AsNoTracking().Where(s => s.Status == 0).ToList();
        }

        public DictInfo GetInfo(int dicId)
        {
            return _dictInfoContext.Retrieve(s => s.DictId == dicId);
        }

        public bool Exist(int dicId)
        {
            return _dictInfoContext.Retrieve(s => s.DictId == dicId) != null;
        }

        public List<DictInfo> GetList(int dicType)
        {
            return _dictInfoContext.Queryable().AsNoTracking().Where(s => s.Status == 0 && s.DictType == dicType).ToList();
        }
    }
}
