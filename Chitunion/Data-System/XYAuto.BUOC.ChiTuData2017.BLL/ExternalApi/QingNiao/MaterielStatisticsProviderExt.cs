/********************************************************
*创建人：lixiong
*创建时间：2017/10/30 16:49:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.QingNiao;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.QingNiao;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.QingNiao
{
    public class MaterielStatisticsProviderExt : VerifyOperateBase
    {
        private string _queryDay;
        private readonly ConfigEntity _configEntity;
        private readonly PullDataConfig _pullDataConfig;
        private DateTime _todyDateTime;

        public MaterielStatisticsProviderExt(PullDataConfig pullDataConfig)
        {
            _configEntity = new ConfigEntity();
            var dt = DateTime.Now;
            _pullDataConfig = pullDataConfig;
            _queryDay = DateTime.Now.AddDays(pullDataConfig.DateOffset).ToString("yyyy-MM-dd");
            _todyDateTime = DateTime.Parse(_queryDay).AddHours(dt.Hour).AddMinutes(dt.Minute).AddSeconds(dt.Second);
        }

        public void LoopPullStatistics()
        {
            //todo:现在需要直接找到-34天之前的时间，然后在-34的时间点上往后 遍历 10天或者15天
            var loopEnd = Math.Abs(_pullDataConfig.DateOffset);//34
            var startDate = _todyDateTime;
            Loger.Log4Net.Info($" 全网域..拉取时间节点为：{loopEnd}天 开始时间：{startDate} 执行开始...");
            for (var i = 0; i < loopEnd; i++)
            {
                _todyDateTime = startDate.AddDays(i);
                _queryDay = _todyDateTime.ToString("yyyy-MM-dd");
                //进行数据组装
                DoPullRunning(_todyDateTime);
                Loger.Log4Net.Info($" 全网域..拉取时间节点为：{loopEnd}天 {_queryDay} 已执行完成");
            }
            Loger.Log4Net.Info($" 全网域..拉取时间节点为：{loopEnd}天 开始时间：{startDate} 执行结束{_queryDay}...");
        }

        public void DoPullRunning(DateTime queryDateTime)
        {
            //验证时间  //这里应该不用判断当前时间与拉取时间的校验，理论上同步表里面有数据则拉取（有一种情况，就是当天会有陆续进来数据）
            var currentDateTime = Convert.ToDateTime(queryDateTime.ToString("yyyy-MM-dd"));//startDays.AddDays(i);
            Loger.Log4Net.Info($" chitu全网域 .. DoPullRunning 循环拉取物料统计信息， {currentDateTime}开始..");

            //todo:现在是需要查找过去8天的数据

            if ((DateTime.Now - queryDateTime).Days < 1)
            {
                Loger.Log4Net.Info($" chitu全网域 .. DoPullRunning 循环拉取物料统计信息， {currentDateTime}  不满足条件继续拉取数据...终止");
                return;//证明是昨天或者今天的数据，不拉取
            }

            if (!CopyDataToMateriel(currentDateTime.ToString("yyyy-MM-dd")))
            {
                Loger.Log4Net.Info($" chitu全网域 .. DoPullRunning 循环拉取物料统计信息 {currentDateTime.ToString("yyyy-MM-dd") }" +
                                    $"  CopyDataToMateriel同步物料表失败,任务终止..");
                return;
            }

            //todo:找到渠道列表，入库
            ChituChannelStatQuery(currentDateTime);
            //todo:统计明细
            ChituClickStatQuery(currentDateTime);

            Loger.Log4Net.Info($" qingniao全网域 .. DoPullRunning 循环拉取物料统计信息， {currentDateTime} 结束..");
        }

        /// <summary>
        /// 物料分发明细表
        /// </summary>
        /// <param name="queryDateTime"></param>
        /// <returns></returns>
        private bool CopyDataToMateriel(string queryDateTime)
        {
            try
            {
                var excuteId = Dal.QingNiao.ChituMaterialStat.Instance.CopyDataToMateriel(queryDateTime);

                if (excuteId <= 0)
                {
                    Loger.Log4Net.Error($"CopyDataToMateriel 失败！{queryDateTime}");
                    return false; //CreateFailMessage(retValue, "20001", $"CopyDataToMateriel 失败！{queryDateTime}");
                }
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($" CopyDataToMateriel 失败 错误：{exception.Message} {exception.StackTrace ?? string.Empty}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 渠道信息
        /// </summary>
        /// <param name="queryDateTime"></param>
        public void ChituChannelStatQuery(DateTime queryDateTime)
        {
            var providerQuery = new ChituChannelStatQuery(_configEntity);

            var query = new RequestChituChannelDto
            {
                PageIndex = 1,
                Date = queryDateTime.ToString("yyyy-MM-dd")
            };
            var resp = providerQuery.GetQueryList(query);
            var list = AutoMapper.Mapper.Map<List<RespChituChannelDto>,
                    List<Entities.Distribute.MaterielChannelDetailed>>(resp.List);

            list.ForEach(s =>
            {
                if (Dal.Distribute.MaterielChannelDetailed.Instance.Insert(s, queryDateTime) <= 0)
                {
                    Loger.Log4Net.Error($"ChituChannelStatQuery page:{query.PageIndex} {query.Date}  插入失败：{JsonConvert.SerializeObject(s)}");
                }
            });

            if (resp.TotalCount == query.PageIndex)
            {
                return;
            }
            var pageTotle = GetOffsetPageCount(resp.TotalCount, query.PageSize);
            var page = query.PageIndex++;
            for (var i = page; i <= pageTotle; i++)
            {
                query.PageIndex = i;
                resp = providerQuery.GetQueryList(query);
                list = AutoMapper.Mapper.Map<List<RespChituChannelDto>,
                    List<Entities.Distribute.MaterielChannelDetailed>>(resp.List);
                list.ForEach(s =>
                {
                    if (Dal.Distribute.MaterielChannelDetailed.Instance.Insert(s, queryDateTime) <= 0)
                    {
                        Loger.Log4Net.Info($"ChituChannelStatQuery page:{query.PageIndex} {query.Date}  插入失败：{JsonConvert.SerializeObject(s)}");
                    }
                });
            }
        }

        /// <summary>
        /// 统计信息
        /// </summary>
        public void ChituClickStatQuery(DateTime queryDateTime)
        {
            //todo:1.查询第一步导入的物料(赤兔的)，依次去chitu_click_stat 表里面取点击明细统计
            var query = new RequestDistributeQueryDto()
            {
                StartDate = queryDateTime.ToString("yyyy-MM-dd"),
                EndDate = queryDateTime.ToString("yyyy-MM-dd"),
                DistributeType = (int)DistributeTypeEnum.QuanWangYu,
                PageSize = 200
            };
            var resp = new DistributeDetailedQuery(_configEntity).GetQueryList(query);
            if (resp.List == null || !resp.List.Any())
            {
                Loger.Log4Net.Info($" qing niao 入库点击数据 没有数据，{query.StartDate} ChituClickStatQuery DistributeDetailedQuery,终止..");
                return;
            }
            var totleNumber = resp.TotalCount;
            var offestCount = GetOffsetPageCount(totleNumber, query.PageSize);

            var queryClick = new DistributeQuery<ChituClickStat>()
            {
                StartDate = queryDateTime.ToString("yyyy-MM-dd"),
                EndDate = queryDateTime.ToString("yyyy-MM-dd"),
            };
            resp.List.ForEach(item =>
            {
                queryClick.MaterielId = item.MaterielId;
                if (!VerifyOfStatistics(item.DistributeId))
                {
                    Loger.Log4Net.Info($" qing niao 入库点击数据 VerifyOfStatistics 已经存在，DistributeId：{item.DistributeId} 已经存在..");
                    return;
                }
                var clickStatList = Dal.QingNiao.ChituClickStat.Instance.GetList(queryClick);
                //todo:组装数据，批量插入
                //item.DistributeId
                //文章id ？？
                InsertStatistics(clickStatList, item.DistributeId, queryDateTime);
            });
            query.PageIndex++;
            while (offestCount >= query.PageIndex)
            {
                resp = new DistributeDetailedQuery(_configEntity).GetQueryList(query);
                if (resp.List == null)
                {
                    break;
                }
                resp.List.ForEach(item =>
                {
                    queryClick.MaterielId = item.MaterielId;
                    if (!VerifyOfStatistics(item.DistributeId))
                    {
                        return;
                    }
                    var clickStatList = Dal.QingNiao.ChituClickStat.Instance.GetList(queryClick);
                    //todo:组装数据，批量插入
                    //item.DistributeId
                    //文章id ？？
                    InsertStatistics(clickStatList, item.DistributeId, queryDateTime);
                });
                query.PageIndex++;
            }
        }

        private bool VerifyOfStatistics(int distributeId)
        {
            var statisticsList = Dal.Distribute.MaterielDetailedStatistics.Instance.GetList(distributeId, new List<int>());
            if (statisticsList.Any())
            {
                Loger.Log4Net.Error($"qing niao InsertStatistics 校验 已经存在:distributeId={distributeId}");
                return false;
            }
            return true;
        }

        private void InsertStatistics(List<Entities.QingNiao.ChituClickStat> clickStats, int distributeId, DateTime queryDateTime)
        {
            var list =
                AutoMapper.Mapper.Map<List<Entities.QingNiao.ChituClickStat>, List<Entities.Distribute.MaterielDetailedStatistics>>(clickStats);

            if (Dal.Distribute.MaterielDetailedStatistics.Instance.Insert(list, distributeId, queryDateTime) <= 0)
            {
                Loger.Log4Net.Error($"qing niao InsertStatistics 失败");
            }
        }
    }
}