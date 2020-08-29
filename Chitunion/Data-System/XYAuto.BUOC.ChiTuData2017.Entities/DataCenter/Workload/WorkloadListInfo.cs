using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Workload
{
    public class WorkloadListInfo
    {
        /// <summary>
        /// 操作类型名称
        /// </summary>		
        private string _operatetypename;
        public string OperateTypeName
        {
            get { return _operatetypename; }
            set { _operatetypename = value; }
        }
        /// <summary>
        /// 操作类型
        /// </summary>		
        private int _operatetype;
        public int OperateType
        {
            get { return _operatetype; }
            set { _operatetype = value; }
        }

        /// <summary>
        /// 文章类型
        /// </summary>		
        private string _articletypename = string.Empty;
        public string ArticleTypeName
        {
            get { return _articletypename; }
            set { _articletypename = value; }
        }
        /// <summary>
        /// 待处理
        /// </summary>		
        private int _pendingcount;
        public int PendingCount
        {
            get { return _pendingcount; }
            set { _pendingcount = value; }
        }
        /// <summary>
        /// 领取数
        /// </summary>		
        private int _receivecount;
        public int ReceiveCount
        {
            get { return _receivecount; }
            set { _receivecount = value; }
        }
        /// <summary>
        /// 完成数
        /// </summary>		
        private int _completedcount;
        public int CompletedCount
        {
            get { return _completedcount; }
            set { _completedcount = value; }
        }
        /// <summary>
        /// 未完成数
        /// </summary>		
        private int _uncompletedcount;
        public int UncompletedCount
        {
            get { return _uncompletedcount; }
            set { _uncompletedcount = value; }
        }
        /// <summary>
        /// 保留数
        /// </summary>		
        private int _retaincount;
        public int RetainCount
        {
            get { return _retaincount; }
            set { _retaincount = value; }
        }
        /// <summary>
        /// 设置为腰
        /// </summary>		
        private int _settowaist;
        public int SetToWaist
        {
            get { return _settowaist; }
            set { _settowaist = value; }
        }
        /// <summary>
        /// 作废数
        /// </summary>		
        private int _invalidcount;
        public int InvalidCount
        {
            get { return _invalidcount; }
            set { _invalidcount = value; }
        }
        /// <summary>
        /// 被作废数
        /// </summary>		
        private int _cancelledcount;
        public int CancelledCount
        {
            get { return _cancelledcount; }
            set { _cancelledcount = value; }
        }
        /// <summary>
        /// 作废数(头)
        /// </summary>		
        private int _cancelledheadcount;
        public int CancelledHeadCount
        {
            get { return _cancelledheadcount; }
            set { _cancelledheadcount = value; }
        }
        /// <summary>
        /// 作废数(腰)
        /// </summary>		
        private int _cancelledwaistcount;
        public int CancelledWaistCount
        {
            get { return _cancelledwaistcount; }
            set { _cancelledwaistcount = value; }
        }
        /// <summary>
        /// 退回数
        /// </summary>		
        private int _returncount;
        public int ReturnCount
        {
            get { return _returncount; }
            set { _returncount = value; }
        }
        /// <summary>
        /// 退回数(头)
        /// </summary>		
        private int _returnheadcount;
        public int ReturnHeadCount
        {
            get { return _returnheadcount; }
            set { _returnheadcount = value; }
        }
        /// <summary>
        /// 退回数(腰)
        /// </summary>		
        private int _returnwaistcount;
        public int ReturnWaistCount
        {
            get { return _returnwaistcount; }
            set { _returnwaistcount = value; }
        }
        /// <summary>
        /// 被退回数
        /// </summary>		
        private int _bereturnedcount;
        public int BeReturnedCount
        {
            get { return _bereturnedcount; }
            set { _bereturnedcount = value; }
        }
        /// <summary>
        /// 放弃(头)
        /// </summary>		
        private int _giveuphead;
        public int GiveUpHead
        {
            get { return _giveuphead; }
            set { _giveuphead = value; }
        }
        /// <summary>
        /// 放弃(腰)
        /// </summary>		
        private int _giveupwaist;
        public int GiveUpWaist
        {
            get { return _giveupwaist; }
            set { _giveupwaist = value; }
        }
        /// <summary>
        /// 操作人Id
        /// </summary>		
        private int _userid;
        public int UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 操作人姓名
        /// </summary>		
        private string _username = string.Empty;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// 统计时间
        /// </summary>		
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }


    }
}
