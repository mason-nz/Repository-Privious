using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using NPOI.SS.Formula.Functions;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���KLFAQ ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:08 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KLFAQ
    {
        #region Instance
        public static readonly KLFAQ Instance = new KLFAQ();
        #endregion

        #region Contructor
        protected KLFAQ()
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
        public DataTable GetKLFAQ(QueryKLFAQ query, string order, int currentPage, int pageSize, out int totalCount)
        {
            //֪ʶ����ദ��

            return Dal.KLFAQ.Instance.GetKLFAQ(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetKLFAQ(QueryKnowledgeLib query, string order, int currentPage, int pageSize, string wherePlug, out int totalCount)
        {
            return Dal.KLFAQ.Instance.GetKLFAQForManage(query, order, currentPage, pageSize, wherePlug, out totalCount);
        }


        public DataSet GetKLFAQReport(int UserId, string order, string where, int currentPage, int pageSize)
        {
            return Dal.KLFAQ.Instance.GetKLFAQReport(UserId, order, where, currentPage, pageSize);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KLFAQ.Instance.GetKLFAQ(new QueryKLFAQ(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region �ж�һ��֪ʶ�����Ƿ�
        /// <summary>
        /// �ж�һ��֪ʶ�����Ƿ���FAQ
        /// </summary>
        /// <param name="knoledgeID">֪ʶ��ID</param>
        /// <returns></returns>
        public bool IsHaveFAQ(string knoledgeID)
        {
            if (knoledgeID == "" || knoledgeID == null)
            {
                return false;
            }
            return Dal.KLFAQ.Instance.IsHaveFAQ(knoledgeID);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.KLFAQ GetKLFAQ(long KLFAQID)
        {

            return Dal.KLFAQ.Instance.GetKLFAQ(KLFAQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByKLFAQID(long KLFAQID)
        {
            QueryKLFAQ query = new QueryKLFAQ();
            query.KLFAQID = KLFAQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKLFAQ(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLFAQ model)
        {
            return Dal.KLFAQ.Instance.Update(sqltran, model);
        }

        public bool UpdateFAQs(SqlTransaction sqltran, List<Entities.KLFAQ> FAQList, long KLID)
        {
            string msg = "";
            try
            {
                foreach (Entities.KLFAQ FAQ in FAQList)
                {
                    FAQ.ModifyTime = DateTime.Now;
                    FAQ.ModifyUserID = Util.GetLoginUserID();
                    FAQ.KLID = KLID;
                    if (FAQ.KLFAQID == 0)
                    {//����
                        FAQ.CreateTime = DateTime.Now;
                        FAQ.CreateUserID = Util.GetLoginUserID();
                        Insert(FAQ);
                        msg += "���FAQ�����⣺" + FAQ.Ask + ",�𰸣�" + FAQ.Question + "��";
                    }
                    else
                    {//�޸�
                        Entities.KLFAQ FAQOri = new Entities.KLFAQ();
                        FAQOri = GetKLFAQ(FAQ.KLFAQID);
                        if (FAQ.Ask != FAQOri.Ask || FAQ.Question != FAQOri.Question)
                        {
                            msg += " �޸�FAQ";
                            if (FAQ.Ask != FAQOri.Ask)
                            {
                                msg += ",�����ɡ�" + FAQOri.Ask + "����Ϊ��" + FAQ.Ask + "��";
                            }
                            else
                            {
                                msg += ",���ɡ�" + FAQOri.Question + "����Ϊ��" + FAQ.Question + "��";
                            }
                            msg += ";";
                            Update(FAQ);
                        }
                    }
                }
                //д����־
                BLL.Util.InsertUserLog(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Delete

        public int DeleteByKLID(SqlTransaction sqltran, long KLID)
        {
            return Dal.KLFAQ.Instance.DeleteByKLID(sqltran, KLID);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long KLFAQID)
        {
            return Dal.KLFAQ.Instance.Delete(KLFAQID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLFAQID)
        {
            return Dal.KLFAQ.Instance.Delete(sqltran, KLFAQID);
        }

        public bool Delete(SqlTransaction sqltran, string KLFAQIDs, out string msg)
        {
            KLFAQIDs = removeLastComma(KLFAQIDs);
            string[] FAQIDArr = KLFAQIDs.Split(',');

            try
            {
                msg = "";
                foreach (string FAQID in FAQIDArr)
                {
                    long faqID = long.Parse(FAQID);
                    Entities.KLFAQ FAQ = new Entities.KLFAQ();
                    FAQ = this.GetKLFAQ(faqID);
                    if (FAQ != null)
                    {
                        msg += "ɾ��FAQ�ɹ���ID��" + FAQID + ",���⣺" + FAQ.Ask + "�𰸣�" + FAQ.Question;
                        Delete(sqltran, faqID);
                    }
                }
                //д����־
                BLL.Util.InsertUserLog(msg);
                return true;
            }
            catch
            {
                msg = "FAQɾ��ʧ�ܣ�";
                return false;
            }
        }
        #region ����ȥ�����һ������
        /// <summary>
        /// ����ȥ�����һ������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string removeLastComma(string str)
        {
            int strLength = str.Length;
            if (str == null || str == "")
            {
                return "";
            }
            else
            {
                if (str.Substring(strLength - 1, 1) == ",")
                {
                    return str.Substring(0, strLength - 1);
                }
                else
                {
                    return str;
                }
            }
        }
        #endregion
        #endregion

    }
}

