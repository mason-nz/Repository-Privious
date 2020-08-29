using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization
{
    /// <summary>
    /// PersonalizationHandler 的摘要说明
    /// </summary>
    public class PersonalizationHandler : IHttpHandler, IRequiresSessionState
    {
        public string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Action").ToString();
            }
        }
        public string KLFavoritesId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("KLFavoritesId").ToString();
            }
        }
        public string KLID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("KLID").ToString();
            }
        }
        #region  提问
        public string QuestionId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("QuestionId").ToString();
            }
        }
        public string QuestionTitle
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("QuestionTitle").ToString();
            }
        }
        public string QuestionCid
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("QuestionCid").ToString();
            }
        }
        public string QuestionType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("QuestionType").ToString();
            }
        }
        public string QuestionDetails
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("QuestionDetails").ToString();
            }
        }
        #endregion

        #region 收藏
        public string CollectUserId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CollectUserId").ToString();
            }
        }
        public string CollectRefId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CollectRefId").ToString();
            }
        }
        public string CollectType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CollectType").ToString();
            }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            string msg = string.Empty;
            int userID = BLL.Util.GetLoginUserID();
            switch (Action.ToLower())
            {
                case "cancelcollect":
                    if (!BLL.Util.CheckRight(userID, "SYS024MOD3501"))
                    {
                        msg = "您没有执行此操作的权限";
                    }
                    else
                    {
                        CancelCollection(KLFavoritesId, out msg);
                    }
                    break;
                case "addquestion":
                    if (BLL.Util.CheckRight(userID, "SYS024MOD3001") || BLL.Util.CheckRight(userID, "SYS024MOD3002") || BLL.Util.CheckRight(userID, "SYS024MOD3501"))
                    {
                        AddNewQuestion(out msg);
                    }
                    else
                    {
                        msg = "您没有执行此操作的权限";
                    }
                    break;
                case "addcollection":
                    //需要的参数：CollectUserId，CollectRefId，CollectType
                    //返回”success“表示收藏成功

                    AddCollection(out msg);
                    break;
                case "deletequestion":
                    if (!BLL.Util.CheckRight(userID, "SYS024MOD3608"))
                    {
                        msg = "您没有执行此操作的权限";
                    }
                    else
                    {
                        DeleteQuestion(out msg);
                    }
                    break;
                case "getansweres":
                    GetAnsweresData(out msg);
                    break;
                case "updatequestion":
                    AnswerQuestion(out msg);
                    break;
                case "answerquestion":
                    AnswerQuestion(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        private void AnswerQuestion(out string msg)
        {
            int intQuestionId;
            if (int.TryParse(QuestionId, out intQuestionId))
            {
                DataTable dt = BLL.Personalization.Instance.GetKLRaiseQuestionModelDataById(intQuestionId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    KLRaiseQuestions entity = new KLRaiseQuestions();
                    if (Action.ToLower() == "updatequestion")
                    {
                        entity.Id = intQuestionId;
                        entity.CreateUserId = int.Parse(dt.Rows[0]["CreateUserId"].ToString());
                        entity.CreateDate = Convert.ToDateTime(dt.Rows[0]["CreateDate"].ToString());
                        entity.Title = dt.Rows[0]["Title"].ToString();
                        entity.CONTENT = QuestionDetails;
                        entity.KLCId = int.Parse(dt.Rows[0]["KLCId"].ToString());
                        entity.KLRefId = int.Parse(dt.Rows[0]["KLRefId"].ToString());
                        entity.Type = int.Parse(dt.Rows[0]["Type"].ToString());
                        entity.Status = int.Parse(dt.Rows[0]["Status"].ToString());
                        entity.AnswerUser = dt.Rows[0]["AnswerUser"] == null ? -2 : int.Parse(dt.Rows[0]["AnswerUser"].ToString());
                        entity.BGID = dt.Rows[0]["BGID"] == null ? -2 : int.Parse(dt.Rows[0]["BGID"].ToString());
                        entity.LastModifyDate = DateTime.Now;
                        entity.LastModifyBy = BLL.Util.GetLoginUserID();
                    }
                    else
                    {
                        int userId = BLL.Util.GetLoginUserID();
                        entity.Id = intQuestionId;
                        entity.CreateUserId = int.Parse(dt.Rows[0]["CreateUserId"].ToString());
                        entity.CreateDate = Convert.ToDateTime(dt.Rows[0]["CreateDate"].ToString());
                        entity.Title = dt.Rows[0]["Title"].ToString();
                        entity.CONTENT = QuestionDetails;
                        entity.KLCId = int.Parse(dt.Rows[0]["KLCId"].ToString());
                        entity.KLRefId = int.Parse(dt.Rows[0]["KLRefId"].ToString());
                        entity.Type = int.Parse(dt.Rows[0]["Type"].ToString());
                        entity.Status = 1;
                        entity.AnswerUser = userId;
                        entity.BGID = int.Parse(BLL.EmployeeSuper.Instance.GetEmployeeAgent(userId).Rows[0]["BGID"].ToString());
                        entity.LastModifyDate = DateTime.Now;
                        entity.LastModifyBy = userId;
                    }

                    int backData = BLL.Personalization.Instance.UpdateKLRaiseQuestion(entity);
                    if (backData == 0)
                    {
                        msg = "提交失败";
                    }
                    else
                    {
                        msg = "提交成功";
                    }
                }
                else
                {
                    msg = "该条问题已不存在";
                }
            }
            else
            {
                msg = "传入数据格式不正确";
            }
        }

        private void GetAnsweresData(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BLL.Personalization.Instance.GetAnswerUserListData();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(dr[0].ToString()));

                msg += i == 0 ? "[{AnswerUser:'" + dr[0] + "',TrueName:'" + userName + "'}" : ",{AnswerUser:'" + dr[0] + "',TrueName:'" + userName + "'}";

                if (i == dt.Rows.Count - 1)
                {
                    msg += "]";
                }

            }
        }

        private void DeleteQuestion(out string msg)
        {
            int id;
            if (int.TryParse(QuestionId, out id))
            {
                if (BLL.Personalization.Instance.DeleteKLRaiseQuestionById(id) > 0)
                {
                    msg = "success";
                }
                else
                {
                    msg = "删除提问失败";
                }
            }
            else
            {
                msg = "参数格式有问题";
            }
        }

        private void AddCollection(out string msg)
        {
            int userId;
            int refId;
            int type;
            if (int.TryParse(CollectRefId, out refId) && int.TryParse(CollectType, out type))
            {
                Entities.QueryKLFavorites query = new Entities.QueryKLFavorites();
                query.UserId = BLL.Util.GetLoginUserID();
                query.KLRefId = refId;
                query.Type = type;
                bool isCollected = BLL.Personalization.Instance.IsCollected(query);
                if (isCollected)
                {
                    msg = "您已收藏了该条数据！";
                }
                else
                {
                    query.CreateTime = DateTime.Now;
                    msg = BLL.Personalization.Instance.Insert(query) > 0 ? "success" : "收藏失败！";
                }
            }
            else
            {
                msg = "数据格式有误！";
            }
        }

        private void AddNewQuestion(out string msg)
        {
            Entities.KLRaiseQuestions entity = new Entities.KLRaiseQuestions();
            entity.CreateUserId = BLL.Util.GetLoginUserID();
            entity.CreateDate = DateTime.Now;
            entity.Title = QuestionTitle;
            entity.CONTENT = QuestionDetails;
            entity.KLCId = int.Parse(QuestionCid); //0:知识点，1:FAQ
            entity.KLRefId = int.Parse(KLID);
            entity.Type = int.Parse(QuestionType);
            entity.Status = 0;
            DataTable dt = BLL.EmployeeSuper.Instance.GetEmployeeAgent(entity.CreateUserId);
            if (dt != null && dt.Rows.Count > 0)
            {
                entity.BGID = int.Parse(dt.Rows[0]["BGID"].ToString());
            };

            //entity.AnswerUser = -1;
            //entity.LastModifyDate = DateTime.Now;
            //entity.LastModifyBy = -1; ;

            msg = BLL.Personalization.Instance.InsertKLRaiseQuestion(entity) > 0 ? "success" : "提问失败！";
        }


        private void CancelCollection(string KLFavoritesId, out string msg)
        {
            int klfId;
            if (int.TryParse(KLFavoritesId, out klfId))
            {
                msg = BLL.Personalization.Instance.Delete(klfId) > 0 ? "success" : "取消收藏失败";
            }
            else
            {
                msg = "数据异常";
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