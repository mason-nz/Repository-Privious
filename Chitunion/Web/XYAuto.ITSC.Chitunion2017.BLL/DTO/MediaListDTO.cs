using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.DTO
{
    /// <summary>
    /// ls
    /// </summary>
    public class MediaListDTO
    {
        public List<MediaItemDTO> List { get; set; }
        public int TotalCount { get; set; }
        
    }

    public class MediaItemDTO
    {
        public int MediaID { get; set; }
        public MediaTypeEnum MediaType { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int FansCount { get; set; }
        public string HeadIconURL { get; set; }
        public string City { get; set; }
        public string Overlay { get; set; }
        public string Category { get; set; }
        public string LevelType { get; set; }
        public string Source { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string Platform { get; set; }
        public int DailyLive { get; set; }
        public int ADCount { get; set; }
    }
}
