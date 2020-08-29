/**
*----------Dragon be here!----------/
* 　　    ┏ ┓　.┏ ┓
* 　　┏━┛ ┻━━┛ ┻━━┓
* 　　┃　　    .　　  ┃ 
* 　　┃　 ┳┛　┗┳　 ┃
* 　　┃　　　　　　┃
* 　　┃　    ━┻━　　┃
* 　　┗━┓　　　  ┏━┛
* 　　    ┃  　　　┃
* 　　　 ┃  　　   ┗━━━━━━━━┓
* 　　　 ┃  　神兽保佑　  　  .┣┓
* 　　　 ┃　  永无BUG　　　 ┏┛
* 　　　 ┗┓┓┏━━━┳┓┏━━━━━━┛
* 　　　   ┗┻┛      ┗┻┛
*-------------------------------------/
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Configuration;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using log4net.Config;
using log4net;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Interaction;

namespace XYAuto.ITSC.Chitunion2017.DataImport
{
    class Program
    {
        static readonly string excelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["ExcelPath"]);
        static readonly int userID = int.Parse(ConfigurationManager.AppSettings["ImportUserID"]);
        static readonly int source = int.Parse(ConfigurationManager.AppSettings["UserSource"]);
        static readonly string importItem = ConfigurationManager.AppSettings["ImportItem"];
        static readonly string defaultPubEndDate = ConfigurationManager.AppSettings["DefaultPubEndDate"];


        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));
            //Import_WeixinData(logger);

            APP_MediaHelper.Instance.Import_AppData();
            Console.ReadKey();
        }

        private static void Import_WeixinData(ILog logger)
        {
            logger.Info("程序开始运行...");
            logger.Info("Excel位置:" + excelPath);
            logger.Info("统一导入UserID:" + userID);
            if (File.Exists(excelPath))
            {
                BLL.ExcelDataImport bll = new BLL.ExcelDataImport();
                //Sheet名与媒体类型的对应关系
                Dictionary<string, MediaTypeEnum> dict = new Dictionary<string, MediaTypeEnum>();
                if (importItem.Contains("微信号"))
                    dict.Add("微信号", MediaTypeEnum.微信);
                if (importItem.Contains("微博"))
                    dict.Add("微博", MediaTypeEnum.微博);
                if (importItem.Contains("视频"))
                    dict.Add("视频", MediaTypeEnum.视频);
                if (importItem.Contains("直播"))
                    dict.Add("直播", MediaTypeEnum.直播);
                if (importItem.Contains("APP"))
                    dict.Add("APP", MediaTypeEnum.APP);
                logger.Info("开始读取Excel...");
                DataSet ds = ConvertExcelToDataSet(dict.Keys.ToArray());
                List<ImportDTO> list = new List<ImportDTO>();
                List<string> failList = new List<string>();
                List<string> appNameList = new List<string>();
                foreach (var item in dict)
                {
                    #region 依次转化
                    if (ds.Tables[item.Key] != null && ds.Tables[item.Key].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[item.Key];
                        foreach (DataRow dr in dt.Rows)
                        {
                            string error = string.Empty;
                            var dto = ConvertToDTO(dr, item.Value, ref error);
                            if (dto == null)
                            {
                                failList.Add(item.Key + "第" + (dt.Rows.IndexOf(dr) + 2) + "行转化实体失败:" + error);
                                logger.Error(failList.Last());
                                continue;
                            }
                            //else {
                            //logger.Info(item.Key + "第" + (dt.Rows.IndexOf(dr) + 2) + "行转化实体成功!");
                            //}
                            list.Add(dto);
                            if (dto.MediaType.Equals(MediaTypeEnum.APP) && !appNameList.Contains(dto.MediaInfo.Name))
                                appNameList.Add(dto.MediaInfo.Name);
                        }
                    }
                    else
                    {
                        logger.Error("未找到" + item.Key + "的Sheet!");
                    }
                    #endregion
                }
                logger.Info("共读取到" + list.Count + "条记录");
                logger.Info("开始插入数据库...");
                int success = 0;
                int updateCount = 0;
                bool isUpdate = false;
                bll.DeleteAppDetail(appNameList, userID);
                foreach (var dto in list)
                {
                    if (string.IsNullOrWhiteSpace(dto.Key))
                        continue;
                    int res = bll.DoProcess(dto, ref isUpdate);
                    if (res > 0)
                    {
                        if (isUpdate)
                            updateCount++;
                        success++;
                        logger.Info(Enum.GetName(typeof(MediaTypeEnum), dto.MediaType) + ":" + dto.Key + " " + (isUpdate ? "更新" : "新增") + "成功");
                    }
                    else
                    {
                        failList.Add(Enum.GetName(typeof(MediaTypeEnum), dto.MediaType) + ":" + dto.Key + " " + (isUpdate ? "更新" : "新增") + "失败");
                        logger.Error(failList.Last());
                    }
                }
                logger.Info(string.Format("共导入成功{0}条记录,新增{1}条,更新{2}条", success, success - updateCount, updateCount));
                failList.ForEach(i => logger.Error(i));
            }
            else
            {
                logger.Error("未找到Excel文件!");
            }
        }

        /// <summary>  
        /// 读取Excel文件到DataSet中  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        public static DataSet ConvertExcelToDataSet(string[] sheetNames)
        {
            DataSet ds = new DataSet();
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
            {
                #region 遍历Sheet
                ISheet sheet = hssfworkbook.GetSheetAt(i);
                if (!sheetNames.Contains(sheet.SheetName))
                    continue;
                DataTable dt = new DataTable();
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    dt.Columns.Add(cell == null?string.Empty:cell.ToString().Trim());
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
                ds.Tables.Add(dt);
                #endregion
            }
            return ds;
        }

        public static ImportDTO ConvertToDTO(DataRow dr, MediaTypeEnum mediaType,ref string error)
        {
            try
            {
                ImportDTO dto = new ImportDTO() { MediaType = mediaType };
                dto.MediaInfo = ConvertToMediaInfo(dr, mediaType);
                dto.PubBasicInfo = ConvertToPublishBasic(dr, mediaType);
                dto.Interaction = ConvertToInteraction(dr, mediaType);
                dto.PubDetailList = ConvertToPublishDetailList(dr, mediaType);
                //五个媒体默认覆盖区域全国
                dto.MappingList = new List<Entities.Media.MediaAreaMapping>() { 
                    new Entities.Media.MediaAreaMapping(){
                        ProvinceID = 0,
                        CityID = -1,
                        CreateUserID = userID,
                        CreateTime = DateTime.Now
                    }
                };
                switch (mediaType)
                {
                    case MediaTypeEnum.微信:
                        dto.Key = dto.MediaInfo.Number;
                        break;
                    case MediaTypeEnum.微博:
                        dto.Key = dto.MediaInfo.Name;
                        break;
                    case MediaTypeEnum.视频:
                        dto.Key = dto.MediaInfo.Number;
                        break;
                    case MediaTypeEnum.直播:
                        dto.Key = dto.MediaInfo.Number;
                        break;
                    case MediaTypeEnum.APP:
                        dto.Key = dto.MediaInfo.Name;
                        string path = GetDataRowValue<string>(dr, "图例位置");
                        if (!string.IsNullOrWhiteSpace(path))
                            path = "/UploadFiles/app_pic/"+path;
                        #region 广告位
                        dto.PubExtend = new ADPositionDTO()
                        {
                            AdForm = GetDataRowValue<string>(dr,"广告形式"),
                            AdPosition = GetDataRowValue<string>(dr, "广告位位置"),
                            ADShow = GetDataRowValue<string>(dr, "广告展示逻辑"),
                            AdLegendURL = path,
                            ADRemark = GetDataRowValue<string>(dr, "广告位说明"),
                            Style =  GetDataRowValue<string>(dr, "样式"),
                            DisplayLength = GetDataRowValue<int>(dr,"时长（秒）"),
                            CarouselCount = GetDataRowValue<int>(dr,"轮播数"),
                            SysPlatform = GetSysPlatform(dr["系统平台"].ToString()),
                            ThrMonitor = GetThrMonitor(dr),
                            PlayPosition = GetDataRowValue<string>(dr, "广告位位置"),
                            CanClick = GetDataRowValue<bool>(dr,"是否可点击"),
                            DailyClickCount = GetDataRowValue<int>(dr, "日均点击量"),
                            CreateTime = DateTime.Now,
                            CreateUserID = userID,
                            LastUpdateTime = DateTime.Now,
                            LastUpdateUserID = userID
                        };
                        #endregion
                        break;
                }
                return dto;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
        
        public static dynamic ConvertToMediaInfo(DataRow dr, MediaTypeEnum mediaType) {

            BLL.ExcelDataImport bll = new ExcelDataImport();
            ImportDTO dto = new ImportDTO();
            int platform = -1;//平台
            Tuple<int, int> area = new Tuple<int, int>(-1, -1);
            //覆盖区域默认为全国
            dynamic mediaInfo = null;
            switch (mediaType) {
                case MediaTypeEnum.微信:
                    area = bll.GetArea(dr["所在地"].ToString());
                    mediaInfo =new Entities.Media.MediaWeixin()
                    {
                        Number = dr["微信号"].ToString(),
                        Name = dr["微信名称"].ToString(),
                        HeadIconURL = dr["头像"].ToString(),
                        TwoCodeURL = dr["二维码"].ToString(),
                        FansCount = GetDataRowValue<int>(dr,"粉丝数"),
                        CategoryID = bll.GetDictID(DictTypeEnum.行业分类一微信, GetDataRowValue<string>(dr, "行业分类")),
                        LevelType = dr["媒体级别"].ToString().Trim().Equals("1") ? 4001 : 4002,
                        IsAuth = GetDataRowValue<bool>(dr, "微信是否认证"),
                        Sign = GetDataRowValue<string>(dr, "描述／签名"),
                        IsReserve = GetDataRowValue<bool>(dr, "预约情况"),
                        ProvinceID = area.Item1,
                        CityID = area.Item2,
                        Status = 0
                    };
                    if (mediaInfo.Sign.Length > 100)
                        mediaInfo.Sign = mediaInfo.Sign.Substring(0,98)+"...";
                    mediaInfo.CategoryID = mediaInfo.CategoryID == -1 ? 20015 : mediaInfo.CategoryID;
                    break;
                case MediaTypeEnum.微博:
                    area = bll.GetArea(dr["所在地"].ToString());
                    mediaInfo = new Entities.Media.MediaWeibo()
                    {
                        Number = dr["微博账号"] == DBNull.Value ? dr["微博昵称"].ToString() : dr["微博账号"].ToString(),
                        Name = dr["微博昵称"].ToString(),
                        HeadIconURL = dr["头像"].ToString(),
                        Sex = GetSex(dr["性别"].ToString()),
                        FansCount = GetDataRowValue<int>(dr, "粉丝数"),
                        FansSex = GetSex(dr["粉丝性别"].ToString()),
                        LevelType = dr["媒体级别"].ToString().Trim().Equals("1") ? 4001 : 4002,
                        CategoryID = bll.GetDictID(DictTypeEnum.行业分类一微博, GetDataRowValue<string>(dr, "行业分类")),
                        AuthType = bll.GetDictID(DictTypeEnum.微博认证,GetDataRowValue<string>(dr,"微博认证")),
                        OrderRemark = GetWBOrderRemark(dr),
                        Sign = GetDataRowValue<string>(dr, "描述／签名"),
                        IsReserve = GetDataRowValue<bool>(dr, "预约情况"),
                        ProvinceID = area.Item1,
                        CityID = area.Item2,
                        Status = 0
                    };
                    if (mediaInfo.Sign.Length > 100)
                        mediaInfo.Sign = mediaInfo.Sign.Substring(0,98)+"...";
                    mediaInfo.CategoryID = mediaInfo.CategoryID == -1 ? 19023 : mediaInfo.CategoryID;
                    break;
                case MediaTypeEnum.视频:
                    platform = bll.GetDictID(DictTypeEnum.所属平台一视频, dr["所属平台"].ToString());
                    area = bll.GetArea(dr["所在地"].ToString());
                    mediaInfo = new Entities.Media.MediaVideo()
                    {
                        Platform = platform,
                        Number = dr["平台账号"].ToString(),
                        Name = dr["昵称"].ToString(),
                        HeadIconURL = dr["头像"].ToString(),
                        LevelType = dr["媒体级别"].ToString().Trim().Equals("1") ? 4001 : 4002,
                        CategoryID = bll.GetDictID(DictTypeEnum.行业分类一视频直播, GetDataRowValue<string>(dr, "行业分类")),
                        Profession = bll.GetDictID(DictTypeEnum.职业信息一视频直播, GetDataRowValue<string>(dr, "职业")),
                        FansCount = GetDataRowValue<int>(dr, "粉丝数"),
                        Sex = GetSex(dr["性别"].ToString()),
                        IsReserve = GetDataRowValue<bool>(dr,"预约情况"),
                        AuthType = dr["是否认证"].ToString().Equals("Y") ? 1 : 0,
                        ProvinceID = area.Item1,
                        CityID = area.Item2,
                        Status = 0
                    };
                    mediaInfo.CategoryID = mediaInfo.CategoryID == -1 ? 25024 : mediaInfo.CategoryID;
                    break;
                case MediaTypeEnum.直播:
                    platform = bll.GetDictID(DictTypeEnum.所属平台一直播, dr["所属平台"].ToString());
                    area = bll.GetArea(dr["所在地"].ToString());
                    mediaInfo = new Entities.Media.MediaBroadcast()
                    {
                        Number = dr["平台账号"].ToString(),
                        Name = dr["昵称"].ToString(),
                        HeadIconURL = dr["头像"].ToString(),
                        Platform = platform,
                        LevelType = dr["媒体级别"].ToString().Trim().Equals("1") ? 4001 : 4002,
                        CategoryID = bll.GetDictID(DictTypeEnum.行业分类一视频直播, GetDataRowValue<string>(dr, "行业分类")),
                        Profession = bll.GetDictID(DictTypeEnum.职业信息一视频直播, GetDataRowValue<string>(dr, "职业")),
                        FansCount = GetDataRowValue<int>(dr, "粉丝数"),
                        Sex = GetSex(dr["性别"].ToString()),
                        IsReserve = GetDataRowValue<bool>(dr,"预约情况"),
                        ProvinceID = area.Item1,
                        CityID = area.Item2,
                        IsAuth = dr["是否认证"].ToString().Equals("Y") ? 1 : 0,
                        Status = 0
                    };
                    mediaInfo.CategoryID = mediaInfo.CategoryID == -1 ? 25024 : mediaInfo.CategoryID;
                    break;
                case MediaTypeEnum.APP:
                    area = bll.GetArea(dr["所在地"].ToString());
                    mediaInfo = new Entities.Media.MediaPcApp()
                    {   
                        Name = dr["媒体名称"].ToString(),
                        ProvinceID = area.Item1,
                        CityID = area.Item2,
                        CategoryID = bll.GetDictID(DictTypeEnum.行业分类一APP, GetDataRowValue<string>(dr, "行业分类")),
                        Status = 0
                    };
                    mediaInfo.CategoryID = mediaInfo.CategoryID == -1 ? 22026 : mediaInfo.CategoryID;
                    break;
            }
            mediaInfo.Source = source;
            mediaInfo.CreateUserID = userID;
            mediaInfo.LastUpdateUserID = userID;
            mediaInfo.CreateTime = DateTime.Now;
            mediaInfo.LastUpdateTime = DateTime.Now;
            return mediaInfo;
        }

        public static PublishBasicInfo ConvertToPublishBasic(DataRow dr,MediaTypeEnum mediaType){

            PublishBasicInfo basic = new Entities.Publish.PublishBasicInfo()
            {
                Status = AuditStatusEnum.已通过,
                PublishStatus = PublishStatusEnum.已上架,
                BeginTime = DateTime.Now,
                EndTime = DateTime.Parse(defaultPubEndDate),
                CreateUserID = userID,
                LastUpdateUserID = userID,
                CreateTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                SaleDiscount= GetDataRowValue<decimal>(dr,"销售折扣"),
                PurchaseDiscount = GetDataRowValue<decimal>(dr,"采购折扣")
            };
            if (mediaType.Equals(MediaTypeEnum.APP)) {
                string beginTime = dr["刊例开始日期"].ToString();
                string endTime = dr["刊例结束日期"].ToString();
                if (string.IsNullOrWhiteSpace(beginTime))
                    beginTime = DateTime.Now.ToString("yyyy-MM-dd");
                if (string.IsNullOrWhiteSpace(endTime))
                    endTime = defaultPubEndDate;
                basic.BeginTime = DateTime.Parse(beginTime);
                basic.EndTime = DateTime.Parse(endTime);
            }
            return basic;
        }

        public static dynamic ConvertToInteraction(DataRow dr, MediaTypeEnum mediaType) {

            dynamic interaction = null;
            switch (mediaType) {
                case MediaTypeEnum.微信:
                    interaction = new InteractionWeixin
                    {
                        ReferReadCount = GetDataRowValue<int>(dr, "参考阅读数"),
                        AveragePointCount = GetDataRowValue<int>(dr, "平均点赞数"),
                        OrigArticleCount = GetDataRowValue<int>(dr, "原创文章数"),
                        UpdateCount = GetDataRowValue<int>(dr, "周更新频率"),
                        MaxinumReading = GetDataRowValue<int>(dr,"最高阅读数"),
                        MoreReadCount = GetDataRowValue<int>(dr,"10W+阅读量的文章数")
                    };
                    break;
                case MediaTypeEnum.微博:
                    interaction = new Entities.Interaction.InteractionWeibo()
                    {
                        AverageForwardCount = GetDataRowValue<int>(dr, "平均转发数"),
                        AverageCommentCount = GetDataRowValue<int>(dr, "平均评论数"),
                        AveragePointCount = GetDataRowValue<int>(dr, "平均点赞数")
                    };
                    break;
                case MediaTypeEnum.视频:
                    interaction = new Entities.Interaction.InteractionVideo()
                    {
                        AveragePlayCount = GetDataRowValue<int>(dr,"平均播放数"),
                        AverageCommentCount = GetDataRowValue<int>(dr,"平均评论数"),
                        AveragePointCount = GetDataRowValue<int>(dr,"平均点赞数")
                    };
                    break;
                case MediaTypeEnum.直播:
                    interaction = new Entities.Interaction.InteractionBroadcast()
                    {
                        MaximumAudience = GetDataRowValue<int>(dr,"最高观众数"),
                        AverageAudience = GetDataRowValue<int>(dr,"平均观众数"),
                        CumulateReward = GetDataRowValue<int>(dr,"累计打赏数")
                    };
                    break;
                case MediaTypeEnum.APP:
                    return null;
            }
            interaction.CreateUserID = userID;
            interaction.CreateTime = DateTime.Now;
            interaction.LastUpdateUserID = userID;
            interaction.LastUpdateTime = DateTime.Now;
            return interaction;
        }

        public static List<PublishDetailInfo> ConvertToPublishDetailList(DataRow dr,MediaTypeEnum mediaType) {

            List<PublishDetailInfo> list = new List<PublishDetailInfo>();
            switch (mediaType) {
                case MediaTypeEnum.微信:
                    #region
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.单图文,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.发布,
                        Price = GetDataRowValue<decimal>(dr, "单图文软广发布"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.单图文,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.原创发布,
                        Price = GetDataRowValue<decimal>(dr, "单图文软广原创+发布"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.多图文头条,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.发布,
                        Price = GetDataRowValue<decimal>(dr, "多图文头条软广发布"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.多图文头条,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.原创发布,
                        Price = GetDataRowValue<decimal>(dr, "多图文头条软广原创+发布"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.多图文第二条,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.发布,
                        Price = GetDataRowValue<decimal>(dr, "多图文第二条软广发布"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.多图文第二条,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.原创发布,
                        Price = GetDataRowValue<decimal>(dr, "多图文第二条软广原创+发布"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.多图文3N条,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.发布,
                        Price = GetDataRowValue<decimal>(dr, "多图文第3—n条软广发布"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdPositionMapping.多图文3N条,
                        ADPosition2 = (int)AdTypeMapping.软广,
                        ADPosition3 = (int)AdFormality3.原创发布,
                        Price = GetDataRowValue<decimal>(dr, "多图文第3—n条软广原创+发布"),
                    });
                    #endregion
                    break;
                case MediaTypeEnum.微博:
                    #region
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdTypeMapping.硬广,
                        ADPosition2 = (int)AdFormality4.直发,
                        Price = GetDataRowValue<decimal>(dr, "硬广直发价"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdTypeMapping.硬广,
                        ADPosition2 = (int)AdFormality4.转发,
                        Price = GetDataRowValue<decimal>(dr, "硬广转发价"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdTypeMapping.软广,
                        ADPosition2 = (int)AdFormality4.直发,
                        Price = GetDataRowValue<decimal>(dr, "软广直发价"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdTypeMapping.软广,
                        ADPosition2 = (int)AdFormality4.转发,
                        Price = GetDataRowValue<decimal>(dr, "软广转发价"),
                    });
                    #endregion
                    break;
                case MediaTypeEnum.视频:
                    #region
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        //直发价
                        ADPosition1 = (int)AdFormality4.直发,
                        Price = GetDataRowValue<decimal>(dr,"直发价"),
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        //直发价
                        ADPosition1 = (int)AdFormality4.原创发布,
                        Price = GetDataRowValue<decimal>(dr,"原创+直发"),
                    });        
                    #endregion
                    break;
                case MediaTypeEnum.直播:
                    #region
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdFormality5.活动现场直播,
                        Price = GetDataRowValue<decimal>(dr,"现场直播价")
                    });
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = (int)AdFormality5.直播广告植入,
                        Price = GetDataRowValue<decimal>(dr, "直播广告值入价")
                    });
                    #endregion
                    break;
                case MediaTypeEnum.APP:
                    #region
                    list.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = dr["售卖方式"].ToString().ToUpper().Equals("CPM") ? (int)SaleMode.CPM : (int)SaleMode.CPD,
                        Price = GetDataRowValue<decimal>(dr,"价格"),
                        IsCarousel = dr["价格单位"].ToString().Contains("轮播")
                    });
                    #endregion
                    break;
            }
            list = list.Where(i => !i.Price.Equals(0M)).ToList();//去掉0的
            list.ForEach(i => {
                i.PublishStatus = PublishStatusEnum.已上架;
                i.CreateUserID = userID;
                i.CreateTime = DateTime.Now;
            });
            return list;
        }

        public static dynamic GetDataRowValue<T>(DataRow dr, string colName)
        {
            string value = dr[colName].ToString().Trim();
            if (typeof(int).Equals(typeof(T)))
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 0; 
                value = value.Replace(",","");
                return int.Parse(value);
            }
            if (typeof(decimal).Equals(typeof(T)))
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 0m;
                value = value.Replace(",", "");
                return decimal.Parse(value);
            }
            if (typeof(bool).Equals(typeof(T)))
            {
                if (string.IsNullOrWhiteSpace(value))
                    return false;
                return value.ToUpper().Equals("Y");
            }
            return value;
        }

        public static string GetSex(string sex)
        {
            switch (sex)
            {
                case "男":
                    return "1";
                case "女":
                    return "0";
                default:
                    return "-1";
            }
        }

        public static List<int> GetSysPlatform(string sysPlatform)
        {
            var list = new List<int>();
            if (string.IsNullOrWhiteSpace(sysPlatform)) return list;
            var sp = sysPlatform.Split('&');
            foreach (var item in sp)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                if (item.Equals("Android"))
                {
                    list.Add((int)SysSysPlatform.Android);
                }
                if (item.Equals("IOS"))
                {
                    list.Add((int)SysSysPlatform.IOS);
                }
            }
            return list;
        }

        public static List<int> GetThrMonitor(DataRow dr)
        {
            var list = new List<int>();
            if (dr["第三方曝光监测"].ToString().ToUpper().Equals("Y"))
                list.Add((int)ThrMonitor.曝光监测);
            if (dr["第三方点击监测"].ToString().ToUpper().Equals("Y"))
                list.Add((int)ThrMonitor.点击监测);
            return list;
        }

        public static string GetWBOrderRemark(DataRow dr)
        {
            string res = string.Empty;
            if (dr["可接硬广"].ToString().ToUpper().Equals("Y"))
                res += "18001";
            if (dr["可接特殊活动"].ToString().ToUpper().Equals("Y"))
            {
                if (res.Length > 0)
                    res += ",";
                res += "18002";
            }
            return res;
        }

    }

}
