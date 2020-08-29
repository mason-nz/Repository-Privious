using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetAppQualifucationResDTO
    {
        public int MediaRelations { get; set; }
        public string MediaRelationsName { get; set; }
        public int OperatingType { get; set; }
        public string OperatingTypeName { get; set; }
        public string EnterpriseName { get; set; }
        public string BusinessLicense { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public bool CanEdit { get; set; }
    }
}
