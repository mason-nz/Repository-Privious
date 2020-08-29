using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    /// <summary>
    /// 2017-02-21 张立彬
    /// 刊例Api
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class PeriodicationController : ApiController
    {
        #region V1.0
        /// <summary>
        /// 2017-02-22 张立彬
        /// 根据刊例ID和媒体类型查询
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="pubID">刊例ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetPublishInfoBymediaTypeAndPubID(int mediaType, int pubID)
        {
            Entities.Periodication PerModel = new Entities.Periodication(); ;
            try
            {
                bool resultVerification = AuthorizationCommon.PublishSelectVerification(mediaType);
                if (!resultVerification)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }

                PerModel = BLL.Periodication.Instance.GetPublishInfoBymediaTypeAndPubID(mediaType, pubID);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[PeriodicationController]*****GetPublishInfoBymediaTypeAndPubID ->pubID:" + pubID + " ->mediaType:" + mediaType + ",查询刊例详情信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(PerModel, "Success");
        }
        /// <summary>
        /// 2017-02-24 张立彬
        /// 根据广告位ID查询APP媒体广告位信息
        /// </summary>
        /// <param name="pubID">广告位ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetAppPublishInfoByAdvID(int ADDetailID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                bool resultVerification = AuthorizationCommon.PublishSelectVerification((int)MediaType.APP);
                if (!resultVerification)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }
                dic = BLL.Periodication.Instance.GetAppPublishInfoByAdvID(ADDetailID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PeriodicationController]*****GetAppPublishInfoByAdvID ->ADDetailID:" + ADDetailID + ",查询APP媒体广告位信息出错:" + ex.Message);
                //Utils.Loger.Log4Net.Info(string.Format("调用webapi 方法GetAttendPendingCount接口，参数employeeNumber：{0},取数据出现异常，异常原因{1}！", employeeNumber, ex.Message.ToString()));
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        /// <summary>
        ///2017-03-01 张立彬
        /// 根据媒体ID或刊例ID查询刊例基本信息详情
        /// </summary>
        /// <param name="pubIDOrMediaID"></param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetPublishBasicInfoByID(int pubIDOrMediaID, int mediaType = 0)
        {
            DateTime dtStart = DateTime.Now;
            List<PeriodicationDetaill> listPer = new List<PeriodicationDetaill>();
            try
            {
                listPer = BLL.Periodication.Instance.GetPublishBasicInfoByID(pubIDOrMediaID, mediaType);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PeriodicationController]*****GetPublishBasicInfoByID ->pubIDOrMediaID:" + pubIDOrMediaID + " ->mediaType:" + mediaType + ",查询刊例基本信息详情出错:" + ex.Message);
                //Utils.Loger.Log4Net.Info(string.Format("调用webapi 方法GetAttendPendingCount接口，参数employeeNumber：{0},取数据出现异常，异常原因{1}！", employeeNumber, ex.Message.ToString()));
                throw ex;
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan span = (TimeSpan)(dtEnd - dtStart);
            Loger.Log4Net.Info("GetPublishBasicInfoByID—>用时：" + span.TotalMilliseconds + "毫秒");
            return Common.Util.GetJsonDataByResult(listPer, "Success");
        }
        /// <summary>
        /// 2017-03-02 张立彬
        ///根据刊例ID和其他条件查询APP刊例下广告位的信息列表
        /// </summary>
        /// <param name="pubID">刊例ID</param>
        /// <param name="publishStatus">发布状态</param>
        /// <param name="adPosition">广告位置</param>
        /// <param name="adForm">广告形式</param>
        /// <param name="style">广告样式</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult SelectAppAdvListByPubID(int pubID, int pagesize = 20, int PageIndex = 1, int publishStatus = 0, string adPosition = "", string adForm = "", string style = "")
        {
            BLL.ListTotal listAdv = new ListTotal();
            try
            {
                bool resultVerification = AuthorizationCommon.PublishSelectVerification((int)MediaType.APP);
                if (!resultVerification)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }
                listAdv = BLL.Periodication.Instance.SelectAppAdvListByPubID(pubID, publishStatus, adPosition, adForm, style, pagesize, PageIndex);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PeriodicationController]*****SelectAppAdvListByPubID ->pubID:" + pubID + ",查询APP刊例下广告位的信息列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(listAdv, "Success");
        }
        /// 2017-03-04 张立彬
        ///根据广告位ID查询APP刊例和广告位信息
        /// </summary>
        /// <param name="ADDetailID">广告位ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetAppPublishAdvInfoByAdvID(int ADDetailID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                bool resultVerification = AuthorizationCommon.PublishSelectVerification((int)MediaType.APP);
                if (!resultVerification)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }
                dic = BLL.Periodication.Instance.GetAppPublishAdvInfoByAdvID(ADDetailID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PeriodicationController]*****GetAppPublishAdvInfoByAdvID ->ADDetailID:" + ADDetailID + ",查询APP媒体广告位信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        /// <summary>
        /// 2017-03-07 张立彬
        /// 根据媒体ID查询APP媒体刊例信息
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetAppMediaByMediaID(int MediaID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                bool resultVerification = AuthorizationCommon.PublishSelectVerification((int)MediaType.APP);
                if (!resultVerification)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }
                dic = BLL.Periodication.Instance.GetAppMediaByMediaID(MediaID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PeriodicationController]*****GetAppMediaByMediaID ->MediaID:" + MediaID + ",根据媒体ID查询APP媒体刊例信息信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        #endregion

        #region V1.1
        /// <summary>
        /// 根据刊例ID和媒体类型查询刊例信息 2017-04-21 张立彬
        /// </summary>
        /// <param name="PubID">刊例ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns></returns>

        [HttpGet]
        public Common.JsonResult SelectWXPublishByIDAndType(int PubID, int MediaType)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                bool resultVerification = AuthorizationCommon.PublishSelectVerification(MediaType);
                if (!resultVerification)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }

                dic = BLL.Periodication.Instance.SelectWXPublishByIDAndType(PubID, MediaType);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[PeriodicationController]*****SelectWXPublishByIDAndType ->pubID:" + PubID + " ->mediaType:" + MediaType + ",查询微信刊例详情信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        /// <summary>
        /// 根据媒体ID和刊例执行周期查询微信有效刊例信息 2017-04-22 张立彬
        /// </summary>
        /// <param name="MediaID"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult SelectShoppingPublish(int MediaType, int MediaID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                //bool resultVerification = AuthorizationCommon.PublishSelectVerification(MediaType);
                //if (!resultVerification)
                //{
                //    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                //}

                dic = BLL.Periodication.Instance.SelectShoppingPublish(MediaType, MediaID);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[PeriodicationController]*****SelectShoppingPublish ->MediaID:" + MediaID + " ->mediaType:" + MediaType + ",查询购物车刊例信息失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        #endregion

        #region V1.1.1
        /// <summary>
        ///查询广告下的政策列表及对应的广告位信息集合 张立彬 2017-05-15
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <returns>政策列表集合</returns>
        [HttpGet]
        public Common.JsonResult SelectPublishesByMediaID(int MediaType, int MediaID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                //bool resultVerification = AuthorizationCommon.PublishSelectVerification(MediaType);
                //if (!resultVerification)
                //{
                //    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                //}

                dic = BLL.Periodication.Instance.SelectPublishesByMediaID(MediaType, MediaID);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[PeriodicationController]*****SelectPublishesByMediaID ->MediaID:" + MediaID + " ->mediaType:" + MediaType + ",查询广告下的政策列表失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        #region V1.1.4
        /// <summary>
        /// 2017-06-06 zlb
        ///查询App模板对应的广告信息，媒体信息,刊例信息
        /// </summary>
        /// <param name="TemplateID">模板ID</param>
        /// <param name="MediaID">订单ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult SelectShoppingAppPublish(int TemplateID, int MediaID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.Periodication.Instance.SelectShoppingAppPublish(TemplateID, MediaID);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[PeriodicationController]*****SelectShoppingAppPublish ->MediaID:" + MediaID + " ->TemplateID:" + TemplateID + ",查询APP购物车详情信息:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        #endregion

        #endregion
    }
}
