using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Dal.Media;
using XYAuto.ITSC.Chitunion2017.Entities.AdTemplate;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.DataImport
{
    public class APP_MediaHelper
    {
        public static readonly APP_MediaHelper Instance = new APP_MediaHelper();
        ILog logger;

        private readonly string excelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["APP_ExcelPath"]);
        private readonly int importTempleteUserID = int.Parse(ConfigurationManager.AppSettings["APP_ImportTempleteUserID"]);
        private readonly int importPublishUserID = int.Parse(ConfigurationManager.AppSettings["APP_ImportPublishUserID"]);
        private readonly bool APP_isDelete = ConfigurationManager.AppSettings["APP_isDelete"] == "1" ? true : false;
        protected APP_MediaHelper()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = LogManager.GetLogger(typeof(Program));
        }

        /// <summary>
        /// 导入APP类型，媒体下，模板、刊例、广告数据
        /// </summary>
        public void Import_AppData()
        {
            //Test();
            //return;
            logger.Info("[Import_AppData]程序开始运行...");
            logger.Info("[Import_AppData]Excel位置:" + excelPath);
            logger.Info("[Import_AppData]统一导入APP模板的UserID：" + importTempleteUserID + ",APP刊例的UserID：" + importPublishUserID);
            if (File.Exists(excelPath))
            {
                DataSet ds = ConvertExcelToDataSet(new string[] { "广告位模板", "广告块价格" });

                if (ds != null && ds.Tables.Count > 0)
                {                    
                    DataView dataView = ds.Tables["广告位模板"].DefaultView;
                    DataTable dt_mediaName = dataView.ToTable(true, "媒体名称");
                    int tempCount = 0;
                    tempCount = ds.Tables["广告位模板"].Rows.Count;
                    int tempSuccessCount = 0;
                    int pubCount = 0;
                    //pubCount = ds.Tables["广告块价格"].Rows.Count;
                    int pubSuccessCount = 0;
                    int tempExistCount = 0;
                    string tempNameIDS = string.Empty;
                    foreach (DataRow dr in dt_mediaName.Rows)
                    {
                        string mediaName = dr["媒体名称"].ToString().Trim();
                        BLL.Loger.Log4Net.Info($"[Import_AppData]媒体：{mediaName}导入开始");
                        var baseMedia = AdTemplateRelationDataProvider.GetBaseMedia(mediaName);
                        if (baseMedia == null)
                        {
                            BLL.Loger.Log4Net.Info($"[Import_AppData]媒体：{mediaName}在基表中不存在");
                            continue;
                        }
                        int aeMediaID = 0;
                        aeMediaID = XYAuto.ITSC.Chitunion2017.DataImport.DAL.Util.Instance.QueryMediaIDAE(baseMedia.Name);
                        if (aeMediaID == -2)
                        {
                            BLL.Loger.Log4Net.Info($"[Import_AppData]媒体名称：{baseMedia.Name}在副表中不存在");
                            continue;
                        }
                        int rownum = 0;
                        dataView = ds.Tables["广告位模板"].DefaultView;
                        dataView.RowFilter = string.Format("媒体名称='{0}'", mediaName);
                        DataTable dt_Templete = dataView.ToTable();
                        foreach (DataRow dr_Templete in dt_Templete.Rows)
                        {
                            rownum++;
                            string tempName = dr_Templete["广告模板名称"].ToString().Trim();
                            logger.Info(string.Format("[Import_AppData]APP媒体，媒体名称={0}，广告模板名称={1},记录数：{2}", mediaName, tempName,rownum.ToString()));
                            int tempID = -2;
                            tempID = XYAuto.ITSC.Chitunion2017.DataImport.DAL.Util.Instance.p_VerifyADTemplateByName(tempName, baseMedia.RecID,APP_isDelete);
                            if (tempID > 0)
                            {                                
                                if(!APP_isDelete)
                                {
                                    tempExistCount++;
                                    tempNameIDS += $"[{tempName}]:{tempID},";
                                    BLL.Loger.Log4Net.Info($"[Import_AppData]媒体BaseMediaID:{baseMedia.RecID},媒体名称：{baseMedia.Name},广告模板名称：{tempName}广告模板ID：{tempID},存在跳过");
                                    continue;
                                }   
                                else
                                    BLL.Loger.Log4Net.Info($"[Import_AppData]媒体BaseMediaID:{baseMedia.RecID},媒体名称：{baseMedia.Name},广告模板名称：{tempName}广告模板ID：{tempID},删除成功");
                            }
                            //添加APP模板数据
                            if (Add_AppTemplete(baseMedia, dr_Templete, out tempID))
                                tempSuccessCount++;
                            else
                            {
                                continue;
                            }

                            dataView = ds.Tables["广告块价格"].DefaultView;
                            dataView.RowFilter = string.Format("媒体名称='{0}' And 广告名称='{1}'", mediaName, tempName);
                            DataTable dt_Publish = dataView.ToTable();

                            pubCount+= dt_Publish.Rows.Count;
                            //添加APP刊例数据
                            if (Add_AppPublish(aeMediaID, tempID, dt_Publish))
                                pubSuccessCount += dt_Publish.Rows.Count;

                        }
                        BLL.Loger.Log4Net.Info($"[Import_AppData]媒体：{mediaName}导入结束");
                    }
                    logger.Info($"[Import_AppData]************************导入结束**************************************");
                    logger.Info(string.Format("[Import_AppData]模板共导入成功{0}条记录,跳过已存在{1}条,失败{2}条", tempSuccessCount, tempExistCount, tempCount - tempSuccessCount - tempExistCount));
                    if (tempNameIDS.Contains(","))
                    {
                        tempNameIDS = tempNameIDS.Substring(0, tempNameIDS.Length - 1);
                        logger.Info($"[Import_AppData]已存在模板{tempNameIDS}");
                    }
                    logger.Info(string.Format("[Import_AppData]价格共导入成功{0}条记录,失败{1}条", pubSuccessCount, pubCount - pubSuccessCount));
                }
            }
        }

        /// <summary>
        /// 添加APP刊例数据
        /// </summary>
        /// <param name="dt_Publish"></param>
        private bool Add_AppPublish(int aeMediaID, int tempID, DataTable dt_Publish)
        {            
            Entities.DTO.ModifyPublishReqDTO dto = null;            
            int i = 0;
            string tempName = string.Empty;           
            List<Entities.DTO.ADPrice> priceList = new List<Entities.DTO.ADPrice>();
            foreach (DataRow dr_Publish in dt_Publish.Rows)
            {
                i++;
                BLL.Loger.Log4Net.Info($"[Add_AppPublish]副表广告名称：{dr_Publish["广告名称"].ToString().Trim()}，添加广告价格记录数：{i}");                
                //刊例
                decimal purchaseDiscount = 0;
                decimal.TryParse(dr_Publish["采购折扣"].ToString().Trim(), out purchaseDiscount);
                decimal saleDiscount = 0;
                decimal.TryParse(dr_Publish["销售折扣"].ToString().Trim(), out saleDiscount);

                //价格            
                int aDStyle = 0;
                aDStyle = XYAuto.ITSC.Chitunion2017.DataImport.DAL.Util.Instance.QueryADStyleRecid(tempID, dr_Publish["广告样式"].ToString().Trim());
                if (aDStyle == -2)
                    BLL.Loger.Log4Net.Info($"[Add_AppPublish]广告样式：{dr_Publish["广告样式"].ToString().Trim()}:不存在");
                int carouselNumber = 0;
                int.TryParse(dr_Publish["轮播"].ToString().Trim(), out carouselNumber);
                int salePlatForm = 0;
                salePlatForm = AdTemplateRelationDataProvider.GetSalePlatform(dr_Publish["售卖平台"].ToString().Trim());
                if (salePlatForm == 0)
                    BLL.Loger.Log4Net.Info($"[Add_AppPublish]售卖平台：{dr_Publish["售卖平台"].ToString().Trim()}:不存在");

                int saleType = 0;
                saleType = AdTemplateRelationDataProvider.GetSaleType(dr_Publish["售卖方式"].ToString().Trim());
                if (saleType == 0)
                    BLL.Loger.Log4Net.Info($"[Add_AppPublish]售卖方式：{dr_Publish["售卖方式"].ToString().Trim()}:不存在");
                int clickCount = 0;
                int.TryParse(dr_Publish["点击量"].ToString().Trim(), out clickCount);
                int exposureCount = 0;
                string strexposureCount = dr_Publish["曝光量"].ToString().Trim();
                if (!string.IsNullOrEmpty(strexposureCount) && !int.TryParse(strexposureCount, out exposureCount))
                    BLL.Loger.Log4Net.Info($"[Add_AppPublish]曝光量：{dr_Publish["曝光量"].ToString().Trim()}：价格导入转换失败");
                decimal salePrice_Holiday = 0;
                decimal.TryParse(dr_Publish["销售价（节假日）"].ToString().Trim(), out salePrice_Holiday);
                decimal pubPrice_Holiday = 0;
                decimal.TryParse(dr_Publish["刊例价（节假日）"].ToString().Trim(), out pubPrice_Holiday);
                decimal salePrice = 0;
                decimal.TryParse(dr_Publish["销售价（工作日）"].ToString().Trim(), out salePrice);
                decimal pubPrice = 0;
                decimal.TryParse(dr_Publish["刊例价（工作日）"].ToString().Trim(), out pubPrice);

                int groupID = 0;
                groupID = XYAuto.ITSC.Chitunion2017.DataImport.DAL.Util.Instance.QuerySaleAreaGroupID(tempID, dr_Publish["售卖区域"].ToString().Trim());
                if (groupID == -2)
                    BLL.Loger.Log4Net.Info($"[Add_AppPublish]售卖区域：{dr_Publish["售卖区域"].ToString().Trim()}:不存在");
                if (i == 1)
                {
                    tempName = dr_Publish["广告名称"].ToString().Trim();
                    BLL.Loger.Log4Net.Info($"[Add_AppPublish]广告名称：{tempName}，价格导入开始");
                    dto = new Entities.DTO.ModifyPublishReqDTO()
                    {
                        Pwd= "xingyuanimportdata",
                        Publish = new Entities.DTO.ModifyPublish()
                        {
                            PubID = 0,
                            BeginTime = new DateTime(2017, 1, 1),
                            EndTime = new DateTime(2017, 9, 30),
                            HasHoliday = salePrice_Holiday == 0 ? false : true,
                            ImgUrl = "/UploadFiles/2017/7/6/20/默认图片$989f2579-977b-47d1-8d83-81aabb94503a.png",
                            IsAppointment = false,
                            MediaID = aeMediaID,
                            MediaType = 14002,
                            PurchaseDiscount = purchaseDiscount,
                            SaleDiscount = saleDiscount,
                            TemplateID = tempID,
                            CreateUserID=importPublishUserID
                        },
                        PriceList = priceList
                    };
                }
                priceList.Add(new Entities.DTO.ADPrice()
                {
                    ADStyle = aDStyle,
                    CarouselNumber = carouselNumber,
                    SalePlatform = salePlatForm,
                    SaleType = saleType,
                    ClickCount = clickCount,
                    ExposureCount = exposureCount,
                    PubPrice = pubPrice,
                    SalePrice = salePrice,
                    PubPrice_Holiday = pubPrice_Holiday,
                    SalePrice_Holiday = salePrice_Holiday,
                    SaleArea = groupID
                });

            }
            
            string msg = string.Empty;
            int pubID = 0;
            if (dto != null)
            {
                var res = BLL.PublishInfo.Instance.AddPublishBasicInfoV1_1(dto, ref msg, ref pubID);
                if (res == true)
                {
                    logger.Info($"[Add_AppPublish]广告名称：{tempName}，价格导入成功");
                    return true;
                }
                else
                    logger.Info($"[Add_AppPublish]广告名称：{tempName}，价格导入失败：{msg}");
            }

            return false;
        }
        
        /// <summary>
        /// 添加APP模板数据
        /// </summary>
        /// <param name="dr_Templete"></param>
        private bool Add_AppTemplete(Entities.Media.MediaBasePCAPP baseMedia,DataRow dr_Templete,out int tempID)
        {
            string tempName = dr_Templete["广告模板名称"].ToString().Trim();
            BLL.Loger.Log4Net.Info($"[Add_AppTemplete]广告模板名称：{tempName}，模板导入开始");
            tempID = -2;
            var adForm = AdTemplateRelationDataProvider.GetADForm(dr_Templete["广告形式"].ToString().Trim());
            if (adForm == 0)
            {
                BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体：{baseMedia.Name},模板广告形式：{dr_Templete["广告形式"].ToString().Trim()}模板导入不存在");
                return false;
            }

            //广告样式
            List<AdTempStyleDto> adTempStyleList = new List<AdTempStyleDto>();
            string adStyle = dr_Templete["广告样式"].ToString().Trim();
            foreach (var adTempStyle in adStyle.Split('；'))
            {
                if (!string.IsNullOrEmpty(adTempStyle))
                    adTempStyleList.Add(new AdTempStyleDto() {
                        BaseMediaID = baseMedia.BaseMediaID,
                        AdStyle = adTempStyle,
                        CreateUserId = importTempleteUserID
                    });
            }

            int carouselCount = 0;
            int.TryParse(dr_Templete["轮播数"].ToString().Trim(), out carouselCount);
            BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体：{baseMedia.Name},轮播数：{carouselCount}");

            string salePlatForm = dr_Templete["售卖平台"].ToString().Trim();
            int sellingPlatform = 0; //AdTemplateRelationDataProvider.GetSellingPlatform(dr_Templete["售卖平台"].ToString().Trim());
            if (salePlatForm.Contains("；"))
            {
                string[] salePlatArray = salePlatForm.Split('；');
                int iSale = 0;
                foreach (var itemSalePlat in salePlatForm.Split('；'))
                {
                    iSale++;
                    int itmp = 0;
                    itmp = AdTemplateRelationDataProvider.GetSellingPlatform(itemSalePlat);
                    if (AdTemplateRelationDataProvider.GetSellingPlatform(itemSalePlat) == 0)
                    {
                        BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体：{baseMedia.Name},模板售卖平台：{itemSalePlat}模板导入不存在");
                        return false;
                    }
                    if (iSale == 1)
                    {
                        sellingPlatform = itmp;
                    }
                    else
                    {
                        sellingPlatform = sellingPlatform | itmp;
                    }
                }
            }
            else
            {
                sellingPlatform = AdTemplateRelationDataProvider.GetSellingPlatform(salePlatForm);
            }

            string drselling = dr_Templete["售卖方式"].ToString().Trim();
            int sellingMode = 0;// = AdTemplateRelationDataProvider.GetDicSaleMode(drselling);
            if (drselling.Contains("；"))
            {
                string[] arraySelling = drselling.Split('；');
                int itmp1 = 0;
                itmp1 = AdTemplateRelationDataProvider.GetDicSaleMode(arraySelling[0]);
                if (itmp1 == 0)
                {
                    BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体：{baseMedia.Name},模板售卖方式：{arraySelling[0]}模板导入不存在");
                    return false;
                }
                int itmp2 = 0;
                itmp2 = AdTemplateRelationDataProvider.GetDicSaleMode(arraySelling[1]);
                if (itmp2 == 0)
                {
                    BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体：{baseMedia.Name},模板售卖方式：{arraySelling[1]}模板导入不存在");
                    return false;
                }
                sellingMode = itmp1 | itmp2;
            }
            else
            {
                sellingMode = AdTemplateRelationDataProvider.GetDicSaleMode(drselling);
                if (sellingMode == 0)
                {
                    BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体：{baseMedia.Name},模板售卖方式：{drselling}模板导入不存在");
                    return false;
                }
            }
            

            int adDisplayLength = 0;
            int.TryParse(dr_Templete["起投天数"].ToString().Trim(), out adDisplayLength);

            //素材规格
            //展示逻辑

            //售卖区域
            //全国；
            //一级城市（北京、上海、广州、深圳、天津、南京、杭州、成都、重庆、西安、武汉）；
            //二级城市（郑州、长沙、合肥、济南、佛山、无锡、苏州、石家庄、青岛、大连、常州、温州、南昌、厦门、福州、昆明、太原、潍坊、泉州、南通、沈阳、东莞、金华、柳州）
            List<AdSaleAreaGroupDto> salareaList = new List<AdSaleAreaGroupDto>();
            int groupType = 0;
            string strSalearea = dr_Templete["售卖区域"].ToString().Trim();
            foreach (var saleArea in strSalearea.Split('；'))
            {
                groupType = AdTemplateRelationDataProvider.GetDicGroupType(saleArea);                
                if (groupType == 0 || groupType == -1)
                {
                    salareaList.Add(new AdSaleAreaGroupDto()
                    {
                        GroupName = saleArea.Trim(),
                        GroupType = groupType
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(saleArea))
                        break;                    

                    var groupArray = saleArea.Split('（');
                    AdSaleAreaGroupDto group = new AdSaleAreaGroupDto()
                    {
                        GroupName = groupArray[0].Trim(),
                        GroupType = 1
                    };
                    group.DetailArea = new List<AdSaleAreaGroupDetailDto>();
                    if (groupArray[1].Contains("）"))
                        groupArray[1] = groupArray[1].Substring(0, groupArray[1].Length - 1);
                    foreach (var city in groupArray[1].Split('、'))
                    {
                        AreaInfoEntity area = AdTemplateRelationDataProvider.GetCity(city);
                        int cityid = 0;
                        int pid = 0;
                        if (area != null)
                        {
                            int.TryParse(area.AreaID, out cityid);
                            int.TryParse(area.PID, out pid);
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体：{baseMedia.Name},媒体baseMeidaID:{baseMedia.RecID},模板售卖城市：{city}模板导入不存在");
                            return false;
                        }
                        group.DetailArea.Add(new AdSaleAreaGroupDetailDto() {
                            ProvinceId = pid,
                            CityId = cityid
                        });
                    }
                    salareaList.Add(group);
                }
            }
            if (strSalearea.Contains("（"))
            {
                salareaList.Add(new AdSaleAreaGroupDto()
                {
                    GroupName = "其他",
                    GroupType = -1
                });
            }
            var _requestMediaDto = new RequestMediaDto()
            {
                BusinessType = 15000,
                OperateType = 1,
                Temp = new RequestTemplateDto()
                {
                    BaseMediaId = baseMedia.RecID,
                    OriginalFile = "/UploadFiles/2017/7/6/20/默认图片$989f2579-977b-47d1-8d83-81aabb94503a.png",
                    AdTemplateName = tempName,
                    AdForm = adForm,
                    CarouselCount = carouselCount,
                    SellingPlatform = sellingPlatform,
                    SellingMode = sellingMode,
                    AdDisplayLength = adDisplayLength,
                    AdDescription = dr_Templete["素材规格"].ToString().Trim(),
                    AdDisplay = dr_Templete["展示逻辑"].ToString().Trim(),
                    AdLegendUrl = string.IsNullOrEmpty(dr_Templete["示例图"].ToString().Trim())==true? "/UploadFiles/2017/7/6/20/默认图片$989f2579-977b-47d1-8d83-81aabb94503a.png" : dr_Templete["示例图"].ToString().Trim(),
                    Remarks = dr_Templete["备注"].ToString().Trim(),
                    CreateUserId = importTempleteUserID,
                    CreateTime = DateTime.Now,
                    AuditStatus = 48002,
                    AdSaleAreaGroup = salareaList,
                    AdTempStyle = adTempStyleList
                }
            };

            ConfigEntity _configEntity = new ConfigEntity() {
                RoleTypeEnum = BLL.Publish.RoleEnum.YunYingOperate,
                CreateUserId=importTempleteUserID,
                CureOperateType= BLL.Media.Dto.OperateType.Insert
            };
            BLL.AutoMapperConfig.MediaMapperConfig.Configure();
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            if(retValue.HasError)
            {
                BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体baseMeidaID:{baseMedia.RecID},媒体名称：{baseMedia.Name},广告模板名称：{tempName}，模板导入失败：{retValue.Message}");
                return false;
            }
            else
            {
                BLL.Loger.Log4Net.Info($"[Add_AppTemplete]媒体baseMeidaID:{baseMedia.RecID},媒体名称：{baseMedia.Name},广告模板名称：{tempName},模板ID:{retValue.ReturnObject}，模板导入成功");
                tempID = (int)retValue.ReturnObject;
            }
            return true;
        }

        public DataSet ConvertExcelToDataSet(string[] sheetNames)
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
                    dt.Columns.Add(cell == null ? string.Empty : cell.ToString().Trim());
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

        public void Test()
        {
            int tmp = -2;
            tmp = AdTemplateRelationDataProvider.GetDicSaleMode("a");

            var areaList = XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1.AdTemplateRelationDataProvider.GetAreaList();           
            var ctiy = areaList.Find(x => x.AreaName == "北京");

            var media = AdTemplateRelationDataProvider.GetBaseMedia("雪球");
        }
    }

    public class AdTemplateRelationDataProvider
    {
        public static void Insert(RequestMediaDto reqDto,out string errormsg)
        {
            errormsg = string.Empty;
            ConfigEntity _configEntity = new ConfigEntity() {
                RoleTypeEnum=BLL.Publish.RoleEnum.YunYingOperate,
                CreateUserId= 1225,
                CureOperateType=BLL.Media.Dto.OperateType.Insert
            };         
            
            var retValue = new MediaOperateProxy(reqDto, _configEntity).Excute();
            if (retValue.HasError)
                errormsg = retValue.Message;
        }

        public static int GetADForm(string dataName)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>() {
                { "banner（轮播）",51001},
                { "信息流（feeds流）",51002},
                { "启动页（开机画面）",51003},
                { "异型",51004},
                { "通栏",51005},
                { "焦点图（幻灯片）",51006},
                { "消息（推送、通知）",51007},
                { "冠名",51008},
                { "帖子（话题、文章发布、问答）",51009},
                { "弹窗",51010},
                { "文字链",51011},
                { "背景图",51012}
            };
            return dict.FirstOrDefault(x => x.Key == dataName).Value;
        }

        /// <summary>
        /// 获取到售卖平台的组合
        /// </summary>
        /// <param name="dataNumber"></param>
        /// <returns>12001|Android,12002|IOS</returns>
        public static int GetSellingPlatform(string dataName)
        {
            var dic = GetDicSellingPlatformValue();
            var dicValue = dic.FirstOrDefault(s => s.Key == dataName);            

            return dicValue.Value;
        }
        

        /// <summary>
        /// 获取售卖平台的集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicSellingPlatformValue()
        {
            return new Dictionary<string, int>() {
               {"Android",   1},
               {"IOS" ,   2},
               {"Android&IOS",4 },
               {"PAD",8 },
               {"Gphone",16},
               {"Phone",32}
            };            
        }

        public static int GetSalePlatform(string name)
        {
            var dic = new Dictionary<string, int>() {
               {"Android",   12001},
               {"IOS" ,   12002},
               {"Android&IOS",12003 },
               {"PAD",12004 },
               {"Gphone",12005},
               {"Phone",12006}
            };

            return dic.FirstOrDefault(s => s.Key == name).Value;
        }
        public static int GetSaleType(string name)
        {
            var dic = new Dictionary<string, int>() {
               {"CPD",   11001},
               {"CPM" ,   11002}
            };

            return dic.FirstOrDefault(s => s.Key == name).Value;
        }

        /// <summary>
        /// 获取到售卖方式的组合
        /// </summary>
        /// <param name="dataNumber"></param>
        /// <returns>11001|CPD,11002|CPM</returns>
        public static int GetDicSaleMode(string dataName)
        {
            var dic = GetDicSaleModeValue();
            var dicValue = dic.FirstOrDefault(s => s.Key == dataName);            

            return dicValue.Value;
        }

        /// <summary>
        /// 获取售卖方式的集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicSaleModeValue()
        {
            return new Dictionary<string, int>()
            {
                {"CPD" ,1},
                {"CPM",2}
            };
        }

        public static int GetDicGroupType(string dataName)
        {
            Dictionary<string,int> dict = new Dictionary<string, int>()
            {
                {"全国",-2 },
                {"其他",-1 },
                {"普通", 1 }
            };

            var dicValue = dict.FirstOrDefault(x => x.Key == dataName);

            if (dicValue.Value == 0)
                return -2;

            if (dicValue.Value == -2)
                return 0;

            return dicValue.Value;
        }

        public static AreaInfoEntity GetCity(string cityname)
        {
            var areaList = XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1.AdTemplateRelationDataProvider.GetAreaList();           
            return areaList.Find(x => x.AreaName == cityname);
        }
        
        public static Entities.Media.MediaBasePCAPP GetBaseMedia(string name) => Dal.Media.MediaBasePCAPP.Instance.GetEntity(name);
    }
}
