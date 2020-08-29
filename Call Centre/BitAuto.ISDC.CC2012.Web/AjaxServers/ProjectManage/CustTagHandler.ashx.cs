using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// CustTagHandler 的摘要说明
    /// </summary>
    public class CustTagHandler : IHttpHandler, IRequiresSessionState
    {
        public string Action
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Action"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Action"]);
            }
        }
        public string TagIds
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TagIds"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["TagIds"]);
            }
        }

        public string CustId
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CustId"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CustId"]);
            }
        }
        public string UserId
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["UserId"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["UserId"]);
            }
        }

        #region
        public string RequestCustName
        {
            get { return HttpContext.Current.Request["CustName"] == null ? string.Empty : HttpContext.Current.Request["CustName"].Trim(); }
        }
        public string RequestBrand
        {
            get { return HttpContext.Current.Request["Brand"] == null ? string.Empty : HttpContext.Current.Request["Brand"].Trim(); }
        }
        public string RequestSearchTrueNameID
        {
            get { return HttpContext.Current.Request["SearchTrueNameID"] == null ? string.Empty : HttpContext.Current.Request["SearchTrueNameID"]; }
        }

        public string RequestProvinceID
        {
            get { return HttpContext.Current.Request["Province"] == null ? "-1" : HttpContext.Current.Request["Province"]; }
        }
        public string RequestCityID
        {
            get { return HttpContext.Current.Request["City"] == null ? "-1" : HttpContext.Current.Request["City"]; }
        }
        public string RequestCountyID
        {
            get { return HttpContext.Current.Request["County"] == null ? "-1" : HttpContext.Current.Request["County"]; }
        }
        public int RequestNoResponser
        {
            get { return HttpContext.Current.Request["NoResponser"] == "-2" ? -2 : int.Parse(HttpContext.Current.Request["NoResponser"].ToString()); }
        }
        public string ClientType
        {
            get { return HttpContext.Current.Request["ClientType"] == null ? string.Empty : HttpContext.Current.Request["ClientType"].ToString(); }
        }
        public string CarType
        {
            get { return HttpContext.Current.Request["CarType"] == null ? string.Empty : HttpContext.Current.Request["CarType"].ToString(); }
        }

        public string StartTime
        {
            get { return HttpContext.Current.Request["StartTime"] == null ? string.Empty : HttpContext.Current.Request["StartTime"].ToString(); }
        }
        public string EndTime
        {
            get { return HttpContext.Current.Request["EndTime"] == null ? string.Empty : HttpContext.Current.Request["EndTime"].ToString(); }
        }
        public int Contact
        {
            get { return HttpContext.Current.Request["Contact"] == "-2" ? -2 : int.Parse(HttpContext.Current.Request["Contact"].ToString()); }
        }
        /// <summary>
        /// 集采项目名
        /// </summary>
        public string RequestProjectName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("ProjectName"); }
        }
        /// <summary>
        /// CC项目名称
        /// </summary>
        public string ReqeustCCProjectName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CCProjectName"); }
        }

        public string radioTaoche
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("radioTaoche"); }
        }
        #endregion
        public string TagID
        {
            get { return HttpContext.Current.Request["TagID"] == null ? "0" : HttpContext.Current.Request["TagID"].ToString(); }
        }
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";

            switch (Action)
            {
                case "saveCustTagChange":
                    SaveCustTagChange(out msg);
                    break;
                case "getTagStatisticData":
                    GetTagStatisticData(out msg);
                    break;
                default:
                    break;
            }
            context.Response.Write(msg);
        }

        private void GetTagStatisticData(out string msg)
        {
            msg = "";

            int totcount;
            int userID = BLL.Util.GetLoginUserID();

            Entities.QueryCC_CustUserMapping query = new Entities.QueryCC_CustUserMapping();
            if (!string.IsNullOrEmpty(RequestCustName))
            {
                query.CustName = RequestCustName.Trim();
            }
            if (!string.IsNullOrEmpty(RequestBrand))
            {
                query.Brandids = RequestBrand.Trim();
            }
            if (!string.IsNullOrEmpty(RequestSearchTrueNameID))
            {
                query.UserName = RequestSearchTrueNameID.Trim();
            }
            //坐席查询条件：是否有坐席
            query.NoResponser = RequestNoResponser;
            if (!string.IsNullOrEmpty(RequestProvinceID) && int.Parse(RequestProvinceID) > 0)
            {
                query.ProvinceID = RequestProvinceID.Trim();
            }
            if (!string.IsNullOrEmpty(RequestCityID) && int.Parse(RequestCityID) > 0)
            {
                query.CityID = RequestCityID.Trim();
            }
            if (!string.IsNullOrEmpty(RequestCountyID) && int.Parse(RequestCountyID) > 0)
            {
                query.CountyID = RequestCountyID.Trim();
            }
            //add by qizq 2012-5-30 客户类型经和营范围
            if (!string.IsNullOrEmpty(ClientType))
            {
                query.TypeID = ClientType;
            }
            if (!string.IsNullOrEmpty(CarType))
            {
                query.CarType = CarType;
            }
            //add lxw 12.6.8 最近访问时间 
            if (!string.IsNullOrEmpty(StartTime))
            {
                query.StartTime = DateTime.Parse(StartTime);
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                query.EndTime = DateTime.Parse(EndTime);
            }
            if (Contact != -2)
            {
                query.Contact = Contact;
            }
            if (!string.IsNullOrEmpty(RequestProjectName))
            {
                query.ProjectName = RequestProjectName.Trim();
            }
            if (!string.IsNullOrEmpty(radioTaoche))
            {
                query.radioTaoche = radioTaoche;
            }
            //取当前人所对应的数据权限组
            Entities.QueryUserGroupDataRigth QueryUserGroupDataRigth = new Entities.QueryUserGroupDataRigth();
            QueryUserGroupDataRigth.UserID = userID;
            DataTable dtUserGroupDataRigth = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigth(QueryUserGroupDataRigth, "", 1, 100000, out totcount);
            string Rolename = string.Empty;
            query.UserID = userID;//进列表肯定有查看本人负责的客户信息
            if (dtUserGroupDataRigth != null && dtUserGroupDataRigth.Rows.Count > 0)
            {
                for (int i = 0; i < dtUserGroupDataRigth.Rows.Count; i++)
                {
                    //4s电话营销，非4s电话营销
                    if (dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "6" || dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "7" || dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "19" || dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "27")
                    {
                        //本组
                        query.BGIDStr += dtUserGroupDataRigth.Rows[i]["bgid"].ToString() + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(ReqeustCCProjectName))
            {
                query.ReqeustCCProjectName = ReqeustCCProjectName;
            }
            DataTable dtUserTags = BitAuto.YanFa.Crm2009.BLL.CustTag.Instance.GetCustTagByUserID(userID);
            string TagIDs = "";
            foreach (DataRow row in dtUserTags.Rows)
            {
                TagIDs +=  "," + row["TagID"];
            }
            if (TagIDs.Length > 0)
            {
                TagIDs = TagIDs.Substring(1);
            }
            else
            {
                TagIDs = "0";
            }
            query.TagID = TagIDs;
            DataTable dt = BLL.CC_UserCustDataRigth.Instance.GetCustUserMappingTagStatisticsByUserID(query, userID);

            
            DataColumn newcol = new DataColumn("ThisTagNum", typeof(string));
            dtUserTags.Columns.Add(newcol);
            //拼接起来
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataRow row2 in dtUserTags.Rows)
                    {
                        if (row["TagName"].ToString() == row2["TagName"].ToString())
                        {
                            row2["ThisTagNum"] = row["TheTagNum"];

                        }
                    }
                }
            }
            if (dtUserTags == null || dtUserTags.Rows.Count < 1)
            {
                msg = "{ }";
            }
            else
            {
                int i = 0;
                foreach (DataRow row in dtUserTags.Rows)
                {
                    if (row["ThisTagNum"] == null || string.IsNullOrEmpty(row["ThisTagNum"].ToString()))
                    {
                        msg += "'" +i+ "':['" + row["TagID"].ToString() + "','0','" + row["TagName"].ToString() + "'],";
                        i++;
                    }
                    else
                    {
                        msg += "'" + i + "':['" + row["TagID"].ToString() + "','" + row["ThisTagNum"] + "','" + row["TagName"].ToString() + "'],";
                        i++;
                       // msg += "'" + row["TagName"].ToString() + "':['" + row["TagID"].ToString() + "','" + row["ThisTagNum"] + "'],";
                    }
                }
                if (msg.Length < 1)
                {
                    msg = "{ }";
                }
                else
                {
                    msg = "{ " + msg.Substring(0, msg.Length - 1) + "}";
                }
            }
        }
        
        private void SaveCustTagChange(out string msg)
        {
            msg = "";
            if (CustId == "" || UserId == "")
            {
                msg = "参数传递出现异常！";
            }
            else
            {
                string[] straTagID = TagIds.Split(',');

                StringBuilder strXmlObject = new StringBuilder();
                strXmlObject.Append("<ItemRoot>");
                for (int i = 0; i < straTagID.Length; i++)
                {
                    strXmlObject.Append("<Item>");
                    strXmlObject.Append("<TagID>" + straTagID[i] + "</TagID>");
                    strXmlObject.Append("<CustID>" + CustId + "</CustID>");
                    strXmlObject.Append("</Item>");
                }
                strXmlObject.Append("</ItemRoot>");

               int outvalue = BitAuto.YanFa.Crm2009.BLL.CustTag.Instance.SetCustTagMapping(strXmlObject.ToString(), CustId, int.Parse(UserId));
               if (outvalue == -1)
               {
                   msg = "数据保存失败！";
               }
               else
               {
                   msg = "数据保存成功！";
               }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}