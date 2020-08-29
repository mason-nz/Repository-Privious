using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.App;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    /// <summary>
    /// 注释：ShareProviderTest
    /// 作者：lix
    /// 日期：2018/5/22 17:21:48
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class ShareProviderTest
    {
        [TestMethod]
        public void GetSuccessTipsDtos()
        {
            var tips = new ShareProvider(new ConfigEntity(), new ReqCreateShareDto()).GetSuccessTipsDtos(
                  LeShareDetailTypeEnum.首次欢迎分享);
            Console.WriteLine(tips);
            Console.WriteLine(string.Format(tips, 0.87));
        }

        [TestMethod]
        public void VerifyOfShareDetail()
        {
            var retValue =
                new ShareProvider(new ConfigEntity() { CreateUserId = 3466 }, new ReqCreateShareDto()
                {
                    ShareType = (int)LeShareDetailTypeEnum.提现分享
                }).VerifyOfShareDetail(new ReturnValue());
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void Share()
        {
            var reqCreate = new ReqCreateShareDto()
            {
                ShareType = 202006,
                ShareContent = new ShareContentDto()
                {
                    TaskId = 0,
                    ShareContentType = 208002,
                    Ip = "127.0.0.1",
                    ShareUrl = "http://app1.chitunion.com/images/poster.png"
                }
            };
            var retValue = new ShareProvider(new ConfigEntity()
            {
                CreateUserId = 3489
            }, reqCreate).Log();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }
    }
}
