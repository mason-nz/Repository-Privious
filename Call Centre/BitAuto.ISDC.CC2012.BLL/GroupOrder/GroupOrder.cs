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
    /// ҵ���߼���GroupOrder ��ժҪ˵����
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
    public class GroupOrder
    {
        #region Instance
        public static readonly GroupOrder Instance = new GroupOrder();
        #endregion

        #region Contructor
        protected GroupOrder()
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
        public DataTable GetGroupOrder(QueryGroupOrder query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.GroupOrder.Instance.GetGroupOrder(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetGroupOrder(string wheresql)
        {

            return Dal.GroupOrder.Instance.GetGroupOrder(wheresql);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.GroupOrder.Instance.GetGroupOrder(new QueryGroupOrder(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.GroupOrder GetGroupOrder(long TaskID)
        {

            return Dal.GroupOrder.Instance.GetGroupOrder(TaskID);
        }

        public List<Entities.GroupOrder> GetGroupOrderList(Entities.QueryGroupOrder query)
        {
            return Dal.GroupOrder.Instance.GetGroupOrderList(query);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByTaskID(long TaskID)
        {
            QueryGroupOrder query = new QueryGroupOrder();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrder(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.GroupOrder model)
        {
            Dal.GroupOrder.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.GroupOrder model)
        {
            return Dal.GroupOrder.Instance.Insert(sqltran, model);
        }

        /// <summary>
        /// ���������Ź�����ʵ����Ϣ�������GroupOrder��Ϣ
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="taskID">����ID</param>
        /// <param name="model">�����Ź�����ʵ����Ϣ</param>
        /// <returns></returns>
        public bool InsertByTaskID(SqlTransaction tran, long taskID, Entities.GroupOrderOrg model)
        {
            Entities.GroupOrder onModel = new Entities.GroupOrder();
            onModel.TaskID = taskID;
            onModel.OrderID = model.OrderID;
            onModel.OrderCode = model.OrderCode;
            onModel.CustomerName = model.CustomerName.Trim();
            onModel.CustomerTel = model.CustomerTel.Trim();
            onModel.ProvinceID = model.ProvinceID;
            onModel.ProvinceName = model.ProvinceName;
            onModel.CityID = model.CityID;
            onModel.CityName = model.CityName;

            //�޸Ĵ��� ǿ� 2014-12-17
            BitAuto.YanFa.Crm2009.Entities.AreaInfo info = Util.GetAreaInfoByPCC(
                   CommonFunction.ObjectToString(model.ProvinceID),
                   CommonFunction.ObjectToString(model.CityID),
                   null);
            onModel.AreaID = info == null ? "" : info.District;
            
            onModel.OrderCreateTime = model.OrderCreateTime;
            onModel.CarMasterID = model.CarMasterID;
            onModel.CarMasterName = model.CarMasterName;
            onModel.CarSerialID = model.CarSerialID;
            onModel.CarSerialName = model.CarSerialName;
            onModel.CarID = model.CarID;
            onModel.CarName = model.CarName;
            onModel.DealerID = model.DealerID;
            onModel.DealerName = model.DealerName;
            onModel.OrderPrice = model.Price;
            onModel.CreateTime = DateTime.Now;
            onModel.CreateUserID = null;
            onModel.LastUpdateTime = null;
            onModel.LastUpdateUserID = null;
            onModel.UserName = null;


            long recid = Insert(tran, onModel);
            if (recid > 0)
            {
                BLL.Loger.Log4Net.Info(string.Format("�Ź��������ɱ�GroupOrder�ɹ�������IDΪ��{0},��������IDΪ��{1},����ID��{2},��Ʒ��ID��{3}����Ʒ��IDΪ��{4}",
                    recid, onModel.OrderID,
                    onModel.CarID == null ? 0 : onModel.CarID.Value,
                    onModel.CarSerialID == null ? 0 : onModel.CarSerialID.Value,
                    onModel.CarMasterID == null ? 0 : onModel.CarMasterID.Value
                    ));
                return true;
            }
            return false;
        }
        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.GroupOrder model)
        {
            return Dal.GroupOrder.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.GroupOrder model)
        {
            return Dal.GroupOrder.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long TaskID)
        {

            return Dal.GroupOrder.Instance.Delete(TaskID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {

            return Dal.GroupOrder.Instance.Delete(sqltran, TaskID);
        }

        #endregion

        #region ��ȡ�����鿴ҳ�ͻ�������Ϣ
        /// <summary>
        /// ��ȡ�����鿴ҳ�ͻ�������Ϣ
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public Entities.GroupOrder GetGroupOrderCustInfo(long TaskID)
        {
            return Dal.GroupOrder.Instance.GetGroupOrderCustInfo(TaskID);
        }
        #endregion
    }
}

