/********************************************************
*创建人：lixiong
*创建时间：2017/8/16 10:18:28
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.Demand;
using XYAuto.BUOC.BOP2017.BLL.Demand.Resolver;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Response;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Entities.GDT;
using XYAuto.BUOC.BOP2017.Entities.Query.GDT;
using XYAuto.BUOC.BOP2017.Infrastruction;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.BOP2017.BLL.GDT
{
    /// <summary>
    /// 智慧云相关逻辑中转
    /// </summary>
    public class LogicTransferProvider : Infrastruction.Verification.VerifyOperateBase
    {
        public LogicTransferProvider()
        {
        }

        private ReturnValue VerifySign(RequestBaseTokenDto token)
        {
            return new ReturnValue();
        }

        /// <summary>
        /// 智慧云推送用户
        /// </summary>
        /// <param name="pushUserContext"></param>
        /// <returns></returns>
        public ReturnValue PushUser(RequestPushUserDto pushUserContext)
        {
            //todo:
            //1.先同步数据到userInfo表，
            //2.再添加到指定角色下（广告主）
            /*
             1.OrganizeId查询是否已经存在
                --->存在：更新其他字段，手机号不修改（避免这次修改又和赤兔重复）
                --->不存在：查询手机号在赤兔，广告主角色下是否重复
                                --->存在：终止
                                --->不存在：入库：用户表-->用户详情表-->用户角色表（默认广告主角色）-->UserOrganize关联表
             */

            var retValue = VerifyOfNecessaryParameters<RequestPushUserDto>(pushUserContext);
            if (retValue.HasError)
                return retValue;

            var organizeInfo = Dal.GDT.GdtAccountInfo.Instance.VerifyOfOrganizeId(pushUserContext.OrganizeId);
            int userId = 0;
            if (organizeInfo == null)
            {
                //校验是否在
                var userRole = Dal.GDT.GdtAccountInfo.Instance.VerifyOfRole(pushUserContext.Mobile, RoleInfoMapping.Advertiser);
                if (userRole != null)
                {
                    return CreateFailMessage(retValue, "10001", $"此用户：{pushUserContext.Mobile} 已存在赤兔系统里，请更换手机号");
                }
                var reqUserInfo = AutoMapper.Mapper.Map<RequestPushUserDto, Entities.GDT.UserInfo>(pushUserContext);
                userId = Dal.GDT.GdtAccountInfo.Instance.InsertUserInfo(reqUserInfo);
                if (userId <= 0)
                {
                    return CreateFailMessage(retValue, "10002", "用户入库失败");
                }
            }
            else
            {
                //todo:更新其他字段，手机号不修改（避免这次修改又和赤兔重复）
                userId = organizeInfo.UserID;
            }
            Dal.GDT.GdtAccountInfo.Instance.InsertUserRole(userId, RoleInfoMapping.Advertiser, pushUserContext.CorporationName,
                pushUserContext.ContactsPerson, pushUserContext.OrganizeId);

            return CreateSuccessMessage(retValue, "0", "success", userId);
        }

        /// <summary>
        /// 智慧云推送的需求详情
        /// </summary>
        /// <param name="pushDemandContext"></param>
        /// <returns></returns>
        public ReturnValue PushDemand(RequestPushDemandDto pushDemandContext)
        {
            //todo:
            //1.接受智慧云推送的需求，默认是待审核，审核操作之后要调取智慧云接口进行推送通知
            //2.校验需求提交人的信息，获取userid
            //3.如果同一个需求单号过来，判断状态，如果是驳回，就认为是重新提交，反之就返回错误，不能重复添加
            var retValue = VerifyOfNecessaryParameters<RequestPushDemandDto>(pushDemandContext);
            if (retValue.HasError)
                return retValue;
            var beginDate = DateTime.Parse(pushDemandContext.BeginDate);
            var endDate = DateTime.Parse(pushDemandContext.EndDate);

            if ((endDate - beginDate).TotalDays < 0)
            {
                return CreateFailMessage(retValue, "10014", "需求时间区间有问题");
            }

            var userInfo = Dal.GDT.GdtAccountInfo.Instance.VerifyOfOrganizeId(pushDemandContext.OrganizeId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "10010", $"此操作人：{pushDemandContext.OrganizeId} 不存在");
            }
            var gdtDemand = AutoMapper.Mapper.Map<RequestPushDemandDto, Entities.GDT.GdtDemand>(pushDemandContext);

            var billInfo = Dal.GDT.GdtDemand.Instance.GetInfoByDemandBillNo(pushDemandContext.DemandBillNo);
            if (billInfo == null)
            {
                //todo:新增
                gdtDemand.CreateUserId = userInfo.UserID;
                var demandId = Dal.GDT.GdtDemand.Instance.Insert(gdtDemand);
                if (demandId <= 0)
                {
                    Loger.Log4Net.ErrorFormat($"需求更新入库失败,参数:{JsonConvert.SerializeObject(gdtDemand)}");
                    return CreateFailMessage(retValue, "10011", "需求入库失败");
                }
            }
            else
            {
                if (billInfo.AuditStatus != DemandAuditStatusEnum.Rejected
                    && billInfo.AuditStatus != DemandAuditStatusEnum.Revoke)
                {
                    return CreateFailMessage(retValue, "10012", "此需求当前状态下不允许更新");
                }
                gdtDemand.DemandBillNo = pushDemandContext.DemandBillNo;
                gdtDemand.UpdateUserId = userInfo.UserID;
                gdtDemand.AuditStatus = DemandAuditStatusEnum.PendingAudit;
                if (Dal.GDT.GdtDemand.Instance.UpdateByDemandBillNo(gdtDemand) == 0)
                {
                    Loger.Log4Net.ErrorFormat($"需求更新入库失败,参数:{JsonConvert.SerializeObject(billInfo)}");
                    return CreateFailMessage(retValue, "10013", "需求更新入库失败");
                }
            }
            var jsonAnalysis = new DemandJsonAnalysis();
            Task.Run(() =>
            {
                jsonAnalysis.CarSerielAnalysis(pushDemandContext.CarInfo, pushDemandContext.DemandBillNo);
                jsonAnalysis.CitysAnalysis(pushDemandContext.AreaInfo, pushDemandContext.DemandBillNo);
            });

            return CreateSuccessMessage(retValue, "0", "success");
        }

        /// <summary>
        /// 需求撤销
        /// </summary>
        /// <param name="demandRevokeContext"></param>
        /// <returns></returns>
        public ReturnValue DemandRevoke(RequestDemandRevokeDto demandRevokeContext)
        {
            //todo:
            //1.只允许待投放状态下的（审核通过）撤销
            //2.
            var retValue = VerifyOfNecessaryParameters<RequestDemandRevokeDto>(demandRevokeContext);
            if (retValue.HasError)
                return retValue;
            var userInfo = Dal.GDT.GdtAccountInfo.Instance.VerifyOfOrganizeId(demandRevokeContext.OrganizeId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "10030", $"此操作人：{demandRevokeContext.OrganizeId} 不存在");
            }
            var billInfo = Dal.GDT.GdtDemand.Instance.GetInfoByDemandBillNo(demandRevokeContext.DemandBillNo);
            if (billInfo == null)
                return CreateFailMessage(retValue, "10031", $"此需求单号不存在：{demandRevokeContext.DemandBillNo}");
            if (billInfo.AuditStatus != DemandAuditStatusEnum.PendingAudit)
            {
                return CreateFailMessage(retValue, "10032", "此需求当前状态下不允许撤销");
            }
            var updateTime = DateTime.Now;
            if (Dal.GDT.GdtDemand.Instance.UpdateAuditStatus(demandRevokeContext.DemandBillNo,
                DemandAuditStatusEnum.Revoke, userInfo.UserID, updateTime) <= 0)
            {
                Loger.Log4Net.Error($"需求单号:{demandRevokeContext.DemandBillNo},撤销失败");
                return CreateFailMessage(retValue, "10033", "撤销失败");
            }
            //todo:撤销成功,通知智慧云
            Task.Run(() => ToNoteDemandStatus(demandRevokeContext, updateTime));

            return retValue;
        }

        public void ToNoteDemandStatus(RequestDemandRevokeDto demandRevokeContext, DateTime updateTime)
        {
            var request = new ToDemandNotes()
            {
                DemandBillNo = demandRevokeContext.DemandBillNo,
                AuditStatus = DemandAuditStatusEnum.Revoke,
                HandleTime = updateTime,
                OrganizeId = demandRevokeContext.OrganizeId
            };
            var retValue1 = new LogicByZhyProvider().DemandStatusNote(request);
            if (retValue1.HasError)
            {
                Loger.Log4Net.Error($"需求单号:{demandRevokeContext.DemandBillNo},撤销成功之后，接着通知智慧云失败！,请求参数：{JsonConvert.SerializeObject(request)}");
            }
        }

        /// <summary>
        /// 充值单回传接口,智慧云充值成功之后会给系统一个充值单号，及时判断金额是否对不对
        /// </summary>
        /// <param name="rechargeContext"></param>
        /// <returns></returns>
        public ReturnValue RechargeReceipt(RequestRechargeReceiptDto rechargeContext)
        {
            var retValue = VerifyOfNecessaryParameters<RequestRechargeReceiptDto>(rechargeContext);
            if (retValue.HasError)
                return retValue;
            var userInfo = Dal.GDT.GdtAccountInfo.Instance.VerifyOfOrganizeId(rechargeContext.OrganizeId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "10021", $"此操作人：{rechargeContext.OrganizeId} 不存在");
            }
            //todo:校验DemandBillNo
            Entities.GDT.GdtDemand entityDemand;
            retValue = VerifyOfDemandBillNo(retValue, rechargeContext.DemandBillNo, out entityDemand);
            if (retValue.HasError || entityDemand == null)
                return retValue;
            if (entityDemand.AuditStatus != DemandAuditStatusEnum.PendingPutIn
                && entityDemand.AuditStatus != DemandAuditStatusEnum.Puting)
                return CreateFailMessage(retValue, "10022", "需求没有审核通过，不能进行此操作");

            var rechargeInfoRe = Dal.GDT.GdtRechargeRelation.Instance.VerifyOfRechargeNumber(rechargeContext.DemandBillNo, rechargeContext.RechargeNumber);
            if (rechargeInfoRe != null)
            {
                return CreateFailMessage(retValue, "10025", "需求单号，充值单号重复，不满足条件");
            }
            //todo:校验金额
            if (entityDemand.TotalBudget != rechargeContext.Amount)
            {
                return CreateFailMessage(retValue, "10023", "需求单里面的总预算金额小于(不等于)当前金额，不符合操作");
            }
            var rechargeInfo = AutoMapper.Mapper.Map<RequestRechargeReceiptDto, Entities.GDT.GdtRechargeRelation>(rechargeContext);
            rechargeInfo.CreateUserId = userInfo.UserID;
            var id = Dal.GDT.GdtRechargeRelation.Instance.Insert(rechargeInfo);
            if (id <= 0)
            {
                return CreateFailMessage(retValue, "10024", "充值单回传入库失败");
            }
            //立即对账
            Task.Run(
                () => LogicByZhyProvider(rechargeInfo, organizeId: rechargeContext.OrganizeId, entityDemand: entityDemand));

            return CreateSuccessMessage(retValue, "0", "success");
        }

        private void LogicByZhyProvider(Entities.GDT.GdtRechargeRelation rechargeRelation, int organizeId,
            Entities.GDT.GdtDemand entityDemand)
        {
            var retValue = new LogicByZhyProvider().AccountFundsNote(new ToAccountFundNotes()
            {
                DemandBillNo = rechargeRelation.DemandBillNo,
                MoneyTpe = ZhyEnum.ZhyMoneyTpeEnum.现金,
                OrganizeId = organizeId,
                TradeType = ZhyEnum.ZhyTradeTypeEnum.充值,
                TradeMoney = rechargeRelation.Amount,
                TradeNo = rechargeRelation.RechargeNumber
            });
            if (retValue.HasError)
            {
                Loger.ZhyLogger.Info($" 立即校验对账错误： {retValue.Message}");
                //todo:异常充值记录
                InsertIntoAbnormalFunds(rechargeRelation, entityDemand);
            }
            else
            {
                //确认时间
                Dal.GDT.GdtRechargeRelation.Instance.UpdateConfirmTime(rechargeRelation.DemandBillNo, rechargeRelation.RechargeNumber);
            }
        }

        public ReturnValue VerifyOfUserInfo(ReturnValue retValue, string mobile)
        {
            var userInfo = Dal.GDT.GdtAccountInfo.Instance.GetUserInfo(mobile);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "10010", $"此操作人：{mobile} 不存在");
            }
            retValue.HasError = false;
            return retValue;
        }

        public ReturnValue VerifyOfDemandBillNo(ReturnValue retValue, int demandBillNo, out Entities.GDT.GdtDemand entityDemand)
        {
            entityDemand = null;
            var billInfo = Dal.GDT.GdtDemand.Instance.GetInfoByDemandBillNo(demandBillNo);
            if (billInfo == null)
            {
                return CreateFailMessage(retValue, "10031", "需求单号不存在");
            }
            entityDemand = billInfo;
            return CreateSuccessMessage(retValue, "0", "");
        }

        /// <summary>
        /// 智慧云获取小时报表数据
        /// </summary>
        /// <param name="requestReport"></param>
        /// <returns></returns>
        public Tuple<ReturnValue, List<GetToZhyReportDto>> GetToZhyReport(RequestReportByZhyDto requestReport)
        {
            var retValue = VerifyOfNecessaryParameters<RequestReportByZhyDto>(requestReport);
            if (retValue.HasError)
                return new Tuple<ReturnValue, List<GetToZhyReportDto>>(retValue, null);
            //校验DemandBillNo
            Entities.GDT.GdtDemand entityDemand;
            retValue = VerifyOfDemandBillNo(retValue, requestReport.DemandBillNo, out entityDemand);
            if (retValue.HasError)
                return new Tuple<ReturnValue, List<GetToZhyReportDto>>(retValue, null);
            //获取账户id
            //var userRelateInfo = Dal.GDT.GdtAccountRelation.Instance.GetInfo(entityDemand.CreateUserId);
            //if (userRelateInfo == null)
            //    return new Tuple<ReturnValue, List<GetToZhyReportDto>>(CreateFailMessage(retValue, "10041", $"当前需求提交人id:{entityDemand.CreateUserId}没有找到关联到广点通账户id"), null);
            var list = Dal.GDT.GdtHourlyRrportForZhy.Instance.GetHourlyRrportForZhy(new ReportQuery<GdtHourlyRrport>()
            {
                DemandBillNo = requestReport.DemandBillNo,
                Level = ReportLevelEnum.ADVERTISER,
                Date = requestReport.Date
            });
            return new Tuple<ReturnValue, List<GetToZhyReportDto>>
                (retValue, AutoMapper.Mapper.Map<List<Entities.GDT.GdtHourlyRrportForZhy>, List<GetToZhyReportDto>>(list));
        }

        public void InsertIntoAbnormalFunds(Entities.GDT.GdtRechargeRelation rechargeRelation,
            Entities.GDT.GdtDemand entityDemand)
        {
            Dal.GDT.GdtAbnormalFunds.Instance.Insert(new GdtAbnormalFunds()
            {
                DemandBillNo = rechargeRelation.DemandBillNo,
                RechargeNumber = rechargeRelation.RechargeNumber,
                TradeType = (int)TradeTypeEnum.CHARGE,
                CreateUserId = entityDemand.CreateUserId,
                RechargeAmount = rechargeRelation.Amount,
            });
        }

        #region 导入数据相关

        public ReturnValue ImportUser(string fileName)
        {
            var retValue = new ReturnValue();

            var filePath = GetImportUserPath();

            //读取文件
            var userInfos = new ExcelHelper<RespExcelUserOrganizeDto>().GetListFromFile(filePath + @"\" + fileName).ToList();

            var pushUserContexts = AutoMapper.Mapper.Map<List<RespExcelUserOrganizeDto>, List<RequestPushUserDto>>(userInfos.ToList());
            var successCount = 0;
            pushUserContexts.ForEach(s =>
            {
                try
                {
                    retValue = PushUser(s);
                    if (retValue.HasError)
                    {
                        Loger.Log4Net.Error($" ImportUser PushUser 返回错误：{JsonConvert.SerializeObject(retValue)}");
                    }
                    else
                    {
                        successCount++;
                    }
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.Error($" ImportUser is error :{exception.Message}.{exception.StackTrace ?? string.Empty}");
                }
                retValue.HasError = false;
            });
            retValue.Message = $" 导入数据完成！   成功数量：{successCount}，失败数量：{pushUserContexts.Count - successCount} ";
            return retValue;
        }

        private string GetImportUserPath()
        {
            var filePath = Path.Combine(GetSetting(), @"UploadFiles\DemandImportUser");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return filePath;
        }

        private string GetSetting(string key = "UploadFilePath")
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($"请配置相关信息，现在默认取的是【UploadFilePath】配置...{exception.Message},{exception.StackTrace ?? string.Empty}");
                //return ConfigurationManager.AppSettings["UploadFilePath"];
                throw;
            }
        }

        #endregion 导入数据相关
    }
}