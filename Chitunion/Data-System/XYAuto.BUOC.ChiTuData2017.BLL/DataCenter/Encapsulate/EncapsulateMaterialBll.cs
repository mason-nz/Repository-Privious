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
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Encapsulate;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter
{
    public class EncapsulateMaterialBll
    {
        #region 初始化
        static EncapsulateMaterialBll instance = null;
        static readonly object padlock = new object();

        public static EncapsulateMaterialBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new EncapsulateMaterialBll();
                        }
                    }
                }
                return instance;
            }
        }
        public EncapsulateMaterialBll()
        {
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
                { EnumData.fz_head_pie.ToString(),m=> GetEncapsulateStatistics_Pie(m) },

                { EnumData.fz_head_bar.ToString(), m=> GetEncapsulateStatistics_Bar(m) },

                { EnumData.fz_scenc.ToString(), m=> GetEncapsulateScene(m) },

                { EnumData.fz_account.ToString(), m=> GetEncapsulateAccount(m)},

                { EnumData.fz_essay.ToString(), m=> GetEncapsulateHeadEssay(m)},

                { EnumData.fz_condition.ToString(), m=> GetEncapsulateCondition(m)}
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

        #region 物料封装—表头统计-饼图
        /// <summary>
        /// 物料封装—表头统计-饼图 业务处理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetEncapsulateStatistics_Pie(BasicQueryArgs query)
        {
            dynamic Encapsulate_HeadPie = new ExpandoObject();
            var dataSet = EncapsulateMaterialDa.Instance.GetEncapsulateStatistics_Pie(query);


            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var table = dataSet.Tables[0];

                //图形DataLegend
                Encapsulate_HeadPie.DataLegend = from t in table.AsEnumerable()
                                                 select new
                                                 {
                                                     name = t.Field<string>("MaterielTypeName"),
                                                     value = t.Field<int>("EncapsulateCount")
                                                 };
                //图形Data
                Encapsulate_HeadPie.Data = from t in table.AsEnumerable()
                                           where t.Field<int>("MaterielTypeID") > 0
                                           select new
                                           {
                                               name = t.Field<string>("MaterielTypeName"),
                                               value = t.Field<int>("EncapsulateCount")
                                           };
                //封装标题时间
                Encapsulate_HeadPie.TitleDate = new { BeginTime = query.BeginTime, EndTime = query.EndTime };

            }


            return new Tuple<string, dynamic>("Encapsulate_HeadPie", Encapsulate_HeadPie);
        }
        #endregion

        #region 物料封装—表头统计-柱状图
        /// <summary>
        ///  物料封装—表头统计-柱状图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetEncapsulateStatistics_Bar(BasicQueryArgs query)
        {
            var queryData = EncapsulateMaterialDa.Instance.GetEncapsulateStatistics_Bar(query);

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
                            where t.Field<int>("MaterielTypeID") > 0
                            group t by t.Field<string>("MaterielTypeName") into p
                            select p.First().Field<string>("MaterielTypeName")).ToList();

                foreach (var itemName in listName)
                {
                    dynamic SeriesItem = new ExpandoObject();
                    var queryNanme = from t in BarTable.AsEnumerable()
                                     where t.Field<string>("MaterielTypeName") == itemName && t.Field<int>("MaterielTypeID") > 0
                                     select t;
                    List<int> amount = new List<int>();
                    //根据日期循环查询，并对每个场景赋值
                    for (int i = 0; i < query.DateType; i++)
                    {
                        string timeData = Convert.ToDateTime(query.BeginTime).AddDays(i).ToString("yyyy-MM-dd");

                        int queryNum = (from q in queryNanme.AsEnumerable()
                                        where q.Field<DateTime>("Date").ToString("yyyy-MM-dd") == timeData
                                        select q.Field<int>("EncapsulateCount")).FirstOrDefault();
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
                dynamicInfo.Series = ListDynamic;
                dynamicInfo.DateList = listDate;
                dynamicInfo.NameList = listName;
            }
            return new Tuple<string, dynamic>("Encapsulate_HeadBar", dynamicInfo);
        }
        #endregion

        #region 物料封装—场景分布图
        /// <summary>
        /// 物料封装—场景分布图
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetEncapsulateScene(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = EncapsulateMaterialDa.Instance.GetEncapsulateScene(query);
            ; if (result != null && result.Tables.Count > 0)
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
                                       value = t.Field<int>("EncapsulateCount"),
                                       TypeId = t.Field<int>("MaterielTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("MaterielTypeID") > 0 && t.Field<int>("EncapsulateCount")>0
                                      group t by new { value = t.Field<int>("MaterielTypeID"),  name = t.Field<string>("MaterielTypeName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };

            }
            return new Tuple<string, dynamic>("Encapsulate_Scene", dynamicInfo);
        }

        #endregion

        #region 物料封装—账号分值图

        /// <summary>
        /// 物料封装—账号分值图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetEncapsulateAccount(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = EncapsulateMaterialDa.Instance.GetEncapsulateAccount(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          orderby t.Field<int>("OrderNum") ascending, t.Field<string>("DictName") ascending
                                          group t by t.Field<string>("DictName") into p
                                          select p.First().Field<string>("DictName" + string.Empty)).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   select new
                                   {
                                       name = t.Field<string>("DictName") + string.Empty,
                                       value = t.Field<int>("EncapsulateCount"),
                                       TypeId = t.Field<int>("MaterielTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("MaterielTypeID") > 0 && t.Field<int>("EncapsulateCount") > 0
                                      group t by new {  value = t.Field<int>("MaterielTypeID"), name = t.Field<string>("MaterielTypeName") } into m
                                      orderby m.Key.value ascending
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };

            }
            return new Tuple<string, dynamic>("Encapsulate_Account", dynamicInfo);
        }
        #endregion

        #region 物料封装—头部文章分值图
        /// <summary>
        /// 物料封装—头部文章分值图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetEncapsulateHeadEssay(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = EncapsulateMaterialDa.Instance.GetEncapsulateHeadEssay(query);
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
                                       value = t.Field<int>("EncapsulateCount"),
                                       TypeId = t.Field<int>("MaterielTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("MaterielTypeID") > 0 && t.Field<int>("EncapsulateCount") > 0
                                      group t by new { value = t.Field<int>("MaterielTypeID"),  name = t.Field<string>("MaterielTypeName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };

            }
            return new Tuple<string, dynamic>("Encapsulate_Essay", dynamicInfo);
        }
        #endregion

        #region 物料封装-状态分布图
        /// <summary>
        /// 物料封装-状态分布图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<string, dynamic> GetEncapsulateCondition(BasicQueryArgs query)
        {

            dynamic dynamicInfo = new ExpandoObject();

            var result = EncapsulateMaterialDa.Instance.GetEncapsulateCondition(query);
            if (result != null && result.Tables.Count > 0)
            {
                var table = result.Tables[0];

                //图形DataLegend
                dynamicInfo.DataLegend = (from t in table.AsEnumerable()
                                          group t by t.Field<string>("ConditionName") into p
                                          select p.First().Field<string>("ConditionName") + string.Empty).ToList();
                //图形Data
                dynamicInfo.Data = from t in table.AsEnumerable()
                                   select new
                                   {
                                       name = t.Field<string>("ConditionName") + string.Empty,
                                       value = t.Field<int>("EncapsulateCount"),
                                       TypeId = t.Field<int>("MaterielTypeID")
                                   };
                dynamicInfo.DicInfo = from t in table.AsEnumerable()
                                      where t.Field<int>("MaterielTypeID")> 0 && t.Field<int>("EncapsulateCount") > 0
                                      group t by new { value = t.Field<int>("MaterielTypeID"),  name = t.Field<string>("MaterielTypeName") } into m
                                      select new
                                      {
                                          Name = m.Key.name,
                                          TypeId = m.Key.value
                                      };
            }
            return new Tuple<string, dynamic>("Encapsulate_Condition", dynamicInfo);
        }
        #endregion

        #region 汇总列表
        public BasicResultDto GetEncapsulateStatisticsList(ListQueryArgs query)
        {
            var resultQuery = EncapsulateMaterialDa.Instance.GetEncapsulateStatisticsList(query);
            if (resultQuery.Item2.Tables[0].Columns.Contains("RowNumber"))
            {
                resultQuery.Item2.Tables[0].Columns.Remove("RowNumber");
            }
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };

        }
        #endregion

        #region 明细列表
        public BasicResultDto GetEncapsulateDetailList(ListQueryArgs query)
        {
            var resultQuery = EncapsulateMaterialDa.Instance.GetEncapsulateDetailList(query);
            

            var queryList = DataCommon.DataTableToList<EncapsulateDetailInfo>(resultQuery.Item2.Tables[0]);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = queryList };
        }

        public BasicResultDto GetEncapsulateDetailTable(ListQueryArgs query)
        {
            var resultQuery = EncapsulateMaterialDa.Instance.GetEncapsulateDetailList(query);
            return new BasicResultDto { TotalCount = resultQuery.Item1, List = resultQuery.Item2.Tables[0] };
        }
        #endregion
    }
}
