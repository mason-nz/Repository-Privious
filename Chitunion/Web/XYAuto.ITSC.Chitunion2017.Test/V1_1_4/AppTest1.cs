using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_4;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using System.Collections.Generic;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_4
{
    [TestClass]
    public class AppTest1
    {
        /// <summary>
        /// 测试添加下单备注
        /// </summary>
        [TestMethod]
        public void InsertMediaRemark()
        {
            string f;
            MediaCommonInfo.Instance.InsertMediaRemark(MediaRelationType.BaseTable, 45003, 1, new List<int> { 40001, 40003 }, "sdf", out f);
            Console.WriteLine(f);
        }

        /// <summary>
        /// 测试审核媒体
        /// </summary>
        [TestMethod]
        public void ToExamineMedia()
        {
            int NextMediaID;
            #region 0：审核状态不在枚举内
            string result1 = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToExamineMedia(89, 14002, "", 0, out NextMediaID);
            Console.WriteLine(result1);
            Console.WriteLine(NextMediaID);
            #endregion

            #region 审核通过
            string result2 = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToExamineMedia(89, 14002, "", 43002, out NextMediaID);
            Console.WriteLine(result2);
            Console.WriteLine(NextMediaID);
            #endregion



            #region 审核不通过
            string result3 = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToExamineMedia(77, 14002, "不想通过", 43003, out NextMediaID);
            Console.WriteLine(result3);
            Console.WriteLine(NextMediaID);
            #endregion
            // ToExamineMedia(89);
        }
        /// <summary>
        /// 审核媒体递归方法
        /// </summary>
        /// <param name="MediaID"></param>
        void ToExamineMedia(int MediaID)
        {
            #region 审核通过
            int NextMediaID;
            string result2 = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToExamineMedia(MediaID, 14002, "", 43002, out NextMediaID);
            Console.WriteLine(result2);
            Console.WriteLine(NextMediaID);
            #endregion
            if (NextMediaID > 0)
            {
                ToExamineMedia(NextMediaID);
            }
        }
        /// <summary>
        /// 测试添加媒体案列
        /// </summary>
        [TestMethod]
        public void InsertMediaCaseInfo()
        {
            Entities.Media.MediaCase mc = new Entities.Media.MediaCase();
            mc.CaseContent = @"<p>自1886年汽车诞生以来，就始终伴随着人们内心不灭的希望与追求，为人们把理想和远方联系起来。</p><p>　　约47年前，互联网的诞生，帮助人们跨越时空，链接彼此，几十年来，互联网始终伴随着人们不断探索的步伐，通过电子信息，穿越物理现实，为人们打开了心向往之的虚拟空间。</p><p>　　2017年4月18日，“幸福之路”行圆公司发布会在<span style=""text - decoration:underline; ""><span style=""""color:#0066cc"">上海</span></span>举行，这个成立已数月的新兴企业，要发布一个大愿望，携手汽车和互联网两大产业，实现产业信息的融合，帮助人们连接世界，通往幸福，铺就一条“幸福之路”。</p><p>　　本次发布会共吸引包括行业协会、汽<span style=""text-decoration:underline;""><span style=""color:#0066cc"">车厂</span></span>商、<span style=""text-decoration:underline;""><span style=""color:#0066cc"">经销商</span></span>和相关媒体在内的300余位嘉宾参加。会上，行圆公司发布了行圆汽车品牌、旗下三大业务平台及三大在线产品。并由行圆汽车创始人邵京宁先生阐释了产业互联网时代的行圆模式。?</p><p><br/></p><p style=""text-align: center;""><img title=""img2.png"" alt=""img2.png"" src=""http://www.chitunion.com/UploadFiles/2017/05/16/14/img2$-2147483648.png""/></p><p><img title=""img3.png"" alt=""img3.png"" src=""http://www.chitunion.com/UploadFiles/2017/05/16/14/img3$-2147483648.png""/></p>";
            mc.CaseStatus = 1;
            mc.MediaType = EnumMediaType.APP;
            mc.MediaID = 89;
            string result = BLL.Media.Business.V1_1.MediaCase.Instance.InsertMediaCaseInfo(mc);
            if (result == "")
            {
                result = "测试成功";
            }
            Console.WriteLine(result);

        }
        /// <summary>
        /// 删除媒体案例
        /// </summary>
        [TestMethod]
        public void DeleteMediaCaseByMidAndMtype()
        {
            //ID不存在
            string Msg2;
            int result2 = BLL.Media.Business.V1_1.MediaCase.Instance.DeleteMediaCaseByMidAndMtype(99999, 14002, out Msg2);
            if (result2 > 0)
            {
                Msg2 = "测试成功";
            }
            Console.WriteLine(result2);
            Console.WriteLine(Msg2);

            //类型不在枚举内
            string Msg1;
            int result1 = BLL.Media.Business.V1_1.MediaCase.Instance.DeleteMediaCaseByMidAndMtype(89, 888, out Msg1);
            if (result1 > 0)
            {
                Msg1 = "测试成功";
            }
            Console.WriteLine(result1);
            Console.WriteLine(Msg1);


            //正确参数
            string Msg;
            int result = BLL.Media.Business.V1_1.MediaCase.Instance.DeleteMediaCaseByMidAndMtype(89, 14002, out Msg);
            if (result > 0)
            {
                Msg = "测试成功";
            }
            Console.WriteLine(result);
            Console.WriteLine(Msg);
        }
        /// <summary>
        /// 测试删除接口
        /// </summary>
        [TestMethod]
        public void ToDeleteMedia()
        {
            //媒体类型错误
            string result1 = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToDeleteMedia(69979, 22);
            if (result1 == "")
            {
                result1 = "测试成功";
            }
            Console.WriteLine(result1);

            //运营角色删除媒体ID不存在，媒体主或AE角色删除媒体id存在（刊例不存在）
            string result = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToDeleteMedia(14002, 529);
            if (result == "")
            {
                result = "测试成功";
            }
            Console.WriteLine(result);

            //媒体主角色删除媒体（刊例存在）
            string result2 = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToDeleteMedia(14002, 87);
            if (result2 == "")
            {
                result2 = "测试成功";
            }
            Console.WriteLine(result2);

            //运营角色删除媒体（刊例存在）
            string result3 = BLL.Media.Business.V1_1.MediaStatusOperate.Instance.ToDeleteMedia(14002, 10);
            if (result3 == "")
            {
                result3 = "测试成功";
            }
            Console.WriteLine(result3);
        }
        /// <summary>
        /// 测试删除模板
        /// </summary>
        [TestMethod]
        public void ToDeleteAppTemplate()
        {
            //模板ID不存在
            string Msg;
            int result = BLL.AppTemplate.Instance.ToDeleteAppTemplate(88, out Msg);
            if (result > 0)
            {
                Msg = "测试成功";
            }
            Console.WriteLine(result);
            Console.WriteLine(Msg);
            //模板ID存在
            string Msg1;
            int result1 = BLL.AppTemplate.Instance.ToDeleteAppTemplate(19, out Msg1);
            if (result1 > 0)
            {
                Msg1 = "测试成功";
            }
            Console.WriteLine(result1);
            Console.WriteLine(Msg1);
        }
        /// <summary>
        /// 测试查询收藏拉黑列表
        /// </summary>
        [TestMethod]
        public void SelectCollectionPullBlack()
        {
            //参数类型错误
            string Msg;
            ListTotal t = BLL.Media.Business.V1_4.MediaCommonInfo.Instance.SelectCollectionPullBlack(0, 1, 5, out Msg);
            Console.WriteLine(Msg);


            //正确参数
            string Msg1;
            ListTotal t1 = BLL.Media.Business.V1_4.MediaCommonInfo.Instance.SelectCollectionPullBlack(2, 1, 5, out Msg1);
            Console.WriteLine(Msg1);
        }
        /// <summary>
        /// 测试查询排名前十城市的数据
        /// </summary>
        [TestMethod]
        public void SelectTopTenCitys()
        {
            DataTable dt = BLL.AppTemplate.Instance.SelectTopTenCitys();
        }
        /// <summary>
        /// 测试查询项目和订单列表
        /// </summary>
        [TestMethod]
        public void SelectOrderInfo()
        {
            ////查询项目列表
            //Dictionary<string, object> dic1 = BLL.ADOrderInfo.Instance.SelectOrderInfo(0, "4324", "23423", 14001, "324234", 10, 1, 16001, "",0,0);
            ////查询订单列表
            //Dictionary<string, object> dic2 = BLL.ADOrderInfo.Instance.SelectOrderInfo(1, "4324", "23423", 14001, "324234", 10, 1, 16001, "",0,0);
        }
        /// <summary>
        /// 测试查询购物车详情
        /// </summary>
        [TestMethod]
        public void SelectShoppingAppPublish()
        {
            Dictionary<string, object> dic = BLL.Periodication.Instance.SelectShoppingAppPublish(9, 89);
        }




    }
}
