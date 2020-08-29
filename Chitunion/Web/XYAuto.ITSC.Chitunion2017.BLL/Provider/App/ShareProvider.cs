using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.APP;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Enums;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.WebService.Common;

namespace XYAuto.ITSC.Chitunion2017.BLL.Provider.App
{
    public class ShareProvider : VerifyOperateBase
    {
        /*
       1.分享之后要记录分享日志
       2.附加业务：
           1.签到的逻辑，
           2.新手欢迎奖励

       */
        private readonly ConfigEntity _configEntity;
        private readonly ReqCreateShareDto _reqCreateShareDto;
        private readonly Dictionary<int, Func<string, dynamic>> _dictionarys;

        public ShareProvider(ConfigEntity configEntity, ReqCreateShareDto reqCreateShareDto)
        {
            _configEntity = configEntity;
            _reqCreateShareDto = reqCreateShareDto;
            _dictionarys = InitShareBusinessDic();
        }

        public ReturnValue GetOrderUrl(int taskId)
        {
            var retValue = new ReturnValue();
            var info = BLL.WeChat.Order.Instance.GetTaskInfo(taskId);
            if (info == null || string.IsNullOrWhiteSpace(info.MaterialUrl))
                return CreateFailMessage(retValue, "10010", "任务id无效");
            var code = Common.Util.GenerateRandomCode(10);
            var orderUrl = new TaskProvider(_configEntity, new ReqTaskReceiveDto()).SetUrlParamsContent(info.MaterialUrl, code);

            return CreateSuccessMessage(retValue, "0", "success", new
            {
                TaskId = taskId,
                OrderUrl = ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetDomainByRandom_ShareArticle(orderUrl),
                Synopsis = info.Synopsis,
                TaskName = info.TaskName,
                ImgUrl = info.ImgUrl
            });
        }

        public ReturnValue Log()
        {
            var retValue = VerifyOfShare();
            if (retValue.HasError)
                return retValue;

            retValue = VerifyOfShareDetail(retValue);
            if (retValue.HasError)
                return retValue;

            //todo:set ip
            _reqCreateShareDto.ShareContent.Ip = TaskProvider.GetIP();

            retValue = AddLeShareDetail(retValue, _reqCreateShareDto);
            if (retValue.HasError)
                return retValue;

            //todo:校验用户和ip 黑名单
            BLL.LETask.Provider.TaskProvider pro = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto());
            retValue = pro.VerifyAntiCheating(retValue, _reqCreateShareDto.ShareContent.Ip, _configEntity.CreateUserId);
            if (retValue.HasError)
            {
                return CreateSuccessMessage(retValue, "0", "success");
            }

            //todo:生成订单
            retValue = CreateShareOrder();
            if (retValue.HasError)
                return retValue;

            //todo:根据分享的类别处理不同的业务
            retValue = ToShareBusinessDic();
            if (retValue.HasError)
                return retValue;
            return retValue;
        }

        private ReturnValue CreateShareOrder()
        {
            //todo:如果分享的是任务，则生成订单，如果是海报，则不用生成
            if (_reqCreateShareDto.ShareContent.ShareContentType == (int)ShareContentTypeEnum.任务物料)
            {
                //todo:生成订单,调用接口：ReceiveByWeiXin
                var requestUrl = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GetOrderUrlByPost");
                var postData = JsonConvert.SerializeObject(GetPostSubmitOrderDto());
                //var doHttpClient = new DoHttpClient(new System.Net.Http.HttpClient());
                //var result = new DoPostApiLogClient(requestUrl, postData)
                //    .GetPostResult<RespBaseChituDto<RespPostReceiceDto>>(
                //        s => doHttpClient.PostByJson(requestUrl, postData).Result, Loger.Log4Net.Info);

                var result = new DoPostApiLogClient(requestUrl, postData)
                   .GetPostResult<RespBaseChituDto<RespPostReceiceDto>>(
                       s => WebService.Common.HttpClient.PostByJson(requestUrl, postData), Loger.Log4Net.Info);

                if (result == null || result.Status != 0)
                {
                    Loger.Log4Net.Error($"CreateShareOrder http post ReceiveByWx fail." +
                                        (result == null ? string.Empty : JsonConvert.SerializeObject(result)));
                    return CreateFailMessage(new ReturnValue(), "10023", "生成订单错误");
                }
                Loger.Log4Net.Info($"CreateShareOrder生成订单:{result.Result}");
            }

            return CreateSuccessMessage(new ReturnValue(), "0", "生成订单成功");
        }

