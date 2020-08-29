using System.Collections.Generic;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017
{
    /// <summary>
    /// 注释：业务接口
    /// 作者：guansl
    /// 日期：2018/5/7 20:20:53
    /// </summary>
    public interface IAccountArticle : Repository<AccountArticle>
    {
        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        AccountArticle AccountArticleByPK(int id);
        /// <summary>
        /// 分頁獲取數據
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<AccountArticle> GetAccountArticleList(int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 更新(sql)
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        int PutAccountArticle(int? accountID, string content = "test");
        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="brandIDs"></param>
        /// <returns></returns>
        int DeleteAccountArticle(IEnumerable<int> brandIDs);
    }
}
