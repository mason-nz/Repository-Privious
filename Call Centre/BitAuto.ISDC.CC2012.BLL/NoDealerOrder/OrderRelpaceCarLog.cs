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
    /// ҵ���߼���OrderRelpaceCarLog ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderRelpaceCarLog
    {
        #region Instance
        public static readonly OrderRelpaceCarLog Instance = new OrderRelpaceCarLog();
        #endregion

        #region Contructor
        protected OrderRelpaceCarLog()
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
        public DataTable GetOrderRelpaceCarLog(QueryOrderRelpaceCarLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderRelpaceCarLog.Instance.GetOrderRelpaceCarLog(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OrderRelpaceCarLog.Instance.GetOrderRelpaceCarLog(new QueryOrderRelpaceCarLog(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ��ȡ������ȶ���ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxYPOrderID()
        {
            return Dal.OrderRelpaceCarLog.Instance.GetMaxYPOrderID();
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.OrderRelpaceCarLog GetOrderRelpaceCarLog(long RecID)
        {

            return Dal.OrderRelpaceCarLog.Instance.GetOrderRelpaceCarLog(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryOrderRelpaceCarLog query = new QueryOrderRelpaceCarLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderRelpaceCarLog(query, string.Empty, 1, 1, out count);
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
            return Dal.OrderRelpaceCarLog.Instance.IsExistsByYPOrderID(YPOrderID);
        }
        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.OrderRelpaceCarLog model)
        {
            return Dal.OrderRelpaceCarLog.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderRelpaceCarLog model)
        {
            return Dal.OrderRelpaceCarLog.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.OrderRelpaceCarLog model)
        {
            return Dal.OrderRelpaceCarLog.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderRelpaceCarLog model)
        {
            return Dal.OrderRelpaceCarLog.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.OrderRelpaceCarLog.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.OrderRelpaceCarLog.Instance.Delete(sqltran, RecID);
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
            BLL.Loger.Log4Net.Info("����_����_�û�����_��ʼ:");
            DateTime dtNow = DateTime.Now;
            DataTable dtNew = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int YPOrderID = int.Parse(dt.Rows[i]["OrderBusinessOpportunityID"].ToString());
                int dealerID = -1;
                int.TryParse(dt.Rows[i]["DealerID"].ToString(), out dealerID);
                if (BLL.OrderRelpaceCarLog.Instance.IsExistsByYPOrderID(YPOrderID))//����
                {
                    BLL.Util.InsertUserLogNoUser("���û������ȶ���ID��" + YPOrderID + "�ļ�¼�Ѿ����ڣ�������������");
                }
                else if (dealerID > 0)//Add=Masj,Date=2013-08-26��ȥ����Ѷ������������
                {
                    BLL.Loger.Log4Net.Info("���û������ȶ���ID��" + YPOrderID + "�ļ�¼������Ϊ" + dealerID + "����Ѷ�����������������");
                }
                else//������
                {
                    dtNew.ImportRow(dt.Rows[i]);
                }
            }
            BLL.Loger.Log4Net.Info("����õ����û�������������" + existCount + "�������ں������Ŀ����Ѿ�������");

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("OrderRelpaceCarLogTransaction");
            try
            {
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    Entities.OrderRelpaceCarLog model = InitOrderRelpaceCarLog(dtNew.Rows[i]);
                    if (model != null)
                    {
                        long OrderRelpaceCarLogID = Insert(tran, model); //����OrderRelpaceCarLog��

                        if (model.CarID != Constant.INT_INVALID_VALUE && model.CarID != 0)
                        {
                            //Add By Chybin At 2013.2.13  ���Carid��OrderQuantityΪ�գ���ֻ����OrderRelpaceCarLog �����������񣬲�����OrderRelpaceCar ��
                            //Add By Masj At 2013.7.26 OrderQuantityΪ��ȥ��������
                            long taskID = BLL.OrderTask.Instance.InsertByOrder(tran, model);
                            if (BLL.OrderRelpaceCar.Instance.IsExistsByTaskID(taskID))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "���û�������ID" + taskID + "�ļ�¼�Ѿ����ڣ�������������");
                            }
                            else if (BLL.OrderRelpaceCar.Instance.InsertByTaskID(tran, taskID, model))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "���û�������ID" + taskID + "�ļ�¼���ɳɹ���"); taskCount++;
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
                string msg = "���û�����������ʧ��!ʧ��ԭ��" + ex.Message;
                BLL.Util.InsertUserLogNoUser(msg);
                BLL.Loger.Log4Net.Error(msg, ex);
            }
            finally
            {
                connection.Close();
            }
            string msgTitle = "һ�����ɣ��û�������" + taskCount + "��";
            BLL.Util.InsertUserLogNoUser(msgTitle);
            BLL.Loger.Log4Net.Error(msgTitle);
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Now - dtNow;
            BLL.Loger.Log4Net.Info("����_����_�û�����_����:" + msgTitle + ",��ʱ" + ts.TotalSeconds + "�롣");
        }

        /// <summary>
        /// ��ʼ���û�������־��Ϣ
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Entities.OrderRelpaceCarLog InitOrderRelpaceCarLog(DataRow dr)
        {
            Entities.OrderRelpaceCarLog model = new Entities.OrderRelpaceCarLog();

            try
            {
                model.YPOrderID = int.Parse(dr["OrderBusinessOpportunityID"].ToString());
                model.UserName = dr["UserName"].ToString().Trim();
                model.UserPhone = dr["UserPhone"].ToString().Trim();
                model.UserMobile = dr["UserMobile"].ToString().Trim();
                model.UserMail = dr["UserMail"].ToString().Trim();
                int userGender = -1;
                if (int.TryParse(dr["UserGender"].ToString(), out userGender))
                {
                    model.UserGender = userGender;
                }
                else
                {
                    model.UserGender = null;
                }
                int locationID = -1;
                if (int.TryParse(dr["LocationID"].ToString(), out locationID))
                {
                    model.LocationID = locationID;
                }
                else
                {
                    model.LocationID = null;
                }
                model.LocationName = dr["LocationName"].ToString().Trim();
                model.UserAddress = dr["UserAddress"].ToString().Trim();
                model.OrderCreateTime = DateTime.Parse(dr["OrderBusinessOpportunityCreateTime"].ToString());
                decimal orderPrice = -1;
                if (decimal.TryParse(dr["OrderPrice"].ToString(), out orderPrice))
                {
                    model.OrderPrice = orderPrice;
                }
                else
                {
                    model.OrderPrice = null;
                }

                int orderQuantity = -1;
                if (int.TryParse(dr["OrderQuantity"].ToString(), out orderQuantity))
                {
                    model.OrderQuantity = orderQuantity;
                }
                else
                {
                    model.OrderQuantity = null;
                }
                model.OrderRemark = dr["OrderRemark"].ToString().Trim();
                int carID = -1;
                if (int.TryParse(dr["CarID"].ToString(), out carID))
                {
                    model.CarID = carID;
                }
                else
                {
                    model.CarID = null;
                }
                model.CarFullName = dr["CarFullName"].ToString().Trim();
                decimal carPrice = -1;
                if (decimal.TryParse(dr["CarPrice"].ToString(), out carPrice))
                {
                    model.CarPrice = carPrice;
                }
                else
                {
                    model.CarPrice = null;
                }
                model.CarColor = dr["CarColor"].ToString().Trim();
                model.CarPromotions = dr["CarPromotions"].ToString().Trim();

                int replacementCarId = -1;
                if (int.TryParse(dr["ReplacementCarId"].ToString(), out replacementCarId))
                {
                    model.ReplacementCarId = replacementCarId;
                }
                else
                {
                    model.ReplacementCarId = null;
                }
                int replacementCarBuyYear = -1;
                if (int.TryParse(dr["ReplacementCarBuyYear"].ToString(), out replacementCarBuyYear))
                {
                    model.ReplacementCarBuyYear = replacementCarBuyYear;
                }
                else
                {
                    model.ReplacementCarBuyYear = null;
                }
                int replacementCarBuyMonth = -1;
                if (int.TryParse(dr["ReplacementCarBuyMonth"].ToString(), out replacementCarBuyMonth))
                {
                    model.ReplacementCarBuyMonth = replacementCarBuyMonth;
                }
                else
                {
                    model.ReplacementCarBuyMonth = null;
                }
                model.ReplacementCarColor = dr["ReplacementCarColor"].ToString().Trim();
                decimal replacementCarUsedMiles = -1;
                if (decimal.TryParse(dr["ReplacementCarUsedMiles"].ToString(), out replacementCarUsedMiles))
                {
                    model.ReplacementCarUsedMiles = replacementCarUsedMiles;
                }
                else
                {
                    model.ReplacementCarUsedMiles = null;
                }
                decimal saleprice = 0;
                if (decimal.TryParse(dr["SalePrice"].ToString(), out saleprice))
                {
                    model.SalePrice = saleprice;
                }
                else
                {
                    model.SalePrice = null;
                }
                int replacementCarLocationID = -1;
                if (int.TryParse(dr["ReplacementCarLocationID"].ToString(), out replacementCarLocationID))
                {
                    model.ReplacementCarLocationID = replacementCarLocationID;
                }
                else
                {
                    model.ReplacementCarLocationID = null;
                }
                int dealerID = -1;
                if (int.TryParse(dr["DealerID"].ToString(), out dealerID))
                {
                    model.DealerID = dealerID;
                }
                else
                {
                    model.DealerID = null;
                }
                BLL.Loger.Log4Net.Info(string.Format("��ʼ���û���������{0}���ݣ������۸�:{1},�����۸�:{2},��ʻ���:{3},���ۼ۸�:{4},������ID:{5}",
                    model.YPOrderID, model.OrderPrice, model.CarPrice, model.ReplacementCarUsedMiles, model.SalePrice, dealerID));
            }
            catch (Exception ex)
            {
                BLL.Util.InsertUserLogNoUser("��ʼ���û�������Ϣʧ��!ʧ��ԭ��" + ex.Message);
                return null;
            }
            return model;
        }
    }
}

