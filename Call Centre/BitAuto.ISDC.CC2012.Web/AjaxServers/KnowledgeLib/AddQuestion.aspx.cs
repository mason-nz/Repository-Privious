using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class AddQuestion : PageBase
    {
        public string KLID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("KLID").ToString();
            }
        }
        public string KLType   //0:知识点；1:FAQ
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("KLType").ToString();
            }
        }

        public int KCid = -1;
        public int KCPid = -1;
        public int KLPLevel = -1;
        public int RegionID = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userid = BLL.Util.GetLoginUserID();
                EmployeeAgent a = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
                if (a.RegionID.HasValue)
                {
                    RegionID = a.RegionID.Value;
                }

                bindKnowledgeCategory();
            }
        }

        //获取分类下拉列表
        private void bindKnowledgeCategory()
        {
            try
            {
                int qCount;
                int parentParentId = -1;
                DataTable dtkc = new DataTable();
                if (KLType == "0")
                {
                    Entities.QueryKnowledgeLib queryM = new QueryKnowledgeLib();
                    queryM.KLID = int.Parse(KLID);
                    dtkc = BLL.KnowledgeLib.Instance.GetKnowledgeLib(queryM, "", 1, 1, out qCount);
                }
                else if (KLType == "1")
                {
                    Entities.QueryKLFAQ queryF = new QueryKLFAQ();
                    queryF.KLID = int.Parse(KLID);
                    dtkc = BLL.KLFAQ.Instance.GetKLFAQ(queryF, "", 1, 1, out qCount);
                }
                if (dtkc != null && dtkc.Rows.Count > 0)
                {
                    KnowledgeCategory categoryEntity = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(dtkc.Rows[0]["KCID"].ToString()));
                    KCid = categoryEntity.KCID;
                    KCPid = categoryEntity.Pid.Value;
                    KLPLevel = categoryEntity.Level.Value;
                    if (KCid != 0)
                    {
                        KnowledgeCategory categoryEntityPP = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(KCPid);
                        parentParentId = categoryEntityPP.Pid.Value;
                    }
                }

                Entities.QueryKnowledgeCategory query = new Entities.QueryKnowledgeCategory();
                //query.Level = 1;
                if (parentParentId != -1)
                {
                    query.Pid = parentParentId;
                }
                else
                {
                    query.Pid = 0;
                }
                query.Regionid = RegionID;
                int count;
                DataTable dt = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(query, "", 1, 10000, out count);

                selKCID1.DataSource = dt;
                selKCID1.DataTextField = "Name";
                selKCID1.DataValueField = "KCID";
                selKCID1.DataBind();
                selKCID1.Items.Insert(0, new ListItem() { Text = "请选择", Value = "-1" });
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("异常：" + ex.Message);
            }
        }
    }
}