/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.BLL.WeChat
* 类 名 称 ：ActivityVerifyBll
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/12 18:14:20
********************************/
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Dal.WeChat;
using XYAuto.ITSC.Chitunion2017.Entities.WeChat;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    public class ActivityVerifyBll
    {
        #region 单例
        private ActivityVerifyBll() { }

        public static ActivityVerifyBll instance = null;
        public static readonly object padlock = new object();

        public static ActivityVerifyBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ActivityVerifyBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 判断是否新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true：新用户  false：老用户</returns>
        public bool IsNewUserByUserId(int userId)
        {
            var configText = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("OneYuanTiXianActivity");
            var jObject = JObject.Parse(configText);
            var model = new VerifyUserModel
            {
                UserID = userId,
                BeginTime = ConvertHelper.GetDateTime(jObject["BeginTime"]),
                EndTime = ConvertHelper.GetDateTime(jObject["EndTime"])

            };
            if (VerifyIncomeDetail(model) && VerifyUser(model))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 判断是否新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true：新用户  false：老用户</returns>
        public bool IsNewUserByOpenId(string openId)
        {
            int userId = ActivityVerifyDa.Instance.GetUserIdByOpenId(openId);
            return IsNewUserByUserId(userId);
        }
        /// <summary>
        /// 根据用户获取订单数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int GetUserOrderNum(VerifyUserModel model)
        {
            return ActivityVerifyDa.Instance.GetOrderNum(model);
        }

        /// <summary>
        /// 一级判断：是否是新用户，需要判断是否存在提现成功的记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool VerifyUserByUserId(int userId)
        {
            var configText = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("OneYuanTiXianActivity");
            var jObject = JObject.Parse(configText);

            //是否有提现成功的
            var isSuccess = ActivityVerifyDa.Instance.VerifyWithdrawalsSuccess(new VerifyUserModel()
            {
                UserID = userId,
                Source = 103009
            });
            var newUser = VerifyUser(new VerifyUserModel
            {
                UserID = userId,
                BeginTime = ConvertHelper.GetDateTime(jObject["BeginTime"]),
                EndTime = ConvertHelper.GetDateTime(jObject["EndTime"])
            });

            return !isSuccess && newUser;
        }

        public bool VerifyUser(VerifyUserModel model)
        {
            return ActivityVerifyDa.Instance.VerifyUser(model);
        }

        public bool VerifyIncomeDetail(VerifyUserModel model)
        {
            return ActivityVerifyDa.Instance.VerifyIncomeDetail(model);
        }

        public bool VerifyWithdrawals(VerifyUserModel model)
        {
            return ActivityVerifyDa.Instance.VerifyWithdrawals(model);
        }

    }
}
