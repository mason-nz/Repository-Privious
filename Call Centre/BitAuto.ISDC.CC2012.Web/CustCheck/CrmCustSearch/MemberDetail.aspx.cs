using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.CustInfo;

namespace BitAuto.ISDC.CC2012.Web.CustCheck.CrmCustSearch
{
    public partial class MemberDetail : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    userID = BLL.Util.GetLoginUserID();

                    CustInfoHelper custInfoHelper = new CustInfoHelper();
                    this.UCMemberDetail1.MemberInfo = custInfoHelper.GetOneMemberFromCRM();

                    //功能点验证逻辑
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2201"))//客户联系人
                    {
                        this.UCMemberDetail1.ListOfContact = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2202"))//合作项
                    {
                        this.UCMemberDetail1.ListOfCooperationProjects = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2203"))//负责员工
                    {
                        this.UCMemberDetail1.ListOfCustUser = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2204"))//访问记录
                    {
                        this.UCMemberDetail1.ListOfReturnVisit = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2205"))//年检记录
                    {
                        this.UCMemberDetail1.ListOfBusinessLicense = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2206"))//二手车规模
                    {
                        this.UCMemberDetail1.ListOfBusinessScaleInfo = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2207"))//任务记录
                    {
                        this.UCMemberDetail1.ListOfTaskRecord = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2208"))//品牌授权书
                    {
                        this.UCMemberDetail1.ListOfBusinessBrandLicense = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2213"))//工单
                    {
                        this.UCMemberDetail1.ListOfWorkOrder = true;
                    }


                    this.UCMemberDetail1.MemberInfoHref = "/CustCheck/CrmCustSearch/MemberDetail.aspx";
                    this.UCMemberDetail1.CustInfoHref = "/CustCheck/CrmCustSearch/CustDetail.aspx";
                }
            }
            catch (Exception ex)
            {
                //日志
            }
        }
    }
}