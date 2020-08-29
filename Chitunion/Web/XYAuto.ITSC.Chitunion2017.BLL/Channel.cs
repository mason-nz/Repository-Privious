using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class Channel
    {
        ILog logger = LogManager.GetLogger(typeof(Channel));
        public static readonly Channel Instance = new Channel();

        #region 第一部分

        /// <summary>
        /// 新增编辑渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool ModifyChannel(ModifyChannelReqDTO dto, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (Dal.Channel.Instance.CheckChannelIsRepeat(dto)) {
                msg = "存在相同名称周期交叉的渠道!";
                return false;
            }
            int channelID = 0;
            bool flag = false;
            dto.CreateTime = DateTime.Now;
            dto.LastUpdateTime = DateTime.Now;
            dto.CreateUserID = ur.UserID;
            DateTime BeginDt = new DateTime();
            DateTime EndDt = new DateTime();
            if (!dto.ChannelID.Equals(0))
            {
                var old = Dal.Channel.Instance.GetChannelDetail(dto.ChannelID);
                if (old.CooperateBeginDate.Date != dto.CooperateBeginDate || old.CooperateEndDate != dto.CooperateEndDate)
                {
                    flag = true;
                    BeginDt = old.CooperateBeginDate;
                    EndDt = old.CooperateEndDate;
                }
            }
            var res = Dal.Channel.Instance.ModifyChannel(dto, ref channelID);
            if (res && flag)
            {
                if (( BeginDt >= dto.CooperateBeginDate && BeginDt <= dto.CooperateEndDate) 
                  || (EndDt >= dto.CooperateBeginDate && EndDt <= dto.CooperateEndDate)
                  || (dto.CooperateBeginDate >= BeginDt && dto.CooperateBeginDate <= EndDt)
                  || (dto.CooperateEndDate >= BeginDt && dto.CooperateEndDate <= EndDt))
                {//如果修改后的有效期和修改前的有效期有交集 取最小和最大时间 重新计算
                    BeginDt = (new List<DateTime>() { BeginDt, EndDt, dto.CooperateBeginDate, dto.CooperateEndDate }).Min();
                    EndDt = (new List<DateTime>() { BeginDt, EndDt, dto.CooperateBeginDate, dto.CooperateEndDate }).Max();
                    Task.Factory.StartNew(() => ComputePriceByChannel(dto.ChannelID, BeginDt, EndDt));
                }
                else
                {//没有交集 修改前后的两个时间段都要重新计算
                    Task.Factory.StartNew(() => ComputePriceByChannel(dto.ChannelID,BeginDt, EndDt));
                    Task.Factory.StartNew(() => ComputePriceByChannel(channelID));
                }
            }
            return res;
        }

        /// <summary>
        /// 删除渠道
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool DeleteChannel(int channelID, ref string msg)
        {
            if (Dal.Channel.Instance.SelectMediaCountOfChannel(channelID) > 0)
            {
                msg = "渠道下有媒体，不能删除";
                return false;
            }
            var res = Dal.Channel.Instance.DeleteChannel(channelID);
            return res;
        }

        /// <summary>
        /// 获取渠道列表
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="status">0正常 1过期 -2全部(不包括删除)</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GetChannelListResDTO GetChannelList(GetChannelListReqDTO req)
        {
            return Dal.Channel.Instance.GetChannelList(req.ChannelName, req.Status, req.PageIndex, req.PageSize);
        }

        /// <summary>
        /// 获取渠道详情
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public GetChannelInfoResDTO GetChannelDetail(int channelID)
        {
            return Dal.Channel.Instance.GetChannelDetail(channelID);
        }

        #endregion

        #region 第二部分
        public bool DeleteCost(int costID)
        {
            var res =  Dal.Channel.Instance.DeleteCost(costID);
            if (res)
            {
                Task.Factory.StartNew(() => ComputePriceByCost(costID));
            }
            return res;
        }

        public bool BatchCostOperate(BatchCostOperateReqDTO req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (req.CostIDList == null || req.CostIDList.Count.Equals(0))
            {
                msg = "没有任何成本ID!";
                return false;
            }
            else if (!Enum.IsDefined(typeof(MediaPublishStatusEnum), req.OpType))
            {
                msg = "操作类型错误!";
                return false;
            }

            var res = Dal.Channel.Instance.BatchCostOperate(req.CostIDList, req.OpType);
            if (res)
            {
                Task.Factory.StartNew(() => ComputePriceByCost(req.CostIDList.ToArray()));
            }
            return res;
        }

        public GetCostMediaListResDTO GetCostMediaList(string mediaName)
        {
            return Dal.Channel.Instance.GetCostMediaList(mediaName);
        } 

        public GetCostChannelListResDTO GetCostChannelList()
        {
            return Dal.Channel.Instance.GetCostChannelList();
        }

        public GetCostListResDTO GetCostList(GetCostListReqDTO req)
        {
            return Dal.Channel.Instance.GetCostList(req.MediaID, req.ChannelID, req.SaleStatus, req.PageIndex, req.PageSize);
        }

        public GetCostDetailResDTO GetCostDetail(int costID)
        {
            return Dal.Channel.Instance.GetCostDetail(costID);
        }

        //上传资源、预览、提交 限定只能是AE操作

        /// <summary>
        /// 上传渠道资源
        /// </summary>
        /// <param name="req"></param>
        /// <param name="msg"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public UploadChannelResourceResDTO UploadChannelResource(UploadChannelResourceReqDTO req, ref string msg)
        {
            try
            {
                var ur = Common.UserInfo.GetUserRole();
                if (!ur.IsAE)
                {
                    msg = "角色错误";
                    return null;
                }
                UploadChannelResourceResDTO res = new UploadChannelResourceResDTO();
                List<string> errorList = new List<string>();
                string filePath = WebConfigurationManager.AppSettings["UploadFilePath"] + req.FilePath;
                string suffix = FileHelper.GetSuffix(filePath);
                var channel = Dal.Channel.Instance.GetChannelDetail(req.ChannelID);
                decimal taxScale = 1;
                if (!channel.IncludingTax)
                    taxScale = decimal.Parse(WebConfigurationManager.AppSettings["TaxScale"]);

                #region 基础验证
                if (channel == null)
                {
                    msg = "渠道不存在";
                    return null;
                }
                if (suffix.ToLower() != ".xls" && suffix.ToLower() != ".xlsx")
                {
                    msg = "文件格式要求xls或xlsx";
                    return null;
                }
                if (!File.Exists(filePath))
                {
                    msg = "文件不存在";
                    return null;
                }
                DataTable dt = ConvertExcelToDataTable(filePath);
                if (dt == null || dt.Rows.Count.Equals(0))
                {
                    msg = "没有任何数据";
                    return null;
                }
                List<string> excelColumnList = new List<string>();
                excelColumnList.Add("微信账号");
                excelColumnList.Add("微信名称");
                excelColumnList.Add("粉丝数");
                excelColumnList.Add("分类");
                excelColumnList.Add("地域");
                excelColumnList.Add("媒体级别");
                excelColumnList.Add("下单备注");
                excelColumnList.Add("原创价格");
                excelColumnList.Add("单图文软广发布");
                excelColumnList.Add("单图文软广原创+发布");
                excelColumnList.Add("单图文贴片广告");
                excelColumnList.Add("多图文头条软广发布");
                excelColumnList.Add("多图文头条软广原创+发布");
                excelColumnList.Add("多图文头条贴片广告");
                excelColumnList.Add("多图文第二条软广发布");
                excelColumnList.Add("多图文第二条软广原创+发布");
                excelColumnList.Add("多图文第二条贴片广告");
                excelColumnList.Add("多图文第三到N条软广发布");
                excelColumnList.Add("多图文第三到N条软广原创+发布");
                excelColumnList.Add("多图文第三到N条贴片广告");
                excelColumnList.Add("单图文软广发布售卖价");
                excelColumnList.Add("单图文软广原创+发布售卖价");
                excelColumnList.Add("单图文贴片广告售卖价");
                excelColumnList.Add("多图文头条软广发布售卖价");
                excelColumnList.Add("多图文头条软广原创+发布售卖价");
                excelColumnList.Add("多图文头条贴片广告售卖价");
                excelColumnList.Add("多图文第二条软广发布售卖价");
                excelColumnList.Add("多图文第二条软广原创+发布售卖价");
                excelColumnList.Add("多图文第二条贴片广告售卖价");
                excelColumnList.Add("多图文第三到N条软广发布售卖价");
                excelColumnList.Add("多图文第三到N条软广原创+发布售卖价");
                excelColumnList.Add("多图文第三到N条贴片广告售卖价");
                if (dt.Columns.Count != 32)
                {
                    msg = "Excel应总共有32列，媒体信息7列，价格信息25列";
                    return null;
                }
                foreach(DataColumn dc in dt.Columns)
                {
                    if (!excelColumnList.Exists(i => i.Equals(dc.ColumnName)))
                    {
                        msg = "列名:" + dc.ColumnName + " 错误";
                        return null;
                    }
                }
                #endregion

                #region 初始化参数
                StringBuilder sb = new StringBuilder();
                int fansCount = 0;
                var categoryDict = Dal.Channel.Instance.GetDictNames(47);//常见分类
                var areaRemarkDict = Dal.Channel.Instance.GetDictNames(60);//区域媒体的下单备注
                var remarkDict = Dal.Channel.Instance.GetDictNames(40);//非区域媒体的下单备注
                var mediaLevelDict = Dal.Channel.Instance.GetDictNames(4);
                Dictionary<string, List<int>> adPriceDict = new Dictionary<string, List<int>>();
                #region 16个广告位 32个价格  ConvertExcelToDataTable已经在售卖价前面加了广告位名称
                adPriceDict.Add("单图文软广发布", new List<int>() { (int)ADPosition1Enum.单图文, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("单图文软广发布售卖价", new List<int>() { (int)ADPosition1Enum.单图文, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("单图文软广原创+发布", new List<int>() { (int)ADPosition1Enum.单图文, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("单图文软广原创+发布售卖价", new List<int>() { (int)ADPosition1Enum.单图文, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("单图文贴片广告", new List<int>() { (int)ADPosition1Enum.单图文, (int)ADPosition3Enum.贴片广告, 0 });
                adPriceDict.Add("单图文贴片广告售卖价", new List<int>() { (int)ADPosition1Enum.单图文, (int)ADPosition3Enum.贴片广告, 0 });
                adPriceDict.Add("多图文头条软广发布", new List<int>() { (int)ADPosition1Enum.多图文头条, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("多图文头条软广发布售卖价", new List<int>() { (int)ADPosition1Enum.多图文头条, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("多图文头条软广原创+发布", new List<int>() { (int)ADPosition1Enum.多图文头条, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("多图文头条软广原创+发布售卖价", new List<int>() { (int)ADPosition1Enum.多图文头条, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("多图文头条贴片广告", new List<int>() { (int)ADPosition1Enum.多图文头条, (int)ADPosition3Enum.贴片广告, 0 });
                adPriceDict.Add("多图文头条贴片广告售卖价", new List<int>() { (int)ADPosition1Enum.多图文头条, (int)ADPosition3Enum.贴片广告, 0 });
                adPriceDict.Add("多图文第二条软广发布", new List<int>() { (int)ADPosition1Enum.多图文第二条, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("多图文第二条软广发布售卖价", new List<int>() { (int)ADPosition1Enum.多图文第二条, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("多图文第二条软广原创+发布", new List<int>() { (int)ADPosition1Enum.多图文第二条, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("多图文第二条软广原创+发布售卖价", new List<int>() { (int)ADPosition1Enum.多图文第二条, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("多图文第二条贴片广告", new List<int>() { (int)ADPosition1Enum.多图文第二条, (int)ADPosition3Enum.贴片广告, 0 });
                adPriceDict.Add("多图文第二条贴片广告售卖价", new List<int>() { (int)ADPosition1Enum.多图文第二条, (int)ADPosition3Enum.贴片广告, 0 });
                adPriceDict.Add("多图文第三到N条软广发布", new List<int>() { (int)ADPosition1Enum.多图文3一N条, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("多图文第三到N条软广发布售卖价", new List<int>() { (int)ADPosition1Enum.多图文3一N条, (int)ADPosition3Enum.直发, 0 });
                adPriceDict.Add("多图文第三到N条软广原创+发布", new List<int>() { (int)ADPosition1Enum.多图文3一N条, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("多图文第三到N条软广原创+发布售卖价", new List<int>() { (int)ADPosition1Enum.多图文3一N条, (int)ADPosition3Enum.原创十发布, 0 });
                adPriceDict.Add("多图文第三到N条贴片广告", new List<int>() { (int)ADPosition1Enum.多图文3一N条, (int)ADPosition3Enum.贴片广告, 0 });
                adPriceDict.Add("多图文第三到N条贴片广告售卖价", new List<int>() { (int)ADPosition1Enum.多图文3一N条, (int)ADPosition3Enum.贴片广告, 0 });
                #endregion

                CostExcelDTO excelDTO = new CostExcelDTO();
                excelDTO.ChannelID = channel.ChannelID;
                excelDTO.ChannelName = channel.ChannelName;
                excelDTO.CooperateBeginDate = channel.CooperateBeginDate;
                excelDTO.CooperateEndDate = channel.CooperateEndDate;
                #endregion

                foreach (DataRow row in dt.Rows)
                {
                    int mediaLevelId = 0;
                    int originalPrice = 0;
                    int tempPrice = 0;//临时存价格
                    #region 遍历验证
                    Tuple<int, int> areaTp = new Tuple<int, int>(-2, -2);
                    Dictionary<int, string> selCategoryIdDict = new Dictionary<int, string>();
                    Dictionary<int, string> selRemarkIdDict = new Dictionary<int, string>();
                    ImportCostRow rowDTO = new ImportCostRow();
                    sb = new StringBuilder();
                    sb.Append("第" + (dt.Rows.IndexOf(row) + 2) + "行");
                    if (string.IsNullOrWhiteSpace(row.Field<string>("微信账号")))
                        sb.Append("微信账号为空、");
                    else if (excelDTO.ImportRowList.Exists(r => r.WxNumber == (row.Field<string>("微信账号"))))
                        sb.Append("微信账号重复、");
                    else
                        rowDTO.WxNumber = row.Field<string>("微信账号");

                    if (string.IsNullOrWhiteSpace(row.Field<string>("微信名称")))
                        sb.Append("微信名称为空、");
                    //else if (excelDTO.ImportRowList.Exists(r => r.WxName == row.Field<string>("微信名称")))
                    //sb.Append("微信名称重复、");
                    else
                        rowDTO.WxName = row.Field<string>("微信名称");

                    if (string.IsNullOrWhiteSpace(row.Field<string>("粉丝数")))
                        sb.Append("粉丝数为空、");
                    else if (!int.TryParse(row.Field<string>("粉丝数"), out fansCount))
                        sb.Append("粉丝数格式错误、");
                    else if (fansCount < 500)
                        sb.Append("粉丝数不能小于500、");
                    else
                        rowDTO.FansCount = fansCount;

                    msg = CheckExcelCategory(row.Field<string>("分类"), categoryDict, ref selCategoryIdDict);
                    if (msg.Length > 0)
                        sb.Append(msg);
                    else
                        rowDTO.CategoryDict = selCategoryIdDict;

                    msg = CheckExcelArea(row.Field<string>("地域"), ref areaTp);
                    if (msg.Length > 0)
                        sb.Append(msg);
                    else
                    {
                        if (areaTp.Item2 != Entities.Constants.Constant.INT_INVALID_VALUE)
                        {
                            rowDTO.ProvinceName = row.Field<string>("地域").Split(' ')[0];
                            rowDTO.CityName = row.Field<string>("地域").Split(' ')[1];
                        }
                        else if(!string.IsNullOrWhiteSpace(row.Field<string>("地域")))
                        {
                            rowDTO.ProvinceName = row.Field<string>("地域").Split(' ')[0];
                        }
                        rowDTO.ProvinceID = areaTp.Item1;
                        rowDTO.CityID = areaTp.Item2;
                    }

                    msg = CheckExcelMediaLevel(row.Field<string>("媒体级别"), mediaLevelDict, ref mediaLevelId);
                    if (msg.Length > 0)
                        sb.Append(msg);
                    else
                    {
                        rowDTO.LevelType = mediaLevelId;
                        rowDTO.LevelTypeName = row.Field<string>("媒体级别");
                    }


                    msg = CheckExcelRemark(row.Field<string>("下单备注"), string.IsNullOrWhiteSpace(row.Field<string>("地域")) ? remarkDict : areaRemarkDict, ref selRemarkIdDict);
                    if (msg.Length > 0)
                        sb.Append(msg);
                    else
                        rowDTO.RemarkDict = selRemarkIdDict;

                    if (!string.IsNullOrEmpty(row.Field<string>("原创价格")) && !int.TryParse(row.Field<string>("原创价格"), out originalPrice))
                        sb.Append("原创价格格式错误、");
                    else
                        rowDTO.OriginalPrice = originalPrice;

                    #region 广告位价格
                    List<string> keys = adPriceDict.Keys.ToList();
                    foreach (var key in keys)
                    {
                        if (!string.IsNullOrWhiteSpace(row.Field<string>(key)))
                        {
                            if (!int.TryParse(row.Field<string>(key), out tempPrice))
                            {
                                sb.Append(key + "格式错误、");
                            }
                            else if (tempPrice < 0)
                            {
                                sb.Append(key + "格式错误、");
                            }
                            else if (key.Contains("售卖价") && tempPrice > 0 && adPriceDict[keys[keys.IndexOf(key) - 1]][2] == 0)
                            {//没有成本价 有售卖价
                                sb.Append("缺少" + keys[keys.IndexOf(key) - 1] + "成本价、");
                            }
                            else
                            {
                                #region 价格不为空
                                adPriceDict[key][2] = (int)(tempPrice * taxScale);
                                var curPrice = rowDTO.PriceList.FirstOrDefault(p => p.ADPosition1.Equals(adPriceDict[key][0]) && p.ADPosition3.Equals(adPriceDict[key][1]));
                                if (curPrice == null)
                                {
                                    if (key.Contains("售卖价"))
                                        rowDTO.PriceList.Add(new ImportCostPrice() { ADPosition1 = adPriceDict[key][0], ADPosition2 = 7002, ADPosition3 = adPriceDict[key][1], SalePrice = adPriceDict[key][2] });
                                    else
                                        rowDTO.PriceList.Add(new ImportCostPrice() { ADPosition1 = adPriceDict[key][0], ADPosition2 = 7002, ADPosition3 = adPriceDict[key][1], CostPrice = adPriceDict[key][2] });
                                }
                                else
                                {
                                    if (key.Contains("售卖价"))
                                    {
                                        curPrice.SalePrice = adPriceDict[key][2];
                                    }
                                    else
                                    {
                                        curPrice.CostPrice = adPriceDict[key][2];
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            if (key.Contains("售卖价") && adPriceDict[keys[keys.IndexOf(key) - 1]][2] != 0)
                            {//没有售卖价 有成本价
                                sb.Append("缺少" + keys[keys.IndexOf(key) - 1] + "售卖价、");
                            }
                            else
                            {
                                #region 价格为0不入库
                                adPriceDict[key][2] = 0;
                                //var curPrice = rowDTO.PriceList.FirstOrDefault(p => p.ADPosition1.Equals(adPriceDict[key][0]) && p.ADPosition3.Equals(adPriceDict[key][1]));
                                //if (curPrice == null)
                                //{
                                //    if (key.Contains("售卖价"))
                                //        rowDTO.PriceList.Add(new ImportCostPrice() { ADPosition1 = adPriceDict[key][0], ADPosition2 = 7002, ADPosition3 = adPriceDict[key][1], SalePrice = adPriceDict[key][2] });
                                //    else
                                //        rowDTO.PriceList.Add(new ImportCostPrice() { ADPosition1 = adPriceDict[key][0], ADPosition2 = 7002, ADPosition3 = adPriceDict[key][1], CostPrice = adPriceDict[key][2] });
                                //}
                                //else
                                //{
                                //    if (key.Contains("售卖价"))
                                //    {
                                //        curPrice.SalePrice = adPriceDict[key][2];
                                //    }
                                //    else
                                //    {
                                //        curPrice.CostPrice = adPriceDict[key][2];
                                //    }
                                //}
                                #endregion
                            }
                        }
                    }
                    if (adPriceDict.Values.Count(v => v[2] > 0).Equals(0))
                    {
                        sb.Append("缺少价格、");
                    }
                    #endregion

                    if (!sb.ToString().EndsWith("行"))
                    {
                        errorList.Add(sb.ToString().TrimEnd('、'));
                    }
                    #endregion
                    excelDTO.ImportRowList.Add(rowDTO);
                }

                msg = string.Empty;
                if (!errorList.Count.Equals(0))
                {
                    res.ErrorList = errorList;
                    return res;
                }
                else
                {
                    UploadSuccessRes successRes = new UploadSuccessRes();
                    OldDataDTO oldData = Dal.Channel.Instance.GetAEMediaInfo(req.ChannelID);
                    foreach (var row in excelDTO.ImportRowList)
                    {
                        #region 媒体信息
                        if (!oldData.MediaList.Exists(m => m.Number.Equals(row.WxNumber)))
                        {//新增
                            successRes.NewMedias.List.Add(CreateMediaItem(row, false));
                        }
                        else
                        {
                            #region 修改或不变
                            var oldMedia = oldData.MediaList.Find(m => m.Number.Equals(row.WxNumber));
                            var oldCategoryList = oldData.CategoryList.Where(c => c.MediaID.Equals(oldMedia.MediaID)).OrderBy(c => c.CategoryID).ToList();
                            var oldRemarkList = oldData.RemarkList.Where(r => r.RelationID.Equals(oldMedia.MediaID)).OrderBy(r => r.RemarkID).ToList();
                            var oldArea = oldData.AreaList.Where(a => a.MediaID.Equals(oldMedia.MediaID)).ToList().FirstOrDefault();
                            if (row.WxName.Equals(oldMedia.Name) && row.FansCount.Equals(oldMedia.FansCount) && row.LevelType.Equals(oldMedia.LevelType)
                                && ((oldArea == null && row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE) && row.CityID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE)) || (oldArea != null && row.ProvinceID.Equals(oldArea.ProvinceID) && row.CityID.Equals(oldArea.CityID)))
                                && oldCategoryList.Count == row.CategoryDict.Count
                                && ((from c in oldCategoryList select c.CategoryID).ToList().All(row.CategoryDict.Keys.ToList().Contains))
                                && oldRemarkList.Count == row.RemarkDict.Count
                                && ((from r in oldRemarkList select r.RemarkID).ToList().All(row.RemarkDict.Keys.ToList().Contains))
                                )
                            {//不变
                                row.MediaID = oldMedia.MediaID;
                                row.MediaIsOld = true;
                                successRes.OldMedias.List.Add(CreateMediaItem(row, false));
                            }
                            else
                            {//修改
                                row.MediaID = oldMedia.MediaID;
                                successRes.UpdateMedias.List.Add(CreateMediaItem(row, true, oldMedia, oldCategoryList, oldRemarkList));
                            }
                            #endregion
                        }
                        #endregion

                        #region 价格信息
                        if (row.MediaID.Equals(0) || !oldData.CostList.Exists(c => c.MediaID.Equals(row.MediaID)))
                        {
                            #region 没有媒体 或 有媒体没有成本 新增成本、成本广告位
                            successRes.NewPrice.List.AddRange(CreatePriceList(row));
                            if(row.OriginalPrice != 0)
                                successRes.NewPrice.List.Add(new UploadSuccessPriceItem() { WxName = row.WxName, WxNumber = row.WxNumber, ADPosition = "原创", CostPrice = row.OriginalPrice });
                            #endregion
                        }
                        else
                        {
                            #region 有媒体有成本 更新成本 广告位通过比对 删除没有的
                            var oldCost = oldData.CostList.FirstOrDefault(c => c.MediaID.Equals(row.MediaID));
                            var oldPriceList = oldData.PriceList.Where(p => p.MediaID.Equals(row.MediaID)).ToList();
                            row.CostID = oldCost.CostID;
                            foreach (var newPrice in row.PriceList)
                            {
                                #region 根据维度判断
                                var oldPrice = oldPriceList.FirstOrDefault(o => o.ADPosition1.Equals(newPrice.ADPosition1) && o.ADPosition2.Equals(newPrice.ADPosition2) && o.ADPosition3.Equals(newPrice.ADPosition3));
                                if (oldPrice == null)
                                {//新增
                                    successRes.NewPrice.List.Add(new UploadSuccessPriceItem()
                                    {
                                        WxNumber = row.WxNumber,
                                        WxName = row.WxName,
                                        ADPosition = Enum.GetName(typeof(ADPosition1Enum), newPrice.ADPosition1).Replace("一", "-") + Enum.GetName(typeof(ADPosition3Enum), newPrice.ADPosition3).Replace("十", "+"),
                                        CostPrice = newPrice.CostPrice,
                                        SalePrice = newPrice.SalePrice
                                    });
                                }
                                else if (newPrice.SalePrice != oldPrice.SalePrice || newPrice.CostPrice != oldPrice.CostPrice)
                                {//更新
                                    newPrice.DetailID = oldPrice.DetailID;
                                    successRes.UpdatePrice.List.Add(new UploadSuccessPriceItem()
                                    {
                                        WxNumber = row.WxNumber,
                                        WxName = row.WxName,
                                        ADPosition = Enum.GetName(typeof(ADPosition1Enum), newPrice.ADPosition1).Replace("一", "-") + Enum.GetName(typeof(ADPosition3Enum), newPrice.ADPosition3).Replace("十", "+"),
                                        CostPrice = newPrice.CostPrice,
                                        SalePrice = newPrice.SalePrice,
                                        OldCostPrice = oldPrice.CostPrice,
                                        OldSalePrice = oldPrice.SalePrice
                                    });
                                }
                                else
                                {//不变
                                    newPrice.DetailID = oldPrice.DetailID;
                                    successRes.OldPrice.List.Add(new UploadSuccessPriceItem()
                                    {
                                        WxNumber = row.WxNumber,
                                        WxName = row.WxName,
                                        ADPosition = Enum.GetName(typeof(ADPosition1Enum), newPrice.ADPosition1).Replace("一", "-") + Enum.GetName(typeof(ADPosition3Enum), newPrice.ADPosition3).Replace("十", "+"),
                                        CostPrice = newPrice.CostPrice,
                                        SalePrice = newPrice.SalePrice
                                    });
                                }
                                newPrice.ADPosition = Enum.GetName(typeof(ADPosition1Enum), newPrice.ADPosition1).Replace("一", "-") + Enum.GetName(typeof(ADPosition3Enum), newPrice.ADPosition3).Replace("十", "+");
                                #endregion
                            }
                            if (row.OriginalPrice != oldCost.OriginalPrice)
                                successRes.UpdatePrice.List.Add(new UploadSuccessPriceItem() { WxName = row.WxName, WxNumber = row.WxNumber, ADPosition = "原创", CostPrice = row.OriginalPrice, OldCostPrice = oldCost.OriginalPrice });
                            else if (row.OriginalPrice != 0)
                                successRes.OldPrice.List.Add(new UploadSuccessPriceItem() { WxName = row.WxName, WxNumber = row.WxNumber, ADPosition = "原创", CostPrice = row.OriginalPrice });
                            row.DeleteDetailIDs.AddRange((from d in oldPriceList.Where(o => !row.PriceList.Exists(n => n.ADPosition1.Equals(o.ADPosition1) && n.ADPosition2.Equals(o.ADPosition2) && n.ADPosition3.Equals(o.ADPosition3))) select d.DetailID).ToList());
                            #endregion
                        }
                        #endregion
                    }

                    System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                    objCache.Insert(ur.UserID + "UploadResourceExcel", excelDTO, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                    successRes.NewMedias.Total = successRes.NewMedias.List.Count;
                    successRes.OldMedias.Total = successRes.OldMedias.List.Count;
                    successRes.UpdateMedias.Total = successRes.UpdateMedias.List.Count;
                    successRes.NewPrice.Total = successRes.NewPrice.List.Count;
                    successRes.OldPrice.Total = successRes.OldPrice.List.Count;
                    successRes.UpdatePrice.Total = successRes.UpdatePrice.List.Count;
                    successRes.TotalCount = successRes.NewMedias.Total + successRes.UpdateMedias.Total + successRes.OldMedias.Total;
                    res.Data = successRes;
                    return res;
                }
            }
            catch (Exception ex)
            {
                logger.Error("UpdateChannelResource异常", ex);
                msg = "上传错误";
                return null;
            }
        }

        #region 上传资源相关

        /// <summary>  
        /// 读取Excel文件到DataSet中  
        /// 已经去掉空格
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        private DataTable ConvertExcelToDataTable(string excelPath)
        {
            DataTable dt = new DataTable();
            IWorkbook workbook;
            using (FileStream file = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = WorkbookFactory.Create(file);
            }
            #region 位置0 Sheet
            ISheet sheet = workbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            string columnName = string.Empty;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                columnName = cell == null ? string.Empty : cell.ToString().Trim();
                if (columnName == "售卖价" && j > 0)
                    columnName = headerRow.GetCell(j - 1).ToString().Trim() + "售卖价";
                dt.Columns.Add(columnName);
            }
            for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
            {
                IRow row = sheet.GetRow(j);
                DataRow dataRow = dt.NewRow();

                for (int k = row.FirstCellNum; k < cellCount; k++)
                {
                    if (row.GetCell(k) != null)
                        dataRow[k] = row.GetCell(k).ToString().Trim();
                }
                dt.Rows.Add(dataRow);
            }
            dt.TableName = sheet.SheetName;
            #endregion
            return dt;
        }

        /// <summary>
        /// 检查Excel地域
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        private string CheckExcelArea(string cellValue, ref Tuple<int, int> tp)
        {
            string msg = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    var arr = cellValue.Split(' ');
                    string provinceName = string.Empty;
                    string cityName = string.Empty;
                    if (arr.Count() == 1)
                    {
                        provinceName = cellValue;
                    }
                    else if (arr.Count() == 2)
                    {
                        provinceName = arr[0];
                        cityName = arr[1];
                    }
                    else
                    {
                        msg = "地域格式错误、";
                        return msg;
                    }
                    tp = Dal.Channel.Instance.GetAreaNames(provinceName, cityName);
                    if (tp.Item2.Equals(-99))
                        msg = "地域不匹配、";
                }
            }
            catch
            {
                msg = "地域错误、";
            }
            return msg;
        }

        /// <summary>
        /// 检查Excel分类
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="categoryNameDict"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        private string CheckExcelCategory(string cellValue, Dictionary<int, string> categoryDict, ref Dictionary<int, string> selCategoryDict)
        {
            string msg = "";
            try
            {
                if (string.IsNullOrWhiteSpace(cellValue))
                {
                    msg = "分类为空、";
                }
                if (cellValue.Split(' ').Length > 5)
                {
                    msg = "分类不能超过5个、";
                }
                foreach (string categoryName in cellValue.Split(' '))
                {
                    if (!categoryDict.Values.Contains(categoryName))
                    {
                        msg = "分类不匹配、";
                        break;
                    }
                    var one = categoryDict.First(i => i.Value.Equals(categoryName));
                    if (selCategoryDict.Keys.Contains(one.Key))
                    {
                        msg = "分类重复、";
                        break;
                    }
                    else
                    {
                        selCategoryDict.Add(one.Key, one.Value);
                    }
                }
            }
            catch
            {
                msg = "分类错误、";
            }
            return msg;
        }

        /// <summary>
        /// 检查Excel媒体级别
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="mediaLevelNameDict"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CheckExcelMediaLevel(string cellValue, Dictionary<int, string> mediaLevelNameDict, ref int id)
        {
            string msg = "";
            try
            {
                if (string.IsNullOrWhiteSpace(cellValue))
                {
                    id = 4002;//默认4002普通
                    cellValue = "普通";
                }
                if (!mediaLevelNameDict.Values.Contains(cellValue))
                {
                    msg = "媒体级别不匹配、";
                }
                else
                {
                    id = mediaLevelNameDict.First(i => i.Value.Equals(cellValue)).Key;
                }
            }
            catch
            {
                msg = "媒体级别错误、";
            }
            return msg;
        }

        /// <summary>
        /// 检查Excel订单备注
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="remarkNameDict"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        private string CheckExcelRemark(string cellValue, Dictionary<int, string> remarkNameDict, ref Dictionary<int, string> selRemarkDict)
        {
            string msg = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    foreach (string remarkName in cellValue.Split(' '))
                    {
                        if (!remarkNameDict.Values.Contains(remarkName))
                        {
                            msg = "下单备注不匹配、";
                            break;
                        }
                        var one = remarkNameDict.First(i => i.Value.Equals(remarkName));
                        if (selRemarkDict.Keys.Contains(one.Key))
                        {
                            msg = "下单备注重复、";
                            break;
                        }
                        else
                        {
                            selRemarkDict.Add(one.Key, one.Value);
                        }
                    }
                }
            }
            catch
            {
                msg = "下单备注错误、";
            }
            return msg;
        }

        private UploadSuccessMediaItem CreateMediaItem(ImportCostRow row, bool hasOld,
            Entities.Media.MediaWeixin oldMedia = null, List<Entities.Media.MediaCommonlyClass> oldCategoryList = null, List<Entities.Publish.PublishRemark> oldRemarkList = null)
        {
            var item = new UploadSuccessMediaItem();
            item.WxNumber = row.WxNumber;
            item.WxName = row.WxName;
            item.FansCount = row.FansCount;
            item.LevelTypeName = row.LevelTypeName;
            item.AreaName = string.IsNullOrEmpty(row.ProvinceName) ? "" : row.ProvinceName;
            if (item.AreaName.Length > 0 && !string.IsNullOrEmpty(row.CityName))
                item.AreaName += "-";
            item.AreaName += row.CityName;
            item.CategoryName = string.Join("、", row.CategoryDict.Values.ToArray());
            item.OrderRemarkName = string.Join("、", row.RemarkDict.Values.ToArray());
            if (hasOld && oldMedia != null && oldCategoryList != null && oldRemarkList != null)
            {
                item.OldWxName = oldMedia.Name;
                item.OldFansCount = oldMedia.FansCount;
                item.OldLevelTypeName = oldMedia.LevelTypeName;
                item.OldAreaName = string.IsNullOrEmpty(oldMedia.ProvinceName) ? "" : oldMedia.ProvinceName;
                if (item.OldAreaName.Length > 0 && !string.IsNullOrEmpty(oldMedia.CityName))
                    item.OldAreaName += "-";
                item.OldAreaName += oldMedia.CityName;
                item.OldCategoryName = string.Join("、", (from c in oldCategoryList select c.CategoryName).ToArray());
                item.OldOrderRemarkName = string.Join("、", (from r in oldRemarkList select r.RemarkName).ToArray());
            }
            return item;
        }

        private List<UploadSuccessPriceItem> CreatePriceList(ImportCostRow row)
        {
            List<UploadSuccessPriceItem> list = new List<UploadSuccessPriceItem>();
            foreach (var p in row.PriceList)
            {
                list.Add(new UploadSuccessPriceItem()
                {
                    WxNumber = row.WxNumber,
                    WxName = row.WxName,
                    ADPosition = Enum.GetName(typeof(ADPosition1Enum), p.ADPosition1).Replace("一", "-") + Enum.GetName(typeof(ADPosition3Enum), p.ADPosition3).Replace("十", "+"),
                    CostPrice = p.CostPrice,
                    SalePrice = p.SalePrice
                });
                p.ADPosition = Enum.GetName(typeof(ADPosition1Enum), p.ADPosition1).Replace("一", "-") + Enum.GetName(typeof(ADPosition3Enum), p.ADPosition3).Replace("十", "+");
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 预览上传
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public GetUploadPreviewResDTO GetUploadPreview(ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsAE)
            {
                msg = "角色错误";
                return null;
            }
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            var excelDTO = objCache.Get(ur.UserID + "UploadResourceExcel") as CostExcelDTO;
            if (excelDTO == null)
            {
                msg = "没有上传过Excel，或距上次上传超过30分钟，请重新上传";
                return null;
            }
            else
            {
                try
                {
                    GetUploadPreviewResDTO res = new GetUploadPreviewResDTO();
                    //新增mediaID为0 自然查不出来
                    string mediaRange = string.Join(",", (from m in excelDTO.ImportRowList select m.MediaID).Distinct().ToArray());
                    List<string> adPositionList = new List<string>();
                    excelDTO.ImportRowList.ForEach(r =>
                    {
                        adPositionList = adPositionList.Union((from p in r.PriceList select p.ADPosition1.ToString() + p.ADPosition2.ToString() + p.ADPosition3.ToString()).Distinct().ToList()).ToList();
                    });
                    string adPositionRange = "'" + string.Join("','", adPositionList.ToArray()) + "'";
                    //导入所有涉及媒体以及所有涉及广告位
                    var allDetailList = Dal.Channel.Instance.PreviewGetCostDetailList(excelDTO.ChannelID, mediaRange, excelDTO.CooperateBeginDate, excelDTO.CooperateEndDate, adPositionRange);
                    List<int> channelList = new List<int>();//渠道
                    channelList.Add(excelDTO.ChannelID);
                    foreach (var row in excelDTO.ImportRowList)
                    {
                        #region 媒体为维度遍历
                        if (!row.MediaID.Equals(0))
                        {
                            #region 原来有媒体
                            //旧数据+导入数据
                            var oldDetailList = allDetailList.Where(d => d.MediaID == row.MediaID).ToList();
                            var curDetailList = (from d in row.PriceList
                                                    select new Entities.Channel.ChannelCostDetail()
                                                    {
                                                        ADPosition1 = d.ADPosition1,
                                                        ADPosition2 = d.ADPosition2,
                                                        ADPosition3 = d.ADPosition3,
                                                        CostPrice = d.CostPrice,
                                                        SalePrice = d.SalePrice,
                                                        OriginalPrice = row.OriginalPrice,
                                                        CooperateBeginDate = excelDTO.CooperateBeginDate,
                                                        CooperateEndDate = excelDTO.CooperateEndDate,
                                                        ChannelID = excelDTO.ChannelID,
                                                        ADPosition = d.ADPosition,
                                                        MediaID = row.MediaID
                                                    }).ToList();
                            curDetailList.AddRange(oldDetailList);
                            curDetailList.ForEach(d => 
                            {
                                if (!channelList.Exists(c => c.Equals(d.ChannelID)))
                                {
                                    channelList.Add(d.ChannelID);
                                }
                            });
                            Queue<OneDayPubDTO> oneDayPubQ = new Queue<OneDayPubDTO>();
                            for (DateTime curDate = excelDTO.CooperateBeginDate; curDate < excelDTO.CooperateEndDate.AddDays(1); curDate = curDate.AddDays(1))
                            {
                                #region 取每天的刊例及详情

                                var gb = from d in curDetailList.Where(d=> d.CooperateBeginDate <= curDate && d.CooperateEndDate >= curDate)
                                         group d by new { d.ADPosition1, d.ADPosition2, d.ADPosition3 } into g
                                         select new { g.Key, DetailID = g.OrderBy(d => d.CostPrice).First().DetailID, OriginalPrice = g.OrderBy(d => d.CostPrice).First().OriginalPrice, CostPrice = g.OrderBy(d => d.CostPrice).First().CostPrice, SalePrice = g.OrderBy(d => d.CostPrice).First().SalePrice, ChannelID = g.OrderBy(d => d.CostPrice).First().ChannelID };
                                if (gb.Count() == 0)//当天没有价格
                                    continue;
                                OneDayPubDTO pub = new OneDayPubDTO() { PubDate = curDate };
                                foreach (var group in gb)
                                {
                                    pub.ADList.Add(new ADDTO()
                                    {
                                        ADPosition1 = group.Key.ADPosition1,
                                        ADPosition2 = group.Key.ADPosition2,
                                        ADPosition3 = group.Key.ADPosition3,
                                        CostPrice = group.CostPrice,
                                        SalePrice = group.SalePrice,
                                        ChannelID = group.ChannelID
                                    });
                                }
                                oneDayPubQ.Enqueue(pub);
                                #endregion
                            }

                            #region 拼接有效期
                            Queue<ManyDaysPubDTO> manyDayPubQ = new Queue<ManyDaysPubDTO>();
                            if (oneDayPubQ.Count == 0)
                                continue;
                            var startPub = oneDayPubQ.Dequeue();
                            DateTime endDate = startPub.PubDate;
                            while (oneDayPubQ.Count > 0)
                            {
                                var nextPub = oneDayPubQ.Dequeue();
                                if (endDate.AddDays(1) == nextPub.PubDate && startPub.ADList.Count == nextPub.ADList.Count && startPub.ADList.All(s => nextPub.ADList.Exists(n => n.ADPosition1 == s.ADPosition1 && n.ADPosition2 == s.ADPosition2 && n.ADPosition3 == s.ADPosition3 && n.CostPrice == s.CostPrice && n.SalePrice == s.SalePrice)))
                                {//有效期大的 且 广告位完全相同
                                    endDate = nextPub.PubDate;
                                }
                                else
                                {
                                    manyDayPubQ.Enqueue(new ManyDaysPubDTO() { ADList = startPub.ADList, PubBeginDate = startPub.PubDate, PubEndDate = endDate });
                                    startPub = nextPub;
                                    endDate = startPub.PubDate;
                                }
                            }
                            manyDayPubQ.Enqueue(new ManyDaysPubDTO() { ADList = startPub.ADList, PubBeginDate = startPub.PubDate, PubEndDate = endDate });
                            #endregion

                            #region 循环插入刊例、广告位
                            while (manyDayPubQ.Count > 0)
                            {
                                var pub = manyDayPubQ.Dequeue();
                                var originalPrice = curDetailList.Where(d => d.CooperateBeginDate <= pub.PubBeginDate && d.CooperateEndDate >= pub.PubEndDate).OrderBy(d => d.OriginalPrice).First().OriginalPrice;
                                foreach (var ad in pub.ADList)
                                {
                                    PreviewItem newItem = new PreviewItem()
                                    {
                                        MediaID = row.MediaID,
                                        WxNumber = row.WxNumber,
                                        WxName = row.WxName,
                                        ADPosition = Enum.GetName(typeof(ADPosition1Enum), ad.ADPosition1).Replace("一", "-") + Enum.GetName(typeof(ADPosition3Enum), ad.ADPosition3).Replace("十", "+"),
                                        ADPosition1 = ad.ADPosition1,
                                        ADPosition2 = ad.ADPosition2,
                                        ADPosition3 = ad.ADPosition3,
                                        SalePrice = ad.SalePrice,
                                        CostPrice = ad.CostPrice,
                                        CooperateBeginDate = pub.PubBeginDate,
                                        CooperateEndDate = pub.PubEndDate,
                                        CooperateDate = pub.PubBeginDate.ToString("yyyy-M-d") + "至" + pub.PubEndDate.ToString("yyyy-M-d")
                                    };
                                    res.List.Add(newItem);
                                }
                            }
                            #endregion
                            #region 旧
                            //List<DateItem> dateList = new List<DateItem>();//有效期时间
                            ////加入导入的渠道有效期
                            //dateList.Add(new DateItem { Date = excelDTO.CooperateBeginDate.Date, IsBeginDate = true });
                            //dateList.Add(new DateItem { Date = excelDTO.CooperateEndDate.Date, IsBeginDate = false });
                            ////所有价格  其它渠道 有当前媒体的成本 Excel涉及的渠道内包含
                            //var curDetailList = allDetailList.Where(d => d.MediaID.Equals(row.MediaID) && row.PriceList.Exists(p => p.ADPosition1.Equals(d.ADPosition1) && p.ADPosition2.Equals(d.ADPosition2) && p.ADPosition3.Equals(d.ADPosition3))).ToList();
                            //foreach (var detail in curDetailList)
                            //{//遍历 取涉及渠道 和 有效期
                            //    if (!channelList.Exists(c => c.Equals(detail.ChannelID)))
                            //    {
                            //        channelList.Add(detail.ChannelID);
                            //    }
                            //    if (!dateList.Exists(d => d.Date == detail.CooperateBeginDate && d.IsBeginDate == true))
                            //        dateList.Add(new DateItem { Date = detail.CooperateBeginDate.Date, IsBeginDate = true });
                            //    if (!dateList.Exists(d => d.Date == detail.CooperateEndDate && d.IsBeginDate == false))
                            //        dateList.Add(new DateItem { Date = detail.CooperateEndDate.Date, IsBeginDate = false });
                            //}
                            //dateList = dateList.OrderBy(d => d.Date).ToList();
                            //Dictionary<DateTime, DateTime> dateDict = new Dictionary<DateTime, DateTime>();
                            ////截取拆分时间
                            //for (var i = 0; i < dateList.Count - 1; i++)
                            //{
                            //    dateDict.Add(dateList[i].Date, dateList[i + 1].IsBeginDate ? dateList[i + 1].Date.AddDays(-1) : dateList[i + 1].Date);
                            //}
                            //#region 时间段+广告位 维度 遍历
                            //foreach (var item in dateDict)
                            //{
                            //    foreach (var price in row.PriceList)
                            //    {
                            //        PreviewItem newItem = new PreviewItem()
                            //        {
                            //            MediaID = row.MediaID,
                            //            WxNumber = row.WxNumber,
                            //            WxName = row.WxName,
                            //            ADPosition = price.ADPosition,
                            //            ADPosition1 = price.ADPosition1,
                            //            ADPosition2 = price.ADPosition2,
                            //            ADPosition3 = price.ADPosition3,
                            //            SalePrice = price.SalePrice,
                            //            CostPrice = price.CostPrice,
                            //            CooperateBeginDate = item.Key,
                            //            CooperateEndDate = item.Value,
                            //            CooperateDate = item.Key.ToString("yyyy-M-d") + "至" + item.Value.ToString("yyyy-M-d")
                            //        };
                            //        var selDetail = curDetailList.Where(p => p.ADPosition1.Equals(price.ADPosition1) && p.ADPosition2.Equals(price.ADPosition2) && p.ADPosition3.Equals(price.ADPosition3)).OrderBy(p => p.CostPrice).FirstOrDefault();
                            //        if (selDetail != null && selDetail.CostPrice < newItem.CostPrice)
                            //        {
                            //            newItem.CostPrice = selDetail.CostPrice;
                            //            newItem.SalePrice = selDetail.SalePrice;
                            //        }
                            //        res.List.Add(newItem);
                            //    }
                            //}
                            //#endregion
                            #endregion
                            #endregion
                        }
                        else
                        {
                            #region 新增媒体情况
                            foreach (var price in row.PriceList)
                            {
                                res.List.Add(new PreviewItem()
                                {
                                    WxNumber = row.WxNumber,
                                    WxName = row.WxName,
                                    ADPosition = price.ADPosition,
                                    ADPosition1 = price.ADPosition1,
                                    ADPosition2 = price.ADPosition2,
                                    ADPosition3 = price.ADPosition3,
                                    SalePrice = price.SalePrice,
                                    CostPrice = price.CostPrice,
                                    CooperateDate = excelDTO.CooperateBeginDate.ToString("yyyy-M-d") + "至" + excelDTO.CooperateEndDate.ToString("yyyy-M-d")
                                });
                            }
                            #endregion
                        }
                        #endregion
                    }

                    #region 补列名 和 价格
                    foreach (var channelID in channelList)
                    {
                        if (channelID.Equals(excelDTO.ChannelID))
                        {
                            #region 当前导入资源的渠道
                            res.ColumnNameList.Add(new ColumnNameItem
                            {
                                ChannelID = channelID,
                                ChannelName = excelDTO.ChannelName,
                                CooperateDate = excelDTO.CooperateBeginDate.ToString("yyyy-M-d") + "至" + excelDTO.CooperateEndDate.ToString("yyyy-M-d")
                            });
                            foreach (var item in res.List)
                            {//这里不用过滤时间段 导入时间段已覆盖 都是一个价格
                                var price = excelDTO.ImportRowList.First(i => i.WxNumber.Equals(item.WxNumber)).PriceList.FirstOrDefault(p => p.ADPosition1.Equals(item.ADPosition1) && p.ADPosition2.Equals(item.ADPosition2) && p.ADPosition3.Equals(item.ADPosition3));
                                if (price != null)
                                {
                                    item.PriceList.Add(price.SalePrice);
                                    item.PriceList.Add(price.CostPrice);
                                }
                                else
                                {//为0的没有进来 但是有这个广告位
                                    item.PriceList.Add(0);
                                    item.PriceList.Add(0);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 其它渠道
                            var firstChannelDetail = allDetailList.FirstOrDefault(d => d.ChannelID.Equals(channelID));
                            res.ColumnNameList.Add(new ColumnNameItem
                            {
                                ChannelID = channelID,
                                ChannelName = firstChannelDetail.ChannelName,
                                CooperateDate = firstChannelDetail.CooperateBeginDate.ToString("yyyy-M-d") + "至" + firstChannelDetail.CooperateEndDate.ToString("yyyy-M-d")
                            });
                            foreach (var item in res.List)
                            {//这里注意要限制时间
                                var selDetail = allDetailList.FirstOrDefault(d => d.ChannelID.Equals(channelID) && d.WxNumber.Equals(item.WxNumber) 
                                                                                            && d.CooperateBeginDate <= item.CooperateEndDate && d.CooperateEndDate >= item.CooperateBeginDate 
                                                                                            && d.ADPosition1.Equals(item.ADPosition1) && d.ADPosition2.Equals(item.ADPosition2) && d.ADPosition3.Equals(item.ADPosition3));
                                if (selDetail != null)
                                {
                                    item.PriceList.Add(selDetail.SalePrice);
                                    item.PriceList.Add(selDetail.CostPrice);
                                }
                                else
                                {
                                    item.PriceList.Add(0);
                                    item.PriceList.Add(0);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    objCache.Insert(ur.UserID + "UploadResourceView", res, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                    return res;
                }
                catch (Exception ex)
                {
                    logger.Error("资源预览异常", ex);
                    msg = "系统异常:" + ex.ToString();
                    return null;
                }
            }
        }

        /// <summary>
        /// 提交资源
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SubmitUploadResource(ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            string domainPath = WebConfigurationManager.AppSettings["CatchDomain"];
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (!ur.IsAE)
            {
                msg = "角色错误";
                return false;
            }
            try
            {
                var excelDTO = objCache.Get(ur.UserID + "UploadResourceExcel") as CostExcelDTO;//Excel
                var previewDTO = objCache.Get(ur.UserID + "UploadResourceView") as GetUploadPreviewResDTO;//Preview
                var updateStatus = objCache.Get("ChannelResourseUploadStatus");
                if (excelDTO == null || previewDTO == null) {
                    msg = "超过有效期，请重新上传";
                    return false;
                }
                if (updateStatus != null && (int)updateStatus == 1)
                {
                    msg = "还有正在提交的数据，请稍后上传";
                    return false;
                }
                objCache.Insert("ChannelResourseUploadStatus", 1);
                foreach (var row in excelDTO.ImportRowList)
                {
                    #region 遍历
                    Entities.DTO.CatchWeixinModel catchData = null;
                    string biz = Dal.WeixinOAuth.Instance.GetBizByWxNumber(row.WxNumber);
                    if (!string.IsNullOrWhiteSpace(biz))
                    {
                        catchData = BLL.WeixinOAuth.Instance.GetCatchData("",biz);
                        if (catchData != null)
                        {
                            if (!string.IsNullOrWhiteSpace(catchData.HeadImg))
                                catchData.HeadImg = domainPath + "/" + catchData.HeadImg;
                            else
                                catchData.HeadImg = "http://www.chitunion.com/images/default_touxiang.png";
                            if (!string.IsNullOrWhiteSpace(catchData.QrCode))
                                catchData.QrCode = domainPath + "/" + catchData.QrCode;
                        }
                    }
                    int wxID = UpdateBasicMedia(row, ur.UserID, catchData);
                    int mediaID = UpdateMedia(row, ur.UserID, wxID, catchData);
                    UpdateCost(row, ur.UserID, excelDTO.ChannelID, mediaID);
                    #endregion
                }
                ComputePriceByChannel(excelDTO.ChannelID);
                objCache.Remove(ur.UserID + "UploadResourceExcel");
                objCache.Remove(ur.UserID + "UploadResourceView");
                objCache.Insert("ChannelResourseUploadStatus", 0);
                return true; 
            }
            catch (Exception ex)
            {
                objCache.Insert("ChannelResourseUploadStatus", 0);
                logger.Error("资源提交异常",ex);
                msg = "系统异常："+ex.ToString();
                return false;
            }
        }

        #region 提交相关
        /// <summary>
        /// 更新基表信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private int UpdateBasicMedia(ImportCostRow row, int userID, Entities.DTO.CatchWeixinModel catchData)
        {
            var basic = Dal.WeixinOAuth.Instance.GetWeixinInfoByWxNumber(row.WxNumber, 0, -1);
            int wxID = 0;
            if (basic == null)
            {
                #region 新增基表媒体 区域 分类 备注
                //媒体
                basic = new Entities.WeixinOAuth.WeixinInfo()
                {
                    WxNumber = row.WxNumber,
                    NickName = row.WxName,
                    FansCount = row.FansCount,
                    LevelType = row.LevelType,
                    SourceType = (int)SourceTypeEnum.手工,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    Status = 0,
                };
                if (!row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE) || !row.CityID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE))
                {
                    basic.IsAreaMedia = true;
                }
                if (catchData != null)
                {
                    basic.HeadImg = catchData.HeadImg;
                    basic.QrCodeUrl = catchData.QrCode;
                }
                wxID = Dal.WeixinOAuth.Instance.AddWeixinInfo(basic);
                #endregion
            }
            else
            {
                wxID = basic.RecID;
                #region 更新基表媒体 删除 区域、分类、备注
                basic.NickName = row.WxName;
                basic.FansCount = row.FansCount;
                basic.LevelType = row.LevelType;
                basic.ModifyTime = DateTime.Now;
                basic.Status = 0;
                if (!row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE) || !row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE))
                {
                    basic.IsAreaMedia = true;
                }
                else
                {
                    basic.IsAreaMedia = false;
                }
                if (catchData != null && basic.SourceType != (int)SourceTypeEnum.扫码授权)
                {
                    basic.HeadImg = catchData.HeadImg;
                    basic.QrCodeUrl = catchData.QrCode;
                }
                Dal.WeixinOAuth.Instance.UpdateWeixinInfo(basic);
                Dal.Media.MediaAreaMapping.Instance.DeleteBase(wxID, (int)MediaTypeEnum.微信, (int)MediaAreaMappingType.AreaMedia);
                Dal.Media.MediaRemark.Instance.DeleteBasic((int)MediaRemarkTypeEnum.微信备注, wxID);
                Dal.Media.MediaCommonlyClass.Instance.DeleteBasic((int)MediaTypeEnum.微信, wxID);
                #endregion
            }
            #region 区域 备注 分类
            //区域
            if (basic.IsAreaMedia)
            {
                Entities.Media.MediaAreaMappingBase map = new Entities.Media.MediaAreaMappingBase()
                {
                    BaseMediaID = wxID,
                    MediaType = (int)Entities.Enum.MediaTypeEnum.微信,
                    ProvinceID = row.ProvinceID,
                    CityID = row.CityID,
                    RelateType = (int)Entities.Enum.MediaAreaMappingType.AreaMedia,
                    CreateUserID = userID,
                    CreateTime = DateTime.Now
                };
                Dal.Media.MediaAreaMapping.Instance.InsertBasic(map);
            }
            //分类
            foreach (var categoryID in row.CategoryDict.Keys.ToList())
            {
                Entities.Media.MediaCommonlyClass category = new Entities.Media.MediaCommonlyClass()
                {
                    MediaID = wxID,
                    MediaType = (int)Entities.Enum.MediaTypeEnum.微信,
                    CategoryID = categoryID,
                    SortNumber = row.CategoryDict.Keys.ToList().IndexOf(categoryID).Equals(0) ? 1 : 0
                };
                Dal.Media.MediaCommonlyClass.Instance.InsertBasic(category);
            }
            //备注
            foreach (var item in row.RemarkDict)
            {
                Entities.Media.MediaRemarkBasic remark = new Entities.Media.MediaRemarkBasic()
                {
                    RelationID = wxID,
                    EnumType = (int)MediaRemarkTypeEnum.微信备注,
                    RemarkID = item.Key,
                    CreateTime = DateTime.Now,
                    CreateUserID = userID
                };
                Dal.Media.MediaRemark.Instance.InsertBasic(remark);
            }
            #endregion
            return wxID;
        }

        private int UpdateMedia(ImportCostRow row,  int userID, int wxID, Entities.DTO.CatchWeixinModel catchData)
        {
            int mediaID = row.MediaID;
            if (row.MediaID.Equals(0))
            {
                #region  新增媒体
                Entities.Media.MediaWeixin media = new Entities.Media.MediaWeixin()
                {
                    Number = row.WxNumber,
                    Name = row.WxName,
                    FansCount = row.FansCount,
                    LevelType = row.LevelType,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    CreateUserID = userID,
                    LastUpdateUserID = userID,
                    AuditStatus = (int)MediaAuditStatusEnum.AlreadyPassed,
                    AuthType = (int)SourceTypeEnum.手工,
                    Source = 3001,
                    Status = 0,
                    ADName = row.WxName,
                    WxID = wxID
                };
                if (!row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE) || !row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE))
                {
                    media.IsAreaMedia = true;
                }
                if (catchData != null)
                {
                    media.HeadIconURL = catchData.HeadImg;
                    media.TwoCodeURL = catchData.QrCode;
                }
                mediaID = Dal.Media.MediaWeixin.Instance.Insert(media);
                #endregion
            }
            else if (!row.MediaIsOld)
            {
                #region 更新媒体 删除备注 分类
                var oldMedia = Dal.Media.MediaWeixin.Instance.GetEntity(row.MediaID);
                oldMedia.Name = row.WxName;
                oldMedia.FansCount = row.FansCount;
                oldMedia.LevelType = row.LevelType;
                oldMedia.LastUpdateTime = DateTime.Now;
                oldMedia.LastUpdateUserID = userID;
                oldMedia.AuditStatus = (int)MediaAuditStatusEnum.AlreadyPassed;
                oldMedia.ADName = row.WxName;
                oldMedia.WxID = wxID;
                if (!row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE) || !row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE))
                {
                    oldMedia.IsAreaMedia = true;
                }
                else
                {
                    oldMedia.IsAreaMedia = false;
                }
                if (catchData != null)
                {
                    oldMedia.HeadIconURL = catchData.HeadImg;
                    oldMedia.TwoCodeURL = catchData.QrCode;
                }
                mediaID = oldMedia.MediaID;
                Dal.Media.MediaWeixin.Instance.Update(oldMedia);//此方法没有更新WxID 所以调用下行方法
                Dal.Media.MediaWeixin.Instance.UpdateWxId(mediaID, wxID);
                Dal.Media.MediaAreaMapping.Instance.Delete(mediaID, (int)MediaTypeEnum.微信, (int)MediaAreaMappingType.AreaMedia);
                Dal.Media.MediaRemark.Instance.Delete((int)MediaRemarkTypeEnum.微信备注, mediaID);
                Dal.Media.MediaCommonlyClass.Instance.Delete((int)MediaTypeEnum.微信, mediaID);
                #endregion
            }
            if (!row.MediaIsOld)
            {
                #region 更新区域 分类 备注
                //区域
                if (!row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE) || !row.ProvinceID.Equals(Entities.Constants.Constant.INT_INVALID_VALUE))
                {
                    Entities.Media.MediaAreaMapping map = new Entities.Media.MediaAreaMapping()
                    {
                        MediaID = mediaID,
                        MediaType = (int)Entities.Enum.MediaTypeEnum.微信,
                        ProvinceID = row.ProvinceID,
                        CityID = row.CityID,
                        RelateType = (int)Entities.Enum.MediaAreaMappingType.AreaMedia,
                        CreateUserID = userID,
                        CreateTime = DateTime.Now
                    };
                    Dal.Media.MediaAreaMapping.Instance.Insert(map);
                }
                //分类
                foreach (var categoryID in row.CategoryDict.Keys.ToList())
                {
                    Entities.Media.MediaCommonlyClass category = new Entities.Media.MediaCommonlyClass()
                    {
                        MediaID = mediaID,
                        MediaType = (int)Entities.Enum.MediaTypeEnum.微信,
                        CategoryID = categoryID,
                        SortNumber = row.CategoryDict.Keys.ToList().IndexOf(categoryID).Equals(0) ? 1 : 0,
                        CreateUserID = userID,
                        CreateTime = DateTime.Now
                    };
                    Dal.Media.MediaCommonlyClass.Instance.Insert(category);
                }
                //备注
                foreach (var item in row.RemarkDict)
                {
                    Entities.Media.MediaRemarkBasic remark = new Entities.Media.MediaRemarkBasic()
                    {
                        RelationID = mediaID,
                        EnumType = (int)MediaRemarkTypeEnum.微信备注,
                        RemarkID = item.Key,
                        CreateTime = DateTime.Now,
                        CreateUserID = userID
                    };
                    Dal.Media.MediaRemark.Instance.Insert(remark);
                }
                #endregion
            }
            return mediaID;
        }

        private void UpdateCost(ImportCostRow row, int userID, int channelID, int mediaID)
        {
            int costID = row.CostID;
            if (costID.Equals(0))
            {
                Entities.Channel.ChannelCost cost = new Entities.Channel.ChannelCost();
                cost.ChannelID = channelID;
                cost.MediaID = mediaID;
                cost.CreateUserID = userID;
                cost.SaleStatus = (int)MediaPublishStatusEnum.UpOnshelf;
                cost.OriginalPrice = row.OriginalPrice;
                cost.Status = 0;
                cost.CreateTime = DateTime.Now;
                cost.LastUpdateTime = DateTime.Now;
                costID = Dal.Channel.Instance.AddCost(cost);
            }
            else
            {
                var cost = Dal.Channel.Instance.GetCost(costID);
                cost.SaleStatus = (int)MediaPublishStatusEnum.UpOnshelf;
                cost.OriginalPrice = row.OriginalPrice;
                cost.Status = 0;
                cost.LastUpdateTime = DateTime.Now;
                Dal.Channel.Instance.UpdateCost(cost);
                if (row.DeleteDetailIDs != null && row.DeleteDetailIDs.Count > 0)
                {
                    Dal.Channel.Instance.DeleteCostDetail(row.DeleteDetailIDs);
                }
            }
            foreach (var p in row.PriceList)
            {
                #region 新增、更新价格
                if (p.DetailID.Equals(0))
                {
                    Entities.Channel.ChannelCostDetail detail = new Entities.Channel.ChannelCostDetail()
                    {
                        ADPosition1 = p.ADPosition1,
                        ADPosition2 = p.ADPosition2,
                        ADPosition3 = p.ADPosition3,
                        ChannelID = channelID,
                        CostID = costID,
                        Status = 0,
                        CreateUserID = userID,
                        CreateTime = DateTime.Now,
                        SalePrice = p.SalePrice,
                        CostPrice = p.CostPrice
                    };
                    Dal.Channel.Instance.AddCostDetail(detail);
                }
                else
                {
                    Entities.Channel.ChannelCostDetail detail = new Entities.Channel.ChannelCostDetail()
                    {
                        DetailID = p.DetailID,
                        SalePrice = p.SalePrice,
                        CostPrice = p.CostPrice
                    };
                    Dal.Channel.Instance.UpdateCostDetail(detail);
                }
                #endregion
            }
        }
        #endregion

        /// <summary>
        /// 按渠道计算价格 如果有新的时间 可以不按照渠道对应的有效期
        /// </summary>
        /// <param name="channelID">渠道ID</param>
        /// <param name="beginDate">旧开始时间</param>
        /// <param name="endDate">新开始时间</param>
        private void ComputePriceByChannel(int channelID, DateTime? beginDate = null, DateTime? endDate = null)
        {
            try
            {
                var list = Dal.Channel.Instance.GetNeedComputeMediaIDList(channelID);
                var channel = Dal.Channel.Instance.GetChannelDetail(channelID);
                if(beginDate.HasValue && endDate.HasValue)
                    ComputePriceByCondition(beginDate.Value.Date, endDate.Value.Date, channel.CreateUserID, list.ToArray());
                else
                    ComputePriceByCondition(channel.CooperateBeginDate.Date, channel.CooperateEndDate.Date, channel.CreateUserID, list.ToArray());
            }
            catch (Exception ex)
            {
                logger.Error("计算价格错误 ChannelID:" + channelID, ex);
            }
        }

        /// <summary>
        /// 按成本ID计算价格（成本所属媒体 成本所属渠道周期 成本涉及广告位 成本的创建者）
        /// </summary>
        /// <param name="costIDs"></param>
        private void ComputePriceByCost(params int[] costIDs)
        {
            //创建人 按照成本的创建人统一处理
            try
            {
                foreach (int costID in costIDs)
                {
                    var costInfo = Dal.Channel.Instance.GetCostDetail(costID);
                    if (costInfo == null || costInfo.CostDetailList == null || costInfo.CostDetailList.Count == 0)
                        continue;
                    this.ComputePriceByCondition(costInfo.CooperateBeginDate, costInfo.CooperateEndDate, costInfo.CreateUserID, costInfo.MediaID);
                }
            }
            catch (Exception ex)
            {
                logger.Error("ComputePriceByCost计算价格错误 CostIDs:" + string.Join(",", costIDs), ex);
            }
        }

        /// <summary>
        /// 按条件计算价格
        /// </summary>
        /// <param name="mediaID">媒体ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="createUserID">创建人ID</param>
        private void ComputePriceByCondition(DateTime beginDate, DateTime endDate, int createUserID, params int[] mediaIDs)
        {
            foreach (var mediaID in mediaIDs)
            {
                Dal.PublishInfo.Instance.DeleteCrossDatePublish(mediaID, beginDate, endDate);
                //获取一个媒体在这个时间段内所有广告位价格
                var allDetailList = Dal.Channel.Instance.PreviewGetCostDetailList(0, mediaID.ToString(), beginDate, endDate, string.Empty);
                Queue<OneDayPubDTO> oneDayPubQ = new Queue<OneDayPubDTO>();
                for (DateTime curDate = beginDate; curDate < endDate.AddDays(1); curDate = curDate.AddDays(1))
                {
                    #region 取每天的刊例及详情

                    var gb = from d in allDetailList.Where(d => d.CooperateBeginDate <= curDate && d.CooperateEndDate >= curDate)
                             group d by new { d.ADPosition1, d.ADPosition2, d.ADPosition3 } into g
                             select new
                             {
                                 g.Key,
                                 DetailID = g.OrderBy(d => d.CostPrice).First().DetailID,
                                 OriginalPrice = g.OrderBy(d => d.CostPrice).First().OriginalPrice,
                                 CostPrice = g.Min(i => i.CostPrice),
                                 SalePrice = g.Min(i => i.SalePrice),
                                 ChannelID = g.OrderBy(d => d.CostPrice).First().ChannelID
                             };
                    if (gb.Count() == 0)//当天没有价格
                        continue;
                    OneDayPubDTO pub = new OneDayPubDTO() { PubDate = curDate };
                    foreach (var group in gb)
                    {
                        pub.ADList.Add(new ADDTO()
                        {
                            ADPosition1 = group.Key.ADPosition1,
                            ADPosition2 = group.Key.ADPosition2,
                            ADPosition3 = group.Key.ADPosition3,
                            CostPrice = group.CostPrice,
                            SalePrice = group.SalePrice,
                            CostDetailID = group.DetailID
                        });
                    }
                    pub.OriginalPrice = gb.Min(i => i.OriginalPrice);
                    oneDayPubQ.Enqueue(pub);
                    #endregion
                }
                #region 拼接有效期
                Queue<ManyDaysPubDTO> manyDayPubQ = new Queue<ManyDaysPubDTO>();
                if (oneDayPubQ.Count == 0)
                    return;
                var startPub = oneDayPubQ.Dequeue();
                DateTime deadLine = startPub.PubDate;
                while (oneDayPubQ.Count > 0)
                {
                    var nextPub = oneDayPubQ.Dequeue();
                    if (deadLine.AddDays(1) == nextPub.PubDate && startPub.ADList.Count == nextPub.ADList.Count && startPub.ADList.All(s => nextPub.ADList.Exists(n => n.ADPosition1 == s.ADPosition1 && n.ADPosition2 == s.ADPosition2 && n.ADPosition3 == s.ADPosition3 && n.CostPrice == s.CostPrice && n.SalePrice == s.SalePrice)))
                    {//有效期大的 且 广告位完全相同
                        deadLine = nextPub.PubDate;
                        if (startPub.OriginalPrice > nextPub.OriginalPrice)
                            startPub.OriginalPrice = nextPub.OriginalPrice;
                    }
                    else
                    {
                        manyDayPubQ.Enqueue(new ManyDaysPubDTO() { ADList = startPub.ADList, PubBeginDate = startPub.PubDate, PubEndDate = deadLine, OriginalPrice = startPub.OriginalPrice});
                        startPub = nextPub;
                        deadLine = startPub.PubDate;
                    }
                }
                manyDayPubQ.Enqueue(new ManyDaysPubDTO() { ADList = startPub.ADList, PubBeginDate = startPub.PubDate, PubEndDate = deadLine, OriginalPrice = startPub.OriginalPrice });
                #endregion

                #region 旧
                ////创建人 按照成本的创建人统一处理
                //var combinList = Dal.PublishInfo.Instance.SplitCrossDatePublish(mediaID, beginDate, endDate, adPositionRange);
                //Queue<OneDayPubDTO> oneDayPubQ = new Queue<OneDayPubDTO>();
                //for (DateTime curDate = beginDate; curDate < endDate.AddDays(1); curDate = curDate.AddDays(1))
                //{
                //    #region 取每天的刊例及详情

                //    if (combinList.Exists(c => curDate >= c.BeginDate && curDate <= c.EndDate))
                //    {
                //        #region 和旧刊例重叠的部分
                //        var cb = combinList.First(c => curDate >= c.BeginDate && curDate <= c.EndDate);
                //        var curDetailList = Dal.Channel.Instance.PreviewGetCostDetailList(0, mediaID.ToString(), cb.BeginDate, cb.EndDate, string.Join(",",cb.PositionList.ToArray()));
                //        if (curDetailList == null || curDetailList.Count == 0)
                //            continue;
                //        var gb = from d in curDetailList.Where(d => d.CooperateBeginDate <= curDate && d.CooperateEndDate >= curDate)
                //                 group d by new { d.ADPosition1, d.ADPosition2, d.ADPosition3 } into g
                //                 select new { g.Key, DetailID = g.OrderBy(d => d.CostPrice).First().DetailID, CostPrice = g.OrderBy(d => d.CostPrice).First().CostPrice, SalePrice = g.OrderBy(d => d.CostPrice).First().SalePrice, OriginalPrice = g.OrderBy(d => d.CostPrice).First().OriginalPrice };
                //        if (gb.Count() == 0)//当天没有价格
                //            continue;
                //        OneDayPubDTO pub = new OneDayPubDTO() { PubDate = curDate };
                //        foreach (var group in gb)
                //        {
                //            pub.ADList.Add(new ADDTO()
                //            {
                //                ADPosition1 = group.Key.ADPosition1,
                //                ADPosition2 = group.Key.ADPosition2,
                //                ADPosition3 = group.Key.ADPosition3,
                //                CostPrice = group.CostPrice,
                //                SalePrice = group.SalePrice,
                //                CostDetailID = group.DetailID
                //            });
                //        }
                //        pub.OriginalPrice = gb.Min(i => i.OriginalPrice);
                //        oneDayPubQ.Enqueue(pub);
                //        #endregion
                //    }
                //    else
                //    {
                //        #region 非重叠部分
                //        var curDetailList = Dal.Channel.Instance.PreviewGetCostDetailList(0, mediaID.ToString(), beginDate, endDate, adPositionRange);
                //        var gb = from d in curDetailList.Where(d => d.CooperateBeginDate <= curDate && d.CooperateEndDate >= curDate)
                //                 group d by new { d.ADPosition1, d.ADPosition2, d.ADPosition3 } into g
                //                 select new { g.Key, DetailID = g.OrderBy(d => d.CostPrice).First().DetailID, CostPrice = g.OrderBy(d => d.CostPrice).First().CostPrice, SalePrice = g.OrderBy(d => d.CostPrice).First().SalePrice, OriginalPrice = g.OrderBy(d=>d.CostPrice).First().OriginalPrice };
                //        if (gb.Count() == 0)//当天没有价格
                //            return;
                //        OneDayPubDTO pub = new OneDayPubDTO() { PubDate = curDate };
                //        foreach (var group in gb)
                //        {
                //            pub.ADList.Add(new ADDTO()
                //            {
                //                ADPosition1 = group.Key.ADPosition1,
                //                ADPosition2 = group.Key.ADPosition2,
                //                ADPosition3 = group.Key.ADPosition3,
                //                CostPrice = group.CostPrice,
                //                SalePrice = group.SalePrice,
                //                CostDetailID = group.DetailID
                //            });
                //        }
                //        pub.OriginalPrice = gb.Min(i => i.OriginalPrice);
                //        oneDayPubQ.Enqueue(pub);
                //        #endregion
                //    }
                //    #endregion
                //}

                //#region 拼接有效期
                //Queue<ManyDaysPubDTO> manyDayPubQ = new Queue<ManyDaysPubDTO>();
                //if (oneDayPubQ.Count == 0)
                //    return;
                //var startPub = oneDayPubQ.Dequeue();
                //DateTime toDate = startPub.PubDate;
                //while (oneDayPubQ.Count > 0)
                //{
                //    var nextPub = oneDayPubQ.Dequeue();
                //    if (startPub.PubDate < nextPub.PubDate && startPub.ADList.Count == nextPub.ADList.Count && startPub.ADList.All(s => nextPub.ADList.Exists(n => n.ADPosition1 == s.ADPosition1 && n.ADPosition2 == s.ADPosition2 && n.ADPosition3 == s.ADPosition3 && n.CostPrice == s.CostPrice && n.SalePrice == s.SalePrice)))
                //    {//有效期大的 且 广告位完全相同
                //        endDate = nextPub.PubDate;
                //        if (startPub.OriginalPrice > nextPub.OriginalPrice)
                //            startPub.OriginalPrice = nextPub.OriginalPrice;
                //    }
                //    else
                //    {
                //        manyDayPubQ.Enqueue(new ManyDaysPubDTO() { ADList = startPub.ADList, PubBeginDate = startPub.PubDate, PubEndDate = endDate, OriginalPrice = nextPub.OriginalPrice });
                //        startPub = nextPub;
                //        endDate = startPub.PubDate;
                //    }
                //}
                //manyDayPubQ.Enqueue(new ManyDaysPubDTO() { ADList = startPub.ADList, PubBeginDate = startPub.PubDate, PubEndDate = endDate, OriginalPrice = startPub.OriginalPrice });
                //#endregion
                #endregion

                #region 循环插入刊例、广告位
                while (manyDayPubQ.Count > 0)
                {
                    var pub = manyDayPubQ.Dequeue();
                    Entities.Publish.PublishBasicInfo pubInfo = new Entities.Publish.PublishBasicInfo()
                    {
                        MediaID = mediaID,
                        Status = AuditStatusEnum.初始状态,
                        PublishStatus = PublishStatusEnum.初始状态,
                        Wx_Status = PublishBasicStatusEnum.上架,
                        MediaType = MediaTypeEnum.微信,
                        BeginTime = pub.PubBeginDate,
                        EndTime = pub.PubEndDate,
                        PurchaseDiscount = 1,
                        SaleDiscount = 1,
                        OriginalReferencePrice = pub.OriginalPrice,
                        CreateTime = DateTime.Now,
                        CreateUserID = createUserID,
                        LastUpdateTime = DateTime.Now,
                        LastUpdateUserID = createUserID,
                        IsDel = 0
                    };
                    int pubID = Dal.PublishInfo.Instance.AddPublishBasicInfo(pubInfo);
                    foreach (var price in pub.ADList)
                    {
                        #region 广告位
                        Entities.Publish.PublishDetailInfo detailInfo = new Entities.Publish.PublishDetailInfo()
                        {
                            PubID = pubID,
                            MediaType = MediaTypeEnum.微信,
                            MediaID = mediaID,
                            ADPosition1 = price.ADPosition1,
                            ADPosition2 = price.ADPosition3,
                            Price = price.SalePrice,
                            SalePrice = price.SalePrice,
                            CostReferencePrice = price.CostPrice,
                            CreateTime = DateTime.Now,
                            CreateUserID = createUserID,
                            CostDetailID = price.CostDetailID
                        };
                        Dal.PublishInfo.Instance.AddPublishDetail(detailInfo);
                        #endregion
                    }
                }
                #endregion
            }
        }
      
        #endregion

    }
}
