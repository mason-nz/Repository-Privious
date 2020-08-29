using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// Summary description for TagEdit
    /// </summary>
    public class TagEdit : IHttpHandler, IRequiresSessionState
    {
        #region 定义属性
        public int BGID
        {
            get { return currentContext.Request["BGID"] == null ? -1 : Convert.ToInt32(currentContext.Request["BGID"]); }
        }
        public int TagID
        {
            get { return currentContext.Request["TagID"] == null ? -1 : Convert.ToInt32(currentContext.Request["TagID"]); }
        }
        public int PID
        {
            get { return currentContext.Request["pid"] == null ? -1 : Convert.ToInt32(currentContext.Request["pid"]); }
        }
        public string tagsJSON
        {
            get { return currentContext.Request["tagsJSON"] == null ? string.Empty : Convert.ToString(currentContext.Request["tagsJSON"]); }
        }
        public string status
        {
            get { return currentContext.Request["status"] == null ? string.Empty : Convert.ToString(currentContext.Request["status"]); }
        }

        public string tagName
        {
            get { return currentContext.Request["tagName"] == null ? string.Empty : HttpUtility.UrlDecode((currentContext.Request["tagName"])); }
        }


        public string Q
        {
            get { return currentContext.Request["q"] == null ? string.Empty : HttpUtility.UrlDecode((currentContext.Request["q"])); }
        }

        public int userID
        {
            get { return BLL.Util.GetLoginUserID(); }
        }

        public string action
        {
            get
            {
                return currentContext.Request["action"] == null ? string.Empty : Convert.ToString(currentContext.Request["action"]);
            }
        }

        public string isUp
        {
            get
            {
                return currentContext.Request["isUp"] == null ? string.Empty : Convert.ToString(currentContext.Request["isUp"]);
            }
        }

        #endregion
        bool success = true;
        string result = "";
        string message = "";
        public HttpContext currentContext;


        private string UpdateWorkTag(int nStatus)
        {
            string strResult = "ok";
            try
            {
                if (TagID > 0)
                {
                    var t = BLL.WorkOrderTag.Instance.GetWorkOrderTag(TagID);
                    t.Status = nStatus;
                    BLL.WorkOrderTag.Instance.Update(t);
                }
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }

        private string InsertNewTag()
        {
            try
            {
                Entities.WorkOrderTag tnew = new WorkOrderTag()
                             {
                                 BGID = BGID,
                                 CreateTime = DateTime.Now,
                                 CreateUserID = userID,
                                 TagName = tagName,
                                 PID = PID,
                                 Status = 0,
                                 ModifyTime = DateTime.Now,
                                 ModifyUserID = userID
                             };

                return BLL.WorkOrderTag.Instance.Insert(tnew).ToString();
            }
            catch (Exception ex)
            {
                //return ex.Message;
                if (ex.Message.IndexOf("\n") > 0)
                {
                    return ex.Message.Substring(0, ex.Message.IndexOf("\n") - 1);
                }
                else
                {
                    return ex.Message;
                }

            }


        }

        private string GetSimulerWorkOrder()
        {
            var strSearch = string.Empty;
            if (Q.IndexOf("--", System.StringComparison.OrdinalIgnoreCase) > 0)
            {
                strSearch = Q.Substring(Q.IndexOf("--", System.StringComparison.OrdinalIgnoreCase) + 2);
            }
            else
            {
                strSearch = Q;
            }
            var dt = BLL.WorkOrderTag.Instance.GetSimulerWorkOrder(userID, strSearch);
            StringBuilder sb = new StringBuilder();
            //sb.Append("name1|val1\nname2|val2\nname3|va3\n");
            try
            {
                // return Newtonsoft.Json.JavaScriptConvert.SerializeObject(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //sb.Append(string.Format("\"text\":\"{0}\",\"val\":\"{1}\"", dr["tagname"], dr["id"]));
                        sb.Append(string.Format("{0}|{1}\n", dr["tagname"], dr["id"]));
                    }
                }

            }
            catch (Exception ex)
            {
                sb = new StringBuilder();
                //sb.Append("[]");
            }
            return sb.ToString();
        }

        private string DeleTag()
        {
            return UpdateWorkTag(-1);
        }

        private string ChangeValidate()
        {

            if (string.IsNullOrEmpty(status))
            {
                return "参数错误";
            }
            int nV = 0;
            if (int.TryParse(status, out nV))
            {
                return UpdateWorkTag(nV);
            }
            return "参数错误";
        }

        private string ChangeName()
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return "名称不能为空";
            }
            string strResult = "ok";
            try
            {
                if (TagID > 0)
                {
                    var t = BLL.WorkOrderTag.Instance.GetWorkOrderTag(TagID);
                    t.TagName = tagName;
                    BLL.WorkOrderTag.Instance.Update(t);
                }
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }

        private string ChangeOrder()
        {
            if (string.IsNullOrEmpty(isUp) || string.IsNullOrEmpty(status)) return "参数错误";
            try
            {
                BLL.WorkOrderTag.Instance.ChangeOrder(TagID, isUp == "1" ? true : false, status);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "ok";
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            currentContext = context;

            if (!string.IsNullOrEmpty(action))
            {
                string strResult = string.Empty;
                switch (action)
                {
                    case "ChangeName":
                        strResult = ChangeName();
                        break;
                    case "ChangeOrder":
                        strResult = ChangeOrder();
                        break;
                    case "ChangeValidate":
                        strResult = ChangeValidate();
                        break;
                    case "DeleTag":
                        strResult = DeleTag();
                        break;
                    case "InsertNewTag":
                        strResult = InsertNewTag();
                        break;
                    case "GetSimulerWorkOrder":
                        strResult = GetSimulerWorkOrder();
                        break;
                }

                context.Response.Write(strResult);
                context.Response.End();
                return;
            }

            if ((context.Request["TagsSelectAll"] + "").Trim() == "yes")
            {
                //取当前选择的业务组
                if (BGID != -1 && userID != -1)
                {
                    //DataTable dt = BLL.BusinessGroup.Instance.GetBusinessGroupTagsByBGID(userID,BGID);
                    //标签管理页面已做数据权限制，这里直接取业务组下标签即可
                    DataTable dt = BLL.BusinessGroup.Instance.GetBusinessGroupTagsByBGIDOnly(BGID);
                    if (dt.Rows.Count > 0)
                    {
                        //在已选标签中遍历
                        //判断标签是否在已选择标签里，是则isSelected=true
                        StringBuilder sb = new StringBuilder();

                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append("{'BGID':'" + row["BGID"].ToString() + "','GroupName':'" + row["GroupName"].ToString() + "','TagID':'" + row["TagID"].ToString() +
                                        "','TagName':'" + row["TagName"].ToString() + "','IsUsed':'" + row["IsUsed"].ToString() + "','order':'" + row["OrderNum"].ToString() + "'},");
                        }

                        message = sb.ToString();
                        if (message.EndsWith(","))
                            message = message.Substring(0, message.Length - 1);
                    }


                }


                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if ((context.Request["TagsIsUsedSelect"] + "").Trim() == "yes")
            {
                if (TagID != -1)
                {
                    bool isUsed = false;
                    isUsed = BLL.WorkOrderTag.Instance.isUsedTagByTagID(TagID);

                    if (isUsed)
                    {
                        message = "isUsed";
                    }
                    else
                    {
                        message = "notUsed";
                    }

                }


                context.Response.Write(message);
                context.Response.End();
            }
            else if ((context.Request["InsertTag2DB"] + "").Trim() == "yes")
            {
                int retval = 0;
                if (BGID != -1)
                {
                    List<JsonTag> Tags = new List<JsonTag>();
                    Tags = (List<JsonTag>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(tagsJSON, typeof(List<JsonTag>));

                    Entities.WorkOrderTag model = new Entities.WorkOrderTag();

                    foreach (JsonTag tag in Tags)
                    {
                        model.BGID = BGID;
                        model.CreateTime = DateTime.Now;
                        model.CreateUserID = userID;
                        model.ModifyUserID = userID;
                        model.ModifyTime = DateTime.Now;
                        model.Status = Convert.ToInt32(tag.Status);
                        model.TagName = HttpContext.Current.Server.UrlDecode(tag.TagName);
                        model.OrderNum = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(tag.OrderNum.ToString()));
                        retval = BLL.WorkOrderTag.Instance.Insert(model);
                    }
                }

                if (retval > 0)
                {
                    message = "操作成功";
                }
                else
                {
                    message = "操作失败";
                }
                context.Response.Write(message);
                context.Response.End();
            }
            else if ((context.Request["DeleteTag2DB"] + "").Trim() == "yes")
            {
                int retval = 0;

                List<JsonTag> Tags = new List<JsonTag>();
                Tags = (List<JsonTag>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(tagsJSON, typeof(List<JsonTag>));

                Entities.WorkOrderTag model = new Entities.WorkOrderTag();

                string tagids = "";
                foreach (JsonTag tag in Tags)
                {
                    tagids += tag.TagID + ",";
                }

                if (tagids.EndsWith(","))
                    tagids = tagids.Substring(0, tagids.Length - 1);

                tagids = "(" + tagids + ")";
                retval = BLL.WorkOrderTag.Instance.DeleteMany(tagids);
                if (retval > 0)
                {
                    message = "操作成功";
                }
                else
                {
                    message = "操作失败";
                }
                context.Response.Write(message);
                context.Response.End();
            }
            else if ((context.Request["UpdateTag2DB"] + "").Trim() == "yes")
            {
                int retval = 0;
                if (BGID != -1)
                {
                    List<JsonTag> Tags = new List<JsonTag>();
                    Tags = (List<JsonTag>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(tagsJSON, typeof(List<JsonTag>));

                    Entities.WorkOrderTag model = new Entities.WorkOrderTag();


                    foreach (JsonTag tag in Tags)
                    {
                        model.BGID = BGID;
                        model.CreateUserID = userID;
                        model.ModifyTime = DateTime.Now;
                        model.Status = Convert.ToInt32(tag.Status);
                        model.TagName = HttpContext.Current.Server.UrlDecode(tag.TagName);
                        model.RecID = tag.TagID;
                        model.OrderNum = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(tag.OrderNum.ToString()));
                        retval = BLL.WorkOrderTag.Instance.Update(model);
                    }
                }

                if (retval > 0)
                {
                    message = "操作成功";
                }
                else
                {
                    message = "操作失败";
                }
                context.Response.Write(message);
                context.Response.End();
            }
            else
            {
                success = false;
                message = "request error";
                BitAuto.ISDC.CC2012.BLL.AJAXHelper.WrapJsonResponse(success, result, message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class JsonTag
        {
            public int TagID { get; set; }
            public string TagName { get; set; }
            public string Status { get; set; }
            public int OrderNum { get; set; }
        }
    }
}