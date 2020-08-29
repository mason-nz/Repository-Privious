/********************************************************
*创建人：hant
*创建时间：2017/12/22 10:31:05 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu
{
    public class DataStatisticsByMonth
    {
        private const string UpladFilesPath = "/UploadFiles/";
        private readonly string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
        private readonly string _todayDate = DateTime.Now.ToString("yyyy-MM-dd");

        public static readonly DataStatisticsByMonth Instance = new DataStatisticsByMonth();

        /// <summary>
        /// 返回结算信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResponseSummaryByMonth GetMonthStatistics(RequestSummaryByMonth req)
        {
            //if (RolesVerification.Instance.IsAllData() && !(RolesVerification.Instance.IsViewData()))
            //{
            //    req.ChannelID = BLL.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID);
            //}
            ResponseSummaryByMonth model = new ResponseSummaryByMonth();
            List<SummaryByMonth> listsummary = new List<SummaryByMonth>();
            int totalCount = 0;
            List<Entities.Chitu.DataStatisticsByMonth> list = Dal.Chitu.DataStatisticsByMonth.Instance.GetMonthStatistics(QueryWhere(req), req.PageIndex, req.PageSize, out totalCount);
            model.TotalCount = totalCount;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    SummaryByMonth entity = new SummaryByMonth();
                    entity.ChannelID = item.ChannelID;
                    entity.ChannelName = item.ChannelName;
                    entity.CreateTime = item.CreateTime;
                    entity.Date = item.Date.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    entity.FirstDay = FirstDayOfMonth(item.Date).ToString("yyyy-MM-dd");
                    entity.EndDay = LastDayOfMonth(item.Date).ToString("yyyy-MM-dd");
                    entity.OperaterName = item.OperaterName;
                    entity.OrderNumber = item.OrderNumber;
                    entity.RecID = item.RecID;
                    entity.StateOfSettlement = item.StateOfSettlement;
                    entity.TimeOfSettlement = item.TimeOfSettlement;
                    entity.TotalAmount = item.TotalAmount;
                    listsummary.Add(entity);
                }
            }
            model.List = listsummary;
            return model;
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        private DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /**//// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        private DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private string QueryWhere(RequestSummaryByMonth req)
        {
            StringBuilder sWhere = new StringBuilder();
            if (req.BeginTime != null && req.BeginTime.ToString().Length > 0)
            {
                sWhere.AppendFormat(" AND [Date]>='{0}'",req.BeginTime);
            }
            if (req.EndTime != null && req.EndTime.ToString().Length > 0)
            {
                sWhere.AppendFormat(" AND [Date]<='{0}'", req.EndTime);
            }
            if (req.ChannelID > 0)
            {
                sWhere.AppendFormat(" AND DM.[ChannelID]='{0}'", req.ChannelID);
            }
            if (req.StateOfSettlement > 0)
            {
                sWhere.AppendFormat(" AND [StateOfSettlement]='{0}'", req.StateOfSettlement);
            }
            return sWhere.ToString();
        }



        public string Export(RequestSummaryByMonth req)
        {
            List<SummaryByMonth> list = new List<SummaryByMonth>();

            List<SummaryByMonthOperater> listoper = new List<SummaryByMonthOperater>();

            DataSet ds = new DataSet();
            var fileName = $"结算信息下载-月结信息.xlsx";
            var dicFilePath = GetStatPath(fileName);
            if (RolesVerification.Instance.IsAllData())
            {
                list = DataTableToList<SummaryByMonth>(GetExcel(req).Tables[0]);
                if (list.Count > 0)
                {
                    new ExcelHelper<SummaryByMonth>().SaveExcelToFile(list, dicFilePath.Item1, fileName);
                }
                else
                {
                    return "暂无数据";
                }
            }
            else
            {
                listoper = DataTableToList<SummaryByMonthOperater>(GetExcel(req).Tables[0]);
                if(listoper.Count >0)
                {
                    new ExcelHelper<SummaryByMonthOperater>().SaveExcelToFile(listoper, dicFilePath.Item1, fileName);
                }
                else
                {
                    return "暂无数据";
                }

            }
            var httpUrl = dicFilePath.Item2;
            return httpUrl;
        }


        /// <summary>
        /// 生成文件名,item1:物理path ,item2:生成后的url路径
        /// </summary>
        /// <param name="fileName">需要定义的文件名称：111.png</param>
        /// <param name="statisticsDataTypeEnum"></param>
        /// <returns>item1:物理path ,item2:生成后的url路径</returns>
        public Tuple<string, string> GetStatPath(string fileName)
        {
            //UploadLoad
            string relatedPath = $"{UpladFilesPath}ExportExcel/DataStatistics/{_todayDate}/";
            var webFilePath = relatedPath + fileName;
            string dir = _uploadFilePath + relatedPath.Replace(@"/", "\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new Tuple<string, string>(dir, webFilePath);
        }

        private DataSet GetExcel(RequestSummaryByMonth req)
        {
            string swhere = QueryWhere(req);
            DataSet ds = Dal.Chitu.DataStatisticsByMonth.Instance.GetMonthStatisticsExcel(swhere);
            return ds;
        }


        private List<T> DataTableToList<T>(DataTable table) //where T : EntityBase, new()
        {
            List<T> list = new List<T>();
            if (table != null && table.Rows != null && table.Rows.Count > 0)
            {
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    //创建泛型对象
                    T entity = Activator.CreateInstance<T>();
                    //属性和名称相同时则赋值
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        var property = entity.GetType().GetProperty(table.Columns[j].ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                        if (property != null && table.Rows[i][j] != DBNull.Value)
                        {
                            property.SetValue(entity, table.Rows[i][j], null);
                        }
                    }
                    list.Add(entity);
                }
            }
            return list;
        }

    }
}
