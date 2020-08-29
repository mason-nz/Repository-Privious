using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Web.Channels;
using BitAuto.DSC.IM_2015.Entities;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.CustInfo
{
    public partial class VisitCust : System.Web.UI.Page
    {
        public string LoginID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["loginid"]))
                {
                    return HttpContext.Current.Request["loginid"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestVisitID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["VisitID"]))
                {
                    return HttpContext.Current.Request["VisitID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestCSID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["CSID"]))
                {
                    return HttpContext.Current.Request["CSID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string URLTitle;
        public string TitleURL;
        public string sourctypeinfo;
        public string AgentSTime;
        public string ConvertsTime;
        public string VisitID;
        public string ProvinceID;
        public string CityID;
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(LoginID))
                {
                    string userid = BLL.Util.GetLoginUserID().ToString();
                    #region 加载访客信息
                    Entities.UserVisitLog model = null;
                    if (!string.IsNullOrEmpty(RequestVisitID))
                    {
                        int _visitid = 0;
                        int.TryParse(RequestVisitID, out _visitid);
                        int _csid = 0;
                        int.TryParse(RequestCSID, out _csid);
                        DataTable dt = BLL.UserVisitLog.Instance.GetVisitAndCs(_visitid, _csid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            model = new UserVisitLog();
                            model.VisitID = _visitid;
                            model.UserReferURL = dt.Rows[0]["UserReferURL"].ToString();
                            model.UserReferTitle = dt.Rows[0]["UserReferTitle"].ToString();
                            model.UserName = dt.Rows[0]["UserName"].ToString();
                            if (dt.Rows[0]["UpdateTime"] != DBNull.Value)
                            {
                                model.UpdateTime = Convert.ToDateTime(dt.Rows[0]["UpdateTime"].ToString());
                            }
                            if (dt.Rows[0]["CreateTime"] != DBNull.Value)
                            {
                                model.CreatTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"].ToString());
                            }
                            if (dt.Rows[0]["queuefailtime"] != DBNull.Value)
                            {
                                model.QueuefailTime = Convert.ToDateTime(dt.Rows[0]["queuefailtime"].ToString());
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[0]["SourceType"].ToString()))
                            {
                                model.SourceType = dt.Rows[0]["SourceType"].ToString();
                            }
                            model.LoginID = dt.Rows[0]["LoginID"].ToString(); ;
                            model.CustID = dt.Rows[0]["CustID"].ToString();
                            model.Phone = dt.Rows[0]["Phone"].ToString();
                            if (dt.Rows[0]["Sex"].ToString() != "")
                            {
                                if (dt.Rows[0]["Sex"].ToString() == "True")
                                {
                                    model.Sex = true;
                                }
                                else
                                {
                                    model.Sex = false;
                                }
                            }
                            model.Remark = dt.Rows[0]["Remark"].ToString();
                            if (dt.Rows[0]["ProvinceID"].ToString() != "")
                            {
                                int _ProvinceID = 0;
                                int.TryParse(dt.Rows[0]["ProvinceID"].ToString(), out _ProvinceID);
                                model.ProvinceID = _ProvinceID;
                            }
                            if (dt.Rows[0]["CityID"].ToString() != "")
                            {
                                int _CityID = 0;
                                int.TryParse(dt.Rows[0]["CityID"].ToString(), out _CityID);
                                model.CityID = _CityID;
                            }
                            if (dt.Rows[0]["AgentStartTime"].ToString() != "")
                            {
                                AgentSTime = dt.Rows[0]["AgentStartTime"].ToString();
                            }

                            if (dt.Rows[0]["cscreatetime"].ToString() != "")
                            {
                                ConvertsTime = dt.Rows[0]["cscreatetime"].ToString();
                            }
                        }
                    }
                    if (model == null)
                    {
                        return;
                    }
                    VisitID = model.VisitID.ToString();
                    ProvinceID = model.ProvinceID.ToString();
                    CityID = model.CityID.ToString();

                    //用现有信息填充
                    this.username.Value = model.UserName;
                    this.tel.Value = model.Phone;
                    if (model.Sex)
                    {
                        this.radMan.Checked = true;
                    }
                    else if (!model.Sex)
                    {
                        this.radWoman.Checked = true;
                    }
                    this.selProvince.SelectedIndex = this.selProvince.Items.IndexOf(this.selProvince.Items.FindByValue(model.ProvinceID.ToString()));
                    this.selCity.SelectedIndex = this.selCity.Items.IndexOf(this.selCity.Items.FindByValue(model.CityID.ToString()));
                    //如果电话，姓名不为空//调用cc接口，取custinfo
                    if (string.IsNullOrEmpty(model.CustID) && !string.IsNullOrEmpty(model.UserName.Trim()) && !string.IsNullOrEmpty(model.Phone.Trim()))
                    {

                        //如果custinfo 存在，用custinfo 信息填充表单，否则用现有信息填充
                        Entities.CEntity CEntityModel = BitAuto.DSC.IM_2015.WebService.CC.CCWebServiceHepler.Instance.GetCustomer(model.Phone.Trim(), model.UserName.Trim());
                        if (CEntityModel != null)
                        {
                            this.username.Value = CEntityModel.Name;
                            this.tel.Value = CEntityModel.Tel;
                            if (CEntityModel.Sex == "男")
                            {
                                this.radMan.Checked = true;
                            }
                            else
                            {
                                this.radWoman.Checked = true;
                            }
                            this.selProvince.SelectedIndex =
                                this.selProvince.Items.IndexOf(
                                    this.selProvince.Items.FindByValue(CEntityModel.ProvinceID));
                            this.selCity.SelectedIndex =
                                this.selCity.Items.IndexOf(this.selCity.Items.FindByValue(CEntityModel.CityID));

                            model.CustID = CEntityModel.CustID;

                            BLL.UserVisitLog.Instance.UpdateUserVisitLog(model);

                        }
                    }
                    this.Remark.Value = model.Remark;
                    URLTitle = model.UserReferTitle;
                    TitleURL = model.UserReferURL;
                    sourctypeinfo = BLL.Util.GetSourceTypeName(model.SourceType);
                    #endregion

                }
            }
        }
    }
}