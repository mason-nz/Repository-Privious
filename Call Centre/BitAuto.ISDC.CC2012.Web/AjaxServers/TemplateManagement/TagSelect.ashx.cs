using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// Summary description for TagSelect
    /// </summary>
    public class TagSelect : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        #region 定义属性
        public string OrderID
        {
            get { return Request.QueryString["OrderID"] == null ? string.Empty : Request.QueryString["OrderID"].ToString().Trim(); }
        }
        public string tagsJSON
        {
            get { return currentContext.Request["tagsJSON"] == null ? string.Empty : Convert.ToString(currentContext.Request["tagsJSON"]); }
        }
        public int userID
        {
            get { return BLL.Util.GetLoginUserID(); }
        }
        #endregion
        bool success = true;
        string result = "";
        string message = "";
        public HttpContext currentContext;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            currentContext = context;
            if ((context.Request["TagsSelectAll"] + "").Trim() == "yes")//取登录坐席所属业务组下的标签
            {                
                //取当前登录人的业务组
                if (userID != -1)
                {
                    DataTable dt = BLL.BusinessGroup.Instance.GetBusinessGroupTagsByUserID(userID);

                    if (dt.Rows.Count > 0)
                    {
                        //在已选标签中遍历
                        //判断标签是否在已选择标签里，是则isSelected=true
                        int isSelected = 0;                       
                        StringBuilder sb = new StringBuilder();                        
                        foreach (DataRow row in dt.Rows)
                        {
                            if (OrderID != string.Empty)
                            {
                                //根据工单ID取已选标签
                                //已选择标签跟坐席权限下标签对比，如果相同则用IsSelected标识，供前台页标注”已选择“样式操作使用。
                                DataTable dt2 = BLL.WorkOrderTag.Instance.GetWorkOrderTagByOrderID(OrderID);

                                foreach (DataRow r in dt2.Rows)
                                {
                                    if (row["TagID"].ToString() == r["TagID"].ToString())
                                    {
                                        isSelected = 1;

                                    }
                                    else
                                    {
                                        isSelected = 0;
                                    }
                                }
                            }

                            sb.Append("{'BGID':'" + row["BGID"].ToString() + "','GroupName':'" + row["GroupName"].ToString() + "','TagID':'" + row["TagID"].ToString() +
                                        "','TagName':'" + row["TagName"].ToString() + "','IsSelected':" + isSelected + "},");
                        }

                        message = sb.ToString();
                        if (message.EndsWith(","))
                            message = message.Substring(0, message.Length - 1);
                    }


                }
                
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if ((context.Request["TagsSelectByOrderID"] + "").Trim() == "yes")//根据工单ID取已选择标签
            {                
                DataTable dt2 = new DataTable();

                dt2.Columns.Add("BGID", Type.GetType("System.String"));
                dt2.Columns.Add("TagID", Type.GetType("System.String"));
                dt2.Columns.Add("TagName", Type.GetType("System.String"));

                Random seed = new Random();
                Random randomNum = new Random(seed.Next());

                DataRow newRow;
                for (int i = 0; i < 3; i++)
                {
                    
                    for (int j = 0; j < 10; j++)
                    {
                        newRow = dt2.NewRow();
                        newRow["BGID"] = i;
                        newRow["TagID"] = randomNum.ToString();
                        newRow["TagName"] = "标签" + j;
                        dt2.Rows.Add(newRow);
                    }
                }

                if (dt2.Rows.Count > 0)
                {

                    StringBuilder sb = new StringBuilder();

                    foreach (DataRow row in dt2.Rows)
                    {
                        sb.Append("{'BGID':'" + row["BGID"].ToString() + "','TagID':'" + row["TagID"].ToString() +
                                  "','TagName':'" + row["TagName"].ToString() + "'},");
                    }

                    message = sb.ToString();
                    if (message.EndsWith(","))
                        message = message.Substring(0, message.Length - 1);

                }
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if ((context.Request["InsertOrderTagMapping2DB"] + "").Trim() == "yes")
            {
                int retval = 0;

                List<JsonTag> Tags = new List<JsonTag>();
                Tags = (List<JsonTag>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(tagsJSON, typeof(List<JsonTag>));

                Entities.WorkOrderTagMapping model = new Entities.WorkOrderTagMapping();


                foreach (JsonTag tag in Tags)
                {
                    model.OrderID = tag.OrderID;
                    model.TagID = tag.TagID;
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = userID;
                    model.ModifyUserID = userID;
                    model.ModifyTime = DateTime.Now;
                    model.Status = Convert.ToInt32(tag.Status);
                    retval = BLL.WorkOrderTagMapping.Instance.Insert(model);
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
            public string OrderID { get; set; }
            public string TagName { get; set; }
            public string Status { get; set; }
        }
    }
}