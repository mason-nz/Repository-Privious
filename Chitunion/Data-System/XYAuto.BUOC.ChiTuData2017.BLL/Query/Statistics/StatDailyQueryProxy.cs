/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 19:46:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics
{
    /// <summary>
    /// auth:lixiong
    /// desc:日汇总数据代理类
    /// </summary>
    public class StatDailyQueryProxy : CurrentOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqDailyDto _reqDailyDto;
        private readonly Lazy<Dictionary<string, Func<string, dynamic>>> _lazyQueryDic;//lazy加载

        public StatDailyQueryProxy(ConfigEntity configEntity, ReqDailyDto reqDailyDto)
        {
            _configEntity = configEntity;
            _reqDailyDto = reqDailyDto;
            _lazyQueryDic = new Lazy<Dictionary<string, Func<string, dynamic>>>(InitQuery);
        }

        private Dictionary<string, Func<string, dynamic>> InitQuery()
        {
            return new Dictionary<string, Func<string, dynamic>>()
            {
                { GetDailyTypeEnum.grab.ToString(), s =>  new StatDailyGrabQuery(_configEntity ).GetQueryList(_reqDailyDto)},
                { GetDailyTypeEnum.jxrk.ToString(), s =>  new StatDailyJxQuery(_configEntity ).GetQueryList(_reqDailyDto)},
                { GetDailyTypeEnum.cxpp.ToString(), s =>  new StatDailyCarMatchQuery(_configEntity ).GetQueryList(_reqDailyDto)},
                { GetDailyTypeEnum.cs.ToString(), s =>  new StatDailyCsQuery(_configEntity ).GetQueryList(_reqDailyDto)},
                { GetDailyTypeEnum.rgqx.ToString(), s =>  new StatDailyRgqxQuery(_configEntity ).GetQueryList(_reqDailyDto)}
            };
        }

        public dynamic GetQuery()
        {
            return _lazyQueryDic.Value.ContainsKey(_reqDailyDto.TabType)
                  ? _lazyQueryDic.Value[_reqDailyDto.TabType].Invoke(string.Empty)
                  : CreateFailMessage(new ReturnValue(), "10001", "请输入合法的 TabType");
        }
    }

    public enum GetDailyTypeEnum
    {
        [Description("抓取")]
        grab,

        [Description("机洗入库")]
        jxrk,

        [Description("车型匹配")]
        cxpp,

        [Description("初筛")]
        cs,

        [Description("人工清洗")]
        rgqx
    }
}