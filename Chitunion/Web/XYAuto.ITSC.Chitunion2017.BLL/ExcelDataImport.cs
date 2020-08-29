using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class ExcelDataImport
    {
        Dal.ExcelDataImport dal = new Dal.ExcelDataImport();

        public int DoProcess(ImportDTO dto, ref bool isUpdate)
        {

            int platform = 0;
            if (dto.MediaType.Equals(MediaTypeEnum.视频) || dto.MediaType.Equals(MediaTypeEnum.直播))
                platform = dto.MediaInfo.Platform;
            int mediaID = dal.GetExistsMedia(dto.MediaType, dto.MediaInfo.CreateUserID, dto.Key, platform);
            int res = 0;
            if (mediaID.Equals(0))
            {
                isUpdate = false;
                res = dal.InsertOne(dto);
            }
            else
            {
                isUpdate = true;
                dto.MediaInfo.MediaID = mediaID;
                res = dal.UpdateOne(dto);
            }
            return res;
        }

        /// <summary>
        /// 根据类型 名称 获取字典ID
        /// </summary>
        /// <param name="type">字典类型</param>
        /// <param name="name">字典名称</param>
        /// <returns></returns>
        public int GetDictID(DictTypeEnum type, string name)
        {
            return dal.GetDictID((int)type, name);
        }

        /// <summary>
        /// 根据所在地 省名-市名 获取省市对应ID 没有为-1
        /// </summary>
        /// <param name="location">所在地</param>
        /// <returns>省ID,市ID</returns>
        public Tuple<int, int> GetArea(string location)
        {
            return dal.GetArea(location);
        }

        public void DeleteAppDetail(List<string> appNameList, int userID)
        {
            dal.DeleteAppDetail(appNameList, userID);
        }
    }
}
