using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.DSC.IM_2015.Dal
{
    public class DictionaryDataCache : DataBase
    {
        #region Instance
        public static readonly DictionaryDataCache Instance = new DictionaryDataCache();
        #endregion

        /// 从数据库获取数据
        /// <summary>
        /// 从数据库获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataByDBFromCC(string sql)
        {
            return SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sql).Tables[0];
        }
        /// 从数据库获取数据
        /// <summary>
        /// 从数据库获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataByDBFromBD(string sql)
        {
            return SqlHelper.ExecuteDataset(BD_ConnectionStrings, CommandType.Text, sql).Tables[0];
        }
        /// 从数据库获取数据
        /// <summary>
        /// 从数据库获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataByDBFromCRM(string sql)
        {
            return SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sql).Tables[0];
        }

        public DataTable GetCar_Car(out string keycol)
        {
            string sql = @"SELECT CarID,CarName,CsID,CarYearType,Status FROM CRM2009.dbo.Car_Car";
            keycol = "CarID";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetCar_Serial(out string keycol)
        {
            string sql = @"SELECT  [MasterBrandID] ,
                                        [BrandID] ,
                                        [SerialID] AS CSID ,
                                        [Name] ,
                                        [SEOName] ,
                                        [AllSpell] ,
                                        [Spell] ,
                                        [CsSaleState] ,
                                        [CreateTime] ,
                                        [CsLevel]
                                FROM    CRM2009.dbo.Car_Serial";
            keycol = "CSID";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetCar_Brand(out string keycol)
        {
            string sql = @"SELECT MasterBrandID,BrandID,Name,SEOName FROM CRM2009.dbo.Car_Brand";
            keycol = "BrandID";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetCar_MasterBrand(out string keycol)
        {
            string sql = @"SELECT MasterBrandID,Name,Ename,SEOName FROM CRM2009.dbo.Car_MasterBrand";
            keycol = "MasterBrandID";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetAreaInfo_Province(out string keycol)
        {
            string sql = @"SELECT AreaID,PID,AreaName,AbbrName FROM CRM2009.dbo.AreaInfo WHERE Level=1";
            keycol = "AreaID";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetAreaInfo_City(out string keycol)
        {
            string sql = @"SELECT AreaID,PID,AreaName,AbbrName FROM CRM2009.dbo.AreaInfo WHERE Level=2";
            keycol = "AreaID";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetAreaInfo_County(out string keycol)
        {
            string sql = @"SELECT AreaID,PID,AreaName,AbbrName FROM CRM2009.dbo.AreaInfo WHERE Level=3";
            keycol = "AreaID";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetDMSMember(out string keycol)
        {
            string sql = @"SELECT ID,CustID,MemberCode,Name,Status,ProvinceID,CityID,CountyID FROM CRM2009.dbo.DMSMember";
            keycol = "MemberCode";
            return GetDataByDBFromCRM(sql);
        }
        public DataTable GetAcitvityInfo(out string keycol)
        {
            string sql = @"SELECT ActivityID,ActivityName,AGuid,Url FROM dbo.AcitvityInfo";
            keycol = "AGuid";
            return GetDataByDBFromBD(sql);
        }
    }
}
