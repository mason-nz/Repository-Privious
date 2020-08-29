using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.DataCenter
{
    public class WorkloadDa : DataBase
    {
        #region 单例
        private WorkloadDa() { }

        static WorkloadDa instance = null;
        static readonly object padlock = new object();

        public static WorkloadDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WorkloadDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 工作量统计列表


        public Tuple<int, DataSet> GetWorkloadList(WorkloadQuery queryArgs, bool IsStatistics)
        {
            string strSmallScore = string.Empty;
            string strBigScore = string.Empty;
            StringBuilder SQL = new StringBuilder($@"
                        SELECT  OperateType ,
                                DI.DictName AS OperateTypeName,
								d.DictName AS ArticleTypeName,
                                PendingCount ,
                                ReceiveCount ,
                                CompletedCount ,
                                UncompletedCount ,
                                RetainCount ,
                                SetToWaist ,
                                CancelledHeadCount,
                                CancelledWaistCount,
                                ReturnHeadCount,
                                ReturnWaistCount,
                                GiveUpHead,
                                GiveUpWaist,
                                InvalidCount ,
                                CancelledCount ,
                                ReturnCount ,
                                BeReturnedCount ,
                                UserName ,
                                Date
                         YanFaFROM   dbo.Stat_WorkloadStatistics AS S
                                LEFT JOIN ( SELECT  *
                                            FROM    dbo.DictInfo
                                            WHERE   DictType = 74
                                          ) AS D ON D.DictId = S.ArticleType
                                LEFT JOIN DictInfo AS DI ON DI.DictId = S.OperateType
                         WHERE  S.Status = 0 
                                AND OperateType in ({GetEnumDescription(queryArgs.Operator)})");
            if (IsStatistics)
            {
                SQL.Append($" and S.UserId=0 AND S.Date = (SELECT MAX(Date) FROM Stat_WorkloadStatistics)");
            }
            else
            {
                SQL.Append($" and S.UserId>0 ");
                if (queryArgs.Operator == (int)WorkEnum.初筛)
                {
                    SQL.Append($" AND (ReceiveCount>0 OR CompletedCount>0 OR RetainCount>0 OR InvalidCount>0 OR SetToWaist>0 ) ");
                }
                if (queryArgs.Operator == (int)WorkEnum.清洗)
                {
                    SQL.Append($" AND (ReceiveCount>0 OR CompletedCount>0 OR RetainCount>0 OR InvalidCount>0 OR SetToWaist>0  OR ReturnCount>0)");
                }
                if (queryArgs.Operator == (int)WorkEnum.封装)
                {
                    SQL.Append($" AND (CompletedCount>0 OR ReturnHeadCount>0 OR ReturnWaistCount>0 OR CancelledHeadCount>0 OR CancelledWaistCount>0 OR GiveUpHead>0 OR GiveUpWaist>0 OR CancelledCount>0) ");
                }
                if (!string.IsNullOrEmpty(queryArgs.UserName))
                {
                    SQL.Append($" AND UserName like '%{queryArgs.UserName}%'");
                }
                if (!string.IsNullOrEmpty(queryArgs.BeginTime))
                {
                    SQL.Append($" AND Date>='{queryArgs.BeginTime}' ");
                }
                if (!string.IsNullOrEmpty(queryArgs.EndTime))
                {
                    SQL.Append($" AND Date<='{queryArgs.EndTime}' ");
                }
            }
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",queryArgs.PageSize),
                new SqlParameter("@CurPage",queryArgs.PageIndex),
                new SqlParameter("@Order"," Date DESC ")
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);
            return new Tuple<int, DataSet>(totalCount, data);
        }
        #endregion


        public string GetEnumDescription(int enumName)
        {
            try
            {
                return Enum.GetValues(typeof(WorkEnum)).OfType<Enum>().Where(m => Convert.ToInt32(m) == enumName).Select(x => new
                {
                    Key = Convert.ToInt32(x),
                    Value = x.ToString(),
                    Description = x.GetDescription()
                }).FirstOrDefault().Description;
            }
            catch
            {
                return enumName.ToString();
            }

        }
    }
}
