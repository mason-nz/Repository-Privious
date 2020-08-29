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
using BitAuto.ISDC.CC2012.Web.Base;
namespace BitAuto.ISDC.CC2012.Web.ExamObject
{
    public partial class ExamProjectEdit :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            selExamBG.DataSource = BLL.EmployeeSuper.Instance.GetCurrentUserGroups(BLL.Util.GetLoginUserID());
            selExamBG.DataValueField = "BGID";
            selExamBG.DataTextField = "Name";
            selExamBG.DataBind();

            CateBind();
            if (Request["ExamObj"] != null)
            {//编辑

            }
            else
            { //添加
                
            }
        }

        #region 绑定项目分类
        public void CateBind()
        {
            DataTable dt = new DataTable();
            QueryExamCategory query = new QueryExamCategory();
            query.Type = 1;
            int total = 0;
            dt = BLL.ExamCategory.Instance.GetExamCategory(query, " CreateTime", 1, 10, out total);
            Rpt.DataSource = dt;
            Rpt.DataBind();
        }         
        #endregion

        public Entities.ExamInfo examInfo = new Entities.ExamInfo();
        public Entities.MakeUpExamInfo makeExamInfo = new Entities.MakeUpExamInfo();
        public Entities.ExamPaper examPaper = new Entities.ExamPaper();
        public Entities.ExamPaper makeExamPaper = new Entities.ExamPaper();
        public string ExamPersionsIDs = "";
        public string ExamPersionsNames = "";
        public string MakeExamPersionsIDs = "";
        public string MakeExamPersionsNames = "";
        
        #region 是否是编辑状态
        public bool Action()
        {
            if (Request["id"] != null)
            {//编辑                
                int EIID = 0;
                if (int.TryParse(Request["id"].ToString(), out EIID))
                {
                    if (BLL.ExamInfo.Instance.IsExistsByEIID(EIID))
                    {
                        examInfo = BLL.ExamInfo.Instance.GetExamInfo(EIID);
                        examPaper = BLL.ExamPaper.Instance.GetExamPaper(examInfo.EPID);
                        GetExamPersions(examInfo.EIID, 0,out ExamPersionsIDs,out ExamPersionsNames);
                        if (examInfo.IsMakeUp == 1)
                        {
                            makeExamInfo = BLL.MakeUpExamInfo.Instance.GetMakeUpExamInfoByEIID(EIID);
                            makeExamPaper = BLL.ExamPaper.Instance.GetExamPaper(makeExamInfo.MakeUpEPID);
                            GetExamPersions(examInfo.EIID, makeExamInfo.MEIID, out MakeExamPersionsIDs, out MakeExamPersionsNames);
                        }
                    }
                    else
                    {
                        Response.Write("alert('未能找到ID为" + Request["id"].ToString() + "的考试项目');closePage();");
                    }
                }
                else
                {
                    Response.Write("alert('参数" + Request["id"].ToString() + "转换成数字失败');closePage();"); 
                }
                return true;
            }
            else
            { //添加
                return false;
            }
        }
        #endregion

        #region 得到考生IDs和Names
        public void GetExamPersions(long EIID, int MEIID, out string IDs,out string Names)
        {
            IDs = "";
            Names = "";
            int total = 0;

            Entities.QueryExamPerson query = new QueryExamPerson();
            DataTable dt = new DataTable();

            if (MEIID == 0)
            {//获取普通考试考生数据
                query.ExamType = 0;
                query.EIID = EIID;
            }
            else
            {//获取补考考生数据
                query.ExamType =1;
                query.EIID = EIID;
                query.MEIID = MEIID;
            }

            dt = BLL.ExamPerson.Instance.GetExamPerson(query, "", 1, 1000, out total);
            foreach (DataRow row in dt.Rows)
            {
                IDs += row["ExamPerSonID"].ToString() + ",";
                Names += BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(row["ExamPerSonID"].ToString())) + ",";
            }
            if (IDs.Length > 0)
            {
                IDs = IDs.Substring(0, IDs.Length - 1);
                Names = Names.Substring(0, Names.Length - 1);
            }
        }
        #endregion
    }
}