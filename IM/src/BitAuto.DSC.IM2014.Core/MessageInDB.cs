using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM2014.Core
{
    public class MessageInDB
    {
        //消息类型
        private int messagetype;
        public int MessageType
        {
            set
            {
                messagetype = value;
            }
            get
            {
                return messagetype;
            }
        }
        //是否已入库
        private bool isindb;
        public bool IsInDB
        {
            set
            {
                isindb = value;
            }
            get
            {
                return isindb;
            }
        }
        //分配坐席标识
        private long allocid;
        public long AllocID
        {
            set
            {
                allocid = value;
            }
            get
            {
                return allocid;
            }
        }

        private string mfrom;
        public string MFrom
        {
            set
            {
                mfrom = value;
            }
            get
            {
                return mfrom;
            }
        }
        private string msendto;
        public string MSendTo
        {
            set
            {
                msendto = value;
            }
            get
            {
                return msendto;
            }
        }
        private DateTime sendtime;
        public DateTime SendTime
        {
            set
            {
                sendtime = value;
            }
            get
            {
                return sendtime;
            }
        }
        private string message;
        public string Message
        {
            set
            {
                message = value;
            }
            get
            {
                return message;
            }
        }
    }
}
