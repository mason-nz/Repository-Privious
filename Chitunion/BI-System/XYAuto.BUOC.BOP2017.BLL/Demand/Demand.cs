using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using log4net;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using XYAuto.BUOC.BOP2017.BLL.GDT;
using XYAuto.BUOC.BOP2017.Entities.Demand;
using XYAuto.BUOC.BOP2017.Entities.Dto;
using XYAuto.BUOC.BOP2017.Entities.Query.Demand;
using System.Data;
using XYAuto.BUOC.BOP2017.Infrastruction;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.Demand
{
    public class Demand
    {
        public static readonly Demand Instance = new Demand();
        private ILog logger = LogManager.GetLogger(typeof(Demand));

        public XYAuto.ITSC.Chitunion2017.Common.LoginUser GetUserInfo =>
              XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();

        private static string SuperAdmin = "SYS005RL00019";
        private static string AdYY = "SYS005RL00021";

        public GetDemandListResDTO GetDemandList(GetDemandListReqDTO req, ref string msg)
        {
            var res = Dal.Demand.Demand.Instance.GetDemandList(req.CreateUser, req.BelongYY, req.DemandName, req.AuditStatus, req.UserId, req.PageIndex, req.PageSize);
            return res;
        }

        public GetDemandDetailResDTO GetDemandDetail(int demandBillNo, ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }

            var res = Dal.Demand.Demand.Instance.GetDemandDetail(demandBillNo, userID);
            if (res.Demand != null)
            {
                #region 车型车系、区域、经销商

                if (!string.IsNullOrWhiteSpace(res.Demand.BrandSerialJson))
                {
                    var list = JsonConvert.DeserializeObject<List<GDT.Dto.Request.CarInfoDto>>(res.Demand.BrandSerialJson);
                    if (list != null && list.Count > 0)
                    {
                        list.ForEach(brand =>
                        {
                            brand.CarSerialInfo.ForEach(serial =>
                            {
                                res.Demand.BrandAndCarSerialList.Add(brand.BrandName + "—" + serial.CarSerialName);
                            });
                        });
                    }
                    res.Demand.BrandSerialJson = string.Empty;
                }
                if (!string.IsNullOrWhiteSpace(res.Demand.ProvinceCityJson))
                {
                    var list = JsonConvert.DeserializeObject<List<GDT.Dto.Request.AreaInfoDto>>(res.Demand.ProvinceCityJson);
                    if (list != null && list.Count > 0)
                    {
                        list.ForEach(province =>
                        {
                            string key = string.Empty;
                            key += province.ProvinceName + "（";
                            province.City.ForEach(city =>
                            {
                                key += city.CityName + "、";
                            });
                            key = key.TrimEnd('、');
                            key += "）";
                            if (key.StartsWith("全国"))
                                key = "全国";
                            res.Demand.ProvinceAndCityList.Add(key);
                        });
                    }
                    res.Demand.ProvinceCityJson = string.Empty;
                }
                if (!string.IsNullOrWhiteSpace(res.Demand.DistributorJson))
                {
                    var list = JsonConvert.DeserializeObject<List<GDT.Dto.Request.DistributorDto>>(res.Demand.DistributorJson);
                    if (list != null && list.Count > 0)
                    {
                        list.ForEach(d =>
                        {
                            res.Demand.DistributorList.Add(d.DistributorName);
                        });
                    }
                    res.Demand.DistributorJson = string.Empty;
                }

                #endregion 车型车系、区域、经销商

                //广告最后更新时间
                if (res.ADGroupList != null && res.ADGroupList.Count > 0)
                    res.Demand.ADLastUpdateTime = res.ADGroupList.Max(a => a.PullTime);
                res.Demand.DuringDate = "（共" + ((res.Demand.EndDate.Date - res.Demand.BeginDate.Date).Days + 1) + "天）" + res.Demand.BeginDate.ToString("yyyy年M月d日") + "—" + res.Demand.EndDate.ToString("yyyy年M月d日");
                res.Demand.ClueNumberStr = res.Demand.ClueNumber + "条（约每日线索量：" + Math.Round((decimal)res.Demand.ClueNumber / ((res.Demand.EndDate.Date - res.Demand.BeginDate.Date).Days + 1), MidpointRounding.AwayFromZero) + "，线索单价：" + Math.Round(res.Demand.TotalBudget / res.Demand.ClueNumber / 100, 2) + "元）";
            }
            return res;
        }

        public bool AuditDemand(AuditDemandReqDto req, ref string msg, ref int nextBillNo)
        {
            var startTime = DateTime.Now;
            XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，记录时间 开始.....{startTime}");
            if (req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Rejected
         && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn
         && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated
         && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.IsOver)
            {
                msg = "审核类型错误";
                return false;
            }
            if (req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Rejected && string.IsNullOrWhiteSpace(req.Reason))
            {
                msg = "缺少审核原因";
                return false;
            }
            var demand = Dal.Demand.Demand.Instance.GetDemandEntityByBillNo(req.DemandBillNo);
            XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，GetDemandEntityByBillNo 开始.....{(startTime - DateTime.Now).TotalMilliseconds} ms");
            if (demand == null)
            {
                msg = "需求不存在";
                return false;
            }

            //if (demand.AuditStatus == DemandAuditStatusEnum.Revoke)
            //{
            //    msg = "需求已撤销";
            //    return false;
            //}

            if ((req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated || req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.IsOver)
                && !Dal.Demand.Demand.Instance.CheckDemandCanStop(req.DemandBillNo))
            {
                msg = "需求下有有效的广告";
                return false;
            }

            XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，CheckDemandCanStop 开始.....{(DateTime.Now - startTime).TotalMilliseconds} ms");
            if (req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn
                || req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated
                || req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.IsOver)
                req.Reason = string.Empty;

            bool res = Dal.Demand.Demand.Instance.AuditDemand(req.DemandBillNo, req.AuditStatus, req.Reason, GetUserInfo.UserID);

            XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，AuditDemand 开始.....{(DateTime.Now - startTime).TotalMilliseconds} ms");

            if (res)
            {
                int organizeld = Dal.Demand.Demand.Instance.GetOrganizeldByDemandBillNo(req.DemandBillNo);
                XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，GetOrganizeldByDemandBillNo 开始.....{(DateTime.Now - startTime).TotalMilliseconds} ms");

                if (organizeld != 0)
                {
                    int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                    XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，GetLoginUserID 开始.....{(DateTime.Now - startTime).TotalMilliseconds} ms");

                    Task.Run(() =>
                    {
                        LogicByZhyProvider provider = new LogicByZhyProvider();
                        provider.DemandStatusNote(new GDT.Dto.Request.ToDemandNotes
                        {
                            DemandBillNo = req.DemandBillNo,
                            Reject = string.IsNullOrWhiteSpace(req.Reason) ? "无" : req.Reason,
                            AuditStatus = (Entities.Enum.GDT.DemandAuditStatusEnum)req.AuditStatus,
                            OrganizeId = organizeld
                        });
                    });
                    Task.Run(() =>
                    {
                        if (req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated || req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.IsOver)
                        {
                            decimal ZhyRechargeAmount = Dal.ZHY.ZhyInfo.Instance.SelectZhyRechargeAmount(req.DemandBillNo);
                            decimal ZhyConsumeAmount = Dal.ZHY.ZhyInfo.Instance.SelectZhyConsumeAmount(req.DemandBillNo);
                            if (ZhyRechargeAmount != 0)
                            {
                                if (ZhyRechargeAmount - ZhyConsumeAmount < 0)
                                {
                                    DataTable dt = Dal.Demand.Demand.Instance.SelectDemandRechargeInfo(req.DemandBillNo);
                                    if (dt == null || dt.Rows.Count <= 0)
                                    {
                                        Loger.Log4Net.Error("[XYAuto.BUOC.BOP2017.BLL.Demand]->AuditDemand:回划需求单：" + req.DemandBillNo + "未查到广点通广告主信息");
                                        return;
                                    }
                                    DemandRefundInfo drInfo = new DemandRefundInfo();
                                    drInfo.AccountId = Convert.ToInt32(dt.Rows[0]["AccountId"]);
                                    drInfo.DemandBillNo = req.DemandBillNo;
                                    drInfo.RechargeNumber = dt.Rows[0]["RechargeNumber"].ToString();
                                    drInfo.TradeType = TradeTypeEnum.TRANSFER_BACK;
                                    drInfo.RechargeAmount = Convert.ToInt32(ZhyRechargeAmount);
                                    drInfo.SpendAmount = Convert.ToInt32(ZhyConsumeAmount);
                                    drInfo.TransferBackAmount = Convert.ToInt32(ZhyRechargeAmount - ZhyConsumeAmount);
                                    drInfo.CreateTime = DateTime.Now;
                                    drInfo.CreateUserId = userId;
                                    if (Dal.Demand.Demand.Instance.InsertAbnormalFunds(drInfo) <= 0)
                                    {
                                        Loger.Log4Net.Error("[XYAuto.BUOC.BOP2017.BLL.Demand]->AuditDemand:插入回划异常数据错误—>DemandBillNo:" + drInfo.DemandBillNo + "RechargeNumber:" + drInfo.RechargeNumber + "AccountId:" + drInfo.AccountId + "RechargeAmount:" + drInfo.RechargeAmount + "SpendAmount:" + drInfo.SpendAmount + "TransferBackAmount:" + drInfo.TransferBackAmount + "CreateUserId:" + drInfo.CreateUserId);
                                    }
                                    else
                                    {
                                        LogicByZhyProvider lbzp = new LogicByZhyProvider();




                                        ToAccountFundNotes tfnModel = new ToAccountFundNotes();
                                        tfnModel.DemandBillNo = req.DemandBillNo;
                                        tfnModel.OrganizeId = organizeld;
                                        tfnModel.TradeNo = "";
                                        tfnModel.TradeMoney = ZhyRechargeAmount - ZhyConsumeAmount;
                                        tfnModel.MoneyTpe = ZhyEnum.ZhyMoneyTpeEnum.现金;
                                        tfnModel.TradeType = ZhyEnum.ZhyTradeTypeEnum.回划异常;
                                        ReturnValue zhyResult = lbzp.AccountFundsNote(tfnModel);
                                        if (zhyResult.HasError)
                                        {
                                            logger.Error($"需求单：{req.DemandBillNo}回划异常错误：" + zhyResult.Message);
                                        }
                                    }
                                }
                            }
                        }
                    });
                    XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，Task.Run 开始.....{(DateTime.Now - startTime).TotalMilliseconds} ms");
                }
                else
                {
                    logger.Error($"DemandBillNo={req.DemandBillNo}缺少Organizeld!");
                }
                nextBillNo = Dal.Demand.Demand.Instance.SelectNextWaittingAuditDemand();
                XYAuto.BUOC.BOP2017.Infrastruction.Loger.Log4Net.Info($" AuditDemand 方法，SelectNextWaittingAuditDemand 开始.....{(DateTime.Now - startTime).TotalMilliseconds} ms");
            }
            else
            {
                msg = "需求状态不正确";
                nextBillNo = 0;
            }
            return res;
        }

        public List<ADGroupDTO> GetWaittingADList(int deliveryId, ref string msg)
        {//只能从广告主关联的子客的所属广告组里面选
            var groundDeliverys = Dal.Demand.DemandGroundDelivery.Instance.GetList(new DemandGroundQuery<DemandGroundDelivery>()
            {
                DeliveryId = deliveryId
            });

            if (!groundDeliverys.Any())
            {
                msg = "需求单号不存在或deliveryId不存在";
                return null;
            }
            var demand = groundDeliverys.FirstOrDefault();
            if (demand == null)
            {
                msg = "需求单号不存在或deliveryId不存在-2";
                return null;
            }
            if (demand.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn
                && demand.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Puting)
            {
                msg = "当前需求审核状态不可关联广告";
                return null;
            }
            if (!Dal.Demand.Demand.Instance.CheckDemandHasReCharge(demand.DemandBillNo))
            {
                msg = "该需求没有充值单，不可关联广告";
                return null;
            }
            // zlb 去除需求结束时间判断
            if (demand.BeginDate.Date > DateTime.Now.Date)
            {
                msg = "暂未到需求开始日期，不可关联";
                return null;
            }
            return Dal.Demand.Demand.Instance.GetCanSelectAdGroupList(deliveryId, demand.DemandBillNo);
        }

        public bool RelateToADGroup(AuditDemandReqDto req, ref string msg)
        {
            var demand = Dal.Demand.Demand.Instance.GetDemandEntityByBillNo(req.DemandBillNo);
            if (demand == null)
            {
                msg = "需求单号不存在";
                return false;
            }
            if (demand.AuditStatus != Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn && demand.AuditStatus != Entities.Enum.GDT.DemandAuditStatusEnum.Puting)
            {
                msg = "当前需求审核状态不可关联广告";
                return false;
            }
            if (!Dal.Demand.Demand.Instance.CheckDemandHasReCharge(req.DemandBillNo))
            {
                msg = "该需求没有充值单，不可关联广告";
                return false;
            }
            if (demand.BeginDate.Date > DateTime.Now.Date || demand.EndDate.Date < DateTime.Now.Date)
            {
                msg = "暂未到需求开始日期，不可关联";
                return false;
            }
            if (req.ADGroupList == null || req.ADGroupList.Count == 0)
            {
                msg = "至少选择一个广告组";
                return false;
            }
            bool res = Dal.Demand.Demand.Instance.RelateToADGroupList(req.DemandBillNo, req.ADGroupList, GetUserInfo.UserID);

            return res;
        }

        #region 效果图相关

        public List<MapGetLeftItemDTO> MapGetLeft(ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }

            return Dal.Demand.Demand.Instance.GetDemandADGroupMenu(userID);
        }

        public MapGetRightOneResDTO MapGetRightOne(ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }

            return Dal.Demand.Demand.Instance.GetAccountInfo(userID);
        }

        public MapGetRightTwoResDTO MapGetRightTwo(MapGetRightReqDTO req, ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }
            String begintime = "1900-01-01";
            String endtime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            switch (req.DateType)
            {
                case -1:
                    begintime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    endtime = DateTime.Now.ToString("yyyy-MM-dd");
                    break;

                case 1:
                    begintime = DateTime.Now.ToString("yyyy-MM-dd");
                    endtime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    break;

                case 7:
                    begintime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endtime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    break;

                case 30:
                    begintime = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd");
                    endtime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    break;
            }
            return Dal.Demand.Demand.Instance.GetStatistic(req.DemandBillNo, req.ADGroupID, userID, begintime, endtime);
        }

        public List<MapGetRightThreeItemDTO> MapGetRightThree(MapGetRightReqDTO req, ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }

            switch (req.DateType)
            {
                #region 时间类型验证

                case -1://昨天
                    req.BeginDate = DateTime.Now.Date.AddDays(-1);
                    req.EndDate = DateTime.Now.Date;
                    break;
                //case 0://自定义
                //    if (req.BeginDate == Entities.Constants.Constant.DATE_INVALID_VALUE && req.EndDate == Entities.Constants.Constant.DATE_INVALID_VALUE)
                //    {
                //        req.BeginDate = DateTime.Now.Date;
                //        req.EndDate = req.BeginDate;
                //    }
                //    else if (req.BeginDate == Entities.Constants.Constant.DATE_INVALID_VALUE)
                //    {
                //        req.BeginDate = req.EndDate;
                //    }
                //    else if (req.EndDate == Entities.Constants.Constant.DATE_INVALID_VALUE)
                //    {
                //        req.EndDate = req.BeginDate;
                //    }
                //    break;
                case 1://今天
                    req.BeginDate = DateTime.Now.Date;
                    req.EndDate = req.BeginDate.AddDays(1);
                    break;

                case 7://近7天
                    req.BeginDate = DateTime.Now.Date.AddDays(-7);
                    req.EndDate = DateTime.Now.Date;
                    break;

                case 30://近30天
                    req.BeginDate = DateTime.Now.Date.AddDays(-30);
                    req.EndDate = DateTime.Now.Date;
                    break;

                default:
                    msg = "时间类型错误";
                    return null;

                    #endregion 时间类型验证
            }
            if (!new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Contains(req.DataType))
            {
                msg = "数据类型错误";
                return null;
            }
            var res = Dal.Demand.Demand.Instance.GetChart(req.DemandBillNo, req.ADGroupID, req.BeginDate, req.EndDate, req.DataType, userID);
            if (res != null && res.Count > 0 && (req.EndDate.Date - req.BeginDate.Date).Days > 2)
            {//大于两天 按天统计
                List<MapGetRightThreeItemDTO> dayRes = new List<MapGetRightThreeItemDTO>();
                res.GroupBy(i => i.Date).ToList().ForEach(gb =>
                {
                    dayRes.Add(new MapGetRightThreeItemDTO { Key = gb.Key.ToString("yyyy-MM-dd"), Value = req.DataType == 3 ? gb.Average(i => i.Value) : gb.Sum(i => i.Value) });
                });
                //return dayRes;
                return FillData(req, dayRes);
            }
            //return res;
            return FillData(req, res);
        }

        /// <summary>
        /// 数据填充
        /// </summary>
        /// <param name="req">请求</param>
        /// <param name="source">原始数据</param>
        /// <returns></returns>
        private static List<MapGetRightThreeItemDTO> FillData(MapGetRightReqDTO req, List<MapGetRightThreeItemDTO> source)
        {
            List<MapGetRightThreeItemDTO> result = new List<MapGetRightThreeItemDTO>();
            switch (req.DateType)
            {
                #region 昨天

                case -1:
                    for (int i = 0; i < 24; i++)
                    {
                        MapGetRightThreeItemDTO entity = new MapGetRightThreeItemDTO();
                        var item = source.Where(p => p.Key == i.ToString()).ToList<MapGetRightThreeItemDTO>();
                        if (item.Count == 0)
                        {
                            entity.Date = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                            entity.Key = i.ToString();
                            entity.Value = 0.0M;
                        }
                        else
                        {
                            entity.Date = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                            entity.Key = item[0].Key;
                            entity.Value = item[0].Value;
                        }
                        result.Add(entity);
                    }
                    break;

                #endregion 昨天

                #region 今天

                case 1:
                    for (int i = 0; i < 24; i++)
                    {
                        MapGetRightThreeItemDTO entity = new MapGetRightThreeItemDTO();
                        var item = source.Where(p => p.Key == i.ToString()).ToList<MapGetRightThreeItemDTO>();
                        if (item.Count == 0)
                        {
                            entity.Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            entity.Key = i.ToString();
                            entity.Value = 0.0M;
                        }
                        else
                        {
                            entity.Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            entity.Key = item[0].Key;
                            entity.Value = item[0].Value;
                        }
                        result.Add(entity);
                    }
                    break;

                #endregion 今天

                #region 进7天

                case 7:
                    for (int i = 0; i <= 6; i++)
                    {
                        MapGetRightThreeItemDTO entity = new MapGetRightThreeItemDTO();
                        var item = source.Where(p => p.Key == DateTime.Now.Date.AddDays(-(7 - i)).ToString("yyyy-MM-dd")).ToList<MapGetRightThreeItemDTO>();
                        if (item.Count == 0)
                        {
                            entity.Date = DateTime.Now.Date.AddDays(-(7 - i));
                            entity.Key = DateTime.Now.Date.AddDays(-(7 - i)).ToString("yyyy-MM-dd");
                            entity.Value = 0.0M;
                        }
                        else
                        {
                            entity.Date = item[0].Date;
                            entity.Key = item[0].Key;
                            entity.Value = item[0].Value;
                        }
                        result.Add(entity);
                    }
                    break;

                #endregion 进7天

                #region 进30天

                case 30:
                    for (int i = 0; i <= 29; i++)
                    {
                        MapGetRightThreeItemDTO entity = new MapGetRightThreeItemDTO();
                        var item = source.Where(p => p.Key == DateTime.Now.Date.AddDays(-(30 - i)).ToString("yyyy-MM-dd")).ToList<MapGetRightThreeItemDTO>();
                        if (item.Count == 0)
                        {
                            entity.Date = DateTime.Now.Date.AddDays(-(30 - i));
                            entity.Key = DateTime.Now.Date.AddDays(-(30 - i)).ToString("yyyy-MM-dd");
                            entity.Value = 0.0M;
                        }
                        else
                        {
                            entity.Date = item[0].Date;
                            entity.Key = item[0].Key;
                            entity.Value = item[0].Value;
                        }
                        result.Add(entity);
                    }
                    break;

                    #endregion 进30天
            }
            return result;
        }

        public MapGetRightADListResDTO MapGetRightFour(MapGetRightReqDTO req, ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }

            string orderByStr = string.Empty;
            switch (req.OrderBy)
            {
                case 1:
                    orderByStr = "TotalImpression ";
                    break;

                case 2:
                    orderByStr = "TotalClick ";
                    break;

                case 3:
                    orderByStr = "AvgClickPercent ";
                    break;

                case 4:
                    orderByStr = "TotalCost ";
                    break;

                case 5:
                    orderByStr = "OrderQuantity ";
                    break;

                case 6:
                    orderByStr = "BillOfQuantities ";
                    break;

                case 7:
                    orderByStr = "PV ";
                    break;

                case 8:
                    orderByStr = "UV ";
                    break;

                case 9:
                    orderByStr = "ClueCount ";
                    break;

                case 10:
                    orderByStr = "CluePrice ";
                    break;
            }
            orderByStr += (req.IsDesc.Equals(1) ? "desc" : "asc");
            return Dal.Demand.Demand.Instance.GetADGroupRankList(req.DemandBillNo, userID, orderByStr, req.PageIndex, req.PageSize);
        }

        public MapGetRightADListResDTO MapGetRightFive(MapGetRightReqDTO req, ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }

            string startDt = null, endDt = null;
            if (req.BeginDate != Entities.Constants.Constant.DATE_INVALID_VALUE && req.EndDate != Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                startDt = req.BeginDate.ToString("yyyy-MM-dd");
                endDt = req.EndDate.ToString("yyyy-MM-dd");
            }
            else if (req.BeginDate != Entities.Constants.Constant.DATE_INVALID_VALUE && req.EndDate == Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                startDt = endDt = req.BeginDate.ToString("yyyy-MM-dd");
            }
            else if (req.BeginDate == Entities.Constants.Constant.DATE_INVALID_VALUE && req.EndDate != Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                startDt = endDt = req.BeginDate.ToString("yyyy-MM-dd");
            }
            return Dal.Demand.Demand.Instance.GetADGroupDetailList(req.DemandBillNo, req.ADGroupID, startDt, endDt, userID, req.PageIndex, req.PageSize);
        }

        public string MapExportToExcel(MapGetRightReqDTO req, ref string msg)
        {
            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            int userID = 0;
            if (ur.IndexOf(AdYY, StringComparison.Ordinal) > -1)
            {
                //广告运营
                userID = GetUserInfo.UserID;
            }

            string startDt = null, endDt = null;
            if (req.BeginDate != Entities.Constants.Constant.DATE_INVALID_VALUE && req.EndDate != Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                startDt = req.BeginDate.ToString("yyyy-MM-dd");
                endDt = req.EndDate.ToString("yyyy-MM-dd");
            }
            else if (req.BeginDate != Entities.Constants.Constant.DATE_INVALID_VALUE && req.EndDate == Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                startDt = endDt = req.BeginDate.ToString("yyyy-MM-dd");
            }
            else if (req.BeginDate == Entities.Constants.Constant.DATE_INVALID_VALUE && req.EndDate != Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                startDt = endDt = req.BeginDate.ToString("yyyy-MM-dd");
            }
            var list = Dal.Demand.Demand.Instance.GetAllADGroupDetailList(req.DemandBillNo, req.ADGroupID, startDt, endDt, userID);
            if (list == null || list.Count == 0)
                return string.Empty;
            string pagePath = string.Format("{0}\\UploadFiles\\ADGroupData\\{1}\\{2}\\{3}\\{4}",
                            WebConfigurationManager.AppSettings["UploadFilePath"],
                            DateTime.Now.Year,
                            DateTime.Now.Month,
                            DateTime.Now.Day,
                            DateTime.Now.Hour);
            if (!Directory.Exists(pagePath))
                Directory.CreateDirectory(pagePath);
            string fileName = "需求数据明细.xls";
            string backFilePath = pagePath + "\\" + fileName;
            string frontFilePath = string.Format("/uploadfiles/ADGroupData/{0}/{1}", DateTime.Now.ToString("yyyy/M/d/H"), fileName);

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            string headerStr = "日期|时间段|点击量|曝光量|平均点击率|花费|订单量|话单量|PV|UV|线索量|线索单价·";
            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            string[] headers = headerStr.Split('|');
            for (int i = 0; i < headers.Length; i++)
                dataRow.CreateCell(i).SetCellValue(headers[i]);
            int rownumber = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(rownumber);
                dataRow.CreateCell(0).SetCellValue(item.Date.ToString("yyyy-MM-dd"));
                dataRow.CreateCell(1).SetCellValue($"{item.Hour - 1}至{item.Hour}");
                dataRow.CreateCell(2).SetCellValue(item.TotalClick);
                dataRow.CreateCell(3).SetCellValue(item.TotalImpression);
                dataRow.CreateCell(4).SetCellValue(item.AvgClickPercent * 100 + "%");
                dataRow.CreateCell(5).SetCellValue(item.TotalCost);
                dataRow.CreateCell(6).SetCellValue(item.OrderQuantity);
                dataRow.CreateCell(7).SetCellValue(item.BillOfQuantities);

                dataRow.CreateCell(8).SetCellValue(item.PV);
                dataRow.CreateCell(9).SetCellValue(item.UV);
                dataRow.CreateCell(10).SetCellValue(item.ClueCount);
                dataRow.CreateCell(11).SetCellValue(Convert.ToDouble(item.CluePrice.ToString("0.00")));
                rownumber++;
            }
            using (FileStream fs = new FileStream(backFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            return frontFilePath;
        }

        #endregion 效果图相关

        public int GetDemandBillNoByPromotionUrlCode(string code)
        {
            return Dal.Demand.DemandGroundDelivery.Instance.GetDemandBillNoByPromotionUrlCode(code);
        }
    }
}