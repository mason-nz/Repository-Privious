using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers
{
    public class SendEmailInfo
    {
        private int receiverID;

        public int ReceiverID
        {
            get { return receiverID; }
            set { receiverID = value; }
        }
        private string receiverName;

        public string ReceiverName
        {
            get { return receiverName; }
            set { receiverName = value; }
        }
        private string departName;

        public string DepartName
        {
            get { return departName; }
            set { departName = value; }
        }
        private string orderID;

        public string OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string desc;

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private string lastOperTime;

        public string LastOperTime
        {
            get { return lastOperTime; }
            set { lastOperTime = value; }
        }
    }
}