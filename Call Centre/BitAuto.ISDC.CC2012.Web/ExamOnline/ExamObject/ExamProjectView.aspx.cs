using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Web.SessionState;
using System.Data.SqlClient;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject
{
    public partial class ExamProjectView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        /// <summary>
        /// 项目ID
        /// </summary>
        public string EIID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["eiid"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["eiid"]);
            }
        }

        public Entities.ExamInfo model;
        public Entities.MakeUpExamInfo examInfo_Make = new Entities.MakeUpExamInfo();
        public Entities.ExamPaper examPaper = new Entities.ExamPaper();
        public Entities.ExamPaper examPaper_Make = new Entities.ExamPaper();
        public string EmployeeNames = "";
        public string EmployeeNames_Make = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //long longVal = 0;
                //if (EIID != String.Empty && long.TryParse(EIID,out longVal))
                //{
                //    model = BLL.ExamInfo.Instance.GetExamInfo(longVal);
                //    examPaper = BLL.ExamPaper.Instance.GetExamPaper(model.EPID);
                //    EmployeeNames = getEmployeeNames(long.Parse(EIID), 0,model.EPID);

                //    if (model.IsMakeUp == 1) 
                //    {
                //        examInfo_Make = BLL.MakeUpExamInfo.Instance.GetMakeUpExamInfoByEIID( Convert.ToInt32(model.EIID));

                //        examPaper_Make = BLL.ExamPaper.Instance.GetExamPaper(examInfo_Make.MakeUpEPID);

                //        EmployeeNames_Make = getEmployeeNames(long.Parse(EIID), examInfo_Make.MEIID,Convert.ToInt32(examInfo_Make.MakeUpEPID));
                //    }
                //    if (model != null)
                //    {
                //        hidEIID.Value = EIID;
                //        hidEName.Value = model.Name;
                //    }
                //}


                if (string.IsNullOrEmpty(EIID))
                {
                    Response.Write(@"<script language='javascript'>alert('考试项目ID不能为空，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                }
                else
                {
                    long longVal = 0;
                    if (long.TryParse(EIID, out longVal))
                    {
                        model = BLL.ExamInfo.Instance.GetExamInfo(longVal);


                        if (model == null)
                        {
                            Response.Write(@"<script language='javascript'>alert('考试项目不存在，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                        }
                        else
                        {
                            examPaper = BLL.ExamPaper.Instance.GetExamPaper(model.EPID);
                            if (examPaper == null)
                            {
                                Response.Write(@"<script language='javascript'>alert('考试试卷不存在，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                            }
                            else
                            {
                                EmployeeNames = getEmployeeNames(long.Parse(EIID), 0, model.EPID);
                                hidEIID.Value = EIID;
                                hidEName.Value = model.Name;
                                if (model.IsMakeUp == 1)
                                {
                                    examInfo_Make = BLL.MakeUpExamInfo.Instance.GetMakeUpExamInfoByEIID(Convert.ToInt32(model.EIID));
                                    if (examInfo_Make != null)
                                    {
                                        examPaper_Make = BLL.ExamPaper.Instance.GetExamPaper(examInfo_Make.MakeUpEPID);
                                        if (examPaper_Make != null)
                                        {
                                            EmployeeNames_Make = getEmployeeNames(long.Parse(EIID), examInfo_Make.MEIID, Convert.ToInt32(examInfo_Make.MakeUpEPID));
                                        }
                                        else
                                        {
                                            Response.Write(@"<script language='javascript'>alert('补考试卷不存在，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                                        }

                                    }
                                    else
                                    {
                                        Response.Write(@"<script language='javascript'>alert('补考项目不存在，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>alert('考试项目ID数据格式不正确，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                    }
                }
            }
        }


        #region 根据分类ID获取分类名称
        /// <summary>
        /// 根据分类ID获取分类名称
        /// </summary>
        /// <param name="PagerID"></param>
        /// <returns></returns>
        public string GetCatageName(string PagerID)
        {
            string name = "";
            int catageID = 0;
            if (int.TryParse(PagerID, out catageID))
            {
                Entities.ExamCategory model = BLL.ExamCategory.Instance.GetExamCategory(catageID);
                if (model != null)
                {
                    name = model.Name;
                }
            }
            return name;
        }
        #endregion

        #region 得到考生串
        public string getEmployeeNames(long EIID, int MEIID, int EPID)
        {
            string Names = "";
            int total = 0;
            string type = "";
            Entities.QueryExamPerson query = new QueryExamPerson();
            DataTable dt = new DataTable();

            Entities.QueryExamOnline examOnlineQuery = new QueryExamOnline();
            examOnlineQuery.EIID = Convert.ToInt32(EIID);

            if (MEIID == 0)
            {//获取普通考试考生数据
                query.ExamType = 0;
                query.EIID = EIID;
                examOnlineQuery.IsMakeUp = 0;
                type = "0";
            }
            else
            {//获取补考考生数据
                query.ExamType = 1;
                query.EIID = EIID;
                query.MEIID = MEIID;
                examOnlineQuery.IsMakeUp = 1;
                examOnlineQuery.MEIID = MEIID;
                type = "1";
            }

            dt = BLL.ExamPerson.Instance.GetExamPerson(query, "", 1, 1000, out total);
            foreach (DataRow row in dt.Rows)
            {
                string persionID = row["ExamPerSonID"].ToString();
                string name = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(persionID));
                examOnlineQuery.ExamPersonID = Convert.ToInt32(persionID);


                DataTable st_examOnline = BLL.ExamOnline.Instance.GetExamOnline(examOnlineQuery, "", 1, 1, out total);
                if (st_examOnline.Rows.Count > 0 && st_examOnline.Rows[0]["IsMarking"].ToString() == "1")
                {
                    Names += "<a href='../ExamScoreManagement/MarkExamPaper.aspx?eiid=" + BLL.Util.EncryptString(EIID.ToString())
                        + "&type=" + BLL.Util.EncryptString(type)
                        + "&come=" + BLL.Util.EncryptString("2")
                        + "&ExamPersonID=" + BLL.Util.EncryptString(persionID)
                        + "&epid=" + BLL.Util.EncryptString(EPID.ToString()) + "'>" + name + "</a>，";
                }
                else
                {
                    Names += name + "，";
                }
            }

            if (Names.Length > 0)
            {
                Names = Names.Substring(0, Names.Length - 1);
            }
            return Names;
        }

        #endregion


        public string GetBusinessGroupNameById(int bgid)
        {
           return BLL.BusinessGroup.Instance.GetBusinessGroupByBGID(bgid).Rows[0]["Name"].ToString();
        }

    }
}