using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpertPdf.HtmlToPdf;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage
{
    public partial class ExamPaperPDF : PageBase
    {
        public string RequestEPID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["epid"]))
                {
                    return Request["epid"];
                }
                else
                {
                    return "1";
                }
            }
        }
        public string Paper
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["paper"]))
                {
                    return Request["paper"];
                }
                else
                {
                    return "";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Paper))
            {
                DownLoadPDF();
            }
            else
            {
                DownLoadPaperPDF();
            }

        }
        protected void btnPDF_Click222(object sender, EventArgs e)
        {
            string downloadName = "Report";
            byte[] downloadBytes = null;
            downloadName += ".pdf";
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfStandardSubset = PdfStandardSubset.Full;
            pdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
            pdfConverter.PdfDocumentOptions.EmbedFonts = true;
            pdfConverter.PdfDocumentOptions.BottomMargin = 80;
            pdfConverter.PdfDocumentOptions.TopMargin = 50;

            string url = string.Format("http://{0}/ExamOnline/ExamPaperStorage/ExamPaperView.aspx?epid=39&isPdf=1", Request.Url.Host);
            downloadBytes = pdfConverter.GetPdfBytesFromUrl(url);

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader("Content-Disposition",
                "attachment; filename=" + downloadName + "; size=" + downloadBytes.Length.ToString());
            response.Flush();
            response.BinaryWrite(downloadBytes);
            response.Flush();
            response.End();
        }

        private void DownLoadPaperPDF()
        {
            string url = string.Format("http://{0}/ExamOnline/ExamPaperStorage/ExamPaperView.aspx?epid={1}&isPdf=1", Request.Url.Host, RequestEPID);

            BLL.Loger.Log4Net.Info("试卷导出PDF，访问URL：" + url);
            byte[] downloadBytes = new byte[] { };
            string downloadName = "文字版试卷.pdf";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.KeepAlive = true;

                Uri uri = new Uri(url);
                CookieContainer cookies = new CookieContainer();
                string name = BLL.Util.SysLoginCookieName;
                cookies.Add(uri, new Cookie(name, HttpContext.Current.Request.Cookies[name].Value));
                request.CookieContainer = cookies;
                var s = request.GetResponse();
                var r = new StreamReader(s.GetResponseStream(), Encoding.UTF8);

                var htmlText = r.ReadToEnd();
                BLL.Loger.Log4Net.Info("试卷导出PDF—生成前文本内容：" + htmlText);

                htmlText = htmlText.Replace("<input name=", "<span name=");
                int iV = htmlText.IndexOf("<div class=\"taskT\">");
                htmlText = htmlText.Replace("<div class=\"taskT\">", "");
                if (iV > 0)
                {
                    int iVLast = htmlText.IndexOf("div>", iV);
                    if (iVLast > 0)
                    {
                        htmlText = htmlText.Substring(0, iV + 1) + htmlText.Substring(iVLast + 4);
                    }
                }

                int iName = htmlText.IndexOf("iname=\"1\"");
                if (iName > 0)
                {
                    int iNameE = htmlText.IndexOf("</b>", iName);
                    if (iNameE > 0)
                    {
                        downloadName = HttpUtility.UrlEncode(htmlText.Substring(iName + 9 + 1, iNameE - iName - 10).Trim(), Encoding.UTF8).Replace("+", "%20") + ".pdf";        
                    }
                }
                BLL.Loger.Log4Net.Info("试卷导出PDF—生成文件名：" + downloadName);
                PdfConverter pdfConverter = GetPdfConvert();
                downloadBytes = pdfConverter.GetPdfBytesFromHtmlString(htmlText);
            }
            catch (Exception ex)
            {
                downloadBytes = new byte[] { };
                BLL.Loger.Log4Net.Error("在页面ExamPaperPDF.aspx 报错：" + ex.Message,ex);
            }

            try
            {
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.AddHeader("Content-Type", "binary/octet-stream");
                response.AddHeader("Content-Disposition",
                    "attachment; filename=" + downloadName + "; size=" + downloadBytes.Length.ToString());
                response.Flush();
                response.BinaryWrite(downloadBytes);
                response.Flush();
                response.End();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("导出试卷pdf", ex);
            }
        }



        private void DownLoadPDF()
        {
            string url = string.Format("http://{0}/ExamOnline/ExamPaperStorage/ExamPaperView.aspx?epid={1}&isPdf=1", Request.Url.Host, RequestEPID);

            #region Cookies添加
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);            
            //request.Credentials = CredentialCache.DefaultCredentials;
            //request.Headers.Add("Cookie", Request.Headers["Cookie"]);

            //request.CookieContainer = new CookieContainer();
            ////string strCookieFilter = ".ASPXAUTH,BitAutoLogId,BitAutoUserCode,ASP.NET_SessionId,mmploginusername";
            //string strCookieFilter = "BitAutoLogId,BitAutoUserCode,ASP.NET_SessionId,mmploginusername";
            //HttpCookieCollection oCookies = Context.Request.Cookies;
            //for (int j = 0; j < oCookies.Count; j++)
            //{
            //    HttpCookie oCookie = oCookies.Get(j);

            //    if (strCookieFilter.IndexOf(oCookie.Name) > -1)
            //    {
            //        Cookie oC = new Cookie();

            //        // Convert between the System.Net.Cookie to a System.Web.HttpCookie...
            //        oC.Domain = Context.Request.Url.Host; // myRequest.RequestUri.Host;
            //        oC.Expires = oCookie.Expires;
            //        oC.Name = oCookie.Name;
            //        oC.Path = oCookie.Path;
            //        oC.Secure = oCookie.Secure;
            //        oC.Value = oCookie.Value.Replace(",", "%2C");

            //        request.CookieContainer.Add(oC);
            //    }
            //}

            //var s = request.GetResponse();
            //var r = new StreamReader(s.GetResponseStream(), Encoding.UTF8);


            //var htmlText = r.ReadToEnd();

            //htmlText = htmlText.Replace("<input name=", "<span name=");
            //htmlText = htmlText.Replace("<div class=\"taskT\">试卷查看</div>", "");

            #endregion
            byte[] downloadBytes = new byte[] { };
            string downloadName = "网页版试卷.pdf";
            try
            {

                PdfConverter pdfConverter = GetPdfConvert();
                //var downloadBytes = pdfConverter.GetPdfBytesFromHtmlString(htmlText);
                //var downloadBytes = pdfConverter.GetPdfBytesFromUrl("http://ncc.sys1.bitauto.com/AjaxServers/ExamOnline/tt.htm");
                downloadBytes = pdfConverter.GetPdfBytesFromUrl(url);

            }
            catch (Exception ex)
            {
                downloadBytes = new byte[] { };
                BLL.Loger.Log4Net.Info("在页面ExamPaperPDF.aspx 报错：" + ex.Message);
            }

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader("Content-Disposition",
                "attachment; filename=" + downloadName + "; size=" + downloadBytes.Length.ToString());
            response.Flush();
            response.BinaryWrite(downloadBytes);
            response.Flush();
            response.End();
        }

        private PdfConverter GetPdfConvert()
        {
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfStandardSubset = PdfStandardSubset.Full;
            pdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
            pdfConverter.PdfDocumentOptions.EmbedFonts = true;


            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            pdfConverter.PdfDocumentOptions.ShowFooter = true;

            pdfConverter.PdfDocumentOptions.LeftMargin = 0;
            pdfConverter.PdfDocumentOptions.RightMargin = 0;
            pdfConverter.PdfDocumentOptions.TopMargin = 20;
            pdfConverter.PdfDocumentOptions.BottomMargin = 20;
            pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;

            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            //pdfConverter.PdfHeaderOptions.HeaderText = "Sample header: " + TxtURL.Text;
            //pdfConverter.PdfHeaderOptions.HeaderTextColor = Color.Blue;
            //pdfConverter.PdfHeaderOptions.HeaderDescriptionText = string.Empty;
            //pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;

            pdfConverter.PdfFooterOptions.FooterText = "";
            //pdfConverter.PdfFooterOptions.FooterTextColor = Color.Blue;
            pdfConverter.PdfFooterOptions.DrawFooterLine = false;
            pdfConverter.PdfFooterOptions.PageNumberText = "页";
            pdfConverter.PdfFooterOptions.ShowPageNumber = true;
            return pdfConverter;
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {

        }
    }
}