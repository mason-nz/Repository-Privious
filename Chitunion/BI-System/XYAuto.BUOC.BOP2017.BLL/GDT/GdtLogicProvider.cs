/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 15:50:38
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.GDT.PullConfigLable;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Ads;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Fund;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Report;
using XYAuto.BUOC.BOP2017.Infrastruction;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.GDT
{
    public class GdtLogicProvider : Infrastruction.Verification.VerifyOperateBase
    {
        private PullConfigLableProvider _pullConfigLableProvider;

        public GdtLogicProvider()
        {
            _pullConfigLableProvider = new PullConfigLableProvider();
        }

        #region 账户相关逻辑

        public ConfigBaseInfo<T> GetConfigBaseInfo<T>(PullCategoryEnum pullCategoryEnum)
        {
            var configPageInfo = _pullConfigLableProvider.GetConfig<T>(pullCategoryEnum);
            if (configPageInfo == null || configPageInfo.ConfigPageInfo == null)
            {
                return new ConfigBaseInfo<T>();
            }
            return configPageInfo;
        }

        /// <summary>
        /// 账户拉取
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public ReturnValue DoPullAccountUserInfo(int accountId = -2)
        {
            //todo:拉取广点通的接口，入库
            //todo:1.先删除当前accountId对应的信息，再插入
            //todo:2.因为这里拉取的是一个代理商的帐号，下面会有多个子客，不能批量删除
            var retValue = new ReturnValue();
            var configPageInfo = _pullConfigLableProvider.GetConfig<int>(PullCategoryEnum.GdtAccunt);
            var query = new ReqReportDto()
            {
                AccountId = accountId,
                Page = configPageInfo.ConfigPageInfo.Page,
                PageSize = configPageInfo.ConfigPageInfo.PageSize
            };
            var accounInfotList = ServiceHelper.Instance.GetAdvertiserInfo(query);
            if (accounInfotList.List == null)
            {
                var msg = $"获取广告主信息为null，参数:{JsonConvert.SerializeObject(query)} 等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10001", msg);
            }

            accounInfotList.List.ForEach(item =>
            {
                if (Dal.GDT.GdtAccountInfo.Instance.InsertByGdtServer(AutoMapper.Mapper.Map<RespAccountInfoDto,
                    Entities.GDT.GdtAccountInfo>(item)) <= 0)
                {
                    Loger.Log4Net.Error($"{System.Environment.NewLine}DoPullAccountUserInfo入库失败：{JsonConvert.SerializeObject(item)}");
                }
            });

            retValue = DoPullAccountUserInfoPageNext(retValue, query, accounInfotList);

            //记录标记配置
            _pullConfigLableProvider.SetConfig(PullCategoryEnum.GdtAccunt, new ConfigBaseInfo<int>()
            {
                ConfigPageInfo = new ConfigPageInfo()
                {
                    Page = query.Page,
                    PageSize = query.PageSize
                }
            });
            return retValue;
        }

        private ReturnValue DoPullAccountUserInfoPageNext(ReturnValue retValue, ReqReportDto query,
            RespPageInfo<List<RespAccountInfoDto>> pageInfo)
        {
            if (pageInfo.PageInfo.TotalPage == pageInfo.PageInfo.Page)
            {
                return CreateSuccessMessage(retValue, "0", "DoPullAccountUserInfo  获取资金账户流水入库完成");
            }
            var startPage = pageInfo.PageInfo.Page + 1;
            for (var i = startPage; i <= pageInfo.PageInfo.TotalPage; i++)
            {
                var msg = $"获取广告主信息 DoPullAccountUserInfoPageNext 第：{startPage}页 开始";
                Loger.GdtLogger.Info(msg);
                query.Page = i;//pageIndex ++
                var pageInfoNext = ServiceHelper.Instance.GetAdvertiserInfo(query);
                if (pageInfoNext.List == null || pageInfoNext.List.Count == 0)
                {
                    msg += $" 获取广告主信息 DoPullAccountUserInfoPageNext，拉取信息业务参数:{JsonConvert.SerializeObject(query)}等待下一次执行";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10006", msg);
                }
                pageInfoNext.List.ForEach(item =>
                {
                    if (Dal.GDT.GdtAccountInfo.Instance.InsertByGdtServer(AutoMapper.Mapper.Map<RespAccountInfoDto,
                        Entities.GDT.GdtAccountInfo>(item)) <= 0)
                    {
                        Loger.Log4Net.Error($"{System.Environment.NewLine}DoPullAccountUserInfo入库失败：{JsonConvert.SerializeObject(item)}");
                    }
                });

                Loger.GdtLogger.Info($"获取广告主信息 DoPullAccountUserInfoPageNext 第：{startPage}页 结束");
            }

            return CreateSuccessMessage(retValue, "", "DoPullAccountUserInfo 账户拉取入库完成");
        }

        /// <summary>
        /// 资金账户信息拉取入库
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public ReturnValue DoPullGetFundsInfo(int accountId)
        {
            var retValue = new ReturnValue();
            var fundList = ServiceHelper.Instance.GetFundsInfo(accountId);
            if (fundList == null || fundList.Count == 0)
            {
                var msg = $"获取资金账户信息GetFundsInfo为null，accountId:{accountId}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10002", msg);
            }
            var list = AutoMapper.Mapper.Map<List<RespFundDto>,
                List<Entities.GDT.GdtAccountBalance>>(fundList);
            var excuteCount = Dal.GDT.GdtAccountBalance.Instance.InsertByGdtServer(list, new Entities.GDT.GdtAccountBalance { AccountId = accountId });
            if (excuteCount < 1)
            {
                Loger.Log4Net.Error($"{System.Environment.NewLine}DoPullGetFundsInfo入库失败：{JsonConvert.SerializeObject(list)}");
            }
            return CreateSuccessMessage(retValue, "", "DoPullGetFundsInfo 资金账户信息拉取完成");
        }

        /// <summary>
        /// 获取今日消耗(定时每隔一个小时拉取，因为接口返回的是一条数据，不是流水，所以，我们自己得记录下来，形式一个报表)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ReturnValue DoPullGetRealtimeCost(ReqReportDto request)
        {
            var retValue = new ReturnValue();
            var pageInfo = ServiceHelper.Instance.GetRealtimeCost(request);
            if (pageInfo == null || pageInfo.List == null || pageInfo.List.Count == 0)
            {
                var msg = $"获取今日消耗信息GetRealtimeCost为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10003", msg);
            }
            var date = DateTime.Now;
            pageInfo.List.ForEach(item =>
            {
                Dal.GDT.GdtRealtimeCost.Instance.InsertByGdtServer(new Entities.GDT.GdtRealtimeCost
                {
                    AccountId = request.AccountId,
                    AdgroupId = item.AdgroupId,
                    CampaignId = item.CampaignId,
                    Cost = item.Cost,
                    Level = GdtDataEnumProvider.GetDicLevel(request.Level.ToString()),
                    Date = date.Date//取天
                });
            });
            return CreateSuccessMessage(retValue, "", "DoPullGetRealtimeCost  获取今日消耗入库完成");
        }

        /// <summary>
        /// 获取资金账户日结明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ReturnValue DoPullGetFundStatementsDaily(ReqFundDto request)
        {
            var retValue = new ReturnValue();

            var list = ServiceHelper.Instance.GetFundStatementsDaily(request);
            if (list == null || list.Count == 0)
            {
                var msg = $"获取资金账户日结明细 GetFundStatementsDaily为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10004", msg);
            }
            var insertList = AutoMapper.Mapper.Map<List<FundStatementsDailyDto>, List<Entities.GDT.GdtStatementDaily>>(list);

            Dal.GDT.GdtStatementDaily.Instance.InsertByGdtServer(insertList, new Entities.GDT.GdtStatementDaily
            {
                Date = DateTime.Parse(request.Date),
                AccountId = request.AccountId,
                FundType = GdtDataEnumProvider.GetDicFundType(request.FundType.ToString())
            }, 1);
            return CreateSuccessMessage(retValue, "", "DoPullGetFundStatementsDaily  获取资金账户日结明细入库完成");
        }

        /// <summary>
        /// 获取资金账户流水
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ReturnValue DoPullGetFundStatementsDetailed(ReqFundDetaileDto request)
        {
            var retValue = new ReturnValue();
            var pageInfo = ServiceHelper.Instance.GetFundStatementsDetailed(request);
            if (pageInfo == null || pageInfo.List == null || pageInfo.List.Count == 0)
            {
                var msg = $"获取获取资金账户流水 GetFundStatementsDetailed为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10005", msg);
            }
            //如果第一次请求的大于1页的数据，则需要循环取数据
            var insertList = AutoMapper.Mapper.Map<List<FundStatementsDailyDto>, List<Entities.GDT.GdtStatementsDetailed>>(pageInfo.List);
            //第一页处理
            var entityWhere = new Entities.GDT.GdtStatementsDetailed
            {
                AccountId = request.AccountId,
                FundType = GdtDataEnumProvider.GetDicFundType(request.FundType.ToString()),
                Date = GetDate(request.DateRange) //
            };
            Dal.GDT.GdtStatementsDetailed.Instance.InsertByGdtServer(insertList, entityWhere, pageInfo.PageInfo.Page);

            return DoPullGetFundStatementsDetailedPageNext(retValue, request, pageInfo, entityWhere);
        }

        /// <summary>
        /// 继续分页处理
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="request"></param>
        /// <param name="pageInfo"></param>
        /// <param name="entityWhere"></param>
        /// <returns></returns>
        public ReturnValue DoPullGetFundStatementsDetailedPageNext(ReturnValue retValue, ReqFundDetaileDto request,
            RespPageInfo<List<FundStatementsDailyDto>> pageInfo, Entities.GDT.GdtStatementsDetailed entityWhere)
        {
            if (pageInfo.PageInfo.TotalPage == pageInfo.PageInfo.Page)
            {
                return CreateSuccessMessage(retValue, "", "DoPullGetFundStatementsDetailed  获取资金账户流水入库完成");
            }
            var startPage = pageInfo.PageInfo.Page + 1;

            for (var i = startPage; i <= pageInfo.PageInfo.TotalPage; i++)
            {
                var msg = $"获取获取资金账户流水 第：{startPage}页 开始";
                Loger.GdtLogger.Info(msg);
                request.Page = i;//pageIndex ++
                var pageInfoNext = ServiceHelper.Instance.GetFundStatementsDetailed(request);
                if (pageInfoNext.List == null || pageInfoNext.List.Count == 0)
                {
                    msg += $" GetFundStatementsDetailed为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10006", msg);
                }
                var insertList = AutoMapper.Mapper.Map<List<FundStatementsDailyDto>, List<Entities.GDT.GdtStatementsDetailed>>(pageInfo.List);
                //第一页处理
                Dal.GDT.GdtStatementsDetailed.Instance.InsertByGdtServer(insertList, entityWhere, i);
                Loger.GdtLogger.Info($"获取获取资金账户流水 第：{startPage}页 结束");
            }

            return CreateSuccessMessage(retValue, "", "DoPullGetFundStatementsDetailed  获取资金账户流水入库完成");
        }

        /// <summary>
        /// 暂时还不确定 StartDate，EndDate 是否有包含的关系
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public DateTime GetDate(DateRangeDto dateRange)
        {
            var ts = DateTime.Parse(dateRange.EndDate) - DateTime.Parse(dateRange.StartDate);
            if (ts.TotalDays > 1)
            {
                //区间，取StartDate+1
                return DateTime.Parse(dateRange.StartDate).AddDays(1).Date;
            }
            return DateTime.Parse(dateRange.StartDate).Date;
        }

        #endregion 账户相关逻辑

        #region 广告相关

        /// <summary>
        /// 获取广告组列表(因为会修改，所以，必须从头开始拉取数据)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ReturnValue DoPullGetAdGroupList(ReqReportDto request)
        {
            var retValue = new ReturnValue();
            var pageInfo = ServiceHelper.Instance.GetAdGroupList(request);
            if (pageInfo == null || pageInfo.List == null || pageInfo.List.Count == 0)
            {
                var msg = $"获取广告组列表 GetAdGroupList为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10007", msg);
            }
            //如果第一次请求的大于1页的数据，则需要循环取数据
            var insertList = AutoMapper.Mapper.Map<List<RespAdGroupDto>, List<Entities.GDT.GdtAdGroup>>(pageInfo.List);
            //第一页处理
            var entityWhere = new Entities.GDT.GdtAdGroup
            {
                AccountId = request.AccountId
            };
            Dal.GDT.GdtAdGroup.Instance.InsertByGdtServer(insertList, entityWhere, pageInfo.PageInfo.Page);

            return DoPullGetAdGroupListPageNext(retValue, request, pageInfo, entityWhere);
        }

        public ReturnValue DoPullGetAdGroupListPageNext(ReturnValue retValue, ReqReportDto request,
            RespPageInfo<List<RespAdGroupDto>> pageInfo, Entities.GDT.GdtAdGroup entityWhere)
        {
            if (pageInfo.PageInfo.TotalPage == pageInfo.PageInfo.Page)
            {
                return CreateSuccessMessage(retValue, "", "DoPullGetAdGroupList  获取广告组列表入库完成");
            }
            var startPage = pageInfo.PageInfo.Page + 1;

            for (var i = startPage; i <= pageInfo.PageInfo.TotalPage; i++)
            {
                var msg = $"获取广告组列表 第：{startPage}页 开始";
                Loger.GdtLogger.Info(msg);
                request.Page = i;//pageIndex ++
                var pageInfoNext = ServiceHelper.Instance.GetAdGroupList(request);
                if (pageInfoNext.List == null || pageInfoNext.List.Count == 0)
                {
                    msg += $" GetAdGroupList 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10008", msg);
                }
                var insertList = AutoMapper.Mapper.Map<List<RespAdGroupDto>, List<Entities.GDT.GdtAdGroup>>(pageInfo.List);
                //第一页处理
                Dal.GDT.GdtAdGroup.Instance.InsertByGdtServer(insertList, entityWhere, i);

                Loger.GdtLogger.Info($"获取广告组列表 第：{startPage}页 结束");
            }

            return CreateSuccessMessage(retValue, "", "DoPullGetAdGroupList  获取广告组列表入库完成");
        }

        /// <summary>
        /// 获取推广计划
        /// </summary>
        /// <returns></returns>
        public ReturnValue DoPullGetAdCampaignsList(ReqReportDto request)
        {
            var retValue = new ReturnValue();
            var pageInfo = ServiceHelper.Instance.GetAdCampaignsList(request);
            if (pageInfo == null || pageInfo.List == null || pageInfo.List.Count == 0)
            {
                var msg = $"获取推广计划 GetAdCampaignsList 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10015", msg);
            }
            //如果第一次请求的大于1页的数据，则需要循环取数据
            var insertList = AutoMapper.Mapper.Map<List<RespAdCampaignDto>, List<Entities.GDT.GdtCampaign>>(pageInfo.List);
            //第一页处理
            var entityWhere = new Entities.GDT.GdtCampaign
            {
                AccountId = request.AccountId
            };
            Dal.GDT.GdtCampaign.Instance.InsertByGdtServer(insertList, entityWhere, pageInfo.PageInfo.Page);

            return DoPullGetAdCampaignsListPageNext(retValue, request, pageInfo, entityWhere);
        }

        public ReturnValue DoPullGetAdCampaignsListPageNext(ReturnValue retValue, ReqReportDto request,
            RespPageInfo<List<RespAdCampaignDto>> pageInfo, Entities.GDT.GdtCampaign entityWhere)
        {
            if (pageInfo.PageInfo.TotalPage == pageInfo.PageInfo.Page)
            {
                return CreateSuccessMessage(retValue, "", "DoPullGetAdCampaignsListPageNext  获取广告组列表入库完成");
            }
            var startPage = pageInfo.PageInfo.Page + 1;

            for (var i = startPage; i <= pageInfo.PageInfo.TotalPage; i++)
            {
                var msg = $" 获取推广计划 第：{startPage}页 开始";
                Loger.GdtLogger.Info(msg);
                request.Page = i;//pageIndex ++
                var pageInfoNext = ServiceHelper.Instance.GetAdCampaignsList(request);
                if (pageInfoNext == null || pageInfoNext.List == null || pageInfoNext.List.Count == 0)
                {
                    msg += $" GetAdGroupList 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10008", msg);
                }
                var insertList = AutoMapper.Mapper.Map<List<RespAdCampaignDto>, List<Entities.GDT.GdtCampaign>>(pageInfo.List);
                //第一页处理
                Dal.GDT.GdtCampaign.Instance.InsertByGdtServer(insertList, entityWhere, i);

                Loger.GdtLogger.Info($" 获取推广计划 第：{startPage}页 结束");
            }

            return CreateSuccessMessage(retValue, "", "DoPullGetAdCampaignsList  获取推广计划入库完成");
        }

        #endregion 广告相关

        #region 报表

        /// <summary>
        /// 获取日报表
        /// </summary>
        /// <returns></returns>
        public ReturnValue DoPullGetReportDaily(ReqReportDto request)
        {
            var retValue = new ReturnValue();
            var pageInfo = ServiceHelper.Instance.GetReportDaily(request);
            if (pageInfo == null || pageInfo.List == null || pageInfo.List.Count == 0)
            {
                var msg = $"获取日报表 GetReportDaily为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10010", msg);
            }
            var insertList = AutoMapper.Mapper.Map<List<RespReportDto>, List<Entities.GDT.GdtDailyRrport>>(pageInfo.List);
            //第一页处理
            var entityWhere = new Entities.GDT.GdtDailyRrport
            {
                AccountId = request.AccountId,
                Level = GdtDataEnumProvider.GetDicLevel(request.Level.ToString()),
                Date = GetDate(request.DateRange),
                StartDate = DateTime.Parse(request.DateRange.StartDate),
                EndDate = DateTime.Parse(request.DateRange.EndDate)
            };
            Dal.GDT.GdtDailyRrport.Instance.InsertByGdtServer(insertList, entityWhere, pageInfo.PageInfo.Page);

            return DoPullGetReportDailyPageNext(retValue, request, pageInfo, entityWhere);
            //CreateSuccessMessage(retValue, "", "DoPullGetReportDaily  获取日报表入库完成");
        }

        public ReturnValue DoPullGetReportDailyPageNext(ReturnValue retValue, ReqReportDto request,
         RespPageInfo<List<RespReportDto>> pageInfo, Entities.GDT.GdtDailyRrport entityWhere)
        {
            if (pageInfo.PageInfo.TotalPage == pageInfo.PageInfo.Page)
            {
                return CreateSuccessMessage(retValue, "", "DoPullGetReportDailyPageNext  获取广告组列表入库完成");
            }
            var startPage = pageInfo.PageInfo.Page + 1;

            for (var i = startPage; i < pageInfo.PageInfo.TotalPage; i++)
            {
                var msg = $"获取日报表 第：{startPage}页 开始";
                Loger.GdtLogger.Info(msg);
                request.Page = i;//pageIndex ++
                var pageInfoNext = ServiceHelper.Instance.GetAdGroupList(request);
                if (pageInfoNext.List == null || pageInfoNext.List.Count == 0)
                {
                    msg += $" GetAdGroupList 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10008", msg);
                }
                var insertList = AutoMapper.Mapper.Map<List<RespReportDto>, List<Entities.GDT.GdtDailyRrport>>(pageInfo.List);
                //第一页处理
                Dal.GDT.GdtDailyRrport.Instance.InsertByGdtServer(insertList, entityWhere, i);

                Loger.GdtLogger.Info($"获取日报表 第：{startPage}页 结束");
            }

            return CreateSuccessMessage(retValue, "", "DoPullGetReportDaily  获取日报表入库完成");
        }

        /// <summary>
        /// 获取小时报表
        /// </summary>
        /// <returns></returns>
        public ReturnValue DoPullGetReportHourly(ReqReportDto request)
        {
            var retValue = new ReturnValue();
            var pageInfo = ServiceHelper.Instance.GetReportHourly(request);
            if (pageInfo == null || pageInfo.List == null || pageInfo.List.Count == 0)
            {
                var msg = $"获取小时报表 GetReportHourly 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10010", msg);
            }
            //第一页处理
            var entityWhere = new Entities.GDT.GdtHourlyRrport
            {
                AccountId = request.AccountId,
                AdgroupId = request.AdGroupId,
                Level = GdtDataEnumProvider.GetDicLevel(request.Level.ToString()),
                Date = DateTime.Parse(request.Date)
            };
            var maxHour = Dal.GDT.GdtHourlyRrport.Instance.GetMaxHour(entityWhere);

            var filterPageList = pageInfo.List.Where(s => s.Hour > maxHour).ToList();
            if (!filterPageList.Any())
            {
                var msg = $"获取小时报表 GetReportHourly 为null，过滤掉了数据库里已存在的最大hour，GetMaxHour 参数:{JsonConvert.SerializeObject(entityWhere)}";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10011", msg);
            }
            var insertList = AutoMapper.Mapper.Map<List<RespReportDto>, List<Entities.GDT.GdtHourlyRrport>>(filterPageList);

            Dal.GDT.GdtHourlyRrport.Instance.InsertByGdtServer(insertList, entityWhere, pageInfo.PageInfo.Page);

            //return CreateSuccessMessage(retValue, "0", "GetReportHourly  获取小时报表入库完成");
            //todo:因为小时报表24条数据，一次就可以拉取完
            return DoPullGetReportHourlyPageNext(retValue, request, pageInfo, entityWhere);
        }

        public ReturnValue DoPullGetReportHourlyPageNext(ReturnValue retValue, ReqReportDto request,
         RespPageInfo<List<RespReportDto>> pageInfo, Entities.GDT.GdtHourlyRrport entityWhere)
        {
            if (pageInfo.PageInfo.TotalPage == pageInfo.PageInfo.Page)
            {
                return CreateSuccessMessage(retValue, "", "DoPullGetReportHourlyPageNext  获取小时报表入库完成");
            }
            var startPage = pageInfo.PageInfo.Page + 1;

            for (var i = startPage; i < pageInfo.PageInfo.TotalPage; i++)
            {
                var msg = $"获取小时报表 第：{startPage}页 开始";
                Loger.GdtLogger.Info(msg);
                request.Page = i;//pageIndex ++
                var pageInfoNext = ServiceHelper.Instance.GetReportHourly(request);
                if (pageInfoNext.List == null || pageInfoNext.List.Count == 0)
                {
                    msg += $" GetReportHourly 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10012", msg);
                }
                var maxHour = Dal.GDT.GdtHourlyRrport.Instance.GetMaxHour(entityWhere);

                var filterPageList = pageInfo.List.Where(s => s.Hour > maxHour).ToList();
                if (!filterPageList.Any())
                {
                    msg = $"获取小时报表 GetReportHourly 为null，过滤掉了数据库里已存在的最大hour，GetMaxHour 参数:{JsonConvert.SerializeObject(entityWhere)}";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10021", msg);
                }
                var insertList = AutoMapper.Mapper.Map<List<RespReportDto>, List<Entities.GDT.GdtHourlyRrport>>(filterPageList);
                //第一页处理
                Dal.GDT.GdtHourlyRrport.Instance.InsertByGdtServer(insertList, entityWhere, i);

                Loger.GdtLogger.Info($"获取小时报表 第：{startPage}页 结束");
            }

            return CreateSuccessMessage(retValue, "0", "GetReportHourly  获取小时报表入库完成");
        }

        /// <summary>
        ///  获取智慧云小时报表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="demandBillNo"></param>
        /// <returns></returns>
        public ReturnValue DoPullGetZhyHourlyReport(ReqReportDto request, int demandBillNo)
        {
            var retValue = new ReturnValue();
            var pageInfo = ServiceHelper.Instance.GetReportHourly(request);
            if (pageInfo == null || pageInfo.List == null || pageInfo.List.Count == 0)
            {
                var msg = $"获取智慧云小时报表 GetReportHourly 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                Loger.GdtLogger.Info(msg);
                return CreateFailMessage(retValue, "10030", msg);
            }
            var insertList = AutoMapper.Mapper.Map<List<RespReportDto>, List<Entities.GDT.GdtHourlyRrportForZhy>>(pageInfo.List);
            //第一页处理
            var entityWhere = new Entities.GDT.GdtHourlyRrportForZhy
            {
                AccountId = request.AccountId,
                Level = GdtDataEnumProvider.GetDicLevel(request.Level.ToString()),
                Date = DateTime.Parse(request.Date),
                DemandBillNo = demandBillNo
            };
            Dal.GDT.GdtHourlyRrportForZhy.Instance.InsertByGdtServer(insertList, entityWhere, pageInfo.PageInfo.Page);
            return DoPullGetZhyHourlyReportPageNext(retValue, request, pageInfo, entityWhere);
        }

        public ReturnValue DoPullGetZhyHourlyReportPageNext(ReturnValue retValue, ReqReportDto request,
            RespPageInfo<List<RespReportDto>> pageInfo, Entities.GDT.GdtHourlyRrportForZhy entityWhere)
        {
            if (pageInfo.PageInfo.TotalPage == pageInfo.PageInfo.Page)
            {
                return CreateSuccessMessage(retValue, "", "DoPullGetZhyHourlyReportPageNext  获取智慧云小时报表入库完成");
            }
            var startPage = pageInfo.PageInfo.Page + 1;

            for (var i = startPage; i < pageInfo.PageInfo.TotalPage; i++)
            {
                var msg = $"获取智慧云小时报表 第：{startPage}页 开始";
                Loger.GdtLogger.Info(msg);
                request.Page = i;//pageIndex ++
                var pageInfoNext = ServiceHelper.Instance.GetReportHourly(request);
                if (pageInfoNext.List == null || pageInfoNext.List.Count == 0)
                {
                    msg += $" GetReportHourly Zhy 为null，拉取信息业务参数:{JsonConvert.SerializeObject(request)}等待下一次执行";
                    Loger.GdtLogger.Info(msg);
                    return CreateFailMessage(retValue, "10012", msg);
                }
                var insertList = AutoMapper.Mapper.Map<List<RespReportDto>, List<Entities.GDT.GdtHourlyRrportForZhy>>(pageInfo.List);
                //第一页处理
                Dal.GDT.GdtHourlyRrportForZhy.Instance.InsertByGdtServer(insertList, entityWhere, i);

                Loger.GdtLogger.Info($"获取智慧云小时报表 第：{startPage}页 结束");
            }

            return CreateSuccessMessage(retValue, "", "GetReportHourly  获取智慧云小时报表入库完成");
        }

        #endregion 报表

        #region 刷新获取 Access Token

        public ReturnValue DoPullGetAccessTokenByRefreshToken(Entities.GDT.GdtAccessToken reqAccessToken)
        {
            var retValue = new ReturnValue();
            if (reqAccessToken == null || string.IsNullOrWhiteSpace(reqAccessToken.RefreshToken))
            {
                return CreateFailMessage(retValue, "10021", "refreshToken为null，不执行下一步");
            }
            var token = ServiceHelper.Instance.GetAccessTokenByRefreshToken(reqAccessToken.RefreshToken);
            if (token == null)
            {
                return CreateFailMessage(retValue, "10022", "GetAccessTokenByRefreshToken为null，不执行下一步");
            }

            reqAccessToken.AccessToken = token.AccessToken;
            reqAccessToken.RefreshToken = token.RefreshToken;
            reqAccessToken.AccessTokenExpiresIn = token.AccessTokenExpiresIn;
            reqAccessToken.ClientId = reqAccessToken.ClientId;

            Dal.GDT.GdtAccessToken.Instance.InsertByGdtServer(reqAccessToken);

            return CreateSuccessMessage(retValue, "", "DoPullGetAccessToken  获取刷新获取 Access Token入库完成");
        }

        #endregion 刷新获取 Access Token
    }
}