        private ReqTaskReceiveDto GetPostSubmitOrderDto()
        {
            return new ReqTaskReceiveDto()
            {
                TaskId = _reqCreateShareDto.ShareContent.TaskId,
                ChannelId = (int)ShareChannelIdEnum.App,
                IP = _reqCreateShareDto.ShareContent.Ip,
                OrderUrl = _reqCreateShareDto.ShareContent.ShareUrl,
                PromotionChannelId = 0,
                ShareType = _reqCreateShareDto.ShareType,
                TaskType = (int)LeTaskTypeEnum.ContentDistribute,
                UserId = _configEntity.CreateUserId
            };
        }

        private ReturnValue VerifyOfShare()
        {
            var retValue = new ReturnValue();
            if (!Enum.IsDefined(typeof(LeShareDetailTypeEnum), _reqCreateShareDto.ShareType))
            {
                return CreateFailMessage(retValue, "10001", "ShareType 参数错误");
            }
            if (_reqCreateShareDto.ShareContent == null)
            {
                return CreateFailMessage(retValue, "10002", "ShareContent 参数错误");
            }
            if (!Enum.IsDefined(typeof(ShareContentTypeEnum), _reqCreateShareDto.ShareContent.ShareContentType))
            {
                return CreateFailMessage(retValue, "10003", "ShareContentType 参数错误");
            }
            if (_reqCreateShareDto.ShareContent.ShareContentType == (int)ShareContentTypeEnum.任务物料 && _reqCreateShareDto.ShareContent.TaskId <= 0)
            {
                return CreateFailMessage(retValue, "10004", "TaskId必须");
            }
            if (!_dictionarys.ContainsKey(_reqCreateShareDto.ShareType))
            {
                return CreateFailMessage(new ReturnValue(), "10005", "ShareType 非法");
            }
            return retValue;
        }

        /// <summary>
        /// 新用户分享：只能分享一次
        /// 提现分享：一天只能一次
        /// 签到分享：暂无
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyOfShareDetail(ReturnValue retValue)
        {
            if (_reqCreateShareDto.ShareType == (int)LeShareDetailTypeEnum.首次欢迎分享)
            {
                //todo:只能有一次
                if (Dal.LETask.LeShareDetail.Instance.IsExist(_configEntity.CreateUserId, LeShareDetailTypeEnum.首次欢迎分享))
                {
                    return CreateFailMessage(retValue, "11001", "您已不是新用户");
                }
            }
            else if (_reqCreateShareDto.ShareType == (int)LeShareDetailTypeEnum.提现分享)
            {
                //todo:还需要查看用户今天是否存在提现支付记录
                if (!Dal.LETask.LeWithdrawalsDetail.Instance.IsExist(_configEntity.CreateUserId))
                {
                    return CreateFailMessage(retValue, "11002", "未查询到有提现申请记录，请先提现");
                }
                //todo:一天只允许一次分享
                if (Dal.LETask.LeShareDetail.Instance.IsExistWithdrawas(_configEntity.CreateUserId))
                {
                    return CreateFailMessage(retValue, "11003", "您已分享过了");
                }
            }
            return retValue;
        }

        /// <summary>
        /// 这个方法是分享海报的日志，OrderCoding，CategoryId 获取不到
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="shareDto"></param>
        /// <returns></returns>
        private ReturnValue AddLeShareDetail(ReturnValue retValue, ReqCreateShareDto shareDto)
        {
            //todo:物料分享不再添加分享日志，因为生成订单里面已经生成过了
            if (_reqCreateShareDto.ShareContent.ShareContentType == (int)ShareContentTypeEnum.任务物料)
                return retValue;
            var entity = new Entities.LETask.LeShareDetail()
            {
                CreateUserId = _configEntity.CreateUserId,
                IP = shareDto.ShareContent.Ip,
                CategoryId = 0,
                CreateTime = DateTime.Now,
                OrderCoding = string.Empty,
                ShareResult = 1,
                ShareURL = shareDto.ShareContent.ShareUrl,
                Status = 0,
                Type = shareDto.ShareType
            };
            var excuteId = Dal.LETask.LeShareDetail.Instance.Insert(entity);
            if (excuteId <= 0)
            {
                Loger.Log4Net.Error($" AddLeShareDetail 添加失败：{JsonConvert.SerializeObject(entity)}");
                return CreateFailMessage(retValue, "10022", "log添加失败");
            }

            return retValue;
        }

