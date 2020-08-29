using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_DMS2014.WebService.CC
{
    public class CCWebServiceHepler
    {
        #region Instance
        public static readonly CCWebServiceHepler Instance = new CCWebServiceHepler();
        #endregion


        private CCDataInterfaceService.CCDataInterfaceServiceSoapClient c = new CCDataInterfaceService.CCDataInterfaceServiceSoapClient();
        private string authorize = System.Configuration.ConfigurationManager.AppSettings["AgentInfoAuthorizeCode"].ToString();

        /// <summary>
        /// CC工单页面接口，根据参数，此接口返回工单添加页面包括参数
        /// </summary>
        /// <param name="phone">电话</param>
        /// <param name="csID">业务主键（留言表主键或会话表主键）</param>
        /// <param name="cbName">客户姓名</param>
        /// <param name="cbSex">性别</param>
        /// <param name="provinceID">省份</param>
        /// <param name="cityID">城市</param>
        /// <param name="county">县</param>
        /// <param name="memberCode">会员Code</param>
        /// <param name="imBussinessType">1是对话，2是留言</param>
        /// <returns></returns>
        public string CCDataInterface_GetAddWOrderComeIn_IMNC_URL(string phone, string csID, string cbName, int cbSex, int provinceID, int cityID, int county, string memberCode,int imBussinessType)
        {
            string msg = string.Empty;
            long csid = 0;
            long.TryParse(csID, out csid);
            CCDataInterfaceService.CallSourceEnum callSource = CCDataInterfaceService.CallSourceEnum.C03_IM对话;
            if (imBussinessType == 2)
            {
                callSource = CCDataInterfaceService.CallSourceEnum.C04_IM留言;
            }
            msg = c.GetAddWOrderComeIn_IMJXS_NC_URL(authorize, callSource, phone, csid, cbName, cbSex, provinceID, cityID, county, memberCode);
            return msg;
        }
    }
}
