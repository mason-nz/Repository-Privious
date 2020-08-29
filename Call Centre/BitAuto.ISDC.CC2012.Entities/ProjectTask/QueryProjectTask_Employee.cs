using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ʵ����QueryProjectTask_Employee ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// ������Դ 1:EXCEL,2:CRM
        /// </summary>
        public int Source
        {
            set { _source = value; }
            get { return _source; }
        }
        /// <summary>
        /// ����ID��CC_ExcelInfo��������CustInfo������
        /// </summary>
        public string RelationID
        {
            set { _relationID = value; }
            get { return _relationID; }
        }
        /// <summary>
        /// ��¼״̬ 0:��Ч -1:��Ч
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

