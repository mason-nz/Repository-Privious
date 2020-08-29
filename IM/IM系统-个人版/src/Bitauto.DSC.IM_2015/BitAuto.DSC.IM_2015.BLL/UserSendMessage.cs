using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    public class UserSendMessage
    {
        #region Instance
        public static readonly UserSendMessage Instance = new UserSendMessage();
        #endregion


        /// <summary>
        /// 查询用户下的用户组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigthByUserID(int userId)
        {
            return Dal.UserSendMessage.Instance.GetUserGroupDataRigthByUserID(userId);
        }

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SMSTemplate GetSMSTemplate(int RecID)
        {
            //该表无主键信息，请自定义主键/条件字段
            return Dal.UserSendMessage.Instance.GetSMSTemplate(RecID);
        }
        #endregion

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetSMSTemplate(QuerySMSTemplate query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserSendMessage.Instance.GetSMSTemplate(query, order, currentPage, pageSize, out totalCount);
        }


        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetSurveyCategory(QuerySurveyCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserSendMessage.Instance.GetSurveyCategory(query, order, currentPage, pageSize, out totalCount);
        }


        /// <summary>
        /// 获取当前用户所属分组合并管辖分组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetCurrentUserGroups(int userid)
        {
            //所属分组
            DataTable bgdt1 = Dal.UserSendMessage.Instance.GetEmployeeAgent(userid);
            //管辖分组
            DataTable bgdt2 = Dal.UserSendMessage.Instance.GetInUseBusinessGroup(userid);
            //合并数据
            DataTable bgdt = new DataTable();
            bgdt.Columns.Add("BGID");
            bgdt.Columns.Add("Name");
            AddDatatable(bgdt1, bgdt);
            AddDatatable(bgdt2, bgdt);
            return bgdt;
        }
        /// <summary>
        /// 合并表格
        /// </summary>
        /// <param name="rdt"></param>
        /// <param name="dedt"></param>
        private static void AddDatatable(DataTable rdt, DataTable dedt)
        {
            if (rdt != null)
            {
                foreach (DataRow dr in rdt.Rows)
                {
                    if (dedt.Select("BGID='" + ObjectToInteger(dr["BGID"]) + "'").Length == 0)
                    {
                        dedt.Rows.Add(new object[] { dr["BGID"], dr["Name"] });
                    }
                }
            }
        }
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjectToInteger(object obj)
        {
            return ObjectToInteger(obj, 0);
        }
        public static int ObjectToInteger(object obj, int dvalue)
        {
            if (obj == null)
                return dvalue;
            int a = 0;
            if (int.TryParse(obj.ToString(), out a))
                return a;
            else return dvalue;
        }


        public string GetGroupStr(int userid)
        {
            return Dal.UserSendMessage.Instance.GetGroupStr(userid);
        }

        /// <summary>
        /// 根据组串获取业务组数据
        /// </summary>
        /// <param name="bgids"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupByBGIDS(string bgids)
        {
            return Dal.UserSendMessage.Instance.GetBusinessGroupByBGIDS(bgids);
        }


        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectInfo(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserSendMessage.Instance.GetProjectInfo(query, order, currentPage, pageSize, out totalCount);
        }



        /// <summary>
        /// 通过UserID得到一个对象实体
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgentByUserID(int UserID)
        {
            return Dal.UserSendMessage.Instance.GetEmployeeAgentByUserID(UserID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.SMSSendHistory model)
        {
            Dal.UserSendMessage.Instance.Insert(model);
        }
    }
}
