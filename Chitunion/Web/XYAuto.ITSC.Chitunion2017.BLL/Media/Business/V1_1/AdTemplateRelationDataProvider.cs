/********************************************************
*创建人：lixiong
*创建时间：2017/6/22 15:09:55
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Support;
using XYAuto.ITSC.Chitunion2017.Entities.AdTemplate;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    public class AdTemplateRelationDataProvider
    {
        /// <summary>
        /// 获取到售卖平台的组合
        /// </summary>
        /// <param name="dataNumber"></param>
        /// <returns>12001|Android,12002|IOS</returns>
        public static string GetSellingPlatform(int dataNumber)
        {
            var dic = GetDicSellingPlatformValue();
            var dicValue = dic.FirstOrDefault(s => s.Key == dataNumber);
            if (string.IsNullOrWhiteSpace(dicValue.Value))
                return string.Empty;

            return dicValue.Value;
        }

        /// <summary>
        /// 获取到售卖平台的组合,12001|Android,12002|IOS 逗号分割
        /// </summary>
        /// <param name="dataNumber"></param>
        /// <returns>List:12002|IOS</returns>
        public static List<string> GetSellingPlatformList(int dataNumber)
        {
            return GetSellingPlatform(dataNumber).Split(',').ToList();
        }

        /// <summary>
        /// 获取售卖平台的集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetDicSellingPlatformValue()
        {
            return new Dictionary<int, string>()
           {
               {1 ,   "12001|Android"},
                      {2 ,   "12002|IOS"},
                      {3 ,   "12001|Android,12002|IOS"},
                      {4 ,   "12003|Android&IOS"},
                      {5 ,   "12001|Android,12003|Android&IOS"},
                      {6 ,   "12002|IOS,12003|Android&IOS"},
                      {7 ,   "12001|Android,12002|IOS,12003|Android&IOS"},
                      {8 ,   "12004|PAD"},
                      {9 ,   "12001|Android,12004|PAD"},
                      {10 ,  "12002|IOS,12004|PAD"},
                      {11 ,  "12001|Android,12002|IOS,12004|PAD"},
                      {12 ,  "12003|Android&IOS,12004|PAD"},
                      {13 ,  "12001|Android,12003|Android&IOS,12004|PAD"},
                      {14 ,  "12002|IOS,12003|Android&IOS,12004|PAD"},
                      {15 ,  "12001|Android,12002|IOS,12003|Android&IOS,12004|PAD"},
                      {16 ,  "12005|Gphone"},
                      {17 ,  "12001|Android,12005|Gphone"},
                      {18 ,  "12002|IOS,12005|Gphone"},
                      {19 ,  "12001|Android,12002|IOS,12005|Gphone"},
                      {20 ,  "12003|Android&IOS,12005|Gphone"},
                      {21 ,  "12001|Android,12003|Android&IOS,12005|Gphone"},
                      {22 ,  "12002|IOS,12003|Android&IOS,12005|Gphone"},
                      {23 ,  "12001|Android,12002|IOS,12003|Android&IOS,12005|Gphone"},
                      {24 ,  "12004|PAD,12005|Gphone"},
                      {25 ,  "12001|Android,12004|PAD,12005|Gphone"},
                      {26 ,  "12002|IOS,12004|PAD,12005|Gphone"},
                      {27 ,  "12001|Android,12002|IOS,12004|PAD,12005|Gphone"},
                      {28 ,  "12003|Android&IOS,12004|PAD,12005|Gphone"},
                      {29 ,  "12001|Android,12003|Android&IOS,12004|PAD,12005|Gphone"},
                      {30 ,  "12002|IOS,12003|Android&IOS,12004|PAD,12005|Gphone"},
                      {31 ,  "12001|Android,12002|IOS,12003|Android&IOS,12004|PAD,12005|Gphone"},
                      {32 ,  "12006|Phone"},
                      {33 ,  "12001|Android,12006|Phone"},
                      {34 ,  "12002|IOS,12006|Phone"},
                      {35 ,  "12001|Android,12002|IOS,12006|Phone"},
                      {36 ,  "12003|Android&IOS,12006|Phone"},
                      {37 ,  "12001|Android,12003|Android&IOS,12006|Phone"},
                      {38 ,  "12002|IOS,12003|Android&IOS,12006|Phone"},
                      {39 ,  "12001|Android,12002|IOS,12003|Android&IOS,12006|Phone"},
                      {40 ,  "12004|PAD,12006|Phone"},
                      {41 ,  "12001|Android,12004|PAD,12006|Phone"},
                      {42 ,  "12002|IOS,12004|PAD,12006|Phone"},
                      {43 ,  "12001|Android,12002|IOS,12004|PAD,12006|Phone"},
                      {44 ,  "12003|Android&IOS,12004|PAD,12006|Phone"},
                      {45 ,  "12001|Android,12003|Android&IOS,12004|PAD,12006|Phone"},
                      {46 ,  "12002|IOS,12003|Android&IOS,12004|PAD,12006|Phone"},
                      {47 ,  "12001|Android,12002|IOS,12003|Android&IOS,12004|PAD,12006|Phone"},
                      {48 ,  "12005|Gphone,12006|Phone"},
                      {49 ,  "12001|Android,12005|Gphone,12006|Phone"},
                      {50 ,  "12002|IOS,12005|Gphone,12006|Phone"},
                      {51 ,  "12001|Android,12002|IOS,12005|Gphone,12006|Phone"},
                      {52 ,  "12003|Android&IOS,12005|Gphone,12006|Phone"},
                      {53 ,  "12001|Android,12003|Android&IOS,12005|Gphone,12006|Phone"},
                      {54 ,  "12002|IOS,12003|Android&IOS,12005|Gphone,12006|Phone"},
                      {55 ,  "12001|Android,12002|IOS,12003|Android&IOS,12005|Gphone,12006|Phone"},
                      {56 ,  "12004|PAD,12005|Gphone,12006|Phone"},
                      {57 ,  "12001|Android,12004|PAD,12005|Gphone,12006|Phone"},
                      {58 ,  "12002|IOS,12004|PAD,12005|Gphone,12006|Phone"},
                      {59 ,  "12001|Android,12002|IOS,12004|PAD,12005|Gphone,12006|Phone"},
                      {60 ,  "12003|Android&IOS,12004|PAD,12005|Gphone,12006|Phone"},
                      {61 ,  "12001|Android,12003|Android&IOS,12004|PAD,12005|Gphone,12006|Phone"},
                      {62 ,  "12002|IOS,12003|Android&IOS,12004|PAD,12005|Gphone,12006|Phone"},
                      {63 ,  "12001|Android,12002|IOS,12003|Android&IOS,12004|PAD,12005|Gphone,12006|Phone"},
           };
        }

        /// <summary>
        /// 获取到售卖方式的组合
        /// </summary>
        /// <param name="dataNumber"></param>
        /// <returns>11001|CPD,11002|CPM</returns>
        public static string GetDicSaleMode(int dataNumber)
        {
            var dic = GetDicSellingPlatformValue();
            var dicValue = dic.FirstOrDefault(s => s.Key == dataNumber);
            if (string.IsNullOrWhiteSpace(dicValue.Value))
                return string.Empty;

            return dicValue.Value;
        }

        /// <summary>
        /// 获取到售卖方式的组合,11001|CPD,11002|CPM 逗号分割
        /// </summary>
        /// <param name="dataNumber"></param>
        /// <returns>List:11001|CPD,11002|CPM</returns>
        public static List<string> GetDicSaleModeList(int dataNumber)
        {
            return GetSellingPlatform(dataNumber).Split(',').ToList();
        }

        /// <summary>
        /// 获取售卖方式的集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetDicSaleMode()
        {
            return new Dictionary<int, string>()
            {
                { 1,"11001|CPD"},
                { 2,"11002|CPM"},
                { 3,"11001|CPD,11002|CPM"}
            };
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        public static List<AreaInfoEntity> GetAreaList()
        {
            return CacheHelper<List<AreaInfoEntity>>.Get(HttpRuntime.Cache, () => "xy.chitu.area.citys",
              () => Dal.AdTemplate.SaleAreaInfo.Instance.GetAreaInfoList(),
              null, 30);
        }
    }
}