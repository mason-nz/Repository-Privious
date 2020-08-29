using log4net;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class Demand
    {
        public static readonly Demand Instance = new Demand();
        ILog logger = LogManager.GetLogger(typeof(Demand));

        public GetDemandListResDTO GetDemandList(GetDemandListReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
            {//运营
                userID = ur.UserID;
            }
            else
            {//超管
                if (req.TabType == "pass")
                {
                    if (req.AuditStatus != Entities.Constants.Constant.INT_INVALID_VALUE && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Puting && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.IsOver)
                    {
                        msg = "参数错误";
                        return null;
                    }
                }
                else if (req.TabType == "wait")
                {
                    req.AuditStatus = (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingAudit;
                }
                else if (req.TabType == "reject")
                {
                    req.AuditStatus = (int)Entities.Enum.GDT.DemandAuditStatusEnum.Rejected;
                }
                else
                {
                    msg = "参数错误";
                    return null;
                }
            }
            var res = Dal.Demand.Instance.GetDemandList(req.CreateUser, req.BelongYY, req.DemandName, req.AuditStatus, userID, req.PageIndex, req.PageSize);
            return res;
        }

        public GetDemandDetailResDTO GetDemandDetail(int demandBillNo, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;

            var res = Dal.Demand.Instance.GetDemandDetail(demandBillNo, userID);
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
                #endregion
                
                //广告最后更新时间
                if (res.ADGroupList != null && res.ADGroupList.Count > 0)
                    res.Demand.ADLastUpdateTime = res.ADGroupList.Max(a=>a.PullTime);
                res.Demand.DuringDate = "（共" + ((res.Demand.EndDate.Date - res.Demand.BeginDate.Date).Days + 1) + "天）" + res.Demand.BeginDate.ToString("yyyy年M月d日") + "—" + res.Demand.EndDate.ToString("yyyy年M月d日");
                res.Demand.ClueNumberStr = res.Demand.ClueNumber + "条（约每日线索量：" + Math.Round((decimal)res.Demand.ClueNumber / ((res.Demand.EndDate.Date - res.Demand.BeginDate.Date).Days + 1), MidpointRounding.AwayFromZero) + "，线索单价：" + Math.Round(res.Demand.TotalBudget / res.Demand.ClueNumber / 100, 2) + "元）";
            }
            return res;
        }

        public bool AuditDemand(AuditDemandReqDTO req, ref string msg ,ref int nextBillNo)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return false;
            }
            if (ur.IsADYY && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated)
            {
                msg = "角色错误";
                return false;
            }
            if (req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Rejected
         && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn
         && req.AuditStatus != (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated)
            {
                msg = "审核类型错误";
                return false;
            }
            if (req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Rejected && string.IsNullOrWhiteSpace(req.Reason))
            {
                msg = "缺少审核原因";
                return false;
            }
            var demand = Dal.Demand.Instance.GetDemandEntityByBillNo(req.DemandBillNo);
            if (demand == null)
            {
                msg = "需求不存在";
                return false;
            }
            if (req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated && !Dal.Demand.Instance.CheckDemandCanStop(req.DemandBillNo))
            {
                msg = "需求下有有效的广告";
                return false;
            }
            if (req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn || req.AuditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated)
                req.Reason = string.Empty;
            var res =  Dal.Demand.Instance.AuditDemand(req.DemandBillNo, req.AuditStatus, req.Reason, ur.UserID);
            if (res)
            {
                int organizeld = Dal.Demand.Instance.GetOrganizeldByDemandBillNo(req.DemandBillNo);
                if (organizeld != 0)
                {
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
                }
                else
                {
                    logger.Error($"DemandBillNo={req.DemandBillNo}缺少Organizeld!");
                }
                nextBillNo = Dal.Demand.Instance.SelectNextWaittingAuditDemand();
            }
            else
                nextBillNo = 0;
            return res;
        }

        public List<ADGroupDTO> GetWaittingADList(int demandBillNo, ref string msg)
        {//只能从广告主关联的子客的所属广告组里面选
            var demand = Dal.Demand.Instance.GetDemandEntityByBillNo(demandBillNo);
            if (demand == null)
            {
                msg = "需求单号不存在";
                return null;
            }
            if (demand.AuditStatus != Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn && demand.AuditStatus != Entities.Enum.GDT.DemandAuditStatusEnum.Puting)
            {
                msg = "当前需求审核状态不可关联广告";
                return null;
            }
            if (!Dal.Demand.Instance.CheckDemandHasReCharge(demandBillNo))
            {
                msg = "该需求没有充值单，不可关联广告";
                return null;
            }
            if (demand.BeginDate.Date > DateTime.Now.Date || demand.EndDate.Date < DateTime.Now.Date)
            {
                msg = "暂未到需求开始日期，不可关联";
                return null;
            }
            return Dal.Demand.Instance.GetCanSelectADGroupList(demandBillNo);
        }

        public bool RelateToADGroup(AuditDemandReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return false;
            }
            var demand = Dal.Demand.Instance.GetDemandEntityByBillNo(req.DemandBillNo);
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
            if (!Dal.Demand.Instance.CheckDemandHasReCharge(req.DemandBillNo))
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
            bool res = Dal.Demand.Instance.RelateToADGroupList(req.DemandBillNo, req.ADGroupList, ur.UserID);
            if (res)
            {
                int organizeld = Dal.Demand.Instance.GetOrganizeldByDemandBillNo(req.DemandBillNo);
                if (organizeld != 0)
                {
                    Task.Run(() =>
                    {
                        LogicByZhyProvider provider = new LogicByZhyProvider();
                        provider.DemandStatusNote(new GDT.Dto.Request.ToDemandNotes
                        {
                            DemandBillNo = req.DemandBillNo,
                            Reject = "无",
                            AuditStatus = Entities.Enum.GDT.DemandAuditStatusEnum.Puting,
                            OrganizeId = organizeld
                        });
                    });
                }
                else
                {
                    logger.Error($"DemandBillNo={req.DemandBillNo}缺少Organizeld!");
                }
            }
            return res;
        }

        #region 效果图相关
        public List<MapGetLeftItemDTO> MapGetLeft(ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;
            return Dal.Demand.Instance.GetDemandADGroupMenu(userID);
        }

        public MapGetRightOneResDTO MapGetRightOne(ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;
            return Dal.Demand.Instance.GetAccountInfo(userID);
        }

        public MapGetRightTwoResDTO MapGetRightTwo(MapGetRightReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;
            return Dal.Demand.Instance.GetStatistic(req.DemandBillNo, req.ADGroupID, userID);
        }

        public List<MapGetRightThreeItemDTO> MapGetRightThree(MapGetRightReqDTO req, ref string msg) 
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;
            switch (req.DateType)
            {
                #region 时间类型验证
                case -1://昨天
                    req.BeginDate = DateTime.Now.Date.AddDays(-1);
                    req.EndDate = req.BeginDate;
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
                    req.EndDate = req.BeginDate;
                    break;
                case 7://近7天
                    req.BeginDate = DateTime.Now.Date.AddDays(-6);
                    req.EndDate = DateTime.Now.Date;
                    break;
                case 30://近30天
                    req.BeginDate = DateTime.Now.Date.AddDays(-29);
                    req.EndDate = DateTime.Now.Date;
                    break;
                default:
                    msg = "时间类型错误";
                    return null;
                    #endregion
            }
            if (!new int[] { 1, 2, 3, 4, 5, 6 }.Contains(req.DataType))
            {
                msg = "数据类型错误";
                return null;
            }
            var res = Dal.Demand.Instance.GetChart(req.DemandBillNo, req.ADGroupID, req.BeginDate, req.EndDate, req.DataType, userID);
            if (res != null && res.Count > 0 && (req.EndDate.Date - req.BeginDate.Date).Days > 2)
            {//大于两天 按天统计
                List<MapGetRightThreeItemDTO> dayRes = new List<MapGetRightThreeItemDTO>();
                res.GroupBy(i => i.Date).ToList().ForEach(gb=> 
                {
                    dayRes.Add(new MapGetRightThreeItemDTO { Key = gb.Key.ToString("yyyy-MM-dd") , Value = req.DataType == 3 ? gb.Average(i => i.Value) : gb.Sum(i => i.Value) });
                });
                return dayRes;
            }
            return res;
        }

        public MapGetRightADListResDTO MapGetRightFour(MapGetRightReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;
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
            }
            orderByStr += (req.IsDesc.Equals(1) ? "desc" : "asc");
            return Dal.Demand.Instance.GetADGroupRankList(req.DemandBillNo, userID, orderByStr, req.PageIndex, req.PageSize);
        }

        public MapGetRightADListResDTO MapGetRightFive(MapGetRightReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return null;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;
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
            return Dal.Demand.Instance.GetADGroupDetailList(req.DemandBillNo, req.ADGroupID, startDt, endDt, userID, req.PageIndex, req.PageSize);
        }

        public string MapExportToExcel(MapGetRightReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsADYY && !ur.IsAdministrator)
            {
                msg = "角色错误";
                return string.Empty;
            }
            int userID = 0;
            if (ur.IsADYY)
                userID = ur.UserID;
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
            var list = Dal.Demand.Instance.GetAllADGroupDetailList(req.DemandBillNo, req.ADGroupID, startDt, endDt, userID);
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
            string headerStr = "日期|时间段|点击量|曝光量|平均点击率|花费|订单量|话单量·";
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
                dataRow.CreateCell(1).SetCellValue($"{item.Hour-1}至{item.Hour}");
                dataRow.CreateCell(2).SetCellValue(item.TotalClick);
                dataRow.CreateCell(3).SetCellValue(item.TotalImpression);
                dataRow.CreateCell(4).SetCellValue(item.AvgClickPercent * 100 + "%");
                dataRow.CreateCell(5).SetCellValue(item.TotalCost);
                dataRow.CreateCell(6).SetCellValue(item.OrderQuantity);
                dataRow.CreateCell(7).SetCellValue(item.BillOfQuantities);
                rownumber++;
            }
            using (FileStream fs = new FileStream(backFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            return frontFilePath;
        }
        #endregion

    }
}
