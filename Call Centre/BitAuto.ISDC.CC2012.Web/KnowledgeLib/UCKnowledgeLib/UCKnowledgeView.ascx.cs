using System;
using System.Data;
using System.IO;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{
    public partial class UCKnowledgeView : System.Web.UI.UserControl
    {

        /// <summary>
        /// 分类级别
        /// </summary>
        public string Level1Name = "";
        public string Level2Name = "";
        public string Level3Name = "";
        public string Level = "";
        public string FileUrl;

        public int IsFileExists
        {
            get
            {
                try
                {
                    if (FileUrl.StartsWith("http")) { return 1; }
                    else
                    {
                        return File.Exists(Context.Server.MapPath(FileUrl)) ? 1 : 0;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }

        public int IsMedia
        {
            get
            {
                try
                {
                    if (FileUrl.EndsWith("wav") || FileUrl.EndsWith("mp3"))
                    {
                        return 1;
                    }
                    return 0;
                }
                catch (Exception)
                {

                    return 0;
                }
            }
        }

        public string BindDownLoadA
        {
            get
            {
                if (IsFileExists == 1)
                {
                    return "Personalization/DownLoadFilePage.aspx?theAction=1&theUrl=" + HttpUtility.UrlEncode(FileUrl);
                }
                else
                {
                    return "javascript:void(0);";
                }
            }
        }

        /// <summary>
        /// 是否有附件
        /// </summary>
        public string IsHaveFiles = "";

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
        private bool blIsContentEmpty = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long intVal = 0;
                if (KID != string.Empty && long.TryParse(KID, out intVal))
                {
                    //如果是编辑
                    BindKnowledgeInfo();

                    BindFiles();
                }
            }
        }

        private void BindKnowledgeInfo()
        {

            Entities.KnowledgeLib model = BLL.KnowledgeLib.Instance.GetKnowledgeLib(long.Parse(KID));
            if (model != null)
            {
                FileUrl = model.FileUrl == null ? "" : "/upload/" + (BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.KnowledgeLib) + model.FileUrl).Replace("\\", "/");
                lbCreateTime.Text = model.CreateTime.Value.ToString("yyyy-M-d");
                lbClickCount.Text = model.ClickCount < 0 ? "0" : model.ClickCount.ToString();
                lbDownCount.Text = model.DownLoadCount < 0 ? "0" : model.DownLoadCount.ToString();
                hdAbs.Value = string.IsNullOrEmpty(model.Abstract) ? "" : model.Abstract;
                this.txtTitle.InnerText = model.Title;
                try
                {
                    this.txtContext.InnerHtml = BLL.KnowledgeLib.Instance.GetKnowledgeHtml(long.Parse(KID));
                    blIsContentEmpty = string.IsNullOrEmpty(this.txtContext.InnerHtml);
                }
                catch
                {


                }


                //给摘要赋值

                Entities.KnowledgeCategory categortyModel = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(model.KCID.ToString()));

                if (model.UploadFileCount > 0)
                {
                    IsHaveFiles = "1";
                }
                #region 知识点级别

                Entities.KnowledgeCategory category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(model.KCID.ToString()));
                if (category != null)
                {
                    //lbC.Text = category.Name;
                    Level = category.Level.ToString();
                    if (Level == "3")
                    {
                        Level3Name = category.Name.ToString();

                        category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(category.Pid.ToString()));
                        if (category != null)
                        {
                            Level2Name = category.Name.ToString();
                            category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(category.Pid.ToString()));
                            if (category != null)
                            {
                                Level1Name = category.Name.ToString();
                            }
                        }
                    }
                    else if (Level == "2")
                    {
                        Level2Name = category.Name.ToString();

                        category = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(int.Parse(category.Pid.ToString()));
                        if (category != null)
                        {
                            Level1Name = category.Name.ToString();
                        }
                    }
                    else if (Level == "1")
                    {
                        Level1Name = category.Name.ToString();
                    }
                }
                lbC.Text = Level1Name + "--" + Level2Name;
                /*
                string catageName = "";


                if (Level3Name != "")
                {
                    catageName = Level3Name;
                }
                else if (Level2Name != "")
                {
                    catageName = Level2Name;
                }
                else if (Level1Name != "")
                {
                    catageName = Level1Name;
                }
                */
                //this.txtCategory.InnerHtml = DateTime.Parse(model.CreateTime.ToString()).ToString("yyyy-MM-dd") + "&nbsp;&nbsp;&nbsp;" + catageName;

                #endregion
            }
        }

        /// <summary>
        /// 绑定文件
        /// </summary>
        private void BindFiles()
        {
            //如果content为空代表新的知识点，不需要显示附件。
            if (blIsContentEmpty) return;

            Entities.QueryKLUploadFile query = new Entities.QueryKLUploadFile();
            query.KLID = long.Parse(KID);

            int totalCount = 0;
            DataTable fileDt = BLL.KLUploadFile.Instance.GetKLUploadFile(query, "", 1, 9999, out totalCount); ;

            if (fileDt != null && fileDt.Rows.Count > 0)
            {
                this.rpfileList.DataSource = fileDt;
                this.rpfileList.DataBind();
            }
        }
    }
}