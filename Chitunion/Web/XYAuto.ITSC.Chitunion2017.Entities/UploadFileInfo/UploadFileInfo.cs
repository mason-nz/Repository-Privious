using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.UploadFileInfo
{
    public class UploadFileInfo
    {
        public UploadFileInfo()
        {
            CreateTime = DateTime.Now;
        }

        public int RecID { get; set; }
        public int Type { get; set; }
        public string RelationTableName { get; set; }
        public int RelationID { get; set; }
        public string FilePah { get; set; }
        public string FileName { get; set; }
        public string ExtendName { get; set; }
        public int FileSize { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreaetUserID { get; set; }
    }
}
