using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_2015.Entities
{

    [Serializable]
    public class HMC_Entity
    {
        public bool IsSuccess
        { set; get; }
        public string UserName
        { set; get; }
        public string Mobile
        { set; get; }
        public string City
        { set; get; }
        public string Gender
        { set; get; }
        public string UserID
        {
            set;
            get;
        }
    }

    [Serializable]
    public class Token_Entity
    {
        public string access_token
        { set; get; }
        public string expires_in
        { set; get; }
    }

}
