﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class AuditDemandReqDTO
    {
        public int DemandBillNo { get; set; }
        public int AuditStatus { get; set; }
        public string Reason { get; set; }

        public List<int> ADGroupList { get; set; }
    }
}