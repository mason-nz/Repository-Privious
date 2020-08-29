using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ʵ����QueryProjectTask_DMSMember ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryProjectTask_DMSMember
	{
        public QueryProjectTask_DMSMember()
        {
            ptID = Constant.STRING_INVALID_VALUE;
            memberName = Constant.STRING_INVALID_VALUE;
            memberAbbr = Constant.STRING_INVALID_VALUE;
            custName = Constant.STRING_INVALID_VALUE;
            custID = Constant.STRING_INVALID_VALUE;
            applystarttime = Constant.STRING_INVALID_VALUE;
            applyendtime = Constant.STRING_INVALID_VALUE;
            applyusername = Constant.STRING_INVALID_VALUE;
            memberoptstarttime = Constant.STRING_INVALID_VALUE;
            memberoptendtime = Constant.STRING_INVALID_VALUE;
            dmsmembercreateuserid = Constant.INT_INVALID_VALUE;
            dmssyncstatus = Constant.STRING_INVALID_VALUE;
            defaultdmssyncstatus = Constant.STRING_INVALID_VALUE;
            dmsmemberapplyuserid = Constant.INT_INVALID_VALUE;
            dmsstatus = Constant.STRING_INVALID_VALUE;
        }

        private string ptID;
        /// <summary>
        /// 
        /// </summary>
        public string PTID { get { return ptID; } set { ptID = value; } }


        private string memberName;
        /// <summary>
        /// ��Ա����
        /// </summary>
        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }

        private string memberAbbr;
        /// <summary>
        /// ��Ա���
        /// </summary>
        public string MemberAbbr
        {
            get { return memberAbbr; }
            set { memberAbbr = value; }
        }

        private string custName;
        /// <summary>
        /// �ͻ�����
        /// </summary>
        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        private string custID;
        /// <summary>
        /// �ͻ�ID
        /// </summary>
        public string CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        private string applystarttime;
        /// <summary>
        /// ��Ա���뿪ʼʱ��
        /// </summary>
        public string ApplyStartTime
        {
            get { return applystarttime; }
            set { applystarttime = value; }
        }

        private string applyendtime;
        /// <summary>
        /// ��Ա�������ʱ��
        /// </summary>
        public string ApplyEndTime
        {
            get { return applyendtime; }
            set { applyendtime = value; }
        }

        private string applyusername;
        /// <summary>
        /// ��Ա����������
        /// </summary>
        public string ApplyUserName
        {
            get { return applyusername; }
            set { applyusername = value; }
        }

        private string memberoptstarttime;
        /// <summary>
        /// ��Ա����
        /// </summary>
        public string MemberOptStartTime
        {
            get { return memberoptstarttime; }
            set { memberoptstarttime = value; }
        }

        private string memberoptendtime;
        /// <summary>
        /// ��Ա�������ʱ��
        /// </summary>
        public string MemberOptEndTime
        {
            get { return memberoptendtime; }
            set { memberoptendtime = value; }
        }

        private int dmsmembercreateuserid;
        /// <summary>
        /// CRMϵͳ�У�DMSMember��Ա��Ϣ������ID
        /// </summary>
        public int DMSMemberCreateUserID
        {
            get { return dmsmembercreateuserid; }
            set { dmsmembercreateuserid = value; }
        }

        private int dmsmemberapplyuserid;
        /// <summary>
        /// CRMϵͳ�У�DMSMember��Ա��Ϣ������ID
        /// </summary>
        public int DMSMemberApplyUserID
        {
            get { return dmsmemberapplyuserid; }
            set { dmsmemberapplyuserid = value; }
        }


        private string dmssyncstatus;
        /// <summary>
        /// ��Ա����״̬ID��
        /// </summary>
        public string DMSSyncStatus
        {
            get { return dmssyncstatus; }
            set { dmssyncstatus = value; }
        }

        private string defaultdmssyncstatus;
        /// <summary>
        /// ��Ա����״̬ID����Ĭ�ϣ�
        /// </summary>
        public string DefaultDMSSyncStatus
        {
            get { return defaultdmssyncstatus; }
            set { defaultdmssyncstatus = value; }
        }

        private string dmsstatus;
        /// <summary>
        /// CRMϵͳ�л�Ա״̬
        /// </summary>
        public string DMSStatus
        {
            get { return dmsstatus; }
            set { dmsstatus = value; }
        }

	}
}

