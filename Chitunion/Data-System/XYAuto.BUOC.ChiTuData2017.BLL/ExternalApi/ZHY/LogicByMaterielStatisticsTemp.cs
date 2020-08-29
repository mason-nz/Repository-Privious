/********************************************************
*创建人：lixiong
*创建时间：2017/10/24 10:03:01
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.ZHY
{
    /// <summary>
    /// 拉取智慧云物料统计信息（临时的，每天查找8天前的头部文章数据）
    /// </summary>
    public class LogicByMaterielStatisticsTemp : VerifyOperateBase
    {
        private readonly MaterielStatisticsProvider _contextProvider;
        private readonly PullDataConfig _pullDataConfig;
        private string _dayQuery;
        private DateTime _todyDateTime;

        public LogicByMaterielStatisticsTemp(Infrastruction.Http.DoHttpClient doHttpClient,
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
            //todo:现在需要直接找到-34天之前的时间，然后在-34的时间点上往后 遍历 10天或者15天
            var loopEnd = Math.Abs(_pullDataConfig.DateOffset);//34

            var startDate = _todyDateTime;
            Loger.Log4Net.Info($" zhy..拉取时间节点为：{loopEnd}天 开始时间：{startDate} 执行开始...");
            for (var i = 0; i <= loopEnd - 1; i++)
            {
                _todyDateTime = startDate.AddDays(i);
                _dayQuery = _todyDateTime.ToString("yyyy-MM-dd");
                PullStatistics(_dayQuery, _todyDateTime);
                Loger.Log4Net.Info($" zhy..拉取时间节点为：{loopEnd}天 {_dayQuery} 已执行完成");
            }
            Loger.Log4Net.Info($" zhy..拉取时间节点为：{loopEnd}天 开始时间：{startDate} 执行结束{_dayQuery}...");
        }

        /// <summary>
        /// 拉取物料统计
        /// </summary>
        /// <returns></returns>
        public ReturnValue PullStatistics(string queryDate, DateTime queryDateTime)
        {
            //todo:1.先获取物料列表，循环用头部文章id去拉取智慧云接口数据 （？？因为拉取数据参数是头部文章和昨天的日期，头部文章id列表在哪里获取）
            //todo:2.返回json数据中，头部文章数据入库到：物料分发明细表（Materiel_DistributeDetailed）
            //todo:3.返回json数据中，物料包本页（属于头部文章）入库到：物料详情数据统计明细表（Materiel_DetailedStatistics）
            //todo:4.返回json数据中，腰页入库到：物料详情数据统计明细表（Materiel_DetailedStatistics）
            //todo:5.物料包本页,腰页 里面的点击明细入库到：点击明细，转发明细（Materiel_OperateDetailed）
            var retValue = new ReturnValue();
            var requestDto = new PullStatisticsDto()
            {
                DateTime = queryDate
            };

            var qingNiaoAgentQuery = new DistributeQuery<Entities.Distribute.MaterielTemp>
            {
                Date = queryDate,
                PageIndex = 1,
                PageSize = 50
            };
            var articleList = Dal.Distribute.MaterielInfo.Instance.GetArticleInfo(qingNiaoAgentQuery);
            if (!articleList.Any())
            {
                Loger.ZhyLogger.Info($" zhy .. GetArticleInfo 头部文章，PageIndex={qingNiaoAgentQuery.PageIndex} {qingNiaoAgentQuery.Date} 暂无文章数据，zhy拉取任务终止....");
                return CreateFailMessage(retValue, "20010", $"暂无文章数据，zhy拉取任务终止..{queryDate}");
            }
            //进行数据组装
            DoPullRunning(retValue, qingNiaoAgentQuery, articleList, requestDto, queryDateTime);

            var totleNumber = qingNiaoAgentQuery.Total;
            var offestCount = GetOffsetPageCount(totleNumber, qingNiaoAgentQuery.PageSize);

            while (offestCount >= qingNiaoAgentQuery.PageIndex)
            {
                articleList = Dal.Distribute.MaterielInfo.Instance.GetArticleInfo(qingNiaoAgentQuery);
                if (!articleList.Any())
                {
                    Loger.ZhyLogger.Info($" zhy .. GetArticleInfo 头部文章，PageIndex={qingNiaoAgentQuery.PageIndex} {qingNiaoAgentQuery.Date} 暂无文章数据，zhy拉取任务终止....");
                    break;
                }
                DoPullRunning(retValue, qingNiaoAgentQuery, articleList, requestDto, queryDateTime);
            }

            return retValue;
        }

        private DateTime _currentDateTime;

        private void DoPullRunning(ReturnValue retValue, DistributeQuery<Entities.Distribute.MaterielTemp> qingNiaoAgentQuery,
           List<Entities.Distribute.MaterielTemp> articleList, PullStatisticsDto requestDto,
          DateTime queryDateTime)
        {
            Loger.ZhyLogger.Info($" zhy .. GetArticleInfo 头部文章 DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} {qingNiaoAgentQuery.Date}开始..");

            //todo:需要重新初始化时间，每天的日期，for 循环是每天+10天的维度
            var startDays = queryDateTime;
            articleList.ForEach(at =>
            {
                //添加统计信息
                //todo:现在是需要查找过去8天的数据
                for (var i = 0; i <= _pullDataConfig.PullDataQueryDateOffset; i++)
                {
                    _currentDateTime = startDays.AddDays(i);
                    if ((DateTime.Now.AddDays(-2) - _currentDateTime).TotalDays < 0)
                    {
                        break;//如果日期达到了临界值，就退出
                    }
                    requestDto.DateTime = _currentDateTime.ToString("yyyy-MM-dd");
                    //todo: 底层接口已经换成MaterielId了，所有，这里赋值还用ArticleId，临时用的
                    requestDto.MaterielId = at.ArticleId;

                    Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                         $"第{i}天，{requestDto.DateTime}，文章id：{at.ArticleId} 开始..");
                    RespMaterielDto respStatisticsInfo;
                    DoInsertBefore(retValue, requestDto, out respStatisticsInfo);
                    if (respStatisticsInfo == null || respStatisticsInfo.MaterielId <= 0)
                    {
                        Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                          $"第{i}天，{requestDto.DateTime} ，文章id：{at.ArticleId} 拉取结果为null..");
                        continue;
                    }
                    //验证是否已经存在
                    var retValue1 = VerifyDistribute(retValue, respStatisticsInfo, requestDto.DateTime);
                    if (retValue1.HasError)
                    {
                        Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                          $"第{i}天，{respStatisticsInfo.MaterielId} {requestDto.DateTime} 已经存在..");
                        continue;
                    }
                    retValue = DoInsert(retValue, respStatisticsInfo);
                    retValue.HasError = false;
                    Loger.ZhyLogger.Info($" zhy .. DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} ，拉取过去{ _pullDataConfig.PullDataQueryDateOffset}天历史数据." +
                                        $"第{i}天，{requestDto.DateTime} 结束..");
                }
                //todo:需要重新初始化时间
                startDays = queryDateTime;
            });

            Loger.ZhyLogger.Info($" zhy .. GetArticleInfo 头部文章 DoPullRunning 循环拉取物料统计信息，PageIndex={qingNiaoAgentQuery.PageIndex} {qingNiaoAgentQuery.Date} 结束..");
            qingNiaoAgentQuery.PageIndex++;
            retValue.HasError = false;
        }

        private ReturnValue VerifyDistribute(ReturnValue retValue, RespMaterielDto respStatisticsInfo, string queryDay)
        {
            var distributeDetailed = Dal.Distribute.MaterielDistributeDetailed.Instance.GetList(
                     new DistributeQuery<MaterielDistributeDetailed>()
                     {
                         MaterielId = respStatisticsInfo.MaterielId,
                         Date = queryDay,
                         Source = (int)DistributeTypeEnum.QingNiaoAgent
                     });
            if (distributeDetailed.Any())
            {
                Loger.Log4Net.Error($" zhy 拉取数据 VerifyDistribute 当前物料id在当前已经存在,MaterielId={ respStatisticsInfo.MaterielId}&Date={queryDay}");
                return CreateFailMessage(retValue, "20011", $" zhy 拉取数据 VerifyDistribute 当前物料id在当前已经存在,MaterielId={ respStatisticsInfo.MaterielId}&Date={queryDay}");
            }
            retValue.HasError = false;
            return retValue;
        }

        public ReturnValue DoInsertBefore(ReturnValue retValue, PullStatisticsDto requestDto, out RespMaterielDto respStatisticsInfo)
        {
            respStatisticsInfo = _contextProvider.PullStatistics(requestDto);
            if (respStatisticsInfo == null)
            {
                return CreateFailMessage(retValue, "20001", $"拉取数据为null,请求参数:{JsonConvert.SerializeObject(requestDto)}");
            }
            //补全物料信息（找到头部文章id对应的物料id）
            var materielInfo = Dal.Distribute.MaterielInfo.Instance.GetMaterielInfoByBodyArticleId(respStatisticsInfo.ArticleId);

            if (materielInfo == null)
            {
                var msg = $" GetMaterielInfoByBodyArticleId 错误，没有找到对应的物料信息，bodyArticleId：{respStatisticsInfo.ArticleId}";
                Loger.Log4Net.Error(msg);
                return CreateFailMessage(retValue, "20002", msg);
            }
            respStatisticsInfo.MaterielId = materielInfo.MaterielId;

            return retValue;
        }

        /// <summary>
        /// 入库操作入口
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="respMaterielDto"></param>
        /// <returns></returns>
        public ReturnValue DoInsert(ReturnValue retValue, RespMaterielDto respMaterielDto)
        {
            var distributeId = InsertDistributeDetailed(respMaterielDto);
            if (distributeId <= 0)
            {
                return CreateFailMessage(retValue, "20005", "InsertDistributeDetailed 入库失败");
            }
            InsertDetailedStatistics(retValue, respMaterielDto, distributeId);
            retValue.HasError = false;
            return retValue;
        }

        private int InsertDistributeDetailed(RespMaterielDto respMaterielDto)
        {
            var excuteId = Dal.Distribute.MaterielDistributeDetailed.Instance.Insert(GetDistributeDetailedEntity(respMaterielDto));

            if (excuteId <= 0)
            {
                Loger.Log4Net.Error($" InsertDistributeDetailed 入库失败：{JsonConvert.SerializeObject(GetDistributeDetailedEntity(respMaterielDto))}");
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
        private ReturnValue InsertDetailedStatistics(ReturnValue retValue, RespMaterielDto respMaterielDto, int distributeId)
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

        private Entities.Distribute.MaterielDistributeDetailed GetDistributeDetailedEntity(RespMaterielDto respMaterielDto)
        {
            return new Entities.Distribute.MaterielDistributeDetailed()
            {
                MaterielId = respMaterielDto.MaterielId,
                BrowsePageAvg = DistributeProfile.GetBrowsePageAvg(respMaterielDto.PV, respMaterielDto.UV, 0),
                Date = _currentDateTime,
                PV = respMaterielDto.PV,
                UV = respMaterielDto.UV,
                //OnLineAvgTime = 0,
                JumpProportion = respMaterielDto.JumpChance,
                InquiryNumber = respMaterielDto.Clue?.InquiryCount ?? 0,
                SessionNumber = respMaterielDto.Clue?.ConversationCount ?? 0,
                TelConnectNumber = respMaterielDto.Clue?.PhoneClueCount ?? 0,
                Source = 69003,//(int)DistributeTypeEnum.QingNiaoAgent,
                CreateUserId = -3,//(int)MaterielCreateTypeEnum.QingNiaoAgent,//智慧云
                CreateTime = _currentDateTime,
                DistributeUrl = respMaterielDto.Url
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
                CreateTime = _currentDateTime
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
                    CreateTime = _currentDateTime
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
                CreateTime = _currentDateTime
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
                        CreateTime = _currentDateTime
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