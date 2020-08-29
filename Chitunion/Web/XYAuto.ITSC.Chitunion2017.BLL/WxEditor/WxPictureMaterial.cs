using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;

namespace XYAuto.ITSC.Chitunion2017.BLL.WxEditor
{
    /// <summary>
    /// 2017-06-22 zlb
    /// 微信图片类 BLL
    /// </summary>
    public class WxPictureMaterial
    {
        public static readonly WxPictureMaterial Instance = new WxPictureMaterial();

        /// <summary>
        /// zlb 2017-06-27
        /// 查询用户下的微信图片列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="PicName">图片名称</param>
        /// <param name="WxID">0:查询本地上传 大于0：微信的 小于0：全部的</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectPictrues(int PageIndex, int PageSize, string PicName, int WxID)
        {
            if (PageSize > 50)
            {
                PageSize = 30;
            }
            if (PicName == null)
            {
                PicName = "";
            }
            int userID = Common.UserInfo.GetLoginUserID();
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            DataTable dt = Dal.WxEditor.WxPictureMaterial.Instance.SelectPictrues(PageIndex, PageSize, PicName, WxID, userID);
            dicAll.Add("TotalCount", 0);
            List<Dictionary<string, object>> listPic = new List<Dictionary<string, object>>();
            if (dt != null && dt.Rows.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    DataRow dr = dt.Rows[i];
                    dic.Add("PicID", Convert.ToInt32(dr["PicID"]));
                    dic.Add("PicName", dr["PicName"].ToString());
                    dic.Add("PicSource", dr["PicSource"].ToString());
                    dic.Add("PicUrl", dr["PicUrl"].ToString());
                    listPic.Add(dic);
                }
            }
            dicAll.Add("PicList", listPic);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 批量删除微信图片
        /// </summary>
        /// <param name="req">图片ID集合</param>
        /// <returns></returns>
        public string DeletePictruesByPicIDs(DeletePictrueReqDTO req)
        {
            if (req == null || req.PicIDs == null || req.PicIDs.Count <= 0 || req.PicIDs.Contains(0))
            {
                return "参数错误";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < req.PicIDs.Count; i++)
            {
                sb.Append(req.PicIDs[i] + ",");
            }
            string PicIDs = sb.ToString();
            PicIDs = PicIDs.Substring(0, PicIDs.Length - 1);
            int userID = Common.UserInfo.GetLoginUserID();
            int result = Dal.WxEditor.WxPictureMaterial.Instance.DeletePictruesByPicIDs(PicIDs, userID);
            if (result > 0)
            {
                return "";
            }
            else
            {
                return "删除失败";
            }
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 批量添加微信图片
        /// </summary>
        /// <param name="req">图片名称集合类</param>
        /// <returns></returns>
        public string InsertPictrues(InsertPictruesReqDTO req)
        {
            if (req == null || req.PicUrls == null || req.PicUrls.Count <= 0)
            {
                return "参数错误";
            }
            Dictionary<string, string> dicPic = new Dictionary<string, string>();
            for (int i = 0; i < req.PicUrls.Count; i++)
            {
                string picUrl = req.PicUrls[i];
                string picName = picUrl.Substring(picUrl.LastIndexOf("/") + 1);
                dicPic.Add(picUrl, picName.Substring(0, picName.LastIndexOf("$")));
            }
            int userID = Common.UserInfo.GetLoginUserID();
            int result = Dal.WxEditor.WxPictureMaterial.Instance.InsertPictrues(userID, dicPic);
            if (result > 0)
            {
                return "";
            }
            else
            {
                return "添加失败";
            }
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 修改微信图片名称
        /// </summary>
        /// <param name="req">图片信息类</param>
        /// <returns></returns>
        public string UpdatePictruesByPicID(UpdatePictrueNameReqDTO req)
        {
            if (req == null || req.PicID == 0 || req.PicName == null || req.PicName == "")
            {
                return "参数错误";
            }
            int userID = Common.UserInfo.GetLoginUserID();
            if (Dal.WxEditor.WxPictureMaterial.Instance.UpdatePictruesByPicID(req, userID) > 0)
            {
                return "";
            }
            else
            {
                return "删除失败";
            }
        }
    }
}
