using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatInvite;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.WechatInvite;

namespace XYAuto.ITSC.Chitunion2017.BLL.WechatInvite
{
    public class WechatInvite
    {
        public static readonly WechatInvite Instance = new WechatInvite();
        private Random myRandom = new Random();
        /// <summary>
        /// 被邀请用户关注
        /// </summary>
        /// <param name="DTO"></param>
        public void FriendFollow(int userId)
        {
            Loger.Log4Net.Info("进入FriendFollow方法->");
            Loger.Log4Net.Info($"获取被邀请用户ID:{ userId}");
            #region 判断活动是否结束
            string errorMsg = string.Empty;
            object ret = ITSC.Chitunion2017.BLL.Controller.Sign.Instance.IsValidActivity(new Controller.Dto.IsValidActivity.ReqDto() { ActivityType= Convert.ToInt32(Controller.Dto.IsValidActivity.EnumActivityType.邀请有礼)}, ref errorMsg);
            if (string.IsNullOrWhiteSpace(errorMsg) && (bool)ret == false)
            {
                Loger.Log4Net.Info("进入FriendFollow方法->邀请有礼活动已经束");
                return;
            }
            #endregion
            int InviterId = WechatUser.Instance.GetInviterId(userId);
            if (InviterId > 0)
            {
                if (!Dal.WechatInvite.WechatInvite.Instance.IsBeInvited(userId))
                {
                    WechatInviteInfo info = new WechatInviteInfo();
                    info.InviteUserID = InviterId;
                    info.BeInvitedUserID = userId;
                    string Count = ConfigurationManager.AppSettings["DayRedEvesCount"];
                    int RedEvesCount = Dal.WechatInvite.WechatInvite.Instance.GetRedEvesByStatus(InviterId, (int)RedEvesStatusEnum.已领取);

                    if ((Count == null ? 5 : Convert.ToInt32(Count)) > RedEvesCount)
                    {
                        info.RedEvesStatus = (int)RedEvesStatusEnum.尚未完成分享;
                    }
                    else
                    {
                        info.RedEvesStatus = (int)RedEvesStatusEnum.不可领取;
                    }
                    info.IP = BLL.Util.GetIP($"被邀请用户{userId}关注");
                    if (Dal.WechatInvite.WechatInvite.Instance.InsertInviteRecord(info) <= 0)
                    {
                        Loger.Log4Net.Error("userId:" + userId + " InviterId:" + InviterId + $":添加邀请记录失败");
                    }
                    Loger.Log4Net.Info($"{Json.JsonSerializerBySingleData(info)}插入邀请记录表成功");
                }
            }
            Loger.Log4Net.Info("结束FriendFollow方法->");

        }
        Object locker = new Object();
        /// <summary>
        /// 邀请好友领取红包
        /// </summary>
        /// <param name="DTO"></param>
        /// <param name="RedEvesPrice">领取红包金额</param>
        /// <returns></returns>
        public string ReceiveRedEves(WxInviteIdReqDTO DTO, out decimal RedEvesPrice)
        {
            lock (locker)
            {
                RedEvesPrice = 0;
                if (DTO == null)
                {
                    return "领取失败，请重试";
                }
                int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                string Count = ConfigurationManager.AppSettings["DayRedEvesCount"];
                int RedEvesCount = Dal.WechatInvite.WechatInvite.Instance.GetRedEvesByStatus(userId, (int)RedEvesStatusEnum.已领取, DTO.RecID);
                BLL.Loger.Log4Net.Info($"用户ID:{userId}最高领取数量：{Count}已领取数量：{RedEvesCount}");
                if ((Count == null ? 5 : Convert.ToInt32(Count)) <= RedEvesCount)
                {
                    BLL.Loger.Log4Net.Info($"合并导致->数量：{RedEvesCount}已达上限数{(Count == null ? 5 : Convert.ToInt32(Count))}更新为不可领取");
                    Dal.WechatInvite.WechatInvite.Instance.CancelRedEves(userId, DTO.RecID);
                    return "当日红包已领完";
                }

                int RedEvesStatus = Dal.WechatInvite.WechatInvite.Instance.GetRedEvesStatus(userId, DTO.RecID);
                BLL.Loger.Log4Net.Info($"用户ID:{userId} 红包ID:{DTO.RecID} 状态:{RedEvesStatus}");
                if (RedEvesStatus != (int)RedEvesStatusEnum.待领取)
                {
                    return "红包已领取";
                }
                int i = myRandom.Next(1, 100);
                string price = ConfigurationManager.AppSettings["InvitedPrice"];
                if (string.IsNullOrWhiteSpace(price))
                {
                    price = "0.6,0.8,1.06,1.08,1.66,1.88";
                }
                string[] priceList = price.Split(',');
                if (i <= 30)
                {
                    RedEvesPrice = Convert.ToDecimal(priceList[0]);
                }
                else if (i > 30 && i <= 50)
                {
                    RedEvesPrice = Convert.ToDecimal(priceList[1]);
                }
                else if (i > 50 && i <= 65)
                {
                    RedEvesPrice = Convert.ToDecimal(priceList[2]);
                }
                else if (i > 65 && i <= 80)
                {
                    RedEvesPrice = Convert.ToDecimal(priceList[3]);
                }
                else if (i > 80 && i <= 90)
                {
                    RedEvesPrice = Convert.ToDecimal(priceList[4]);
                }
                else
                {
                    RedEvesPrice = Convert.ToDecimal(priceList[5]);
                }
                //}
                if (Dal.WechatInvite.WechatInvite.Instance.UpdateRedEves(RedEvesPrice, userId, DTO.RecID) <= 0)
                {
                    return "领取失败，请重试";
                };

                if ((RedEvesCount + 1) >= (Count == null ? 5 : Convert.ToInt32(Count)))
                {
                    BLL.Loger.Log4Net.Info($"数量：{RedEvesCount + 1}已达上限数{(Count == null ? 5 : Convert.ToInt32(Count))}更新为不可领取");
                    Dal.WechatInvite.WechatInvite.Instance.CancelRedEves(userId, DTO.RecID);
                }
                BLL.Loger.Log4Net.Info($"领取成功");
                return "";
            }
        }
        /// <summary>
        /// 获取被邀请用户列表
        /// </summary>
        /// <param name="TopCount">查询条数</param>
        /// <param name="RecID">起始ID</param>
        /// <returns></returns>
        public Dictionary<string, object> GetBeInvitedList(int TopCount, int RecID)
        {
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            if (RecID == 0)
            {
                RecID = Dal.WechatInvite.WechatInvite.Instance.GetInvitedMaxId(userId) + 1;
            }
            dicAll.Add("TotalPrice", Dal.WechatSign.WechatSign.Instance.GetTotalPriceByUserID(userId, (int)ProfitTypeEnum.邀请红包统计));
            dicAll.Add("TotalCount", Dal.WechatInvite.WechatInvite.Instance.GetBeInvitedCount(userId));
            dicAll.Add("BeInvitedList", Util.DataTableToList<BeInvitedInfo>(Dal.WechatInvite.WechatInvite.Instance.GetBeInvitedList(userId, RecID, TopCount)));
            return dicAll;
        }
    }
}
