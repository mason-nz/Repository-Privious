using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ʵ����QueryKLQAnswer ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryKLQAnswer
	{
		public QueryKLQAnswer()
		{
		 _klqid = Constant.INT_INVALID_VALUE;
		 _klaoid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

         _appendvaluekey = 0;   //����ֵ�����뿼�Լ���ʱʹ��
		}
		#region Model
		private long _klqid;
		private int _klaoid;
		private DateTime? _createtime;
        private int? _createuserid;
        private int _appendvaluekey;
		/// <summary>
		/// 
		/// </summary>
		public long KLQID
		{
			set{ _klqid=value;}
			get{return _klqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int KLAOID
		{
			set{ _klaoid=value;}
			get{return _klaoid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CreateUserID
		{
			set{ _createuserid=value;}
			get{return _createuserid;}
		}

        public int AppendValueKey
        {
            set { _appendvaluekey = value; }
            get { return _appendvaluekey; }
        }
		#endregion Model

	}
}

