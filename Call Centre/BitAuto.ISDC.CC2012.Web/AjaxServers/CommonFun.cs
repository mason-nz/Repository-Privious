using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    public static class CommonFun
    {
        /// <summary>
        /// 判断是否有编辑该页面的权限 add lxw 13.7.4
        /// </summary>
        /// <param name="thisUserID">当前页面的使用人</param>
        /// <param name="arrayBGID">当前页面的使用业务组 </param>
        /// <returns>1-无权限；2-查看权限；3-编辑权限;</returns>
        public static int judgeLimit(int thisUserID, string[] arrayBGID)
        {
            int loginID = BLL.Util.GetLoginUserID();

            int typeLimit = 1;

            if (thisUserID == loginID)
            {
                return typeLimit = 3;
            }

            DataTable dt_userGroupDataRight = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(loginID);
            string group = string.Empty;

            for (int i = 0; i < dt_userGroupDataRight.Rows.Count; i++)
            {
                //判断该登陆者的业务组是否有存在权限访问的业务组内
                if (Array.IndexOf(arrayBGID, dt_userGroupDataRight.Rows[i]["BGID"].ToString()) != -1)
                {
                    return typeLimit = 2;
                }

            }

            return typeLimit;
        }

    }

    public class OperEnum<T>
    {
        private Type typeEnum;

        public OperEnum()
        {
            typeEnum = typeof(T);
        }

        public void GetEnumName(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BLL.Util.GetEnumDataTable(typeEnum);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                msg += i == 0 ? "[{ID:'" + dr["value"].ToString() + "',Name:'" + dr["name"].ToString() + "'}" : ",{ID:'" + dr["value"].ToString() + "',Name:'" + dr["name"].ToString() + "'}";

                if (i == dt.Rows.Count - 1)
                {
                    msg += "]";
                }

            }
        }
    }


}