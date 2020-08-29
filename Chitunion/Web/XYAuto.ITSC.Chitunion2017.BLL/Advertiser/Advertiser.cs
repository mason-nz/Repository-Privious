using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;

namespace XYAuto.ITSC.Chitunion2017.BLL.Advertiser
{
    public class Advertiser
    {
        private string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        public static readonly Advertiser Instance = new Advertiser();

        public Dictionary<String, object> SelectAdveristerList(string UserName, string Mobile, string TrueName, int UserSource, int Status, int UserType, int ProvinceID, int CityID, string StarDate, string EndDate, int PageIndex, int PageSize)
        {

            if (PageSize > Util.PageSize)
            {
                PageSize = Util.PageSize;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<AdvertiserModle> listInfo = new List<AdvertiserModle>();
            dic.Add("TotalCount", 0);
            if (!string.IsNullOrWhiteSpace(StarDate))
            {
                StarDate = Convert.ToDateTime(StarDate).ToString("yyyy-MM-dd 00:00");
            }
            if (!string.IsNullOrWhiteSpace(EndDate))
            {
                EndDate = Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd 23:59");
            }
            DataTable dt = Dal.Advertiser.Advertiser.Instance.SelectAdveristerList(UserName == null ? "" : UserName.Trim(), Mobile == null ? "" : Mobile.Trim(), TrueName == null ? "" : TrueName.Trim(), UserSource, Status, UserType, ProvinceID, CityID, StarDate, EndDate, PageIndex, PageSize);
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AdvertiserModle advMd = new AdvertiserModle();
                    advMd.UserID = Convert.ToInt32(dt.Rows[i]["UserID"]);
                    advMd.Mobile = dt.Rows[i]["Mobile"].ToString();
                    advMd.UserName = dt.Rows[i]["UserName"].ToString();
                    advMd.TrueName = dt.Rows[i]["TrueName"].ToString();
                    advMd.Area = dt.Rows[i]["Area"].ToString();
                    advMd.UserSource = dt.Rows[i]["UserSource"].ToString();
                    advMd.RegistrationTime = dt.Rows[i]["RegistrationTime"].ToString();
                    advMd.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                    listInfo.Add(advMd);
                }
            }
            dic.Add("UserInfoList", listInfo);
            return dic;
        }
        /// <summary>
        /// zlb 2017-07-21
        /// 批量启用禁用用户
        /// </summary>
        /// <param name="req">用户ID信息</param>
        /// <returns></returns>
        public string UpdateUserStatusInfo(UpdateUserStatusResDTO req)
        {
            if (req == null || req.UserIDList == null || req.UserIDList.Count <= 0 || req.UserIDList.Contains(0) || (req.Status != 0 && req.Status != 1))
            {
                return "参数错误";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < req.UserIDList.Count; i++)
            {
                sb.Append(req.UserIDList[i] + ",");
            }
            string UserIDList = sb.ToString();
            UserIDList = UserIDList.Substring(0, UserIDList.Length - 1);
            int result = Dal.Advertiser.Advertiser.Instance.UpdateUserStatusInfo(UserIDList, req.Status);
            if (result > 0)
            {
                return "";
            }
            else
            {
                return "操作失败";
            }
        }
        /// <summary>
        /// zlb 2017-07-21
        /// 批量重置用户密码
        /// </summary>
        /// <param name="req">用户ID信息</param>
        /// <returns></returns>
        public string UpdateUserPwdInfo(UpdateUserPwdReqsDTO req)
        {
            if (req == null || req.UserIDList == null || req.UserIDList.Count <= 0 || req.UserIDList.Contains(0))
            {
                return "参数错误";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < req.UserIDList.Count; i++)
            {
                sb.Append(req.UserIDList[i] + ",");
            }
            string UserIDList = sb.ToString();
            UserIDList = UserIDList.Substring(0, UserIDList.Length - 1);
            string pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash(UserInfo.DefaultPwd + "29001" + LoginPwdKey, System.Text.Encoding.UTF8);
            int result = Dal.Advertiser.Advertiser.Instance.UpdateUserPwdInfo(UserIDList, pwd);
            if (result > 0)
            {
                return "";
            }
            else
            {
                return "操作失败";
            }
        }


    }

    public class AdvertiserModle
    {
        public int UserID { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string Area { get; set; }
        public string UserSource { get; set; }
        public string RegistrationTime { get; set; }
        public int Status { get; set; }

    }

}
