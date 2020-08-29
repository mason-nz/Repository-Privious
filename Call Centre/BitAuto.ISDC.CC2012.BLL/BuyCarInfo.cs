using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Web;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���BuyCarInfo ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:07 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class BuyCarInfo
    {
        #region Instance
        public static readonly BuyCarInfo Instance = new BuyCarInfo();
        #endregion

        #region Contructor
        protected BuyCarInfo()
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
        public DataTable GetBuyCarInfo(QueryBuyCarInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.BuyCarInfo.Instance.GetBuyCarInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.BuyCarInfo.Instance.GetBuyCarInfo(new QueryBuyCarInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.BuyCarInfo GetBuyCarInfo(string CustID)
        {

            return Dal.BuyCarInfo.Instance.GetBuyCarInfo(CustID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryBuyCarInfo query = new QueryBuyCarInfo();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetBuyCarInfo(query, string.Empty, 1, 1, out count);
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
        /// �Ƿ���ڸü�¼,���ݿͻ�id
        /// </summary>
        public bool IsExistsByCustID(string CustID)
        {
            QueryBuyCarInfo query = new QueryBuyCarInfo();
            query.CustID = CustID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetBuyCarInfo(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.BuyCarInfo model)
        {
            return Dal.BuyCarInfo.Instance.Insert(model);
        }

        public int Insert(SqlTransaction tran, Entities.BuyCarInfo model)
        {
            return Dal.BuyCarInfo.Instance.Insert(tran, model);
        }
        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.BuyCarInfo model)
        {
            return Dal.BuyCarInfo.Instance.Update(model);
        }

        public int Update(SqlTransaction sqlTran, Entities.BuyCarInfo model)
        {
            return Dal.BuyCarInfo.Instance.Update(sqlTran, model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(string CustID)
        {

            return Dal.BuyCarInfo.Instance.Delete(CustID);
        }

        #endregion

        /// <summary>
        /// ȡ��������Ʒ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLCarBrand()
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache["CarBrandCache"] != null)
            {
                return (DataTable)objCache["CarBrandCache"];
            }
            else
            {
                DataTable dt = Dal.BuyCarInfo.Instance.GetALLCarBrand();

                #region ������ƴ��

                if (dt.Columns.Contains("name"))
                {
                    string allPin = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        allPin = BLL.Util.ConvertToPinYinAll(dr["name"].ToString());
                        if (allPin.Length > 0)
                        {
                            allPin = allPin.ToUpper();
                            allPin = allPin.Substring(0, 1);
                            dr["name"] = allPin + "_" + dr["name"];
                        }
                    }
                }

                #endregion

                #region ����

                dt.DefaultView.Sort = "name ASC";
                DataTable dtTemp = dt.DefaultView.ToTable();

                #endregion

                objCache.Insert("CarBrandCache", dtTemp);
                return dtTemp;
            }
        }

        /// <summary>
        /// ȡ������������
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLCarSerial()
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache["ALLCarSerialCache"] != null)
            {
                return (DataTable)objCache["ALLCarSerialCache"];
            }
            else
            {
                DataTable dt = Dal.BuyCarInfo.Instance.GetALLCarSerial();

                #region ������ƴ��

                if (dt.Columns.Contains("name"))
                {
                    string allPin = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        allPin = BLL.Util.ConvertToPinYinAll(dr["name"].ToString());
                        if (allPin.Length > 0)
                        {
                            allPin = allPin.ToUpper();
                            allPin = allPin.Substring(0, 1);
                            dr["name"] = allPin + "_" + dr["name"];
                        }
                    }
                }

                #endregion

                #region ����

                dt.DefaultView.Sort = "name ASC";
                DataTable dtTemp = dt.DefaultView.ToTable();

                #endregion

                objCache.Insert("ALLCarSerialCache", dtTemp);
                return dtTemp;
            }
        }

        /// <summary>
        /// ȡ������������
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLCarBrandAndSerial(string name, string order, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;

            DataTable dt = Dal.BuyCarInfo.Instance.GetALLCarBrandAndSerial(name, order, currentPage, pageSize, out totalCount);

            return dt;

        }

        /// <summary>
        /// ����Ʒ��id��ȡ��ϵ
        /// </summary>
        /// <param name="brandid"></param>
        /// <returns></returns>
        public DataTable GetCarSerial(int brandid)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache["CarSerialCache"] != null)
            {
                return (DataTable)objCache["CarSerialCache"];
            }
            else
            {
                DataTable dt = Dal.BuyCarInfo.Instance.GetCarSerial(brandid);

                #region ������ƴ��

                if (dt.Columns.Contains("name"))
                {
                    string allPin = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        allPin = BLL.Util.ConvertToPinYinAll(dr["name"].ToString());
                        if (allPin.Length > 0)
                        {
                            allPin = allPin.ToUpper();
                            allPin = allPin.Substring(0, 1);
                            dr["name"] = allPin + "_" + dr["name"];
                        }
                    }
                }

                #endregion

                #region ����

                dt.DefaultView.Sort = "name ASC";
                DataTable dtTemp = dt.DefaultView.ToTable();

                #endregion

                objCache.Insert("CarSerialCache", dtTemp);
                return dtTemp;
            }


        }

        /// <summary>
        /// ���ݳ�ϵ��ȡ��ϵID
        /// </summary>
        /// <param name="brandid"></param>
        /// <returns></returns>
        public DataTable GetCarSerialByName(string CarSerialName)
        {
            return Dal.BuyCarInfo.Instance.GetCarSerialByName(CarSerialName);
        }

        /// <summary>
        /// ����Ʒ������ȡ����Ʒ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarBrandByName(string name)
        {
            DataTable dt = Dal.BuyCarInfo.Instance.GetCarBrandByName(name);
            return dt;
        }


        public DataTable GetCarBrandName(int brandid)
        {
            return Dal.BuyCarInfo.Instance.GetCarBrandName(brandid);
        }
        public DataTable GetCarSerialName(int serialid)
        {
            return Dal.BuyCarInfo.Instance.GetCarSerialName(serialid);
        }

        public DataTable GetInfo(string name, string order, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;
            DataTable dt = GetALLCarBrandAndSerial(name, order, currentPage, pageSize, out totalCount);

            DataRow[] rows = dt.Select("bname like '%" + name + "%' or sname like '%" + name + "%'");
            if (rows.Length > 0)
            {
                return rows[0].Table;

            }
            else
            {
                return null;
            }

        }
    }
}

