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
using System.Web;
using System.Collections;
using System.Web.Caching;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 调用车型接口相关数据
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-20 10:30:07 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CarTypeAPI
    {
        #region Instance
        public static readonly CarTypeAPI Instance = new CarTypeAPI();
        private string MasterToBrandToSerialXML = ConfigurationUtil.GetAppSettingValue("MasterToBrandToSerialXML");//主品牌to子品牌XML
        private string CarTypeXML = ConfigurationUtil.GetAppSettingValue("CarTypeXML");//根据车款ID返回车款xml信息
        private string CarColorXML = ConfigurationUtil.GetAppSettingValue("CarColorXML");//根据车型ID，返回车身颜色信息
        DateTime absoluteExpiration = DateTime.Now.AddDays(int.Parse(ConfigurationUtil.GetAppSettingValue("CarTypeCacheDays")));//缓存天数
        #endregion

        #region Contructor
        protected CarTypeAPI()
        {

        }
        #endregion


        /// <summary>
        /// 根据车型ID，返回车身颜色信息
        /// </summary>
        /// <param name="carTypeID">车型ID</param>
        /// <returns></returns>
        public DataTable GetCarColorByCarTypeID(int carTypeID)
        {
            DataTable dtCarColor = null;
            try
            {
                DataSet carColor = (DataSet)HttpRuntime.Cache.Get("CarColor");
                if (carColor == null || (carColor != null && carColor.Tables[0].Rows.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(CarColorXML);
                    HttpRuntime.Cache.Insert("CarColor", ds, null, absoluteExpiration, TimeSpan.Zero);
                    carColor = (DataSet)HttpRuntime.Cache.Get("CarColor");
                }
                carColor.Tables["Color"].DefaultView.RowFilter = "Car_ID=" + carTypeID;
                DataTable dt = carColor.Tables["Color"].DefaultView.ToTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtCarColor = dt.Copy();
                }
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message);
            }
            return dtCarColor;
        }

        /// <summary>
        /// 根据子品牌ID，返回子品牌名称
        /// </summary>
        /// <param name="serialID">子品牌ID</param>
        /// <returns>返回子品牌名称</returns>
        public string GetSerialNameBySerialID(int serialID)
        {
            string name = string.Empty;
            try
            {
                DataSet dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                if (dsSerial == null || (dsSerial != null && dsSerial.Tables.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(MasterToBrandToSerialXML);
                    HttpRuntime.Cache.Insert("CarMasterAndSerial", ds, null, absoluteExpiration, TimeSpan.Zero);
                    dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                }
                dsSerial.Tables["Serial"].DefaultView.RowFilter = "ID=" + serialID;
                DataTable dt = dsSerial.Tables["Serial"].DefaultView.ToTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["Name"].ToString().Trim();
                }
            }
            catch (Exception e)
            {

            }
            return name;
        }

        public int GetMasterBrandIDBySerialID(int serialID)
        {
            int masterbrandID = 0;

            try
            {
                int brandID = 0;
                DataSet dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                if (dsSerial == null || (dsSerial != null && dsSerial.Tables.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(MasterToBrandToSerialXML);
                    HttpRuntime.Cache.Insert("CarMasterAndSerial", ds, null, absoluteExpiration, TimeSpan.Zero);
                    dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                }
                dsSerial.Tables["Serial"].DefaultView.RowFilter = "ID=" + serialID;
                DataTable dt = dsSerial.Tables["Serial"].DefaultView.ToTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    brandID = int.Parse(dt.Rows[0]["Brand_ID"].ToString().Trim());

                    dsSerial.Tables["Brand"].DefaultView.RowFilter = "Brand_ID=" + brandID;
                    DataTable branddt = dsSerial.Tables["Brand"].DefaultView.ToTable();
                    if (branddt != null && branddt.Rows.Count > 0)
                    {
                        masterbrandID = int.Parse(branddt.Rows[0]["MasterBrand_ID"].ToString().Trim());

                        dsSerial.Tables["MasterBrand"].DefaultView.RowFilter = "MasterBrand_ID=" + masterbrandID;
                        DataTable masterbranddt = dsSerial.Tables["MasterBrand"].DefaultView.ToTable();
                        if (masterbranddt != null && masterbranddt.Rows.Count > 0)
                        {
                            masterbrandID = int.Parse(masterbranddt.Rows[0]["ID"].ToString().Trim());
                        }
                    }

                }
            }
            catch (Exception e)
            {

            }

            return masterbrandID;
        }

        public int GetMasterBrandIDByBrandID(int brandID)
        {
            int MasterbrandID = 0;

            try
            {
                DataSet dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                if (dsSerial == null || (dsSerial != null && dsSerial.Tables.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(MasterToBrandToSerialXML);
                    HttpRuntime.Cache.Insert("CarMasterAndSerial", ds, null, absoluteExpiration, TimeSpan.Zero);
                    dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                }
                dsSerial.Tables["Brand"].DefaultView.RowFilter = "ID=" + brandID;
                DataTable dt = dsSerial.Tables["Brand"].DefaultView.ToTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    MasterbrandID = int.Parse(dt.Rows[0]["MasterBrand_ID"].ToString().Trim());
                }
            }
            catch (Exception e)
            {

            }

            return MasterbrandID;
        }

        /// <summary>
        /// 根据主品牌ID，返回主品牌名称
        /// </summary>
        /// <param name="masterBrandID">主品牌ID</param>
        /// <returns>返回主品牌名称</returns>
        public string GetMasterBrandNameByMasterBrandID(int masterBrandID)
        {
            string name = string.Empty;

            try
            {
                DataSet dsMaster = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                if (dsMaster == null || (dsMaster != null && dsMaster.Tables.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(MasterToBrandToSerialXML);
                    HttpRuntime.Cache.Insert("CarMasterAndSerial", ds, null, absoluteExpiration, TimeSpan.Zero);
                    dsMaster = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                }
                dsMaster.Tables["MasterBrand"].DefaultView.RowFilter = "ID=" + masterBrandID;
                DataTable dt = dsMaster.Tables["MasterBrand"].DefaultView.ToTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["Name"].ToString().Trim();
                }
            }
            catch (Exception)
            {

            }
            return name;
        }

        /// <summary>
        /// 根据车款ID，返回车款名称
        /// </summary>
        /// <param name="carTypeID">车款ID</param>
        /// <returns>返回车款名称</returns>
        public string GetCarTypeNameByCarTypeID(int carTypeID)
        {
            string name = string.Empty;

            try
            {
                DataSet dsCarType = (DataSet)HttpRuntime.Cache.Get("CarType_" + carTypeID);
                if (dsCarType == null || (dsCarType != null && dsCarType.Tables.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(CarTypeXML + "&id=" + carTypeID.ToString());
                    HttpRuntime.Cache.Insert("CarType_" + carTypeID, ds, null, absoluteExpiration, TimeSpan.Zero);
                    dsCarType = (DataSet)HttpRuntime.Cache.Get("CarType_" + carTypeID);
                }
                if (dsCarType.Tables["car"] != null && dsCarType.Tables["car"].Rows.Count > 0)
                {
                    name = dsCarType.Tables["car"].Rows[0]["CarName"].ToString().Trim();
                    BLL.Loger.Log4Net.Info("通过XML接口取车款名称为：" + name + "[carid:" + carTypeID + "]");
                }
                else
                {
                    BLL.Loger.Log4Net.Info("通过XML接口取车款，[carid:" + carTypeID + "]缓存名【CarType_" + carTypeID + "】中数据为空");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("通过XML接口取车款出错[carid:" + carTypeID + "]：" + ex.StackTrace);
            }
            return name;
        }

        /// <summary>
        /// 根据车款ID，返回车辆指导价格
        /// </summary>
        /// <param name="carTypeID">车款ID</param>
        /// <returns>返回车辆指导价格</returns>
        public decimal GetCarReferPriceByCarTypeID(int carTypeID)
        {
            decimal carReferPrice = 0;

            try
            {
                DataSet dsCarType = (DataSet)HttpRuntime.Cache.Get("CarType_" + carTypeID);
                if (dsCarType == null || (dsCarType != null && dsCarType.Tables.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(CarTypeXML + "&id=" + carTypeID.ToString());
                    HttpRuntime.Cache.Insert("CarType_" + carTypeID, ds, null, absoluteExpiration, TimeSpan.Zero);
                    dsCarType = (DataSet)HttpRuntime.Cache.Get("CarType_" + carTypeID);
                }
                if (dsCarType.Tables["car"] != null && dsCarType.Tables["car"].Rows.Count > 0)
                {
                    carReferPrice = decimal.Parse(dsCarType.Tables["car"].Rows[0]["ReferPrice"].ToString().Trim());
                }
            }
            catch (Exception)
            {

            }
            return carReferPrice;
        }

        /// <summary>
        /// 根据车款ID，返回主品牌ID，子品牌ID
        /// </summary>
        /// <param name="carTypeID">车款ID</param>
        /// <returns>返回车款名称</returns>
        public void GetSerialIDAndMasterBrandIDByCarTypeID(int carTypeID, out int serialID, out int masterBrandID, out int brandID)
        {
            brandID = 0;
            serialID = 0;
            masterBrandID = 0;
            try
            {
                DataSet dsCarType = (DataSet)HttpRuntime.Cache.Get("CarType_" + carTypeID);
                if (dsCarType == null || (dsCarType != null && dsCarType.Tables.Count == 0))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(CarTypeXML + "&id=" + carTypeID.ToString());
                    HttpRuntime.Cache.Insert("CarType_" + carTypeID, ds, null, absoluteExpiration, TimeSpan.Zero);
                    dsCarType = (DataSet)HttpRuntime.Cache.Get("CarType_" + carTypeID);
                }
                if (dsCarType.Tables["car"] != null && dsCarType.Tables["car"].Rows.Count > 0)
                {
                    serialID = int.Parse(dsCarType.Tables["car"].Rows[0]["SerialID"].ToString().Trim());

                    DataSet dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                    if (dsSerial == null || (dsSerial != null && dsSerial.Tables.Count == 0))
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(MasterToBrandToSerialXML);
                        HttpRuntime.Cache.Insert("CarMasterAndSerial", ds, null, absoluteExpiration, TimeSpan.Zero);
                        dsSerial = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                    }
                    dsSerial.Tables["Serial"].DefaultView.RowFilter = "ID=" + serialID.ToString();
                    DataTable dt = dsSerial.Tables["Serial"].DefaultView.ToTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        brandID = int.Parse(dt.Rows[0]["Brand_ID"].ToString().Trim());

                        DataSet dsMaster = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                        if (dsMaster == null || (dsMaster != null && dsMaster.Tables.Count == 0))
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(MasterToBrandToSerialXML);
                            HttpRuntime.Cache.Insert("CarMasterAndSerial", ds, null, absoluteExpiration, TimeSpan.Zero);
                            dsMaster = (DataSet)HttpRuntime.Cache.Get("CarMasterAndSerial");
                        }
                        dsMaster.Tables["Brand"].DefaultView.RowFilter = "Brand_ID=" + brandID;
                        DataTable dtCarMasterAndSerial = dsMaster.Tables["Brand"].DefaultView.ToTable();
                        if (dtCarMasterAndSerial != null && dtCarMasterAndSerial.Rows.Count > 0)
                        {
                            masterBrandID = int.Parse(dtCarMasterAndSerial.Rows[0]["MasterBrand_ID"].ToString().Trim());

                            dsMaster.Tables["MasterBrand"].DefaultView.RowFilter = "MasterBrand_ID=" + masterBrandID;
                            DataTable dtCarMaster = dsMaster.Tables["MasterBrand"].DefaultView.ToTable();
                            if (dtCarMaster != null && dtCarMaster.Rows.Count > 0)
                            {
                                masterBrandID = int.Parse(dtCarMaster.Rows[0]["ID"].ToString().Trim());
                            }
                        }
                    }
                }
                BLL.Loger.Log4Net.Info(string.Format("根据车款ID：{0},找到子品牌ID：{1}、主品牌ID：{2}", carTypeID, serialID, masterBrandID));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 清空表CarMasterBrand和CarSerial数据
        /// </summary>
        private void DeleteSyncCarTable()
        {
            BLL.CarMasterBrand.Instance.DeleteTable();
            BLL.CarBrand.Instance.DeleteTable();
            BLL.CarSerial.Instance.DeleteTable();
        }

        /// <summary>
        /// 同步车型（主品牌、系列）数据到表CarMasterBrand和CarSerial中
        /// </summary>
        public void SyncCarInfo()
        {
            DeleteSyncCarTable();

            DataSet ds = new DataSet();
            ds.ReadXml(MasterToBrandToSerialXML);
            if (ds.Tables.Contains("MasterBrand") && ds.Tables["MasterBrand"].Rows.Count > 0)//查询MasterBrand信息
            {
                //string msg = "同步车型（主品牌、系列）数据到表CarMasterBrand和CarSerial中——获取接口中表MasterBrand不存在或没有数据";
                //BLL.Loger.Log4Net.Info(msg);
                //throw new Exception(msg);
                foreach (DataRow dr in ds.Tables["MasterBrand"].Rows)
                {
                    Entities.CarMasterBrand cm = new Entities.CarMasterBrand();
                    cm.MasterBrandID = int.Parse(dr["ID"].ToString());
                    cm.Name = dr["Name"].ToString().Trim();
                    cm.EName = dr["EName"].ToString().Trim();
                    cm.Country = dr["Country"].ToString().Trim();
                    cm.AllSpell = dr["AllSpell"].ToString().Trim();
                    cm.Spell = dr["Spell"].ToString().Trim();
                    cm.CreateTime = DateTime.Now;
                    BLL.CarMasterBrand.Instance.Insert(cm);
                }

                //品牌
                if (ds.Tables.Contains("Brand") && ds.Tables["Brand"].Rows.Count > 0)//查询Brand信息
                {
                    foreach (DataRow drBrand in ds.Tables["Brand"].Rows)
                    {
                        int BrandId = int.Parse(drBrand["ID"].ToString().Trim());
                        string Name = drBrand["Name"].ToString();
                        string Country = drBrand["Country"].ToString();
                        string NewCountry = drBrand["NewCountry"].ToString();
                        string AllSpell = drBrand["AllSpell"].ToString();
                        string Spell = drBrand["Spell"].ToString();
                        string BrandSEOName = drBrand["BrandSEOName"].ToString();

                        int MasterBrand_Id = int.Parse(drBrand["MasterBrand_Id"].ToString().Trim());
                        if (ds.Tables.Contains("MasterBrand") && ds.Tables["MasterBrand"].Rows.Count > 0)
                        {
                            ds.Tables["MasterBrand"].DefaultView.RowFilter = "MasterBrand_ID=" + MasterBrand_Id;
                            DataTable dtCarMaster = ds.Tables["MasterBrand"].DefaultView.ToTable();
                            if (dtCarMaster != null && dtCarMaster.Rows.Count > 0)
                            {
                                MasterBrand_Id = int.Parse(dtCarMaster.Rows[0]["ID"].ToString());
                            }
                        }

                        Entities.CarBrand CarBrandModel = new Entities.CarBrand()
                        {
                            BrandID = BrandId,
                            Name = Name,
                            Country = Country,
                            NewCountry = NewCountry,
                            AllSpell = AllSpell,
                            Spell = Spell,
                            BrandSEOName = BrandSEOName,
                            MasterBrandID = MasterBrand_Id,
                            CreateTime = DateTime.Now,
                            ModifyTime = DateTime.Now
                        };

                        BLL.CarBrand.Instance.Insert(CarBrandModel);
                    }
                }

                //车型
                if (ds.Tables.Contains("Serial") && ds.Tables["Serial"].Rows.Count > 0)//查询Serial信息
                {
                    foreach (DataRow drSerial in ds.Tables["Serial"].Rows)
                    {
                        int brandID = int.Parse(drSerial["Brand_ID"].ToString().Trim());
                        int csMasterBrandID = -1;
                        if (ds.Tables.Contains("Brand") && ds.Tables["Brand"].Rows.Count > 0)
                        {
                            ds.Tables["Brand"].DefaultView.RowFilter = "Brand_ID=" + brandID;
                            DataTable dtCarMasterAndSerial = ds.Tables["Brand"].DefaultView.ToTable();
                            if (dtCarMasterAndSerial != null && dtCarMasterAndSerial.Rows.Count > 0)
                            {
                                brandID = int.Parse(dtCarMasterAndSerial.Rows[0]["ID"].ToString());
                                ds.Tables["MasterBrand"].DefaultView.RowFilter = "MasterBrand_ID=" + int.Parse(dtCarMasterAndSerial.Rows[0]["MasterBrand_ID"].ToString().Trim());
                                DataTable dtCarMaster = ds.Tables["MasterBrand"].DefaultView.ToTable();
                                if (dtCarMaster != null && dtCarMaster.Rows.Count > 0)
                                {
                                    csMasterBrandID = int.Parse(dtCarMaster.Rows[0]["ID"].ToString().Trim());
                                }
                            }
                        }
                        if (csMasterBrandID > 0)
                        {
                            Entities.CarSerial csm = new Entities.CarSerial();
                            csm.CSID = int.Parse(drSerial["ID"].ToString());
                            csm.Name = drSerial["Name"].ToString().Trim();
                            csm.OldCbID = drSerial["OldCbID"].ToString();
                            csm.CsLevel = drSerial["CsLevel"].ToString().Trim();
                            csm.MultiPriceRange = drSerial["MultiPriceRange"].ToString().Trim();
                            csm.CsSaleState = drSerial["CsSaleState"].ToString().Trim();
                            csm.AllSpell = drSerial["AllSpell"].ToString().Trim();
                            csm.CsMultiChar = drSerial["CsMultiChar"].ToString().Trim();
                            csm.MasterBrandID = csMasterBrandID;
                            csm.CreateTime = DateTime.Now;
                            csm.BrandID = brandID;

                            BLL.CarSerial.Instance.Insert(csm);
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info("同步车型（主品牌、系列）数据到表CarMasterBrand和CarSerial中——同步到表CarSerial时，根据BrandID:" + brandID + "没有找到MasterBrandID");
                        }
                    }
                }

            }
            else
            {
                string msg = "同步车型（主品牌、系列）数据到表CarMasterBrand和CarSerial中——获取接口中表MasterBrand不存在或没有数据";
                BLL.Loger.Log4Net.Info(msg);
                throw new Exception(msg);
            }
        }




    }
}
