using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Entities;
using System.Text;
using BitAuto.ISDC.CC2012.BLL;
using System.Net;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? string.Empty : HttpContext.Current.Request["Action"].ToString();
            }
        }
        public int UserID
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormInt("UserID");
            }
        }
        public string AgentNum
        {
            get
            {
                return HttpContext.Current.Request["AgentNum"] == null ? string.Empty : HttpContext.Current.Request["AgentNum"].ToString();
            }
        }
        public string AreaID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AreaID");
            }
        }
        public string BusinessType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("BusinessType").Trim();
            }
        }
        public string UserRolesID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("UserRolesID").Trim();
            }
        }
        public string AtGroupID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AtGroupID").Trim();
            }
        }
        //public string ManageBG
        //{
        //    get
        //    {
        //        return BLL.Util.GetCurrentRequestStr("ManageBG").Trim();
        //    }
        //}
        public string ManageBG_BeiJing
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ManageBG_BeiJing").Trim();
            }
        }
        public string ManageBG_XiAn
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ManageBG_XiAn").Trim();
            }
        }
        public string UserIDs
        {
            get
            {
                return HttpContext.Current.Request["UserIDs"] == null ? string.Empty : HttpContext.Current.Request["UserIDs"].ToString();
            }
        }

        public string BGID
        {
            get
            {
                return HttpContext.Current.Request["BGID"] == null ? string.Empty : HttpContext.Current.Request["BGID"].ToString();
            }
        }
        public string Name
        {
            get
            {
                return HttpContext.Current.Request["Name"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Name"].ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public string SGIDSAndPrioritys
        {
            get
            {
                return HttpContext.Current.Request["SGIDSAndPrioritys"] == null ? string.Empty : HttpContext.Current.Request["SGIDSAndPrioritys"].ToString();
            }
        }
        public string UserName
        {
            get
            {
                return HttpContext.Current.Request["UserName"] == null ? string.Empty : HttpContext.Current.Request["UserName"].ToString();
            }
        }
        public string ToHeLiSGIDAndPriority
        {
            get
            {
                return HttpContext.Current.Request["ToHeLiSGIDAndPriority"] == null ? string.Empty : HttpContext.Current.Request["ToHeLiSGIDAndPriority"].ToString();
            }
        }
        public string AtGroupName
        {
            get
            {
                return HttpContext.Current.Request["AtGroupName"] == null ? string.Empty : HttpContext.Current.Request["AtGroupName"].ToString();
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            int userId = BLL.Util.GetLoginUserID();
            switch (Action.ToLower())
            {
                case "getgroupdataright":
                    GetGroupDataRight(out msg);
                    break;
                case "disposebusinessgroup":
                    DisposeBusinessGroup(out msg);
                    break;
                case "changebusinessgroupstatus":
                    if (!BLL.Util.CheckRight(userId, "SYS024MOD5008"))  //此处只做二级菜单的权限（不是三级菜单SYS024MOD500801）
                    {
                        msg = "{result:'no',msg:'您没有执行此操作的权限！'}";
                    }
                    else
                    {
                        ChangeBusinessGroupStatus(out msg);
                    }
                    break;
                case "getgroupbyareaid":
                    GetGroupByAreaID(out msg);
                    break;
                case "getgroupbyregionid":
                    GetGroupByRegionID(out msg);
                    break;
                case "getcurrentuserarea":
                    GetCurrentUserArea(out msg);
                    break;
                case "getcalldisplay":
                    GetCallDisplay(out msg);
                    break;
                case "getareainfoandmanagebgs":
                    GetAreaInfoAndManageBgs(out msg);
                    break;
                case "getroleidforccandimbyuserid":
                    GetRoleIDForCCAndIMByUserid(out msg);
                    break;
                case "employeeagentoptionforpageone":
                    if (!BLL.Util.CheckRight(userId, "SYS024BUT5102"))
                    {
                        msg = "{'result':'failure','msg':'您没有执行此操作的权限！'}";
                    }
                    else
                    {
                        EmployeeAgentOptionForPageOne(out msg);
                    }
                    break;
                case "employeeagentoptionforpagetwo":
                    if (!BLL.Util.CheckRight(userId, "SYS024BUT5102"))
                    {
                        msg = "{'result':'failure','msg':'您没有执行此操作的权限！'}";
                    }
                    else
                    {
                        EmployeeAgentOptionForPageTwo(out msg);
                    }
                    break;
                case "employeeagentmutiloption":
                    if (!BLL.Util.CheckRight(userId, "SYS024BUT5101"))
                    {
                        msg = "{'result':'failure','msg':'您没有执行此操作的权限！'}";
                    }
                    else
                    {
                        EmployeeAgentMutilOption(out msg);
                    }
                    break;
                case "checkgroupnamenotuse":
                    CheckGroupNameNotUse(out msg);
                    break;
                case "getbusinesslinebybgid":
                    GetBusinessLineByBGID(out msg);
                    break;
                case "getallhotlineskillgroup":
                    GetAllHotlineSkillGroup(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        private void GetAllHotlineSkillGroup(out string msg)
        {
            msg = "";
            try
            {
                DataTable dt = BLL.SkillGroupDataRight.Instance.GetAllHotlineSkillGroup();
                string frontCDID = "";
                StringBuilder strhtml = new StringBuilder("");
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++
                if (dt != null)
                {

                    for (int m = 0; m < dt.Rows.Count; m++)
                    {
                        if (dt.Rows[m]["CDID"].ToString() != frontCDID)
                        {
                            frontCDID = dt.Rows[m]["CDID"].ToString();
                            if (frontCDID == "99")//自动外呼
                            {
                                strhtml.Append("<div class=\"jnz clearfix laboverflowvisb\"><div style=\"width: 610px;\"><input style='margin-top:7px 0px; *margin-top:7px 0px;float:left;' type=\"checkbox\" class=\"dx\" onclick='selectfirstradio(this)' /><label style='line-height: 24px;margin-top:0px;float:left;'>" + dt.Rows[m]["CallNum"].ToString() + "</label></div><div style='margin-left:20px;' class=\"jnz_wh clear\" >");
                                for (int n = 0; n < dt.Rows.Count; n++)
                                {
                                    if (dt.Rows[n]["CDID"].ToString() == frontCDID)
                                    {
                                        strhtml.Append("<span><label ><input style='margin-top:0px;*margin-top:0px;' name=\"radio_skillgroup\" id=\"radio_" + dt.Rows[n]["SGID"].ToString() + "\" type=\"radio\" value=\"" + dt.Rows[n]["SGID"].ToString() + "\" class=\"dx\" />" + dt.Rows[n]["Name"].ToString() + "</label></span>");
                                    }
                                }
                                strhtml.Append("</div><div style=\"width: 610px;line-height:1px;\"></div></div>");
                            }
                            else
                            {
                                strhtml.Append("<div class=\"jnz clearfix laboverflowvisb\"><div style=\"width: 610px;\"><input style='margin-top:7px 0px; *margin-top:7px 0px;float:left;' type=\"checkbox\" class=\"dx\" onclick='selectallcheckbox(this)' /><label style='line-height: 24px;margin-top:0px;float:left;'>" + dt.Rows[m]["CallNum"].ToString() + "</label></div><div style='margin-left:20px;' class=\"jnz_xx clear\" >");
                                for (int n = 0; n < dt.Rows.Count; n++)
                                {
                                    if (dt.Rows[n]["CDID"].ToString() == frontCDID)
                                    {
                                        strhtml.Append("<span><label ><input onclick='isallcheckcheckboxchecked(this)' style='margin-top:0px;*margin-top:0px;' name=\"checkbox_skillgroup\" id=\"checkbox_" + dt.Rows[n]["SGID"].ToString() + "\" type=\"checkbox\" value=\"" + dt.Rows[n]["SGID"].ToString() + "\" class=\"dx\" />" + dt.Rows[n]["Name"].ToString() + "</label><select name=\"select_skillgrouppriority\" id='select_skillgrouppriority_" + dt.Rows[n]["SGID"].ToString() + "'><option value='3'>高</option><option value='2'>中</option><option value='1'>低</option></select></span>");
                                    }
                                }
                                strhtml.Append("</div><div style=\"width: 610px;line-height:1px;\"></div></div>");
                            }
                        }
                    }
                }
                msg = strhtml.ToString();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage.Handler.GetAllHotlineSkillGroup()......." + ex.Message.ToString(), ex);
                msg = "";
            }
        }

        private void GetGroupByAreaID(out string msg)
        {
            msg = "";
            int intval = 0;
            if (!int.TryParse(AreaID, out intval))
            {
                return;
            }
            DataTable dt = BLL.BusinessGroup.Instance.GetBusinessGroupByAreaID(int.Parse(AreaID));

            if (dt != null)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (i == 0)
                    {
                        msg += "[";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "{BGID:" + dr["BGID"].ToString() + ",Name:'" + dr["Name"].ToString() + "'}]";
                    }
                    else
                    {
                        msg += "{BGID:" + dr["BGID"].ToString() + ",Name:'" + dr["Name"].ToString() + "'},";
                    }
                    i++;
                }
            }
        }
        /// 获取区域信息和管辖分组
        /// <summary>
        /// 获取区域信息和管辖分组
        /// </summary>
        /// <param name="msg"></param>
        private void GetAreaInfoAndManageBgs(out string msg)
        {
            msg = "";
            int userid = UserID;
            if (userid != 0)
            {
                int areaid = BLL.EmployeeAgent.Instance.GetEmployeeAgentRegionID(userid);
                if (areaid != -1)
                {
                    string managebgs = "";
                    DataTable dt = BLL.BusinessGroup.Instance.GetInUseBusinessGroup(userid);
                    if (dt != null)
                    {
                        //foreach (DataRow dr in dt.Select("RegionID='" + areaid + "'"))
                        foreach (DataRow dr in dt.Rows)
                        {
                            managebgs += dr["BGID"].ToString() + ",";
                        }
                    }
                    msg = "{managebgs:'" + managebgs.TrimEnd(',') + "',areaid:'" + areaid + "'}";
                }
            }
        }
        /// 获取用户IM或者CC系统的权限
        /// <summary>
        /// 获取用户IM或者CC系统的权限
        /// </summary>
        /// <param name="msg"></param>
        private void GetRoleIDForCCAndIMByUserid(out string msg)
        {
            msg = "";
            int userid = UserID;
            if (BusinessType != "" && userid != 0)
            {
                string cc_sysid = ConfigurationUtil.GetAppSettingValue("ThisSysID");
                string im_sysid = ConfigurationUtil.GetAppSettingValue("IMSysID");

                DataTable cc_dt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, cc_sysid);
                DataTable im_dt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, im_sysid);

                string cc_role_id = "";
                string cc_role_name = "";
                string im_role_id = "";
                string im_role_name = "";

                if (cc_dt != null && cc_dt.Rows.Count > 0)
                {
                    cc_role_id = cc_dt.Rows[0]["RoleID"].ToString();
                    cc_role_name = cc_dt.Rows[0]["RoleName"].ToString();
                }
                if (im_dt != null && im_dt.Rows.Count > 0)
                {
                    im_role_id = im_dt.Rows[0]["RoleID"].ToString();
                    im_role_name = im_dt.Rows[0]["RoleName"].ToString();
                }

                if (BusinessType == "1")
                {
                    //根据cc_role_id定位
                    msg = "{'key':'cc_role_id','value':'" + cc_role_id + "'}";
                }
                else if (BusinessType == "2")
                {
                    //根据im_role_id定位
                    msg = "{'key':'im_role_id','value':'" + im_role_id + "'}";
                }
                else if (BusinessType == "1,2" && cc_role_name == im_role_name)
                {
                    //根据cc_role_id定位
                    msg = "{'key':'cc_role_id','value':'" + cc_role_id + "'}";
                }
                else if (BusinessType == "1,2" && cc_role_name != "" && im_role_name == "")
                {
                    //根据cc_role_id定位
                    msg = "{'key':'cc_role_id','value':'" + cc_role_id + "'}";
                }
                else if (BusinessType == "1,2" && cc_role_name == "" && im_role_name != "")
                {
                    //根据im_role_id定位
                    msg = "{'key':'im_role_id','value':'" + im_role_id + "'}";
                }
            }
        }

        private void GetGroupByRegionID(out string msg)
        {
            msg = "";
            DataTable dt = null;
            if (!string.IsNullOrEmpty(AreaID))
            {
                dt = BLL.BusinessGroup.Instance.GetBusinessGroupByAreaID(int.Parse(AreaID));
            }
            else
            {
                AreaManageConfig config = new AreaManageConfig(HttpContext.Current.Server);
                List<string> list = config.GetCurrentUserArea();

                if (list != null && list.Count > 0)
                {
                    if (list.Count == 1)
                    {
                        int areaId = 1;
                        if (list[0] == "西安")
                        {
                            areaId = 2;
                        }
                        dt = BLL.BusinessGroup.Instance.GetBusinessGroupByAreaID(areaId);
                    }
                    else if (list.Count == 2)
                    {
                        dt = BLL.BusinessGroup.Instance.GetAllBusinessGroup().Select("Status=0").CopyToDataTable();
                    }
                }

            }


            if (dt != null)
            {
                //DataView dv = dt.DefaultView;
                //dv.Sort = "Name ASC";
                //dt = dv.ToTable();
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (i == 0)
                    {
                        msg += "[";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "{BGID:" + dr["BGID"].ToString() + ",Name:'" + dr["Name"].ToString() + "'}]";
                    }
                    else
                    {
                        msg += "{BGID:" + dr["BGID"].ToString() + ",Name:'" + dr["Name"].ToString() + "'},";
                    }
                    i++;
                }
            }
        }

        private void GetCurrentUserArea(out string msg)
        {
            msg = string.Empty;

            AreaManageConfig config = new AreaManageConfig(HttpContext.Current.Server);
            List<string> list = config.GetCurrentUserArea();
            if (list != null && list.Count > 0)
            {
                int i = 0;
                foreach (string str in list)
                {
                    int value = 1;
                    if (str == "西安")
                    {
                        value = 2;
                    }
                    if (i == 0)
                    {
                        msg += "[";
                    }
                    if (i == list.Count - 1)
                    {
                        msg += "{Name:'" + str + "',Value:'" + value + "'}]";
                    }
                    else
                    {
                        msg += "{Name:'" + str + "',Value:'" + value + "'},";
                    }
                    i++;
                }
            }
        }

        private void GetAgentInfoByAgeNum(out string msg)
        {
            msg = "";
            if (AgentNum != "")
            {
                Entities.QueryEmployeeAgent query = new Entities.QueryEmployeeAgent();
                query.AgentNum = AgentNum;
                int total = 0;
                DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, "", 1, 10, out total);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = "{\"UserID\":\"" + dt.Rows[0]["UserID"] + "\",\"AgentNum\":\"" + dt.Rows[0]["AgentNum"] + "\",\"BGID\":\"" + dt.Rows[0]["BGID"] + "\"}";
                }
            }
        }

        private void GetGroupDataRight(out string msg)
        {
            msg = string.Empty;
            Entities.QueryUserGroupDataRigth query = new Entities.QueryUserGroupDataRigth();
            query.UserID = UserID;
            int totalCount = 0;
            DataTable dt = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigth(query, "", 1, 1000, out totalCount);
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i == 0)
                {
                    msg += "[";
                }
                if (i == dt.Rows.Count - 1)
                {
                    msg += "{BGID:'" + dr["BGID"].ToString() + "',RightType:'" + dr["RightType"].ToString() + "'}]";
                }
                else
                {
                    msg += "{BGID:'" + dr["BGID"].ToString() + "',RightType:'" + dr["RightType"].ToString() + "'},";
                }
                i++;
            }
        }
        /// 新增或者修改分组
        /// <summary>
        /// 新增或者修改分组
        /// </summary>
        /// <param name="msg"></param>
        private void DisposeBusinessGroup(out string msg)
        {
            msg = string.Empty;
            try
            {
                Entities.BusinessGroup model = BLL.Util.BindQuery<Entities.BusinessGroup>(HttpContext.Current);
                if (model.BGID > 0)
                {
                    //修改逻辑
                    Entities.BusinessGroup info = BLL.BusinessGroup.Instance.GetBusinessGroupInfoByBGID(model.BGID);
                    if (info != null)
                    {
                        info.Name = model.Name;
                        info.RegionID = model.RegionID;
                        info.BusinessType = model.BusinessType;
                        //业务包含cc，才有外显号码
                        if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.CC) == (int)BusinessTypeEnum.CC)
                        {
                            info.CDID = model.CDID;
                        }
                        else
                        {
                            info.CDID = -2;
                        }
                        //修改组数据
                        BLL.BusinessGroup.Instance.Update(info);
                        //业务包含im时
                        if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.IM) == (int)BusinessTypeEnum.IM)
                        {
                            //清空业务线数据
                            BLL.BusinessGroupLineMapping.Instance.Delete(model.BGID);
                            //插入业务线数据
                            InsertBusinessGroupLineMapping(model.BGID, model.LineIDs);
                        }
                        else
                        {
                            //清空业务线数据
                            BLL.BusinessGroupLineMapping.Instance.Delete(model.BGID);
                        }

                        msg = "{result:'yes',BGID:'" + model.BGID + "',AreaID:'" + AreaID + "',msg:'保存成功'}";
                    }
                    else
                    {
                        msg = "{result:'no',BGID:'" + model.BGID + "',AreaID:'" + AreaID + "',msg:'没有找到对应分组信息'}";
                    }
                }
                else
                {
                    //新增逻辑
                    model.CreateTime = DateTime.Now;
                    model.Status = 0;
                    //业务包含cc，才有外显号码
                    if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.CC) != (int)BusinessTypeEnum.CC)
                    {
                        model.CDID = -2;
                    }
                    //插入组数据
                    int id = BLL.BusinessGroup.Instance.Insert(model);
                    //业务包含im时
                    if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.IM) == (int)BusinessTypeEnum.IM)
                    {
                        //插入业务线数据
                        InsertBusinessGroupLineMapping(id, model.LineIDs);
                    }

                    //创建分类
                    Entities.SurveyCategory categoryModel = new Entities.SurveyCategory();
                    categoryModel.BGID = id;
                    categoryModel.Status = -3;
                    categoryModel.CreateTime = DateTime.Now;
                    categoryModel.CreateUserID = BLL.Util.GetLoginUserID();
                    categoryModel.Name = "工单分类";
                    categoryModel.TypeId = 2;
                    int scId = BLL.SurveyCategory.Instance.Insert(categoryModel);

                    //创建业务线url地址
                    string webBaseUrl = ConfigurationUtil.GetAppSettingValue("ExitAddress");
                    webBaseUrl = webBaseUrl + "/WorkOrder/WorkOrderView.aspx?OrderID={0}";
                    BLL.CallRecord_ORIG_Business.Instance.AddBusinessUrl(id, scId, webBaseUrl);

                    msg = "{result:'yes',BGID:'" + id + "',AreaID:'" + AreaID + "',msg:'保存成功'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',BGID:'',AreaID:'',msg:'" + ex.Message + "'}";
            }
        }

        private void InsertBusinessGroupLineMapping(int bgid, string lineids)
        {
            foreach (string key in lineids.Split(','))
            {
                int id = CommonFunction.ObjectToInteger(key, -1);
                if (id >= 0)
                {
                    Entities.BusinessGroupLineMapping model = new Entities.BusinessGroupLineMapping();
                    model.BGID = bgid;
                    model.LineID = id;
                    model.Status = 0;
                    model.CreateUserID = BLL.Util.GetLoginUserID();
                    model.CreateTime = DateTime.Now;
                    BLL.BusinessGroupLineMapping.Instance.Insert(model);
                }
            }
        }

        /// 校验分组姓名是否重复
        /// <summary>
        /// 校验分组姓名是否重复
        /// </summary>
        /// <param name="msg"></param>
        private void CheckGroupNameNotUse(out string msg)
        {
            int bgid = CommonFunction.ObjectToInteger(BGID, -1);
            if (!string.IsNullOrEmpty(Name))
            {
                if (BLL.BusinessGroup.Instance.CheckGroupNameIsNotUsed(bgid, Name))
                {
                    msg = "ok";
                }
                else
                {
                    msg = "error";
                }
            }
            else
            {
                msg = "error";
            }
        }

        private void ChangeBusinessGroupStatus(out string msg)
        {
            msg = string.Empty;
            try
            {
                Entities.BusinessGroup model = BLL.Util.BindQuery<Entities.BusinessGroup>(HttpContext.Current);
                if (model.BGID > 0)
                {
                    if (model.Status == 1)
                    {
                        string where = string.Empty;
                        where += " and BGID=" + model.BGID;
                        //add by qizq 2014-12-1 取分组下人数加人员在职，部门在配置文件指定的部门及其子部门下
                        where += " and ui.Status=0 ";
                        string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("PartID");
                        int DepCount = PartIDs.Split(',').Length;
                        if (DepCount > 0)
                        {
                            where += " and (";
                            for (int i = 0; i < DepCount; i++)
                            {
                                if (i != 0)
                                {
                                    where += " or ";
                                }
                                where += " DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                            }
                            where += " )";
                        }
                        int totalCount = BLL.EmployeeAgent.Instance.GetUserCountByGroup(where);
                        if (totalCount > 0)
                        {
                            msg = "{result:'no',msg:'此业务组目前正在使用，无法进行停用操作！'}";
                            return;
                        }
                    }
                    Entities.BusinessGroup info = BLL.BusinessGroup.Instance.GetBusinessGroupInfoByBGID(model.BGID);
                    if (info != null)
                    {
                        info.Status = model.Status;
                        BLL.BusinessGroup.Instance.Update(info);
                        msg = "{result:'yes',msg:'保存成功'}";
                    }
                    else
                    {
                        msg = "{result:'no',msg:'没有找到对应分组信息'}";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        //获取外形号码
        private void GetCallDisplay(out string msg)
        {
            msg = string.Empty;
            DataTable dt = BLL.BusinessGroup.Instance.GetCallDisplay();
            if (dt != null && dt.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (i == 0)
                    {
                        msg += "[";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "{CDID:" + dr["CDID"].ToString() + ",CallNum:'" + dr["CallNum"].ToString() + "'}]";
                    }
                    else
                    {
                        msg += "{CDID:" + dr["CDID"].ToString() + ",CallNum:'" + dr["CallNum"].ToString() + "'},";
                    }
                    i++;
                }
            }
        }
        /// 获取业务线
        /// <summary>
        /// 获取业务线
        /// </summary>
        /// <param name="msg"></param>
        private void GetBusinessLineByBGID(out string msg)
        {
            int bgid = CommonFunction.ObjectToInteger(BGID);
            DataTable dt = BLL.BusinessGroup.Instance.GetBusinessLine(bgid);
            msg = "";
            foreach (DataRow dr in dt.Rows)
            {
                msg += "{'RecID':'" + dr["RecID"] + "','Name':'" + dr["Name"] + "'},";
            }
            msg = msg.TrimEnd(',');
            msg = "[" + msg + "]";
        }

        #region EmployeeAgentOption
        /// 保存用户信息的第一页内容
        /// <summary>
        /// 保存用户信息的第一页内容
        /// </summary>
        private void EmployeeAgentOptionForPageOne(out string msg)
        {
            msg = "";
            try
            {
                //解析数据
                int userid = UserID;
                string cc_sysid = ConfigurationUtil.GetAppSettingValue("ThisSysID");
                string im_sysid = ConfigurationUtil.GetAppSettingValue("IMSysID");
                string[] role_ids = UserRolesID.Split('_');
                string cc_role_id = role_ids[0];
                string im_role_id = role_ids[1];
                int btype = BLL.Util.GetMutilEnumDataValue<BusinessTypeEnum>(BusinessType);

                //清空CC和IM系统的权限
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.DeleteUserRole(userid, cc_sysid);
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.DeleteUserRole(userid, im_sysid);
                //开通CC和IM系统的权限
                if ((btype & (int)BusinessTypeEnum.CC) == (int)BusinessTypeEnum.CC)
                {
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.InsertUserRole(userid, cc_sysid, cc_role_id);
                    BLL.Loger.Log4Net.Info("【用户基本信息设置】1.更新UserID=" + userid + "的用户在CC系统中的权限RoleID为：" + cc_role_id);
                }
                if ((btype & (int)BusinessTypeEnum.IM) == (int)BusinessTypeEnum.IM)
                {
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.InsertUserRole(userid, im_sysid, im_role_id);
                    BLL.Loger.Log4Net.Info("【用户基本信息设置】2.更新UserID=" + userid + "的用户在IM系统中的权限RoleID为：" + im_role_id);
                }
                //删除其他人重复的工号
                DeleteOtherEmployeeAgentNum();
                //保存员工信息
                SaveEmployeeAgent(userid, btype);
                msg = "{'result':'success'}";
                UpdateUserSkillGroupDataRight(out msg);
                GetHeLiData(out msg);
            }
            catch (Exception ex)
            {
                msg = "{'result':'failure','msg':'" + ex.Message + "'}";
                BLL.Loger.Log4Net.Error("【用户基本信息设置】保存时，出现异常", ex);
            }
        }
        private void GetHeLiData(out string msg)
        {
            msg = string.Empty;
            string strUrl = ConfigurationUtil.GetAppSettingValue("HeLiURL") + "/busiService/addAgentInfo?"
                + "userName=" + UserName
                + "&agentId=" + AgentNum
                + "&deptId=" + AtGroupID
                + "&deptName=" + AtGroupName
                + "&skill=" + ToHeLiSGIDAndPriority;
            try
            {
                HttpWebResponse webResp = HttpHelper.CreateGetHttpResponse(strUrl);
                string data = HttpHelper.GetResponseString(webResp);
                ResultJson jsondata = (ResultJson)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(data, typeof(ResultJson));

                if (jsondata != null)
                {
                    if (jsondata.returncode == "0")
                    {
                        msg = "{'result':'failure','msg':'合力厂商接口调用失败：" + jsondata.returnmsg + "'}";
                        BLL.Loger.Log4Net.Error("【用户基本信息设置】4.合力厂商接口调用失败：访问url(" + strUrl + "),返回信息[" + jsondata.returnmsg + "]");
                    }
                    else
                    {
                        msg = "{'result':'success'}";
                        BLL.Loger.Log4Net.Info("用户基本信息设置】4.合力厂商接口调用成功：访问url(" + strUrl + ")");
                    }
                }
                else
                {
                    msg = "{'result':'failure','msg':'合力厂商接口异常，未能返回数据！'}";
                    BLL.Loger.Log4Net.Error("用户基本信息设置】4.合力厂商接口异常：访问url(" + strUrl + ")未能返回数据！");
                }
            }
            catch (Exception ex)
            {
                msg = "{'result':'failure','msg':'合力厂商接口异常，导致无法访问！'}";
                BLL.Loger.Log4Net.Error("用户基本信息设置】4.合力厂商接口异常：访问url(" + strUrl + "); ", ex);
            }
        }
        private void UpdateUserSkillGroupDataRight(out string msg)
        {
            msg = string.Empty;
            int userid = UserID;
            int loginid = BLL.Util.GetLoginUserID();
            int regionid = int.Parse(AreaID);
            if (userid > 0 && loginid > 0 && regionid > 0)
            {
                int backvalue = BLL.SkillGroupDataRight.Instance.UpdateUserSkillDataRight(userid, SGIDSAndPrioritys, loginid, regionid);
                if (backvalue > 0)
                {
                    msg = "{'result':'success'}";
                }
                else
                {
                    msg = "{'result':'failure','msg':'更新用户技能组数据时发生异常！'}";
                }
            }
            else
            {
                msg = "{'result':'failure','msg':'参数传递发生异常'}";
            }
        }
        private void DeleteOtherEmployeeAgentNum()
        {
            if (!string.IsNullOrEmpty(AgentNum))
            {
                BLL.EmployeeAgent.Instance.EmptyAgentNum(AgentNum, UserID);
            }
            //Entities.EmployeeAgent model = new Entities.EmployeeAgent();
            //Entities.QueryEmployeeAgent query = new Entities.QueryEmployeeAgent();
            //query.AgentNum = AgentNum;

            //DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgentsByAgentNum(AgentNum);

            //if (dt.Rows.Count > 0 && dt.Rows[0]["UserID"].ToString() != UserID.ToString())
            //{
            //    //与别人的工号有重复,把别人的置为空
            //    model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(int.Parse(dt.Rows[0]["UserID"].ToString()));
            //    if (model != null)
            //    {
            //        model.AgentNum = "";
            //        BLL.EmployeeAgent.Instance.Update(model);
            //    }
            //}
        }
        private void SaveEmployeeAgent(int userid, int btype)
        {
            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
            if (model != null)
            {
                //存在，更新
                model.AgentNum = AgentNum;
                model.BGID = int.Parse(AtGroupID);
                model.RegionID = int.Parse(AreaID);
                model.BusinessType = btype;

                BLL.EmployeeAgent.Instance.Update(model);
                BLL.Loger.Log4Net.Info("【用户基本信息设置】3.更新[UserID=" + userid + "]的用户的[AgentNum（工号）=" + AgentNum + "],[RegionID(所属区域)=" + AreaID + "],[BusinessType(所属业务)=" + btype + "],[BGID(所属分组)=" + AtGroupID + "]");

                //清空另一个区域的管辖分组数据
                //BLL.UserGroupDataRigth.Instance.DeleteErrorData(userid);
            }
            else
            {
                //不存在，插入
                model = new Entities.EmployeeAgent();
                model.AgentNum = AgentNum;
                model.BGID = int.Parse(AtGroupID);
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.UserID = UserID;
                model.RegionID = int.Parse(AreaID);
                model.BusinessType = btype;

                BLL.EmployeeAgent.Instance.Insert(model);

                BLL.Loger.Log4Net.Info("【用户基本信息设置】3.新增用户[UserID=" + userid + "],[AgentNum（工号）=" + AgentNum + "],[RegionID(所属区域)=" + AreaID + "],[BusinessType(所属业务)=" + btype + "],[BGID(所属分组)=" + AtGroupID + "]");
            }
        }

        /// 保存用户信息的第二页内容
        /// <summary>
        /// 保存用户信息的第二页内容
        /// </summary>
        private void EmployeeAgentOptionForPageTwo(out string msg)
        {
            msg = "";
            try
            {
                int userid = UserID;
                string[] ids_beijing = ManageBG_BeiJing.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] ids_xian = ManageBG_XiAn.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int login_userid = BLL.Util.GetLoginUserID();
                //清空管辖分组
                BLL.UserGroupDataRigth.Instance.DeleteByUserID(userid);
                //插入管辖分组 北京
                foreach (string bgid in ids_beijing)
                {
                    Entities.UserGroupDataRigth groupDataRigthModel = new Entities.UserGroupDataRigth();
                    groupDataRigthModel.CreateTime = DateTime.Now;
                    groupDataRigthModel.CreateUserID = login_userid;
                    groupDataRigthModel.UserID = userid;
                    groupDataRigthModel.BGID = CommonFunction.ObjectToInteger(bgid);
                    BLL.UserGroupDataRigth.Instance.Insert(groupDataRigthModel);
                }
                BLL.Loger.Log4Net.Info("【用户基本信息设置——数据权限设置】1.更新用户[UserID=" + userid + "]北京的管辖分组为：" + ManageBG_BeiJing + "]");
                //插入管辖分组 西安
                foreach (string bgid in ids_xian)
                {
                    Entities.UserGroupDataRigth groupDataRigthModel = new Entities.UserGroupDataRigth();
                    groupDataRigthModel.CreateTime = DateTime.Now;
                    groupDataRigthModel.CreateUserID = login_userid;
                    groupDataRigthModel.UserID = userid;
                    groupDataRigthModel.BGID = CommonFunction.ObjectToInteger(bgid);
                    BLL.UserGroupDataRigth.Instance.Insert(groupDataRigthModel);
                }
                BLL.Loger.Log4Net.Info("【用户基本信息设置——数据权限设置】2.更新用户[UserID=" + userid + "]西安的管辖分组为：" + ManageBG_XiAn + "]");
                msg = "{'result':'success'}";
            }
            catch (Exception ex)
            {
                msg = "{'result':'failure','msg':'" + ex.Message + "'}";
            }
        }

        /// 批量修改处理逻辑
        /// <summary>
        /// 批量修改处理逻辑
        /// </summary>
        /// <param name="msg"></param>
        private void EmployeeAgentMutilOption(out string msg)
        {
            msg = "";
            try
            {
                string[] userids = UserIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string cc_sysid = ConfigurationUtil.GetAppSettingValue("ThisSysID");
                string im_sysid = ConfigurationUtil.GetAppSettingValue("IMSysID");
                string[] role_ids = UserRolesID.Split('_');
                string cc_role_id = role_ids[0];
                string im_role_id = role_ids[1];
                int btype = BLL.Util.GetMutilEnumDataValue<BusinessTypeEnum>(BusinessType);
                BLL.Loger.Log4Net.Info("【批量修改用户数据权限】Start");
                //权限设置
                foreach (string key in userids)
                {
                    //批量清空权限
                    int userid = CommonFunction.ObjectToInteger(key);
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.DeleteUserRole(userid, cc_sysid);
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.DeleteUserRole(userid, im_sysid);

                    //批量设置权限
                    if ((btype & (int)BusinessTypeEnum.CC) == (int)BusinessTypeEnum.CC)
                    {
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.InsertUserRole(userid, cc_sysid, cc_role_id);
                        BLL.Loger.Log4Net.Info("【批量修改用户数据权限】Step1.更新CC系统中[UserID=" + userid + "]的用户的角色ID为：" + cc_role_id);
                    }
                    if ((btype & (int)BusinessTypeEnum.IM) == (int)BusinessTypeEnum.IM)
                    {
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.InsertUserRole(userid, im_sysid, im_role_id);
                        BLL.Loger.Log4Net.Info("【批量修改用户数据权限】Step1.更新IM系统中[UserID=" + userid + "]的用户的角色ID为：" + im_role_id);
                    }
                }
                //批量更新所属业务和所属分组
                string ids = string.Join(",", userids);
                BLL.EmployeeAgent.Instance.UpdateMutilEmployeeAgent(ids, btype, CommonFunction.ObjectToInteger(AtGroupID));
                BLL.Loger.Log4Net.Info("【批量修改用户数据权限】Step2.更新[UserID in (" + ids + ")]的用户的[BusinessType(所属业务)=" + btype + "],[BGID(所属分组)=" + AtGroupID + "]");
                foreach (string key in userids)
                {
                    int userid = CommonFunction.ObjectToInteger(key);
                    UpdateUserSkillGroupDataRightByUserID(userid, out msg);
                    string TrueNameAgentNum = BLL.EmployeeAgent.Instance.GetAgentNumberAndUserNameByUserId(userid);
                    if (!string.IsNullOrEmpty(TrueNameAgentNum))
                    {
                        string[] tnan = TrueNameAgentNum.Split(';');
                        if (tnan.Length == 2)
                        {
                            GetHeLiDataByUserNameAndAgentNum(tnan[0], tnan[1], out msg);
                        }
                        else
                        {
                            msg = "{'result':'failure','msg':'部分用户由于数据异常导致技能组数据未能传给合力厂商接口，请检查坐席的“用户名”和“工号”数据是否存在！'}";
                            BLL.Loger.Log4Net.Info("【批量修改用户数据权限】Step3.由于未能取到UserID=" + userid + "的用户的UserName或AgentNum的值，导致未能调用合力厂商的接口！");
                        }
                    }
                    else
                    {
                        msg = "{'result':'failure','msg':'部分用户由于数据异常导致技能组数据未能传给合力厂商接口，请检查坐席的“用户名”和“工号”数据是否存在！'}";
                        BLL.Loger.Log4Net.Info("【批量修改用户数据权限】Step3.由于未能取到UserID=" + userid + "的用户的UserName或AgentNum的值，导致未能调用合力厂商的接口！");
                    }
                }
                BLL.Loger.Log4Net.Info("【批量修改用户数据权限】End");
                if (msg == "")
                {
                    msg = "{'result':'success'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{'result':'failure','msg':'" + ex.Message + "'}";
            }
        }

        private void UpdateUserSkillGroupDataRightByUserID(int TheUserId, out string msg)
        {
            msg = string.Empty;
            int loginid = BLL.Util.GetLoginUserID();
            int regionid = CommonFunction.ObjectToInteger(AreaID);
            if (TheUserId > 0 && loginid > 0 && regionid > 0)
            {
                int backvalue = BLL.SkillGroupDataRight.Instance.UpdateUserSkillDataRight(TheUserId, SGIDSAndPrioritys, loginid, regionid);
                if (backvalue > 0)
                {
                    msg = "{'result':'success'}";
                }
                else
                {
                    msg = "{'result':'failure','msg':'调用UpdateUserSkillGroupDataRightByUserID（）更新用户技能组数据时发生异常！'}";
                }
            }
            else
            {
                msg = "{'result':'failure','msg':'调用UpdateUserSkillGroupDataRightByUserID（）参数传递发生异常'}";
            }
        }

        private void GetHeLiDataByUserNameAndAgentNum(string TheUserName, string TheAgentNum, out string msg)
        {
            msg = string.Empty;
            string strUrl = ConfigurationUtil.GetAppSettingValue("HeLiURL") + "/busiService/addAgentInfo?"
                + "userName=" + TheUserName
                + "&agentId=" + TheAgentNum
                + "&deptId=" + AtGroupID
                + "&deptName=" + AtGroupName
                + "&skill=" + ToHeLiSGIDAndPriority;
            try
            {
                HttpWebResponse webResp = HttpHelper.CreateGetHttpResponse(strUrl);
                string data = HttpHelper.GetResponseString(webResp);
                ResultJson jsondata = (ResultJson)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(data, typeof(ResultJson));
                if (jsondata != null)
                {
                    if (jsondata.returncode == "0")
                    {
                        msg = "{'result':'failure','msg':'合力厂商接口调用失败：" + jsondata.returnmsg + "'}";
                        BLL.Loger.Log4Net.Error("【批量修改用户数据权限】Step3.调用合力厂商接口调用失败：访问url(" + strUrl + "),返回信息[" + jsondata.returnmsg + "]");
                    }
                    else
                    {
                        msg = "{'result':'success'}";
                        BLL.Loger.Log4Net.Info("【批量修改用户数据权限】Step3.合力厂商接口调用成功：访问url(" + strUrl + ")");
                    }
                }
                else
                {
                    msg = "{'result':'failure','msg':'合力厂商接口异常，未能返回数据！'}";
                    BLL.Loger.Log4Net.Error("【批量修改用户数据权限】Step3.调用合力厂商接口异常：访问url(" + strUrl + ")未能返回数据！");
                }
            }
            catch (Exception ex)
            {
                msg = "{'result':'failure','msg':'合力厂商接口异常，导致无法访问！'}";
                BLL.Loger.Log4Net.Error("【批量修改用户数据权限】Step3.调用合力厂商接口异常：访问url(" + strUrl + "); ", ex);
            }
        }
        #endregion
    }
    public class ResultJson
    {
        public string returncode
        {
            get;
            set;
        }
        public string returnmsg
        {
            get;
            set;
        }
    }
}