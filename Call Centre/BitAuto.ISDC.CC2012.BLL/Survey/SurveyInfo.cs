using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类SurveyInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyInfo
    {
        #region Instance
        public static readonly SurveyInfo Instance = new SurveyInfo();
        #endregion

        #region Contructor
        protected SurveyInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetSurveyInfo(QuerySurveyInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyInfo.Instance.GetSurveyInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SurveyInfo.Instance.GetSurveyInfo(new QuerySurveyInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyInfo GetSurveyInfo(int SIID)
        {

            return Dal.SurveyInfo.Instance.GetSurveyInfo(SIID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsBySIID(int SIID)
        {
            QuerySurveyInfo query = new QuerySurveyInfo();
            query.SIID = SIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int SIID)
        {

            return Dal.SurveyInfo.Instance.Delete(SIID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SIID)
        {

            return Dal.SurveyInfo.Instance.Delete(sqltran, SIID);
        }

        #endregion

        public DataTable getCreateUser()
        {
            return Dal.SurveyInfo.Instance.getCreateUser();
        }

        /// <summary>
        /// 根据试卷id，和人员id判断该人是否有某权限,RightNo为功能权限编码 add by qizq 2012-10-31
        /// </summary>
        /// <param name="siid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool HaveRight(string BGID, int userID, string RightNo)
        {
            bool flag = true;
            bool right = BLL.Util.CheckRight(userID, RightNo);
            if (right)
            {
                //判断分组权限，如果权限是2-本组，则能看到本组人创建的信息；如果权限是1-本人，则只能看本人创建的信息 
                DataTable dt_userGroupDataRight = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userID);

                List<string> ownGroup = new List<string>();//权限是本组的 组串
                for (int i = 0; i < dt_userGroupDataRight.Rows.Count; i++)
                {
                    if (ownGroup.Contains(dt_userGroupDataRight.Rows[i]["BGID"].ToString()) == false)
                    {
                        ownGroup.Add(dt_userGroupDataRight.Rows[i]["BGID"].ToString());
                    }
                }
                if (ownGroup.Contains(BGID))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

            }
            else
            {
                flag = false;
            }
            return flag;
        }

    }
}

