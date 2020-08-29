using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo
{
    public class CustBasicInfo
    {
        private string custName = string.Empty;

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }
        private string sex = string.Empty;

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        private string tel = string.Empty;

        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        private string custCategoryID = string.Empty;

        public string CustCategoryID
        {
            get { return custCategoryID; }
            set { custCategoryID = value; }
        }
        private string memberID = string.Empty;

        public string MemberID
        {
            get { return memberID; }
            set { memberID = value; }
        }
        private string memberName = string.Empty;

        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }

        private int operID = 0;

        public int OperID
        {
            get { return operID; }
            set { operID = value; }
        }

        private DateTime operTime = DateTime.Now;

        public DateTime OperTime
        {
            get { return operTime; }
            set { operTime = value; }
        }
    }
}