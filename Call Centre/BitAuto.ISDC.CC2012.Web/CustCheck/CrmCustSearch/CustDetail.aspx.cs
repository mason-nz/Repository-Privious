using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.CustInfo;

namespace BitAuto.ISDC.CC2012.Web.CustCheck.CrmCustSearch
{
    public partial class CustDetail : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int userID =0;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    userID = BLL.Util.GetLoginUserID(); 

                    CustInfoHelper custInfoHelper = new CustInfoHelper();
                    this.UCCustDetail1.CustInfo = custInfoHelper.GetOneCustFromCRM();

                    //功能点验证逻辑
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2201"))//客户联系人
                    {
                        this.UCCustDetail1.ListOfContact = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2202"))//合作项
                    {
                        this.UCCustDetail1.ListOfCooperationProjects = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2203"))//负责员工
                    {
                        this.UCCustDetail1.ListOfCustUser = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2204"))//访问记录
                    {
                        this.UCCustDetail1.ListOfReturnVisit = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2205"))//年检记录
                    {
                        this.UCCustDetail1.ListOfBusinessLicense = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2206"))//二手车规模
                    {
                        this.UCCustDetail1.ListOfBusinessScaleInfo = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2207"))//任务记录
                    {
                        this.UCCustDetail1.ListOfTaskRecord = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2208"))//品牌授权书
                    {
                        this.UCCustDetail1.ListOfBusinessBrandLicense = true;
                    }
                    if (BLL.Util.CheckRight(userID, "SYS024BUT2213"))//品牌授权书
                    {
                        this.UCCustDetail1.ListOfWorkOrder = true;
                    }
                    this.UCCustDetail1.MemberInfoHref = "/CustCheck/CrmCustSearch/MemberDetail.aspx";
                }
            }
            catch (Exception ex)
            {
                //日志
            }
        }
    }
}
