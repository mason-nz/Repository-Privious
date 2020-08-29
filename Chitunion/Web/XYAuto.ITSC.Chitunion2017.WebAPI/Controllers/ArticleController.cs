using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ThoughtWorks.QRCode.Codec;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ArticleController : ApiController
    {
        [HttpGet]
        public JsonResult GetWeixinArticleGroupList([FromUri]GetWeixinArticleGroupListReqDTO req) {

            var res = BLL.WxEditor.WxArticleGroup.Instance.GetWeixinArticleGroupList(req);
            return Util.GetJsonDataByResult(res);
        }

        [HttpGet]
        public JsonResult GetWeixinArticleList([FromUri]GetWeixinArticleGroupListReqDTO req)
        {
            GetWeixinArticleListResDTO res = new GetWeixinArticleListResDTO();
            if (req.IsGoodTJ)
                res = BLL.WxEditor.WxArticleGroup.Instance.GetWeixinGoodArticleTJList(req);
            else
                res = BLL.WxEditor.WxArticleGroup.Instance.GetWeixinArticleList(req);
            return Util.GetJsonDataByResult(res);
        }

        //[HttpPost]
        //public JsonResult ImportWeixinArticle([FromBody]ImportWeixinArticleReqDTO req)
        //{
        //    return Util.GetJsonDataByResult(null, "Success");
        //}

        [HttpPost]
        public JsonResult UploadWeixinArticleGroup([FromBody]UploadWeixinArticleGroupReqDTO req)
        {
            string msg = string.Empty;
            var res = BLL.WxEditor.WxArticleGroup.Instance.UploadWeixinArticleGroup(req, ref msg);
            return Util.GetJsonDataByResult(res, res ? "Success" : msg, res ? 0 : 1);
        }

        [HttpPost]
        public JsonResult ModifyArticle([FromBody]ModifyArticleReqDTO req)
        {
            string msg = "保存失败";
            if (!req.CheckSelfModel(out msg))
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
            int groupID = 0;
            int articleID = 0;
            var res = BLL.WxEditor.WxArticleGroup.Instance.ModifyArticle(req, ref msg, ref groupID, ref articleID);
            return Util.GetJsonDataByResult(new { GroupID = groupID, ArticleID = articleID }, res ? "Success" : msg, res ? 0 : -1);
        }

        //[HttpPost]
        //public JsonResult MoveArticle([FromBody]MoveArticleReqDTO req)
        //{
        //    var res = BLL.WxEditor.WxArticleGroup.Instance.MoveArticle(req);
        //    return Util.GetJsonDataByResult(res, res ? "Success" : "Fail", res ? 0 : -1);
        //}

        [HttpPost]
        public JsonResult BatchDelete([FromBody]BatchDeleteArticleReqDTO req)
        {
            bool res = false;
            string msg = string.Empty;
            if (req.GroupID > 0)
                res = BLL.WxEditor.WxArticleGroup.Instance.DeleteArticleGroup(req.GroupID, ref msg);
            else
                res = BLL.WxEditor.WxArticleGroup.Instance.DeleteArticle(req.ArticleIDs, ref msg);
            return Util.GetJsonDataByResult(res, res ? "Success" : msg, res ? 0 : 1);
        }

        [HttpGet]
        public JsonResult GetArticleView(int ArticleID, int OptType)
        {
            string url = string.Empty;
            string msg = string.Empty;
            bool res = BLL.WxEditor.WxArticleGroup.Instance.CreateArticlePage(ArticleID, OptType, ref url, ref msg);
            return Util.GetJsonDataByResult(url, res ? "Success" : msg, res ? 0 : 1);
        }

        [HttpGet]
        public HttpResponseMessage GetQRCode(int ArticleID)
        {
            MemoryStream ms = null;
            try
            {
                //创建二维码生成类  
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                //设置编码模式  
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                //设置编码测量度  
                qrCodeEncoder.QRCodeScale = 10;
                //设置编码版本  
                qrCodeEncoder.QRCodeVersion = 0;
                //设置编码错误纠正  
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                //生成二维码图片  

                var one = BLL.WxEditor.WxArticleGroup.Instance.GetArticleInfoByID(ArticleID);
                string hrefUrl = string.Empty;
                if (one != null && !string.IsNullOrWhiteSpace(one.MobileViewUrl))
                    hrefUrl = one.MobileViewUrl;
                Bitmap image = qrCodeEncoder.Encode(hrefUrl);
                ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
