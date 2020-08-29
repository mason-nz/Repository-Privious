using System;
using XYAuto.ChiTu2018.BO.Msg;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.Service
{
    /// <summary>
    /// 注释：具体业务逻辑 将来列表成分微服务，实现自理
    /// 作者：guansl
    /// 日期：2018/4/19 
    /// </summary>
    public class Message
    {
        private Message() { }
        private static readonly Lazy<Message> linstance = new Lazy<Message>(() => { return new Message(); });

        public static Message Instance { get { return linstance.Value; } }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Msg_Master GetMsgMaster(int? id)
        {
            if (id.HasValue)
            {
                return new MsgMasterBO().GetMsgMaster(id.Value);
            }
            else
            {
                return new Msg_Master();
            }
        }
    }
}
