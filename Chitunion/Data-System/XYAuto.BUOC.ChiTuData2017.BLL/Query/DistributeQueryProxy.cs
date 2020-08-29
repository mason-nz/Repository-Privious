/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 18:13:45
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query
{
    /// <summary>
    /// auth:lixiong
    /// desc:日结数据汇总查询
    /// </summary>
    public class DistributeQueryProxy : CurrentOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestDistributeQueryDto _requestDistributeQuery;
        private readonly Lazy<Dictionary<int, Func<string, dynamic>>> _lazyQueryDic;//lazy加载

        public DistributeQueryProxy(ConfigEntity configEntity, RequestDistributeQueryDto requestDistributeQuery)
        {
            _configEntity = configEntity;
            _requestDistributeQuery = requestDistributeQuery;
            _lazyQueryDic = new Lazy<Dictionary<int, Func<string, dynamic>>>(InitQuery);
        }

        private Dictionary<int, Func<string, dynamic>> InitQuery()
        {
            return new Dictionary<int, Func<string, dynamic>>()
            {
                { (int)DistributeTypeEnum.QuanWangYu, s =>
                { _requestDistributeQuery.DistributeType =(int)DistributeTypeEnum.QuanWangYu;
                   return new DistributeDetailedChannelQuery(_configEntity).GetQueryList(_requestDistributeQuery);
                } },
                { (int)DistributeTypeEnum.QingNiaoAgent , s =>
                {
                    _requestDistributeQuery.DistributeType =(int)DistributeTypeEnum.QingNiaoAgent;
                    return new DistributeDetailedQuery(_configEntity).GetQueryList(_requestDistributeQuery);
                } }
            };
        }

        public dynamic GetQuery()
        {
            return _lazyQueryDic.Value.ContainsKey(_requestDistributeQuery.DistributeType)
                  ? _lazyQueryDic.Value[_requestDistributeQuery.DistributeType].Invoke(string.Empty)
                  : CreateFailMessage(new ReturnValue(), "10001", "请输入合法的 DistributeType");
        }

        public static string GetDistributeUser(int fromType)
        {
            var dic = new Dictionary<int, string>()
            {
                { (int)DistributeTypeEnum.QingNiaoAgent,"青鸟-经纪人系统" },
                { (int)DistributeTypeEnum.QuanWangYu,"全网域" },
            };
            return dic.FirstOrDefault(s => s.Key == fromType).Value ?? "未知";
        }
    }
}