using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WechatUserMsg
{
    public class WechatUmsg
    {
        public static readonly WechatUmsg Instance = new WechatUmsg();
        /// <summary>
        /// 查询没有分发的用户opendID
        /// </summary>
        /// <returns></returns>
        public DataSet NoOrderOpenIds()
        {
            return Dal.WechatUserMsg.WechatUmsg.Instance.NoOrderOpenIds();
        }
        public DataTable OrderWeekProfitOpenIds()
        {
            return Dal.WechatUserMsg.WechatUmsg.Instance.OrderWeekProfitOpenIds();
        }
        public DataTable GetAllWechatUser()
        {
            return Dal.WechatUserMsg.WechatUmsg.Instance.GetAllWechatUser();
        }
        /// <summary>
        /// 查询“1元提现活动”新用户近2天收益数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetWechatUserBy1YuanTx(string startDate,string endDate)
        {
            return Dal.WechatUserMsg.WechatUmsg.Instance.GetWechatUserBy1YuanTx(startDate, endDate);
        }

    }
}
