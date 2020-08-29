using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class KnowledgeLibAudit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
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

        public string status = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            int kid = 0;
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024BUT3101") && !BLL.Util.CheckRight(userID, "SYS024BUT3102"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                if (int.TryParse(KID, out kid))
                {
                    Entities.KnowledgeLib info = BLL.KnowledgeLib.Instance.GetKnowledgeLib(kid);
                    if (info != null)
                    {
                        if (BLL.KnowledgeLib.Instance.IsExistQuestion(kid))
                        {
                            Control ctl = this.LoadControl("~/KnowledgeLib/UCKnowledgeLib/UCQuestionView.ascx", kid);
                            this.phQuestion.Controls.Add(ctl);
                        }
                        status = info.Status.ToString();
                    }
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