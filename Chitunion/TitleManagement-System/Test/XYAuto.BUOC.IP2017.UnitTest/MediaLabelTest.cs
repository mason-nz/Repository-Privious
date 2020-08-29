using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.BUOC.IP2017.WebAPI.Controllers;
using Newtonsoft.Json;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.MediaLabel.DTO.RequestDto.V1_2_4;

namespace XYAuto.BUOC.IP2017.UnitTest
{
    [TestClass]
    public class MediaLabelTest
    {
        private MediaLabelController ctl = new MediaLabelController();
        [TestMethod]
        public void TestGetCommonlyClass()
        {
            var ret = ctl.GetCommonlyClass(14001);
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestInputListMedia()
        {
            //ReqInputListMediaDto req = new ReqInputListMediaDto() {
            //    MediaType=14002,
            //    Name= "柚宝宝"
            //};
            ReqInputListMediaDto req = new ReqInputListMediaDto()
            {
                MediaType = 14001,
                HasArticleType=3
                //Name = "qinghuanandu"
            };
            var ret = ctl.InputListMedia(req);
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestRenderBatchMedia()
        {
            //ReqBatchMediaDto req = new ReqBatchMediaDto()
            //{
            //    MediaType = 14001,
            //    NumberOrName= "chuangyiguan"//"yjbj010"
            //};
            ReqBatchMediaDto req = new ReqBatchMediaDto()
            {
                MediaType = 14003,
                NumberOrName = "芳芳测试"//"yjbj010"
            };
            var ret = ctl.RenderBatchMedia(req);
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestBatchMediaSubmit()
        {
            var ret = ctl.BatchMediaSubmit(new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitDto() {
                MediaType=14003,
                Name= "传媒头条2",
                Number= "111",
                Category=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto>() {
                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                        DictId=3307,
                        DictName="时事"
                    },
                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                        DictId=3308,
                        DictName="民生"
                    }
                },
                MarketScene=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto>() {
                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                        DictId=3336,
                        DictName="毕业"
                    }
                },
                DistributeScene=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto>() {
                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                        DictId=3353,
                        DictName="体育"
                    }
                },
                IPLabel=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitIplabelDto>() {
                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitIplabelDto() {
                        DictId=1,
                        DictName="萌爱",
                        SubIP=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitSubipDto>() {
                            new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitSubipDto() {
                                DictId=2,
                                DictName="可爱",
                                Label=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto>() {
                                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                                        DictName="标签-可爱1"
                                    },
                                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                                        DictName="标签-可爱2"
                                    }
                                }
                            }
                        }
                    }
                }
            });
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestArticleQueryOrRecive()
        {
            //var ret = ctl.ArticleQueryOrRecive(new ReqArticleQueryOrReciveDto()
            //{
            //    IsRecive = false,
            //    Resource = 1,
            //    Number = "lexiangg",
            //    ArticleCount = 10,
            //    StartDate = new DateTime(2017, 1, 1),
            //    EndDate = new DateTime(2017, 12, 31)
            //});
            var ret = ctl.ArticleQueryOrRecive(new ReqArticleQueryOrReciveDto()
            {
                IsRecive = true,
                Resource = 1,
                Number = "lexiangg",
                ArticleCount = 10,
                StartDate = new DateTime(2017, 1, 5),
                EndDate = new DateTime(2017, 10, 25)
            });
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestQueryArticleListByBactchID()
        {
            var ret = ctl.QueryArticleListByBactchID(new ReqArticleListByBactchIDQueryDto() {
                BatchMediaID= 17
            });
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestBatchListMedia()
        {
            var ret = ctl.BatchListMedia(new ReqBatchListMediaDto() {
                MediaType=14001,
                DictId=47001,
                Name=null,
                SelfDoBusiness=-2
            });
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestViewBatchMedia()
        {
            var ret = ctl.ViewBatchMedia(46);
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestStatisticsLabel()
        {
            var ret = BLL.MediaLabel.MediaLabel.Instance.StatisticsLabel(61);
            Assert.AreEqual(true, ret);
        }
        [TestMethod]
        public void TestGetSummaryKeyWord()
        {
            var ret = ctl.GetSummaryKeyWord(14001, 1, 50, 50);
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
    }
}
