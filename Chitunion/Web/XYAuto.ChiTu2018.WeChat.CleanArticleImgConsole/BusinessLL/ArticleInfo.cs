using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FastDFS.Client;
using HtmlAgilityPack;
using XYAuto.CTUtils.Config;

namespace XYAuto.ChiTu2018.WeChat.CleanArticleImgConsole.BusinessLL
{
    /// <summary>
    /// 注释：ArticleInfo
    /// 作者：masj
    /// 日期：2018/5/28 10:37:09
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ArticleInfo
    {
        public static readonly ArticleInfo Instance = new ArticleInfo();

        //private string Conn_BaseData = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_BaseData", true);//ConfigurationManager.AppSettings["ConnectionStrings_BaseData"];
        //private int QueryArticleInfo_RecID = XYAuto.CTUtils.Sys.ConverHelper.ObjectToInteger(ConfigurationUtil.GetAppSettingValue("QueryArticleInfo_RecID", true));
        //private int QueryArticleInfo_TopNum = XYAuto.CTUtils.Sys.ConverHelper.ObjectToInteger(ConfigurationUtil.GetAppSettingValue("QueryArticleInfo_TopNum", true));

        public DataTable GetArticleDataByRecID(int? recId = null)
        {
            return DAL.ArticleInfo.Instance.GetArticleDataByRecID(recId);
        }

        public NameValueCollection GetImageList()
        {
            NameValueCollection imgList = new NameValueCollection();

            int recid = -1;
            int count = 0;

            do
            {
                DataTable dt = new DataTable();
                dt = GetArticleDataByRecID(recid);
                if (recid > 0)
                {
                    dt = GetArticleDataByRecID(recid);
                }
                else
                {
                    dt = GetArticleDataByRecID();
                }
                if (dt != null)
                {
                    count = dt.Rows.Count;
                    recid = int.Parse(dt.Rows[count - 1]["RecID"].ToString());
                    foreach (DataRow dr in dt.Rows)
                    {
                        CleanArticleImg(dr, ref imgList);
                    }
                }


            } while (count > 0);
            return imgList;
        }

        private void CleanArticleImg(DataRow dr, ref NameValueCollection imgList)
        {
            long recID = long.Parse(dr["RecID"].ToString());
            string headImg = dr["HeadImg"] == DBNull.Value ? "" : dr["HeadImg"].ToString();
            string headImgNew = dr["HeadImgNew"] == DBNull.Value ? "" : dr["HeadImgNew"].ToString();
            string headImgNew2 = dr["HeadImgNew2"] == DBNull.Value ? "" : dr["HeadImgNew2"].ToString();
            string headImgNew3 = dr["HeadImgNew3"] == DBNull.Value ? "" : dr["HeadImgNew3"].ToString();
            string content = dr["Content"] == DBNull.Value ? "" : dr["Content"].ToString();
            string createTime = dr["CreateTime"].ToString();
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"当前数据，recID={recID},createTime={createTime}");
            if (IsUrl(headImg))
            {
                //imgList.Add(recID.ToString(), headImg);
                DeleteImg(headImg);
            }
            if (IsUrl(headImgNew))
            {
                //imgList.Add(recID.ToString(), headImg);
                DeleteImg(headImgNew);
            }
            if (IsUrl(headImgNew2))
            {
                //imgList.Add(recID.ToString(), headImg);
                DeleteImg(headImgNew2);
            }
            if (IsUrl(headImgNew3))
            {
                //imgList.Add(recID.ToString(), headImg);
                DeleteImg(headImgNew3);
            }
            if (!string.IsNullOrEmpty(content))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//img[contains(@src,'imgcdn.chitunion.com')]|//img[contains(@src,'img4.qcdqcdn.com')]");
                if (list != null && list.Any())
                {
                    foreach (HtmlNode item in list)
                    {
                        string imgUrl = item.Attributes["src"].Value;
                        DeleteImg(imgUrl);
                    }
                }

            }

        }

        private bool DeleteImg(string imgUrl)
        {
            bool flag = false;
            try
            {
                string info = $"获取图片URL={imgUrl}";
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(info);

                //删除图片逻辑
                flag = XYAuto.CTUtils.Image.FastDFSHelper.Delete(imgUrl);
                info += $",删除结果={flag}";
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(info);
                return flag;
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("删除图片异常", ex);
            }
            return flag;
        }


        private bool IsUrl(string url)
        {
            try
            {
                Uri yUri = new Uri(url);
                if (yUri.Host == "imgcdn.chitunion.com" || yUri.Host == "img4.qcdqcdn.com")
                {
                    //XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("获取图片URL=" + url);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
