using System.Web.Http;
using XYAuto.ChiTu2018.Service.LE;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MssageController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Route("message/")]
        public IHttpActionResult GetMsgMaster()
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default("DBLog").Info("this is log");
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("this is base log");
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("this is base error log");
            var info = LeWithdrawalsDetailService.Instance.GetById(24);
            return Ok(info);
        }
    }
}
