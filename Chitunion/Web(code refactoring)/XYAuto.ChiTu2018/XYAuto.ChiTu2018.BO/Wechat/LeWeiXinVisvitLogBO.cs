using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat.Impl;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.BO.Wechat
{
    /// <summary>
    /// 注释：LeWeiXinVisvitLogBO
    /// 作者：masj
    /// 日期：2018/5/9 15:57:24
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeWeiXinVisvitLogBO
    {
        #region ioc
        //会在程序执行前初始化
        private static readonly UnityContainer Container = new UnityContainer();

        private static ILeWeiXinVisvitLog LeWeiXinVisvitLog()
        {
            //注入
            Container.RegisterType<ILeWeiXinVisvitLog, LeWeiXinVisvitLogImpl>();
            //反转
            return Container.Resolve<ILeWeiXinVisvitLog>();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LE_WeiXinVisvit_Log Add(LE_WeiXinVisvit_Log model)
        {
            return LeWeiXinVisvitLog().Add(model);
        }
    }
}
