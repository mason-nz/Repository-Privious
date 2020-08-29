using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask
{
    public class LablesModel
    {
        public int LabelID { get; set; }
        public int DictType { get; set; }
        public bool IsCustom { get; set; }
        public int DictId { get; set; }
        public int CreateUserID { get; set; }
        public string Name { get; set; }
        public string Creater { get; set; }
    }
    public class SonIpModel
    {
        public int LabelID { get; set; }
        public int SonIpID { get; set; }
        public int DictId { get; set; }
        public int CreateUserID { get; set; }
        public string Name { get; set; }
        public string Creater { get; set; }
    }
}
