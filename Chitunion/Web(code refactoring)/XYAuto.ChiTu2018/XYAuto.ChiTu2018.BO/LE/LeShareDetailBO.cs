using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;

namespace XYAuto.ChiTu2018.BO.LE
{
    /// <summary>
    /// 注释：LeShareDetailBO
    /// 作者：lix
    /// 日期：2018/5/22 14:41:47
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeShareDetailBO
    {
        private readonly ILeShareDetail _leShareDetail;
        public LeShareDetailBO()
        {
            _leShareDetail = IocMannager.Instance.Resolve<ILeShareDetail>();
        }

        public int Insert(LE_ShareDetail entity)
        {
            var retEntity = _leShareDetail.Add(entity);
            return retEntity?.RecID ?? 0;
        }

        public int GetShareDetailCount(int userId)
        {
            return _leShareDetail.Queryable().Where(p => p.Type == 202004 && p.CreateUserID == userId).Count();
        }

        public bool IsExist(int userId, int type)
        {
            var count = _leShareDetail.Queryable().AsNoTracking().Count(s => s.CreateUserID == userId && s.Type == type);
            return count > 0;
        }

        public bool IsExistWithdrawas(int userId)
        {
            var count = _leShareDetail.Queryable().AsNoTracking().Count(s => s.CreateUserID == userId && s.Type == (int)LeShareDetailTypeEnum.提现分享
                            && DbFunctions.DiffDays(s.CreateTime, DateTime.Now) == 0);
            return count > 0;
        }
    }
}
