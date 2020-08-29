using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query
{
    public class UploadFileQuery<T> : QueryPageBase<T>
    {
        public UploadFileQuery()
        {
            this.Type = -1;
            this.CreaetUserID = -1;
            this.RelationID = -1;
        }

        public int Type { get; set; }
        public string RelationTableName { get; set; }
        public int RelationID { get; set; }
        public string FilePah { get; set; }
        public int CreaetUserID { get; set; }

        public string AEUserId { get; set; }
    }
}
