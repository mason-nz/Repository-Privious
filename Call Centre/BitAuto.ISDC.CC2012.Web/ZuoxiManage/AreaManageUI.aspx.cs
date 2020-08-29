using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class AreaManageUI : PageBase
    {
        public string htmlXa = "";
        public string htmlBj = "";

        public int Xaid = 0;
        public int Bjid = 0;
        /// 操作方式
        /// <summary>
        /// 操作方式
        /// </summary>
        public string Action { get { return BitAuto.ISDC.CC2012.BLL.Util.GetCurrentRequestQueryStr("action"); } }
        /// 区域
        /// <summary>
        /// 区域
        /// </summary>
        public string Regionid { get { return BitAuto.ISDC.CC2012.BLL.Util.GetCurrentRequestQueryStr("regionid"); } }
        /// 数据
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get { return BitAuto.ISDC.CC2012.BLL.Util.GetCurrentRequestQueryStr("data"); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024BUT5103"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                Xaid = (int)RegionID.XiAn;
                Bjid = (int)RegionID.BeiJ;

                string xanm = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(RegionID), Xaid);
                string bjnm = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(RegionID), Bjid);

                if (Action == "modify")
                {
                    AreaManageConfig config = new AreaManageConfig(Server);
                    Dictionary<string, string> dic_bj = new Dictionary<string, string>();
                    Dictionary<string, string> dic_xa = new Dictionary<string, string>();
                    if (Regionid == Bjid.ToString())
                    {
                        dic_xa = config.ReadFile(xanm);
                        dic_bj = ConvertDic(Data);
                    }
                    else if (Regionid == Xaid.ToString())
                    {
                        dic_bj = config.ReadFile(bjnm);
                        dic_xa = ConvertDic(Data);
                    }

                    Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
                    dic.Add(bjnm, dic_bj);
                    dic.Add(xanm, dic_xa);
                    config.WriteFile(dic);
                }

                htmlBj = BindUserName(bjnm);
                htmlXa = BindUserName(xanm);
            }
        }
        /// 获取绑定的html代码
        /// <summary>
        /// 获取绑定的html代码
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private string BindUserName(string area)
        {
            AreaManageConfig config = new AreaManageConfig(Server);
            Dictionary<string, string> dic = config.ReadFile(area);
            string html = "";
            foreach (string key in dic.Keys)
            {
                string name = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetTrueNameByUserID(key);
                int length = name.Length + 1;
                double prelen = 12.5;
                int len = (int)(prelen * (double)length);
                html += " <li style='width: " + len + "px;' userid='" + key + "'>" + name + "</li>";
            }
            if (dic.Count == 0)
            {
                //空 占位 使得ul的宽度起作用
                html += " <li style='width: 50px;' userid=''>&nbsp;</li>";
            }
            return html;
        }
        /// 字符串转换字典
        /// <summary>
        /// 字符串转换字典
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Dictionary<string, string> ConvertDic(string data)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                string[] array = data.Split(';');
                string[] array1 = array[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] array2 = array[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array1.Length; i++)
                {
                    dic[array2[i]] = array1[i];
                }
            }
            catch
            {
            }
            return dic;
        }
    }
}