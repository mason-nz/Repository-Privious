using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CC2015_HollyFormsApp.Code.Logic
{
    public static class CurrentCallInfo
    {

        private static long _callID=-1;
        private static long _callidforhangupempty=-1;
        private static string _callNum = string.Empty;
        private static string _callNumforhangupempty = string.Empty;

        /// <summary>
        /// 当前话务ID（挂断后不清空，只有呼入振铃或呼出初始化时，才赋值）
        /// </summary>
        public static long CallID
        {
            set { _callID = value; }
            get { return _callID; }
        }

        /// <summary>
        /// 当前话务ID（挂断后需清空，只有呼入振铃或呼出初始化时，才赋值）
        /// </summary>
        public static long CallIDForHangupEmpty
        {
            set { _callidforhangupempty = value; }
            get { return _callidforhangupempty; }
        }

        /// <summary>
        /// 当前话务中，主叫号码（挂断后不清空，只有呼入振铃时，才赋值）
        /// </summary>
        public static string CallNum
        {
            set { _callNum = value; }
            get { return _callNum; }
        }

        /// <summary>
        /// 当前话务中，主叫号码（挂断后需清空，只有呼入振铃时，才赋值）
        /// </summary>
        public static string CallNumForHangupEmpty
        {
            set { _callNumforhangupempty = value; }
            get { return _callNumforhangupempty; }
        }
    }
}
