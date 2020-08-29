/********************************************************
*创建人：lixiong
*创建时间：2017/9/14 14:30:49
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
    public class MaterielStatisticsProvider : VerifyOperateBase
    {
        private string _queryDay;
        private readonly ConfigEntity _configEntity;
        private readonly PullDataConfig _pullDataConfig;
        private DateTime _todyDateTime;

        public MaterielStatisticsProvider(PullDataConfig pullDataConfig)
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
            for (var i = 0; i <= loopEnd - 1; i++)
            {
                _todyDateTime = startDate.AddDays(i);
                _queryDay = _todyDateTime.ToString("yyyy-MM-dd");
                PullStatistics(_queryDay, _todyDateTime);
                Loger.Log4Net.Info($" 全网域..拉取时间节点为：{loopEnd}天 {_queryDay} 已执行完成");
            }
            Loger.Log4Net.Info($" 全网域..拉取时间节点为：{loopEnd}天 开始时间：{startDate} 执行结束{_queryDay}...");
        }

        public ReturnValue PullStatistics(string queryDate, DateTime queryDateTime)
        {
            //todo:1.先把物料（chitu_material_stat）表里面的前一天数据全部同步到Chitunion_OP2017.dbo.MaterielExtend表中
            //todo:2.再遍历渠道表（chitu_channel_stat）里前一天的数据，查询到与MaterielChannel表相关联的信息（PromotionUrlCode）找到物料信息，以及渠道id
            //todo:3.既步骤2之后，将数据组装好，入库到物料分发-渠道明细表（Materiel_ChannelDetailed）
            //todo:4.

            var retValue = new ReturnValue();

            //todo:必须是同步，因为ChituChannelStatQuery 查询的时候用到了 DistributeId
            var chituQuery = new RequestChituChannelDto()
            {
                Date = queryDate
            };
            var provider = new ChituMaterialStatQuery(new ConfigEntity());
            var materielList = provider.GetQueryList(chituQuery);

            if (materielList.List == null || materielList.List.Count == 0)
            {
                Loger.Log4Net.Error($" 全网域 获取物料信息失败 ：PageIndex={chituQuery.PageIndex} {_queryDay}，同步终止..");
                return CreateFailMessage(retValue, "20001", $" 全网域 获取物料信息失败 ：{_queryDay}，同步终止..");
            }
            //进行数据组装
            DoPullRunning(materielList.List, chituQuery, queryDateTime);

            var totleNumber = materielList.TotalCount;
            var offestCount = GetOffsetPageCount(totleNumber, chituQuery.PageSize);

            while (offestCount >= chituQuery.PageIndex)
            {
                materielList = provider.GetQueryList(chituQuery);

                if (materielList.List == null || materielList.List.Count == 0)
                {
                    Loger.Log4Net.Error($" 全网域 获取物料信息失败 ：PageIndex={chituQuery.PageIndex} {_queryDay}，同步终止..");
                    break;
                }
                //进行数据组装
                DoPullRunning(materielList.List, chituQuery, queryDateTime);
            }
            return retValue;
        }

        private void DoPullRunning(List<MaterielDistributeDetailed> materielList,
            RequestChituChannelDto chituQuery, DateTime queryDateTime)
        {
            Loger.Log4Net.Info($" chitu全网域 .. DoPullRunning 循环拉取物料统计信息，PageIndex={chituQuery.PageIndex} {chituQuery.Date}开始..");

            //todo:现在是需要查找过去8天的数据

            var requestQuery = new RequestChituChannelDto()
            {
            };

            var startDays = queryDateTime;
            for (var i = 0; i <= _pullDataConfig.PullDataQueryDateOffset; i++)
            {
                var currentDateTime = startDays.AddDays(i);
                //这里应该不用判断当前时间与拉取时间的校验，理论上同步表里面有数据则拉取（有一种情况，就是当天会有陆续进来数据）

                materielList.ForEach(s =>
                {
                    //todo:分发表，入库
                    var info = AutoMapper.Mapper
                         .Map<Entities.Distribute.MaterielDistributeDetailed, Entities.QingNiao.ChituMaterialStat>(s);
                    info.CreateTime = currentDateTime;
                    if (Dal.QingNiao.ChituMaterialStat.Instance.Insert(info, (int)DistributeTypeEnum.QuanWangYu) <= 0)
                    {
                        Loger.Log4Net.Error($" 全网域 Insert MaterielDistributeDetailed 失败 ：{_queryDay}，参数：{JsonConvert.SerializeObject(info)}");
                        return;
                    }
                });
                //todo:找到渠道列表，入库
                requestQuery.Date = currentDateTime.ToString("yyyy-MM-dd");
                ChituChannelStatQuery(requestQuery, currentDateTime);
                //todo:统计明细
                ChituClickStatQuery(currentDateTime);

                startDays = queryDateTime;
            }
            chituQuery.PageIndex++;
            Loger.Log4Net.Info($" qingniao全网域 .. DoPullRunning 循环拉取物料统计信息，PageIndex={chituQuery.PageIndex} {chituQuery.Date}开始..");
        }

        /// <summary>
        /// 物料分发明细表
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue CopyDataToMateriel(ReturnValue retValue)
        {
            var excuteId = Dal.QingNiao.ChituMaterialStat.Instance.CopyDataToMateriel(_queryDay);

            if (excuteId <= 0)
            {
                Loger.Log4Net.Error($"CopyDataToMateriel 失败！{_queryDay}");
                return CreateFailMessage(retValue, "20001", $"CopyDataToMateriel 失败！{_queryDay}");
            }

            return retValue;
        }

        /// <summary>
        /// 渠道信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDateTime"></param>
        public void ChituChannelStatQuery(RequestChituChannelDto query, DateTime queryDateTime)
        {
            var providerQuery = new ChituChannelStatQuery(_configEntity);

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
                        Loger.Log4Net.Error($"ChituChannelStatQuery page:{query.PageIndex} {query.Date}  插入失败：{JsonConvert.SerializeObject(s)}");
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
            var resp = new DistributeDetailedQuery(_configEntity).GetQueryList(new RequestDistributeQueryDto()
            {
                EndDate = queryDateTime.ToString("yyyy-MM-dd"),
                DistributeType = (int)DistributeTypeEnum.QuanWangYu
            });
            if (resp.List == null)
            {
                return;
            }
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
                    return;
                }
                var clickStatList = Dal.QingNiao.ChituClickStat.Instance.GetList(queryClick);
                //todo:组装数据，批量插入
                //item.DistributeId
                //文章id ？？
                InsertStatistics(clickStatList, item.DistributeId, queryDateTime);
            });
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