using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryProjectTask_Employee 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:32 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryProjectTask_Employee
	{
		private int _source;
        private string _relationID;
        private int _status;
        private string _ptid;

        public QueryProjectTask_Employee()
        {
            _source = Constant.INT_INVALID_VALUE;
            _relationID = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _ptid = Constant.STRING_INVALID_VALUE;
        }

        /// <summary>
        /// 数据来源 1:EXCEL,2:CRM
        /// </summary>
        public int Source
        {
            set { _source = value; }
            get { return _source; }
        }
        /// <summary>
        /// 关联ID（CC_ExcelInfo主键或者CustInfo主键）
        /// </summary>
        public string RelationID
        {
            set { _relationID = value; }
            get { return _relationID; }
        }
        /// <summary>
        /// 记录状态 0:有效 -1:无效
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        public string PTID
        {
            set { _ptid = value; }
            get { return _ptid; }
        }

	}
}

