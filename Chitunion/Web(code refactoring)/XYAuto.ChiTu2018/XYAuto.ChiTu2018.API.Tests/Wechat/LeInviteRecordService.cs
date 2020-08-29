using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.Service.Wechat.Dto;

namespace XYAuto.ChiTu2018.API.Tests.Wechat
{
    /// <summary>
    /// 注释：LeInviteRecordService
    /// 作者：zhanglb
    /// 日期：2018/5/17 16:53:04
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class LeInviteRecordService
    {

        /// <summary>
        /// 关注公众号
        /// </summary>
        [TestMethod]
        public void FriendFollow()
        {
            //Console.WriteLine("用户ID不存在");
            //Service.Wechat.LeInviteRecordService.Instance.FriendFollow(5);
            //Console.WriteLine("\r\n");

            //Console.WriteLine("用户存在邀请人不存在");
            //Service.Wechat.LeInviteRecordService.Instance.FriendFollow(1635);
            //Console.WriteLine("\r\n");


            Console.WriteLine("用户存在邀请人存在");
            Service.Wechat.LeInviteRecordService.Instance.FriendFollow(1701);
            Console.WriteLine("\r\n");
        }
        /// <summary>
        /// 领取红包
        /// </summary>
        [TestMethod]
        public void ReceiveRedEves()
        {
            Console.WriteLine("用户存在邀请人存在");
            decimal price;
            string errorMsg = Service.Wechat.LeInviteRecordService.Instance.ReceiveRedEves(new ReqInviteRecIdDto() { RecId = 175 }, out price);
            Console.WriteLine("领取金额：" + price);
            Console.WriteLine(errorMsg);
            Console.WriteLine("\r\n");
        }
        /// <summary>
        /// 获取邀请人列表
        /// </summary>
        [TestMethod]
        public void GetBeInvitedList()
        {
            Console.WriteLine("用户存在");
            var dic = Service.Wechat.LeInviteRecordService.Instance.GetBeInvitedList(100,50000);
            Console.WriteLine(JsonConvert.SerializeObject(dic));
            Console.WriteLine("\r\n");
        }
    }
}
