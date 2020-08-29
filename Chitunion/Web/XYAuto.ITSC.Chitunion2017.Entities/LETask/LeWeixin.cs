using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //LE_Weixin
    public class LeWeixin
    {

        //主键
        public int RecID { get; set; }

        //应用ID
        public string AppID { get; set; }

        //钥
        public string AccessToken { get; set; }

        //刷新钥
        public string RefreshAccessToken { get; set; }

        //获取Token时间
        public DateTime GetTokenTime { get; set; }

        //微信号
        public string WxNumber { get; set; }

        //原始ID
        public string OriginalID { get; set; }

        //昵称
        public string NickName { get; set; }

        //公众号类型
        public int ServiceType { get; set; }

        //是否认证
        public bool IsVerify { get; set; }

        //认证类型
        public int VerifyType { get; set; }

        //头像地址
        public string HeadImg { get; set; }

        //二维码图片地址
        public string QrCodeUrl { get; set; }

        //粉丝数
        public int FansCount { get; set; }

        //Biz抓取用
        public string Biz { get; set; }

        //Status
        public int Status { get; set; }

        //当前授权状态
        public int OAuthStatus { get; set; }

        //公众号注册时间
        public DateTime RegTime { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //更新时间
        public DateTime ModifyTime { get; set; }

        //授权方式
        public int SourceType { get; set; }

        //简介
        public string Summary { get; set; }

        //全称\主体
        public string FullName { get; set; }

        //统一社会信用代码
        public string CreditCode { get; set; }

        //前置许可经营范围
        public string BusinessScope { get; set; }

        //企业类型
        public string EnterpriseType { get; set; }

        //企业成立日期
        public string EnterpriseCreateDate { get; set; }

        //企业营业期限
        public string EnterpriseBusinessTerm { get; set; }

        //企业认证日期
        public string EnterpriseVerifyDate { get; set; }

        //所在地
        public string Location { get; set; }

        //媒体级别
        public int LevelType { get; set; }

        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        //描述、签名
        public string Sign { get; set; }

        //是否是区域媒体
        public bool IsAreaMedia { get; set; }

        public int ReadNum { get; set; }

        //行业分类枚举ID
        public int CategoryID { get; set; }

        public bool IsOriginal { get; set; }

        public Byte[] TimestampSign { get; set; }

        public int CreateUserID { get; set; }

        public decimal ManFansRatio { get; set; }
        public decimal WomanFansRatio { get; set; }

        /*冗余*/
        public string PricesInfo { get; set; }
        public int MpProvinceId { get; set; }
        public int MpCityId { get; set; }
    }
}