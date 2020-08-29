using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.CarSerialInfo
{
    public class CarSerial : DataBase
    {
        #region Instance

        public static readonly CarSerial Instance = new CarSerial();

        #endregion Instance

        public List<RespCarBrandDto> GetMasterBrandList()
        {
            var sql = @"SELECT MasterID,Name FROM DBO.CarMasterBrand WITH(NOLOCK) ORDER BY Initial ASC";
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql);
            return DataTableToList<RespCarBrandDto>(data.Tables[0]);
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

        public List<RespCarSerialDto> GetCarSerialList(int brandId)
        {
            var sql = @"SELECT BrandID,SerialID AS CarSerialId,Name,ShowName FROM BaseData2017.DBO.CarSerial WITH(NOLOCK) WHERE BrandID = @BrandID";
            var parameters = new SqlParameter[]{
                            new SqlParameter("@BrandID",brandId)
                        };
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql, parameters);
            return DataTableToList<RespCarSerialDto>(data.Tables[0]);
        }
        public List<RespCarSerialDto> GetCarSerialList()
        {
            var sql = @"SELECT BrandID,SerialID AS CarSerialId,Name,ShowName FROM BaseData2017.DBO.CarSerial";
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql);
            return DataTableToList<RespCarSerialDto>(data.Tables[0]);
        }
    }
}
