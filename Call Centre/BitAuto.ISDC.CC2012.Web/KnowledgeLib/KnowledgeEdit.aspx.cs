using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class KnowledgeEdit : PageBase
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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int userID = BLL.Util.GetLoginUserID(); 
                if (!BLL.Util.CheckRight(userID, "SYS024BUT3202") && !BLL.Util.CheckRight(userID, "SYS024MOD3601"))//	添加试题或为知识点管理
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                int kid = 0;
                int.TryParse(KID, out kid);
                Control ctl;
                if (kid > 0)
                {
                    this.Title = "编辑知识点";
                    ctl = this.LoadControl("~/KnowledgeLib/UCKnowledgeLib/UCQuestionEdit.ascx", kid);
                }
                else
                {
                    this.Title = "添加知识点";
                    ctl = this.LoadControl("~/KnowledgeLib/UCKnowledgeLib/UCQuestionAdd.ascx");
                }

                this.phQuestion.Controls.Add(ctl);
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