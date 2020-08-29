using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.WxOAuth.Model;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class WeixinOAuthController : ApiController
    {
        //[HttpPost]
        //public JsonResult Add(OAuthRequest request)
        //{
        //    if (request.Type.Equals((int)SourceTypeEnum.验证码) && !string.IsNullOrEmpty(request.YZM) && !string.IsNullOrEmpty(request.Url))
        //    {
        //        #region 验证码 and Url
        //        int recID = 0;
        //        bool yzmError = false;
        //        var wxCatch = BLL.WeixinOAuth.Instance.GetCatchDateByYZM(request.YZM, request.Url, ref yzmError);
        //        if (wxCatch != null)
        //        {
        //            recID = BLL.WeixinOAuth.Instance.UpdateWxInfo(wxCatch, null, SourceTypeEnum.验证码);
        //        }
        //        else if (yzmError)
        //        {
        //            recID = -1;//验证码错误 其他情况返回0
        //        }
        //        return Util.GetJsonDataByResult(new { AuthType = 38002, WxID = recID, WxNumber = string.Empty });
        //        #endregion
        //    }
        //    //else if ((ur.IsAE || ur.IsAdministrator) && request.Type.Equals((int)SourceTypeEnum.手工) && (!string.IsNullOrEmpty(request.Url) || !string.IsNullOrEmpty(request.WxNumber)))
        //    else if (request.Type.Equals((int)SourceTypeEnum.手工) && (!string.IsNullOrEmpty(request.Url) || !string.IsNullOrEmpty(request.WxNumber)))
        //    {
        //        #region 手工方式进入 微信号 or Url
        //        CatchWeixinModel wxCatch = null;
        //        int recID = 0;
        //        string biz = string.Empty;
        //        if (!string.IsNullOrEmpty(request.WxNumber))
        //        {//微信号
        //            recID = BLL.WeixinOAuth.Instance.GetWxIDByWxNumber(request.WxNumber);
        //            wxCatch = BLL.WeixinOAuth.Instance.GetCatchData(request.WxNumber);
        //        }
        //        else
        //        {//biz
        //            Regex reg = new Regex(@"biz=\D{16}");
        //            Match match = reg.Match(request.Url);
        //            if (match.Groups.Count > 0 && match.Groups[0].Value.Length > 4)
        //            {
        //                biz = match.Groups[0].Value.Substring(4);
        //                recID = BLL.WeixinOAuth.Instance.GetWxIDByBiz(biz);
        //                wxCatch = BLL.WeixinOAuth.Instance.GetCatchData("", biz);
        //            }
        //        }
        //        if (wxCatch != null)
        //        {
        //            recID = BLL.WeixinOAuth.Instance.UpdateWxInfo(wxCatch, null, SourceTypeEnum.验证码);
        //        }
        //        return Util.GetJsonDataByResult(new { AuthType = 38003, WxID = recID, WxNumber = request.WxNumber });
        //        #endregion
        //    }
        //    else if (request.Type.Equals((int)SourceTypeEnum.资质认证) && !string.IsNullOrEmpty(request.WxNumber))
        //    {
        //        #region 资质认证
        //        CatchWeixinModel wxCatch = null;
        //        int recID = 0;
        //        string biz = string.Empty;
        //        recID = BLL.WeixinOAuth.Instance.GetWxIDByWxNumber(request.WxNumber);
        //        wxCatch = BLL.WeixinOAuth.Instance.GetCatchData(request.WxNumber);
        //        if (wxCatch != null)
        //        {
        //            recID = BLL.WeixinOAuth.Instance.UpdateWxInfo(wxCatch, null, SourceTypeEnum.资质认证);
        //        }
        //        return Util.GetJsonDataByResult(new { AuthType = 38004, WxID = recID, WxNumber = request.WxNumber });
        //        #endregion
        //    }
        //    else
        //    {
        //        return Util.GetJsonDataByResult(null, "Parameters Error!", 1);
        //    }
        //}

        [HttpGet]
        public string Add([FromUri]OAuthRequest request)
        {
            if (request.Type.Equals((int)SourceTypeEnum.验证码) && !string.IsNullOrEmpty(request.YZM) && !string.IsNullOrEmpty(request.Url))
            {
                #region 验证码 and Url
                int recID = 0;
                bool yzmError = false;
                var wxCatch = BLL.WeixinOAuth.Instance.GetCatchDateByYZM(request.YZM, request.Url, ref yzmError);
                if (wxCatch != null)
                {
                    recID = BLL.WeixinOAuth.Instance.UpdateWxInfo(wxCatch, null, SourceTypeEnum.验证码);
                }
                else if (yzmError)
                {
                    recID = -1;//验证码错误 其他情况返回0
                }
                return request.jsonpcallback + "(" + JsonConvert.SerializeObject(Util.GetJsonDataByResult(new { AuthType = 38002, WxID = recID, WxNumber = string.Empty })) + ")";
                #endregion
            }
            //else if ((ur.IsAE || ur.IsAdministrator) && request.Type.Equals((int)SourceTypeEnum.手工) && (!string.IsNullOrEmpty(request.Url) || !string.IsNullOrEmpty(request.WxNumber)))
            else if (request.Type.Equals((int)SourceTypeEnum.手工) && (!string.IsNullOrEmpty(request.Url) || !string.IsNullOrEmpty(request.WxNumber)))
            {
                #region 手工方式进入 微信号 or Url
                CatchWeixinModel wxCatch = null;
                int recID = 0;
                string biz = string.Empty;
                if (!string.IsNullOrEmpty(request.WxNumber))
                {//微信号
                    recID = BLL.WeixinOAuth.Instance.GetWxIDByWxNumber(request.WxNumber);
                    wxCatch = BLL.WeixinOAuth.Instance.GetCatchData(request.WxNumber);
                }
                else
                {//biz
                    Regex reg = new Regex(@"biz=\D{16}");
                    Match match = reg.Match(request.Url);
                    if (match.Groups.Count > 0 && match.Groups[0].Value.Length > 4)
                    {
                        biz = match.Groups[0].Value.Substring(4);
                        recID = BLL.WeixinOAuth.Instance.GetWxIDByBiz(biz);
                        wxCatch = BLL.WeixinOAuth.Instance.GetCatchData("", biz);
                    }
                }
                if (wxCatch != null)
                {
                    recID = BLL.WeixinOAuth.Instance.UpdateWxInfo(wxCatch, null, SourceTypeEnum.验证码);
                }
                return request.jsonpcallback + "(" + JsonConvert.SerializeObject(Util.GetJsonDataByResult(new { AuthType = 38003, WxID = recID, WxNumber = request.WxNumber })) + ")";
                #endregion
            }
            else if (request.Type.Equals((int)SourceTypeEnum.资质认证) && !string.IsNullOrEmpty(request.WxNumber))
            {
                #region 资质认证
                CatchWeixinModel wxCatch = null;
                int recID = 0;
                string biz = string.Empty;
                recID = BLL.WeixinOAuth.Instance.GetWxIDByWxNumber(request.WxNumber);
                wxCatch = BLL.WeixinOAuth.Instance.GetCatchData(request.WxNumber);
                if (wxCatch != null)
                {
                    recID = BLL.WeixinOAuth.Instance.UpdateWxInfo(wxCatch, null, SourceTypeEnum.资质认证);
                }
                return request.jsonpcallback + "(" + JsonConvert.SerializeObject(Util.GetJsonDataByResult(new { AuthType = 38004, WxID = recID, WxNumber = request.WxNumber })) + ")";
                #endregion
            }
            else
            {
                return request.jsonpcallback + "(" + JsonConvert.SerializeObject(Util.GetJsonDataByResult(null, "Parameters Error!", 1)) + ")";
            }
        }


    }
}
