using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public class CustBaseInfoHelper
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #region 修改客户相关属性
        public string CheckedInfoStr
        {
            get
            {
                if (Request["CheckedInfoStr"] != null)
                {

                    return HttpUtility.UrlDecode(Request["CheckedInfoStr"].ToString()).Replace(@"\", @"\\");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion



        public string TaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string TaskIDS
        {
            get
            {
                if (Request["TaskIDS"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskIDS"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string AssignUserID
        {
            get
            {
                if (Request["AssignUserID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["AssignUserID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 收回任务
        /// </summary>
        /// <param name="msg"></param>
        internal void RevokeTask(out string msg)
        {
            msg = string.Empty;

            string Result = "yes";
            string ErrorMsg = "";
            //不为空
            if (!string.IsNullOrEmpty(TaskIDS))
            {
                if (TaskIDS.IndexOf(',') > 0)
                {
                    for (int i = 0; i < TaskIDS.Split(',').Length; i++)
                    {
                        int userid = 0;
                        Entities.OrderTask Model = BLL.OrderTask.Instance.GetOrderTask(Convert.ToInt32(TaskIDS.Split(',')[i]));

                        //判断任务状态，是否为已处理
                        if (Model.TaskStatus != (int)Entities.TaskStatus.Processed)
                        {
                            //取要被收回任务的处理人
                            if (Model.AssignUserID != null && Model.AssignUserID != -2)
                            {
                                userid = Convert.ToInt32(Model.AssignUserID);
                            }
                            Model.AssignUserID = null;
                            Model.TaskStatus = (int)Entities.TaskStatus.NoAllocation;
                            BLL.OrderTask.Instance.Update(Model);
                            Entities.OrderTaskOperationLog logmodel = new Entities.OrderTaskOperationLog();
                            logmodel.TaskID = Model.TaskID;
                            logmodel.OperationStatus = (int)Entities.OperationStatus.Recover;
                            logmodel.TaskStatus = (int)Entities.TaskStatus.NoAllocation;
                            if (userid != 0)
                            {
                                logmodel.Remark = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userid);
                            }
                            logmodel.CreateTime = System.DateTime.Now;
                            logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                            BLL.OrderTaskOperationLog.Instance.Insert(logmodel);
                        }
                        else
                        {
                            Result = "no";
                            ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态为已处理，不能回收。";
                        }

                    }
                }
                else
                {
                    int userid = 0;

                    Entities.OrderTask Model = BLL.OrderTask.Instance.GetOrderTask(Convert.ToInt32(TaskIDS));

                    //判断任务状态，是否为已处理
                    if (Model.TaskStatus != (int)Entities.TaskStatus.Processed)
                    {

                        //取要被收回任务的处理人
                        if (Model.AssignUserID != null && Model.AssignUserID != -2)
                        {
                            userid = Convert.ToInt32(Model.AssignUserID);
                        }
                        Model.AssignUserID = null;
                        Model.TaskStatus = (int)Entities.TaskStatus.NoAllocation;
                        BLL.OrderTask.Instance.Update(Model);

                        Entities.OrderTaskOperationLog logmodel = new Entities.OrderTaskOperationLog();
                        logmodel.TaskID = Model.TaskID;
                        logmodel.OperationStatus = (int)Entities.OperationStatus.Recover;
                        logmodel.TaskStatus = (int)Entities.TaskStatus.NoAllocation;
                        logmodel.Remark = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userid);
                        logmodel.CreateTime = System.DateTime.Now;
                        logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                        BLL.OrderTaskOperationLog.Instance.Insert(logmodel);
                    }
                    else
                    {
                        Result = "no";
                        ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态为已处理，不能回收。";
                    }
                }
            }
            msg = "{Result:'" + Result + "',CustID:'',ErrorMsg:'" + ErrorMsg + "'}";
        }



        /// <summary>
        /// 分配任务
        /// </summary>
        /// <param name="msg"></param>
        internal void AssignTask(out string msg)
        {
            msg = string.Empty;

            string Result = "yes";
            string ErrorMsg = "";

            //不为空
            if (!string.IsNullOrEmpty(TaskIDS) && !string.IsNullOrEmpty(AssignUserID))
            {
                if (TaskIDS.IndexOf(',') > 0)
                {
                    for (int i = 0; i < TaskIDS.Split(',').Length; i++)
                    {
                        Entities.OrderTask Model = BLL.OrderTask.Instance.GetOrderTask(Convert.ToInt32(TaskIDS.Split(',')[i]));

                        if (Model.TaskStatus == (int)Entities.TaskStatus.NoAllocation)
                        {

                            Model.AssignUserID = Convert.ToInt32(AssignUserID);
                            Model.AssignTime = System.DateTime.Now;
                            Model.TaskStatus = (int)Entities.TaskStatus.NoProcess;
                            BLL.OrderTask.Instance.Update(Model);

                            //插入任务操作日志
                            Entities.OrderTaskOperationLog logmodel = new Entities.OrderTaskOperationLog();
                            logmodel.TaskID = Model.TaskID;
                            logmodel.OperationStatus = (int)Entities.OperationStatus.Allocation;
                            logmodel.TaskStatus = (int)Entities.TaskStatus.NoProcess;
                            logmodel.Remark = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(AssignUserID));
                            logmodel.CreateTime = System.DateTime.Now;
                            logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                            BLL.OrderTaskOperationLog.Instance.Insert(logmodel);
                        }
                        else
                        {
                            Result = "no";
                            ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态不是待分配。";
                        }
                    }
                }
                else
                {
                    Entities.OrderTask Model = BLL.OrderTask.Instance.GetOrderTask(Convert.ToInt32(TaskIDS));

                    if (Model.TaskStatus == (int)Entities.TaskStatus.NoAllocation)
                    {

                        Model.AssignUserID = Convert.ToInt32(AssignUserID);
                        Model.AssignTime = System.DateTime.Now;
                        Model.TaskStatus = (int)Entities.TaskStatus.NoProcess;
                        BLL.OrderTask.Instance.Update(Model);


                        //插入任务操作日志
                        Entities.OrderTaskOperationLog logmodel = new Entities.OrderTaskOperationLog();
                        logmodel.TaskID = Model.TaskID;
                        logmodel.OperationStatus = (int)Entities.OperationStatus.Allocation;
                        logmodel.TaskStatus = (int)Entities.TaskStatus.NoProcess;
                        logmodel.Remark = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(AssignUserID));
                        logmodel.CreateTime = System.DateTime.Now;
                        logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                        BLL.OrderTaskOperationLog.Instance.Insert(logmodel);
                    }
                    else
                    {
                        Result = "no";
                        ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态不是待分配。";
                    }
                }
            }
            msg = "{Result:'" + Result + "',CustID:'',ErrorMsg:'" + ErrorMsg + "'}";

        }

        internal void SubmitCheckInfo(bool isUpdate, out string msg)
        {
            msg = string.Empty;
            CheckInfo checkInfo = (CheckInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(CheckedInfoStr, typeof(CheckInfo));
            string errorMsg = string.Empty;

            if (!checkInfo.Validate(isUpdate, out errorMsg))
            {
                msg = "{Result:'no',CustID:'0',ErrorMsg:'" + errorMsg + "'}";
                return;
            }
            if (!string.IsNullOrEmpty(checkInfo.CustBaseInfo.TaskID))
            {
                //已购车
                if (checkInfo.CustBaseInfo.CustCategoryID == "2")
                {
                    Entities.OrderRelpaceCar info = BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(Convert.ToInt32(checkInfo.CustBaseInfo.TaskID));

                    if (info == null)
                    {
                        msg = "{Result:'no',CustID:'0',ErrorMsg:'没找到客户信息'}";
                        return;
                    }
                    else
                    {
                        //记录更新之前的信息
                        StringBuilder sbUpdate = new StringBuilder();
                        sbUpdate.Append("修改客户【" + info.TaskID + "】:");
                        if (info.UserName != checkInfo.CustBaseInfo.CustName)
                        {
                            sbUpdate.Append("把姓名【" + info.UserName + "】修改为【" + checkInfo.CustBaseInfo.CustName + "】,");
                        }
                        if (info.UserGender != Convert.ToInt32(checkInfo.CustBaseInfo.Sex))
                        {
                            sbUpdate.Append("把性别" + info.UserGender == "1" ? "【男】" : "【女】" + "修改为" + checkInfo.CustBaseInfo == "1" ? "【男】" : "【女】" + ",");
                        }
                        if (info.AreaID != Convert.ToInt32(checkInfo.CustBaseInfo.AreaID))
                        {
                            sbUpdate.Append("把区域【" + BLL.Util.GetEnumOptText(typeof(Entities.EnumArea), (int)info.AreaID) + "】修改为【" + BLL.Util.GetEnumOptText(typeof(Entities.EnumArea), Convert.ToInt32(checkInfo.CustBaseInfo.AreaID)) + "】,");
                        }
                        if (info.ProvinceID != Convert.ToInt32(checkInfo.CustBaseInfo.ProvinceID))
                        {
                            sbUpdate.Append("把省【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(info.ProvinceID.ToString()) + "】修改为【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(checkInfo.CustBaseInfo.ProvinceID.ToString()) + "】,");
                        }
                        if (info.CityID != Convert.ToInt32(checkInfo.CustBaseInfo.CityID))
                        {
                            sbUpdate.Append("把市【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(info.CityID.ToString()) + "】修改为【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(checkInfo.CustBaseInfo.CityID.ToString()) + "】,");
                        }
                        if (info.CountyID != Convert.ToInt32(checkInfo.CustBaseInfo.CountyID))
                        {
                            sbUpdate.Append("把县【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(info.CountyID.ToString()) + "】修改为【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(checkInfo.CustBaseInfo.CountyID.ToString()) + "】,");
                        }
                        info.UserAddress = checkInfo.CustBaseInfo.Address;
                        info.AreaID = Convert.ToInt32(checkInfo.CustBaseInfo.AreaID);
                        info.CityID = Convert.ToInt32(checkInfo.CustBaseInfo.CityID);
                        info.CountyID = Convert.ToInt32(checkInfo.CustBaseInfo.CountyID);
                        info.UserName = checkInfo.CustBaseInfo.CustName;
                        info.ProvinceID = Convert.ToInt32(checkInfo.CustBaseInfo.ProvinceID);
                        info.UserGender = Convert.ToInt32(checkInfo.CustBaseInfo.Sex);
                        if (info.UserPhone != checkInfo.CustBaseInfo.UserPhone)
                        {
                            sbUpdate.Append("把电话【" + info.UserPhone + "】修改为【" + checkInfo.CustBaseInfo.UserPhone + "】,");
                        }
                        info.UserPhone = checkInfo.CustBaseInfo.UserPhone;
                        if (info.UserMobile != checkInfo.CustBaseInfo.UserMobile)
                        {
                            sbUpdate.Append("把手机【" + info.UserMobile + "】修改为【" + checkInfo.CustBaseInfo.UserMobile + "】,");
                        }
                        info.UserMobile = checkInfo.CustBaseInfo.UserMobile;
                        if (info.UserMail != checkInfo.CustBaseInfo.Email)
                        {
                            sbUpdate.Append("把邮箱【" + info.UserMail + "】修改为【" + checkInfo.CustBaseInfo.Email + "】,");
                        }
                        info.UserMail = checkInfo.CustBaseInfo.Email;
                        BLL.OrderRelpaceCar.Instance.Update(info);

                        //插入日志记录
                        BLL.Util.InsertUserLog(sbUpdate.ToString());
                    }

                }
                //未购车
                else
                {
                    Entities.OrderNewCar info = BLL.OrderNewCar.Instance.GetOrderNewCar(Convert.ToInt32(checkInfo.CustBaseInfo.TaskID));
                    if (info == null)
                    {
                        msg = "{Result:'no',CustID:'0',ErrorMsg:'没找到客户信息'}";
                        return;
                    }
                    else
                    {
                        //记录更新之前的信息
                        StringBuilder sbUpdate = new StringBuilder();
                        sbUpdate.Append("修改客户【" + info.TaskID + "】:");
                        if (info.UserName != checkInfo.CustBaseInfo.CustName)
                        {
                            sbUpdate.Append("把姓名【" + info.UserName + "】修改为【" + checkInfo.CustBaseInfo.CustName + "】,");
                        }
                        if (info.UserGender != Convert.ToInt32(checkInfo.CustBaseInfo.Sex))
                        {
                            sbUpdate.Append("把性别" + info.UserGender == "1" ? "【男】" : "【女】" + "修改为" + checkInfo.CustBaseInfo == "1" ? "【男】" : "【女】" + ",");
                        }
                        if (info.AreaID != Convert.ToInt32(checkInfo.CustBaseInfo.AreaID))
                        {
                            sbUpdate.Append("把区域【" + BLL.Util.GetEnumOptText(typeof(Entities.EnumArea), (int)info.AreaID) + "】修改为【" + BLL.Util.GetEnumOptText(typeof(Entities.EnumArea), Convert.ToInt32(checkInfo.CustBaseInfo.AreaID)) + "】,");
                        }
                        if (info.ProvinceID != Convert.ToInt32(checkInfo.CustBaseInfo.ProvinceID))
                        {
                            sbUpdate.Append("把省【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(info.ProvinceID.ToString()) + "】修改为【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(checkInfo.CustBaseInfo.ProvinceID.ToString()) + "】,");
                        }
                        if (info.CityID != Convert.ToInt32(checkInfo.CustBaseInfo.CityID))
                        {
                            sbUpdate.Append("把市【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(info.CityID.ToString()) + "】修改为【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(checkInfo.CustBaseInfo.CityID.ToString()) + "】,");
                        }
                        if (info.CountyID != Convert.ToInt32(checkInfo.CustBaseInfo.CountyID))
                        {
                            sbUpdate.Append("把县【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(info.CountyID.ToString()) + "】修改为【" + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(checkInfo.CustBaseInfo.CountyID.ToString()) + "】,");
                        }
                        info.UserAddress = checkInfo.CustBaseInfo.Address;
                        info.AreaID = Convert.ToInt32(checkInfo.CustBaseInfo.AreaID);
                        info.CityID = Convert.ToInt32(checkInfo.CustBaseInfo.CityID);
                        info.CountyID = Convert.ToInt32(checkInfo.CustBaseInfo.CountyID);
                        info.UserName = checkInfo.CustBaseInfo.CustName;
                        info.ProvinceID = Convert.ToInt32(checkInfo.CustBaseInfo.ProvinceID);
                        info.UserGender = Convert.ToInt32(checkInfo.CustBaseInfo.Sex);
                        if (info.UserPhone != checkInfo.CustBaseInfo.UserPhone)
                        {
                            sbUpdate.Append("把电话【" + info.UserPhone + "】修改为【" + checkInfo.CustBaseInfo.UserPhone + "】,");
                        }
                        info.UserPhone = checkInfo.CustBaseInfo.UserPhone;
                        if (info.UserMobile != checkInfo.CustBaseInfo.UserMobile)
                        {
                            sbUpdate.Append("把手机【" + info.UserMobile + "】修改为【" + checkInfo.CustBaseInfo.UserMobile + "】,");
                        }
                        info.UserMobile = checkInfo.CustBaseInfo.UserMobile;
                        if (info.UserMail != checkInfo.CustBaseInfo.Email)
                        {
                            sbUpdate.Append("把邮箱【" + info.UserMail + "】修改为【" + checkInfo.CustBaseInfo.Email + "】,");
                        }
                        info.UserMail = checkInfo.CustBaseInfo.Email;
                        BLL.OrderNewCar.Instance.Update(info);

                        //插入日志记录
                        BLL.Util.InsertUserLog(sbUpdate.ToString());
                    }

                }
                msg = "{Result:'yes',CustID:'',ErrorMsg:'" + errorMsg + "'}";

            }

        }



        ////根据Hr系统EID找到权限系统UserID
        //private int GetSysUserIDByEID(int eid)
        //{
        //    int User_ID = 0;
        //    //根据EID找到域账号
        //    string domainAccount = BLL.Util.GetEmployeeDomainAccountByEid(eid);
        //    //根据域账号找到权限系统该人员的主键
        //    User_ID = UserInfo.Instance.GetUserIDByNameDomainAccount(domainAccount);
        //    return User_ID;
        //}
    }

    public class CheckInfo
    {
        public CheckCustBaseInfo CustBaseInfo;

        public bool Validate(bool isUpdate, out string msg)
        {
            //Regex reTel = new Regex(@"(^0[0-9]{2,3}[0-9]{7,8}$)|(^13[0-9]{9}$)|(^15[0-9]{9}$)|(^18[0-9]{9}$)|(^19[0-9]{9}$)|(^14[0-9]{9}$)|(^400\d{7}$)");
            //Regex reEmail = new Regex(@"/^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/");
            msg = string.Empty;
            #region 客户基础信息验证
            if (string.IsNullOrEmpty(CustBaseInfo.CustCategoryID))
            {
                msg = "请选择客户分类";
                return false;
            }
            if (isUpdate)
            {
                //置换
                if (CustBaseInfo.CustCategoryID == "2")
                {

                    if (BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(Convert.ToInt32(CustBaseInfo.TaskID)) == null)
                    {
                        msg = "不存在此客户";
                        return false;
                    }
                }
                else
                {
                    if (BLL.OrderNewCar.Instance.GetOrderNewCar(Convert.ToInt32(CustBaseInfo.TaskID)) == null)
                    {
                        msg = "不存在此客户";
                        return false;
                    }
                }
            }
            //if (string.IsNullOrEmpty(CustBaseInfo.CustName.Trim()))
            //{
            //    msg = "姓名不能为空";
            //    return false;
            //}
            //if (string.IsNullOrEmpty(CustBaseInfo.CustTels.Trim()))
            //{
            //    msg = "电话不能为空";
            //    return false;
            //}
            if (!string.IsNullOrEmpty(CustBaseInfo.UserPhone))
            {

                if (!BLL.Util.IsTelephoneAnd400Tel(CustBaseInfo.UserPhone))
                {
                    msg = "电话(" + CustBaseInfo.UserPhone + ")不符合规则";
                    return false;
                }

            }
            if (!string.IsNullOrEmpty(CustBaseInfo.UserMobile))
            {

                if (!BLL.Util.IsTelephoneAnd400Tel(CustBaseInfo.UserMobile))
                {
                    msg = "手机(" + CustBaseInfo.UserMobile + ")不符合规则";
                    return false;
                }

            }
            //if (string.IsNullOrEmpty(CustBaseInfo.Sex) && CustBaseInfo.Sex == "0")
            //{
            //    msg = "请选择性别";
            //    return false;
            //}
            if (!string.IsNullOrEmpty(CustBaseInfo.Address.Trim()) && CustBaseInfo.Address.Trim().Length > 200)
            {
                msg = "客户地址长度不能超过200个字符";
                return false;
            }
            if (!string.IsNullOrEmpty(CustBaseInfo.Email.Trim()) && !BLL.Util.IsEmail(CustBaseInfo.Email.Trim()))
            {
                msg = "客户邮箱格式不正确";
                return false;
            }
            #endregion

            return true;
        }

        public class CheckCustBaseInfo
        {
            private string taskID;
            public string TaskID
            {
                get { return taskID; }
                set { taskID = HttpUtility.UrlDecode(value); }
            }
            private string custName;
            public string CustName
            {
                get { return custName; }
                set { custName = HttpUtility.UrlDecode(value); }
            }
            private string sex;
            public string Sex
            {
                get { return sex; }
                set { sex = HttpUtility.UrlDecode(value); }
            }
            private string custCategoryID;
            public string CustCategoryID
            {
                get { return custCategoryID; }
                set { custCategoryID = HttpUtility.UrlDecode(value); }
            }
            private string provinceID;
            public string ProvinceID
            {
                get { return provinceID; }
                set { provinceID = HttpUtility.UrlDecode(value); }
            }
            private string cityID;
            public string CityID
            {
                get { return cityID; }
                set { cityID = HttpUtility.UrlDecode(value); }
            }
            private string countyID;
            public string CountyID
            {
                get { return countyID; }
                set { countyID = HttpUtility.UrlDecode(value); }
            }
            private string areaID;
            public string AreaID
            {
                get { return areaID; }
                set { areaID = HttpUtility.UrlDecode(value); }
            }
            private string address;
            public string Address
            {
                get { return address; }
                set { address = HttpUtility.UrlDecode(value); }
            }
            private string custTels;
            public string CustTels
            {
                get { return custTels; }
                set { custTels = HttpUtility.UrlDecode(value); }
            }

            private string userphone;
            public string UserPhone
            {
                get { return userphone; }
                set { userphone = HttpUtility.UrlDecode(value); }
            }
            private string userMobile;
            public string UserMobile
            {
                get { return userMobile; }
                set { userMobile = HttpUtility.UrlDecode(value); }
            }

            private string email;
            public string Email
            {
                get { return email; }
                set { email = HttpUtility.UrlDecode(value); }
            }
        }
    }
}