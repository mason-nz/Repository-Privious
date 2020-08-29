using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using System.Data;
using BitAuto.Utils.Config;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.Web.CustCheck
{
    public class MemberCheckHelper
    {

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private string action;

        public string Action
        {
            get
            {
                if (action == null)
                {
                    action = HttpUtility.UrlDecode((Request["Action"] + "").Trim());
                }
                return action;
            }
        }
        #region 车易通信息
        private string memberId;
        /// <summary>
        /// 会员ID
        /// </summary>
        public string MemberID
        {
            get
            {
                if (memberId == null)
                {
                    memberId = HttpUtility.UrlDecode((Request["MemberID"] + "").Trim());
                }
                return memberId;
            }
        }
        private string taskId;

        private string name;
        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name
        {
            get
            {
                if (name == null)
                {
                    name = HttpUtility.UrlDecode((Request["MemberName"] + "").Trim());
                }
                return name;
            }
        }
        private string abbr;
        /// <summary>
        /// 会员简称
        /// </summary>
        public string Abbr
        {
            get
            {
                if (abbr == null)
                {
                    abbr = HttpUtility.UrlDecode((Request["MemberAbbr"] + "").Trim());
                }
                return abbr;
            }
        }
        private string phone;
        /// <summary>
        /// 会员电话
        /// </summary>
        public string Phone
        {
            get
            {
                if (phone == null)
                {
                    phone = HttpUtility.UrlDecode((Request["Phone"] + "").Trim());
                }
                return phone;
            }
        }
        private string fax;
        /// <summary>
        /// 会员传真
        /// </summary>
        public string Fax
        {
            get
            {
                if (fax == null)
                {
                    fax = HttpUtility.UrlDecode((Request["Fax"] + "").Trim());
                }
                return fax;
            }
        }
        private string companyWebSite;
        /// <summary>
        /// 会员传真
        /// </summary>
        public string CompanyWebSite
        {
            get
            {
                if (companyWebSite == null)
                {
                    companyWebSite = HttpUtility.UrlDecode((Request["CompanyWebSite"] + "").Trim());
                }
                return companyWebSite;
            }
        }
        private string email;
        /// <summary>
        /// 会员Email
        /// </summary>
        public string Email
        {
            get
            {
                if (email == null)
                {
                    email = HttpUtility.UrlDecode((Request["Email"] + "").Trim());
                }
                return email;
            }
        }
        private string postCode;
        /// <summary>
        /// 会员邮编
        /// </summary>
        public string PostCode
        {
            get
            {
                if (postCode == null)
                {
                    postCode = HttpUtility.UrlDecode((Request["PostCode"] + "").Trim());
                }
                return postCode;
            }
        }
        private string provinceId;
        /// <summary>
        /// 会员省份编号
        /// </summary>
        public string ProvinceId
        {
            get
            {
                if (provinceId == null)
                {
                    provinceId = HttpUtility.UrlDecode((Request["ProvinceId"] + "").Trim());
                }
                return provinceId;
            }
        }
        private string cityId;
        /// <summary>
        /// 会员城市编号
        /// </summary>
        public string CityID
        {
            get
            {
                if (cityId == null)
                {
                    cityId = HttpUtility.UrlDecode((Request["CityID"] + "").Trim());
                }
                return cityId;
            }
        }
        private string countyId;
        /// <summary>
        /// 会员乡镇id
        /// </summary>
        public string CountyId
        {
            get
            {
                if (countyId == null)
                {
                    countyId = HttpUtility.UrlDecode((Request["CountyId"] + "").Trim());
                }
                return countyId;
            }
        }
        private string contactAddress;
        /// <summary>
        /// 会员地址
        /// </summary>
        public string ContactAddress
        {
            get
            {
                if (contactAddress == null)
                {
                    contactAddress = HttpUtility.UrlDecode((Request["Address"] + "").Trim());
                }
                return contactAddress;
            }
        }
        private string longitude;
        /// <summary>
        /// 会员坐标
        /// </summary>
        public string Longitude
        {
            get
            {
                if (longitude == null)
                {
                    longitude = HttpUtility.UrlDecode((Request["Longitude"] + "").Trim());
                }
                return longitude;
            }
        }
        private string lantitude;
        /// <summary>
        /// 会员坐标
        /// </summary>
        public string Lantitude
        {
            get
            {
                if (lantitude == null)
                {
                    lantitude = HttpUtility.UrlDecode((Request["Lantitude"] + "").Trim());
                }
                return lantitude;
            }
        }
        private string trafficInfo;
        /// <summary>
        /// 会员
        /// </summary>
        public string TrafficInfo
        {
            get
            {
                if (trafficInfo == null)
                {
                    trafficInfo = HttpUtility.UrlDecode((Request["TrafficInfo"] + "").Trim());
                }
                return trafficInfo;
            }
        }
        private string brand;
        /// <summary>
        /// 会员
        /// </summary>
        public string Brand
        {
            get
            {
                if (brand == null)
                {
                    brand = HttpUtility.UrlDecode((Request["Brand"] + "").Trim());
                }
                return brand;
            }
        }

        private string serialIds;
        public string SerialIDs
        {
            get
            {
                if (serialIds == null)
                {
                    serialIds = HttpUtility.UrlDecode((Request["SerialIds"] + "").Trim());
                }
                return serialIds;
            }
        }

        private string enterpriseBrief;
        /// <summary>
        /// 会员
        /// </summary>
        public string EnterpriseBrief
        {
            get
            {
                if (enterpriseBrief == null)
                {
                    enterpriseBrief = HttpUtility.UrlDecode((Request["EnterpriseBrief"] + "").Trim());
                }
                return enterpriseBrief;
            }
        }
        private string remarks;
        /// <summary>
        /// 会员备注
        /// </summary>
        public string Remarks
        {
            get
            {
                if (remarks == null)
                {
                    remarks = HttpUtility.UrlDecode((Request["Notes"] + "").Trim());
                }
                return remarks;
            }
        }
        private string memberType;
        /// <summary>
        /// 会员备注
        /// </summary>
        public string MemberType
        {
            get
            {
                if (memberType == null)
                {
                    memberType = HttpUtility.UrlDecode((Request["MemberType"] + "").Trim());
                }
                return memberType;
            }
        }

        private string originalDMSMemberID;
        public string OriginalDMSMemberID
        {
            get
            {
                if (originalDMSMemberID == null)
                {
                    originalDMSMemberID = HttpUtility.UrlDecode((Request["OriginalDMSMemberID"] + "").Trim());
                }
                return originalDMSMemberID;
            }
        }

        private string originalCSTRecID;
        public string OriginalCSTRecID
        {
            get
            {
                if (originalCSTRecID == null)
                {
                    originalCSTRecID = HttpUtility.UrlDecode((Request["OriginalCSTRecID"] + "").Trim());
                }
                return originalCSTRecID;
            }
        }

        #endregion 车易通信息

        #region 车商通信息
        private string cstmemberId;
        /// <summary>
        /// 会员ID
        /// </summary>
        public string CstMemberID
        {
            get
            {
                if (cstmemberId == null)
                {
                    cstmemberId = HttpUtility.UrlDecode((Request["CstMemberID"] + "").Trim());
                }
                return cstmemberId;
            }
        }

        private string cstmemberfullname;
        /// <summary>
        /// 会员全称
        /// </summary>
        public string CstMemberFullName
        {
            get
            {
                if (cstmemberfullname == null)
                {
                    cstmemberfullname = HttpUtility.UrlDecode((Request["CstMemberFullName"] + "").Trim());
                }
                return cstmemberfullname;
            }
        }

        private string cstmembershortname;
        /// <summary>
        /// 会员简称
        /// </summary>
        public string CstMemberShortName
        {
            get
            {
                if (cstmembershortname == null)
                {
                    cstmembershortname = HttpUtility.UrlDecode((Request["CstMemberShortName"] + "").Trim());
                }
                return cstmembershortname;
            }
        }

        private string cstmembertype;
        /// <summary>
        /// 会员类型
        /// </summary>
        public string CstMemberType
        {
            get
            {
                if (cstmembertype == null)
                {
                    cstmembertype = HttpUtility.UrlDecode((Request["CstMemberType"] + "").Trim());
                }
                return cstmembertype;
            }
        }

        private string cstvendorcode;
        /// <summary>
        /// 会员编码
        /// </summary>
        public string CstVendorCode
        {
            get
            {
                if (cstvendorcode == null)
                {
                    cstvendorcode = HttpUtility.UrlDecode((Request["CstVendorCode"] + "").Trim());
                }
                return cstvendorcode;
            }
        }


        private string cstmemberprovince;
        /// <summary>
        /// 会员所属省份
        /// </summary>
        public string CstMemberProvince
        {
            get
            {
                if (cstmemberprovince == null)
                {
                    cstmemberprovince = HttpUtility.UrlDecode((Request["CstMemberProvince"] + "").Trim());
                }
                return cstmemberprovince;
            }
        }

        private string cstmembercity;
        /// <summary>
        /// 会员所属城市
        /// </summary>
        public string CstMemberCity
        {
            get
            {
                if (cstmembercity == null)
                {
                    cstmembercity = HttpUtility.UrlDecode((Request["CstMemberCity"] + "").Trim());
                }
                return cstmembercity;
            }
        }

        private string cstmembercounty;
        /// <summary>
        /// 会员所属县城
        /// </summary>
        public string CstMemberCounty
        {
            get
            {
                if (cstmembercounty == null)
                {
                    cstmembercounty = HttpUtility.UrlDecode((Request["CstMemberCounty"] + "").Trim());
                }
                return cstmembercounty;
            }
        }

        private string cstsuperid;
        /// <summary>
        /// 上级公司
        /// </summary>
        public string CstSuperId
        {
            get
            {
                if (cstsuperid == null)
                {
                    cstsuperid = HttpUtility.UrlDecode((Request["CstSuperId"] + "").Trim());
                }
                return cstsuperid;
            }
        }

        private string cstmemberaddress;
        /// <summary>
        /// 地址
        /// </summary>
        public string CstMemberAddress
        {
            get
            {
                if (cstmemberaddress == null)
                {
                    cstmemberaddress = HttpUtility.UrlDecode((Request["CstMemberAddress"] + "").Trim());
                }
                return cstmemberaddress;
            }
        }

        private string cstmemberpostcode;
        /// <summary>
        /// 邮编
        /// </summary>
        public string CstMemberPostCode
        {
            get
            {
                if (cstmemberpostcode == null)
                {
                    cstmemberpostcode = HttpUtility.UrlDecode((Request["CstMemberPostCode"] + "").Trim());
                }
                return cstmemberpostcode;
            }
        }

        private string csttrafficinfo;
        /// <summary>
        /// 交通信息
        /// </summary>
        public string CstTrafficInfo
        {
            get
            {
                if (csttrafficinfo == null)
                {
                    csttrafficinfo = HttpUtility.UrlDecode((Request["CstTrafficInfo"] + "").Trim());
                }
                return csttrafficinfo;
            }
        }

        private string cstmemberbrand;
        /// <summary>
        /// 主营品牌
        /// </summary>
        public string CstMemberBrand
        {
            get
            {
                if (cstmemberbrand == null)
                {
                    cstmemberbrand = HttpUtility.UrlDecode((Request["CstMemberBrand"] + "").Trim());
                }
                return cstmemberbrand;
            }
        }

        private string cstlinkman;
        /// <summary>
        /// 联系人
        /// </summary>
        public string CstLinkMan
        {
            get
            {
                if (cstlinkman == null)
                {
                    cstlinkman = HttpUtility.UrlDecode((Request["CstLinkMan"] + "").Trim());
                }
                return cstlinkman;
            }
        }

        private string cstlinkmanname;
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string CstLinkManName
        {
            get
            {
                if (cstlinkmanname == null)
                {
                    cstlinkmanname = HttpUtility.UrlDecode((Request["CstLinkManName"] + "").Trim());
                }
                return cstlinkmanname;
            }
        }

        private string cstlinkmandepartment;
        /// <summary>
        /// 部门
        /// </summary>
        public string CstLinkManDepartment
        {
            get
            {
                if (cstlinkmandepartment == null)
                {
                    cstlinkmandepartment = HttpUtility.UrlDecode((Request["CstLinkManDepartment"] + "").Trim());
                }
                return cstlinkmandepartment;
            }
        }

        private string cstlinkmanposition;
        /// <summary>
        /// 职务
        /// </summary>
        public string CstLinkManPosition
        {
            get
            {
                if (cstlinkmanposition == null)
                {
                    cstlinkmanposition = HttpUtility.UrlDecode((Request["CstLinkManPosition"] + "").Trim());
                }
                return cstlinkmanposition;
            }
        }

        private string cstlinkmanmobile;
        /// <summary>
        /// 手机
        /// </summary>
        public string CstLinkManMobile
        {
            get
            {
                if (cstlinkmanmobile == null)
                {
                    cstlinkmanmobile = HttpUtility.UrlDecode((Request["CstLinkManMobile"] + "").Trim());
                }
                return cstlinkmanmobile;
            }
        }

        private string cstlinkmanemail;
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string CstLinkManEmail
        {
            get
            {
                if (cstlinkmanemail == null)
                {
                    cstlinkmanemail = HttpUtility.UrlDecode((Request["CstLinkManEmail"] + "").Trim());
                }
                return cstlinkmanemail;
            }
        }

        #endregion 车商通信息

        public void Validate(bool whenSubmit, out Entities.ProjectTask_DMSMember member)
        {
            Regex reMemberPhoneAndFax = new Regex(@"(^0[0-9]{2,3}-[0-9]{7,8}$)|(^0[0-9]{2,3}-[0-9]{7,8}-[0-9]{1,5}$)|(^13[0-9]{9}$)|(^15[0-9]{9}$)|(^18[0-9]{9}$)|(^400\d{7}$)");
            int id = -1;
            if (MemberID != "")
            {
                if (!int.TryParse(MemberID, out id))
                {
                    throw new Exception("会员ID无法转换为int类型");
                }
                member = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMember(int.Parse(MemberID));
                member.MemberID = int.Parse(MemberID); ;

                if (string.IsNullOrEmpty(Name))
                {
                    throw new Exception("会员名称不能为空");
                }
                if (Name.Length > 256)
                {
                    throw new Exception("会员名称超长");
                }
                member.Name = Name;
                if (string.IsNullOrEmpty(Abbr))
                {
                    throw new Exception("会员简称不可为空");
                }
                member.Abbr = Abbr;
                if (BLL.Util.GetLength(abbr) > 14) { throw new Exception("会员简称不能超过14个字符"); }//Modify=Masj,Date=2012-08-28
                if (whenSubmit)
                {
                    if (string.IsNullOrEmpty(member.OriginalDMSMemberID) == false)
                    {

                        if (BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.IsExistsByAbbrName(member.OriginalDMSMemberID, Abbr,1))
                        {
                            BitAuto.YanFa.Crm2009.Entities.QueryDMSMember query = new BitAuto.YanFa.Crm2009.Entities.QueryDMSMember();
                            query.Abbr = Abbr;
                            int total = 0;
                            DataTable dt = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberAndCust(query, "", 1, 1000000, out total);
                            string msg = string.Empty;
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                msg += "客户名称：" + dt.Rows[j]["CustName"].ToString() + ";会员全称：" + dt.Rows[j]["Name"].ToString() + ";会员简称" + dt.Rows[j]["Abbr"].ToString() + "<br>";
                                if (j < dt.Rows.Count)
                                {
                                    msg += "--------------------------------";
                                }
                            }
                            throw new Exception("已存在此会员简称<br>" + msg);
                        }
                    }
                }
                if (whenSubmit && string.IsNullOrEmpty(MemberType)) { throw new Exception("会员类型不能为空"); }
                member.MemberType = MemberType;

                if (Phone.Length > 256) { throw new Exception("会员电话超长"); }
                if (!string.IsNullOrEmpty(Phone) &&
                        Phone.Split(',').Length > 0)
                {
                    foreach (string phone in Phone.Split(','))
                    {
                        if ((!string.IsNullOrEmpty(phone)) && (!reMemberPhoneAndFax.IsMatch(phone)))
                        {
                            throw new Exception("会员电话格式不正确，<br/>固话：010-68492345 010-68492345-8217 手机：直接输入13245687921 特殊号码：4000689960");
                        }
                    }
                }
                member.Phone = Phone.TrimEnd(',').TrimStart(',');

                //member.BrandIDs = Brand;

                if (Fax.Length > 256) { throw new Exception("会员传真超长"); }
                if (!string.IsNullOrEmpty(Fax) &&
                    Fax.Split(',').Length > 0)
                {
                    foreach (string fax in Fax.Split(','))
                    {
                        if ((!string.IsNullOrEmpty(fax)) && (!reMemberPhoneAndFax.IsMatch(fax)))
                        {
                            throw new Exception("会员传真格式不正确，<br/>固话：010-68492345 010-68492345-8217 手机：直接输入13245687921 特殊号码：4000689960");
                        }
                    }
                }
                member.Fax = Fax.TrimEnd(',').TrimStart(',');

                if (CompanyWebSite.Length > 256) { throw new Exception("会员公司网址超长"); }
                member.CompanyWebSite = CompanyWebSite;

                if (Email.Length > 256) { throw new Exception("会员Email超长"); }
                member.Email = Email;

                if (PostCode.Length > 32) { throw new Exception("会员邮编超长"); }
                member.Postcode = PostCode;

                if (whenSubmit && string.IsNullOrEmpty(Brand)) { throw new Exception("主营品牌必填"); }
                member.BrandIDs = Brand;
                int i = -1;
                if (string.IsNullOrEmpty(ProvinceId) == false && int.TryParse(ProvinceId, out i) == false) { throw new Exception("ProvinceID无法转换成int类型"); }
                if (whenSubmit && i <= 0) { throw new Exception("会员省份必填"); }
                member.ProvinceID = ProvinceId;

                if (string.IsNullOrEmpty(CityID) == false && int.TryParse(CityID, out i) == false) { throw new Exception("CityID无法转换成int类型"); }
                if (whenSubmit && i <= 0) { throw new Exception("会员城市必填"); }
                member.CityID = CityID;

                if (string.IsNullOrEmpty(CountyId) == false && int.TryParse(CountyId, out i) == false) { throw new Exception("CountyID无法转换成int类型"); }

                member.CountyID = CountyId;

                if (ContactAddress.Length > 256) { throw new Exception("会员地址超长"); }
                if (whenSubmit && string.IsNullOrEmpty(contactAddress)) { throw new Exception("会员地址必填"); }
                member.ContactAddress = contactAddress;

                float l = -1;
                if (string.IsNullOrEmpty(Longitude) == false && float.TryParse(Longitude, out l) == false) { throw new Exception("Longitude无法转换成float类型"); }
                if (whenSubmit && l <= 0) { throw new Exception("地图标点必填"); }
                member.Longitude = Longitude;

                if (string.IsNullOrEmpty(Lantitude) == false && float.TryParse(Lantitude, out l) == false) { throw new Exception("Lantitude无法转换成float类型"); }
                if (whenSubmit && l <= 0) { throw new Exception("地图标点必填"); }
                member.Lantitude = Lantitude;

                //member.SerialIds = SerialIds;

                if (TrafficInfo.Length > 1024) { throw new Exception("会员交通信息超长"); }
                member.TrafficInfo = TrafficInfo;

                if (EnterpriseBrief.Length > 1024) { throw new Exception("会员企业简介超长"); }
                member.EnterpriseBrief = EnterpriseBrief;

                if (Remarks.Length > 1024) { throw new Exception("会员备注超长"); }
                member.Remarks = Remarks;

                member.SerialIds = SerialIDs;
            }
            else
            { member = null; }
        }

        //验证车商通信息
        public void ValidateCSTMember(bool whenSubmit, out BitAuto.YanFa.Crm2009.Entities.CstMember member, out string Brands, out BitAuto.YanFa.Crm2009.Entities.CSTLinkMan modelLinkMan)
        {
            Regex reMemberPhoneAndFax = new Regex(@"(^0[0-9]{2,3}-[0-9]{7,8}$)|(^0[0-9]{2,3}-[0-9]{7,8}-[0-9]{1,5}$)|(^13[0-9]{9}$)|(^15[0-9]{9}$)|(^18[0-9]{9}$)|(^400\d{7}$)");
            //Regex regexMobile = new Regex(@"^[1]+[3,5,8]+\d{9}");


            if (OriginalCSTRecID != string.Empty)
            {
                //判断基本条件填写是否无误
                if (string.IsNullOrEmpty(CstMemberFullName))
                {
                    throw new Exception("会员名称不能为空！");
                }
                else if (CstMemberFullName.Length > 50)
                {
                    throw new Exception("会员名称超长！");
                }

                if (string.IsNullOrEmpty(CstMemberShortName))
                {
                    throw new Exception("会员简称不可为空！");
                }
                else if (BLL.Util.GetLength(CstMemberShortName) > 16)//Modify=Masj,Date=2012-08-28
                {
                    throw new Exception("会员简称不能超过16个字符！");
                }

                //if (CstVendorCode.Length == 0)
                //{
                //    throw new Exception("会员编码不能为空！");
                //}
                //else if (CstVendorCode.Length > 8)
                //{
                //    throw new Exception("会员编码不能超过8个字符！");
                //}

                //if (CstMemberPostCode.Length == 0)
                //{
                //    throw new Exception(CstMemberFullName + ",会员邮编不能为空！");
                //}
                //else if (CstMemberPostCode.Length > 6)
                //{
                //    throw new Exception(CstMemberFullName + ",会员邮编填写错误！");
                //}

                if (int.Parse(CstSuperId) <= 0)
                {
                    throw new Exception(CstMemberFullName + ",请选择上级公司！");
                }
                if (CstMemberType == "-1")
                {
                    throw new Exception(CstMemberFullName + ",会员类型不能为空！");
                }
                if (CstMemberProvince == "-1" || CstMemberCity == "-1" || CstMemberCounty == "-1")
                {
                    throw new Exception(CstMemberFullName + ",所在地区填写不完整！");
                }
                if (CstLinkManName == "")
                {
                    throw new Exception("请填写车商通联系人名字！");
                }
                if (CstLinkManMobile == "")
                {
                    throw new Exception("请填写车商通联系人手机号！");
                }
                if (!BLL.Util.IsHandset(CstLinkManMobile))
                {
                    throw new Exception("车商通联系人手机号码格式不正确 ");
                }
                Regex MyRegex1 = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                if (CstLinkManEmail == "")
                {
                    throw new Exception("请填写车商通联系人电子邮件！");
                }
                if (!MyRegex1.IsMatch(CstLinkManEmail))
                {
                    throw new Exception("车商通联系人电子邮件格式不正确 ");
                }

                int id = -1;
                if (!int.TryParse(CstMemberID, out id))
                {
                    throw new Exception("会员ID无法转换为int类型");
                }
                member = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(OriginalCSTRecID);
                Brands = "";
                //modelBrand = Crm2009.BLL.CSTMember_Brand.Instance.GetModelByCSTRecID(OriginalCSTRecID);

                modelLinkMan = BitAuto.YanFa.Crm2009.BLL.CSTLinkMan.Instance.GetModelByCSTRecID(OriginalCSTRecID);
                if (modelLinkMan == null)
                {
                    throw new Exception("车商通会员无联系人信息，无法操作");
                }

                member.ID = int.Parse(CstMemberID);

                member.FullName = CstMemberFullName;

                member.ShortName = CstMemberShortName;

                //对crm库检查会员简称、会员名称、会员编码是否重复
                #region 查重名
                if (whenSubmit)//提交时判断重名
                {
                    //对cc库检查会员简称、会员名称、会员编码是否重复 12.6.7 lxw
                    string where1 = " AND FullName='" + Utils.StringHelper.SqlFilter(CstMemberFullName) + "' and Status=0 and OriginalCSTRecID!='" + StringHelper.SqlFilter(OriginalCSTRecID) + "'";
                    if (BLL.ProjectTask_CSTMember.Instance.IsExistSameName(where1) == true)
                    {
                        throw new Exception(CstMemberFullName + ",cc库中有重名的会员名称！");
                    }

                    string where2 = " AND ShortName='" + Utils.StringHelper.SqlFilter(CstMemberShortName) + "' and Status=0 and OriginalCSTRecID!='" + StringHelper.SqlFilter(OriginalCSTRecID) + "'";
                    if (BLL.ProjectTask_CSTMember.Instance.IsExistSameName(where2) == true)
                    {
                        throw new Exception(CstMemberShortName + ",cc库中有重名的会员简称！");
                    }

                    string where3 = " AND VendorCode='" + Utils.StringHelper.SqlFilter(CstVendorCode) + "' and Status=0 and OriginalCSTRecID!='" + StringHelper.SqlFilter(OriginalCSTRecID) + "'";
                    if (BLL.ProjectTask_CSTMember.Instance.IsExistSameName(where3) == true)
                    {
                        throw new Exception(CstVendorCode + ",cc库中有重名的会员编码！");
                    }

                    //是否存在重名的会员名称 
                    if (OriginalCSTRecID != string.Empty)
                    {
                        string where11 = " FullName='" + Utils.StringHelper.SqlFilter(CstMemberFullName) + "' and CSTRecID!='" + StringHelper.SqlFilter(OriginalCSTRecID) + "'";
                        if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where11) > 0)
                        {
                            throw new Exception(CstMemberFullName + ",crm库中有重名的会员名称！");
                        }
                    }
                    //是否存在重名的会员简称
                    if (OriginalCSTRecID != string.Empty)
                    {
                        string where12 = " ShortName='" + Utils.StringHelper.SqlFilter(CstMemberShortName) + "' and CSTRecID!='" + OriginalCSTRecID + "'";
                        if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where12) > 0)
                        {
                            throw new Exception(CstMemberShortName + ",crm库中有重名的会员简称！");
                        }
                    }
                }
                #endregion 查重名

                member.PostCode = CstMemberPostCode;

                member.SuperId = int.Parse(CstSuperId);

                member.VendorCode = CstVendorCode;

                member.VendorClass = int.Parse(CstMemberType);

                member.ProvinceID = CstMemberProvince;
                member.CityID = CstMemberCity;
                member.CountyID = CstMemberCounty;

                if (CstMemberAddress.Length != 0)
                {
                    member.Address = CstMemberAddress;
                }

                //交通信息 12.6.4注释
                //if (CstTrafficInfo.Length != 0)
                //{
                //    member.TrafficInfo = CstTrafficInfo;
                //}

                //主营品牌信息 12.6.4注释
                //if (CstMemberType == "20003" & CstMemberBrand.Length == 0)
                //{
                //    throw new Exception(CstMemberFullName + ",至少选择一个主营品牌！");
                //}
                //else
                //{
                //    Brands = CstMemberBrand;
                //}
                Brands = "";

                //联系人信息
                //12.6.4注释
                //if (CstLinkManDepartment == "")
                //{
                //    throw new Exception("请填写车商通联系人部门！");
                //}
                //modelLinkMan.Department = CstLinkManDepartment;

                modelLinkMan.Name = CstLinkManName;

                //12.6.4注释
                //if (CstLinkManPosition == "")
                //{
                //    throw new Exception("请填写车商通联系人职务！");
                //}
                //modelLinkMan.Position = CstLinkManPosition;

                modelLinkMan.Mobile = CstLinkManMobile;

                modelLinkMan.Email = CstLinkManEmail;
            }
            else
            {
                member = null;
                Brands = "";
                modelLinkMan = null;
            }
        }

        public void SaveMemberInfo()
        {
            //保存车易通信息
            Entities.ProjectTask_DMSMember member = null;
            Validate(false, out member);
            if (member != null)
            {
                BLL.ProjectTask_DMSMember.Instance.InsertOrUpdate(member);
            }

            //保存车商通信息
            BitAuto.YanFa.Crm2009.Entities.CstMember member1 = null;
            string Brands = "";
            //Crm2009.Entities.CSTMember_Brand modelBrand = null;
            BitAuto.YanFa.Crm2009.Entities.CSTLinkMan modelLinkMan = null;
            ValidateCSTMember(false, out member1, out Brands, out modelLinkMan);
            if (member1 != null)
            {
                BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.UpdateCstMember(member1);
            }
            //主营品牌 12.6.4注释
            //if (Brands != "")
            //{
            //    string[] brandID = Brands.Split(',');
            //    Crm2009.BLL.CSTMember_Brand.Instance.DeleteByCSTRecID(member1.CSTRecID);
            //    for (int i = 0; i < brandID.Length; i++)
            //    {
            //        if (brandID[i] != "")
            //        {
            //            Crm2009.Entities.CSTMember_Brand modelBrand = new Crm2009.Entities.CSTMember_Brand();
            //            modelBrand.CSTRecID = member1.CSTRecID;
            //            modelBrand.CreateTime = DateTime.Now;
            //            modelBrand.BrandID = int.Parse(brandID[i]);
            //            Crm2009.BLL.CSTMember_Brand.Instance.Add(modelBrand);
            //        }
            //    }
            //}
            if (modelLinkMan != null)
            {
                BitAuto.YanFa.Crm2009.BLL.CSTLinkMan.Instance.Update(modelLinkMan);
            }
        }


        public void CheckUserAuthForMember()
        {
            //Crm2009.Entities.DMSMember member = null;
            //Crm2009.Entities.CstMember cstMember = null;
            //if (OriginalDMSMemberID != "")
            //{
            //    Guid id = new Guid(OriginalDMSMemberID);
            //    member = Crm2009.BLL.DMSMember.Instance.GetDMSMember(id);
            //}
            //if (OriginalCSTRecID != "")
            //{
            //    cstMember = Crm2009.BLL.CstMember.Instance.GetCSTMember(OriginalCSTRecID);
            //}

            //if (member == null && cstMember == null)
            //{
            //    throw new Exception("此会员不存在！");
            //}
            //else
            //{
            //    if (!BLL.CC_UserCustDataRigth.Instance.IsManager(BLL.Util.GetLoginUserID()))
            //    {
            //        if (member.ApplyUserID != BLL.Util.GetLoginUserID())
            //        {
            //            throw new Exception("你没有权限访问此会员！");
            //        }
            //    }
            //}
        }


        public void SubmitMemberInfo()
        {
            //车易通信息提交
            Entities.ProjectTask_DMSMember member = null;
            Validate(true, out member);
            if (member != null)
            {
                if (BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.IsExistsByAbbrName(member.OriginalDMSMemberID, member.Abbr,1))
                {
                    throw new Exception("此会员简称在Crm库中已存在");
                }
                else
                {
                    Guid memberGuid = new Guid(member.OriginalDMSMemberID);
                    BitAuto.YanFa.Crm2009.Entities.DMSMember dmsMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(memberGuid);
                    dmsMember.Abbr = member.Abbr;
                    dmsMember.BrandGroupID = member.BrandGroupID;
                    dmsMember.BrandIds = member.BrandIDs;
                    dmsMember.BrandNames = member.BrandNames;
                    dmsMember.CityID = member.CityID;
                    dmsMember.CompanyWebSite = member.CompanyWebSite;
                    dmsMember.ContactAddress = member.ContactAddress;
                    dmsMember.CountyID = member.CountyID;
                    dmsMember.Email = member.Email;
                    dmsMember.EnterpriseBrief = member.EnterpriseBrief;
                    dmsMember.Fax = member.Fax;
                    dmsMember.TrafficInfo = member.TrafficInfo;
                    dmsMember.Remarks = member.Remarks;
                    dmsMember.ProvinceID = member.ProvinceID;
                    dmsMember.Postcode = member.Postcode;
                    dmsMember.Phone = member.Phone;
                    dmsMember.Name = member.Name;
                    dmsMember.MemberType = member.MemberType;
                    dmsMember.Remarks = member.Remarks;
                    dmsMember.SerialIds = member.SerialIds;
                    dmsMember.SerialNames = member.SerialIds;

                    dmsMember.MapCoordinateList.Add(new BitAuto.YanFa.Crm2009.Entities.DMSMapCoordinate(
                                memberGuid, BitAuto.YanFa.Crm2009.Entities.Constants.Constant.MapProviderName,
                                member.Longitude, member.Lantitude)
                            );
                    if (dmsMember.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.Rejected)
                    {
                        string content = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetUpdateContent(dmsMember, "{0}由【{1}】修改为【{2}】", '，');
                        content = string.Format("为【{0}(ID:{1})】修改会员【{2}(ID:{3})】{4}。",
                            dmsMember.CustName, dmsMember.CustID, member.Name, member.MemberCode,
                            content.Length > 0 ? "：" + content : string.Empty);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("MemberLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
                        BLL.ProjectTask_DMSMember.Instance.InsertOrUpdate(member);//保存cc会员信息
                        dmsMember.SyncStatus = (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.ApplyFor;
                        BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.Update(dmsMember);//保存crm会员信息
                        BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.InsertSyncLog(memberGuid, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.ApplyFor, "申请成功（呼叫中心二次审核）", BLL.Util.GetLoginUserID(), DateTime.Now);

                    }
                    else
                    {
                        throw new Exception("此会员不可提交到crm中,因为此会员不是打回状态");
                    }
                }
            }

            //车商通信息提交 
            BitAuto.YanFa.Crm2009.Entities.CstMember cstMember = null;
            string Brands = "";
            //Crm2009.Entities.CSTMember_Brand modelBrand = null;
            BitAuto.YanFa.Crm2009.Entities.CSTLinkMan cstLinkMan = null;
            ValidateCSTMember(true, out cstMember, out Brands, out cstLinkMan);


            if (cstMember != null)
            {
                if (cstMember.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.Rejected)
                {
                    //当CstMemberID字段有值时，更新交通信息的接口 12.6.4注释
                    //if (cstMember.CstMemberID != Entities.Constants.Constant.INT_INVALID_VALUE && cstMember.CstMemberID != 0)
                    //{
                    //    //更新车商通信息接口 
                    //    string msg = "";
                    //    int[] arrBrand = null;
                    //    //主营品牌
                    //if (Brands != "")
                    //{
                    //    string[] ids = Brands.Split(',');
                    //    arrBrand = new int[ids.Length];
                    //    for (int i = 0; i < ids.Length; i++)
                    //    {
                    //        arrBrand[i] = Convert.ToInt32(ids[i].ToString());
                    //    }
                    //}
                    //       bool result = BitAuto.YanFa.DMSInterface.CstMemberServiceHandler.UpdateTranstarUserInfo(cstMember.CstMemberID, cstMember.FullName, cstMember.ShortName,
                    //cstMember.VendorClass, cstMember.SuperId, cstMember.ProvinceID, cstMember.CityID, out msg);
                    //if (result == false)
                    //{
                    //    throw new Exception(msg);
                    //}
                    //}

                    string content = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetUpdateContent(cstMember, cstLinkMan, "{0}由【{1}】修改为【{2}】", '，');
                    content = string.Format("为【{0}(ID:{1})】修改会员【{2}(ID:{3})】{4}。",
                        cstMember.CustName, cstMember.CustID, cstMember.FullName, cstMember.VendorCode,
                        content.Length > 0 ? "：" + content : string.Empty);
                    BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("MemberLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);

                    cstMember.SyncStatus = (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.ApplyFor;
                    BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.UpdateCstMember(cstMember);//保存crm车商通会员信息 

                    //插入开通日志
                    cstMember.CreateTime = DateTime.Now;
                    BitAuto.YanFa.Crm2009.BLL.CSTMemberSyncLog.Instance.AddSynclog(cstMember.CSTRecID, cstMember.SyncStatus, "申请成功(呼叫中心二次审核)", cstMember.CreateUserID, cstMember.CreateTime);

                    //更新Crm车商通主营品牌信息 12.6.4注释
                    //if (Brands != "")
                    //{
                    //    string[] brandID = Brands.Split(',');
                    //    Crm2009.BLL.CSTMember_Brand.Instance.DeleteByCSTRecID(cstMember.CSTRecID);
                    //    for (int i = 0; i < brandID.Length; i++)
                    //    {
                    //        if (brandID[i] != "")
                    //        {
                    //            Crm2009.Entities.CSTMember_Brand modelBrand = new Crm2009.Entities.CSTMember_Brand();
                    //            modelBrand.CSTRecID = cstMember.CSTRecID;
                    //            modelBrand.CreateTime = DateTime.Now;
                    //            modelBrand.BrandID = int.Parse(brandID[i]);
                    //            Crm2009.BLL.CSTMember_Brand.Instance.Add(modelBrand);
                    //        }
                    //    }
                    //}

                    BitAuto.YanFa.Crm2009.BLL.CSTLinkMan.Instance.Update(cstLinkMan);//保存crm车商通会员联系人信息 
                }
                else
                {
                    throw new Exception("此车商通会员不可提交到crm中,因为此车商通会员不是打回状态");
                }
            }
        }

        public void DeleteMemberInfo()
        {
            int id = -1;
            if (MemberID != "")
            {
                if (int.TryParse(MemberID, out id))
                {
                    Entities.ProjectTask_DMSMember member = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMember(id);
                    if (member != null)
                    {
                        BitAuto.YanFa.Crm2009.Entities.DMSMember dmsMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(member.OriginalDMSMemberID));
                        if (dmsMember != null)
                        {
                            if (dmsMember.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.Rejected)
                            {
                                BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.Delete(dmsMember.ID.ToString());
                                BLL.ProjectTask_DMSMember.Instance.ProjectTask_DMSMemberDelete(id);
                                string content = string.Format("为【{0}(ID:{1})】删除会员【{2}(ID:{3})】成功。", dmsMember.CustName, dmsMember.CustID,
                            dmsMember.Name, dmsMember.MemberCode);
                                BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("MemberLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Delete, content);
                            }
                            else
                            {
                                throw new Exception("只有打回的会员才能删除！");
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("会员ID无法转换为int类型");
                }
            }

            //车商通会员删除 
            if (OriginalCSTRecID != string.Empty)
            {
                BitAuto.YanFa.Crm2009.Entities.CstMember cstMember = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(OriginalCSTRecID);
                int ID = BLL.ProjectTask_CSTMember.Instance.GetIDByCSTRecID(OriginalCSTRecID);
                if (cstMember != null)
                {
                    if (cstMember.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.Rejected)
                    {
                        BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.Delete(cstMember.ID.ToString());//删除CRM车商通会员信息
                        BLL.ProjectTask_CSTMember.Instance.ProjectTask_CSTMemberDelete(ID);//删除CC车商通会员信息
                        string content = string.Format("为【{0}(ID:{1})】删除会员【{2}(ID:{3})】成功。", cstMember.CustName, cstMember.CustID,
                    cstMember.FullName, cstMember.VendorCode);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("MemberLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Delete, content);
                    }
                    else
                    {
                        throw new Exception("只有打回的会员才能删除！");
                    }
                }
                else
                {
                    throw new Exception("删除失败");
                }
            }
        }
    }
}