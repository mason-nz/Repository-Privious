using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���GroupOrderOrg ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-11-04 09:34:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class GroupOrderOrg
    {
        #region Instance
        public static readonly GroupOrderOrg Instance = new GroupOrderOrg();
        #endregion

        #region Contructor
        protected GroupOrderOrg()
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
        public DataTable GetGroupOrderOrg(QueryGroupOrderOrg query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrg(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrg(new QueryGroupOrderOrg(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ��ȡ������ȶ���ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxYPOrderID()
        {
            return Dal.GroupOrderOrg.Instance.GetMaxYPOrderID();
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.GroupOrderOrg GetGroupOrderOrg(long RecID)
        {

            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrg(RecID);
        }

        /// <summary>
        /// �������ȶ���ID������ʵ��
        /// </summary>
        /// <param name="OrderID">���ȶ���ID</param>
        /// <returns>����GroupOrderOrgʵ��</returns>
        public Entities.GroupOrderOrg GetGroupOrderOrgByOrderID(int OrderID)
        {
            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrgByOrderID(OrderID);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryGroupOrderOrg query = new QueryGroupOrderOrg();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrderOrg(query, string.Empty, 1, 1, out count);
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
        /// �������ȶ���ID���ж������Ƿ����
        /// </summary>
        /// <param name="YPOrderID">���ȶ���ID</param>
        /// <returns></returns>
        public bool IsExistsByYPOrderID(int YPOrderID)
        {
            return Dal.GroupOrderOrg.Instance.IsExistsByYPOrderID(YPOrderID);
        }
        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.GroupOrderOrg.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.GroupOrderOrg.Instance.Delete(sqltran, RecID);
        }

        #endregion

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dt"></param>
        public void GenTask(DataTable dt)
        {
            int taskCount = 0;
            int existCount = 0;
            BLL.Loger.Log4Net.Info("����_����_�Ź�����_��ʼ:");
            DateTime dtNow = DateTime.Now;
            DataTable dtNew = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int YPOrderID = int.Parse(dt.Rows[i]["OrderID"].ToString());
                if (BLL.GroupOrderOrg.Instance.IsExistsByYPOrderID(YPOrderID))//����
                {
                    BLL.Util.InsertUserLogNoUser("�����Ź�����ID��" + YPOrderID + "�ļ�¼�Ѿ����ڣ�������������"); existCount++;
                }
                else//������
                {
                    dtNew.ImportRow(dt.Rows[i]);
                }
            }
            BLL.Loger.Log4Net.Info("����õ��Ķ�����������" + existCount + "�������ں������Ŀ����Ѿ�������");

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("GroupOrderOrgTransaction");
            try
            {
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    Entities.GroupOrderOrg model = InitOrderNewCarLog(dtNew.Rows[i]);
                    if (model != null)
                    {
                        long GroupOrderOrgID = Insert(tran, model); //���� GroupOrderOrg ��
                        BLL.Loger.Log4Net.Info(string.Format("�Ź��������ɱ�GroupOrderOrg�ɹ�������IDΪ��{0},���ȶ���IDΪ��{1},����ID��{2}",
                                                  GroupOrderOrgID, model.OrderID, model.CarID));

                        if (model.CarMasterID != null && model.CarMasterID.Value > 0 &&
                            model.CarSerialID != null && model.CarSerialID.Value > 0 &&
                            model.CarID != null && model.CarID.Value > 0)
                        {
                            //Add By Masj At 2013.12.4 ����Ϊ�գ���ֻ����GroupOrderOrg �����������񣬲�����GroupOrder ��
                            long taskID = BLL.GroupOrderTask.Instance.InsertByOrder(tran, model); //���ݽӿڵõ���ԭʼ������������
                            if (BLL.GroupOrder.Instance.IsExistsByTaskID(taskID))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "�Ź�����ID" + taskID + "�ļ�¼�Ѿ����ڣ�������������");
                            }
                            else if (BLL.GroupOrder.Instance.InsertByTaskID(tran, taskID, model))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "�Ź�����ID" + taskID + "�ļ�¼���ɳɹ���"); taskCount++;
                            }
                        }
                    }
                }
                //�����ύ
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                string msg = "���³�����������ʧ��!ʧ��ԭ��" + ex.Message;
                BLL.Util.InsertUserLogNoUser(msg);
                BLL.Loger.Log4Net.Error(msg, ex);
            }
            finally
            {
                connection.Close();
            }
            string msgTitle = "һ�������Ź�����" + taskCount + "��";
            BLL.Util.InsertUserLogNoUser(msgTitle);
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Now - dtNow;
            BLL.Loger.Log4Net.Info("����_����_�Ź�����_����:" + msgTitle + ",��ʱ" + ts.TotalSeconds + "�롣");
        }

        /// <summary>
        /// ��ʼ���Ź�������־��Ϣ
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Entities.GroupOrderOrg InitOrderNewCarLog(DataRow dr)
        {
            Entities.GroupOrderOrg model = new Entities.GroupOrderOrg();

            try
            {
                model.OrderID = int.Parse(dr["OrderID"].ToString());
                model.OrderCode = int.Parse(dr["OrderCode"].ToString().Trim());
                model.CarMasterID = (int?)GetColumnDefaultValue(dr, "CarMasterID", typeof(int));
                model.CarMasterName = dr["CarMasterName"].ToString().Trim();
                model.CarSerialID = (int?)GetColumnDefaultValue(dr, "CarSerialID", typeof(int));
                model.CarSerialName = dr["CarSerialName"].ToString().Trim();
                model.CarID = (int?)GetColumnDefaultValue(dr, "CarID", typeof(int));
                model.CarName = dr["CarName"].ToString().Trim();
                model.Price = (decimal?)GetColumnDefaultValue(dr, "Price", typeof(decimal));
                model.DealerID = (int?)GetColumnDefaultValue(dr, "DealerID", typeof(int));
                model.DealerName = dr["DealerName"].ToString().Trim();
                model.CustomerName = dr["CustomerName"].ToString().Trim();
                model.CustomerTel = dr["CustomerTel"].ToString().Trim();
                model.OrderCreateTime = DateTime.Parse(dr["CreateDateTime"].ToString());
                model.ProvinceID = (int?)GetColumnDefaultValue(dr, "ProvinceID", typeof(int));
                model.ProvinceName = dr["ProvinceName"].ToString().Trim();
                model.CityID = (int?)GetColumnDefaultValue(dr, "CityID", typeof(int));
                model.CityName = dr["CityName"].ToString().Trim();

            }
            catch (Exception ex)
            {
                BLL.Util.InsertUserLogNoUser("��ʼ���Ź�������Ϣʧ��!ʧ��ԭ��" + ex.Message);
                return null;
            }
            return model;
        }

        private static object GetColumnDefaultValue(DataRow dr, string columnName, Type t)
        {
            object ret = null;
            if (dr.Table.Columns.Contains(columnName))
            {
                switch (t.ToString())
                {
                    case "System.String": ret = dr[columnName].ToString().Trim();
                        break;
                    case "System.Decimal": decimal dec = -1;
                        if (decimal.TryParse(dr[columnName].ToString(), out dec))
                            ret = dec;
                        break;
                    case "System.Int32": int i = -1;
                        if (int.TryParse(dr[columnName].ToString(), out i))
                            ret = i;
                        break;
                    case "System.DateTime": DateTime dt = new DateTime();
                        if (DateTime.TryParse(dr[columnName].ToString(), out dt))
                            ret = dt;
                        break;
                    default:
                        break;
                }
            }
            return ret;
        }
    }
}

