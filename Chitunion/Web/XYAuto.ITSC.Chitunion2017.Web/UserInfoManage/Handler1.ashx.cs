using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace XYAuto.ITSC.Chitunion2017.Web.UserInfoManage
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            //string employeeNumber = context.Request.Params["employeeNumber"].ToString();
            context.Response.Write(GetUserInfo());
        }
        public string Method
        {
            get { return HttpContext.Current.Request["Method"] == null ? string.Empty : HttpContext.Current.Request["Method"].ToString().Trim(); }
        }
        public string EmployeeNumber
        {
            get { return HttpContext.Current.Request["EmployeeNumber"] == null ? string.Empty : HttpContext.Current.Request["EmployeeNumber"].ToString().Trim(); }
        }
        public string Mobile
        {
            get { return HttpContext.Current.Request["Mobile"] == null ? string.Empty : HttpContext.Current.Request["Mobile"].ToString().Trim(); }
        }
        public string Email
        {
            get { return HttpContext.Current.Request["Email"] == null ? string.Empty : HttpContext.Current.Request["Email"].ToString().Trim(); }
        }
        public string UserName
        {
            get { return HttpContext.Current.Request["UserName"] == null ? string.Empty : HttpContext.Current.Request["UserName"].ToString().Trim(); }
        }
        public string SysID
        {
            get { return HttpContext.Current.Request["SysID"] == null ? string.Empty : HttpContext.Current.Request["SysID"].ToString().Trim(); }
        }
        public string RoleID
        {
            get { return HttpContext.Current.Request["RoleID"] == null ? string.Empty : HttpContext.Current.Request["RoleID"].ToString().Trim(); }
        }
        public string SysUserID
        {
            get { return HttpContext.Current.Request["SysUserID"] == null ? string.Empty : HttpContext.Current.Request["SysUserID"].ToString().Trim(); }
        }
        public string TrueName
        {
            get { return HttpContext.Current.Request["TrueName"] == null ? string.Empty : HttpContext.Current.Request["TrueName"].ToString().Trim(); }
        }
        public string UserID
        {
            get { return HttpContext.Current.Request["UserID"] == null ? string.Empty : HttpContext.Current.Request["UserID"].ToString().Trim(); }
        }
        public string GetUserInfo()
        {
            BLL.EmployeeNumber em = new BLL.EmployeeNumber();
            if (Method == "SelectEmployee")
            {

                return em.GetUserInfo(EmployeeNumber);
            }
            else if (Method == "SelectRole")
            {
                return em.GetInsideRoles();
            }
            else if (Method == "Add")
            {
                if (!string.IsNullOrWhiteSpace(UserID) && Convert.ToInt32(UserID) > 0)
                {
                    return em.UpdateRolIDByUserID(Convert.ToInt32(UserID), RoleID);
                }
                else
                {
                    return em.InsertUserInfo(EmployeeNumber, Mobile, Email, UserName, SysID, RoleID, Convert.ToInt32(SysUserID), TrueName);
                }

            }
            else if (Method == "SelectUserInfo")
            {
                return em.SelectUserInfoByUserID(UserID == "" ? 0 : Convert.ToInt32(UserID));
            }
            else { return "参数类型错误"; }


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