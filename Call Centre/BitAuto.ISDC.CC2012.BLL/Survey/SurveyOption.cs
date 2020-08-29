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
    /// ҵ���߼���SurveyOption ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:18 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyOption
    {
        #region Instance
        public static readonly SurveyOption Instance = new SurveyOption();
        #endregion

        #region Contructor
        protected SurveyOption()
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
        public DataTable GetSurveyOption(QuerySurveyOption query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyOption.Instance.GetSurveyOption(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SurveyOption.Instance.GetSurveyOption(new QuerySurveyOption(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ��������ID��ѯ�������µ�ѡ��
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public int GetSurveyOptionCountBySQID(int SQID)
        {
            return Dal.SurveyOption.Instance.GetSurveyOptionCountBySQID(SQID);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyOption GetSurveyOption(int SOID)
        {

            return Dal.SurveyOption.Instance.GetSurveyOption(SOID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsBySOID(int SOID)
        {
            QuerySurveyOption query = new QuerySurveyOption();
            query.SOID = SOID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyOption(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.SurveyOption model)
        {
            return Dal.SurveyOption.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyOption model)
        {
            return Dal.SurveyOption.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.SurveyOption model)
        {
            return Dal.SurveyOption.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyOption model)
        {
            return Dal.SurveyOption.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int SOID)
        {

            return Dal.SurveyOption.Instance.Delete(SOID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SOID)
        {

            return Dal.SurveyOption.Instance.Delete(sqltran, SOID);
        }

        #endregion



        public List<Entities.SurveyOption> GetSurveyOptionList(int siid)
        {
            return Dal.SurveyOption.Instance.GetSurveyOptionList(siid);
        }
        /// <summary>
        /// ��ѯ�����µ�����ѡ��
        /// </summary>
        /// <param name="sqid"></param>
        /// <returns></returns>
        public List<Entities.SurveyOption> GetSurveyOptionListBySQID(int sqid)
        {
            return Dal.SurveyOption.Instance.GetSurveyOptionListBySQID(sqid);
        }

        /// <summary>
        /// ȡ�ʾ�������ѡ��
        /// </summary>
        /// <param name="siid"></param>
        /// <returns></returns>
        public DataTable GetOptionTable(int siid)
        {
            DataTable dtOption = null;
            QuerySurveyOption query = new QuerySurveyOption();
            query.Status = 0;
            query.SIID = siid;
            int allcount = 0;
            dtOption = GetSurveyOption(query, "OrderNum", 1, 100000, out allcount);
            return dtOption;
        }


        /// <summary>
        /// ������Ŀȡѡ��
        /// </summary>
        /// <param name="dtOption"></param>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable GetOptionBySQID(DataTable dtOption, int SQID)
        {
            DataTable dt = null;
            if (dtOption != null)
            {
                dt = dtOption.Clone();
                DataView dv = dtOption.DefaultView;
                dv.RowFilter = "SQID=" + SQID;
                if (dv != null && dv.Count > 0)
                {
                    for (int i = 0; i < dv.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["SOID"] = dv[i]["SOID"];
                        row["SIID"] = dv[i]["SIID"];
                        row["SQID"] = dv[i]["SQID"];
                        row["OptionName"] = dv[i]["OptionName"];
                        row["IsBlank"] = dv[i]["IsBlank"];
                        row["Score"] = dv[i]["Score"];
                        row["OrderNum"] = dv[i]["OrderNum"];
                        row["Status"] = dv[i]["Status"];
                        row["CreateTime"] = dv[i]["CreateTime"];
                        row["CreateUserID"] = dv[i]["CreateUserID"];
                        row["ModifyTime"] = dv[i]["ModifyTime"];
                        row["ModifyUserID"] = dv[i]["ModifyUserID"];
                        row["linkid"] = dv[i]["linkid"];
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }
    }
}

