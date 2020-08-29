using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.BO.LE
{
    /// <summary>
    /// 注释：LePromotionChannelDictBO
    /// 作者：lihf
    /// 日期：2018/5/14 10:28:45
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LePromotionChannelDictBO
    {
        private static ILePromotionChannelDict LePromotionChannelDict()
        {
            return IocMannager.Instance.Resolve<ILePromotionChannelDict>();
        }

        public List<LE_PromotionChannel_Dict> GetList(Expression<Func<LE_PromotionChannel_Dict, bool>> expression)
        {
            return LePromotionChannelDict().Queryable().Where(expression).ToList();
        }

        public LE_PromotionChannel_Dict GetModel(Expression<Func<LE_PromotionChannel_Dict, bool>> expression)
        {
            return LePromotionChannelDict().Retrieve(expression);
        }

        /// <summary>
        /// 根据渠道编码获取渠道ID
        /// </summary>
        /// <param name="channelCode"></param>
        /// <returns></returns>
        public long GetChanneIdByChanneCode(string channelCode)
        {
            var model = LePromotionChannelDict().Retrieve(x => x.ChannelCode == channelCode);
            return model?.DictID ?? 0;
        }
    }
}
