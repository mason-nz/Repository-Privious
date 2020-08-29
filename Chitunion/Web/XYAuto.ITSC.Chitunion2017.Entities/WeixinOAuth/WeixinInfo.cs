using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth
{
    public class WeixinInfo
    {
        public WeixinInfo()
        {
            this.OAuthStatus = (int)Entities.Enum.OAuthStatusEnum.未授权;
        }

        ///<summary>
        ///RecID
        ///</summary>
        public int RecID { get; set; }

        ///<summary>
        ///AppID
        ///</summary>
        public string AppID { get; set; }

        ///<summary>
        ///AccessToken
        ///</summary>
        public string AccessToken { get; set; }

        public string RefreshAccessToken { get; set; }
        private DateTime gettokentime = Constants.Constant.DATE_INVALID_VALUE;

        public DateTime GetTokenTime
        {
            get { return gettokentime; }
            set { gettokentime = value; }
        }

        ///<summary>
        ///WxNumber
        ///</summary>
        public string WxNumber { get; set; }

        ///<summary>
        ///OriginalID
        ///</summary>
        public string OriginalID { get; set; }

        ///<summary>
        ///NickName
        ///</summary>
        public string NickName { get; set; }

        private int servicetype = Constants.Constant.INT_INVALID_VALUE;

        ///<summary>
        ///ServiceType
        ///</summary>
        public int ServiceType
        {
            get { return servicetype; }
            set { servicetype = value; }
        }

        private bool isverify = false;

        public bool IsVerify
        {
            get { return isverify; }
            set { isverify = value; }
        }

        private int verifytype = Constants.Constant.INT_INVALID_VALUE;

        ///<summary>
        ///VerifyType
        ///</summary>
        public int VerifyType
        {
            get { return verifytype; }
            set { verifytype = value; }
        }

        ///<summary>
        ///HeadImg
        ///</summary>
        public string HeadImg { get; set; }

        ///<summary>
        ///QrCodeUrl
        ///</summary>
        public string QrCodeUrl { get; set; }

        ///<summary>
        ///FansCount
        ///</summary>
        public int FansCount { get; set; }

        public string Biz { get; set; }

        private int status = -1;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public int OAuthStatus { get; set; }
        private DateTime createtime = Constants.Constant.DATE_INVALID_VALUE;

        ///<summary>
        ///CreateTime
        ///</summary>
        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }

        private DateTime modifytime = Constants.Constant.DATE_INVALID_VALUE;

        ///<summary>
        ///ModifyTime
        ///</summary>
        public DateTime ModifyTime
        {
            get { return modifytime; }
            set { modifytime = value; }
        }

        private int sourcetype = Constants.Constant.INT_INVALID_VALUE;

        public int SourceType
        {
            get { return sourcetype; }
            set { sourcetype = value; }
        }

        public string Summary { get; set; }
        public string FullName { get; set; }
        public string CreditCode { get; set; }
        public string BusinessScope { get; set; }
        public string EnterpriseType { get; set; }
        public string EnterpriseCreateDate { get; set; }
        public string EnterpriseBusinessTerm { get; set; }
        public string EnterpriseVerifyDate { get; set; }
        public string Location { get; set; }
        private int leveltype = Constants.Constant.INT_INVALID_VALUE;

        public int LevelType
        {
            get { return leveltype; }
            set { leveltype = value; }
        }

        private DateTime regtime = Constants.Constant.DATE_INVALID_VALUE;

        public DateTime RegTime
        {
            get { return regtime; }
            set { regtime = value; }
        }

        private int provinceid = Constants.Constant.INT_INVALID_VALUE;

        public int ProvinceID
        {
            get { return provinceid; }
            set { provinceid = value; }
        }

        private int cityid = Constants.Constant.INT_INVALID_VALUE;

        public int CityID
        {
            get { return cityid; }
            set { cityid = value; }
        }

        public string Sign { get; set; }

        public int IsExist { get; set; }//是否已经存在
        public int HisID { get; set; }
        public string CommonlyClassStr { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string OrderRemarkStr { get; set; }

        public bool IsAreaMedia { get; set; }
        public string AreaMapping { get; set; }

        public int UserId { get; set; }
    }
}