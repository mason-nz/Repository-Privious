using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO.Profit;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.BO.Wechat;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.Profit;
using XYAuto.ChiTu2018.Entities.Enum.Wechat;
using XYAuto.ChiTu2018.Service.Wechat.Dto;
using XYAuto.CTUtils.Html;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.Service.Wechat
{
    /// <summary>
    /// 注释：邀请好友类
    /// 作者：zhanglb
    /// 日期：2018/5/11 17:24:02
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeInviteRecordService
    {
        private LeInviteRecordService() { }
        private static readonly Lazy<LeInviteRecordService> Linstance = new Lazy<LeInviteRecordService>(() => new LeInviteRecordService());

        public static LeInviteRecordService Instance => Linstance.Value;

        /// <summary>
        /// 被邀请用户关注
        /// </summary>
        /// <param name="userId">关注用户ID</param>
        public void FriendFollow(int userId)
        {
            Log4NetHelper.Default().Info("进入FriendFollow方法->");
            Log4NetHelper.Default().Info($"获取被邀请用户ID:{ userId}");
            #region 判断活动是否结束
            var errorMsg = string.Empty;
            var ret = WechatSignService.Instance.IsValidActivity(new SignReqDto() { ActivityType = Convert.ToInt32(ActivityTypeEnum.邀请有礼) }, ref errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) && (bool)ret == false)
            {
                Log4NetHelper.Default().Info("进入FriendFollow方法->邀请有礼活动已经束");
                return;
            }
            #endregion
            var leInviteRecord = new LeInviteRecordBO();
            var weiXinUser = new LEWeiXinUserBO().GetModelByUserId(userId);
            var inviterId = 0;
            if (!string.IsNullOrEmpty(weiXinUser?.Inviter))
            {
                int.TryParse(weiXinUser.Inviter, out inviterId);
            }
            if (inviterId > 0)
            {
                if (leInviteRecord.GetBeInviter(userId) == null)
                {
                    var inviteRecord = new LE_InviteRecord
                    {
                        InviteUserID = inviterId,
                        BeInvitedUserID = userId,
                        InviteTime = DateTime.Now
                    };
                    var count = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("DayRedEvesCount", "5");
                    var redEvesCount = leInviteRecord.GetTodayRedEvesCount(inviterId, (int)RedEvesStatusEnum.已领取);
                    if (Convert.ToInt32(count) > redEvesCount)
                    {
                        inviteRecord.RedEvesStatus = (int)RedEvesStatusEnum.尚未完成分享;
                    }
                    else
                    {
                        inviteRecord.RedEvesStatus = (int)RedEvesStatusEnum.不可领取;
                    }
                    inviteRecord.IP = RequestHelper.GetIpAddress("127.0.0.1");
                    if (leInviteRecord.AddInviteRecord(inviteRecord) != null)
                    {
                        Log4NetHelper.Default().Info("userId:" + userId + " InviterId:" + inviterId + $":添加邀请记录失败");
                    }
                    Log4NetHelper.Default().Info($"{JsonConvert.SerializeObject(inviteRecord)}插入邀请记录表成功");
                }
            }
            Log4NetHelper.Default().Info("结束FriendFollow方法->");
        }

        readonly object _locker = new object();

        /// <summary>
        /// 邀请好友领取红包
        /// </summary>
        /// <param name="dto">主键ID实体</param>
        /// <param name="redEvesPrice">领取红包金额</param>
        /// <returns></returns>
        public string ReceiveRedEves(ReqInviteRecIdDto dto, out decimal redEvesPrice)
        {
            lock (_locker)
            {
                redEvesPrice = 0;
                if (dto == null)
                {
                    return "领取失败，请重试";
                }
                var leInviteRecord = new LeInviteRecordBO();
                var userId = UserInfo.GetLoginUserID();
                var count = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("DayRedEvesCount", "5");
                var redEvesCount = leInviteRecord.GetRedEvesCount(userId, dto.RecId, (int)RedEvesStatusEnum.已领取);
                Log4NetHelper.Default().Info($"用户ID:{userId}最高领取数量：{count}已领取数量：{redEvesCount}");
                if (Convert.ToInt32(count) <= redEvesCount)
                {
                    Log4NetHelper.Default().Info($"合并导致->数量：{redEvesCount}已达上限数{(count == null ? 5 : Convert.ToInt32(count))}更新为不可领取");
                    leInviteRecord.UpdateRedEves(userId, dto.RecId);
                    return "当日红包已领完";
                }

                var redEvesStatus = leInviteRecord.GetRedEvesStatus(userId, dto.RecId);
                Log4NetHelper.Default().Info($"用户ID:{userId} 红包ID:{dto.RecId} 状态:{redEvesStatus}");
                if (redEvesStatus != (int)RedEvesStatusEnum.待领取)
                {
                    return "红包已领取";
                }
                var random = new Random().Next(1, 100);
                var price = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("InvitedPrice", "0-0,2.5-16,2.66-34,2.77-50,2.88-68,2.99-84,3.00-100");
                var priceList = price.Split(',');
                for (int i = 0; i < priceList.Length; i++)
                {
                    if (random > Convert.ToInt32(priceList[i].Split('-')[1]) && random <= Convert.ToInt32(priceList[i + 1].Split('-')[1]))
                    {
                        redEvesPrice = Convert.ToDecimal(priceList[i + 1].Split('-')[0]);
                    }
                }

                if (leInviteRecord.UpdateRedEvesPrice(dto.RecId, userId, redEvesPrice, (int)RedEvesStatusEnum.待领取, (int)RedEvesStatusEnum.已领取) <= 0)
                {
                    return "领取失败，请重试";
                };

                if ((redEvesCount + 1) >= Convert.ToInt32(count))
                {
                    Log4NetHelper.Default().Info($"数量：{redEvesCount + 1}已达上限数{Convert.ToInt32(count)}更新为不可领取");
                    leInviteRecord.UpdateRedEves(userId, dto.RecId);
                }
                Log4NetHelper.Default().Info($"领取成功");
                return "";
            }
        }

        /// <summary>
        /// 获取被邀请用户列表
        /// </summary>
        /// <param name="topCount">查询条数</param>
        /// <param name="recId">起始ID</param>
        /// <returns></returns>
        public Dictionary<string, object> GetBeInvitedList(int topCount, int recId)
        {
            var userId = UserInfo.GetLoginUserID();
            var dicAll = new Dictionary<string, object>();
            var leInviteRecord = new LeInviteRecordBO();
            if (recId == 0)
            {
                recId = leInviteRecord.GetInvitedMaxRecId(userId);
            }
            dicAll.Add("TotalPrice", new LeIncomeStatisticsCategoryBO().GetTotalPriceByUserId(userId, (int)ProfitTypeEnum.邀请红包统计));
            dicAll.Add("TotalCount", leInviteRecord.GetInvitedCount(userId));
            dicAll.Add("BeInvitedList", leInviteRecord.GetInvitedList(userId, recId, topCount));
            return dicAll;
        }

    }
}
