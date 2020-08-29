using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.Dal.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Distribute;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Distribute
{
    public class DistributeMaterialBll
    {
        #region 单例
        static DistributeMaterialBll instance = null;
        static readonly object padlock = new object();
        public DistributeMaterialBll()
        {
        }

        public static DistributeMaterialBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DistributeMaterialBll();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        #region 公共类
        /// <summary>
        /// 业务过滤
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="orginPrice"></param>
        /// <returns></returns>
        public Dictionary<string, dynamic> BusinessMap(BasicQueryArgs queryArgs)
        {
            Dictionary<string, Func<BasicQueryArgs, Tuple<string, dynamic>>> _dic = new Dictionary<string, Func<BasicQueryArgs, Tuple<string, dynamic>>>{
                { EnumData.fenf_head.ToString(),m=> GetDistributeStatistics_Bar(m) },

                { EnumData.fenf_materia.ToString(), m=> GetDistributeMateriel(m) },

                { EnumData.fenf_scenc.ToString(), m=> GetDistributeScene(m) },

                { EnumData.fenf_account.ToString(), m=> GetDistributeAccount(m)},

                { EnumData.fenf_essay.ToString(), m=> GetEncapsulateHeadEssay(m)}
            };
            Dictionary<string, dynamic> dynamicList = new Dictionary<string, dynamic>();
            //获取需要查询图形集合
            List<string> chartList = queryArgs.ChartType.Split(',').ToList();

            foreach (var item in chartList)
            {
                if (_dic.ContainsKey(item))
                {
                    var query = _dic[item].Invoke(queryArgs);
                    dynamicList.Add(query.Item1, query.Item2);
                }
            }

            return dynamicList;
        }
        #endregion

        #region 物料分发—表头统计-柱状图
        /// <summary>
        /// 物料分发—表头统计-柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetDistributeStatistics_Bar(BasicQueryArgs query)
        {
            var queryData = DistributeMaterialDa.Instance.GetDistributeStatistics_Bar(query);

            dynamic dynamicInfo = new ExpandoObject();



            if (queryData != null && queryData.Tables.Count > 0)
            {


                List<dynamic> ListDynamic = new List<dynamic>();
                //声明日期集合
                List<string> listDate = new List<string>();
                //声明场景名称集合
                List<string> listName = null;

                dynamicInfo = new ExpandoObject();
                DataTable BarTable = queryData.Tables[0];

                listName = (from t in BarTable.AsEnumerable()
                            group t by t.Field<string>("ChannelName") into p
                            select p.First().Field<string>("ChannelName") + string.Empty).ToList();

                foreach (var itemName in listName)
                {
                    dynamic SeriesItem = new ExpandoObject();
                    var queryNanme = from t in BarTable.AsEnumerable()
                                     where t.Field<string>("ChannelName") == itemName
                                     select t;
                    List<int> amount = new List<int>();
                    //根据日期循环查询，并对每个场景赋值
                    for (int i = 0; i < query.DateType; i++)
                    {
                        string timeData = Convert.ToDateTime(query.BeginTime).AddDays(i).ToString("yyyy-MM-dd");

                        int queryNum = (from q in queryNanme.AsEnumerable()
                                        where q.Field<DateTime>("Date").ToString("yyyy-MM-dd") == timeData
                                        select q.Field<int>("DistributeCount")).FirstOrDefault();
                        amount.Add(queryNum);
                        if (listName.IndexOf(itemName) == (listName.Count - 1))
                        {
                            listDate.Add(timeData);
                        }
                    }
                    SeriesItem.Name = itemName;
                    SeriesItem.Data = amount;
                    ListDynamic.Add(SeriesItem);
                }

                //封装对象
                //图形DataLegend
                dynamicInfo.DataLegend = from t in BarTable.AsEnumerable()
                                         group t by t.Field<string>("ChannelName") into p
                                         select new
                                         {
                                             name = p.Select(x=>x.Field<string>("ChannelName")).First(),
                                             value = p.Sum(x => x.Field<int>("DistributeCount"))
                                         };
                dynamicInfo.Series = ListDynamic;
                dynamicInfo.DateList = listDate;
                dynamicInfo.NameList = listName;
                dynamicInfo.TitleDate = new { BeginTime = query.BeginTime, EndTime = query.EndTime };


            }
            return new Tuple<string, dynamic>("Distribute_Head", dynamicInfo);
        }
        #endregion

        #region 物料分发—物料类型分布图

        /// <summary>
        /// 物料分发—物料类型分布图
        /// </summary>
        /// <returns></returns>
        public Tuple<string, dynamic> GetDistributeMateriel(BasicQueryArgs query)
        {
            dynamic dynamicInfo = new ExpandoObject();

            var result = DistributeMaterialDa.Instance.GetDistributeMateriel(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          group t by t.Field<string>("MaterialName") into p
                                          select p.First().Field<string>("MaterialName") + string.Empty).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   select new
                                   {
                                       name = t.Field<string>("MaterialName") + string.Empty,
                                       value = t.Field<int>("DistributeCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where !string.IsNullOrEmpty(t.Field<string>("ChannelName")) && t.Field<int>("DistributeCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),   name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };
            }
            return new Tuple<string, dynamic>("Distribute_Materia", dynamicInfo);
        }
        #endregion

        #region 物料分发—场景分布图
        /// <summary>
        /// 物料分发—场景分布图
        /// </summary>
        /// <returns></returns>
        public Tuple<string, dynamic> GetDistributeScene(BasicQueryArgs query)
        {
            dynamic dynamicInfo = new ExpandoObject();

            var result = DistributeMaterialDa.Instance.GetDistributeScene(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          group t by t.Field<string>("SceneName") into p
                                          select p.First().Field<string>("SceneName") + string.Empty).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   select new
                                   {
                                       name = t.Field<string>("SceneName") + string.Empty,
                                       value = t.Field<int>("DistributeCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ChannelId") > 0 && t.Field<int>("DistributeCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),   name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };
            }
            return new Tuple<string, dynamic>("Distribute_Scene", dynamicInfo);
        }
        #endregion

        #region 物料分发—账号分值图

        /// <summary>
        /// 物料分发—账号分值图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetDistributeAccount(BasicQueryArgs query)
        {
            dynamic dynamicInfo = new ExpandoObject();

            var result = DistributeMaterialDa.Instance.GetDistributeAccount(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          orderby t.Field<int>("OrderNum") ascending, t.Field<string>("DictName") ascending
                                          group t by t.Field<string>("DictName") into p
                                          select p.First().Field<string>("DictName") + string.Empty).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   select new
                                   {
                                       name = t.Field<string>("DictName") + string.Empty,
                                       value = t.Field<int>("DistributeCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ChannelId") > 0 && t.Field<int>("DistributeCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),   name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };
            }
            return new Tuple<string, dynamic>("Distribute_Account", dynamicInfo);
        }
        #endregion

        #region 物料分发—头部文章分值图
        /// <summary>
        /// 物料分发—头部文章分值图
        /// </summary>
        /// <returns></returns>
        public Tuple<string, dynamic> GetEncapsulateHeadEssay(BasicQueryArgs query)
        {
            dynamic dynamicInfo = new ExpandoObject();

            var result = DistributeMaterialDa.Instance.GetDistributeHeadEssay(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          orderby t.Field<int>("OrderNum") ascending, t.Field<string>("DictName") ascending
                                          group t by t.Field<string>("DictName") into p
                                          select p.First().Field<string>("DictName") + string.Empty).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   select new
                                   {
                                       name = t.Field<string>("DictName") + string.Empty,
                                       value = t.Field<int>("DistributeCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ChannelId")> 0 && t.Field<int>("DistributeCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),  name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };
            }
            return new Tuple<string, dynamic>("Distribute_Essay", dynamicInfo);

        }
        #endregion

        #region 物料分发-日汇总列表
        /// <summary>
        /// 物料分发-日汇总列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasicResultDto GetDistributeStatisticsList(ListQueryArgs query)
        {
            var resultQuery = DistributeMaterialDa.Instance.GetDistributeStatisticsList(query);
            if (resultQuery.Item2.Tables[0].Columns.Contains("RowNumber"))
            {
                resultQuery.Item2.Tables[0].Columns.Remove("RowNumber");
            }
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };

        }
        #endregion

        #region 物料分发-明细列表
        /// <summary>
        /// 物料分发-明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasicResultDto GetDistributeDetailList(ListQueryArgs query)
        {
            var resultQuery = DistributeMaterialDa.Instance.GetDistributeDetailList(query);

            var queryList = DataCommon.DataTableToList<DistributeDetailInfo>(resultQuery.Item2.Tables[0]);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = queryList };
        }
        public BasicResultDto GetDistributeDetailTable(ListQueryArgs query)
        {
            var resultQuery = DistributeMaterialDa.Instance.GetDistributeDetailList(query);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };
        }

        #endregion
    }
}
