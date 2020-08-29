using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using WcfSamples.DynamicProxy;


namespace BitAuto.ISDC.CC2012.WebService
{
    public class WebServiceHelper
    {
        private static string webServiceTimeoutStr = System.Configuration.ConfigurationManager.AppSettings["WebServiceTimeout"];//WebService接口超时时间
        private static int webServiceTimeout = 0;

        /// <summary>
        /// WebService接口超时时间，读取配置文件，若找不到配置，默认20秒
        /// </summary>
        public static int WebServiceTimeout
        {
            get {
                return int.TryParse(webServiceTimeoutStr, out webServiceTimeout) ? webServiceTimeout : 20;
            }
        }

        #region InvokeWebService

        //动态调用web服务
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return WebServiceHelper.InvokeWebService(url, null, methodname, args);
        }

        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = WebServiceHelper.GetWsClassName(url);
            }
            WebClient wc = new WebClient();//获取WSDL
            CSharpCodeProvider csc = new CSharpCodeProvider();
            try
            {

                //WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);

                ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = null;
                mi = t.GetMethod(methodname);
                //if (types == null || types.Length == 0)
                //    mi = t.GetMethod(methodname);
                //else
                //    mi = t.GetMethod(methodname, types);
                object objr = mi.Invoke(obj, args);
                stream.Close();
                return objr;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("动态调用WebService出错,URL:{0},methodname:{1}", url, methodname), ex);
                csc.Dispose();
                wc.Dispose();
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
            finally
            {
                csc.Dispose();
                wc.Dispose();
            }
        }

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }

        /// <summary>
        /// 动态调用WCF服务
        /// </summary>
        /// <param name="pUrl">WCF服务地址，如http://m.bitauto.com/wcf.svc?wsdl</param>
        /// <param name="pMethodName">调用方法名称</param>
        /// <param name="pParams">参数数组</param>
        /// <returns>返回类型</returns>
        public static object InvokeWCF(string pUrl, string pMethodName, params object[] pParams)
        {
            string serviceWsdlUri = pUrl;

            DynamicProxyFactory factory = new DynamicProxyFactory(serviceWsdlUri);
            int count = 0;
            List<object> myEndpoints = new List<object>();

            foreach (System.ServiceModel.Description.ServiceEndpoint endpoint in factory.Endpoints)
            {
                myEndpoints.Add(endpoint.Contract.Name);
            }
            foreach (string endpoint in myEndpoints)
            {
                DynamicProxy dp = factory.CreateProxy(endpoint);
                System.ServiceModel.Description.OperationDescriptionCollection operations = factory.GetEndpoint(endpoint).Contract.Operations;
                Type proxyType = dp.ProxyType;
                System.Reflection.MethodInfo[] mi = proxyType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                for (int i = 0; i < mi.Length; i++)
                {
                    string name = mi[i].Name;
                    if (name == pMethodName)
                    {
                        DynamicProxy proxy = factory.CreateProxy(endpoint);
                        return proxy.CallMethod(pMethodName, pParams);
                    }
                }
                dp.Close();
            }
            return null;
        }
        #endregion
    }
}
