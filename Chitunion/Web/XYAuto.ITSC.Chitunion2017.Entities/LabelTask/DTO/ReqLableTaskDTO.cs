using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO
{
    public class ReqLableTaskDTO
    {
        public int TaskID { get; set; }
        public int Summary { get; set; }
        public int KeyWord { get; set; }
        public int OptType { get; set; }

        public List<LableCategory> CategoryInfo { get; set; }

        public List<LableScene> SceneInfo { get; set; }

        public List<LableCustomScene> CustomSceneInfo { get; set; }

        public List<LableIP> IPInfo { get; set; }

        //public List<LableCustom> CustomLableInfo { get; set; }
    }
    public class LableCategory
    {

        public int DictId { get; set; }

        public ENUM.EnumLableType DictType { get; set; } = ENUM.EnumLableType.分类;

        public string DictName { get; set; }
    }
    public class LableCustomScene
    {
        public ENUM.EnumLableType DictType { get; set; } = ENUM.EnumLableType.场景;
        public string DictName { get; set; }
    }
    public class LableScene
    {
        public int DictId { get; set; }
        public ENUM.EnumLableType DictType { get; set; } = ENUM.EnumLableType.场景;
        public string DictName { get; set; }
    }
    public class LableIP
    {
        public int DictId { get; set; }
        public ENUM.EnumLableType DictType { get; set; } = ENUM.EnumLableType.IP;
        public string DictName { get; set; }

        public List<LableSonIP> SonIP { get; set; }
    }
    public class LableSonIP
    {
        public int DictId { get; set; }
        public string DictName { get; set; }
        public List<LableSonIPTag> CustomLableInfo { get; set; }
    }
    public class LableSonIPTag
    {
        public string DictName { get; set; }
    }
}
