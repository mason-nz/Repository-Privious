/********************************************************
*创建人：lixiong
*创建时间：2017/7/25 9:43:21
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.CarSerialInfo
{
    public class CarSerial : DataBase
    {
        #region Instance

        public static readonly CarSerial Instance = new CarSerial();

        #endregion Instance

        public List<RespCarMasterDto> GetMasterBrandList()
        {
            var sql = @"SELECT MasterID,Name FROM DBO.CarMasterBrand WITH(NOLOCK) ORDER BY MasterID ASC";
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql);
            return DataTableToList<RespCarMasterDto>(data.Tables[0]);
        }

        public List<RespCarMasterDto> GetMasterListByName(string masterName)
        {
            var sql = @"SELECT MasterID,Name FROM DBO.CarMasterBrand WITH(NOLOCK) WHERE Name LIKE '%" + StringHelper.SqlFilter(masterName) + "%' ORDER BY MasterID ASC";
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql);
            return DataTableToList<RespCarMasterDto>(data.Tables[0]);
        }

        public List<RespCarBrandDto> GetBrandList(int masterBrandId)
        {
            var sql = @"SELECT MasterId,BrandID,Name FROM DBO.CarBrand WITH(NOLOCK) WHERE MasterId = @MasterId";
            var parameters = new SqlParameter[]{
                            new SqlParameter("@MasterId",masterBrandId)
                        };
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql, parameters);
            return DataTableToList<RespCarBrandDto>(data.Tables[0]);
        }

        public DataTable GetBrandSerialList(string masterName)
        {
            var sql = @"SELECT cb.MasterId,cmb.Name AS MasterName,
	   cb.BrandID,cb.Name AS BrandName,
	   cs.SerialID,cs.ShowName 
FROM BaseData2017.dbo.CarSerial AS cs WITH(NOLOCK) 
JOIN BaseData2017.dbo.CarBrand AS cb WITH(NOLOCK) ON cs.BrandID=cb.BrandID
JOIN BaseData2017.dbo.CarMasterBrand AS cmb WITH(NOLOCK) ON cmb.MasterID=cb.MasterId
WHERE cs.SaleState=1 " + (string.IsNullOrEmpty(masterName) ? "" : " And cmb.Name LIKE '%" + StringHelper.SqlFilter(masterName) + "%' ")+
" ORDER BY cmb.UrlSpell,cb.Initial,cs.UrlSpell";
            //var parameters = new SqlParameter[]{
            //                new SqlParameter("@MasterId",masterBrandId)
            //            };
            DataSet data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql);
            //return DataTableToList<RespCarBrandSerialDto>(data.Tables[0]);
            if (data != null && data.Tables.Count > 0)
            {
                return data.Tables[0];
            }
            return null;
        }

        public List<RespCarSerialDto> GetCarSerialList(int brandId)
        {
            var sql = @"SELECT BrandID,SerialID AS CarSerialId,Name,ShowName FROM BaseData2017.DBO.CarSerial WITH(NOLOCK) WHERE BrandID = @BrandID";
            var parameters = new SqlParameter[]{
                            new SqlParameter("@BrandID",brandId)
                        };
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql, parameters);
            return DataTableToList<RespCarSerialDto>(data.Tables[0]);
        }
    }
}