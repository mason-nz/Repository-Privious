using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{

    public partial class UCKnowledgeEditPDF : System.Web.UI.UserControl
    {

        /// <summary>
        ///  知识点ID
        /// </summary>
        public string KID
        {
            get
            {
                if (HttpContext.Current.Request["kid"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["kid"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 分类ID
        /// </summary>
        public string KCID = "";

        /// <summary>
        /// 分类级别
        /// </summary>
        public string Level1ID = "";
        public string Level2ID = "";
        public string Level3ID = "";
        public string Level = "";

        /// <summary>
        /// 知识点状态
        /// </summary>
        public string status = "";

        public int RegionID = -2;
        public string DirectryName = BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.KnowledgeLib);
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                long intVal = 0;
                if (KID != string.Empty && long.TryParse(KID, out intVal))
                {
                    //如果是编辑
                    BindKnowledgeInfo();

                    this.knowID.Value = KID;

                }
            }
            int userid = BLL.Util.GetLoginUserID();
            EmployeeAgent a = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
            if (a.RegionID.HasValue)
            {
                RegionID = a.RegionID.Value;
            }
            if (KID != string.Empty)
            { BindFiles(); }
        }

        private void BindKnowledgeInfo()
        {

            Entities.KnowledgeLib model = BLL.KnowledgeLib.Instance.GetKnowledgeLib(long.Parse(KID));
            if (model != null)
            {
                this.txtTitle.Value = model.Title;
                // this.txtContext.Value = BLL.Util.HtmlDiscode(model.Content);
                // this.FCKeditor1.Value = BLL.KnowledgeLib.Instance.GetKnowledgeHtml(long.Parse(KID));
                this.txtAbstract.Value = model.Abstract;

                status = model.Status.ToString(); ;

                #region 知识点级别

                Entities.KnowledgeCategory category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(model.KCID.ToString()));
                if (category != null)
                {
                    Level = category.Level.ToString();
                    if (Level == "3")
                    {
                        Level3ID = category.KCID.ToString();

                        category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(category.Pid.ToString()));
                        if (category != null)
                        {
                            Level2ID = category.KCID.ToString();
                            category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(category.Pid.ToString()));
                            if (category != null)
                            {
                                Level1ID = category.KCID.ToString();
                            }
                        }
                    }
                    else if (Level == "2")
                    {
                        Level2ID = category.KCID.ToString();

                        category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(category.Pid.ToString()));
                        if (category != null)
                        {
                            Level1ID = category.KCID.ToString();
                        }
                    }
                    else if (Level == "1")
                    {
                        Level1ID = category.KCID.ToString();
                    }

                    KCID = model.KCID.ToString();
                }
                #endregion


            }
        }



        /// <summary>
        /// 绑定文件
        /// </summary>
        private void BindFiles()
        {
            Entities.QueryKLUploadFile query = new Entities.QueryKLUploadFile();
            query.KLID = long.Parse(KID);

            int totalCount = 0;
            DataTable fileDt = BLL.KLUploadFile.Instance.GetKLUploadFile(query, "", 1, 9999, out totalCount); ;

            if (fileDt != null && fileDt.Rows.Count > 0)
            {
                this.labFiles.Visible = true;
                this.rpfileList.DataSource = fileDt;
                this.rpfileList.DataBind();
            }
            else
            {
                this.labFiles.Visible = false;
            }
        }


    }
}