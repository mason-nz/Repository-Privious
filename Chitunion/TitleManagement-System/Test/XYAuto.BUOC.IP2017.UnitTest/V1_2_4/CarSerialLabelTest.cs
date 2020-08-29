using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.BUOC.IP2017.WebAPI.Controllers;
using Newtonsoft.Json;

namespace XYAuto.BUOC.IP2017.UnitTest.V1_2_4
{
    [TestClass]
    public class CarSerialLabelTest
    {
        private CarSerialLabelController ctl = new CarSerialLabelController();
        [TestMethod]
        public void TestInputListCar()
        {
            var ret = ctl.InputListCar(new BLL.Business.DTO.RequestDto.V1_2_4.ReqInputListCarDto()
            {
                MasterId = 2,
                BrandID = 218,
                LabelStatus=1
            });
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestRenderBatchCar()
        {
            var ret = ctl.RenderBatchCar(new BLL.CarSerialLabel.DTO.RequestDto.V1_2_4.ReqRenderBatchCarDto()
            {
                BrandID = 218,
                SerialID = 2178
            });
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestBatchCarSubmit()
        {
            var ret = ctl.BatchCarSubmit(new BLL.CarSerialLabel.DTO.RequestDto.V1_2_4.ReqBatchCarSubmitDto()
            {
                BrandID = 218,
                SerialID = 2178,
                IPLabel = new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitIplabelDto>() {
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
                            },
                            new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitSubipDto() {
                                DictId=57,
                                DictName="友爱",
                                Label=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto>() {
                                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                                        DictName="标签-友爱1"
                                    },
                                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                                        DictName="标签-友爱2"
                                    }
                                }
                            }
                        }
                    },
                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitIplabelDto() {
                        DictId=383,
                        DictName="修复",
                        SubIP=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitSubipDto>() {                            
                            new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitSubipDto() {
                                DictId=364,
                                DictName="自我调节",
                                Label=new System.Collections.Generic.List<BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto>() {
                                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                                        DictName="标签-自我调节1"
                                    },
                                    new BLL.BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitTitleDto() {
                                        DictName="标签-自我调节2"
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
        public void TestBatchListCar()
        {
            var ret = ctl.BatchListCar(new BLL.Business.DTO.RequestDto.V1_2_4.ReqInputListCarDto() {
                MasterId=2,
                BrandID = 218
            });
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestViewBatchCar()
        {
            var ret = ctl.ViewBatchCar(8);
            string str = JsonConvert.SerializeObject(ret.Result);
            Assert.AreEqual(0, ret.Status);
        }
    }
}
