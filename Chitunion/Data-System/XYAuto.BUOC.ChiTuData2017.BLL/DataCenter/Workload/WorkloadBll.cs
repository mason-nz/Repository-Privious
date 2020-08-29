using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.Dal.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Workload;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Workload
{
    public class WorkloadBll
    {
        #region 初始化
        public WorkloadBll()
        {
        }
        static WorkloadBll instance = null;
        static readonly object padlock = new object();
        static int ExcelCount = DataCommon.GetAppSettingInt32Value("ExcelQuantity");
        public static WorkloadBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WorkloadBll();
                        }
                    }
                }
                return instance;
            }
        }


        #endregion

        #region 工作量统计列表

        /// <summary>
        /// 根据参数获取工作量统计列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public dynamic GetWorkloadList(WorkloadQuery queryArgs)
        {
            var queryData = WorkloadDa.Instance.GetWorkloadList(queryArgs, false);

            //WorkloadStatisticsInfo workStatisticsInfo = new WorkloadStatisticsInfo();
            if (queryData.Item2 != null && queryData.Item2.Tables[0].Rows.Count > 0)
            {
                DataTable workTable = queryData.Item2.Tables[0];
                var workList = DataCommon.DataTableToList<WorkloadListInfo>(workTable);

                var StatisticsDataSet = WorkloadDa.Instance.GetWorkloadList(new WorkloadQuery{ Operator = queryArgs.Operator }, true);
               

                var StatisticsList = DataCommon.DataTableToList<WorkloadListInfo>(StatisticsDataSet.Item2.Tables[0]);

                //获取列表头 统计信息
                var workStatisticsInfo = (from q in StatisticsList
                                          group q by q.OperateType into t
                                          select new WorkloadStatisticsInfo
                                          {
                                              PendingCountTotal = t.Select(x => x.PendingCount).First(),
                                              OperateTypeName = t.Select(x => x.OperateTypeName).First(),
                                              ReceiveCountTotal = t.Select(x => x.ReceiveCount).First(),
                                              CompletedCountTotal = t.Select(x => x.CompletedCount).First(),
                                              UncompletedCountTotal = t.Select(x => x.UncompletedCount).First(),
                                              RetainCountTotal = t.Select(x => x.RetainCount).First(),
                                              ReturnCountTotal = t.Select(x => x.ReturnCount).First(),
                                              InvalidCountTotal = t.Select(x => x.InvalidCount).First(),
                                              CancelledCountTotal = t.Select(x => x.CancelledCount).First(),
                                              BeReturnedCountTotal = t.Select(x => x.BeReturnedCount).First(),
                                              SetToWaistTotal = t.Select(x => x.SetToWaist).First(),
                                              CancelledHeadCountTotal = t.Select(x => x.CancelledHeadCount).First(),
                                              CancelledWaistCountTotal = t.Select(x => x.CancelledWaistCount).First(),
                                              ReturnHeadCountTotal = t.Select(x => x.ReturnHeadCount).First(),
                                              ReturnWaistCountTotal = t.Select(x => x.ReturnWaistCount).First(),
                                              GiveUpHeadTotal = t.Select(x => x.GiveUpHead).First(),
                                              GiveUpWaistTotal = t.Select(x => x.GiveUpWaist).First(),
                                              StatisticsDate =t.Select(x=>x.Date).First()
                                          }).ToList();

                //workStatisticsInfo.List = workList;
                //dynamicInfo.List = workTable;//列表数据
                //dynamicInfo.TotalCount = queryData.Item1;
                //dynamicInfo.ReceiveCountTotal = query.领取数;
                //dynamicInfo.CompletedCountTotal = query.完成数;
                //dynamicInfo.UncompletedCountTotal = query.未完成数;
                //dynamicInfo.RetainCountTotal = query.保留数;
                //dynamicInfo.SetToWaistTotal = query.置为腰数;
                //dynamicInfo.InvalidCountTotal = query.作废数;
                //dynamicInfo.CancelledCountTotal = query.被作废数;
                //dynamicInfo.ReturnCountTotal = query.退回数;
                //dynamicInfo.BeReturnedCountTotal = query.被退回数;
                //dynamicInfo.PendingCountTotal = query.待处理数;
                //dynamicInfo.CancelledHeadCountTotal = query.作废头数;
                //dynamicInfo.CancelledWaistCountTotal = query.作废腰数;
                //dynamicInfo.ReturnHeadCountTotal = query.退回头数;
                //dynamicInfo.ReturnWaistCountTotal = query.退回腰数;
                //dynamicInfo.GiveUpHeadTotal = query.放弃头数;
                //dynamicInfo.GiveUpWaistTotal = query.放弃腰数;

                return new { List = workList, TitleList = workStatisticsInfo, TotalCount = queryData.Item1 };

            }
            return new { List = new List<string>(), TitleList = new List<string>(), TotalCount = queryData.Item1 };
        }
        #endregion

        #region 工作量统计导出

        public ResultExcelInfo WorkloadStatisticsExport(WorkloadQuery queryArgs)
        {
            Tuple<int, DataSet> ExcelTuple = WorkloadDa.Instance.GetWorkloadList(queryArgs, false);
            Dictionary<string, string> dictSieve = null;
            if (queryArgs.Operator == (int)WorkEnum.初筛 || queryArgs.Operator == (int)WorkEnum.清洗)
            {
                dictSieve = new Dictionary<string, string> {
                { "Date","日期"},
                { "UserName","操作人姓名"},
                { "ArticleTypeName","文章类型"},
                { "ReceiveCount","领取数"},
                { "UncompletedCount","未完成数"},
                { "CompletedCount","已完成数"},
                { "RetainCount","保留数"},
                { "InvalidCount","作废数"},
                { "SetToWaist","置为腰"},
                { "ReturnCount","退回数"},
                { "CancelledCount","被作废数"},
                { "BeReturnedCount","被退回数"}};
                if (queryArgs.Operator == (int)WorkEnum.初筛)
                {
                    dictSieve.Remove("ReturnCount");
                }
            }
            if (queryArgs.Operator == (int)WorkEnum.封装)
            {
                dictSieve = new Dictionary<string, string> {
                { "Date","日期"},
                { "UserName","操作人姓名"},
                { "CompletedCount","已完成(物料)"},
                { "ReturnHeadCount","退回数（头）"},
                { "ReturnWaistCount","退回数（腰）"},
                { "CancelledHeadCount","作废数（头）"},
                { "CancelledWaistCount","作废数（腰）"},
                { "GiveUpHead","放弃（头）"},
                { "GiveUpWaist","放弃（腰）"},
                { "CancelledCount","被作废数（物料）"} };
            };
            string excelUrl = string.Empty;
            var queryTable = ExcelTuple.Item2.Tables[0];
            if (queryTable.Rows.Count > 0)
            {
                if (queryTable.Rows.Count > 100000)
                {
                    return new ResultExcelInfo { Status = -1, Url = null };
                }
                excelUrl = ExcelHelper.ExportEcel(queryTable, "工作量统计", dictSieve);

            }
            return new ResultExcelInfo { Status = 1, Url = excelUrl };

        }

        #endregion

        /// <summary>
        /// 工作量导出  输出流
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public Tuple<MemoryStream, string, bool> DataStreamExel(WorkloadQuery queryArgs)
        {
            Tuple<int, DataSet> ExcelTuple = WorkloadDa.Instance.GetWorkloadList(queryArgs, false);
            Dictionary<string, string> dictSieve = null;

            if (queryArgs.Operator == (int)WorkEnum.初筛 || queryArgs.Operator == (int)WorkEnum.清洗)
            {
                dictSieve = new Dictionary<string, string> {
                { "Date","日期"},
                { "UserName","操作人姓名"},
                { "ArticleTypeName","文章类型"},
                { "ReceiveCount","领取数"},
                { "UncompletedCount","未完成数"},
                { "CompletedCount","已完成数"},
                { "RetainCount","保留数"},
                { "InvalidCount","作废数"},
                { "SetToWaist","置为腰"},
                { "ReturnCount","退回数"},
                { "CancelledCount","被作废数"},
                { "BeReturnedCount","被退回数"}};
                if (queryArgs.Operator == (int)WorkEnum.初筛)
                {
                    dictSieve.Remove("ReturnCount");
                }
            }
            if (queryArgs.Operator == (int)WorkEnum.封装)
            {
                dictSieve = new Dictionary<string, string> {
                { "Date","日期"},
                { "UserName","操作人姓名"},
                { "CompletedCount","已完成(物料)"},
                { "ReturnHeadCount","退回数（头）"},
                { "ReturnWaistCount","退回数（腰）"},
                { "CancelledHeadCount","作废数（头）"},
                { "CancelledWaistCount","作废数（腰）"},
                { "GiveUpHead","放弃（头）"},
                { "GiveUpWaist","放弃（腰）"},
                { "CancelledCount","被作废数（物料）"} };
            };
            var queryTable = ExcelTuple.Item2.Tables[0];
            if (queryTable.Rows.Count > 0)
            {
                if (queryTable.Rows.Count > ExcelCount)
                {
                    return new Tuple<MemoryStream, string, bool>(null, string.Empty, false);

                }

                return new Tuple<MemoryStream, string, bool>(ExcelHelper.Export(queryTable, dictSieve), "工作量统计", true);
            }
            return null;
        }

    }
}