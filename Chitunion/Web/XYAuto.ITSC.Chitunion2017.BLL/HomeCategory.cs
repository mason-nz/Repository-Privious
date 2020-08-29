using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Dal;
using XYAuto.ITSC.Chitunion2017.Entities;
namespace XYAuto.ITSC.Chitunion2017.BLL
{
   /// <summary>
   /// 首页媒体信息展示
   /// </summary>
    public class HomeCategory
    {
        public static readonly HomeCategory Instance = new HomeCategory();
        public static readonly int CategoryTopCount = 8;
        /// <summary>
        /// 2017-03-15 张立彬
        /// 查询不同媒体的行业统计
        /// </summary>
        /// <param name="TopCount">查询行数</param>
        /// <returns>不同媒体的行业分类数据</returns>
        public List<Entities.HomeCategory> SelectHomeCategoryStatistics(int TopCount)
        {
            List<Entities.HomeCategory> listHC1 = SelectAllHomeCategory(1, 0);
            if (listHC1.Count >= 4)
            {
                return listHC1;
            }
            else
            {
                List<Entities.HomeCategory> listHC2 = SelectCategory(TopCount);
                foreach (var item in listHC2)
                {
                    if (listHC1.Where(T => T.MediaTypeID == item.MediaTypeID).Count() <= 0)
                    {
                        listHC1.Add(item);
                    }
                }
            }
            var sortedList =
             (from a in listHC1
              orderby a.MediaTypeID
              select a).ToList();
            return sortedList;
        }

