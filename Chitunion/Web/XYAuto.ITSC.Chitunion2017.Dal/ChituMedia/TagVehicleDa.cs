using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.ChituMedia
{
    public class TagVehicleDa:DataBase
    {
        #region 单例
        private TagVehicleDa() { }

        static TagVehicleDa instance = null;
        static readonly object padlock = new object();

        public static TagVehicleDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new TagVehicleDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取标签-车型对应表
        /// </summary>
        /// <returns></returns>
        public List<TagVehicleModel> GetTagVehicleList()
        {
            string SQL = $@"
                        SELECT  CM.Name AS MasterName ,
                                CB.Name AS BrandName ,
                                CS.ShowName AS SerialName ,
                                T.MediaTagName,CS.SerialID
                        FROM    BaseData2017..CarSerial AS CS
                                JOIN BaseData2017..CarBrand AS CB ON CS.BrandID = CB.BrandID
                                JOIN BaseData2017..CarMasterBrand CM ON CM.MasterID = CB.MasterId
                                JOIN BaseData2017..Tag_VehicleStyle_Map AS T ON T.VehicleStyleID = CS.SerialID;";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL);
     
            return DataTableToList<TagVehicleModel>(data.Tables[0]);



        }
    }
}
