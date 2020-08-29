using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer
{
    public partial class ReceiveCopyLayer : PageBase
    {
        #region 定义属性
        public string RequestEmName
        {
            get
            {
                return BitAuto.ISDC.CC2012.BLL.Util.GetCurrentRequestStr("EmName");
            }
        }
        public int RequestGroupID
        {
            get
            {
                return BitAuto.ISDC.CC2012.BLL.Util.GetCurrentRequestInt("GroupID");
            }
        }
        public string RequestUserIDs
        {
            get
            {
                return BitAuto.ISDC.CC2012.BLL.Util.GetCurrentRequestStr("UserIDs");
            }
        }
        public string LimitSelectCount
        {
            get
            {
                return BitAuto.ISDC.CC2012.BLL.Util.GetCurrentRequestStr("LimitSelectCount");
            }
        }

        public int pageSize = 10;
        public int RecordCount;
        public int GroupLength = 4;
        public string SelectedUserStr = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        #region 员工列表绑定
        private void BindData()
        {
            DataTable dt = BLL.EmployeeSuper.Instance.GetSysRightUserByName(RequestEmName, "ui.UserID", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);

            RptEm.DataSource = dt;
            RptEm.DataBind();
            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 100);

            LoadSelectedEmployees();
        }
        #endregion


        #region 加载已选人员
        public void LoadSelectedEmployees()
        {
            SelectedUserStr = "";
            string EmployeesIDs = "";
            if (!string.IsNullOrEmpty(RequestUserIDs))
            {
                EmployeesIDs = RequestUserIDs; 
                if (EmployeesIDs.Trim() != "")
                {
                    List<SysRightUserInfo> list = BLL.EmployeeSuper.Instance.GetSysRightUserInfo(EmployeesIDs);
                    foreach (SysRightUserInfo userinfo in list)
                    {
                        string name = userinfo.TrueName;
                        string departName = userinfo.NamePath;
                          
                        SelectedUserStr += "<tr>"
                            + "<td><a id='" + userinfo.UserID + "' name='" + name + "' href='javascript:DelSelectCustBrand(\"" + userinfo.UserID + "\");\'>"
                                + "<img title='删除' src='/Images/close.png'></a></td>"
                            + "<td> " + name + "（" + userinfo.ADName + "）&nbsp;</td>"
                            + "<td> " + departName + " &nbsp;</td>"
                            + "</tr>";
                    }
                }
            }
        }
        #endregion
    }
}