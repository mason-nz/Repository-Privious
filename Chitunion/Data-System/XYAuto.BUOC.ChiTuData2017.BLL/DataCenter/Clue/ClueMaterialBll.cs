using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.Dal.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Clue;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Clue
{
    public class ClueMaterialBll
    {
        #region 初始化
        static ClueMaterialBll instance = null;
        static readonly object padlock = new object();
        public ClueMaterialBll()
        {
        }
        public static ClueMaterialBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ClueMaterialBll();
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
                { EnumData.xs_head_pie.ToString(),m=> GetClueStatistics_Pie(m) },

                { EnumData.xs_head_bar.ToString(), m=> GetClueStatistics_Bar(m) },

                { EnumData.xs_materia.ToString(), m=> GetClueMateriel(m) },

                { EnumData.xs_scenc.ToString(), m=> GetClueScene(m)},

                { EnumData.xs_account.ToString(), m=> GetClueAccount(m)},

                { EnumData.xs_essay.ToString(), m=> GetClueHeadEssay(m)}
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

        #region 物料线索—表头统计-饼图
        /// <summary>
        /// 物料线索—表头统计-饼图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetClueStatistics_Pie(BasicQueryArgs query)
        {
            dynamic Encapsulate_HeadPie = new ExpandoObject();
            var dataSet = ClueMaterialDa.Instance.GetClueStatistics_Pie(query);


            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var table = dataSet.Tables[0];

                //图形DataLegend
                Encapsulate_HeadPie.DataLegend = from t in table.AsEnumerable()
                                                 where t.Field<int>("ClueTypeID") == 0
                                                 select new
                                                 {
                                                     name = t.Field<string>("ChannelName") + string.Empty,
                                                     value = t.Field<int>("ClueCount")
                                                 };
                //饼图内圈
                Encapsulate_HeadPie.InsideData = from t in table.AsEnumerable()
                                                 where t.Field<int>("ChannelID") > 0 && t.Field<int>("ClueTypeID") == 0
                                                 select new
                                                 {
                                                     name = t.Field<string>("ChannelName") + string.Empty,
                                                     TypeId = t.Field<int>("ChannelID"),
                                                     value = t.Field<int>("ClueCount")
                                                 };
                //饼图外圈
                Encapsulate_HeadPie.OutsideData = from t in table.AsEnumerable()
                                                  where t.Field<int>("ChannelID") > 0 && t.Field<int>("ClueTypeID") > 0
                                                  select new
                                                  {
                                                      name = t.Field<string>("ClueTypeName") + string.Empty,
                                                      TypeId = t.Field<int>("ChannelID"),
                                                      value = t.Field<int>("ClueCount")
                                                  };
                //封装标题时间
                Encapsulate_HeadPie.TitleDate = new { BeginTime = query.BeginTime, EndTime = query.EndTime };

            }


            return new Tuple<string, dynamic>("Clue_HeadPie", Encapsulate_HeadPie);
        }
        #endregion

        #region 物料线索—表头统计-柱状图
        /// <summary>
        ///  物料线索—表头统计-柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetClueStatistics_Bar(BasicQueryArgs query)
        {
            var queryData = ClueMaterialDa.Instance.GetClueStatistics_Bar(query);

            dynamic dynamicInfo = new ExpandoObject();

            if (queryData != null && queryData.Tables.Count > 0)
            {
                List<dynamic> ListDynamic = new List<dynamic>();
                //声明日期集合
                List<string> listDate = new List<string>();

                dynamicInfo = new ExpandoObject();
                DataTable BarTable = queryData.Tables[0];

                var queryClueList = (from t in BarTable.AsEnumerable()
                                     where t.Field<int>("ChannelID") > 0
                                     group t by new
                                     {
                                         ClueTypeName = t.Field<string>("ClueTypeName"),
                                         ChannelID = t.Field<int>("ChannelID")
                                     } into p
                                     select new
                                     {
                                         p.Key.ClueTypeName,
                                         p.Key.ChannelID
                                     }).ToList();

                foreach (var item in queryClueList)
                {
                    var queryNanme = from t in BarTable.AsEnumerable()
                                     where t.Field<string>("ClueTypeName") == item.ClueTypeName
                                     select t;
                    List<int> amount = new List<int>();
                    //根据日期循环查询，并对每个场景赋值
                    for (int i = 0; i < query.DateType; i++)
                    {
                        string timeData = Convert.ToDateTime(query.BeginTime).AddDays(i).ToString("yyyy-MM-dd");

                        int queryNum = (from q in queryNanme.AsEnumerable()
                                        where q.Field<DateTime>("Date").ToString("yyyy-MM-dd") == timeData
                                        select q.Field<int>("ClueCount")).FirstOrDefault();
                        amount.Add(queryNum);
                        if (queryClueList.IndexOf(item) == (queryClueList.Count() - 1))
                        {
                            listDate.Add(timeData);
                        }
                    }
                    ListDynamic.Add(new { Name = item.ClueTypeName, Data = amount, TypeId = item.ChannelID });
                }
                //封装对象
                dynamicInfo.Series = ListDynamic;
                dynamicInfo.NameList = queryClueList.Select(p => p.ClueTypeName);
                dynamicInfo.DateList = listDate;
            }
            return new Tuple<string, dynamic>("Clue_HeadBar", dynamicInfo);
        }
        #endregion

        #region 物料线索—场景分布图
        /// <summary>
        /// 物料线索—场景分布图
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetClueScene(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ClueMaterialDa.Instance.GetClueScene(query);
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
                                       value = t.Field<int>("ClueCount"),
                                       TypeId = t.Field<int>("ClueTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ClueTypeID") > 0 && t.Field<int>("ClueCount") > 0
                                      group t by new { value = t.Field<int>("ClueTypeID"), name = t.Field<string>("ClueTypeName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };
            }
            return new Tuple<string, dynamic>("Clue_Scene", dynamicInfo);
        }

        #endregion

        #region 物料线索—物料类型分布图
        /// <summary>
        /// 物料线索—物料类型分布图
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetClueMateriel(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ClueMaterialDa.Instance.GetClueMateriel(query);
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
                                       value = t.Field<int>("ClueCount"),
                                       TypeId = t.Field<int>("ClueTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ClueTypeID") > 0 && t.Field<int>("ClueCount")>0
                                      group t by new { value = t.Field<int>("ClueTypeID"), name = t.Field<string>("ClueTypeName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };
            }
            return new Tuple<string, dynamic>("Clue_Materia", dynamicInfo);
        }

        #endregion

        #region 物料线索发—账号分值图

        /// <summary>
        /// 物料线索发—账号分值图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetClueAccount(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ClueMaterialDa.Instance.GetClueAccount(query);
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
                                       value = t.Field<int>("ClueCount"),
                                       TypeId = t.Field<int>("ClueTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ClueTypeID") > 0 && t.Field<int>("ClueCount") > 0
                                      group t by new { value = t.Field<int>("ClueTypeID"), name = t.Field<string>("ClueTypeName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };

            }
            return new Tuple<string, dynamic>("Clue_Account", dynamicInfo);
        }
        #endregion

        #region 物料线索—头部文章分值图
        /// <summary>
        /// 物料线索—头部文章分值图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetClueHeadEssay(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ClueMaterialDa.Instance.GetClueHeadEssay(query);
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
                                       value = t.Field<int>("ClueCount"),
                                       TypeId = t.Field<int>("ClueTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ClueTypeID") > 0 && t.Field<int>("ClueCount") > 0
                                      group t by new { value = t.Field<int>("ClueTypeID"), name = t.Field<string>("ClueTypeName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };

            }
            return new Tuple<string, dynamic>("Clue_Essay", dynamicInfo);
        }
        #endregion

        #region 汇总列表
        public BasicResultDto GetClueStatisticsList(ListQueryArgs query)
        {
            var resultQuery = ClueMaterialDa.Instance.GetClueStatisticsList(query);
            if (resultQuery.Item2.Tables[0].Columns.Contains("RowNumber"))
            {
                resultQuery.Item2.Tables[0].Columns.Remove("RowNumber");
            }
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };

        }
        #endregion

        #region 明细列表
        public BasicResultDto GetClueDetailList(ListQueryArgs query)
        {
            var resultQuery = ClueMaterialDa.Instance.GetClueDetailList(query);
            var listInfo = DataCommon.DataTableToList<ClueDataInfo>(resultQuery.Item2.Tables[0]);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = listInfo };
        }

        public BasicResultDto GetClueDetailTable(ListQueryArgs query)
        {
            var resultQuery = ClueMaterialDa.Instance.GetClueDetailList(query);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };
        }
        #endregion
    }
}