        private Dictionary<int, Func<string, dynamic>> InitShareBusinessDic()
        {
            var dic = new Dictionary<int, Func<string, dynamic>>()
            {
                { (int)LeShareDetailTypeEnum.首次欢迎分享, s => { return FirstWelcomeShare(); }},
                { (int)LeShareDetailTypeEnum.签到分享, s => { return AttendanceShare(); }},
                { (int)LeShareDetailTypeEnum.提现分享, s => { return PutForwardShare(); }}
            };

            return dic;

        }

        public ReturnValue ToShareBusinessDic()
        {
            if (_dictionarys.ContainsKey(_reqCreateShareDto.ShareType))
            {
                return _dictionarys[_reqCreateShareDto.ShareType].Invoke(string.Empty);
            }
            return CreateFailMessage(new ReturnValue(), "10005", "ShareType 非法");
        }

        /// <summary>
        /// 签到分享成功之后的逻辑
        /// </summary>
        /// <returns></returns>
        private ReturnValue AttendanceShare()
        {
            //todo:调用签到奖励的逻辑
            //1.原有签到的逻辑
            return new ReturnValue();
        }

        /// <summary>
        /// 首次欢迎分享奖励的逻辑
        /// </summary>
        /// <returns></returns>
        private ReturnValue FirstWelcomeShare()
        {
            //todo:首次到来的奖励的逻辑
            //先读取到配置的奖励金额，然后添加到收益表

            var retValue = new ReturnValue();
            var money = GenericProfilesBll.Instance.GetWelcomeBountyPrice().ToDecimal();

            var type = _reqCreateShareDto.ShareContent.ShareContentType == (int)ShareContentTypeEnum.任务物料
                ? ProfitTypeEnum.新用户奖励文章
                : ProfitTypeEnum.新用户奖励海报;

            var isSuccess = BLL.Profit.Profit.Instance.AddProfit(_configEntity.CreateUserId, type, $"首次分享奖励", money, null, 1);
            if (!isSuccess)
            {
                Loger.Log4Net.Error($"AddProfit 新用户奖励 错误：UserId={_configEntity.CreateUserId},profitType={type},desc={$"首次分享奖励"},money={money}");
                return CreateFailMessage(retValue, "10025", $"新用户奖励错误");
            }
            retValue.ReturnObject = new RespShareRewardDto { Money = money, MsgTips = string.Format(GetSuccessTipsDtos(LeShareDetailTypeEnum.首次欢迎分享), money) };
            return retValue;

        }

        /// <summary>
        /// 提现成功之后的分享的奖励逻辑
        /// </summary>
        /// <returns></returns>
        private ReturnValue PutForwardShare()
        {
            //todo:提现成功之后的分享的奖励逻辑
            var retValue = new ReturnValue();

            var money = GenericProfilesBll.Instance.GetFlauntBountyPrice().ToDecimal();
            var isSuccess = BLL.Profit.Profit.Instance.AddProfit(_configEntity.CreateUserId, ProfitTypeEnum.提现申请海报奖励, $"提现申请成功", money, DateTime.Now, 1);
            if (!isSuccess)
            {
                Loger.Log4Net.Error($"AddProfit 提现成功之后的分享的奖励 错误：UserId={_configEntity.CreateUserId},profitType={(int)ProfitTypeEnum.提现申请海报奖励},desc={$"提现申请成功"},money={money}");
                return CreateFailMessage(retValue, "10026", "提现申请奖励错误");
            }
            retValue.ReturnObject = new RespShareRewardDto { Money = money, MsgTips = string.Format(GetSuccessTipsDtos(LeShareDetailTypeEnum.提现分享), money) };
            return retValue;
        }

        /// <summary>
        /// 分享奖励成功之后的提示
        /// </summary>
        /// <returns></returns>
        public string GetSuccessTipsDtos(LeShareDetailTypeEnum leShareDetailType)
        {
            var tips = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ShareSuccessTips");
            var list = JsonConvert.DeserializeObject<List<ShareSuccessTipsDto>>(tips);

            return list.Where(s => s.ShareType == (int)leShareDetailType).Select(s => s.MsgTips).FirstOrDefault()?? "恭喜您!,{0}元已经到账";
        }
    }
}
