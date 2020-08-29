using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.ThirdApi;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.ContentDistribute;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.WebService.Common;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:任务-领取  、查询相关
    /// </summary>
    public class TaskProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqOrderStorageDto _reqOrderStorageDto;
        private readonly ReqTaskReceiveDto _reqTaskReceiveDto;
        private Entities.LETask.LeTaskInfo _contexTaskInfo;
        //private readonly int _channelId = (int)LeOrderChannelTypeEnum.赤兔;

        public TaskProvider(ConfigEntity configEntity, ReqTaskReceiveDto reqTaskReceiveDto)
        {
            _configEntity = configEntity;
            _reqTaskReceiveDto = reqTaskReceiveDto;
        }

        public TaskProvider(ConfigEntity configEntity, ReqOrderStorageDto reqOrderStorageDto)
        {
            _configEntity = configEntity;
            _reqOrderStorageDto = reqOrderStorageDto;
        }

        public ReturnValue Receive()
        {
            //todo:黑名单校验，满足之后假装返回领取正确
            var retValue = ReceiveTaskVerifyBase();
            if (retValue.HasError)
            {
                return retValue;
            }
            if (string.IsNullOrWhiteSpace(_reqTaskReceiveDto.IP))
            {
                return CreateFailMessage(retValue, "10025", "领取失败");
            }
            retValue = VerifyAntiCheating(retValue, _reqTaskReceiveDto.IP, _configEntity.CreateUserId);
            if (retValue.HasError)
            {
                return CreateSuccessMessage(retValue, "0", "success");
            }
            if (_reqTaskReceiveDto.TaskType == (int)LeTaskTypeEnum.ContentDistribute)
            {
                return ReceiveByDistribute();
            }
            else if (_reqTaskReceiveDto.TaskType == (int)LeTaskTypeEnum.CoverImage)
            {
                return ReceiveByCoverImage();
            }
            else
            {
                return CreateFailMessage(new ReturnValue(), "1000", "请输入合法的TaskType");
            }
        }

        #region 领取任务-内容分发操作

        /// <summary>
        /// 领取任务-内容分发操作
        /// </summary>
        /// <returns></returns>
        private ReturnValue ReceiveByDistribute()
        {
            //todo:
            //1.判断用户是否已经领取过，userId + status + TaskId
            //2.任务状态是否进行中状态：每天会有sql作业进行更新
            //3.判断任务剩余次数是否为0，等于0则不允许
            //4.同一个用户1小时之内，只能领取60个不同的任务（包括内容分发和贴片广告）
            //5.订单表插入一条数据（生成渠道码）
            //6.修改任务领取次数与剩余次数

            var retValue = ReceiveByDistributeVerify(_reqTaskReceiveDto.ChannelId);
            if (retValue.HasError)
            {
                //已经领取过了
                if (retValue.ErrorCode.Equals("1004"))
                {
                    return GetReturnOrderInfo(retValue, _reqTaskReceiveDto.TaskId);
                }

                return retValue;
            }

            var orderInfo = GetAdOrderInfoEntity(LeTaskTypeEnum.ContentDistribute, 0, string.Empty,
                _reqTaskReceiveDto.ChannelId, string.Empty, true);

            var excuteId =
                Dal.LETask.LeAdOrderInfo.Instance.Insert(orderInfo);
            if (excuteId <= 0)
            {
                return CreateFailMessage(retValue, "10010", "订单入库失败");
            }
            Dal.LETask.LeTaskInfo.Instance.UpdateTakeCount(_reqTaskReceiveDto.TaskId);
            retValue.ReturnObject = new
            {
                OrderId = excuteId,
                Title = orderInfo.OrderName,
                OrderUrl = orderInfo.OrderUrl,
                PasterUrl = orderInfo.PasterUrl,
                ShareTempQrImage = orderInfo.ShareTempQrImage
            };
            return retValue;
        }

        private ReturnValue GetReturnOrderInfo(ReturnValue retValue, int taskId)
        {
            var orderInfo = Dal.LETask.LeAdOrderInfo.Instance.GetAdOrderInfoByTaskId(taskId, _configEntity.CreateUserId);
            if (orderInfo != null)
            {
                //todo:最新修改：将贴片任务的二维码图片，生成二维码提供分享

                retValue.ReturnObject = new
                {
                    OrderId = orderInfo.RecID,
                    Title = orderInfo.OrderName,
                    OrderUrl = orderInfo.OrderUrl,
                    PasterUrl = orderInfo.OrderType == (int)LeTaskTypeEnum.CoverImage ?
                                CreateShareTempQr(orderInfo.PasterUrl)
                                : orderInfo.PasterUrl
                };
                return retValue;
            }
            retValue.ReturnObject = new
            {
                OrderId = 0,
                Title = "暂无",
                OrderUrl = string.Empty,
                PasterUrl = string.Empty
            };
            return retValue;
        }

        private Entities.LETask.LeAdOrderInfo GetAdOrderInfoEntity(LeTaskTypeEnum leTaskTypeEnum, int mediaId,
            string userIdentity, int channelId, string requestOrderUrl, bool isFilterOrderNum)
        {
            var orderDateRangeConfig = ConfigurationUtil.GetAppSettingValue("ConfigOrderDateRange", true);
            string pasterUrl;
            string orderUrl;
            string shareTempQrImage;
            var code = Common.Util.GenerateRandomCode(10);
            if (string.IsNullOrWhiteSpace(requestOrderUrl))
            {
                orderUrl = SetUrlParamsContent(_contexTaskInfo.MaterialUrl, code);
            }
            else
            {
                orderUrl = requestOrderUrl;
                code = GetUrlParams(requestOrderUrl, "utm_term");
            }

            if (leTaskTypeEnum == LeTaskTypeEnum.CoverImage)
            {
                //生成新的图片二维码替换原来的图片，插入订单表，存储到服务器
                pasterUrl = GenerateImage(orderUrl, _contexTaskInfo.ImgUrl);
                //todo:最新修改：将贴片任务的二维码图片，生成二维码提供分享
                shareTempQrImage = CreateShareTempQr(pasterUrl);
            }
            else
            {
                //内容分发也要生成二维码
                var tunplePath = new TwoBarCodeHistoryProvider(null, new ConfigEntity()).GenPath($"{Guid.NewGuid()}.png");

                var provider = new QrCodeProvider(new QrCodeProviderConfig()
                {
                    Content = orderUrl,
                    SaveFileName = tunplePath.Item1
                });
                provider.Generate();
                pasterUrl = SetImageFromImageToCdn(tunplePath.Item1);
                shareTempQrImage = pasterUrl;
            }
            //因为要解决cdn缓存策略问题，上传到cdn之后，异步下载访问一次
            Loger.Log4Net.Info($"pasterUrl={pasterUrl}");
            try
            {
                WebClient client = new WebClient();
                client.DownloadData(pasterUrl);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Info($"报错pasterUrl={pasterUrl}，{exception.Message}");
            }

            var adOrderInfo = new Entities.LETask.LeAdOrderInfo()
            {
                BeginTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(orderDateRangeConfig.ToInt(1)),
                Status = (int)LeOrderStatusEnum.Ing,
                OrderType = (int)leTaskTypeEnum,
                OrderName = _contexTaskInfo.TaskName,
                OrderUrl = orderUrl,
                PasterUrl = pasterUrl,
                BillingRuleName = _contexTaskInfo.BillingRuleName,
                UserID = _configEntity.CreateUserId,
                MediaID = mediaId,
                OrderCoding = code,
                MediaType = 14001,
                ChannelID = channelId,
                UserIdentity = userIdentity,
                TaskID = _contexTaskInfo.RecID,
                IP = _reqTaskReceiveDto.IP,
                ShareTempQrImage = shareTempQrImage
            };

            return isFilterOrderNum ? SetLeAdOrderInfoPrice(adOrderInfo) : adOrderInfo;
        }

        private Entities.LETask.LeAdOrderInfo SetLeAdOrderInfoPrice(Entities.LETask.LeAdOrderInfo adOrderInfo)
        {
            //校验是否满足指定的订单数量
            var retValue = VerifyReceiveTaskCount(_configEntity.CreateUserId);
            if (retValue.HasError)
            {
                //满足指定数量的订单之后，不再收益
                adOrderInfo.CPCUnitPrice = 0;
                adOrderInfo.CPLUnitPrice = 0;
                adOrderInfo.StatisticsStatus = 0;
            }
            else
            {
                adOrderInfo.CPCUnitPrice = _contexTaskInfo.CPCPrice;
                adOrderInfo.CPLUnitPrice = _contexTaskInfo.CPLPrice;
                adOrderInfo.StatisticsStatus = 1;
            }

            return adOrderInfo;
        }

        /// <summary>
        /// 贴片广告-生成贴片二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public string GenerateImage(string content, string imageUrl)
        {
            var qrCoverImageWidthHeight = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("QrCoverImageWidthHeight", false) ?? "100,100";
            var qrCoverImageXy = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("QrCoverImageXY", false) ?? "100,100";

            var time = DateTime.Now;
            var uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
            string downloadImageFileName;

            var filePath = $"/UploadFiles/Task/QRImage/{time.Year}/{time.Month}/{time.Day}/{time.Hour}/";
            var downloadImagePath = string.Format(uploadFilePath + filePath).Replace(@"/", "\\");
            QrCodeProvider.DirectoryAdd(downloadImagePath);
            //如果是远程的或者cdn的图片，那只能是下载
            if (imageUrl.IndexOf("http:", StringComparison.Ordinal) >= 0
                || imageUrl.IndexOf("https://", StringComparison.Ordinal) >= 0)
            {
                downloadImageFileName = downloadImagePath + $"{Guid.NewGuid()}.png";
                DownImage(imageUrl, downloadImageFileName);
            }
            else
            {
                downloadImageFileName = string.Format(uploadFilePath + imageUrl).Replace(@"/", "\\"); //如果不是远程图片，那就直接用硬盘path
            }

            var saveFileName = $"{Guid.NewGuid()}.png";
            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = content,
                FileName = downloadImageFileName,
                Width = qrCoverImageWidthHeight.Split(',')[0].ToInt(100),
                Height = qrCoverImageWidthHeight.Split(',')[1].ToInt(100),
                SaveFileName = downloadImagePath + saveFileName//新生成的贴片图片保存地址
            });
            //形成贴片
            var tempFileQr = downloadImagePath + $@"\\Temp-{Guid.NewGuid()}.png";
            provider.GenerateQrIntoImage(tempFileQr,
                qrCoverImageXy.Split(',')[0].ToInt(575),
                qrCoverImageXy.Split(',')[1].ToInt(75));
            //上传到文件服务器
            //var cdnImageUrl = Util.CleanImg(System.Drawing.Image.FromFile(downloadImagePath + saveFileName));
            //删除文件
            //DeleteFile(new List<string>()
            //{
            //    tempFileQr,
            //    downloadImagePath + saveFileName
            //});
            return SetImageFromImageToCdn(downloadImagePath + saveFileName);
        }

        public string SetImageFromImageToCdn(string fromFilePath)
        {
            //上传到文件服务器
            var cleanImgUrlPrefix = ConfigurationUtil.GetAppSettingValue("CleanImgURLPrefix");//文章中替换图片url后的域名

            var cdnImageUrl = Util.CleanImg(System.Drawing.Image.FromFile(fromFilePath));
            return cleanImgUrlPrefix + cdnImageUrl;
        }

        public void DeleteFile(List<string> deleteFilePathList)
        {
            deleteFilePathList.ForEach(s =>
            {
                if (!string.IsNullOrWhiteSpace(s))
                    File.Delete(s);
            });
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="path">保存路径</param>
        public void DownImage(string url, string path)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.DownloadFile(url, path);
        }

        /// <summary>
        /// 生成提供分享的二维码，只用一次，需要则重新生成（存储在服务器一个临时文件夹中，可删除）
        /// </summary>
        /// <returns></returns>
        public string CreateShareTempQr(string content)
        {
            var tunplePath = new TwoBarCodeHistoryProvider(null, new ConfigEntity()).GenTempPath($"temp_{Guid.NewGuid()}.png");

            var provider = new QrCodeProvider(new QrCodeProviderConfig()
            {
                Content = content,
                SaveFileName = tunplePath.Item1
            });
            provider.Generate();

            return tunplePath.Item2;
        }

        private ReturnValue ReceiveByDistributeVerify(int channelId)
        {
            var retValue = VerifyOfNecessaryParameters(_reqTaskReceiveDto);
            if (retValue.HasError)
                return retValue;

            var tup = Dal.LETask.LeTaskInfo.Instance.VerifyTaskReceviceByDistribute(_reqTaskReceiveDto.TaskId,
                _configEntity.CreateUserId, channelId);

            if (tup.Item1?.ResultCode > 0)
            {
                return CreateFailMessage(retValue, tup.Item1.ResultCode.ToString(),
                    GetVerifyMessage(tup.Item1.ResultCode));
            }
            _contexTaskInfo = tup.Item2;

            if (_contexTaskInfo == null || string.IsNullOrWhiteSpace(_contexTaskInfo.MaterialUrl))
            {
                return CreateFailMessage(retValue, "10016", "没有分发url，不可以领取任务");
            }

            return retValue;
        }


        public string SetUrlParamsContent(string url, string code)
        {
            //http://news.chitunion.com/materiel/20171206/17472.html?utm_source=chitu&utm_term=chitunionm
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;
            string content;
            if (url.Contains("?"))
            {
                //代表不是域名，而是一个带参数的url地址
                content = $"{url}&utm_source=chitu&utm_term={code}";
            }
            else
            {
                content = $"{url}?utm_source=chitu&utm_term={code}";
            }
            return content;
        }

        public static string GetVerifyMessage(int key)
        {
            var dicError = new Dictionary<int, string>()
            {
                { 0,"验证通过"},
                { 1001,"任务信息不存在"},
                { 1002,"该任务已结束，不可领取"},
                { 1003,"该任务已结束，不可领取"},
                { 1004,"不满足条件,您已领取过该任务"},
                { 1005,"同一个用户1小时之内，只能领取60个不同的任务"},
            };
            return dicError.Where(s => s.Key == key).Select(s => s.Value).FirstOrDefault();
        }

        private ReturnValue ReceiveTaskVerifyBase()
        {
            var retValue = VerifyOfNecessaryParameters(_reqTaskReceiveDto);
            if (retValue.HasError)
                return retValue;
            return retValue;
        }

        #endregion 领取任务-内容分发操作

        #region 领取任务-贴片广告操作

        /// <summary>
        /// 领取任务-贴片广告操作
        /// </summary>
        /// <returns></returns>
        private ReturnValue ReceiveByCoverImage()
        {
            //todo:
            //1.贴片广告和内容分发有点不太一样，贴片领取成功之后，调用内容运营系统的分发渠道接口生成一条分发记录
            var retValue = ReceiveByCoverImageVerify();
            if (retValue.HasError)
            {
                if (retValue.ErrorCode.Equals("1004"))
                {
                    return GetReturnOrderInfo(retValue, _reqTaskReceiveDto.TaskId);
                }
                return retValue;
            }
            var orderInfo = GetAdOrderInfoEntity(LeTaskTypeEnum.CoverImage,
                _reqTaskReceiveDto.MediaId, string.Empty, _reqTaskReceiveDto.ChannelId, string.Empty, true);
            var excuteId = Dal.LETask.LeAdOrderInfo.Instance.Insert(orderInfo);
            if (excuteId <= 0)
            {
                return CreateFailMessage(retValue, "10011", "订单入库失败");
            }
            Dal.LETask.LeTaskInfo.Instance.UpdateTakeCount(_reqTaskReceiveDto.TaskId);

            //new DistributeProvider(new DoHttpClient()).PushDistribute(new ReqDistributeDto()
            //{
            //    MaterielId = _reqTaskReceiveDto.MediaId,
            //    MaterielUrl = _contexTaskInfo.MaterialUrl,
            //    DistributeUserID = _configEntity.CreateUserId
            //});

            retValue.ReturnObject = new
            {
                OrderId = excuteId,
                Title = orderInfo.OrderName,
                OrderUrl = orderInfo.OrderUrl,
                PasterUrl = orderInfo.PasterUrl,//因为贴片特殊，PasterUrl 表里存储的是合成图片
                ShareTempQrImage = orderInfo.ShareTempQrImage
            };
            return retValue;
        }

        private ReturnValue ReceiveByCoverImageVerify()
        {
            var retValue = VerifyOfNecessaryParameters(_reqTaskReceiveDto);
            if (retValue.HasError)
                return retValue;

            var tup = Dal.LETask.LeTaskInfo.Instance.VerifyTaskReceviceByCoverImage(_reqTaskReceiveDto.TaskId,
                _configEntity.CreateUserId, _reqTaskReceiveDto.ChannelId);

            if (tup.Item1?.ResultCode > 0)
            {
                return CreateFailMessage(retValue, tup.Item1.ResultCode.ToString(),
                    GetVerifyMessage(tup.Item1.ResultCode));
            }
            _contexTaskInfo = tup.Item2;
            return retValue;
        }

        #endregion 领取任务-贴片广告操作

        #region 领取任务-M站微信分享

        public ReturnValue ReceiveByWeiXin()
        {
            //_reqTaskReceiveDto.ChannelId = (int)LeOrderChannelTypeEnum.微信;
            var retValue = ReceiveByWeiXinVerifyUserId(_reqTaskReceiveDto.UserId);
            if (retValue.HasError)
            {
                return retValue;
            }
            //校验用户和ip 黑名单
            retValue = VerifyAntiCheating(retValue, _reqTaskReceiveDto.IP, _reqTaskReceiveDto.UserId);
            if (retValue.HasError)
            {
                return CreateSuccessMessage(retValue, "0", "success");
            }
            retValue = ReceiveByDistributeVerify(_reqTaskReceiveDto.ChannelId);
            if (retValue.HasError)
            {
                return retValue;
            }
            retValue = ReceiveByWeiXinVerifyUtmtermCode(retValue, _reqTaskReceiveDto.OrderUrl);
            if (retValue.HasError)
            {
                return retValue;
            }

            var orderInfo = GetAdOrderInfoEntity(LeTaskTypeEnum.ContentDistribute, 0, string.Empty,
                _reqTaskReceiveDto.ChannelId, _reqTaskReceiveDto.OrderUrl, true);
            orderInfo.IP = _reqTaskReceiveDto.IP;
            orderInfo.PromotionChannelID = _reqTaskReceiveDto.PromotionChannelId;

            //todo:校验
            if (!Dal.LETask.LeTaskInfo.Instance.VerifyPromotionChannel(_reqTaskReceiveDto.PromotionChannelId))
            {
                orderInfo.PromotionChannelID = 0;
            }
            var excuteId =
                Dal.LETask.LeAdOrderInfo.Instance.Insert(orderInfo);
            if (excuteId <= 0)
            {
                return CreateFailMessage(retValue, "10050", "订单入库失败");
            }
            Dal.LETask.LeTaskInfo.Instance.UpdateTakeCount(_reqTaskReceiveDto.TaskId);
            retValue.ReturnObject = new
            {
                OrderId = excuteId,
                Title = orderInfo.OrderName,
                OrderUrl = orderInfo.OrderUrl,
                PasterUrl = orderInfo.PasterUrl,
                ShareTempQrImage = orderInfo.ShareTempQrImage
            };
            //分享成功之后插入分享日志
            AddLeShareDetail(orderInfo);
            return retValue;
        }

        public ReturnValue ReceiveByWeiXinVerifyUtmtermCode(ReturnValue returnValue, string orderUrl)
        {
            var code = GetUrlParams(_reqTaskReceiveDto.OrderUrl, "utm_term");
            if (string.IsNullOrWhiteSpace(code))
            {
                return CreateFailMessage(returnValue, "10055", "推广链接url，没有获取到推广码code");
            }
            //todo:校验订单表全局里面是否有code
            if (Dal.LETask.LeAdOrderInfo.Instance.GetOrderCodingCount(code) > 0)
            {
                return CreateFailMessage(returnValue, "10056", "推广码code已存在");
            }
            return returnValue;
        }

        public ReturnValue ReceiveByWeiXinVerifyUserId(int userId)
        {
            var retValue = new ReturnValue();
            var userInfo = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.GetUserInfo(userId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "10052", $"用户不存在：{userId}");
            }
            if (userInfo.Category != 29002)
            {
                return CreateFailMessage(retValue, "10053", $"用户没有权限");
            }
            return retValue;
        }

        public void AddLeShareDetail(Entities.LETask.LeAdOrderInfo adOrderInfo)
        {
            if (!Enum.IsDefined(typeof(LeShareDetailTypeEnum), _reqTaskReceiveDto.ShareType))
            {
                Loger.Log4Net.Info($"AddLeShareDetail ShareType 参数错误:{ _reqTaskReceiveDto.ShareType}");
                //return;
            }
            var entity = new Entities.LETask.LeShareDetail()
            {
                CreateUserId = _configEntity.CreateUserId,
                IP = adOrderInfo.IP,
                CategoryId = _contexTaskInfo.CategoryID,
                CreateTime = DateTime.Now,
                OrderCoding = adOrderInfo.OrderCoding,
                ShareResult = 1,
                ShareURL = adOrderInfo.OrderUrl,
                Status = 0,
                Type = _reqTaskReceiveDto.ShareType
            };
            var excuteId = Dal.LETask.LeShareDetail.Instance.Insert(entity);
            if (excuteId <= 0)
            {
                Loger.Log4Net.Error($" AddLeShareDetail 添加失败：{JsonConvert.SerializeObject(entity)}");
            }
        }

        #endregion

        #region 外部接口-调用入库任务

        /// <summary>
        /// 对外接口-任务入库
        /// </summary>
        /// <param name="reqTaskStorageDto"></param>
        /// <returns></returns>
        public ReturnValue ThirdApiTaskStorage(ReqTaskStorageDto reqTaskStorageDto)
        {
            var retValue = VerifyOfNecessaryParameters(reqTaskStorageDto);
            if (retValue.HasError)
                return retValue;

            //todo:同一个物料id只能推送一次
            if (Dal.LETask.LeTaskInfo.Instance.VerifyTaskStorage(reqTaskStorageDto.MaterialId,
                reqTaskStorageDto.TaskType) > 0)
            {
                return CreateFailMessage(retValue, "10013", $"当前物料id:{reqTaskStorageDto.MaterialId}已存在");
            }

            if (Dal.LETask.LeTaskInfo.Instance.VerifyTaskArticleId(reqTaskStorageDto.MaterialId,
                reqTaskStorageDto.TaskType) > 0)
            {
                return CreateFailMessage(retValue, "10015", $"当前物料id:{reqTaskStorageDto.MaterialId}已存在文章id");
            }

            var taskInfo = AutoMapper.Mapper.Map<ReqTaskStorageDto, Entities.LETask.LeTaskInfo>(reqTaskStorageDto);

            var excuteId = Dal.LETask.LeTaskInfo.Instance.Insert(SetRule(taskInfo, reqTaskStorageDto.TaskMoney));
            if (excuteId <= 0)
            {
                return CreateFailMessage(retValue, "10014", "入库失败");
            }
            return retValue;
        }

        /// <summary>
        /// 获取任务配置信息
        /// </summary>
        /// <returns></returns>
        public TaskConfigEntity GeTaskConfigEntity()
        {
            var taskPriceConfig = ConfigurationUtil.GetAppSettingValue("TaskPriceConfig", true);
            TaskConfigEntity configInfo;
            if (!string.IsNullOrWhiteSpace(taskPriceConfig))
            {
                configInfo = JsonConvert.DeserializeObject<TaskConfigEntity>(taskPriceConfig) ??
                                new TaskConfigEntity();
            }
            else
            {
                throw new Exception("请添加任务配置规则");
                //configInfo = new TaskConfigEntity();
            }
            return configInfo;
        }

        public Entities.LETask.LeTaskInfo SetRule(Entities.LETask.LeTaskInfo taskInfo, decimal taskMoeny)
        {
            var configInfo = GeTaskConfigEntity();
            taskInfo.Status = (int)LeTaskStatusEnum.Ing;
            taskInfo.BeginTime = DateTime.Now;
            taskInfo.EndTime = DateTime.Now.AddDays(configInfo.DateRange);
            taskInfo.RuleCount = configInfo.RuleCount;
            taskInfo.CPCPrice = taskMoeny <= 0 ? GetCPCPrice(configInfo) : taskMoeny;//0.01-0.3的范围值
            taskInfo.CPLPrice = configInfo.CPLPrice;//10元/CPL
            taskInfo.TaskAmount = new Random().Next(configInfo.TaskAmount.Split('-')[0].ToInt(800)
                , configInfo.TaskAmount.Split('-')[1].ToInt(1000)); //最高可得金额 800-1000 随机取整数值

            //计算最大可以获取的金额
            var number = Math.Floor(configInfo.CPCLimitPrice / taskInfo.CPCPrice);
            taskInfo.CPCLimitPrice = taskInfo.CPCPrice * number; //点击收益上限为10元
            taskInfo.CPLLimitPrice = configInfo.CPLLimitPrice;//销售线索上限为100元

            taskInfo.BillingRuleName = $"{taskInfo.CPCPrice}元/CPC + {taskInfo.CPLPrice}元/CPL";

            //todo:获取分类id
            return taskInfo;
        }

        public Entities.LETask.LeTaskInfo SetCategory(Entities.LETask.LeTaskInfo taskInfo)
        {
            var list = Dal.LETask.LeTaskInfo.Instance.GetCategoriesByMaterielId(taskInfo.MaterialID);
            taskInfo.CategoryID = list.Select(s => s.CategoryId).FirstOrDefault();
            return taskInfo;
        }

        public decimal GetCPCPrice(TaskConfigEntity configInfo)
        {
            if (configInfo.CPCPriceAppoint > 0)
            {
                return configInfo.CPCPriceAppoint;
            }
            var offsetNumber = GetOffsetLeft(configInfo.CPCPriceRate);
            var offsetRight = GetOffsetRight(configInfo.CPCPriceRate);
            var randomStart = configInfo.CPCPrice.Split('-')[0].ToDecimal(0.01m);

            var randomEnd = configInfo.CPCPrice.Split('-')[1].ToDecimal(30m);
            var startNumber = Convert.ToInt32(offsetRight * randomStart);
            var endNumber = Convert.ToInt32(offsetRight * randomEnd);
            return Convert.ToDecimal(new Random().Next(startNumber, endNumber) * offsetNumber);//0.2-0.8的范围值
        }

        private decimal GetOffsetLeft(int nextNumber)
        {
            var sbCode = new StringBuilder("0.");
            if (nextNumber - 1 <= 0)
            {
                nextNumber = 2;
            }
            for (var i = 0; i < nextNumber - 1; i++)
            {
                sbCode.Append("0");
            }
            return $"{sbCode}1".ToDecimal(0.01m);
        }
        private int GetOffsetRight(int nextNumber)
        {
            var sbCode = new StringBuilder();

            for (var i = 0; i < nextNumber; i++)
            {
                sbCode.Append("0");
            }
            return $"1{sbCode}".ToInt(100);
        }


        /// <summary>
        /// 物料作废通知
        /// </summary>
        /// <param name="materielId"></param>
        /// <returns></returns>
        public ReturnValue ToAbandoned(int materielId)
        {
            //todo:
            //1.物料作废，通知到我方，然后任务修改过期或无效
            var retValue = new ReturnValue();
            if (materielId <= 0)
            {
                return CreateFailMessage(retValue, "1000", "请输入物料id");
            }
            var excuteCount = Dal.LETask.LeTaskInfo.Instance.UpdateTaskStatus(materielId);
            if (excuteCount == 0)
            {
                return CreateFailMessage(retValue, "1001", "更新失败");
            }
            return retValue;
        }

        #endregion 外部接口-调用入库任务

        #region 外部接口-订单入库相关

        public ReturnValue ThirdApiOrderStorage()
        {
            var retValue = OrderStorageVerify();
            if (retValue.HasError)
            {
                return retValue;
            }

            var orderInfo = GetAdOrderInfoEntity(LeTaskTypeEnum.ContentDistribute, 0,
                _reqOrderStorageDto.UserIdentity, _reqOrderStorageDto.ChannelId, string.Empty, false);
            orderInfo.UserID = _reqOrderStorageDto.UserId;

            var excuteId = Dal.LETask.LeAdOrderInfo.Instance.Insert(orderInfo);
            if (excuteId <= 0)
            {
                return CreateFailMessage(retValue, "10010", "订单入库失败");
            }
            Dal.LETask.LeTaskInfo.Instance.UpdateTakeCount(_reqOrderStorageDto.TaskId);
            retValue.ReturnObject = new
            {
                OrderId = excuteId,
                OrderUrl = orderInfo.OrderUrl,
                PasterUrl = orderInfo.PasterUrl,
                ShareTempQrImage = orderInfo.ShareTempQrImage
            };

            return retValue;
        }


        private ReturnValue OrderStorageVerify()
        {
            var retValue = VerifyOfNecessaryParameters(_reqOrderStorageDto);
            if (retValue.HasError)
                return retValue;
            var tup = Dal.LETask.LeTaskInfo.Instance.VerifyTaskReceviceByDistribute(_reqOrderStorageDto.TaskId,
               _reqOrderStorageDto.UserIdentity, _reqOrderStorageDto.ChannelId);

            if (tup.Item1?.ResultCode > 0)
            {
                return CreateFailMessage(retValue, tup.Item1.ResultCode.ToString(),
                    TaskProvider.GetVerifyMessage(tup.Item1.ResultCode));
            }
            _contexTaskInfo = tup.Item2;
            return retValue;
        }

        public string GetUrlParams(string url, string argsName)
        {
            var nameValueList = ExtractQueryParams(url);
            foreach (var name in nameValueList.Keys)
            {
                if (name.ToString().Equals(argsName, StringComparison.OrdinalIgnoreCase))
                {
                    return nameValueList[name.ToString()];
                }
            }
            return string.Empty;
        }


        #endregion

        #region 防作弊相关

        public ReturnValue VerifyAntiCheating(ReturnValue retValue, string ip, int userId)
        {
            var backList = Dal.LETask.LeIPBlacklist.Instance.GetInfo(ip, LeIPBlacklistStatus.启用);
            if (backList != null)
            {
                return CreateFailMessage(retValue, "10055", "此ip已被拉入黑名单");
            }

            if (Dal.LETask.LeUserBlacklist.Instance.VeriftIsExists(userId, LeIPBlacklistStatus.启用))
            {
                return CreateFailMessage(retValue, "10056", "此用户已被拉入黑名单");
            }

            return retValue;
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(ip))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return ip;
        }

        #endregion


        /// <summary>
        /// 校验用户订单数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ReturnValue VerifyReceiveTaskCount(int userId)
        {
            var retValue = new ReturnValue();
            var effectiveOrderCountConfig = ConfigurationUtil.GetAppSettingValue("ConfigEffectiveOrderCount", true);

            if (effectiveOrderCountConfig.ToInt() <= 0)
            {
                return CreateFailMessage(retValue, "30016", "订单数量规则错误");
            }

            var orderCount = Dal.LETask.LeAdOrderInfo.Instance.GetUserOrderCount(userId);

            if (orderCount >= effectiveOrderCountConfig.ToInt())
            {
                return CreateFailMessage(retValue, "3015", "订单数量已达到上线");
            }
            return retValue;
        }

        /// <summary>
        /// 获取任务列表的分类，进行中的任务数量倒序排列
        /// </summary>
        /// <returns></returns>
        public List<Entities.LETask.LeTaskCategory> GeTaskCategories()
        {
            return Dal.LETask.LeTaskInfo.Instance.GeTaskCategories();
        }
    }
}