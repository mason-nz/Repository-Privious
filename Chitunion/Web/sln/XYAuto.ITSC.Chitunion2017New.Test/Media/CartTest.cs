using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017New.Test.Media
{
    [TestClass]
    public class CartTest
    {
  
        /// <summary>
        /// 添加购物车数据
        /// </summary>
        [TestMethod]
        public void AddCartInfo()
        {
            List<CartModel> list = new List<CartModel> {
                new CartModel {  MediaType=14001,MediaID=2},
                new CartModel {  MediaType=14001,MediaID=21}
            };
            var queryResult = CartBll.Instance.AddCartInfo(list, 1409);
        }
        /// <summary>
        /// 获取购物车列表
        /// </summary>
        [TestMethod]
        public void GetCartInfoList()
        {

            var queryResult = CartBll.Instance.GetCartInfoList(18);

            string tem = JsonConvert.SerializeObject(queryResult);
        }
        /// <summary>
        /// 删除购物车
        /// </summary>
        [TestMethod]
        public void DeleteCartInfo()
        {
            var queryResult = CartBll.Instance.DeleteCartInfo(new DeleteCartInfoDto
            {
                CartInfoList = new List<CartModel> {
               new CartModel {  MediaID=2,MediaType=14001},
               new CartModel {  MediaID=3,MediaType=14001},
               new CartModel {  MediaID=5,MediaType=14001}
            }
            });

            string tem = JsonConvert.SerializeObject(queryResult);
        }
        /// <summary>
        /// 获取购物车媒体列表
        /// </summary>
        [TestMethod]
        public void GetCartMediaList()
        {
            var queryResult = CartBll.Instance.GetCartMediaList(18);

            string tem = JsonConvert.SerializeObject(queryResult);
        }
        [TestMethod]
        public void GetCartExport()
        {
            CartBll.Instance.GetCartExport();
        }
        [TestMethod]
        public void TaskListInfo()
        {
           var query= CartBll.Instance.TaskListInfo(new QueryTaskargs ());

            string tem = JsonConvert.SerializeObject(query);
        }
        [TestMethod]
        public void GetSharingPlatformTest()
        {
            var query = XYAuto.ITSC.Chitunion2017.BLL.DictInfo.DictInfo.Instance.GetSharingPlatform(105);            
            string tem = JsonConvert.SerializeObject(query);
            
        }
    }
    
}
