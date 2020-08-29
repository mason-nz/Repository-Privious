using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1
{
    public class RespPublishAuditInfoDto : Entities.PublishAuditInfo
    {
        public string OptTypeName { get; set; }
        public string TrueName { get; set; }
        public string PubStatusName { get; set; }
    }
}