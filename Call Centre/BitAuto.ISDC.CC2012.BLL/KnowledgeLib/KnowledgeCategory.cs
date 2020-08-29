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
    /// ҵ���߼���KnowledgeCategory ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KnowledgeCategory
    {
        #region Instance
        public static readonly KnowledgeCategory Instance = new KnowledgeCategory();
        #endregion

        #region Contructor
        protected KnowledgeCategory()
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
        public DataTable GetKnowledgeCategory(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategory(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetKnowledgeCategoryWithRegion(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategoryWithRegion(query, BLL.Util.GetLoginUserID(),order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetKnowledgeCategoryForSearch(QueryKnowledgeCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategoryForSearch(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ���ݸ����� ȡ���ӽڵ��б�
        /// </summary>
        /// <param name="Parentname"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPName(string Parentname, int RegionID)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPName(Parentname,RegionID);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategory(new QueryKnowledgeCategory(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ȡ�ڵ����ӽڵ��Լ��ýڵ�����
        /// </summary>
        /// <param name="KCID"></param>
        /// <param name="Parentname"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPID(int KCID, string Parentname)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPID(KCID, Parentname);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.KnowledgeCategory GetKnowledgeCategory(int KCID)
        {

            return Dal.KnowledgeCategory.Instance.GetKnowledgeCategory(KCID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByKCID(int KCID)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.KCID = KCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1000000, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// �ж�ĳ�ڵ��Ƿ����ӽڵ�
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public bool IsExistsChildByKCID(int KCID)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Pid = KCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1, out count);
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

        /// <summary>
        /// ȡĳ���ڵ�
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategory(int level)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Level = level;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1000000, out count);
            return dt;
        }
        public DataTable GetCategoryWithRegion(int level)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Level = level;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategoryWithRegion(query, string.Empty, 1, 1000000, out count);
            return dt;
        }
        /// <summary>
        /// ȡĳ�ڵ����ӽڵ�
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryByPID(int KCID)
        {
            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Pid = KCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKnowledgeCategory(query, string.Empty, 1, 1000000, out count);
            return dt;
        }



        /// <summary>
        /// �޸�֪ʶ�����
        /// </summary>
        /// <param name="kcid"></param>
        /// <param name="klid"></param>
        /// <returns></returns>
        public int Update(string kcid, string klid)
        {

            return Dal.KnowledgeCategory.Instance.Update(kcid, klid);
        }

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Update(model);
        }


        /// <summary>
        /// ȡ�ڵ����ӽڵ㣬���ӽڵ㲻�����ӽڵ�
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPIDNotSon(int KCID)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPIDNotSon(KCID);
        }

        /// <summary>
        /// ȡ�ڵ����ӽڵ㣬���ӽڵ�����ӽڵ�
        /// </summary>
        /// <param name="KCID"></param>
        /// <returns></returns>
        public DataTable GetCategoryByPIDHaveSon(int KCID)
        {
            return Dal.KnowledgeCategory.Instance.GetCategoryByPIDHaveSon(KCID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KnowledgeCategory model)
        {
            return Dal.KnowledgeCategory.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int KCID)
        {

            return Dal.KnowledgeCategory.Instance.Delete(KCID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int KCID)
        {

            return Dal.KnowledgeCategory.Instance.Delete(sqltran, KCID);
        }

        #endregion



        /// <summary>
        /// ����֪ʶ�����״̬
        /// </summary>
        /// <param name="RequestKCID">���id</param>
        /// <param name="msg">�����Ϣ</param>
        public void ChangeKnowledgeCategoryStatus(string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid = 0;
            if (int.TryParse(RequestKCID, out kcid))
            {
                #region  ͨ��ʵ����в���
                //Entities.KnowledgeCategory model = GetKnowledgeCategory(kcid);
                //if (model != null)
                //{
                //    if (model.Status == 0)//����ʹ��
                //    {
                //        model.Status = 1;
                //    }
                //    else if (model.Status == 1) //�Ѿ�ͣ��
                //    {
                //        model.Status = 0;
                //    }
                //    int result = 0;
                //    try
                //    {
                //        result = Dal.KnowledgeCategory.Instance.UpdateKnowledgeCategoryStatus(model);
                //        if (result > 0)
                //        {
                //            msg = "{msg:'success'}";
                //        }
                //        else
                //        {
                //            msg = "{msg:'����ʧ�ܣ�'}";
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        msg = "{msg:'" + ex.Message + "'}";
                //    }
                //}
                #endregion

                int status = Dal.KnowledgeCategory.Instance.GetKnowledgeCategoryStatusByKCID(kcid);
                if (status != -5)
                {
                    Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
                    model.KCID = kcid;
                    if (status == 0)//����ʹ��
                    {
                        model.Status = 1;
                    }
                    else if (status == 1) //�Ѿ�ͣ��
                    {
                        model.Status = 0;
                    }
                    int result = 0;
                    try
                    {
                        result = Dal.KnowledgeCategory.Instance.UpdateKnowledgeCategoryStatus(model);
                        if (result > 0)
                        {
                            msg = "{msg:'success'}";
                        }
                        else
                        {
                            msg = "{msg:'����ʧ�ܣ�'}";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "{msg:'" + ex.Message + "'}";
                    }
                }
                else
                {
                    msg = "{msg:'δ�ҵ�ָ�����'}";
                }
            }
        }

        /// <summary>
        /// ���ݷ���id�߼�ɾ�����ࣨ����÷����Ѿ���ʹ�ã�����ɾ����
        /// </summary>
        /// <param name="RequestKCID">����id</param>
        /// <param name="msg">�����Ϣ</param>
        public void DeleteKnowledgeCategory(string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
            if (int.TryParse(RequestKCID, out kcid))
            {
                model.KCID = kcid;
                int count = Dal.KnowledgeCategory.Instance.GetCountTheKnowledgeCategoryUsed(model);
                if (count > 0)
                {
                    msg = "{msg:'�������֪ʶ������ʹ�ã��޷�ɾ����'}";
                }
                else
                {
                    Dal.KnowledgeCategory.Instance.DeleteKnowledgeCategory(model, out msg);
                }
            }
            else
            {
                msg = "{msg:'�������ʹ���'}";
            }
        }

        public void DeleteKnowledgeCategoryAndChildren(string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
            if (int.TryParse(RequestKCID, out kcid))
            {
                model.KCID = kcid;
                string kcids = GetChildrenKnowledgeCategoryKCIDS(RequestKCID);
                int count = Dal.KnowledgeCategory.Instance.GetNotDelStatusNum(kcids);
                if (count > 0)
                {
                    msg = "{msg:'�÷����÷�����ӷ�����֪ʶ������ʹ�ã��޷�ɾ�����볢�Էֲ�ɾ����'}";
                }
                else
                {
                    Dal.KnowledgeCategory.Instance.DeleteKnowledgeCategoryAndChildren(kcids, out msg);
                }
            }
            else
            {
                msg = "{msg:'�������ʹ���'}";
            }
        }
        public string GetChildrenKnowledgeCategoryKCIDS(string RequestKCID)
        {
            DataTable tbAllKC = Dal.KnowledgeCategory.Instance.GetAllKnowledgeCategory();

            string strIds = RequestKCID + "," + GetChildId(RequestKCID, tbAllKC);
            return strIds.Substring(0, strIds.Length - 1);
        }
        public string GetChildId(string KCID, DataTable tbAllKC)
        {
            string strIds = "";
            foreach (DataRow row in tbAllKC.Rows)
            {
                if (row["Pid"].ToString() == KCID)
                {
                    strIds += row["KCID"].ToString() + "," + GetChildId(row["KCID"].ToString(), tbAllKC);
                }
            }
            return strIds;
        }

        /// <summary>
        /// ����µķ�����Ϣ
        /// </summary>
        /// <param name="RequestName">��������</param>
        /// <param name="RequestKCID">����Id</param>
        /// <param name="RequestLevel">���༶��</param>
        /// <param name="msg">�����Ϣ</param>
        public void InsertKnowledgeCategory(string RequestName, string RequestKCID, string RequestLevel, int Regionid, out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();

            model.Name = RequestName;
            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                model.Pid = kcid;
                model.Level = level;
                model.Regionid = Regionid;
                Dal.KnowledgeCategory.Instance.InsertKnowledgeCategory(model, out msg);
            }
            else
            {
                msg = "{msg:'�������ʹ���'}";
            }
        }

        /// <summary>
        /// ���ݷ���Id����ָ���ķ�����Ϣ
        /// </summary>
        /// <param name="RequestName">��������</param>
        /// <param name="RequestKCID">����Id</param>
        /// <param name="msg"></param>
        public void UpdateKnowledgeCategory(string RequestName, string RequestKCID, out string msg)
        {
            msg = string.Empty;
            int kcid;
            Entities.KnowledgeCategory model = new Entities.KnowledgeCategory();
            model.Name = RequestName;
            if (int.TryParse(RequestKCID, out kcid))
            {
                model.KCID = kcid;
                Dal.KnowledgeCategory.Instance.UpdateKnowledgeCategory(model, out msg);
            }
            else
            {
                msg = "{msg:'�������ʹ���'}";
            }
        }


        /// <summary>
        /// ���ݸ�����id�ͼ��𣬻�ȡ�¼��ӷ��������б���Ϣ
        /// </summary>
        /// <param name="RequestLevel">����</param>
        /// <param name="RequestKCID">�����Id</param>
        /// <param name="msg"></param>
        public void BindKnowledgeCategory(string RequestLevel, string RequestKCID, int Regionid, out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;
            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                Entities.QueryKnowledgeCategory query = new Entities.QueryKnowledgeCategory();
                query.Level = level;
                query.Pid = kcid;
                query.Regionid = Regionid;

                DataTable dt = new DataTable(); ;
                dt = Dal.KnowledgeCategory.Instance.BindKnowledgeCategory(query);

                if (dt.Rows.Count > 0)
                {
                    msg += "{root:[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "'},";
                }
                if (dt.Rows.Count > 0)
                {
                    msg = msg.TrimEnd(',') + "]}";
                }
            }
        }


        /// <summary>
        /// ���ݸ�����id�ͼ��𣬻�ȡ�ӷ�����ϸ��Ϣ
        /// </summary>
        /// <param name="msg"></param>
        public void BindChildrenCategoryInfo(string RequestKCID, string RequestLevel, string RequestName, int Regionid, out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;

            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                Entities.QueryKnowledgeCategory query = new Entities.QueryKnowledgeCategory();
                query.Level = level;
                query.Pid = kcid;
                query.Name = RequestName;
                query.Regionid = Regionid;

                DataTable dt = new DataTable(); ;

                dt = Dal.KnowledgeCategory.Instance.BindChildrenCategoryInfo(query);

                if (dt.Rows.Count > 0)
                {
                    msg += "{root:[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "',status:'" + dt.Rows[i]["Status"].ToString() + "',parentName:'" + dt.Rows[i]["parentName"].ToString() + "',sortnum:'" + dt.Rows[i]["SortNum"].ToString() + "'},";
                }
                if (dt.Rows.Count > 0)
                {
                    msg = msg.TrimEnd(',') + "]}";
                }

                #region
                //if (string.IsNullOrEmpty(RequestName))
                //{
                //    if (dt.Rows.Count <= 0)
                //    {
                //        #region  ��ѯ����
                //        string strSelect2 = "SELECT  parentName =  ISNULL((SELECT name FROM KnowledgeCategory WHERE KCID=aa.Pid),'') " +
                //                ",aa.Name,aa.Status,aa.KCID,Pid " +
                //                "FROM    KnowledgeCategory aa " +
                //                "WHERE  aa.Status<>-1 AND aa.kcid='" + kcid + "' AND aa.Level='" + (level - 1) + "' ";
                //        using (SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStrings_CC"].ToString()))
                //        {
                //            conn2.Open();
                //            SqlDataAdapter adp2 = new SqlDataAdapter(strSelect2, conn2);
                //            adp2.Fill(dt);
                //        }
                //        #endregion

                //        if (dt.Rows.Count > 0)
                //        {
                //            msg += "{root:[";
                //        }
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "',status:'" + dt.Rows[i]["Status"].ToString() + "',parentName:'" + dt.Rows[i]["parentName"].ToString() + "'},";
                //        }
                //        if (dt.Rows.Count > 0)
                //        {
                //            msg = msg.TrimEnd(',') + "]}";
                //        }
                //    }
                //}
                #endregion
            }
        }

        public void SortNumUpOrDown(string sortid, string type, string info, out string msg)
        {
            Dal.KnowledgeCategory.Instance.SortNumUpOrDown(sortid, type, info, out  msg);
        }

    }
}

