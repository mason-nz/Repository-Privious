using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Msg;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Msg.Impl;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.BO.Msg
{
    /// <summary>
    /// 注释：通用业务类
    /// 作者：guansl
    /// 日期：2018/4/19 
    /// </summary>
   public class MsgMasterBO
    {
        #region ioc
        //会在程序执行前初始化
        private static readonly UnityContainer Container = new UnityContainer();

        private static IMsgMaster MsgMaster()
        {
            //注入
            Container.RegisterType<IMsgMaster, MsgMasterImpl>();
            //反转
            return Container.Resolve<IMsgMaster>();
        }
        #endregion

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">主键编码</param>
        /// <returns></returns>
        public Msg_Master GetMsgMaster(int id)
        {
            return MsgMaster().GetMsgMaster(id);
        }

       /// <summary>
        ///  新增数据
       /// </summary>
        /// <param name="eo">实体</param>
       /// <returns></returns>
        public int Add(Msg_Master eo)
        {
            return MsgMaster().Add(eo);
        }
    }
}
