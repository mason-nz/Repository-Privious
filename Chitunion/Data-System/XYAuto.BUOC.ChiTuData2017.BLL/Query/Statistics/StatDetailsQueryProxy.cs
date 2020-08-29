/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 19:55:47
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
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics
{
    /// <summary>
    /// auth:lixiong
    /// desc:明细数据代理类
    /// </summary>
    public class StatDetailsQueryProxy : CurrentOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqDetailsDto _reqDetailsDto;
        private readonly Lazy<Dictionary<string, Func<string, dynamic>>> _lazyQueryDic;//lazy加载

        public StatDetailsQueryProxy(ConfigEntity configEntity, ReqDetailsDto reqDetailsDto)
        {
            _configEntity = configEntity;
            _reqDetailsDto = reqDetailsDto;
            _lazyQueryDic = new Lazy<Dictionary<string, Func<string, dynamic>>>(InitQuery);
        }

        private Dictionary<string, Func<string, dynamic>> InitQuery()
        {
            return new Dictionary<string, Func<string, dynamic>>()
                {
                    { GetDailyTypeEnum.grab.ToString(), s =>  new StatDetailsGrabQuery(_configEntity).GetQueryList(_reqDetailsDto)},
                    { GetDailyTypeEnum.jxrk.ToString(), s =>  new StatDetailsJxQuery(_configEntity).GetQueryList(_reqDetailsDto)},
                    { GetDailyTypeEnum.cxpp.ToString(), s =>  new StatDetailsCarMatchQuery(_configEntity).GetQueryList(_reqDetailsDto)},
                    { GetDailyTypeEnum.cs.ToString(), s =>  new StatDetailsCsQuery(_configEntity).GetQueryList(_reqDetailsDto)},
                    { GetDailyTypeEnum.rgqx.ToString(), s =>  new StatDetailsRgqxQuery(_configEntity).GetQueryList(_reqDetailsDto)}
                };
        }

        public dynamic GetQuery()
        {
            return _lazyQueryDic.Value.ContainsKey(_reqDetailsDto.TabType)
                  ? _lazyQueryDic.Value[_reqDetailsDto.TabType].Invoke(string.Empty)
                  : CreateFailMessage(new ReturnValue(), "10001", "请输入合法的 TabType");
        }
    }

    public enum QueryTypeEnum
    {
        [Description("导出数据查询（不需要分页）")]
        Export,

        [Description("分页查询")]
        QueryPage
    }
}