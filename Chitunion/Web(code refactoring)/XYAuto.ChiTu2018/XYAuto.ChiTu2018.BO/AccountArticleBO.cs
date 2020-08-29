using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Chitunion2017;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Impl;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.BO
{
    /// <summary>
    /// 注释：通用业务类
    ///       包括缓存、第三方平台的封装，多个DAO组合
    /// 作者：guansl
    /// 日期：2018/4/19 
    /// </summary>
    public class AccountArticleBO
    {
        #region ioc
        //会在程序执行前初始化
        private static readonly UnityContainer Container = new UnityContainer();

        private static IAccountArticle AccountArticle()
        {
            //注入
            Container.RegisterType<IAccountArticle, AccountArticleImpl>();
            //反转
            return Container.Resolve<IAccountArticle>();
        }
        #endregion

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">主键编码</param>
        /// <returns></returns>
        public AccountArticle AccountArticleByPK(int id)
        {
            return AccountArticle().AccountArticleByPK(id);
        }


        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">条数</param>
        /// <param name="count">总记录数</param>
        /// <returns></returns>
        public IEnumerable<AccountArticle> GetAccountArticleList(int pageIndex, int pageSize, out int count)
        {
            return AccountArticle().GetAccountArticleList(pageIndex, pageSize, out count);
        }
    }
}
