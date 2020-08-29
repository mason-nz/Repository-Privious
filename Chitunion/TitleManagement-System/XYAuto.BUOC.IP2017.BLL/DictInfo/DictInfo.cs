using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.DictInfo
{
    public class DictInfo
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeID(int typeID)
        {
            return Dal.DictInfo.DictInfo.Instance.GetDictInfoByTypeID(typeID);
        }

        public DataTable GetDictInfoByMediaType(Entities.ENUM.ENUM.EnumMediaType mediaType)
        {
            return Dal.DictInfo.DictInfo.Instance.GetDictInfoByTypeID(GetDictTypeIdByMediaType(mediaType));
        }

        public int GetDictTypeIdByMediaType(Entities.ENUM.ENUM.EnumMediaType mediaType)
        {
            Dictionary<Entities.ENUM.ENUM.EnumMediaType, int> dict = new Dictionary<Entities.ENUM.ENUM.EnumMediaType, int>()
            {
                {Entities.ENUM.ENUM.EnumMediaType.微信,47 },
                {Entities.ENUM.ENUM.EnumMediaType.APP,52 },
                {Entities.ENUM.ENUM.EnumMediaType.新浪微博,19 },
                {Entities.ENUM.ENUM.EnumMediaType.视频,25 },
                {Entities.ENUM.ENUM.EnumMediaType.直播,25 },
                {Entities.ENUM.ENUM.EnumMediaType.头条,47 },
                {Entities.ENUM.ENUM.EnumMediaType.搜狐,47 }
            };

            var dicValue = dict.FirstOrDefault(x => x.Key == mediaType);

            if (dicValue.Value == 0)
                return -2;

            return dicValue.Value;
        }
    }
}
