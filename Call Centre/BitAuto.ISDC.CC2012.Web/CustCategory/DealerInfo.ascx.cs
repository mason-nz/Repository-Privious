using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.CustCategory
{
    public partial class DealerInfo : System.Web.UI.UserControl
    {
        private string custid = string.Empty;
        /// <summary>
        /// 客户ＩＤ
        /// </summary>
        public string CustID
        {
            get
            {
                return custid;
            }
            set
            {
                custid = value;
            }
        }
        #region IM调用参数
        //访问系统在型
        //IM系统：isIM
        public string RequestSYSType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("SYSType");
            }
        }
        //IM添加工单传过来的经销商名称
        public string RequestMemberName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("MemberName");
            }
        }
        //IM添加工单传过来的经销商会员号
        public string RequestMemberCode
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("MemberCode");
            }
        }
        #endregion
        //private string view = string.Empty;
        ///// <summary>
        ///// 1,是查看，0是编辑
        ///// </summary>
        //public string View
        //{
        //    get
        //    {
        //        return view;
        //    }
        //    set
        //    {
        //        view = value;
        //    }
        //}
        //public string CityScopeID = "";
        //public string MemberType = "";
        //public string CarTypeID = "";
        //public string MemberStatusID = "";


        public string MemberName = "";
        public string MemberCode = "";
        public string Remark = "";
        public string logmsg = "";

        public string RequestFrom
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("From");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化
                GetDealerInfoModel(CustID);
            }
        }
        private void GetDealerInfoModel(string CustID)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            if (!string.IsNullOrEmpty(CustID))
            {
                Entities.DealerInfo DealerInfoModel = null;
                DealerInfoModel = BLL.DealerInfo.Instance.GetDealerInfo(CustID);

                if (DealerInfoModel != null)
                {
                    MemberName = DealerInfoModel.Name.Trim() ;
                    hid_MemberName.Value = MemberName;
                    MemberCode = DealerInfoModel.MemberCode;
                    Remark = DealerInfoModel.Remark;
                }
            }
            else if (!string.IsNullOrEmpty(RequestSYSType))
            {
                if (RequestSYSType.Contains("isIM"))
                {
                    MemberName = RequestMemberName;
                    hid_MemberName.Value = MemberName; 
                    MemberCode = RequestMemberCode;
                }
            }

            logmsg = "（已" + sw.Elapsed.TotalSeconds.ToString("0.00") + "s）；";
            sw.Stop();
        }
    }
}