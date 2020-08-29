using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;
using static XYAuto.ITSC.Chitunion2017.WebAPI.Controllers.ShoppingCartV1_1Controller;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_4
{
    //[TestFixture]
    [TestClass]
    public class ShoppingCartTest
    {
        private ShoppingCartV1_1Controller ctl = new ShoppingCartV1_1Controller();
        private XYAuto.ITSC.Chitunion2017.WebAPI.Common.JsonResult ret = null;
        //[SetUp]
        public void ShoppingCartTestHelper()
        {
            
        }
        //[Test]
        [TestMethod]
        public void AddShoppingCartAPPTest()
        {
            BLL.ShoppingCart.Dto.RequestAPPAddShoppingDto reqDto = new BLL.ShoppingCart.Dto.RequestAPPAddShoppingDto() {
                MediaType = 14002,
                IDs = new List<BLL.ShoppingCart.Dto.RequestAPPAddShppingIDDto>() {
                    new BLL.ShoppingCart.Dto.RequestAPPAddShppingIDDto() {
                        MediaID=11,
                        PublishDetailID=8121,
                        SaleAreaID=2501,
                        ADLaunchDays=10,
                        ADSchedule=new List<BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto>() {
                            new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                                BeginData=DateTime.Now.AddMonths(1),
                                EndData=DateTime.Now.AddMonths(2)
                            }
                            ,
                            new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                                BeginData=DateTime.Now.AddMonths(1).AddDays(-2),
                                EndData=DateTime.Now.AddMonths(1).AddDays(-1)
                            }
                            ,
                            new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                                BeginData=DateTime.Now.AddMonths(2).AddDays(1),
                                EndData=DateTime.Now.AddMonths(2).AddDays(2)
                            }
                        }
                    }
                    //,
                    //new BLL.ShoppingCart.Dto.RequestAPPAddShppingIDDto() {
                    //    MediaID=89,
                    //    PublishDetailID=1083,
                    //    SaleAreaID=1002,
                    //    ADLaunchDays=100,
                    //    ADSchedule=new List<BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto>() {
                    //        new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                    //            BeginData=DateTime.Now.AddMonths(1),
                    //            EndData=DateTime.Now.AddMonths(2)
                    //        },
                    //        new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                    //            BeginData=DateTime.Now.AddMonths(1).AddDays(-2),
                    //            EndData=DateTime.Now.AddMonths(1).AddDays(-1)
                    //        }
                    //        ,
                    //        new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                    //            BeginData=DateTime.Now.AddMonths(1).AddDays(-4),
                    //            EndData=DateTime.Now.AddMonths(1).AddDays(-3)
                    //        }
                    //    }
                    //}
                    //,
                    //new BLL.ShoppingCart.Dto.RequestAPPAddShppingIDDto() {
                    //    MediaID=521,
                    //    PublishDetailID=1078,
                    //    SaleAreaID=1001,
                    //    ADLaunchDays=100,
                    //    ADSchedule=new List<BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto>() {
                    //        new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                    //            BeginData=DateTime.Now.AddMonths(1),
                    //            EndData=DateTime.Now.AddMonths(2)
                    //        },
                    //        new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                    //            BeginData=DateTime.Now.AddMonths(1).AddDays(-2),
                    //            EndData=DateTime.Now.AddMonths(1).AddDays(-1)
                    //        }
                    //        ,
                    //        new BLL.ShoppingCart.Dto.RequestAPPAddShppingADScheduleDto() {
                    //            BeginData=DateTime.Now.AddMonths(1).AddDays(-4),
                    //            EndData=DateTime.Now.AddMonths(1).AddDays(-3)
                    //        }
                    //    }
                    //}
                }
            };

            ret = ctl.AddShoppingCartAPP(reqDto);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void AddShoppingCartTest()
        {
            JSONAddCart reqDto = new JSONAddCart() {
                MediaType=14001,
                IDs=new List<JSONAPP>() {
                    new JSONAPP() {
                        MediaID=16682,
                        PublishDetailID=187342,
                        ADSchedule=DateTime.Now.AddDays(2)
                    },
                    new JSONAPP() {
                        MediaID=16682,
                        PublishDetailID=187342,
                        ADSchedule=DateTime.Now.AddDays(3)
                    }
                }
            };

            ret = ctl.AddShoppingCart(reqDto);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        //[Test]
        [TestMethod]
        public void GetInfo_ShoppingCartTest()
        {
            ret = ctl.GetInfo_ShoppingCart();
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        //[Test]
        [TestMethod]
        public void DeleteShoppingCartTest()
        {
            ret = ctl.DeleteShoppingCart("5791,5792,5793");
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

         
        //[Test]
        [TestMethod]
        public void Delivery_ShoppingCartTest()
        {
            BLL.ShoppingCart.Dto.RequestDeliveryShoppingDto req = new BLL.ShoppingCart.Dto.RequestDeliveryShoppingDto()
            {
                OrderID = "CT20170704114",
                IDs = new List<BLL.ShoppingCart.Dto.RequestDeliveryShoppingIDDto>() {
                new BLL.ShoppingCart.Dto.RequestDeliveryShoppingIDDto() {
                    MediaType=14002,
                    CartID=5800,
                    MediaID=11,
                    PublishDetailID=8121,
                    SaleAreaID=2501,
                    ADLaunchDays=10,
                    IsSelected=1
                }
            }
            };
            ret = ctl.Delivery_ShoppingCart(req);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void ADScheduleOpt_ShoppingCartTest()
        {
            BLL.ShoppingCart.Dto.RequestADScheduleOptDto req = new BLL.ShoppingCart.Dto.RequestADScheduleOptDto() {
                OptType=1,
                MediaType=14002,
                CartID= 5801,
                RecID= 641,
                BeginTime=DateTime.Now.AddMonths(5),
                EndTime=DateTime.Now.AddMonths(5).AddDays(1)
            };
            ret = ctl.ADScheduleOpt_ShoppingCart(req);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void OrderIDName_FuzzyQueryTest()
        {
            ret = ctl.OrderIDName_FuzzyQuery();
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void PubDetailVertify_ADOrderOrCartTest()
        {
            BLL.ShoppingCart.Dto.RequestPubDetailVertifyDto req = new BLL.ShoppingCart.Dto.RequestPubDetailVertifyDto() {
                OrderID= "CT20170615667",
                Media=new List<BLL.ShoppingCart.Dto.RequestMediaDto>() {
                    new BLL.ShoppingCart.Dto.RequestMediaDto() {
                        MediaType=14002,
                        CartID=5794,
                        PublishDetailID=1080,
                        SaleAreaID=1001
                    }
                }
            };
            ret = ctl.PubDetailVertify_ADOrderOrCart(req);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void QueryHolidaysTest()
        {
            ret = ctl.QueryHolidays(DateTime.Now.AddMonths(-5),DateTime.Now.AddMonths(6));
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
    }
}
