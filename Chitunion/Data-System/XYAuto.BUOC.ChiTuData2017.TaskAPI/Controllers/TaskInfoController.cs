using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.TaskAPI.Common;
using XYAuto.BUOC.ChiTuData2017.TaskAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.TaskAPI.Controllers
{
    public class TaskInfoController : ApiController
    {
        /// <summary>
        /// 获取秘钥
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public HttpResponseMessage GetAppKey([FromBody]RequestBase req)
        {
            string code = string.Empty;
            string msg = string.Empty;
            return BLL.TaskInfo.TaskInfo.Instance.toJson(BLL.TaskInfo.TaskInfo.Instance.GetAppKey(req, ref code, ref msg));
        }



        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult GetTaskList([FromBody]RequestGetTaskList req)
        {
            string code = string.Empty;
            string msg = string.Empty;
            return Util.GetJsonDataByResult(BLL.TaskInfo.TaskInfo.Instance.GetTaskList(req, ref code, ref msg),code,msg);
        }


        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public HttpResponseMessage GetTaskStatus([FromBody]RequestGetTaskStatus req)
        {
            string code = string.Empty;
            string msg = string.Empty;
            return BLL.TaskInfo.TaskInfo.Instance.toJson(BLL.TaskInfo.TaskInfo.Instance.GetTaskStatus(req, ref code, ref msg));
        }


        /// <summary>
        /// 获取任务素材列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult GetTaskMaterialList([FromBody]RequestTaskMaterialList req)
        {
            string code = string.Empty;
            string msg = string.Empty;
            return Util.GetJsonDataByResult(BLL.TaskInfo.Material.Instance.GetTaskMaterialList(req,ref code,ref msg), code, msg);
        }

        /// <summary>
        /// 获取任务推广链接
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public HttpResponseMessage GetTaskOrderUrl([FromBody]RequestTaskOrderUrl req)
        {
            string code = string.Empty;
            string msg = string.Empty;
            return BLL.TaskInfo.TaskInfo.Instance.toJson(BLL.TaskInfo.TaskInfo.Instance.GetTaskOrderUrl(req, ref code, ref msg));
        }


        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult GetStatisticsByOrderUrl([FromBody]RequestGetStatisticsByOrderUrl req)
        {
            string code = string.Empty;
            string msg = string.Empty;
            return Util.GetJsonDataByResult(BLL.TaskInfo.Statistics.Instance.GetStatisticsByOrderUrl(req, ref code, ref msg), code, msg);
        }
    }
}