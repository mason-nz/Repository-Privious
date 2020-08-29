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
    /// 业务逻辑类BuyCarInfo 的摘要说明。
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
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetBuyCarInfo(QueryBuyCarInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.BuyCarInfo.Instance.GetBuyCarInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.BuyCarInfo.Instance.GetBuyCarInfo(new QueryBuyCarInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.BuyCarInfo GetBuyCarInfo(string CustID)
        {

            return Dal.BuyCarInfo.Instance.GetBuyCarInfo(CustID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
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
        /// 是否存在该记录,根据客户id
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
        /// 增加一条数据
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
        /// 更新一条数据
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
        /// 删除一条数据
        /// </summary>
        public int Delete(string CustID)
        {

            return Dal.BuyCarInfo.Instance.Delete(CustID);
        }

        #endregion

        /// <summary>
        /// 取所有汽车品牌
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

                #region 加首字拼音

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

                #region 排序

                dt.DefaultView.Sort = "name ASC";
                DataTable dtTemp = dt.DefaultView.ToTable();

                #endregion

                objCache.Insert("CarBrandCache", dtTemp);
                return dtTemp;
            }
        }

        /// <summary>
        /// 取所有汽车车型
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

                #region 加首字拼音

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

                #region 排序

                dt.DefaultView.Sort = "name ASC";
                DataTable dtTemp = dt.DefaultView.ToTable();

                #endregion

                objCache.Insert("ALLCarSerialCache", dtTemp);
                return dtTemp;
            }
        }

        /// <summary>
        /// 取所有汽车车型
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLCarBrandAndSerial(string name, string order, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;

            DataTable dt = Dal.BuyCarInfo.Instance.GetALLCarBrandAndSerial(name, order, currentPage, pageSize, out totalCount);

            return dt;

        }

        /// <summary>
        /// 根据品牌id，取车系
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

                #region 加首字拼音

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

                #region 排序

                dt.DefaultView.Sort = "name ASC";
                DataTable dtTemp = dt.DefaultView.ToTable();

                #endregion

                objCache.Insert("CarSerialCache", dtTemp);
                return dtTemp;
            }


        }

        /// <summary>
        /// 根据车系，取车系ID
        /// </summary>
        /// <param name="brandid"></param>
        /// <returns></returns>
        public DataTable GetCarSerialByName(string CarSerialName)
        {
            return Dal.BuyCarInfo.Instance.GetCarSerialByName(CarSerialName);
        }

        /// <summary>
        /// 根据品牌名称取汽车品牌
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

