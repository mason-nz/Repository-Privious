using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class EditMember : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMemberControls();
            }
        }

        private void BindMemberControls()
        {
            //获得所有会员，加载
            CustInfoHelper h = new CustInfoHelper();
            //int tid = -1;
            //if (int.TryParse(h.TID, out tid))
            //{
                Control ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditMember.ascx", h.TID);
                this.PlaceHolder.Controls.Add(ctl);
           // }
        }

        /// <summary>
        /// 重写LoadControl，带参数。
        /// </summary>
        private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            System.Reflection.ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

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