using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.CarSerialLabel.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.WebAPI.App_Start;
using XYAuto.BUOC.IP2017.WebAPI.Filter;

namespace XYAuto.BUOC.IP2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
    public class CarSerialLabelController : ApiController
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1309;
                }
                return _currentUserID;
            }
        }
        #endregion        
        #region 标签录入列表接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult InputListCar([FromUri] ReqInputListCarDto request)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.CarSerialLabel.CarSerialLabel.Instance.InputListCar(request, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]InputListCar：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 打标签批次列表接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult BatchListCar([FromUri] ReqInputListCarDto request)
        {
            string errmsg = string.Empty;
            try
            {
                if (!request.CheckSelfModel(out errmsg))
                    return WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);

                request.CurrentUserID = currentUserID;
                var result = new BLL.Business.Query.V1_2_4.BatchListCarQuery().GetQueryList(request);
                return WebAPI.Common.Util.GetJsonDataByResult(result);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]BatchListCar：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 打标签车型渲染接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult RenderBatchCar([FromUri] ReqRenderBatchCarDto request)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.CarSerialLabel.CarSerialLabel.Instance.RenderBatchCar(request, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[CarSerialLabelController]RenderBatchCar：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 查看车型标签已审批次详情接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult ViewBatchCar(int BatchMediaID)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.CarSerialLabel.CarSerialLabel.Instance.ViewBatchCar(BatchMediaID, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[CarSerialLabelController]ViewBatchCar：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 打标签车型提交接口
        [HttpPost]
        [ApiLog]
        public Common.JsonResult BatchCarSubmit([FromBody] ReqBatchCarSubmitDto request)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.CarSerialLabel.CarSerialLabel.Instance.BatchCarSubmit(request, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[CarSerialLabelController]BatchCarSubmit：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
    }
}
