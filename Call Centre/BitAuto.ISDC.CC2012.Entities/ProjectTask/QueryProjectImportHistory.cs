using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryProjectImportHistory
    {
        public QueryProjectImportHistory()
		{
		 _projectid = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private int? _projectid;

		/// <summary>
		/// 
		/// </summary>
		public int? ProjectID
		{
			set{ _projectid=value;}
			get{return _projectid;}
		}

		#endregion Model
    }
}
