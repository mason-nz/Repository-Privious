using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mshtml;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CC2015_HollyFormsApp.Code.Utils
{
    public class CrossFrameIE
    {
        private const int E_ACCESSDENIED = unchecked((int)0x80070005L);
        private static Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
        private static Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E");

        /// <summary>
        /// 根据WebBrowser对象，获取下面页面中Frame的个数
        /// </summary>
        /// <param name="wb">WebBrowser对象</param>
        /// <returns>获取下面页面中Frame的个数</returns>
        private static int GetFramesCount(WebBrowser wb)
        {
            int count = 0;
            try
            {
                if (wb != null && wb.Document != null && wb.Document.DomDocument != null)
                {
                    mshtml.HTMLDocumentClass htmlDoc = wb.Document.DomDocument as mshtml.HTMLDocumentClass;
                    count = htmlDoc.frames.length;
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("调用GetFramesCount方法异常", ex);
                return 0;
            }
            return count;
        }

        /// <summary>
        /// 根据IHTMLWindow2对象，获取此对象中iframe里面的子页面对象HTMLWindow对象
        /// </summary>
        /// <param name="win">IHTMLWindow2对象</param>
        /// <param name="frameName">iframe中frame名称</param>
        /// <returns>返回子页面HTMLWindow对象</returns>
        public static IHTMLWindow2 GetFrameWindowObject(IHTMLWindow2 win, string frameName)
        {
            try
            {
                int framesCount = win.frames.length;
                if (framesCount > 1 && !string.IsNullOrEmpty(frameName))
                {
                    for (int i = 0; i < framesCount; i++)
                    {
                        object index = i as object;//跨域访问js方法
                        mshtml.IHTMLWindow2 frameWindow = win.frames.item(ref index) as mshtml.IHTMLWindow2;
                        var ff = GetDocumentFromWindow(frameWindow);
                        if (ff != null && ff.parentWindow != null && !string.IsNullOrEmpty(ff.parentWindow.name) &&
                            ff.parentWindow.name.ToLower() == frameName.ToLower())
                        {
                            return ff.parentWindow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("调用GetFrameWindowObject(IHTMLWindow2 win, string frameName)方法异常，参数frameName为：" + frameName, ex);
            }
            return null;
        }

        /// <summary>
        /// 根据WebBrowser对象，获取WebBrowser对象中iframe里面的子页面对象HTMLWindow对象
        /// </summary>
        /// <param name="wb">WebBrowser对象</param>
        /// <param name="frameName">iframe中frame名称</param>
        /// <returns>返回子页面HTMLWindow对象</returns>
        public static IHTMLWindow2 GetFrameWindowObject(WebBrowser wb, string frameName)
        {
            int framesCount = GetFramesCount(wb);
            if (framesCount > 1 && !string.IsNullOrEmpty(frameName))
            {
                try
                {
                    mshtml.HTMLDocumentClass htmlDoc = wb.Document.DomDocument as mshtml.HTMLDocumentClass;
                    for (int i = 0; i < framesCount; i++)
                    {
                        object index = i as object;//跨域访问js方法
                        mshtml.IHTMLWindow2 frameWindow = htmlDoc.frames.item(ref index) as mshtml.IHTMLWindow2;
                        var ff = GetDocumentFromWindow(frameWindow);
                        if (ff != null && ff.parentWindow.name.ToLower() == frameName.ToLower())
                        {
                            return ff.parentWindow;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("调用GetFrameWindowObject(WebBrowser wb,string frameName)方法异常，参数frameName为：" + frameName, ex);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据Frame中的子页面HtmlWindow对象，获取子页面中的HtmlDocument对象
        /// </summary>
        /// <param name="htmlWindow">Frame中的子页面HtmlWindow对象</param>
        /// <returns>查不到返回Null，否则返回HtmlDocument对象</returns>
        private static IHTMLDocument2 GetDocumentFromWindow(IHTMLWindow2 htmlWindow)
        {
            if (htmlWindow == null)
            {
                return null;
            }

            // First try the usual way to get the document.
            try
            {
                IHTMLDocument2 doc = htmlWindow.document;
                return doc;
            }
            catch (COMException comEx)
            {
                // I think COMException won't be ever fired but just to be sure ...
                if (comEx.ErrorCode != E_ACCESSDENIED)
                {
                    return null;
                }
            }
            catch (System.UnauthorizedAccessException)
            {
            }
            catch
            {
                // Any other error.
                return null;
            }

            // At this point the error was E_ACCESSDENIED because the frame contains a document from another domain.
            // IE tries to prevent a cross frame scripting security issue.
            try
            {
                // Convert IHTMLWindow2 to IWebBrowser2 using IServiceProvider.
                IServiceProvider sp = (IServiceProvider)htmlWindow;

                // Use IServiceProvider.QueryService to get IWebBrowser2 object.
                Object brws = null;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out brws);

                // Get the document from IWebBrowser2.
                SHDocVw.IWebBrowser2 browser = (SHDocVw.IWebBrowser2)(brws);

                return (IHTMLDocument2)browser.Document;
            }
            catch
            {
            }

            return null;
        }
    }

    // This is the COM IServiceProvider interface, not System.IServiceProvider .Net interface!
    [ComImport(), ComVisible(true), Guid("6D5140C1-7436-11CE-8034-00AA006009FA"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServiceProvider
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
    }
}
