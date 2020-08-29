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
    /// ҵ���߼���OrderNewCarLog ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:31 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderNewCarLog
    {
        #region Instance
        public static readonly OrderNewCarLog Instance = new OrderNewCarLog();
        #endregion

        #region Contructor
        protected OrderNewCarLog()
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
        public DataTable GetOrderNewCarLog(QueryOrderNewCarLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderNewCarLog.Instance.GetOrderNewCarLog(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OrderNewCarLog.Instance.GetOrderNewCarLog(new QueryOrderNewCarLog(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ��ȡ��������³�����ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxYPOrderID()
        {
            return Dal.OrderNewCarLog.Instance.GetMaxYPOrderID(0);
        }

        /// <summary>
        ///  ��ȡ��������Լݶ���ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxTestDriveYPOrderID()
        {
            return Dal.OrderNewCarLog.Instance.GetMaxYPOrderID(1);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.OrderNewCarLog GetOrderNewCarLog(long RecID)
        {
            return Dal.OrderNewCarLog.Instance.GetOrderNewCarLog(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryOrderNewCarLog query = new QueryOrderNewCarLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderNewCarLog(query, string.Empty, 1, 1, out count);
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
            return Dal.OrderNewCarLog.Instance.IsExistsByYPOrderID(YPOrderID);
        }
        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.OrderNewCarLog.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.OrderNewCarLog.Instance.Delete(sqltran, RecID);
        }

        #endregion

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="orderType">�������ͣ�0-�³�����[Ĭ��]��1-�Լݶ�����</param>
        public void GenTask(DataTable dt, int orderType)
        {
            int taskCount = 0;
            int existCount = 0;
            BLL.Loger.Log4Net.Info("����_����_" + (orderType == 0 ? "�³�" : "�Լ�") + "����_��ʼ:");
            DateTime dtNow = DateTime.Now;
            DataTable dtNew = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int YPOrderID = int.Parse(dt.Rows[i]["OrderBusinessOpportunityID"].ToString());
                int? DealerID = (int?)GetColumnDefaultValue(dt.Rows[i], "DealerID", typeof(int));
                if (BLL.OrderNewCarLog.Instance.IsExistsByYPOrderID(YPOrderID))//����
                {
                    BLL.Util.InsertUserLogNoUser("��" + (orderType == 0 ? "�³�" : "�Լ�") + "�����ȶ���ID��" + YPOrderID + "�ļ�¼�Ѿ����ڣ�������������"); existCount++;
                }
                else if (DealerID != null && DealerID.Value > 0)//Add=Masj,Date=2013-08-26��ȥ����Ѷ������������
                {
                    //BLL.Util.InsertUserLogNoUser("��" + (orderType == 0 ? "�³�" : "�Լ�") + "�����ȶ���ID��" + YPOrderID + "�ļ�¼������Ϊ0����Ѷ�����������������");
                    BLL.Loger.Log4Net.Info("��" + (orderType == 0 ? "�³�" : "�Լ�") + "�����ȶ���ID��" + YPOrderID + "�ļ�¼������Ϊ" + DealerID + "����Ѷ�����������������");
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
            SqlTransaction tran = connection.BeginTransaction("OrderNewCarLogTransaction");
            try
            {
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    Entities.OrderNewCarLog model = InitOrderNewCarLog(dtNew.Rows[i], orderType);
                    if (model != null)
                    {
                        long OrderNewCarLogID = Insert(tran, model); //���� OrderNewCarLog ��
                        BLL.Loger.Log4Net.Info(string.Format((orderType == 0 ? "�³�" : "�Լ�") + "�������ɱ�OrderNewCarLog�ɹ�������IDΪ��{0},��������IDΪ��{1},����ID��{2}",
                                                  OrderNewCarLogID, model.YPOrderID, model.CarID));

                        if (model.CarID != Constant.INT_INVALID_VALUE && model.CarID != 0)
                        {
                            //Add By Chybin At 2013.2.13  ���Carid��OrderQuantityΪ�գ���ֻ����OrderNewCarLog �����������񣬲�����OrderNewCar ��
                            //Add By Masj At 2013.7.26 OrderQuantityΪ��ȥ��������
                            long taskID = BLL.OrderTask.Instance.InsertByOrder(tran, model); //���ݽӿڵõ���ԭʼ������������
                            if (BLL.OrderNewCar.Instance.IsExistsByTaskID(taskID))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "��" + (orderType == 0 ? "�³�" : "�Լ�") + "������ID" + taskID + "�ļ�¼�Ѿ����ڣ�������������");
                            }
                            else if (BLL.OrderNewCar.Instance.InsertByTaskID(tran, taskID, model))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "��" + (orderType == 0 ? "�³�" : "�Լ�") + "������ID" + taskID + "�ļ�¼���ɳɹ���"); taskCount++;
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
            string msgTitle = "һ�����ɣ��³�������" + taskCount + "��";
            BLL.Util.InsertUserLogNoUser(msgTitle);
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Now - dtNow;
            BLL.Loger.Log4Net.Info("����_����_�³�����_����:" + msgTitle + ",��ʱ" + ts.TotalSeconds + "�롣");
        }

        /// <summary>
        /// ��ʼ���³�������־��Ϣ
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordertype">�������ͣ�0-�³�����[Ĭ��]��1-�Լݶ�����</param>
        /// <returns></returns>
        private Entities.OrderNewCarLog InitOrderNewCarLog(DataRow dr, int orderType)
        {
            Entities.OrderNewCarLog model = new Entities.OrderNewCarLog();

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
                model.OrderPrice = (decimal?)GetColumnDefaultValue(dr, "OrderPrice", typeof(decimal));
                model.OrderQuantity = (int?)GetColumnDefaultValue(dr, "OrderQuantity", typeof(int));
                model.OrderRemark = (string)GetColumnDefaultValue(dr, "OrderRemark", typeof(string));
                model.CarID = (int?)GetColumnDefaultValue(dr, "CarID", typeof(int));
                model.CarFullName = (string)GetColumnDefaultValue(dr, "CarFullName", typeof(string));
                model.CarPrice = (decimal?)GetColumnDefaultValue(dr, "CarPrice", typeof(decimal));
                model.CarColor = (string)GetColumnDefaultValue(dr, "CarColor", typeof(string));
                model.CarPromotions = (string)GetColumnDefaultValue(dr, "CarPromotions", typeof(string));
                model.DealerID = (int?)GetColumnDefaultValue(dr, "DealerID", typeof(int));
                model.OrderType = orderType;
                //if (dr.Table.Columns.Contains("OrderPrice"))
                //{
                //    decimal orderPrice = -1;
                //    if (decimal.TryParse(dr["OrderPrice"].ToString(), out orderPrice))
                //    {
                //        model.OrderPrice = orderPrice;
                //    }
                //    else
                //    {
                //        model.OrderPrice = null;
                //    }
                //}
                //int orderQuantity = -1;
                //if (int.TryParse(dr["OrderQuantity"].ToString(), out orderQuantity))
                //{
                //    model.OrderQuantity = orderQuantity;
                //}
                //else
                //{
                //    model.OrderQuantity = null;
                //}

                //int carID = -1;
                //if (int.TryParse(dr["CarID"].ToString(), out carID))
                //{
                //    model.CarID = carID;
                //}
                //else
                //{
                //    model.CarID = null;
                //}

                //model.CarPrice = null;
                //if (dr.Table.Columns.Contains("CarPrice"))
                //{
                //    decimal carPrice = -1;
                //    if (decimal.TryParse(dr["CarPrice"].ToString(), out carPrice))
                //    {
                //        model.CarPrice = carPrice;
                //    }
                //    else
                //    {
                //        model.CarPrice = null;
                //    }
                //}


                //int dealerID = -1;
                //if (int.TryParse(dr["DealerID"].ToString(), out dealerID))
                //{
                //    model.DealerID = dealerID;
                //}
                //else
                //{
                //    model.DealerID = null;
                //}

            }
            catch (Exception ex)
            {
                BLL.Util.InsertUserLogNoUser("��ʼ���³�������Ϣʧ��!ʧ��ԭ��" + ex.Message);
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

