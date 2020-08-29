using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.ChituMedia
{
    public class WithdrawalsDa : DataBase
    {
        #region 单例
        private WithdrawalsDa() { }

        static WithdrawalsDa instance = null;
        static readonly object padlock = new object();

        public static WithdrawalsDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WithdrawalsDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion


        /// <summary>
        /// 获取收益统计列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, List<WithdrawalsStatisticsModel>, WithdrawalsTitle> GetWithdrawalsStatisticsList(QueryWithdrawalsArgs query, string orderText)
        {
            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@PageIndex",query.PageIndex),
                new SqlParameter("@PageSize",query.PageSize),
                new SqlParameter("@OrderBy",orderText),                
                new SqlParameter("@Keyword",query.Keyword)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LE_WithdrawalsStatisticsList", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);

            return new Tuple<int, List<WithdrawalsStatisticsModel>, WithdrawalsTitle>(totalCount, DataTableToList<WithdrawalsStatisticsModel>(data.Tables[1]), DataTableToEntity<WithdrawalsTitle>(data.Tables[0]));

        }

        /// <summary>
        /// 根据用户ID获取收益明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, List<IncomeDetailModel>, IncomeDetailTitle> GetIncomeDetailModelList(QueryWithdrawalsArgs query)
        {
            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@PageIndex",query.PageIndex),
                new SqlParameter("@PageSize",query.PageSize),
                new SqlParameter("@IncomeBeginTime",query.IncomeBeginTime),
                new SqlParameter("@IncomeEndTime",query.IncomeEndTime),
                new SqlParameter("@CategoryID",query.CategoryID),
                new SqlParameter("@UserID",query.UserID),
                new SqlParameter("@Keyword",query.Keyword)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LE_IncomeDetail", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);

            return new Tuple<int, List<IncomeDetailModel>, IncomeDetailTitle>(totalCount, DataTableToList<IncomeDetailModel>(data.Tables[1]), DataTableToEntity<IncomeDetailTitle>(data.Tables[0]));

        }
    }
}
