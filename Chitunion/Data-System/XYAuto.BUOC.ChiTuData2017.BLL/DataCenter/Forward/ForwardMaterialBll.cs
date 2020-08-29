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
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Forward;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Forward
{
    public class ForwardMaterialBll
    {
        #region 初始化
        static ForwardMaterialBll instance = null;
        static readonly object padlock = new object();
        public ForwardMaterialBll()
        {
        }
        public static ForwardMaterialBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ForwardMaterialBll();
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
                { EnumData.zf_head_pie.ToString(),m=> GetForwardStatistics_Pie(m) },

                { EnumData.zf_head_bar.ToString(), m=> GetForwardStatistics_Bar(m) },

                { EnumData.zf_materia.ToString(), m=> GetForwardMateriel(m) },

                { EnumData.zf_scenc.ToString(), m=> GetForwardScene(m)},

                { EnumData.zf_account.ToString(), m=> GetForwardAccount(m)},

                { EnumData.zf_essay.ToString(), m=> GetForwardHeadEssay(m)}
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

        #region 物料转发—表头统计-饼图
        /// <summary>
        /// 物料转发—表头统计-饼图 业务处理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetForwardStatistics_Pie(BasicQueryArgs query)
        {
            dynamic Encapsulate_HeadPie = new ExpandoObject();
            var dataSet = ForwardMaterialDa.Instance.GetForwardStatistics_Pie(query);


            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var table = dataSet.Tables[0];

                //图形DataLegend
                Encapsulate_HeadPie.DataLegend = from t in table.AsEnumerable()
                                                 select new
                                                 {
                                                     name = t.Field<string>("ChannelName") + string.Empty,
                                                     value = t.Field<int>("MaterialForwardCount")
                                                 };
                //图形Data
                Encapsulate_HeadPie.Data = from t in table.AsEnumerable()
                                           where t.Field<int>("ChannelId") > 0
                                           select new
                                           {
                                               name = t.Field<string>("ChannelName") + string.Empty,
                                               value = t.Field<int>("MaterialForwardCount")
                                           };
                //封装标题时间
                Encapsulate_HeadPie.TitleDate = new { BeginTime = query.BeginTime, EndTime = query.EndTime };

            }


            return new Tuple<string, dynamic>("Forward_HeadPie", Encapsulate_HeadPie);
        }
        #endregion

        #region 物料转发—表头统计-柱状图
        /// <summary>
        ///  物料转发—表头统计-柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetForwardStatistics_Bar(BasicQueryArgs query)
        {
            var queryData = ForwardMaterialDa.Instance.GetForwardStatistics_Bar(query);

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
                            where t.Field<int>("ChannelId") > 0
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
                                        select q.Field<int>("MaterialForwardCount")).FirstOrDefault();
                        amount.Add(queryNum);
                        if (listName.IndexOf(itemName) == (listName.Count - 1))
                        {
                            listDate.Add(timeData);
                        }
                    }
                    ListDynamic.Add(new { Name = itemName, Data = amount });
                }
                //封装对象
                dynamicInfo.Series = ListDynamic;
                dynamicInfo.DateList = listDate;
                dynamicInfo.NameList = listName;
            }
            return new Tuple<string, dynamic>("Forward_HeadBar", dynamicInfo);
        }
        #endregion

        #region 物料转发—物料类型分布图
        public Tuple<string, dynamic> GetForwardMateriel(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ForwardMaterialDa.Instance.GetForwardMateriel(query);
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
                                       value = t.Field<int>("MaterialForwardCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ChannelId") > 0 && t.Field<int>("MaterialForwardCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),  name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };

            }
            return new Tuple<string, dynamic>("Forward_Materia", dynamicInfo);
        }
        #endregion

        #region 物料转发—场景分布图
        /// <summary>
        /// 物料转发—场景分布图
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetForwardScene(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ForwardMaterialDa.Instance.GetForwardScene(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          where t.Field<int>("SceneId")>0
                                          group t by t.Field<string>("SceneName") into p
                                          select p.First().Field<string>("SceneName") + string.Empty).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   where t.Field<int>("SceneId") > 0
                                   select new
                                   {
                                       name = t.Field<string>("SceneName") + string.Empty,
                                       value = t.Field<int>("MaterialForwardCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ChannelId") > 0 && t.Field<int>("MaterialForwardCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),  name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };

            }
            return new Tuple<string, dynamic>("Forward_Scene", dynamicInfo);
        }

        #endregion

        #region 物料转发—账号分值图

        /// <summary>
        /// 物料转发—账号分值图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetForwardAccount(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ForwardMaterialDa.Instance.GetForwardAccount(query);
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
                                       value = t.Field<int>("MaterialForwardCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ChannelId") > 0 && t.Field<int>("MaterialForwardCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),  name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };


            }
            return new Tuple<string, dynamic>("Forward_Account", dynamicInfo);
        }
        #endregion

        #region 物料转发—头部文章分值图
        /// <summary>
        /// 物料转发—头部文章分值图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetForwardHeadEssay(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = ForwardMaterialDa.Instance.GetForwardHeadEssay(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          orderby t.Field<int>("OrderNum") ascending, t.Field<string>("DictName") ascending
                                          group t by t.Field<string>("DictName")  into p  
                                          select p.First().Field<string>("DictName") + string.Empty).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   select new
                                   {
                                       name = t.Field<string>("DictName") + string.Empty,
                                       value = t.Field<int>("MaterialForwardCount"),
                                       TypeId = t.Field<int>("ChannelId")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("ChannelId")> 0 && t.Field<int>("MaterialForwardCount") > 0
                                      group t by new { value = t.Field<int>("ChannelId"),  name = t.Field<string>("ChannelName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };


            }
            return new Tuple<string, dynamic>("Forward_Essay", dynamicInfo);
        }
        #endregion


        #region 汇总列表
        public BasicResultDto GetForwardStatisticsList(ListQueryArgs query)
        {
            var resultQuery = ForwardMaterialDa.Instance.GetForwardStatisticsList(query);
            if (resultQuery.Item2.Tables[0].Columns.Contains("RowNumber"))
            {
                resultQuery.Item2.Tables[0].Columns.Remove("RowNumber");
            }
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };

        }
        #endregion

        #region 明细列表
        public BasicResultDto GetForwardDetailTable(ListQueryArgs query)
        {
            var resultQuery = ForwardMaterialDa.Instance.GetForwardDetailList(query);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };
        }
        public BasicResultDto GetForwardDetailList(ListQueryArgs query)
        {
            var resultQuery = ForwardMaterialDa.Instance.GetForwardDetailList(query);
            var queryList = DataCommon.DataTableToList<ForwardDetailInfo>(resultQuery.Item2.Tables[0]);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = queryList };
        }

        #endregion

    }
}
