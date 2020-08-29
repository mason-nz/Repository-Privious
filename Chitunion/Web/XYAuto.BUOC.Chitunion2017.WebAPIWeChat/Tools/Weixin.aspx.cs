using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using static XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Tools
{
    public partial class Weixin : System.Web.UI.Page
    {
        //private string WeixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];
        //private string WeixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private string CutHeadImage = ConfigurationManager.AppSettings["CutHeadImage"];
        private string Code = "DFC43B70-8A43-49BF-B4B0-A6FE5CC8AE2C";
        private int QRCodeWidthOrHeight = 430;


        protected void Page_Load(object sender, EventArgs e)
        {
            //XYAuto.ITSC.Chitunion2017.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                VerifyLogic();
            }
        }

        protected bool VerifyLogic()
        {
            //XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();
            if (Request.QueryString["code"] != null && Request.QueryString["code"] == Code)
            {
                string wxNum = ddlWxNum.Value;
                if (wxNum == "-1")
                {
                    XYAuto.Utils.ScriptHelper.ShowAlertScript(this.Page, "请选择一个公众号");
                    return false;
                }
                return true;
            }
            else
            {
                XYAuto.Utils.ScriptHelper.ShowAlertScript(this.Page, "当前登陆人权限不足，无法访问");
                return false;
            }
        }

        protected void btnGenQrCodeUrl_Click(object sender, EventArgs e)
        {
            //XYAuto.ITSC.Chitunion2017.Common.UserInfo.Check();
            if (VerifyLogic())
            {
                string activityName = txtActivityName.Value;
                if (string.IsNullOrEmpty(activityName))
                {
                    XYAuto.Utils.ScriptHelper.ShowAlertScript(this.Page, "当前输入的推广活动名称为空，请重新输入");
                    return;
                }

                string qr = CreateQrCode(activityName);
                string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, QRCodeWidthOrHeight, QRCodeWidthOrHeight);
                string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(CutHeadImage, postdata);
                ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
                litActivityQrCodeUrl.Text = info.Result.CutImageUrl;
                litActivityQrCodeUrl.Visible = true;
                imgActivityQrCode.Src = info.Result.CutImageUrl;
                imgActivityQrCode.Visible = true;
            }
        }


        protected void btnGenImg_Click(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFile)
            {
                XYAuto.Utils.ScriptHelper.ShowAlertScript(this.Page, "请选择一个图片文件");
                return;
            }
            else
            {
                string weixinAppId = ddlWxNum.Value.Split('|')[0];
                string weixinAppSecret = ddlWxNum.Value.Split('|')[1];
                AccessTokenContainer.Register(weixinAppId, weixinAppSecret);
                string filePath = Server.MapPath("/GenImgTemp/");
                filePath += DateTime.Now.ToString("yyyy-MM-dd") + "/";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath += FileUpload1.FileName;
                FileUpload1.SaveAs(filePath);
                //string file = Server.MapPath(txtAddImageUrl.Value);
                var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadForeverMedia(weixinAppId, filePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                litGenImg.Text = uploadResult.media_id + " | url= " + uploadResult.url;
                litGenImg.Visible = true;
                imgForver.Src = uploadResult.url;
                imgForver.Visible = true;
            }
        }

        protected void btnGenQrCodeUrl2_Click(object sender, EventArgs e)
        {
            //XYAuto.ITSC.Chitunion2017.Common.UserInfo.Check();
            if (VerifyLogic())
            {
                string mobile = txtMobile.Value;
                if (string.IsNullOrEmpty(mobile))
                {
                    XYAuto.Utils.ScriptHelper.ShowAlertScript(this.Page, "当前输入的用户手机号为空，请重新输入");
                    return;
                }
                int userID = -2;
                userID = XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserIdByMobile(mobile, (int)UserCategoryEnum.媒体主);
                if (userID <= 0)
                {
                    XYAuto.Utils.ScriptHelper.ShowAlertScript(this.Page, "当前根据用户手机号查不到用户，请重新输入");
                    return;
                }
                string weixinAppId = ddlWxNum.Value.Split('|')[0];
                string weixinAppSecret = ddlWxNum.Value.Split('|')[1];
                AccessTokenContainer.Register(weixinAppId, weixinAppSecret);
                string imgurl = CreateQrCode2(weixinAppId, userID);
                litActivityQrCodeUr2.Text = imgurl;
                litActivityQrCodeUr2.Visible = true;
                imgActivityQrCod2.Src = imgurl;
                imgActivityQrCod2.Visible = true;
            }
        }
        private string CreateQrCode(string activityName, Senparc.Weixin.MP.QrCode_ActionName actionname = Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE)
        {

            //string appId = ConfigurationManager.AppSettings["WeixinAppId"];
            //string WeixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];
            string weixinAppId = ddlWxNum.Value.Split('|')[0];
            string weixinAppSecret = ddlWxNum.Value.Split('|')[1];
            AccessTokenContainer.Register(weixinAppId, weixinAppSecret);
            try
            {
                StringBuilder json = new StringBuilder();
                json.Append("{").AppendFormat("\"t\":\"{0}\",", (int)ReqMessageType.场景).AppendFormat("\"v\":\"{0}\"", activityName.Trim()).Append("}");
                var qrCodeResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(weixinAppId, 0, 1, actionname, json.ToString());
                return QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[WeChatTools CreateQrCode]报错", ex);
                return null;
            }
        }

        public string CreateQrCode2(string appId, int userID, Senparc.Weixin.MP.QrCode_ActionName actionname = Senparc.Weixin.MP.QrCode_ActionName.QR_SCENE)
        {
            try
            {
                StringBuilder json = new StringBuilder();
                var qrCodeResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(appId, 2592000, userID, actionname, json.ToString());
                return QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[CreateQrCode2]报错", ex);
                return string.Empty;
            }
        }
    }
}