/********************************************************
*创建人：lixiong
*创建时间：2017/9/13 10:42:15
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.Op;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.Op.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Http;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.ZHY
{
    /// <summary>
    /// 拉取智慧云物料统计信息
    /// </summary>
    public class LogicByMaterielStatistics : VerifyOperateBase
    {
        private readonly MaterielStatisticsProvider _contextProvider;
        private readonly PullDataConfig _pullDataConfig;
        private readonly string _dayQuery;
        private readonly DateTime _todyDateTime;
        public int RequestApiCount = 0;
        public int RequestApiNotDataCount = 0;

        public LogicByMaterielStatistics(Infrastruction.Http.DoHttpClient doHttpClient,
            PullDataConfig pullDataConfig)
        {
            var dt = DateTime.Now;
            _pullDataConfig = pullDataConfig;
            _dayQuery = dt.AddDays(pullDataConfig.DateOffset).ToString("yyyy-MM-dd");
            _todyDateTime = DateTime.Parse(_dayQuery).AddHours(dt.Hour).AddMinutes(dt.Minute).AddSeconds(dt.Second);
            _contextProvider = new MaterielStatisticsProvider(doHttpClient);
        }

        public void LoopPullStatistics()
        {
            var loopCount = Math.Abs(_pullDataConfig.DateOffset);
            var startTime = DateTime.Now.AddDays(_pullDataConfig.DateOffset);
            var dt = DateTime.Now;

            var requestDto = new PullStatisticsDto();
            var retValue = new ReturnValue();
            //todo:时间直接定位到-35天时间点，然后拉取到当前时间前一天
            for (var i = 0; i < loopCount; i++)
            {
                DoItemLoopPullStatistics(
                    null, i, startTime, loopCount, requestDto, retValue);
            }
        }

        public void LoopPullStatisticsTask()
        {
            var loopCount = Math.Abs(_pullDataConfig.DateOffset);
            var startTime = DateTime.Now.AddDays(_pullDataConfig.DateOffset);

            var requestDto = new PullStatisticsDto();
            var retValue = new ReturnValue();
            var cts = new CancellationTokenSource();
            var ct = cts.Token;
            //todo:时间直接定位到-35天时间点，然后拉取到当前时间前一天
            for (var i = 0; i < loopCount; i++)
            {
                var i1 = i;
                Task.Run(() => DoItemLoopPullStatistics(
                    () => ct.ThrowIfCancellationRequested()
                , i1, startTime, loopCount, requestDto, retValue), ct);
            }
        }

        public void DoItemLoopPullStatistics(Action cancelTaskAction, int dateIndex, DateTime startTime,
            int loopCount, PullStatisticsDto requestDto, ReturnValue retValue)
        {
            var currentTime = startTime.AddDays(dateIndex).ToString("yyyy-MM-dd");
            var dt = DateTime.Now;
            var materielIds = Dal.Distribute.MaterielDistributeQingNiaoAgent.Instance.GetPullDateMaterielIds(currentTime);
            if (!materielIds.Any())
            {
                Loger.ZhyLogger.Info($" zhy .. {currentTime} GetPullDateMaterielIds获取 分发明细最小时间为null，数据不存在，终止..");
                //ct.ThrowIfCancellationRequested();//取消线程
                if (cancelTaskAction != null)
                    cancelTaskAction.Invoke();
                return;
            }

            Loger.Log4Net.Info($" task to zhy..拉取时间节点为：{loopCount}天 开始时间：{currentTime} 执行开始...");
            Loger.ZhyLogger.Info($"task to zhy..拉取时间节点为：{loopCount}天 开始时间：{currentTime} 执行开始...");
            materielIds.ForEach(s =>
            {
                //todo:查询出当前物料所有的时间信息(倒叙10个)
                var dates = Dal.Distribute.MaterielDistributeDetailed.Instance.GetInfoByPullDataVerifyMaterile(_pullDataConfig.PullDataQueryDateOffset,
                     s.MaterielId, (int)DistributeTypeEnum.QingNiaoAgent);
                var minStartDate = Convert.ToDateTime(s.DistributeDate.ToString("yyyy-MM-dd"));

                Loger.ZhyLogger.Info($" task to zhy .. DoPullRunning 循环拉取物料统计信息，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                             $"当前物料最小时间：{minStartDate} 物料：{s.MaterielId} 开始");
                //todo:s.DistributeDate 时间往后10天进行拉取，和已有的数据进行判断过滤
                for (var j = 0; j < _pullDataConfig.PullDataQueryDateOffset; j++)
                {
                    var cutD = minStartDate.AddDays(j);
                    var cut = cutD.ToString("yyyy-MM-dd");
                    if (dates.Any(t => t.Date.ToString("yyyy-MM-dd").Equals(cut)))
                    {
                        Loger.ZhyLogger.Info($" task to zhy .. DoPullRunning 循环拉取物料统计信息，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                            $"  拉取的时间:{cut} 已经存在了，物料：{s.MaterielId} 不满足条件继续拉取数据...终止");

                        continue;
                    }
                    //验证时间(分发时间比较)
                    if ((dt - cutD).Days < 1)
                    {
                        Loger.ZhyLogger.Info($" task to zhy .. DoPullRunning 循环拉取物料统计信息，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                             $"当前时间：{dt} 减去时间:{cutD}={(dt - cutD).Days} Days <= 1 物料：{s.MaterielId} 不满足条件继续拉取数据...终止");
                        break;//证明是昨天或者今天的数据，不拉取
                    }

                    //todo:如果剩下没有重复存在，则补全该日期的数据
                    //验证是否已经存在
                    requestDto.MaterielId = s.MaterielId;
                    requestDto.HeadArticleId = s.ArticleId;
                    requestDto.DateTime = cut;
                    Loger.ZhyLogger.Info($" task to zhy .. DoPullRunning 循环拉取物料统计信息，拉取分发时间加 { _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                      $"第{j}天，{requestDto.DateTime} 物料：{s.MaterielId} 开始..");
                    retValue = VerifyDistribute(retValue, requestDto.MaterielId, requestDto.DateTime);
                    if (retValue.HasError)
                    {
                        Loger.ZhyLogger.Info($" task to zhy .. DoPullRunning 循环拉取物料统计信息 ，拉取分发时间加 { _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                         $"第{j}天，{requestDto.DateTime} 物料：{s.MaterielId} 已经存在..");
                        continue;
                    }
                    //添加统计信息
                    RequestApiCount++;
                    RespMaterielDto respStatisticsInfo;
                    DoInsertBefore(retValue, requestDto, out respStatisticsInfo);
                    if (respStatisticsInfo == null)
                    {
                        //todo:记录汇总，发送邮件
                        RequestApiNotDataCount++;
                        continue;
                    }

                    respStatisticsInfo.MaterielId = s.MaterielId;
                    retValue = DoInsert(retValue, respStatisticsInfo, requestDto.DateTime);
                    retValue.HasError = false;
                    Loger.ZhyLogger.Info($" task to zhy .. DoPullRunning 循环拉取物料统计信息，拉取分发时间加 { _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                  $"第{j}天，{requestDto.DateTime} 物料：{s.MaterielId} 结束..");
                }
                Loger.ZhyLogger.Info($" task to zhy .. DoPullRunning 循环拉取物料统计信息，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                             $"当前物料最小时间：{minStartDate} 物料：{s.MaterielId} 结束");
            });

            Loger.ZhyLogger.Info($" task to zhy..拉取时间节点为：{loopCount}天 开始时间：{startTime} 执行结束...");
            Loger.Log4Net.Info($" task to zhy..拉取时间节点为：{loopCount}天 开始时间：{startTime} 执行结束...");
        }

        /// <summary>
        /// 拉取物料统计
        /// </summary>
        /// <returns></returns>
        public ReturnValue PullStatistics()
        {
            //todo:1.先获取物料列表，循环用头部文章id去拉取智慧云接口数据 （？？因为拉取数据参数是头部文章和昨天的日期，头部文章id列表在哪里获取）
            //todo:2.返回json数据中，头部文章数据入库到：物料分发明细表（Materiel_DistributeDetailed）
            //todo:3.返回json数据中，物料包本页（属于头部文章）入库到：物料详情数据统计明细表（Materiel_DetailedStatistics）
            //todo:4.返回json数据中，腰页入库到：物料详情数据统计明细表（Materiel_DetailedStatistics）
            //todo:5.物料包本页,腰页 里面的点击明细入库到：点击明细，转发明细（Materiel_OperateDetailed）
            var retValue = new ReturnValue();
            var requestDto = new PullStatisticsDto()
            {
                DateTime = _dayQuery
            };
            var providerQuery = new DistributeQingNiaoAgentQuery(new ConfigEntity());
            var qingNiaoAgentQuery = new QingNiaoAgentQueryDto()
            {
                DistributeQueryDateOffset = _pullDataConfig.DateOffset,
                PageIndex = 1,
                PageSize = 100
            };
            var distributeDetails = providerQuery.GetQueryList(qingNiaoAgentQuery);
            if (distributeDetails.List == null || !distributeDetails.List.Any())
                return CreateFailMessage(retValue, "20010", $"暂无分发明细：{qingNiaoAgentQuery.DistributeQueryDateOffset} 天内的，zhy拉取任务终止..");

            Loger.ZhyLogger.Info($" zhy ..利用分发明细 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} {qingNiaoAgentQuery.DistributeQueryDateOffset} 天内的 开始..");
            DoPullRunning(retValue, qingNiaoAgentQuery, distributeDetails.List, requestDto);
            Loger.ZhyLogger.Info($" zhy ..利用分发明细 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} {qingNiaoAgentQuery.DistributeQueryDateOffset} 天内的 结束..");

            var totleNumber = distributeDetails.TotalCount;
            var offestCount = GetOffsetPageCount(totleNumber, qingNiaoAgentQuery.PageSize);

            //todo:1.当前时间-for 循环里面的 min 时间，if > 1 则可以继续拉取
            //todo:2.然后 min 时间 + 8天（配置的维度） 进行拉取，判断是否存在
            qingNiaoAgentQuery.PageIndex++;
            while (offestCount >= qingNiaoAgentQuery.PageIndex)
            {
                Loger.ZhyLogger.Info($" zhy .. 利用分发明细 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} 开始..");
                distributeDetails = providerQuery.GetQueryList(qingNiaoAgentQuery);
                if (distributeDetails.List == null || !distributeDetails.List.Any())
                {
                    Loger.ZhyLogger.Info($" zhy ..利用分发明细 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} 没有数据，任务终止..");
                    break;
                }
                DoPullRunning(retValue, qingNiaoAgentQuery, distributeDetails.List, requestDto);
                Loger.ZhyLogger.Info($" zhy ..利用分发明细 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} 结束..");
            }

            return retValue;
        }

        private void DoPullRunning(ReturnValue retValue, QingNiaoAgentQueryDto qingNiaoAgentQuery,
           List<Entities.Distribute.MaterielDistributeQingNiaoAgent> articleList, PullStatisticsDto requestDto)
        {
            var dt = DateTime.Now;
            //todo:需要重新初始化时间
            var startDays = DateTime.Now.AddDays(_pullDataConfig.DateOffset);
            articleList.ForEach(at =>
            {
                //验证时间(分发时间比较)
                if ((dt - at.DistributeDate).TotalDays < 1)
                {
                    Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                         $"当前时间：{at.DistributeDate} 不满足条件继续拉取数据...终止");
                    return;//证明是昨天或者今天的数据，不拉取
                }
                for (var i = 0; i < _pullDataConfig.PullDataQueryDateOffset; i++)
                {
                    //验证是否已经存在
                    requestDto.MaterielId = at.MaterielId;
                    requestDto.DateTime = startDays.AddDays(i).ToString("yyyy-MM-dd");
                    Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                      $"第{i}天，{requestDto.DateTime} 开始..");
                    retValue = VerifyDistribute(retValue, requestDto.MaterielId, requestDto.DateTime);
                    if (retValue.HasError)
                    {
                        Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                         $"第{i}天，{requestDto.DateTime} 已经存在..");
                        continue;
                    }
                    //添加统计信息
                    RespMaterielDto respStatisticsInfo;
                    DoInsertBefore(retValue, requestDto, out respStatisticsInfo);

                    if (respStatisticsInfo == null || respStatisticsInfo.MaterielId <= 0)
                        continue;
                    retValue = DoInsert(retValue, respStatisticsInfo, requestDto.DateTime);
                    retValue.HasError = false;
                    Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                  $"第{i}天，{requestDto.DateTime} 结束..");
                }
                //todo:需要重新初始化时间
                startDays = DateTime.Now.AddDays(_pullDataConfig.DateOffset);
            });
            qingNiaoAgentQuery.PageIndex++;
        }

        public void LoopPullDistribute()
        {
            var loopCount = Math.Abs(_pullDataConfig.DateOffset);
            var startTime = DateTime.Now.AddDays(_pullDataConfig.DateOffset);
            var dt = DateTime.Now;
            for (var i = 0; i <= loopCount; i++)
            {
                var currentTime = startTime.AddDays(i - 1).ToString("yyyy-MM-dd");

                if ((dt - Convert.ToDateTime(currentTime)).Days < 0)
                {
                    continue;
                }

                Loger.ZhyLogger.Info($" zhy .. {currentTime} LoopPullDistribute 循环拉取数据分发明细，拉取时间节点为：{loopCount}天 开始时间：{currentTime} 执行开始..");

                PullDistribute(currentTime);

                Loger.ZhyLogger.Info($" zhy .. {currentTime} LoopPullDistribute 循环拉取数据分发明细，拉取时间节点为：{loopCount}天 开始时间：{currentTime} 执行结束..");
            }
        }

        /// <summary>
        /// 拉取分发详情明细
        /// </summary>
        /// <returns></returns>
        public ReturnValue PullDistribute(string queryDate)
        {
            //todo:1.请求接口，返回数据（头部文章id，日期date，分发时间）
            //todo:2.组装数据入库（Materiel_DistributeQingNiaoAgent），根据头部文章id获取物料id（后面需要用到物料id）
            var retValue = new ReturnValue();

            var detailsInfo = _contextProvider.PullMaterielDetails(queryDate);
            if (detailsInfo == null || !detailsInfo.Any())
            {
                Loger.ZhyLogger.Info($" zhy ... 拉取分发详情，没有数据:{queryDate},任务终止...");
                //SendWarin(queryDate);//发送邮件
                return CreateFailMessage(retValue, "20031", $" zhy ... 拉取分发详情，没有数据:{queryDate},任务终止...");
            }
            var count = detailsInfo.Count;

            detailsInfo = detailsInfo.Where(s => s.TaskId > 345).ToList();

            Loger.ZhyLogger.Info($" zhy ... 拉取分发详情，{queryDate} 数量：{count} 过滤掉 TaskId > 340 后:{detailsInfo.Count}");

            detailsInfo.ForEach(s =>
            {
                Entities.Distribute.MaterielDistributeQingNiaoAgent info = null;
                try
                {
                    info = AutoMapper.Mapper.Map<RespMaterielDetailDto, Entities.Distribute.MaterielDistributeQingNiaoAgent>(s);
                    Dal.Distribute.MaterielDistributeQingNiaoAgent.Instance.Insert(info);
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.Error($"zhy ... 拉取分发详情，MaterielDistributeQingNiaoAgent Insert 失败。参数:{ JsonConvert.SerializeObject(info)}" +
                                        $"错误信息：{exception.Message}{exception.StackTrace ?? string.Empty}");
                }
            });

            var pushDto = new PushDistributeDetailsDto()
            {
                DictId = (int)DistributeTypeEnum.QingNiaoAgent
            };
            //todo:推送
            var pushProvider = new PushMaterielDetailsProvider(new DoHttpClient());

            Task.Run(() =>
            {
                detailsInfo.ForEach(s =>
                {
                    pushDto.MaterielId = s.TaskId;
                    pushDto.SendTime = s.SendTime;
                    pushDto.TypeId = s.TypeId;
                    var respInfo = pushProvider.PushDistributeDetails(pushDto);
                });
            });

            return retValue;
        }

        private ReturnValue VerifyDistribute(ReturnValue retValue, int materielId, string queryDate)
        {
            var distributeDetailed = Dal.Distribute.MaterielDistributeDetailed.Instance.GetList(
                     new DistributeQuery<MaterielDistributeDetailed>()
                     {
                         MaterielId = materielId,
                         Date = queryDate,
                         Source = (int)DistributeTypeEnum.QingNiaoAgent
                     });
            if (distributeDetailed.Any())
            {
                Loger.Log4Net.Error($" zhy 拉取数据 VerifyDistribute 当前物料id在当前已经存在,MaterielId={ materielId}&Date={queryDate}");
                return CreateFailMessage(retValue, "20011", $" zhy 拉取数据 VerifyDistribute 当前物料id在当前已经存在,MaterielId={ materielId}&Date={queryDate}");
            }
            retValue.HasError = false;
            return retValue;
        }

        public ReturnValue DoInsertBefore(ReturnValue retValue, PullStatisticsDto requestDto, out RespMaterielDto respStatisticsInfo)
        {
            respStatisticsInfo = _contextProvider.PullStatistics(requestDto);
            if (respStatisticsInfo == null)
            {
                Loger.Log4Net.Info($"拉取数据为null,请求参数:{JsonConvert.SerializeObject(requestDto)}");
                return CreateFailMessage(retValue, "20001", $"拉取数据为null,请求参数:{JsonConvert.SerializeObject(requestDto)}");
            }

            return retValue;
        }

        /// <summary>
        /// 入库操作入口
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        public ReturnValue DoInsert(ReturnValue retValue, RespMaterielDto respMaterielDto, string queryDate)
        {
            var distributeId = InsertDistributeDetailed(respMaterielDto, queryDate);
            if (distributeId <= 0)
            {
                return CreateFailMessage(retValue, "20005", "InsertDistributeDetailed 入库失败");
            }
            InsertDetailedStatistics(retValue, respMaterielDto, distributeId, queryDate);

            return retValue;
        }

        /// <summary>
        /// 补全 物料信息
        /// </summary>
        /// <param name="respMaterielDto"></param>
        /// <returns></returns>
        public int AddToMaterielInfo(RespMaterielDto respMaterielDto)
        {
            /*
                物料id：-自建-
                文章标题：头部文章标题
                落地页URL：智慧云接口返回
                业务类型：青鸟-经纪人
                品牌/车型：取第一个腰部文章的
                场景：文章账号的场景
                分发渠道：——
                IP/子IP：——
                组装操作人：青鸟经纪人系统
                分发操作人：青鸟经纪人系统
             */
            //todo:1.创建新的物料信息，各个地方获取参数，补全相关字段信息
            //todo:2.获取车型品牌信息：腰部文章id找NLP2017.DBO.TR_ArticleCarMapping表
            //todo:3.获取场景信息：头部文章id找DataId（微信号），再去Chitunion_OP2017.DBO.ChannelAccount里面的SceneID

            var bodyArticleId = 0;
            if (respMaterielDto.WaistMaterial != null)
            {
                bodyArticleId = respMaterielDto.WaistMaterial.Select(s => s.ArticleId).FirstOrDefault();
            }
            var tp = Dal.Distribute.MaterielInfo.Instance.GetMaterielTemp(respMaterielDto.ArticleId, bodyArticleId);

            var materielId = Dal.Distribute.MaterielInfo.Instance.Insert(new MaterielExtend()
            {
                ArticleID = respMaterielDto.ArticleId,
                Resource = tp.Item2?.Resource ?? 0,
                Content = tp.Item2?.Content ?? string.Empty,
                SerialId = tp.Item1?.CSID ?? 0,
                SceneId = tp.Item2?.SceneId ?? 0,
                Status = 0,
                Title = tp.Item2?.Title ?? string.Empty,
                ArticleFrom = (int)DistributeTypeEnum.QingNiaoAgent,
                CreateTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                MaterielUrl = respMaterielDto.Url,
                CreateUserId = (int)MaterielCreateTypeEnum.QingNiaoAgent
            });

            return materielId;
        }

        private int InsertDistributeDetailed(RespMaterielDto respMaterielDto, string queryDate)
        {
            var excuteId = Dal.Distribute.MaterielDistributeDetailed.Instance.Insert(GetDistributeDetailedEntity(respMaterielDto, queryDate));

            if (excuteId <= 0)
            {
                Loger.Log4Net.Error($" InsertDistributeDetailed 入库失败：{JsonConvert.SerializeObject(GetDistributeDetailedEntity(respMaterielDto, queryDate))}");
            }
            return excuteId;
        }

        /// <summary>
        /// 入库详情统计（日结汇总之下）
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="distributeId"></param>
        /// <returns></returns>
        private ReturnValue InsertDetailedStatistics(ReturnValue retValue, RespMaterielDto respMaterielDto, int distributeId, string queryDate)
        {
            var list = new List<MaterielDetailedStatistics>();
            var listOperateDetailed = new List<MaterielOperateDetailed>();
            //获取统计信息
            GetDetailedStatisticsEntity(list, respMaterielDto, distributeId);

            //获取统计点击明细详情
            GetStatisticsOperateDetailedEntity(listOperateDetailed, respMaterielDto);

            Loger.ZhyLogger.Info(JsonConvert.SerializeObject(listOperateDetailed));

            //1个头部多个腰部
            list.ForEach(s =>
            {
                var statisticsId = Dal.Distribute.MaterielDetailedStatistics.Instance.Insert(s);
                //找到标识用的文章id：ArticleId 与 头部，腰部 组合查找
                var insertList = listOperateDetailed.Where(x => x.ArticleId == s.ArticleId).ToList();
                //将StatisticsId赋值
                insertList.ForEach(t => t.StatisticsId = statisticsId);
                Dal.Distribute.MaterielOperateDetailed.Instance.Insert(insertList);
            });

            return retValue;
        }

        private Entities.Distribute.MaterielDistributeDetailed GetDistributeDetailedEntity(RespMaterielDto respMaterielDto, string queryDate)
        {
            return new Entities.Distribute.MaterielDistributeDetailed()
            {
                MaterielId = respMaterielDto.MaterielId,
                BrowsePageAvg = DistributeProfile.GetBrowsePageAvg(respMaterielDto.PV, respMaterielDto.UV, 0),
                Date = Convert.ToDateTime(queryDate),
                PV = respMaterielDto.PV,
                UV = respMaterielDto.UV,
                //OnLineAvgTime = 0,
                JumpProportion = respMaterielDto.JumpChance,
                InquiryNumber = respMaterielDto.Clue?.InquiryCount ?? 0,
                SessionNumber = respMaterielDto.Clue?.ConversationCount ?? 0,
                TelConnectNumber = respMaterielDto.Clue?.PhoneClueCount ?? 0,
                Source = (int)DistributeTypeEnum.QingNiaoAgent,
                CreateUserId = (int)MaterielCreateTypeEnum.QingNiaoAgent,//智慧云
                CreateTime = DateTime.Now,
                DistributeUrl = respMaterielDto.Url,
                DistributeDetailType = (int)DistributeTypeEnum.QingNiaoAgent
            };
        }

        /// <summary>
        /// 获取详情统计信息（日结汇总之下）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="distributeId"></param>
        /// <returns></returns>
        private List<Entities.Distribute.MaterielDetailedStatistics> GetDetailedStatisticsEntity(
            List<Entities.Distribute.MaterielDetailedStatistics> list,
            RespMaterielDto respMaterielDto, int distributeId)
        {
            //头部
            list.Add(new Entities.Distribute.MaterielDetailedStatistics()
            {
                MaterielId = respMaterielDto.MaterielId,
                DistributeId = distributeId,
                ArticleId = respMaterielDto.ArticleId,
                ConentType = (int)MaterielConentTypeEnum.Head,//头部
                ArticleType = (int)HeadContentTypeEnum.图文,//文章类型（内容）：分别对应头部（图文），腰部（视频，信息流），腿部的类型（询价卡片，H5商务专题等），
                PV = respMaterielDto.HeadMaterial?.PV ?? 0,
                UV = respMaterielDto.HeadMaterial?.UV ?? 0,
                LikeNumber = respMaterielDto.HeadMaterial?.AgreeCount ?? 0,
                ForwardNumber = respMaterielDto.HeadMaterial?.ShareTotal ?? 0,
                ReadNumber = respMaterielDto.HeadMaterial?.PV ?? 0,
                Url = respMaterielDto.Url,
                CreateTime = DateTime.Now
            });
            //头部-腿部点击
            GetDetailedStatisticsHeadEntity(list, respMaterielDto, distributeId);
            //腰部
            respMaterielDto.WaistMaterial.ForEach(s =>
            {
                list.Add(new Entities.Distribute.MaterielDetailedStatistics()
                {
                    MaterielId = respMaterielDto.MaterielId,
                    ArticleId = s.ArticleId,
                    DistributeId = distributeId,
                    ConentType = (int)MaterielConentTypeEnum.Body,//腰部
                    ArticleType = (int)HeadContentTypeEnum.图文,//文章类型（内容）：分别对应头部（图文），腰部（视频，信息流），腿部的类型（询价卡片，H5商务专题等），
                    Url = s.Url,
                    PV = s.PV,
                    UV = s.UV,
                    ClickPV = s.WaistClikePV,
                    ClickUV = s.WaistClikeUV,
                    //LikeNumber = s.AgreeCount,
                    ReadNumber = s.PV,
                    ForwardNumber = s.ShareTotal,
                    CreateTime = DateTime.Now
                });
            });
            //腰部-腿部点击
            GetDetailedStatisticsFootEntity(list, respMaterielDto, distributeId);
            return list;
        }

        /// <summary>
        /// 解析统计明细-头部里面的腿部信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="distributeId"></param>
        /// <returns></returns>
        private List<Entities.Distribute.MaterielDetailedStatistics> GetDetailedStatisticsHeadEntity(
            List<Entities.Distribute.MaterielDetailedStatistics> list,
            RespMaterielDto respMaterielDto, int distributeId)
        {
            if (respMaterielDto.HeadMaterial == null)
            {
                return list;
            }

            var dicMaterielClickDetailsEnum = EnumExtensions.GetEnumItemValueDesc(typeof(MaterielClickDetailsEnum));

            //腿部文章需要统计阅读数（暂只取腿部）
            //var waistMaterialArticleIds = Dal.Distribute.MaterielInfo.Instance.GetArticleInfo(new List<int>()
            //{ respMaterielDto.ArticleId }).FirstOrDefault();

            //var readNum = waistMaterialArticleIds?.ReadNum ?? 0;

            list.AddRange(dicMaterielClickDetailsEnum.Select(item => new MaterielDetailedStatistics
            {
                MaterielId = respMaterielDto.MaterielId,
                ArticleId = respMaterielDto.ArticleId,//这里的文章id不能变动，因为这是一个标识（代表这个腿是跟着头部走的）
                DistributeId = distributeId,
                ConentType = (int)MaterielConentTypeEnum.Foot, //腿部
                ArticleType = item.Key.ToInt(), //文章类型（内容）：分别对应头部（图文），腰部（视频，信息流），腿部的类型（询价卡片，H5商务专题等），
                //Url = s.Url,
                //PV = s.PV,
                //UV = s.UV,
                ClickPV = GetClickUvValue(item.Key.ToInt(), respMaterielDto.HeadMaterial.ClikePV),
                ClickUV = GetClickUvValue(item.Key.ToInt(), respMaterielDto.HeadMaterial.ClikeUV),
                //ReadNumber = readNum,
                //LikeNumber = s.AgreeCount,
                //ForwardNumber = s.ShareTotal,
                CreateTime = DateTime.Now
            }));
            return list;
        }

        /// <summary>
        /// 解析统计明细-腰部里面的腿部信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="distributeId"></param>
        /// <returns></returns>
        private List<Entities.Distribute.MaterielDetailedStatistics> GetDetailedStatisticsFootEntity(
            List<Entities.Distribute.MaterielDetailedStatistics> list,
            RespMaterielDto respMaterielDto, int distributeId)
        {
            if (respMaterielDto.WaistMaterial == null || !respMaterielDto.WaistMaterial.Any())
            {
                return list;
            }

            var dicMaterielClickDetailsEnum = EnumExtensions.GetEnumItemValueDesc(typeof(MaterielClickDetailsEnum));
            //腰部的文章id列表
            //var waistMaterialArticleIds = Dal.Distribute.MaterielInfo.Instance.GetArticleInfo(
            //    respMaterielDto.WaistMaterial.Select(s => s.ArticleId).ToList());

            //腰部
            respMaterielDto.WaistMaterial.ForEach(s =>
            {
                //var readNum = waistMaterialArticleIds.Where(x => x.ArticleId == s.ArticleId)
                //             .Select(x => x.ReadNum)
                //             .FirstOrDefault();
                foreach (var item in dicMaterielClickDetailsEnum)
                {
                    //item.Key
                    list.Add(new MaterielDetailedStatistics
                    {
                        MaterielId = respMaterielDto.MaterielId,
                        ArticleId = s.ArticleId,//这里的文章id不能变动，因为这是一个标识（代表这个腿是跟着腰部走的）
                        DistributeId = distributeId,
                        ConentType = (int)MaterielConentTypeEnum.Foot,//腿部
                        ArticleType = item.Key.ToInt(),//文章类型（内容）：分别对应头部（图文），腰部（视频，信息流），腿部的类型（询价卡片，H5商务专题等），
                        //Url = s.Url,
                        PV = s.PV,
                        UV = s.UV,
                        ClickPV = GetClickUvValue(item.Key.ToInt(), s.ClikePV),
                        ClickUV = GetClickUvValue(item.Key.ToInt(), s.ClikeUV),
                        ReadNumber = s.PV,
                        //LikeNumber =,
                        //ForwardNumber = s.ShareTotal,
                        CreateTime = DateTime.Now
                    });
                }
            });

            return list;
        }

        private List<Entities.Distribute.MaterielOperateDetailed> GetStatisticsOperateDetailedEntity(
            List<Entities.Distribute.MaterielOperateDetailed> listOperateDetailed,
            RespMaterielDto respMaterielDto)
        {
            //头部
            GetStatisticsOperateDetailedEntityByHead(listOperateDetailed, respMaterielDto);
            //腰部
            GetStatisticsOperateDetailedEntityByFoot(listOperateDetailed, respMaterielDto);

            return listOperateDetailed;
        }

        private List<MaterielOperateDetailed> GetStatisticsOperateDetailedEntityByHead(
            List<Entities.Distribute.MaterielOperateDetailed> listOperateDetailed,
            RespMaterielDto respMaterielDto)
        {
            if (respMaterielDto.HeadMaterial == null)
            {
                return listOperateDetailed;
            }

            #region 头部

            var dicMaterielClickDetailsEnum = EnumExtensions.GetEnumItemValueDesc(typeof(MaterielClickDetailsEnum));
            //点击pv
            if (respMaterielDto.HeadMaterial.ClikePV != null)
            {
                //点击pv明细：循环5次，都是一种点击类型点击pv（因为现在点击pv和uv都是5种）
                GetStatisticsOperateDetailedsByClikePv(listOperateDetailed, respMaterielDto,
                    MaterielConentTypeEnum.Head, dicMaterielClickDetailsEnum, respMaterielDto.HeadMaterial.ClikePV,
                    respMaterielDto.ArticleId);
            }
            //点击uv明细：循环5次，都是一种点击类型点击pv（因为现在点击pv和uv都是5种）
            if (respMaterielDto.HeadMaterial.ClikeUV != null)
            {
                GetStatisticsOperateDetailedsByClikeUv(listOperateDetailed, respMaterielDto,
                    MaterielConentTypeEnum.Head, dicMaterielClickDetailsEnum, respMaterielDto.HeadMaterial.ClikeUV,
                    respMaterielDto.ArticleId);
            }
            //转发明细（腰部：3种）
            if (respMaterielDto.HeadMaterial.Share != null)
            {
                var dicMaterielShareTypeEnum = EnumExtensions.GetEnumItemValueDesc(typeof(MaterielShareTypeEnum));
                GetStatisticsOperateDetailedsByShare(listOperateDetailed, respMaterielDto,
                    MaterielConentTypeEnum.Body, dicMaterielShareTypeEnum, respMaterielDto.HeadMaterial.Share,
                    respMaterielDto.ArticleId);
            }

            #endregion 头部

            return listOperateDetailed;
        }

        private List<MaterielOperateDetailed> GetStatisticsOperateDetailedEntityByFoot(
            List<Entities.Distribute.MaterielOperateDetailed> listOperateDetailed,
            RespMaterielDto respMaterielDto)
        {
            if (respMaterielDto.WaistMaterial == null || !respMaterielDto.WaistMaterial.Any())
            {
                return listOperateDetailed;
            }

            #region 腰部

            var dicMaterielClickDetailsEnum = EnumExtensions.GetEnumItemValueDesc(typeof(MaterielClickDetailsEnum));

            var dicMaterielShareTypeEnum = EnumExtensions.GetEnumItemValueDesc(typeof(MaterielShareTypeEnum));
            //循环腰部
            respMaterielDto.WaistMaterial.ForEach(w =>
            {
                if (w.ClikePV != null)
                {
                    //点击pv明细：循环5次，都是一种点击类型点击pv（因为现在点击pv和uv都是5种）
                    GetStatisticsOperateDetailedsByClikePv(listOperateDetailed, respMaterielDto,
                        MaterielConentTypeEnum.Foot, dicMaterielClickDetailsEnum, w.ClikePV, w.ArticleId);
                }

                //点击uv明细：循环5次，都是一种点击类型点击pv（因为现在点击pv和uv都是5种）
                if (w.ClikeUV != null)
                {
                    GetStatisticsOperateDetailedsByClikeUv(listOperateDetailed, respMaterielDto,
                        MaterielConentTypeEnum.Foot, dicMaterielClickDetailsEnum, w.ClikeUV, w.ArticleId);
                }

                //转发明细（腰部：3种）
                if (w.Share != null)
                {
                    GetStatisticsOperateDetailedsByShare(listOperateDetailed, respMaterielDto,
                        MaterielConentTypeEnum.Body, dicMaterielShareTypeEnum, w.Share, w.ArticleId);
                }
            });

            #endregion 腰部

            return listOperateDetailed;
        }

        /// <summary>
        /// 通用转发明细（3种）
        /// </summary>
        /// <param name="listOperateDetailed"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="conentType">内容类型（头，腰，尾）</param>
        /// <param name="dicMaterielShareTypeEnum"></param>
        /// <param name="share"></param>
        /// <param name="articleId">文章id 必须指定，因为腰部文章里面有腿部点击</param>
        /// <returns></returns>
        private List<MaterielOperateDetailed> GetStatisticsOperateDetailedsByShare(
            List<Entities.Distribute.MaterielOperateDetailed> listOperateDetailed,
            RespMaterielDto respMaterielDto, MaterielConentTypeEnum conentType,
            Dictionary<string, string> dicMaterielShareTypeEnum, ShareDto share, int articleId)
        {
            if (share != null)
            {
                foreach (var item in dicMaterielShareTypeEnum)
                {
                    listOperateDetailed.Add(new MaterielOperateDetailed
                    {
                        MaterielId = respMaterielDto.MaterielId,
                        ArticleId = articleId,
                        StatisticsId = 0,
                        ConentType = (int)conentType,//腰部
                        ClickType = (int)MaterielClickTypeEnum.Share,
                        DicId = item.Key.ToInt(),
                        ClickValue = GetShareTypeValue(item.Key.ToInt(), share)
                    });
                }
            }
            return listOperateDetailed;
        }

        /// <summary>
        ///  通用点击uv
        /// </summary>
        /// <param name="listOperateDetailed"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="conentType">内容类型（头，腰，尾）</param>
        /// <param name="dicMaterielClickDetailsEnum"></param>
        /// <param name="clickUv"></param>
        /// <param name="articleId">文章id 必须指定，因为腰部文章里面有腿部点击</param>
        /// <returns></returns>
        private List<MaterielOperateDetailed> GetStatisticsOperateDetailedsByClikeUv(
           List<Entities.Distribute.MaterielOperateDetailed> listOperateDetailed,
           RespMaterielDto respMaterielDto, MaterielConentTypeEnum conentType,
           Dictionary<string, string> dicMaterielClickDetailsEnum, ClikepvDto clickUv, int articleId)
        {
            if (clickUv != null)
            {
                listOperateDetailed.AddRange(dicMaterielClickDetailsEnum.Select(item => new MaterielOperateDetailed
                {
                    MaterielId = respMaterielDto.MaterielId,
                    ArticleId = articleId,
                    StatisticsId = 0,
                    ConentType = (int)conentType, //头部
                    ClickType = (int)MaterielClickTypeEnum.ClickUv,
                    DicId = item.Key.ToInt(),
                    ClickValue = GetClickUvValue(item.Key.ToInt(), clickUv)
                }));
            }
            return listOperateDetailed;
        }

        /// <summary>
        ///  通用点击pv
        /// </summary>
        /// <param name="listOperateDetailed"></param>
        /// <param name="respMaterielDto"></param>
        /// <param name="conentType">内容类型（头，腰，尾）</param>
        /// <param name="dicMaterielClickDetailsEnum"></param>
        /// <param name="clickPv"></param>
        /// <param name="articleId">文章id 必须指定，因为腰部文章里面有腿部点击</param>
        /// <returns></returns>
        private List<MaterielOperateDetailed> GetStatisticsOperateDetailedsByClikePv(
           List<Entities.Distribute.MaterielOperateDetailed> listOperateDetailed,
           RespMaterielDto respMaterielDto, MaterielConentTypeEnum conentType,
           Dictionary<string, string> dicMaterielClickDetailsEnum, ClikepvDto clickPv, int articleId)
        {
            if (clickPv != null)
            {
                foreach (var item in dicMaterielClickDetailsEnum)
                {
                    listOperateDetailed.Add(new MaterielOperateDetailed
                    {
                        MaterielId = respMaterielDto.MaterielId,
                        ArticleId = articleId,
                        StatisticsId = 0,
                        ConentType = (int)conentType,//头部
                        ClickType = (int)MaterielClickTypeEnum.ClickPv,
                        DicId = item.Key.ToInt(),
                        ClickValue = GetClickUvValue(item.Key.ToInt(), clickPv)
                    });
                }
            }
            return listOperateDetailed;
        }

        private int GetClickUvValue(int key, ClikepvDto clikeuv)
        {
            var valDic = new Dictionary<int, int>()
            {
                { (int)MaterielClickDetailsEnum.小程序,clikeuv.Applet },
                { (int)MaterielClickDetailsEnum.头像,clikeuv.HeadPortrait },
                { (int)MaterielClickDetailsEnum.电话,clikeuv.Phone },
                { (int)MaterielClickDetailsEnum.询价,clikeuv.Inquiry },
                { (int)MaterielClickDetailsEnum.问答,clikeuv.QA }
            };

            return valDic.FirstOrDefault(s => s.Key == key).Value;
        }

        private int GetShareTypeValue(int key, ShareDto share)
        {
            var valDic = new Dictionary<int, int>()
            {
                { (int)MaterielShareTypeEnum.WeChat,share.WeChatFriend },
                { (int)MaterielShareTypeEnum.WeChatFriends,share.WeChatFriends },
                { (int)MaterielShareTypeEnum.QQ,share.QQ }
            };

            return valDic.FirstOrDefault(s => s.Key == key).Value;
        }
    }
}