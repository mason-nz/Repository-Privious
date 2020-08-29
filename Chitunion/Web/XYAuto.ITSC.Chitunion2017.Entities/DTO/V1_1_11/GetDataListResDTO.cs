using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11
{
    public class GetDataListResDTO
    {
        public GetDataListResDTO()
        {
            this.TypeList = new List<TypeItem>();
            this.Total = new List<MaterielChannelData>();
        }

        public List<TypeItem> TypeList { get; set; }
        public List<MaterielChannelData> Total { get; set; }
    }

    public class TypeItem
    {
        public TypeItem()
        {
            this.MediaList = new List<MediaItem>();
        }
        public string MediaTypeName { get; set; }
        public List<MediaItem> MediaList { get; set; }
        public int OrderBy { get; set; }
    }


    public class MediaItem
    {
        public MediaItem()
        {
            this.DataList = new List<MaterielChannelData>();
        }
        public int ChannelID { get; set; }
        public string MediaNumber { get; set; }
        public string MediaName { get; set; }
        public List<MaterielChannelData> DataList { get; set; }
    }
}
