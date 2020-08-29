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
    /// ҵ���߼���SurveyInfo ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetSurveyInfo(QuerySurveyInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyInfo.Instance.GetSurveyInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SurveyInfo.Instance.GetSurveyInfo(new QuerySurveyInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyInfo GetSurveyInfo(int SIID)
        {

            return Dal.SurveyInfo.Instance.GetSurveyInfo(SIID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
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
        /// ����һ������
        /// </summary>
        public int Insert(Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            return Dal.SurveyInfo.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int SIID)
        {

            return Dal.SurveyInfo.Instance.Delete(SIID);
        }

        /// <summary>
        /// ɾ��һ������
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
        /// �����Ծ�id������Աid�жϸ����Ƿ���ĳȨ��,RightNoΪ����Ȩ�ޱ��� add by qizq 2012-10-31
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
                //�жϷ���Ȩ�ޣ����Ȩ����2-���飬���ܿ��������˴�������Ϣ�����Ȩ����1-���ˣ���ֻ�ܿ����˴�������Ϣ 
                DataTable dt_userGroupDataRight = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userID);

                List<string> ownGroup = new List<string>();//Ȩ���Ǳ���� �鴮
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

