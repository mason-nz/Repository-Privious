using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1
{
    [TestClass]
    public class CollectPullBackTest
    {
        private AddToCollectPullBackDto requestDto;
        private RemoveCollectPullBackDto pullBackRequestDto;

        public CollectPullBackTest()
        {
            requestDto = new AddToCollectPullBackDto()
            {
                BusinessType = (int)MediaType.WeiXin,
                MediaId = 100,
                OperateType = (int)CollectPullBackTypeEnum.Collection,
                CreateUserId = 100
            };
            pullBackRequestDto = new RemoveCollectPullBackDto()
            {
                BusinessType = (int)MediaType.WeiXin,
                MediaId = 100,
                OperateType = (int)CollectPullBackTypeEnum.PullBack,
                CreateUserId = 100
            };
        }

        [TestMethod]
        public void CollectPullBack_add_VerifyFalse_OperateType_Test()
        {
            requestDto.OperateType = 0;
            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void CollectPullBack_add_VerifyFalse_params_Test()
        {
            requestDto.OperateType = (int)CollectPullBackTypeEnum.Collection;
            requestDto.BusinessType = 0;
            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void CollectPullBack_add_Test()
        {
            requestDto.OperateType = (int)CollectPullBackTypeEnum.Collection;
            requestDto.BusinessType = (int)MediaType.WeiXin;
            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("重复添加测试")]
        [TestMethod]
        public void CollectPullBack_add_VerifyFalse_mediaId_Test()
        {
            requestDto.OperateType = (int)CollectPullBackTypeEnum.Collection;
            requestDto.BusinessType = 0;
            //requestDto.MediaId = 100;
            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void Remove_Collection_verifyfalse_operateType_Test()
        {
            pullBackRequestDto.OperateType = 0;
            pullBackRequestDto.BusinessType = (int)MediaType.WeiXin;
            var retValue = new CollectPullBackProxy(pullBackRequestDto).RemoveExcute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void Remove_Collection_verifyfalse_mediaId_Test()
        {
            pullBackRequestDto.OperateType = (int)CollectPullBackTypeEnum.Collection;
            pullBackRequestDto.BusinessType = (int)MediaType.WeiXin;
            pullBackRequestDto.MediaId = 0;
            var retValue = new CollectPullBackProxy(pullBackRequestDto).RemoveExcute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void Remove_Collection_Test()
        {
            pullBackRequestDto.OperateType = (int)CollectPullBackTypeEnum.Collection;
            pullBackRequestDto.BusinessType = (int)MediaType.WeiXin;
            pullBackRequestDto.MediaId = 100;
            var retValue = new CollectPullBackProxy(pullBackRequestDto).RemoveExcute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void PullBack_add_VerifyFalse_OperateType_Test()
        {
            requestDto.OperateType = 0;
            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            Assert.IsTrue(retValue.HasError);
        }

        [Description("拉黑操作添加-验证参数-false验证")]
        [TestMethod]
        public void PullBack_add_VerifyFalse_params_Test()
        {
            requestDto.OperateType = (int)CollectPullBackTypeEnum.PullBack;
            requestDto.BusinessType = 0;
            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            Assert.IsTrue(retValue.HasError);
        }

        [Description("拉黑操作测试")]
        [TestMethod]
        public void PullBack_add_Test()
        {
            requestDto.OperateType = (int)CollectPullBackTypeEnum.PullBack;
            requestDto.BusinessType = 0;
            requestDto.BusinessType = (int)MediaType.WeiXin;

            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void Remove_PullBack_verifyfalse_operateType_Test()
        {
            pullBackRequestDto.OperateType = 0;
            pullBackRequestDto.BusinessType = (int)MediaType.WeiXin;
            var retValue = new CollectPullBackProxy(pullBackRequestDto).RemoveExcute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void Remove_PullBack_verifyfalse_mediaId_Test()
        {
            pullBackRequestDto.OperateType = (int)CollectPullBackTypeEnum.PullBack;
            pullBackRequestDto.BusinessType = (int)MediaType.WeiXin;
            pullBackRequestDto.MediaId = 0;
            var retValue = new CollectPullBackProxy(pullBackRequestDto).RemoveExcute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
            Assert.AreEqual(true, retValue.HasError);
        }

        [TestMethod]
        public void Remove_PullBack_Test()
        {
            pullBackRequestDto.OperateType = (int)CollectPullBackTypeEnum.PullBack;
            pullBackRequestDto.BusinessType = (int)MediaType.WeiXin;
            pullBackRequestDto.MediaId = 100;
            var retValue = new CollectPullBackProxy(pullBackRequestDto).RemoveExcute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void Remove_Rmove_Parmas_Test()
        {
            //var retValue = new CurrentOperateBase().VerifyOfNecessaryParameters(pullBackRequestDto);
            //Console.WriteLine(JsonConvert.SerializeObject(retValue));
            //requestDto.OperateType = (int)DataStatusEnum.Delete;//移除

            //Assert.IsFalse(retValue.HasError);
        }
    }
}