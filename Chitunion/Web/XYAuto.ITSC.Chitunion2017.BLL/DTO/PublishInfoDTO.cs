using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.DTO
{
    /// <summary>
    /// ls
    /// </summary>
    public class PublishInfoDTO
    {
        public Entities.Publish.PublishBasicInfo Publish { get; set; }
        public List<string> Prices { get; set; }
    }
}