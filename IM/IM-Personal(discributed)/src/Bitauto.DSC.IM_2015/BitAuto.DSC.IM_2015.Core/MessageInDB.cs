using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_2015.Core
{
    public class MessageInDB
    {
        ////消息类型
        //private int messagetype;
        //public int MessageType
        //{
        //    set
        //    {
        //        messagetype = value;
        //    }
        //    get
        //    {
        //        return messagetype;
        //    }
        //}
        ////是否已入库
        //private bool isindb;
        //public bool IsInDB
        //{
        //    set
        //    {
        //        isindb = value;
        //    }
        //    get
        //    {
        //        return isindb;
        //    }
        //}
        ////分配坐席标识
        //private long allocid;
        //public long AllocID
        //{
        //    set
        //    {
        //        allocid = value;
        //    }
        //    get
        //    {
        //        return allocid;
        //    }
        //}

        //private string mfrom;
        //public string MFrom
        //{
        //    set
        //    {
        //        mfrom = value;
        //    }
        //    get
        //    {
        //        return mfrom;
        //    }
        //}
        //private string msendto;
        //public string MSendTo
        //{
        //    set
        //    {
        //        msendto = value;
        //    }
        //    get
        //    {
        //        return msendto;
        //    }
        //}
        //private DateTime sendtime;
        //public DateTime SendTime
        //{
        //    set
        //    {
        //        sendtime = value;
        //    }
        //    get
        //    {
        //        return sendtime;
        //    }
        //}
        //private string message;
        //public string Message
        //{
        //    set
        //    {
        //        message = value;
        //    }
        //    get
        //    {
        //        return message;
        //    }
        //}


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

        public DateTime CreateTime { get; set; }
        //0：网友，1：客服，2：系统
        public int Sender { get; set; }

        //会话ID
        public int CSID { get; set; }

        //0: Text; 1:File
        public int Type { get; set; }

        //0:正常，1删除，-1停用     
        public int Status { get; set; }

        //内容        
        public string Content { get; set; }

    }
}
