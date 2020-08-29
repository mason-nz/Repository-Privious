using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.EmployeeService;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.EmployeeInfo;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class EmployeeNumber
    {
        private string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");

        /// <summary>
        /// 调用webService获取员工信息
        /// </summary>
        /// <param name="EmployeeNumber"></param>
        /// <returns></returns>
        public string GetUserInfo(string EmployeeNumber)
        {
            EmployeeService.EmployeeServiceSoapClient s = new EmployeeService.EmployeeServiceSoapClient();
            BLL.EmployeeService.Employee emp = s.GetEmployeeByEmployeeNumber(EmployeeNumber);
            return JsonConvert.SerializeObject(emp);
        }
        /// <summary>
        /// 获取内部角色
        /// </summary>
        /// <returns></returns>
        public string GetInsideRoles()
        {
            return JsonConvert.SerializeObject(Dal.EmployeeInfo.UserRoleInfo.Instance.GetInsideRoles());
        }

        public string InsertUserInfo(string EmployeeNumber, string Mobile, string Email, string UserName, string SysID, string RoleID, int SysUserID, string TrueName)
        {
            if (EmployeeInfo.Instance.GetUserNameCount(UserName.Trim(), UserConstant.Category) > 0)
            {
                return "该用户已存在，请重新输入！";
            }
            int CreateUserID = Common.UserInfo.GetLoginUserID();
            string pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash(UserInfo.DefaultPwd + UserConstant.Category + LoginPwdKey, System.Text.Encoding.UTF8);
            int userID = EmployeeInfo.Instance.InsertUserInfo(EmployeeNumber.Trim(), UserName.Trim(), Mobile, pwd, Email.Trim(), SysUserID, CreateUserID);
            if (userID > 0)
            {
                EmployeeInfo.Instance.InsertUserDetailAndRoleInfo(userID, TrueName.Trim(), RoleID.Trim(), SysID.Trim(), CreateUserID);
                return "保存成功";
            }
            else
            {
                return "保存失败";
            }
        }
        /// <summary>
        /// zlb 2017-07-24
        /// 根据用户ID查询用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public string SelectUserInfoByUserID(int UserID)
        {
            return JsonConvert.SerializeObject(EmployeeInfo.Instance.SelectUserInfoByUserID(UserID));
        }
        /// <summary>
        /// zlb 2017-07-24
        /// 根据用户ID修改角色
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="RoleID">角色</param>
        /// <returns></returns>
        public string UpdateRolIDByUserID(int UserID, string RoleID)
        {
            if (string.IsNullOrWhiteSpace(RoleID))
            {
                return "参数错误";
            }
            if (EmployeeInfo.Instance.UpdateRolIDByUserID(UserID, RoleID) > 0)
            {
                return "保存成功";
            }
            else
            {
                return "保存失败";
            }
        }

        ///// <summary>
        ///// 去除重复的用户名
        ///// </summary>
        ///// <param name="UserName">用户名</param>
        ///// <returns></returns>
        //private string RemoveRepeatUserName(string UserName)
        //{

        //}
    }
}
