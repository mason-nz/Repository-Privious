using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl
{
    /// <summary>
    /// 注释：业务实现类，不能被继承
    /// 作者：guansl
    /// 日期：2018/5/7 20:20:53
    /// </summary>
    public sealed class AccountArticleImpl : RepositoryImpl<AccountArticle>, IAccountArticle
    {
        public AccountArticle AccountArticleByPK(int id)
        {
            return Retrieve(w => w.RecID == id);
        }

        public IEnumerable<AccountArticle> GetAccountArticleList(int pageIndex, int pageSize, out int count)
        {
            return FindAll(w => 1 == 1, o => new { o.CreateTime }, SortOrder.Descending, pageIndex * pageSize, pageSize, out count);
        }

        public int PutAccountArticle(int? accountID, string content = "test")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("update AccountArticle set");
            if (accountID.HasValue)
            {
                sb.AppendFormat(" CleanContent = @content");
            }
            else
            {
                return 0;
            }
            sb.AppendFormat(" where accountID=@accountID");

            SqlParameter[] paras = new SqlParameter[] {
                 new SqlParameter("@accountID",accountID),
                 new SqlParameter("@content",content)
                };

            return context.Database.ExecuteSqlCommand(sb.ToString(), paras);
        }

        public int DeleteAccountArticle(IEnumerable<int> recIDs)
        {
            const string deleteSql = "update AccountArticle set Status=1 where RecID in ({0})";
            string sqlCmd = string.Format(deleteSql, string.Join(",", recIDs));
            return context.Database.ExecuteSqlCommand(sqlCmd, "");
        }
    }
}
