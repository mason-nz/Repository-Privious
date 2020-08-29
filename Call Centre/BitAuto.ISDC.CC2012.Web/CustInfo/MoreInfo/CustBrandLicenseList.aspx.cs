using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class CustBrandLicenseList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region Properties
        public string CustID
        {
            get { return Request["CustID"] + ""; }
        }

        public string DataSource
        {
            get { return Request["DataSource"] + ""; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //加入 品牌授权书 页面权限
                //int userId = BLL.Util.GetLoginUserID();
                //if (!BLL.Util.CheckRight(userId, "SYS024BUT2208"))
                //{
                //    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                //    Response.End();
                //}
            }
            //查询
            DataTable table = new DataTable();

            BitAuto.YanFa.Crm2009.Entities.QueryCustBrandLicense query = new BitAuto.YanFa.Crm2009.Entities.QueryCustBrandLicense();
            query.CustID = this.CustID;

            table = BitAuto.YanFa.Crm2009.BLL.CustBrandLicense.Instance.GetTable(query);
            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater.DataSource = table;
            }
            //绑定列表数据
            repeater.DataBind();
        }

        protected string GetFile(string id)
        {
            string fileUrl = "";
            BitAuto.YanFa.Crm2009.Entities.QueryAttachFile query = new YanFa.Crm2009.Entities.QueryAttachFile();
            query.RelevantID = int.Parse(id);
            query.Type = BitAuto.YanFa.Crm2009.Entities.AttachFileType.CustBrandLicense;
            IList<YanFa.Crm2009.Entities.AttachFile> fileList = BitAuto.YanFa.Crm2009.BLL.AttachFile.Instance.Get(query);
            if (fileList != null && fileList.Count > 0)
            {
                fileUrl = fileList[0].FilePath;
            }

            return fileUrl;
        }

        //获取姓名
        public string getUserName(string userID)
        {
            string name = string.Empty;

            int _id;
            if (int.TryParse(userID, out _id))
            {
                name = BLL.Util.GetNameInHRLimitEID(_id);
            }
            return name;
        }
    }
}