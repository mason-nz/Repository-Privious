using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.SeniorManage
{
    /// <summary>
    /// ComSenManageHandler 的摘要说明
    /// </summary>
    public class ComSenManageHandler : IHttpHandler, IRequiresSessionState
    {
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? string.Empty : HttpContext.Current.Request["Action"].ToString();
            }
        }
        public string RequestDirect
        {
            get
            {
                return HttpContext.Current.Request["Direct"] == null ? string.Empty : HttpContext.Current.Request["Direct"].ToString();
            }
        }
        public int CurrentUserId =0;
        public void ProcessRequest(HttpContext context)
        {
            //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            CurrentUserId = BLL.Util.GetLoginUserID();

            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "labeltableedit":
                    LabelTableEdit(out msg);
                    break;
                case "changlabeltablestatus":
                    ChangLabelTableStatus(out msg);
                    break;
                case "isrepeatlablecs":
                    IsRepeatLableCS(out msg);
                    break;
                case "iseditcs":
                    IsEditCS(out msg);
                    break;
                case "deleteconsentence":
                    DeleteConSentence(out msg);
                    break;
                case "moveupordown":
                    MoveUpOrDown(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        private void MoveUpOrDown(out string msg)
        {
            msg = string.Empty;
            int type = RequestDirect == "up" ? 1 : -1;
            try
            {
                Entities.QueryComSentence query = BLL.Util.BindQuery<Entities.QueryComSentence>(HttpContext.Current);

                Entities.ComSentence info = BLL.ComSentence.Instance.GetComSentence(query.CSID);

                if (info != null)
                {
                    info.ModifyTime = DateTime.Now;
                    info.ModifyUserID = CurrentUserId;

                    if (BLL.ComSentence.Instance.MoveUpOrDown(query, Convert.ToInt32(info.SortNum), type))
                    {
                        msg = "{result:'yes',msg:'移动成功!'}";
                    }
                    else
                    {
                        msg = "{result:'no',msg:'移动失败!'}";
                    }
                }
                else
                {
                    msg = "{result:'no',msg:'移动失败常用语不存在!'}";
                }

            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        private void DeleteConSentence(out string msg)
        {
            msg = string.Empty;

            try
            {
                Entities.ComSentence model = BLL.Util.BindQuery<Entities.ComSentence>(HttpContext.Current);

                Entities.ComSentence info = BLL.ComSentence.Instance.GetComSentence(model.CSID);

                if (info != null)
                {               
                    info.ModifyTime = DateTime.Now;
                    info.ModifyUserID = CurrentUserId;
                    info.Status = -1;//删除状态

                    BLL.ComSentence.Instance.Update(info);
                    msg = "{result:'yes',CSID:'" + model.CSID + "',msg:'删除成功!'}";
                }
                else
                {
                    msg = "{result:'no',msg:'常用语不存在!'}";
                }

            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        private void IsEditCS(out string msg)
        {
            msg = string.Empty;

            try
            {    
                //如果变更标签 需 判断标签下是否已有相同内容的常用语
                Entities.ComSentence model = BLL.Util.BindQuery<Entities.ComSentence>(HttpContext.Current);
                if (BLL.ComSentence.Instance.IsRepeatLableCS(Convert.ToInt32(model.LTID), model.Name))//查询同一标签下常用语是否有重复
                {
                    //重复
                    msg = "{result:'no',msg:'同一标签下不能添加相同的常用语!'}";
                }
                else
                {
                    Entities.ComSentence info = BLL.ComSentence.Instance.GetComSentence(model.CSID);
                    //没有重复的 则 更新                    
                    info.ModifyTime = DateTime.Now;
                    info.ModifyUserID = CurrentUserId;
                    info.Name = model.Name;
                    info.LTID = model.LTID;

                    BLL.ComSentence.Instance.Update(info);
                    msg = "{result:'yes',CSID:'" + model.CSID + "',msg:'保存成功!'}";
                }

            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        private void IsRepeatLableCS(out string msg)
        {
            msg = string.Empty;
            try
            {
                int csid;
                Entities.ComSentence model = BLL.Util.BindQuery<Entities.ComSentence>(HttpContext.Current);
                if (BLL.ComSentence.Instance.IsLabelUsedInCS(Convert.ToInt32(model.LTID)))//常用语已使用该标签
                {
                    if (BLL.ComSentence.Instance.IsRepeatLableCS(Convert.ToInt32(model.LTID), model.Name))//查询同一标签下常用语是否有重复
                    {
                        //重复
                        msg = "{result:'no',msg:'同一标签下不能添加相同的常用语!'}";
                    }
                    else
                    {
                        //没有重复的 则 插入
                        model.CreateTime = DateTime.Now;
                        model.CreateUserID = CurrentUserId;
                        model.ModifyTime = DateTime.Now;
                        model.ModifyUserID = CurrentUserId;
                        model.Status = 0;

                        csid = BLL.ComSentence.Instance.Insert(model);
                        msg = "{result:'yes',CSID:'" + csid + "',msg:'保存成功!'}";
                    } 
                }
                else
                { 
                    //未使用 直接插入
                    //没有重复的 则 插入
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = CurrentUserId;
                    model.ModifyTime = DateTime.Now;
                    model.ModifyUserID = CurrentUserId;
                    model.Status = 0;

                    csid = BLL.ComSentence.Instance.Insert(model);

                    msg = "{result:'yes',CSID:'" + csid + "',msg:'保存成功!'}";
                }
                
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        private void ChangLabelTableStatus(out string msg)
        {
            msg = string.Empty;
            try
            {
                Entities.LabelTable model = BLL.Util.BindQuery<Entities.LabelTable>(HttpContext.Current);
                if (model.LTID > 0)
                {       
                    //停用时 是否需要判断当前标签 是否在常用语中被使用
                    //if (model.Status == 1)
                    //{                        
                    //    if (BLL.ComSentence.Instance.LabelIsUsedInCS(model.LTID))
                    //    {
                    //        msg = "{result:'no',msg:'此标签目前正在使用，无法进行停用操作！'}";
                    //        return;
                    //    }
                    //}


                    Entities.LabelTable info = BLL.LabelTable.Instance.GetLabelTable(model.LTID);
                    if (info != null)
                    {
                        info.Status = model.Status;
                        BLL.LabelTable.Instance.Update(info);
                        msg = "{result:'yes',msg:'保存成功'}";
                    }
                    else
                    {
                        msg = "{result:'no',msg:'没有找到对应标签信息'}";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        private void LabelTableEdit(out string msg)
        {
            msg = string.Empty;
            try
            {
                Entities.LabelTable model = BLL.Util.BindQuery<Entities.LabelTable>(HttpContext.Current);
                if (model.LTID > 0)
                {
                    Entities.LabelTable info = BLL.LabelTable.Instance.GetLabelTable(model.LTID);
                    if (info != null)
                    {
                        info.Name = model.Name;
                        info.ModifyTime = DateTime.Now;
                        info.ModifyUserID = CurrentUserId;
                        BLL.LabelTable.Instance.Update(info);
                        msg = "{result:'yes',LTID:'" + model.LTID + "',msg:'保存成功'}";
                    }
                    else
                    {
                        msg = "{result:'no',LTID:'" + model.LTID + "',msg:'没有找到对应标签信息'}";
                    }
                }
                else
                {
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = CurrentUserId;
                    model.ModifyUserID = CurrentUserId;
                    model.ModifyTime = DateTime.Now;
                    model.Status = 0;
                    int id = BLL.LabelTable.Instance.Insert(model);

                    msg = "{result:'yes',LTID:'" + id + "',msg:'保存成功'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',LTID:'',msg:'" + ex.Message + "'}";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}