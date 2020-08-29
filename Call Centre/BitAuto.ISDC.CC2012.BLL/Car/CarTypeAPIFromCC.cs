using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Caching;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CarTypeAPIFromCC
    {
        public static readonly CarTypeAPIFromCC Instance = new CarTypeAPIFromCC();

        #region 从缓存中取数据
        /// <summary>
        /// 取主品牌ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetMasterBrandIDByNameFormCache(string name)
        {
            int brandid = -1;
            DataTable brandDt = GetAllMasterBrandInfo();
            DataRow[] rows = brandDt.Select("Name='" + name.Trim() + "'");
            if (rows.Length > 0)
            {
                brandid = int.Parse(rows[0]["MasterBrandID"].ToString());
            }

            return brandid;
        }
        /// <summary>
        /// 取品牌ID,和对应的主品牌ID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="brandID"></param>
        /// <param name="masterBrandID"></param>
        public void GetBrandIDByNameFormCache(string name, out int brandID, out int masterBrandID)
        {
            brandID = -1;
            masterBrandID = -1;
            DataTable brandDt = GetAllBrandInfo();
            if (brandDt == null)
            {
                Loger.Log4Net.Info("GetAllBrandInfo 查询为空");
                return;
            }
            DataRow[] rows = brandDt.Select("Name='" + name.Trim() + "'");
            if (rows.Length > 0)
            {
                brandID = CommonFunction.ObjectToInteger(rows[0]["BrandID"], -1);
                masterBrandID = CommonFunction.ObjectToInteger(rows[0]["MasterBrandID"], -1);
            }
        }
        /// <summary>
        /// 根据品牌ID和名称查找车型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        public int GetSerilIDByNameFormCache(string name, int PID)
        {
            int serilid = -1;

            DataTable serialDt = GetAllCarSerialInfo();
            DataRow[] rows = serialDt.Select("Name='" + name.Trim() + "' AND BrandID=" + PID.ToString());
            if (rows.Length > 0)
            {
                serilid = int.Parse(rows[0]["CSID"].ToString());
            }
            else
            {
                //按照主品牌找
                rows = serialDt.Select("Name='" + name.Trim() + "' AND MasterBrandID=" + PID.ToString());
                if (rows.Length > 0)
                {
                    serilid = int.Parse(rows[0]["CSID"].ToString());
                }
            }

            return serilid;
        }
        public string GetSerialNameBySerialID(int serialID)
        {
            string name = "";
            DataTable serialDt = GetAllCarSerialInfo();

            serialDt.DefaultView.RowFilter = "CSID=" + serialID;
            DataTable dt = serialDt.DefaultView.ToTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["Name"].ToString().Trim();
            }
            return name;
        }
        public string GetMasterNameByMasterID(int masterID)
        {
            string name = "";
            DataTable masterDt = GetAllMasterBrandInfo();

            masterDt.DefaultView.RowFilter = "MasterBrandID=" + masterID;
            DataTable dt = masterDt.DefaultView.ToTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["Name"].ToString().Trim();
            }
            return name;
        }
        internal int GetMasterBrandIDBySerialID(int serialID)
        {
            int MasterBrandID = 0;
            DataTable serialDt = GetAllCarSerialInfo();

            serialDt.DefaultView.RowFilter = "CSID=" + serialID;
            DataTable dt = serialDt.DefaultView.ToTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                MasterBrandID = int.Parse(dt.Rows[0]["MasterBrandID"].ToString().Trim());
            }

            return MasterBrandID;
        }
        #endregion

        private DataTable GetAllMasterBrandInfo()
        {
            return DictionaryDataCache.Instance.GetDataTableByKey(DataCacheKey.Car_MasterBrand);
        }
        private DataTable GetAllBrandInfo()
        {
            return DictionaryDataCache.Instance.GetDataTableByKey(DataCacheKey.Car_Brand);
        }
        private DataTable GetAllCarSerialInfo()
        {
            return DictionaryDataCache.Instance.GetDataTableByKey(DataCacheKey.Car_Serial);
        }
    }
}
