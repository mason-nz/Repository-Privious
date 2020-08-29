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
    /// ҵ���߼���WorkOrderTag ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderTag
    {
        #region Instance
        public static readonly WorkOrderTag Instance = new WorkOrderTag();
        #endregion

        #region Contructor
        protected WorkOrderTag()
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
        public DataTable GetWorkOrderTag(QueryWorkOrderTag query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderTag.Instance.GetWorkOrderTag(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.WorkOrderTag.Instance.GetWorkOrderTag(new QueryWorkOrderTag(), string.Empty, 1, 1000000, out totalCount);
        }

        public DataTable GetWorkOrderTagByBGID(int bgid, bool needIgnoreBgid)
        {
            return Dal.WorkOrderTag.Instance.GetWorkOrderTagByBGID(bgid, needIgnoreBgid);
        }
        public DataTable GetSimulerWorkOrder(int userid, string strTagName)
        {
            return Dal.WorkOrderTag.Instance.GetSimulerWorkOrder(userid, strTagName);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.WorkOrderTag GetWorkOrderTag(int RecID)
        {

            return Dal.WorkOrderTag.Instance.GetWorkOrderTag(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryWorkOrderTag query = new QueryWorkOrderTag();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderTag(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.WorkOrderTag model)
        {
            return Dal.WorkOrderTag.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderTag model)
        {
            return Dal.WorkOrderTag.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.WorkOrderTag model)
        {
            return Dal.WorkOrderTag.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderTag model)
        {
            return Dal.WorkOrderTag.Instance.Update(sqltran, model);
        }

        public void ChangeOrder(int RecId, bool isUp, string strStatus)
        {
            Dal.WorkOrderTag.Instance.ChangeOrder(RecId, isUp, strStatus);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.WorkOrderTag.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.WorkOrderTag.Instance.Delete(sqltran, RecID);
        }

        #endregion

        #region DeleteMany
        /// <summary>
        /// ɾ����������
        /// </summary>
        public int DeleteMany(string RecIDS)
        {
            return Dal.WorkOrderTag.Instance.DeleteMany(RecIDS);
        }
        #endregion

        #region SelectByOrderID
        /// <summary>
        /// ���ݹ����Ų�ѯ�õ��ı�ǩ
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderTagByOrderID(string orderid)
        {
            return Dal.WorkOrderTag.Instance.GetWorkOrderTagByOrderID(orderid);
        }

        public string GetWorkOrderTagNames(string orderid)
        {
            string tagNames = string.Empty;

            DataTable dtTag = Dal.WorkOrderTag.Instance.GetWorkOrderTagByOrderID(orderid);
            for (int i = 0; i < dtTag.Rows.Count; i++)
            {
                tagNames += dtTag.Rows[i]["TagName"].ToString() + ",";
            }

            return tagNames.TrimEnd(' ');
        }

        #endregion

        /// <summary>
        /// ����TagID�жϱ�ǩ�Ƿ�ʹ��
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public bool isUsedTagByTagID(int TagID)
        {
            return Dal.WorkOrderTag.Instance.isUsedTagByTagID(TagID);
        }

        public string GetWorkOrderTagJsonBySql(string where)
        {
            string jsonData = string.Empty;

            DataTable dt = BLL.Util.GetTableInfoBySql("select * from WorkOrderTag where status=0 " + where + " order by OrderNum asc");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                jsonData += i == 0 ? "[{ID:'" + dr["RecID"] + "',Name:'" + dr["TagName"] + "'}" : ",{ID:'" + dr["RecID"] + "',Name:'" + dr["TagName"] + "'}";

                if (i == dt.Rows.Count - 1)
                {
                    jsonData += "]";
                }

            }

            return jsonData;

        }
    }
}

