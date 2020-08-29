using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.Profit;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.Profit;
using XYAuto.ChiTu2018.Infrastructure.Extensions;
using XYAuto.ChiTu2018.Infrastructure.HttpLog;
using XYAuto.ChiTu2018.Infrastructure.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Response;
using XYAuto.ChiTu2018.Service.App.AppInfo.Enums;
using XYAuto.ChiTu2018.Service.App.Profiles;
using XYAuto.ChiTu2018.Service.App.ThirdApi.Dto.Request;
using XYAuto.CTUtils.Sys;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Provider
{
    /// <summary>
    /// 注释：ShareProvider        
    /// 1.分享之后要记录分享日志
    ///    2.附加业务：
    ///        1.签到的逻辑，
    ///        2.新手欢迎奖励
    /// 作者：lix
    /// 日期：2018/5/21 19:20:49
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ShareProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqCreateShareDto _reqCreateShareDto;
        private readonly Dictionary<int, Func<string, dynamic>> _dictionarys;
        private readonly ProfitBO _profitBo;
        private readonly LeShareDetailBO _leShareDetailBo;
        private readonly LeWithdrawalsDetailBO _leWithdrawalsDetailBo;

        public ShareProvider(ConfigEntity configEntity, ReqCreateShareDto reqCreateShareDto)
        {
            _configEntity = configEntity;
            _reqCreateShareDto = reqCreateShareDto;
            _dictionarys = InitShareBusinessDic();
            _profitBo = new ProfitBO();
            _leShareDetailBo = new LeShareDetailBO();
            _leWithdrawalsDetailBo = new LeWithdrawalsDetailBO();
        }

        #region init 初始化

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

        #endregion

        public ReturnValue GetOrderUrl(int taskId)
        {
            var retValue = new ReturnValue();
            var info = new LeTaskInfoBO().GeTaskInfo(taskId);
            if (info == null || string.IsNullOrWhiteSpace(info.MaterialUrl))
                return CreateFailMessage(retValue, "10010", "任务id无效");
            var code = RandomHelper.GenerateRandomCode(10, GenerateRandomType.LetterAndNum);
            var orderUrl = LeTaskBasicSupport.SetUrlParamsContent(info.MaterialUrl, code);

            return CreateSuccessMessage(retValue, "0", "success", new
            {
                TaskId = taskId,
                OrderUrl = LeTaskBasicSupport.GetDomainByRandom_ShareArticle(orderUrl),
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
            //_reqCreateShareDto.ShareContent.Ip = TaskProvider.GetIP();

            retValue = AddLeShareDetail(retValue, _reqCreateShareDto);
            if (retValue.HasError)
                return retValue;

            //todo:校验用户和ip 黑名单
            retValue = VerifyAntiCheating(retValue, _configEntity.Ip, _configEntity.UserId);
            if (retValue.HasError)
            {
                return CreateFailMessage(retValue, "0", "success");
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

        /// <summary>
        /// 校验ip黑名单，用户黑名单
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="ip">客户端ip</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public ReturnValue VerifyAntiCheating(ReturnValue retValue, string ip, int userId)
        {
            if (new LeIpBlacklistBO().VeriftIsExists(ip, LeIPBlacklistStatus.启用))
            {
                return CreateFailMessage(retValue, "10055", "此ip已被拉入黑名单");
            }
            if (new LeUserBlacklistBO().VeriftIsExists(userId, LeIPBlacklistStatus.启用))
            {
                return CreateFailMessage(retValue, "10056", "此用户已被拉入黑名单");
            }

            return retValue;
        }

        public ReturnValue VerifyShareType(ReturnValue retValue, int shareType)
        {
            if (!new DictInfoBO().GetList(202).Exists(s => s.DictId == shareType))
            {
                return CreateFailMessage(retValue, "10001", "ShareType 参数错误");
            }
            return retValue;
        }

        private ReturnValue CreateShareOrder()
        {
            //todo:如果分享的是任务，则生成订单，如果是海报，则不用生成
            if (_reqCreateShareDto.ShareContent.ShareContentType == (int)ShareContentTypeEnum.任务物料)
            {
                //todo:生成订单,调用接口：ReceiveByWeiXin
                var requestUrl = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("GetOrderUrlByPost");
                var postData = JsonConvert.SerializeObject(GetPostSubmitOrderDto());

                var result = new DoPostApiLogClient(requestUrl, postData)
                   .GetPostResult<RespBaseChituDto<RespPostReceiceDto>>(
                       s => Infrastructure.HttpLog.HttpClient.PostByJson(requestUrl, postData), CTUtils.Log.Log4NetHelper.Default().Info);

                if (result == null || result.Status != 0)
                {
                    CTUtils.Log.Log4NetHelper.Default().Error($"CreateShareOrder http post ReceiveByWx fail." +
                                        (result == null ? string.Empty : JsonConvert.SerializeObject(result)));
                    return CreateFailMessage(new ReturnValue(), "10023", "生成订单错误");
                }
                CTUtils.Log.Log4NetHelper.Default().Info($"CreateShareOrder生成订单:{result.Result}");
            }

            return CreateSuccessMessage(new ReturnValue(), "0", "生成订单成功");
        }

        private ReqTaskReceiveDto GetPostSubmitOrderDto()
        {
            return new ReqTaskReceiveDto()
            {
                TaskId = _reqCreateShareDto.ShareContent.TaskId,
                ChannelId = (int)ShareChannelIdEnum.App,
                Ip = _configEntity.Ip,
                OrderUrl = _reqCreateShareDto.ShareContent.ShareUrl,
                PromotionChannelId = 0,
                ShareType = _reqCreateShareDto.ShareType,
                TaskType = (int)LeTaskTypeEnum.ContentDistribute,
                UserId = _configEntity.UserId
            };
        }

        private ReturnValue VerifyOfShare()
        {
            var retValue = new ReturnValue();

            retValue = VerifyShareType(retValue, _reqCreateShareDto.ShareType);
            if (retValue.HasError)
            {
                return retValue;
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
                if (_leShareDetailBo.IsExist(_configEntity.UserId, (int)LeShareDetailTypeEnum.首次欢迎分享))
                {
                    return CreateFailMessage(retValue, "11001", "您已不是新用户");
                }
            }
            else if (_reqCreateShareDto.ShareType == (int)LeShareDetailTypeEnum.提现分享)
            {
                //todo:还需要查看用户今天是否存在提现支付记录
                if (!_leWithdrawalsDetailBo.IsExist(_configEntity.UserId))
                {
                    return CreateFailMessage(retValue, "11002", "未查询到有提现申请记录，请先提现");
                }
                //todo:一天只允许一次分享
                if (_leShareDetailBo.IsExistWithdrawas(_configEntity.UserId))
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
            var entity = new Entities.Chitunion2017.LE.LE_ShareDetail
            {
                CreateUserID = _configEntity.UserId,
                IP = _configEntity.Ip,
                CategoryID = 0,
                CreateTime = DateTime.Now,
                OrderCoding = string.Empty,
                ShareResult = 1,
                ShareURL = shareDto.ShareContent.ShareUrl,
                Status = 0,
                Type = shareDto.ShareType
            };
            var excuteId = new LeShareDetailBO().Insert(entity);
            if (excuteId <= 0)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($" AddLeShareDetail 添加失败：{JsonConvert.SerializeObject(entity)}");
                return CreateFailMessage(retValue, "10022", "log添加失败");
            }

            return retValue;
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
            var money = GenericProfilesService.Instance.GetWelcomeBountyPrice().ToDecimal();
            var type = _reqCreateShareDto.ShareContent.ShareContentType == (int)ShareContentTypeEnum.任务物料
                    ? ProfitTypeEnum.新用户奖励文章
                    : ProfitTypeEnum.新用户奖励海报;
            var isSuccess = AppProfitService.Instance.AddProfit(_configEntity.UserId, type, $"首次分享奖励", money, null, 1);
            if (!isSuccess)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error($"AddProfit 新用户奖励 错误：UserId={_configEntity.UserId},profitType={type},desc={$"首次分享奖励"},money={money}");
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
            var money = GenericProfilesService.Instance.GetFlauntBountyPrice().ToDecimal();
            var isSuccess = AppProfitService.Instance.AddProfit(_configEntity.UserId, ProfitTypeEnum.提现申请海报奖励, $"提现申请成功", money, DateTime.Now, 1);
            if (!isSuccess)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error($"AddProfit 提现成功之后的分享的奖励 错误：UserId={_configEntity.UserId},profitType={(int)ProfitTypeEnum.提现申请海报奖励},desc={$"提现申请成功"},money={money}");
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
            var tips = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("ShareSuccessTips");
            var list = JsonConvert.DeserializeObject<List<ShareSuccessTipsDto>>(tips);

            return list.Where(s => s.ShareType == (int)leShareDetailType).Select(s => s.MsgTips).FirstOrDefault() ?? "恭喜您!,{0}元已经到账";
        }
    }
}
