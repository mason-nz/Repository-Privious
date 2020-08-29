using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers
{
    public partial class YTGActivityTaskActivityInfo : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string ActivityID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ActivityID").ToString();
            }
        }
        public YTGActivityInfo model = new YTGActivityInfo();
        public string StatusName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                model = BLL.YTGActivity.Instance.GetComAdoInfo<YTGActivityInfo>(ActivityID);

                if (model.Status.HasValue)
                {
                   StatusName = getStatusName(model.Status.Value.ToString());
                }
            }
        }

        //获取活动状态名称
        public string getStatusName(string status)
        {
            string name = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                name = BLL.Util.GetEnumOptText(typeof(Entities.YTGActivityStatus), _status);
            }
            return name;
        }
 /// <summary>
        /// 获取意向车型名称
        /// </summary>
        /// <param name="ActionID"></param>
        protected string GetYiXiangCheXing()
        {

            List<Entities.CarSerial> list = BLL.YTGActivity.Instance.GetCarSerialsByIds(model.CarSerials.ToString());
            string strNames = "";
            foreach (CarSerial node in list)
            {
                strNames += "，" + node.Name;
            }
            if (strNames !="")
            {
                strNames = strNames.Substring(1);
            }
            return strNames;
        }
    }
}