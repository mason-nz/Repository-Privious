/**
*
*创建人：lixiong
*创建时间：2018/5/8 17:15:30
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using XYAuto.ChiTu2018.DAO.Chitunion2017;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Android;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Impl;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.Android;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.LE;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.User;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Msg;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Msg.Impl;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task.Impl;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Profit;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Profit.Impl;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat.Impl;

namespace XYAuto.ChiTu2018.BO.RegisterConfig
{
    public class IocMannager
    {
        /// <summary>
        /// UnityContainer 容器可以公布出去
        /// </summary>
        public static readonly UnityContainer UnityContainer = new UnityContainer();

        private static readonly Lazy<IocMannager> Lazy = new Lazy<IocMannager>(() => new IocMannager());
        public static IocMannager Instance => Lazy.Value;

        /// <summary>
        /// 不需要new 一个实例
        /// </summary>
        public IocMannager()
        {
            //RegisterWithdrawals();
            //RegisterUser();
            //RegisterWechat();
            //RegisterProfit();
            //RegisterInfrastructure();
            //RegisterAndroid();
        }

        private static void RegisterUser()
        {
            UnityContainer.RegisterType<IUserInfo, UserInfoImpl>();
            UnityContainer.RegisterType<IUserDetailInfo, UserDetailInfoImpl>();
            UnityContainer.RegisterType<ILEWeiXinUser, LEWeiXinUserImpl>();
        }

        /// <summary>
        /// 基础设施
        /// </summary>
        private static void RegisterInfrastructure()
        {
            UnityContainer.RegisterType<IDictInfo, DictInfoImpl>();
        }

        /// <summary>
        /// 提现相关IOC
        /// </summary>
        private static void RegisterWithdrawals()
        {
            UnityContainer.RegisterType<IAuditInfo, AuditInfoImpl>();
            //UnityContainer.RegisterType<ILeWithdrawalsDetail, LeWithdrawalsDetailImpl>();
            UnityContainer.RegisterType<ILeTaskInfo, LeTaskInfoImpl>();
            UnityContainer.RegisterType<ILEADOrderInfo, LEADOrderInfoImpl>();
            UnityContainer.RegisterType<ILEAccountBalance, LEAccountBalanceImpl>();
            UnityContainer.RegisterType<ILEWXUserScene, LEWXUserSceneImpl>();
            UnityContainer.RegisterType<ILeIpBlacklist, LeIpBlacklistImpl>();
            UnityContainer.RegisterType<ILeUserBlacklist, LeUserBlacklistImpl>();
            UnityContainer.RegisterType<ILeUserBankAccount, LeUserBankAccountImpl>();
            UnityContainer.RegisterType<IProfit, ProfitImpl>();

            UnityContainer.RegisterType<ILeWithdrawalsStatistics, LeWithdrawalsStatisticsImpl>();
            UnityContainer.RegisterType<ILeDaySign, LeDaySignImpl>();
            UnityContainer.RegisterType<ILeIncomeStatisticsCategory, LeIncomeStatisticsCategoryImpl>();

            UnityContainer.RegisterType<ILePromotionChannelDict, LePromotionChannelDictImpl>();
            UnityContainer.RegisterType<IDictScene, DictSceneImpl>();
            UnityContainer.RegisterType<IVUserInfo, VUserInfoImpl>();

            UnityContainer.RegisterType<ILeDisbursementPay, LeDisbursementPayImpl>();
            UnityContainer.RegisterType<ILeShareDetail, LeShareDetailImpl>();
            UnityContainer.RegisterType<ILeIncomeDetail, LeIncomeDetailImpl>();
            UnityContainer.RegisterType<ILeFeedback, LeFeedbackImpl>();

            UnityContainer.RegisterType<ILeIpRequestLog, LeIpRequestLogImpl>();
        }

        /// <summary>
        /// 微信相关IOC
        /// </summary>
        public static void RegisterWechat()
        {
            UnityContainer.RegisterType<ILeInviteRecord, LeInviteRecordImpl>();
        }
        /// <summary>
        /// 收益相关IOC
        /// </summary>
        public static void RegisterProfit()
        {
            UnityContainer.RegisterType<IIncomeStatistics, IncomeStatisticsImpl>();
        }

        /// <summary>
        /// android app 相关IOC
        /// </summary>
        public static void RegisterAndroid()
        {
            UnityContainer.RegisterType<IAppDevice, AppDeviceImpl>();
            UnityContainer.RegisterType<IAppPushMsgSwitchLog, AppPushMsgSwitchLogImpl>();
        }


        /// <summary>
        /// 获取 Ioc 接口对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            try
            {
                return UnityContainer.Resolve<T>();
            }
            catch (Exception exception)
            {
                throw new IoCResolveException($"XYAuto.ChiTu2018.BO.RegisterConfig.Resolve<> 方法错误，未发现Ioc注册信息,{typeof(T)}" +
                                              $"{exception.Message}{exception.StackTrace ?? string.Empty}");
            }
        }
    }
}
