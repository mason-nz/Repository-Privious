using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.BO.Task
{
    /// <summary>
    /// 注释：DictSceneBO
    /// 作者：lihf
    /// 日期：2018/5/14 15:42:31
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class DictSceneBO
    {
        private static IDictScene DictScene()
        {
            return IocMannager.Instance.Resolve<IDictScene>();
        }

        public List<DictScene> GetList(Expression<Func<DictScene, bool>> expression)
        {
            return DictScene().Queryable().Where(expression).ToList();
        }

        public List<DictScene> GetListByParentId()
        {
            return DictScene().Queryable().Where(x => x.ParenID > 0).ToList();
        }
    }
}
