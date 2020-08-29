using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Clue;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Forward;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter
{
    public class DataStreamExportExceBll
    {
        #region 初始化
        static DataStreamExportExceBll instance = null;
        static readonly object padlock = new object();
        static Dictionary<string, Dictionary<string, string>> dicTitle;
        static int ExcelCount = DataCommon.GetAppSettingInt32Value("ExcelQuantity");

        private static int DataCommonGetAppSettingInt32Value(string v)
        {
            throw new NotImplementedException();
        }

        public DataStreamExportExceBll()
        {
            dicTitle = DetailExportTitle();
        }
        public static DataStreamExportExceBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DataStreamExportExceBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public Tuple<MemoryStream, string, bool> DetailExport(ListQueryArgs query)
        {
            Dictionary<string, Func<ListQueryArgs, BasicResultDto>> _dic = new Dictionary<string, Func<ListQueryArgs, BasicResultDto>>{
                { ExcelEnum.fz.ToString(),m=> EncapsulateMaterialBll.Instance.GetEncapsulateStatisticsList(m)},

                { ExcelEnum.ff.ToString(), m=> DistributeMaterialBll.Instance.GetDistributeStatisticsList(m) },

                { ExcelEnum.zf.ToString(), m=> ForwardMaterialBll.Instance.GetForwardStatisticsList(m) },

                { ExcelEnum.xs.ToString(), m=> ClueMaterialBll.Instance.GetClueStatisticsList(m)},

                { ExcelEnum.fz_detail.ToString(),m=> EncapsulateMaterialBll.Instance.GetEncapsulateDetailTable(m)},

                { ExcelEnum.ff_detail.ToString(), m=> DistributeMaterialBll.Instance.GetDistributeDetailTable(m) },

                { ExcelEnum.zf_detail.ToString(), m=> ForwardMaterialBll.Instance.GetForwardDetailTable(m) },

                { ExcelEnum.xs_detail.ToString(), m=> ClueMaterialBll.Instance.GetClueDetailTable(m)}
            };
            string filePath = string.Empty;
            if (_dic.ContainsKey(query.ListType))
            {
                DataTable table = (DataTable)_dic[query.ListType].Invoke(query).List;
                if (table.Rows.Count > 0)
                {
                    if (table.Rows.Count > ExcelCount)
                    {
                        return new Tuple<MemoryStream, string, bool>(null, string.Empty, false);

                    }
                    return new Tuple<MemoryStream, string, bool>(GetMemoryStream(table, dicTitle[query.ListType]), GetEnumDescription(query.ListType), true);
                }
            }
            return null;

        }

        private MemoryStream GetMemoryStream(DataTable table, Dictionary<string, string> dicTitle)
        {
            if (dicTitle == null)
            {
                return ExcelHelper.Export(table);
            }
            else
            {
                return ExcelHelper.Export(table, dicTitle);
            }
        }
        /// <summary>
        /// 生成Excel导出表头
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> DetailExportTitle()
        {
            Dictionary<string, Dictionary<string, string>> dicList = new Dictionary<string, Dictionary<string, string>>();
            string strKey = string.Empty;

            //封装明细列表表头
            Dictionary<string, string> encapsulateDic = new Dictionary<string, string> {
                { "MaterialID","物料ID"},
                { "Title","标题"},
                { "Url","URL"},
                { "MaterialName","物料类型"},
                { "EncapsulateTime","封装时间"},
                { "ChannelName","渠道"},
                { "SceneName","场景"},
                { "AccountName","头部文章抓取账号"},
                { "AccountScore","头部文章所属账号分值"},
                { "ArticleScore","头部文章分值"},
                { "BrandNmae","品牌"},
                { "SerialName","车型"},
                { "ConditionName","状态"},
                { "Reason","原因"},
            };
            //分发明细列表表头
            Dictionary<string, string> distributeDic = new Dictionary<string, string> {
                { "MaterialID","物料ID"},
                { "Title","标题"},
                { "Url","落地页URL"},
                { "MaterialTypeName","物料类型"},
                { "SceneName","场景"},
                { "AccountScore","头部文章所属账号分值"},
                { "ArticleScore","头部文章分值"},
                { "BrandNmae","品牌"},
                { "SerialName","车型"},
                { "EncapsulateTime","封装时间"},
                { "DistributeTime","分发时间"},
                { "ChannelName","分发渠道类型"}
            };
            //转发明细列表表头
            Dictionary<string, string> forwardDic = new Dictionary<string, string> {
                { "ForwardStatisticsTime","统计时间"},
                { "MaterialID","物料ID"},
                { "Title","标题"},
                { "Url","落地页URL"},
                { "MaterialTypeName","物料类型"},
                { "EncapsulateTime","封装时间"},
                { "DistributeTime","分发时间"},
                { "ChannelName","分发渠道类型"},
                { "SceneName","场景"},
                { "AccountScore","头部文章所属账号分值"},
                { "ArticleScore","头部文章分值"},
                { "BrandNmae","品牌"},
                { "SerialName","车型"},

            };
            //线索明细列表表头
            Dictionary<string, string> clueDic = new Dictionary<string, string> {
                { "ClueDate","数据时间"},
                { "MaterialID","物料ID"},
                { "Title","标题"},
                { "Url","落地页URL"},
                { "MaterialName","物料类型"},
                { "SceneName","场景"},
                { "AccountScore","账号分值"},
                { "ArticleScore","头部文章分值"},
                { "ChannelName","分发渠道"},
                { "BrandNmae","品牌"},
                { "SerialName","车型"},
                { "InqueryCount","询价数"},
                { "SessionCount","会话数"},
                { "TelConnectCount","车型"}
            };
            dicList.Add(ExcelEnum.fz_detail.ToString(), encapsulateDic);
            dicList.Add(ExcelEnum.xs_detail.ToString(), clueDic);
            dicList.Add(ExcelEnum.ff_detail.ToString(), distributeDic);
            dicList.Add(ExcelEnum.zf_detail.ToString(), forwardDic);
            dicList.Add(ExcelEnum.fz.ToString(), null);
            dicList.Add(ExcelEnum.xs.ToString(), null);
            dicList.Add(ExcelEnum.ff.ToString(), null);
            dicList.Add(ExcelEnum.zf.ToString(), null);
            return dicList;
        }


        /// <summary>
        /// 获取枚举信息
        /// </summary>
        /// <returns></returns>
        public string GetEnumDescription(string enumName)
        {
            return Enum.GetValues(typeof(ExcelEnum)).OfType<Enum>().Where(m => m.ToString() == enumName).Select(x => new
            {
                Key = Convert.ToInt32(x),
                Value = x.ToString(),
                Description = x.GetDescription()
            }).FirstOrDefault().Description;
        }

    }
}
