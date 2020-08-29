using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Config;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class WOrderTag
    {
        public static WOrderTag Instance = new WOrderTag();

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="status">�Ƿ����,�����á�,���ָ�</param>
        /// <returns></returns>
        public DataTable GetAllData(Entities.QueryWOrderTag query)
        {
            return Dal.WOrderTag.Instance.GetAllData(query);
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public int GetMaxSortNum(string pid = "")
        {
            return Dal.WOrderTag.Instance.GetMaxSortNum(pid);
        }

        /// <summary>
        /// ���ơ�����
        /// </summary>
        /// <param name="CurrId"></param>
        /// <param name="CurrSort"></param>
        /// <param name="NextId"></param>
        /// <param name="NextSort"></param>
        public bool ChangeOrder(int CurrId, int CurrSort, int NextId, int NextSort)
        {
            int loginID = BLL.Util.GetLoginUserID();

            Entities.WOrderTagInfo entity = new WOrderTagInfo();
            entity.RecID = CurrId;
            entity.SortNum = NextSort;
            entity.LastUpdateTime = DateTime.Now;
            entity.LastUpdateUserID = loginID;

            Entities.WOrderTagInfo entity2 = new WOrderTagInfo();
            entity2.RecID = NextId;
            entity2.SortNum = CurrSort;
            entity2.LastUpdateTime = DateTime.Now;
            entity2.LastUpdateUserID = loginID;


            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection conn = new SqlConnection(connectionstrings);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction("businesstype");
            try
            {
                CommonDal.Instance.UpdateComAdoInfo<Entities.WOrderTagInfo>(entity, tran);
                CommonDal.Instance.UpdateComAdoInfo<Entities.WOrderTagInfo>(entity2, tran);

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }

                BLL.Loger.Log4Net.Error("��ǩ�ƶ�����," + ex.StackTrace + " ," + ex.Message.ToString());

            }
            finally
            {
                conn.Close();
                tran.Dispose();
                conn.Dispose();
            }
            return false;

        }


        /// <summary>
        /// ��ȡ�ȼ�����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetLevelData(Entities.QueryWOrderTag query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WOrderTag.Instance.GetLevelData(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ��ȡ��ǩ����
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public string GetTagNameByRecId(int recId)
        {
            return Dal.WOrderTag.Instance.GetTagNameByRecId(recId);
        }

        /// <summary>
        /// ���ݱ�ǩid��ȡ��ǩ·����һ��+����
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public string GetTagNamePathByRecId(int recId, string spliter)
        {
            string backVal = "";
            DataTable dt = Dal.WOrderTag.Instance.GetTagNamePathByRecId(recId);
            if (dt != null && dt.Rows.Count > 0)
            {
                string pTagName = dt.Rows[0][0] == null ? "" : dt.Rows[0][0].ToString();
                string tagName = dt.Rows[0][1] == null ? "" : dt.Rows[0][1].ToString();
                if (!string.IsNullOrEmpty(pTagName) && !string.IsNullOrEmpty(tagName))
                {
                    backVal = pTagName + spliter + tagName;
                }
                else
                {
                    backVal = pTagName + tagName;
                }
            }
            return backVal;
        }

        /// <summary>
        /// ��ȡÿ��ҵ�����͵ı�ǩ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetBusiTagCount(Entities.QueryWOrderTag query)
        {
            return Dal.WOrderTag.Instance.GetBusiTagCount(query);
        }

        /// <summary>
        /// ��ȡ��ǩ�����ҵ������
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetMaxBusiTagCount(Entities.QueryWOrderTag query)
        {
            DataTable dt = GetBusiTagCount(query);

            int busid = 0;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    busid = int.Parse(dt.Rows[0]["BusiTypeID"].ToString());
                }
            }
            catch (Exception ex) { throw ex; }

            return busid;
        }
    }
}
