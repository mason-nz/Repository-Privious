using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class SysRightUserInfo
    {
        /// <summary>
        /// 员工id
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string DepartID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 部门id路径
        /// </summary>
        public string DepartPath { get; set; }
        /// <summary>
        /// 部门全名称
        /// </summary>
        public string NamePath { get; set; }

        /// 分组id
        /// <summary>
        /// 分组id
        /// </summary>
        public int BGID { get; set; }
        /// 工号
        /// <summary>
        /// 工号
        /// </summary>
        public string AgentNum { get; set; }
        /// 业务线
        /// <summary>
        /// 业务线
        /// </summary>
        public int BusinessLine { get; set; }
        /// 邮箱
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// 域账号
        /// <summary>
        /// 域账号
        /// </summary>
        public string ADName { get; set; }

        /// <summary>
        /// 主部门名称
        /// </summary>
        public string MainDepartName
        {
            get
            {
                if (!string.IsNullOrEmpty(NamePath))
                {
                    string[] array = NamePath.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length > 0)
                    {
                        return array[0];
                    }
                }
                return DepartName;
            }
        }

        public SysRightUserInfo(DataRow dr)
        {
            UserID = CommonFunction.ObjectToInteger(dr["UserID"]);
            UserCode = CommonFunction.ObjectToString(dr["UserCode"]);
            TrueName = CommonFunction.ObjectToString(dr["TrueName"]);
            DepartID = CommonFunction.ObjectToString(dr["DepartID"]);
            DepartName = CommonFunction.ObjectToString(dr["DepartName"]);
            DepartPath = CommonFunction.ObjectToString(dr["DepartPath"]);
            NamePath = CommonFunction.ObjectToString(dr["NamePath"]);
            BGID = CommonFunction.ObjectToInteger(dr["BGID"]);
            AgentNum = CommonFunction.ObjectToString(dr["AgentNum"]);
            BusinessLine = CommonFunction.ObjectToInteger(dr["BusinessLine"].ToString());
            Email = CommonFunction.ObjectToString(dr["Email"]);
            ADName = CommonFunction.ObjectToString(dr["ADName"]);
        }
        public SysRightUserInfo()
        {
        }
    }
}
