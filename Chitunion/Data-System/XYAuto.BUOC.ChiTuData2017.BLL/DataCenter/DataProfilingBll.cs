using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Dal.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter
{
    public class DataProfilingBll
    {
        #region 初始化
        static DataProfilingBll instance = null;
        static readonly object padlock = new object();
        public DataProfilingBll()
        {
        }
        public static DataProfilingBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DataProfilingBll();
                        }
                    }
                }
                return instance;
            }
        }


        #endregion

        #region 数据概况—物料封装
        /// <summary>
        /// 数据概况—物料封装
        /// </summary>
        /// <param name="latelyDays"></param>
        /// <returns></returns>
        public dynamic GetEncapsulateGather(int latelyDays)
        {
            BasicQueryArgs entity = new BasicQueryArgs { DateType = latelyDays };

            dynamic EncapsulateGather = new ExpandoObject();

            var PieData = EncapsulateMaterialDa.Instance.GetEncapsulateStatistics_Pie(entity);

            var BarData = EncapsulateMaterialDa.Instance.GetEncapsulateStatistics_Bar(entity);

            dynamic Encapsulate_HeadPie = null;//饼图
            dynamic Encapsulate_HeadBar = null;//柱状图

            //封装标题时间
            EncapsulateGather.Date = new { BeginTime = entity.BeginTime, EndTime = entity.EndTime };
            if (PieData != null && PieData.Tables.Count > 0)
            {
                Encapsulate_HeadPie = new ExpandoObject();
                var tablePie = PieData.Tables[0];

                //图形Info
                EncapsulateGather.Info = from t in tablePie.AsEnumerable()
                                         select new
                                         {
                                             Name = t.Field<string>("MaterielTypeName"),
                                             ArticleCount = t.Field<int>("EncapsulateCount"),
                                             AccountCount = t.Field<int>("EncapsulateCount")
                                         };
                //图形DataLegend
                Encapsulate_HeadPie.DataLegend = from t in tablePie.AsEnumerable()
                                                 where t.Field<int>("MaterielTypeID") > 0
                                                 select t.Field<string>("MaterielTypeName");

                //图形Data
                Encapsulate_HeadPie.Data = from t in tablePie.AsEnumerable()
                                           where t.Field<int>("MaterielTypeID") > 0
                                           select new
                                           {
                                               name = t.Field<string>("MaterielTypeName"),
                                               value = t.Field<int>("EncapsulateCount")
                                           };

            }
            if (BarData != null && BarData.Tables.Count > 0)
            {
                //声明日期集合
                List<string> listDate = new List<string>();

                List<dynamic> ListDynamic = new List<dynamic>();

                Encapsulate_HeadBar = new ExpandoObject();

                DataTable BarTable = BarData.Tables[0];

                var listQuery = (from t in BarTable.AsEnumerable()
                                 group t by new { value = t.Field<int>("MaterielTypeID"), name = t.Field<string>("MaterielTypeName") } into p
                                 select new { p.Key.name, p.Key.value }).ToList();

                foreach (var item in listQuery)
                {

                    dynamic SeriesItem = new ExpandoObject();
                    var queryNanme = from t in BarTable.AsEnumerable()
                                     where t.Field<string>("MaterielTypeName") == item.name
                                     select t;
                    List<int> amount = new List<int>();
                    //根据日期循环查询，并对每个场景赋值
                    for (int i = 0; i < latelyDays; i++)
                    {
                        string timeData = Convert.ToDateTime(entity.BeginTime).AddDays(i).ToString("yyyy-MM-dd");

                        int queryNum = (from q in queryNanme.AsEnumerable()
                                        where q.Field<DateTime>("Date").ToString("yyyy-MM-dd") == timeData
                                        select q.Field<int>("EncapsulateCount")).FirstOrDefault();
                        amount.Add(queryNum);
                        if (listQuery.IndexOf(item) == (listQuery.Count - 1))
                        {
                            listDate.Add(timeData);
                        }
                    }
                    ListDynamic.Add(new { name = item.name, data = amount, TypeId = item.value });
                }
                //获取字典集合
                Encapsulate_HeadBar.DicInfo = from t in BarTable.AsEnumerable()
                                              group t by new { value = t.Field<int>("MaterielTypeID"), name = t.Field<string>("MaterielTypeName") } into m
                                              orderby m.Key.value ascending
                                              select new
                                              {
                                                  Name = m.Key.name,
                                                  TypeId = m.Key.value
                                              };
                //封装对象
                Encapsulate_HeadBar.Data = ListDynamic;
                Encapsulate_HeadBar.DataLegend = listDate;
            }
            EncapsulateGather.DataPie = Encapsulate_HeadPie;
            EncapsulateGather.DataBar = Encapsulate_HeadBar;

            return EncapsulateGather;
        }
        #endregion

        #region 数据概况—物料分发
        /// <summary>
        /// 数据概况—物料分发
        /// </summary>
        /// <param name="latelyDays"></param>
        /// <returns></returns>
        public dynamic GetDistributeGather(int latelyDays)
        {
            BasicQueryArgs entity = new BasicQueryArgs { DateType = latelyDays };

            dynamic DistributeGather = new ExpandoObject();

            var queryData = DistributeMaterialDa.Instance.GetDistributeStatistics_Bar(entity);

            dynamic Encapsulate_HeadPie = null;
            if (queryData != null && queryData.Tables.Count > 0)
            {

                Encapsulate_HeadPie = new ExpandoObject();

                List<dynamic> ListDynamic = new List<dynamic>();
                //声明日期集合
                List<string> listDate = new List<string>();

                DataTable BarTable = queryData.Tables[0];

                var listQuery = (from t in BarTable.AsEnumerable()
                                 group t by new { value = t.Field<int>("ChannelId"), name = t.Field<string>("ChannelName") } into p
                                 select new { p.Key.name, p.Key.value }).ToList();

                foreach (var item in listQuery)
                {
                    var queryNanme = from t in BarTable.AsEnumerable()
                                     where t.Field<string>("ChannelName") == item.name
                                     select t;
                    List<int> amount = new List<int>();
                    //根据日期循环查询，并对每个场景赋值
                    for (int i = 0; i < latelyDays; i++)
                    {
                        string timeData = Convert.ToDateTime(entity.BeginTime).AddDays(i).ToString("yyyy-MM-dd");

                        int queryNum = (from q in queryNanme.AsEnumerable()
                                        where q.Field<DateTime>("Date").ToString("yyyy-MM-dd") == timeData
                                        select q.Field<int>("DistributeCount")).FirstOrDefault();
                        amount.Add(queryNum);
                        if (listQuery.IndexOf(item) == (listQuery.Count - 1))
                        {
                            listDate.Add(timeData);
                        }
                    }
                    ListDynamic.Add(new { name = item.name, data = amount, TypeId = item.value });
                }
                //获取字典集合
                Encapsulate_HeadPie.DicInfo = from t in BarTable.AsEnumerable()
                                              group t by new { value = t.Field<int>("ChannelId"), name = t.Field<string>("ChannelName") } into m
                                              orderby m.Key.value ascending
                                              select new
                                              {
                                                  Name = m.Key.name,
                                                  TypeId = m.Key.value
                                              };
                //图形Info
                DistributeGather.Info = from t in BarTable.AsEnumerable()
                                        group t by t.Field<string>("ChannelName") into p
                                        select new
                                        {
                                            Name = p.Select(x => x.Field<string>("ChannelName")).First(),
                                            ArticleCount = p.Sum(x => x.Field<int>("DistributeCount")),
                                            AccountCount = p.Sum(x => x.Field<int>("DistributeCount"))
                                        };
                Encapsulate_HeadPie.Data = ListDynamic;
                Encapsulate_HeadPie.DataLegend = listDate;
                DistributeGather.Date = new { BeginTime = entity.BeginTime, EndTime = entity.EndTime };

            }
            DistributeGather.DataBar = Encapsulate_HeadPie;
            return DistributeGather;
        }

        #endregion

        #region 数据概况—物料转发
        /// <summary>
        /// 数据概况—物料转发
        /// </summary>
        /// <param name="latelyDays"></param>
        /// <returns></returns>
        public dynamic GetForwardGather(int latelyDays)
        {
            BasicQueryArgs entity = new BasicQueryArgs { DateType = latelyDays };

            dynamic EncapsulateGather = new ExpandoObject();

            var PieData = ForwardMaterialDa.Instance.GetForwardStatistics_Pie(entity);

            var BarData = ForwardMaterialDa.Instance.GetForwardStatistics_Bar(entity);

            dynamic Encapsulate_HeadPie = null;//饼图
            dynamic Encapsulate_HeadBar = null;//柱状图

            if (PieData != null && PieData.Tables.Count > 0)
            {
                Encapsulate_HeadPie = new ExpandoObject();
                var tablePie = PieData.Tables[0];

                //图形Info
                EncapsulateGather.Info = from t in tablePie.AsEnumerable()
                                         select new
                                         {
                                             Name = t.Field<string>("ChannelName"),
                                             ArticleCount = t.Field<int>("MaterialForwardCount"),
                                             AccountCount = t.Field<int>("MaterialForwardCount")
                                         };
                //图形DataLegend
                Encapsulate_HeadPie.DataLegend = from t in tablePie.AsEnumerable()
                                                 where t.Field<int>("ChannelId") > 0
                                                 select t.Field<string>("ChannelName");

                //图形Data
                Encapsulate_HeadPie.Data = from t in tablePie.AsEnumerable()
                                           where t.Field<int>("ChannelId") > 0
                                           select new
                                           {
                                               name = t.Field<string>("ChannelName"),
                                               value = t.Field<int>("MaterialForwardCount")
                                           };
                //封装标题时间
                EncapsulateGather.Date = new { BeginTime = entity.BeginTime, EndTime = entity.EndTime };

            }
            if (BarData != null && BarData.Tables.Count > 0)
            {
                //声明日期集合
                List<string> listDate = new List<string>();

                List<dynamic> ListDynamic = new List<dynamic>();

                Encapsulate_HeadBar = new ExpandoObject();

                DataTable BarTable = BarData.Tables[0];

                var listQuery = (from t in BarTable.AsEnumerable()
                                 group t by new { value = t.Field<int>("ChannelId"), name = t.Field<string>("ChannelName") } into p
                                 select new { p.Key.name, p.Key.value }).ToList();

                foreach (var item in listQuery)
                {

                    dynamic SeriesItem = new ExpandoObject();
                    var queryNanme = from t in BarTable.AsEnumerable()
                                     where t.Field<string>("ChannelName") == item.name
                                     select t;
                    List<int> amount = new List<int>();
                    //根据日期循环查询，并对每个场景赋值
                    for (int i = 0; i < latelyDays; i++)
                    {
                        string timeData = Convert.ToDateTime(entity.BeginTime).AddDays(i).ToString("yyyy-MM-dd");

                        int queryNum = (from q in queryNanme.AsEnumerable()
                                        where q.Field<DateTime>("Date").ToString("yyyy-MM-dd") == timeData
                                        select q.Field<int>("MaterialForwardCount")).FirstOrDefault();
                        amount.Add(queryNum);
                        if (listQuery.IndexOf(item) == (listQuery.Count - 1))
                        {
                            listDate.Add(timeData);
                        }
                    }
                    ListDynamic.Add(new { name = item.name, data = amount, TypeId = item.value });
                }
                //获取字典集合
                Encapsulate_HeadBar.DicInfo = from t in BarTable.AsEnumerable()
                                              group t by new { value = t.Field<int>("ChannelId"), name = t.Field<string>("ChannelName") } into m
                                              orderby m.Key.value ascending
                                              select new
                                              {
                                                  Name = m.Key.name,
                                                  TypeId = m.Key.value
                                              };
                //封装对象
                Encapsulate_HeadBar.Data = ListDynamic;
                Encapsulate_HeadBar.DataLegend = listDate;
            }
            EncapsulateGather.DataPie = Encapsulate_HeadPie;
            EncapsulateGather.DataBar = Encapsulate_HeadBar;

            return EncapsulateGather;
        }
        #endregion

        #region 数据概况—物料线索
        /// <summary>
        /// 数据概况—物料线索
        /// </summary>
        /// <param name="latelyDays"></param>
        /// <returns></returns>
        public dynamic GetClueGather(int latelyDays)
        {
            BasicQueryArgs entity = new BasicQueryArgs { DateType = latelyDays };

            dynamic EncapsulateGather = new ExpandoObject();

            var PieData = ClueMaterialDa.Instance.GetClueStatistics_Pie(entity);

            var BarData = ClueMaterialDa.Instance.GetClueStatisticsIndex_Bar(entity);

            dynamic Encapsulate_HeadPie = null;//饼图
            dynamic Encapsulate_HeadBar = null;//柱状图

            if (PieData != null && PieData.Tables.Count > 0)
            {
                Encapsulate_HeadPie = new ExpandoObject();
                var tablePie = PieData.Tables[0];

                //图形Info
                EncapsulateGather.Info = from t in tablePie.AsEnumerable()
                                         where t.Field<int>("ClueTypeID") == 0
                                         select new
                                         {
                                             Name = t.Field<string>("ChannelName"),
                                             ArticleCount = t.Field<int>("ClueCount"),
                                             AccountCount = t.Field<int>("ClueCount")
                                         };
                //图形DataLegend
                Encapsulate_HeadPie.DataLegend = from t in tablePie.AsEnumerable()
                                                 where t.Field<int>("ClueTypeID") == 0 && t.Field<int>("ChannelID") > 0
                                                 select t.Field<string>("ChannelName");

                //图形Data
                Encapsulate_HeadPie.Data = from t in tablePie.AsEnumerable()
                                           where t.Field<int>("ClueTypeID") == 0 && t.Field<int>("ChannelID") > 0
                                           select new
                                           {
                                               name = t.Field<string>("ChannelName"),
                                               value = t.Field<int>("ClueCount")
                                           };
                //封装标题时间
                EncapsulateGather.Date = new { BeginTime = entity.BeginTime, EndTime = entity.EndTime };

            }
            if (BarData != null && BarData.Tables.Count > 0)
            {
                //声明日期集合
                List<string> listDate = new List<string>();

                List<dynamic> ListDynamic = new List<dynamic>();

                Encapsulate_HeadBar = new ExpandoObject();

                DataTable BarTable = BarData.Tables[0];

                var listQuery = (from t in BarTable.AsEnumerable()
                                 group t by new { value = t.Field<int>("ChannelId"), name = t.Field<string>("ChannelName") } into p
                                 select new { p.Key.name, p.Key.value }).ToList();

                foreach (var item in listQuery)
                {

                    dynamic SeriesItem = new ExpandoObject();
                    var queryNanme = from t in BarTable.AsEnumerable()
                                     where t.Field<string>("ChannelName") == item.name
                                     select t;
                    List<int> amount = new List<int>();
                    //根据日期循环查询，并对每个场景赋值
                    for (int i = 0; i < latelyDays; i++)
                    {
                        string timeData = Convert.ToDateTime(entity.BeginTime).AddDays(i).ToString("yyyy-MM-dd");

                        int queryNum = (from q in queryNanme.AsEnumerable()
                                        where q.Field<DateTime>("Date").ToString("yyyy-MM-dd") == timeData
                                        select q.Field<int>("ClueCount")).FirstOrDefault();
                        amount.Add(queryNum);
                        if (listQuery.IndexOf(item) == (listQuery.Count - 1))
                        {
                            listDate.Add(timeData);
                        }
                    }
                    ListDynamic.Add(new { name = item.name, data = amount, TypeId = item.value });
                }
                //获取字典集合
                Encapsulate_HeadBar.DicInfo = from t in BarTable.AsEnumerable()
                                              group t by new { value = t.Field<int>("ChannelId"), name = t.Field<string>("ChannelName") } into m
                                              orderby m.Key.value ascending
                                              select new
                                              {
                                                  Name = m.Key.name,
                                                  TypeId = m.Key.value
                                              };
                //封装对象
                Encapsulate_HeadBar.Data = ListDynamic;
                Encapsulate_HeadBar.DataLegend = listDate;
            }
            EncapsulateGather.DataPie = Encapsulate_HeadPie;
            EncapsulateGather.DataBar = Encapsulate_HeadBar;

            return EncapsulateGather;
        }
        #endregion
    }
}
