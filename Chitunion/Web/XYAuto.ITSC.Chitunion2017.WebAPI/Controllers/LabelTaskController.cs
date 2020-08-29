using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
    public class LabelTaskController : ApiController
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1225;
                }
                return _currentUserID;
            }
        }
        private static string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        #endregion        

        #region 查询审核人列表
        [HttpGet]
        [ApiLog]
        public Common.JsonResult GetAuditUser()
        {
            try
            {
                var resDto = BLL.LabelTask.LabelTask.Instance.GetAuditUser();
                if (resDto.Count > 0)
                    return WebAPI.Common.Util.GetJsonDataByResult(resDto, "操作成功");
                return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]GetAuditUser出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 提交项目
        [HttpPost]
        [ApiLog]
        public Common.JsonResult LabelProjectCreate([FromBody] Entities.LabelTask.DTO.ReqProjectCreateDTO reqDto)
        {
            try
            {
                BLL.Loger.Log4Net.Info("提交项目-----接口开始");
                int projectID = -2;
                string msg = string.Empty;
                BLL.LabelTask.LabelTask.Instance.LabelProjectCreate(reqDto, out projectID, out msg);
                if (!string.IsNullOrEmpty(msg))
                    return Common.Util.GetJsonDataByResult(null, $"验证失败：{msg}", -1);
                BLL.Loger.Log4Net.Info("提交项目-----接口结束");
                return Common.Util.GetJsonDataByResult(projectID, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]LabelProjectCreate出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 未处理任务统计
        [HttpGet]
        [ApiLog]
        public Common.JsonResult LabelStatistics(int projectType, string uploadFileURL)
        {
            try
            {
                BLL.Loger.Log4Net.Info("任务统计-----接口开始");
                string msg = string.Empty;
                int taskCount = 0;
                BLL.LabelTask.LabelTask.Instance.LabelStatistics(projectType, uploadFileURL, out msg, out taskCount);
                if (!string.IsNullOrEmpty(msg))
                    return Common.Util.GetJsonDataByResult(null, $"验证失败：{msg}", -1);
                BLL.Loger.Log4Net.Info("任务统计-----接口结束");
                return Common.Util.GetJsonDataByResult(taskCount, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]任务统计出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 标签任务列表
        [HttpGet]
        [ApiLog]
        public Common.JsonResult LabelListQuery([FromUri]ReqLabelListQueryDTO reqDto)
        {
            try
            {
                string msg = string.Empty;
                var res = BLL.LabelTask.LabelTask.Instance.LabelListQuery(reqDto, out msg);
                if (!string.IsNullOrEmpty(msg))
                    return WebAPI.Common.Util.GetJsonDataByResult("验证出错", msg, -1);
                return WebAPI.Common.Util.GetJsonDataByResult(res);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]标签任务列表出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult("标签任务列表出错", ex.Message, -1);
            }
        }
        #endregion
        #region 领取标签任务
        [HttpGet]
        [ApiLog]
        public Common.JsonResult LabelReceiveTask()
        {
            try
            {
                string msg = string.Empty;
                var res = BLL.LabelTask.LabelTask.Instance.LabelReceiveTask(out msg);
                if (!string.IsNullOrEmpty(msg))
                    return WebAPI.Common.Util.GetJsonDataByResult("验证错误", msg, -1);
                if (res.Count == 0)
                    return WebAPI.Common.Util.GetJsonDataByResult("没有数据", msg, 0);
                return WebAPI.Common.Util.GetJsonDataByResult(res);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]领取标签任务出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult("出错", ex.Message, -1);
            }
        }
        #endregion
        #region 任务列表审核领取
        [HttpGet]
        [ApiLog]
        public Common.JsonResult LabelTaskAuditQuery(int taskID)
        {
            try
            {
                string msg = string.Empty;
                var res = BLL.LabelTask.LabelTask.Instance.LabelTaskAuditQuery(taskID, out msg);
                if (!string.IsNullOrEmpty(msg))
                    return WebAPI.Common.Util.GetJsonDataByResult("验证错误", msg, -2);

                if (res > 0)
                    return WebAPI.Common.Util.GetJsonDataByResult(res, "当前任务正被别人审核，系统会为您自动选择任务请稍后...", -1);
                else
                    return WebAPI.Common.Util.GetJsonDataByResult("操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]任务列表审核领取出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult("出错", ex.Message, -2);
            }
        }
        #endregion
        #region 取消任务审核
        [HttpGet]
        [ApiLog]
        public Common.JsonResult LabelTaskAuditCancel(int taskID)
        {
            try
            {
                var res = BLL.LabelTask.LabelTask.Instance.LabelTaskAuditCancel(taskID);
                return WebAPI.Common.Util.GetJsonDataByResult("操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]取消审核出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult("出错", ex.Message, -2);
            }
        }
        #endregion
        #region 删除停止项目
        [HttpGet]
        [ApiLog]
        public Common.JsonResult LabelProjectStatus(int projectID, int status)
        {
            try
            {
                string msg = string.Empty;
                BLL.LabelTask.LabelTask.Instance.LabelProjectStatus(projectID, status, out msg);
                if (!string.IsNullOrEmpty(msg))
                    return WebAPI.Common.Util.GetJsonDataByResult("验证错误", msg, -1);

                return WebAPI.Common.Util.GetJsonDataByResult("操作成功!");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]删除停止项目出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult("出错", ex.Message, -1);
            }
        }
        #endregion
        #region 查询项目列表
        /// <summary>
        /// 2017-08-11
        /// 查询项目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult ProjectListQuery(int pageIndex, int pageSize)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.LabelTask.LB_Project.Instance.SelectProjectList(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****ProjectListQuery -> 查询标签项目列表出错" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        #endregion
        #region 获取文章摘要关键词
        [HttpGet]
        [ApiLog]
        public Common.JsonResult GetSummaryKeyWord(int articleID, int summarySize, int keyWordSize)
        {
            try
            {
                var ret = BLL.LabelTask.LabelTask.Instance.GetSummaryKeyWord(articleID, summarySize, keyWordSize);
                return WebAPI.Common.Util.GetJsonDataByResult(ret);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[LabelTaskController]GetSummaryKeyWord出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        /// <summary>
        /// 2017-08-08 张立彬
        /// 打标签
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult InserLableTakeInfo([FromBody]ReqLableTaskDTO ReqDTO)
        {
            string strJson = Json.JsonSerializerBySingleData(ReqDTO);
            BLL.Loger.Log4Net.Info("[LabelTaskController]******InserLableTakeInfo begin...->ReqLableTaskDTO:" + strJson + "");

            string messageStr = "";
            try
            {
                int result = BLL.LabelTask.LB_Task.Instance.InserLableTakeInfo(ReqDTO, out messageStr);
                Common.JsonResult jr;
                if (result > 0)
                {
                    jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
                }
                else
                {
                    jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
                }
                BLL.Loger.Log4Net.Info("[LabelTaskController]******InserLableTakeInfo end->");
                return jr;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****InserLableTakeInfo ->ReqLableTaskDTO:" + strJson + "打标签出错:" + ex.Message);
                throw ex;
            }

        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询媒体或文章信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult SelectMediaOrArticleInfo(int TaskID)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.LabelTask.LB_Task.Instance.SelectMediaOrArticleInfo(TaskID);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****SelectMediaOrArticleInfo ->TaskID:" + TaskID + ",查询查询文章或媒体信息接口出错:" + ex.Message);
                throw ex;
            }

        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询媒体或文章信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult SelectMediaOrArticleLable(int TaskID, int SelectType)
        {
            try
            {
                string ErrorMessage = "";
                Dictionary<string, object> dicAll = BLL.LabelTask.LB_Task.Instance.SelectMediaOrArticleLable(TaskID, SelectType, out ErrorMessage);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****SelectMediaOrArticleInfo ->TaskID:" + TaskID + ",查询查询文章或媒体信息接口出错:" + ex.Message);
                throw ex;
            }

        }
        /// <summary>
        /// 2017-08-11
        /// 查询项目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult ProjectQuery(int projectID)
        {
            try
            {
                Entities.LabelTask.ProjectInfo projectInfo = BLL.LabelTask.LB_Project.Instance.SelectProjectInfo(projectID);
                return Common.Util.GetJsonDataByResult(projectInfo, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****SelectProjectInfo ->projectID:" + projectID + ", 查询标签项目详情出错" + ex.Message);
                throw ex;
            }

        }
        [HttpPost]
        [ApiLog]
        public Common.JsonResult ExmainLableInfo([FromBody]ReqLableTaskDTO ReqDTO)
        {
            string strJson = Json.JsonSerializerBySingleData(ReqDTO);
            BLL.Loger.Log4Net.Info("[LabelTaskController]******ExmainLableInfo begin...->ReqLableTaskDTO:" + strJson + "");

            string messageStr = "";
            try
            {
                int result = BLL.LabelTask.LB_Task.Instance.ExmainLableInfo(ReqDTO, out messageStr);
                Common.JsonResult jr;
                if (result > 0)
                {
                    jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
                }
                else
                {
                    jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
                }
                BLL.Loger.Log4Net.Info("[LabelTaskController]******ExmainLableInfo end->");
                return jr;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****ExmainLableInfo ->ReqLableTaskDTO:" + strJson + "审核标签出错:" + ex.Message);
                throw ex;
            }

        }

        /// <summary>
        /// zlb 2017-08-28
        /// 查询标签信息
        /// </summary>
        /// <param name="LableType">标签类型</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult SelectLableInfo(int LableType)
        {
            try
            {
                DataTable dt = BLL.LabelTask.LB_Task.Instance.SelectLableInfo(LableType);
                return Common.Util.GetJsonDataByResult(dt, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****SelectLableInfo ->LableType:" + LableType + ", 查询标签信息出错" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-08-29
        /// 查询子Ip信息
        /// </summary>
        /// <param name="IpId">父IpId</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult SelectSonIpInfo(int IpId)
        {
            try
            {
                DataTable dt = BLL.LabelTask.LB_Task.Instance.SelectSonIpInfo(IpId);
                return Common.Util.GetJsonDataByResult(dt, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[LabelTaskController]*****SelectSonIpInfo ->IpId:" + IpId + ", 查询子Ip标签信息出错" + ex.Message);
                throw ex;
            }
        }
    }
}
