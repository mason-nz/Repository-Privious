using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{

    public partial class AddSingleQuestion : PageBase
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
                    return HttpContext.Current.Request["kid"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public int RegionID = 0;
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
                int kid = 0;
                int.TryParse(KID, out kid);
                Control ctl;
                if (kid > 0)
                {
                    this.Title = "编辑试题";
                    ctl = this.LoadControl("~/KnowledgeLib/UCKnowledgeLib/UCQuestionEdit.ascx", kid);
                }
                else
                {
                    this.Title = "添加试题";
                    ctl = this.LoadControl("~/KnowledgeLib/UCKnowledgeLib/UCQuestionAdd.ascx");
                }

                this.phQuestion.Controls.Add(ctl);
            }
        }

        //获取分类下拉列表
        private void bindKnowledgeCategory()
        {
            //ListItem firstItem = new ListItem();
            //firstItem.Text = "";
            //firstItem.Value = "";

            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Level = 1;
            query.Pid = 0;
            query.Regionid = RegionID;
            int count;
            DataTable dt = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(query, "", 1, 10000, out count);

            selKCID1.DataSource = dt;
            selKCID1.DataTextField = "Name";
            selKCID1.DataValueField = "KCID";
            selKCID1.DataBind();
            selKCID1.Items.Insert(0, new ListItem() { Text = "请选择", Value = "-1" });

        }

        public string CommonSingleKLID
        {
            get
            {
                QueryKnowledgeLib query = new QueryKnowledgeLib();
                query.Status = 5;
                int kcidd = 0;
                try
                {
                    return BLL.KnowledgeLib.Instance.GetKnowledgeLib(query, "", 1, 1, out kcidd).Rows[0]["klid"].ToString();
                }
                catch (Exception)
                {
                    return "-1";
                }

            }

        }

        /// <summary>
        /// 是否是管理员 1、管理员  0、普通用户
        /// </summary>
        public string IsManager
        {
            get
            {
                try
                {
                    if (BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin())
                    {

                        int userID = BLL.Util.GetLoginUserID();
                        bool right_Approval = BLL.Util.CheckRight(userID, "SYS024BUT3101");//审核通过
                        bool right_Reject = BLL.Util.CheckRight(userID, "SYS024BUT3102");//驳回
                        bool right_Disable = BLL.Util.CheckRight(userID, "SYS024BUT3104");//停用

                        if (!right_Approval && !right_Reject && !right_Disable)     //如果不具备 审核通过、驳回、停用的权限 则是普通用户
                        {
                            return "0";
                        }
                        else
                        {
                            return "1";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                catch (Exception ex)
                {
                    return "0";
                }


            }
        }


        private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            //And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }

            // Finally return the fully initialized UC
            return ctl;
        }
    }
}