using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    public class HomeCategoryController : ApiController
    {

        /// <summary>
        /// 2017-03-15 张立彬
        /// 查询不同媒体的行业统计
        /// </summary>
        [HttpGet]
        public Common.JsonResult SelectHomeCategoryStatistics(int TopCount = 5)
        {
            List<Entities.HomeCategory> listHc = new List<Entities.HomeCategory>();
            try
            {
                listHc = BLL.HomeCategory.Instance.SelectHomeCategoryStatistics(TopCount);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[HomeCategoryController]*****SelectHomeCategoryStatistics ->首页查询媒体下的行业统计出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(listHc, "Success");
        }
        /// <summary>
        /// 2017-03-16 张立彬
        /// 查询不同媒体信息
        /// </summary>
        /// <param name="MediaTypeID">媒体类型</param>
        /// <param name="CategoryID">行业分类ID</param>
        /// <param name="TopCount">查询行数</param>
        /// <param name="RandNum">随机显示行数</param>
        [HttpGet]
        public Common.JsonResult SelectHomeMediaInfo(int MediaTypeID, int CategoryID, int TopCount = 8)
        {
            List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
            try
            {
                listDic = BLL.HomeCategory.Instance.SelectHomeMediaInfo(MediaTypeID, CategoryID, TopCount);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[HomeCategoryController]*****SelectHomeMediaInfo-> MediaTypeID:" + MediaTypeID + "->CategoryID:" + CategoryID + "->首页查询媒体信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(listDic, "Success");
        }
        /// <summary>
        /// 2017-04-12 张立彬
        /// 添加首页的行业分类
        /// </summary>
        /// <param name="listHomeCategory">行业分类集合</param>
        /// <param name="TopCount">查询数量</param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult InsertHomeCategory(List<HomeCategoryModle> listHomeCategory)
        {
            string strJson = Json.JsonSerializerBySingleData(listHomeCategory);
            BLL.Loger.Log4Net.Info("[HomeCategoryController]******InsertHomeCategory begin...->listHomeCategory:" + strJson + "");
            string messageStr = "";
            Common.JsonResult jr;
            try
            {
                messageStr = BLL.HomeCategory.Instance.InsertHomeCategory(listHomeCategory);
                if (messageStr != "")
                {
                    jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
                }
                else
                {
                    jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[HomeCategoryController]*****InsertHomeCategory ->listHomeCategory:" + strJson + ",添加首页分类数据失败:" + ex.Message);
                throw ex;
            }
            BLL.Loger.Log4Net.Info("[HomeCategoryController]******InsertHomeCategory end->");
            return jr;
        }

        //[HttpGet]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        //public Common.JsonResult SelectHomeCategoryInfo(int MediaType)
        //{
        //    DataTable dt = BLL.HomeCategory.Instance.SelectHomeCategoryInfo(MediaType, 0);
        //    return Common.Util.GetJsonDataByResult(dt, "Success");
        //}

        /// <summary>
        /// 2017-04-13 张立彬
        /// 查询首页媒体预览
        /// </summary>
        /// <param name="MediaTypeID">媒体类型</param>
        /// <param name="CategoryID">行业分类ID</param>
        /// <param name="TopCount">查询行数</param>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectHomeMediaPreview(int MediaTypeID, int CategoryID, int TopCount = 8)
        {
            List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
            try
            {
                listDic = BLL.HomeCategory.Instance.SelectHomeMediaPreview(MediaTypeID, CategoryID, TopCount, 0);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[HomeCategoryController]*****SelectHomeMediaPreview-> MediaTypeID:" + MediaTypeID + "->CategoryID:" + CategoryID + "->首页查询媒体信息预览出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(listDic, "Success");
        }
        /// <summary>
        /// 2017-04-13 张立彬
        /// 查询预览页行业统计
        /// </summary>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectAllHomeCategory(int mediaType)
        {
            List<Entities.HomeCategory> listHc = new List<Entities.HomeCategory>();
            try
            {
                listHc = BLL.HomeCategory.Instance.SelectAllHomeCategory(0, mediaType);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Info("[HomeCategoryController]*****SelectAllHomeCategory ->查询预览页行业统计出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(listHc, "Success");
        }

    }
}