        /// <summary>
        /// 2017-03-16 张立彬
        /// 查询不同媒体信息
        /// </summary>
        /// <param name="MediaTypeID">媒体类型</param>
        /// <param name="CategoryID">行业分类ID</param>
        /// <param name="TopCount">查询行数</param>
        /// <returns>媒体信息</returns>
        public List<Dictionary<string, object>> SelectHomeMediaInfo(int MediaTypeID, int CategoryID, int TopCount)
        {
            List<Dictionary<string, object>> listDic = SelectHomeMediaPreview(MediaTypeID, CategoryID, TopCount, 1);
            if (listDic == null || listDic.Count <= 0)
            {
                DataTable dt = Dal.HomeCategory.Instance.SelectHomeMediaInfo(MediaTypeID, CategoryID, TopCount);
                return SelectMediaClass(dt, MediaTypeID, 0);
            }
            return listDic;
        }
        /// <summary>
        /// 2017-04-12 张立彬
        /// 添加首页的行业分类
        /// </summary>
        /// <param name="listHomeCategory">行业分类集合</param>
        /// <param name="TopCount">查询数量</param>
        /// <returns>为空添加成功,否：添加失败</returns>
        public string InsertHomeCategory(List<HomeCategoryModle> listHomeCategory)
        {

            if (listHomeCategory == null || listHomeCategory.Count <= 0)
            {
                return "请选择分类";
            }
            if (listHomeCategory.Count > CategoryTopCount)
            {
                return "最多选择" + CategoryTopCount + "个分类，请修改已选分类！";
            }
            foreach (var item in listHomeCategory)
            {
                EnumMediaType flag;
                if (!Enum.TryParse<EnumMediaType>(item.MediaType.ToString(), out flag))
                {
                    return "媒体类型错误";
                }
            }
            Dal.HomeCategory.Instance.ClearHomeCategory(listHomeCategory[0].MediaType);
            Dal.HomeCategory.Instance.InsertHomeCategory(listHomeCategory);
            return "";
        }
        /// <summary>
        /// 2017-04-12 张立彬
        /// 查询对应媒体下已选行业分类
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="IsPublish">是否只查询查发布的(1：只查询发布的,其他全部)</param>
        /// <returns>行业分类集合</returns>
        public DataTable SelectSelectedCategory(int MediaType, int IsPublish)
        {
            return Dal.HomeCategory.Instance.SelectSelectedCategory(MediaType, IsPublish);
        }
        /// <summary>
        /// 2017-04-13 张立彬
        /// 查询首页媒体信息预览
        /// </summary>
        /// <param name="MediaTypeID">媒体类型</param>
        /// <param name="CategoryID">行业分类ID</param>
        /// <param name="TopCount">查询行数</param>
        /// <param name="PublishState">1已发布 0未发布</param>
        /// <returns>媒体信息集合</returns>
        public List<Dictionary<string, object>> SelectHomeMediaPreview(int MediaTypeID, int CategoryID, int TopCount, int PublishState)
        {
            DataTable dt = Dal.HomeCategory.Instance.SelectHomeMediaPreview(MediaTypeID, CategoryID, TopCount, PublishState);
            return SelectMediaClass(dt, MediaTypeID, 1);
        }
        /// <summary>
        /// 根据媒体类型不同返回不同的媒体数据
        /// </summary>
        /// <param name="dt">媒体信息集合</param>
        /// <param name="MediaTypeID">媒体类型</param>
        /// <param name="IsPreview">1为预览方法</param>
        /// <returns>媒体信息集合</returns>
        private List<Dictionary<string, object>> SelectMediaClass(DataTable dt, int MediaTypeID, int IsPreview)
        {
            List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
            if (dt != null)
            {

                if (MediaTypeID == 14001)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Dictionary<string, object> dic1 = new Dictionary<string, object>();
                        dic1.Add("HeadIconURL", dt.Rows[i]["HeadIconURL"]);
                        dic1.Add("Name", dt.Rows[i]["Name"].ToString());
                        dic1.Add("IsAuth", dt.Rows[i]["IsAuth"]);
                        dic1.Add("Sign", dt.Rows[i]["Sign"].ToString());
                        dic1.Add("IsReserve", dt.Rows[i]["IsReserve"]);
                        dic1.Add("FansCount", dt.Rows[i]["FansCount"]);
                        dic1.Add("AveragePointCount", dt.Rows[i]["AveragePointCount"]);
                        dic1.Add("ReferReadCount", dt.Rows[i]["ReferReadCount"]);
                        dic1.Add("MaxinumReading", dt.Rows[i]["MaxinumReading"]);
                        dic1.Add("UpdateCount", dt.Rows[i]["UpdateCount"]);
                        dic1.Add("maxFansCount", dt.Rows[i]["maxFansCount"]);
                        dic1.Add("maxAveragePointCount", dt.Rows[i]["maxAveragePointCount"]);
                        dic1.Add("maxReferReadCount", dt.Rows[i]["maxReferReadCount"]);
                        dic1.Add("maxMaxinumReading", dt.Rows[i]["maxMaxinumReading"]);
                        dic1.Add("maxUpdateCount", dt.Rows[i]["maxUpdateCount"]);

                        listDic.Add(dic1);
                    }
                }
                else if (MediaTypeID == 14002)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Dictionary<string, object> dic1 = new Dictionary<string, object>();
                        dic1.Add("HeadIconURL", dt.Rows[i]["HeadIconURL"]);
                        dic1.Add("Name", dt.Rows[i]["Name"]);
                        dic1.Add("AdPosition", dt.Rows[i]["AdPosition"]);
                        dic1.Add("AdForm", dt.Rows[i]["AdForm"]);
                        //dic1.Add("Style", dt.Rows[i]["Style"] == DBNull.Value ? "" : dt.Rows[i]["Style"]);
                        //dic1.Add("DailyExposureCount", dt.Rows[i]["DailyExposureCount"]);
                        listDic.Add(dic1);
                    }
                }
                else if (MediaTypeID == 14003)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Dictionary<string, object> dic1 = new Dictionary<string, object>();
                        dic1.Add("HeadIconURL", dt.Rows[i]["HeadIconURL"]);
                        dic1.Add("Name", dt.Rows[i]["Name"]);
                        dic1.Add("AuthType", dt.Rows[i]["AuthType"]);
                        dic1.Add("FansCount", dt.Rows[i]["FansCount"]);
                        dic1.Add("AverageCommentCount", dt.Rows[i]["AverageCommentCount"]);
                        dic1.Add("AverageForwardCount", dt.Rows[i]["AverageForwardCount"]);
                        dic1.Add("AveragePointCount", dt.Rows[i]["AveragePointCount"]);
                        listDic.Add(dic1);
                    }
                }
                else if (MediaTypeID == 14004)
                {
                    string path = "/Video/";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Dictionary<string, object> dic1 = new Dictionary<string, object>();
                        dic1.Add("HeadIconURL", dt.Rows[i]["HeadIconURL"]);
                        dic1.Add("Name", dt.Rows[i]["Name"]);
                        dic1.Add("FansCount", dt.Rows[i]["FansCount"]);
                        dic1.Add("AverageCommentCount", dt.Rows[i]["AverageCommentCount"]);
                        dic1.Add("AveragePlayCount", dt.Rows[i]["AveragePlayCount"]);
                        dic1.Add("AveragePointCount", dt.Rows[i]["AveragePointCount"]);
                        if (IsPreview == 1)
                        {
                            dic1.Add("PosterUrl", dt.Rows[i]["ImageUrl"]);
                            dic1.Add("VideoUrl", dt.Rows[i]["VideoUrl"]);
                        }
                        else
                        {
                            dic1.Add("VideoUrl", path + dt.Rows[i]["MediaID"] + ".MP4");
                            dic1.Add("PosterUrl", path + dt.Rows[i]["MediaID"] + "_poster.jpg");
                        }
                        listDic.Add(dic1);

                    }
                }
                else if (MediaTypeID == 14005)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Dictionary<string, object> dic1 = new Dictionary<string, object>();
                        dic1.Add("HeadIconURL", dt.Rows[i]["HeadIconURL"]);
                        dic1.Add("Name", dt.Rows[i]["Name"]);
                        dic1.Add("FansCount", dt.Rows[i]["FansCount"]);
                        dic1.Add("Number", dt.Rows[i]["Number"]);
                        dic1.Add("CumulateIncome", dt.Rows[i]["CumulateIncome"]);
                        dic1.Add("Sex", dt.Rows[i]["Sex"]);
                        dic1.Add("maxFansCount", dt.Rows[i]["maxFansCount"]);
                        dic1.Add("maxCumulateIncome", dt.Rows[i]["maxCumulateIncome"]);
                        listDic.Add(dic1);
                    }
                }
            }
            return listDic;
        }

        /// <summary>
        ///  查询行业分类
        /// </summary>
        /// <param name="PublishState"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public List<Entities.HomeCategory> SelectAllHomeCategory(int PublishState, int mediaType)
        {
            List<Entities.HomeCategory> list = new List<Entities.HomeCategory>();

            DataTable dt = Dal.HomeCategory.Instance.SelectAllHomeCategory(PublishState);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Entities.HomeCategory HC = list.Where(t => t.MediaTypeID == Convert.ToInt32(dt.Rows[i]["MediaType"])).FirstOrDefault();
                    if (HC == null)
                    {
                        Entities.HomeCategory hc1 = new Entities.HomeCategory();

                        Category cg1 = new Category();
                        cg1.CategoryID = Convert.ToInt32(dt.Rows[i]["CategoryID"]);
                        cg1.CategoryName = dt.Rows[i]["CategoryName"] == DBNull.Value ? "" : dt.Rows[i]["CategoryName"].ToString();
                        cg1.CategoryCount = Convert.ToInt64(dt.Rows[i]["CategoryCount"]);
                        hc1.listCategory.Add(cg1);
                        hc1.MediaTypeID = Convert.ToInt32(dt.Rows[i]["MediaType"]);
                        list.Add(hc1);
                    }
                    else
                    {
                        Category cg1 = new Category();
                        cg1.CategoryID = Convert.ToInt32(dt.Rows[i]["CategoryID"]);
                        cg1.CategoryName = dt.Rows[i]["CategoryName"] == DBNull.Value ? "" : dt.Rows[i]["CategoryName"].ToString();
                        cg1.CategoryCount = Convert.ToInt64(dt.Rows[i]["CategoryCount"]);
                        HC.listCategory.Add(cg1);
                    }
                }
                if (mediaType != 0)
                {
                    List<Entities.HomeCategory> list1 = new List<Entities.HomeCategory>();
                    list1 = list.Where(t => t.MediaTypeID == mediaType).ToList();
                    return list1;
                }
            }
            return list.OrderBy(T => T.MediaTypeID).ToList();
        }
        /// <summary>
        /// 查询行业分类
        /// </summary>
        /// <param name="TopCount">查询数量</param>
        /// <returns></returns>
        private List<Entities.HomeCategory> SelectCategory(int TopCount)
        {
            DataSet ds = Dal.HomeCategory.Instance.SelectHomeCategoryStatistics(TopCount);
            List<Entities.HomeCategory> listHc = new List<Entities.HomeCategory>();

            Entities.HomeCategory hc1 = new Entities.HomeCategory();
            hc1.MediaTypeID = Convert.ToInt16("14001");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Category cg1 = new Category();
                cg1.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryID"]);
                cg1.CategoryName = ds.Tables[0].Rows[i]["CategoryName"] == DBNull.Value ? "" : ds.Tables[0].Rows[i]["CategoryName"].ToString();

                hc1.listCategory.Add(cg1);
            }
            listHc.Add(hc1);

            for (int j = 0; j < 3; j++)
            {
                Entities.HomeCategory hc = new Entities.HomeCategory();
                hc.MediaTypeID = Convert.ToInt16("1400" + (j + 3));
                for (int i = 0; i < ds.Tables[j + 1].Rows.Count; i++)
                {
                    Category cg = new Category();
                    cg.CategoryID = Convert.ToInt32(ds.Tables[j + 1].Rows[i]["CategoryID"]);
                    cg.CategoryName = ds.Tables[j + 1].Rows[i]["CategoryName"] == DBNull.Value ? "" : ds.Tables[j + 1].Rows[i]["CategoryName"].ToString();
                    hc.listCategory.Add(cg);
                }
                listHc.Add(hc);
            }
            return listHc;
        }

        //public DataTable SelectPreviewCategory(int MediaTypeID)
        //{

        //}



    }
}
