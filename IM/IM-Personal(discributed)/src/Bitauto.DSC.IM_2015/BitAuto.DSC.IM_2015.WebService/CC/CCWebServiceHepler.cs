using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services.Description;

namespace BitAuto.DSC.IM_2015.WebService.CC
{
    public class CCWebServiceHepler
    {
        #region Instance
        public static readonly CCWebServiceHepler Instance = new CCWebServiceHepler();
        #endregion


        private CCDataInterfaceService.CCDataInterfaceServiceSoapClient c = new CCDataInterfaceService.CCDataInterfaceServiceSoapClient();
        private string authorize = System.Configuration.ConfigurationManager.AppSettings["AgentInfoAuthorizeCode"].ToString();
        private string SMSKey = System.Configuration.ConfigurationManager.AppSettings["SMSKey"];//发送手机短信Key
        /// <summary>
        /// 根据custid取业务记录
        /// </summary>
        /// <returns></returns>
        public DataTable CCDataInterface_GetCustHistoryInfo(string custid, string userid)
        {
            DataTable dt = null;
            int totalcount = 0;
            CCDataInterfaceService.QueryCustHistoryInfo querymodel = new CCDataInterfaceService.QueryCustHistoryInfo();
            querymodel.CustID = custid;
            string authorize = string.Empty;
            string msg = string.Empty; ;
            authorize = System.Configuration.ConfigurationManager.AppSettings["AgentInfoAuthorizeCode1"].ToString();
            dt = c.CCDataInterface_GetCustHistoryInfo(out totalcount, authorize, Convert.ToInt32(userid), querymodel, 1, ref msg);
            return dt;
        }
        /// <summary>
        /// CC工单页面接口，根据参数，此接口返回工单添加页面包括参数
        /// </summary>
        /// <param name="phone">电话</param>
        /// <param name="csID">业务主键（留言表主键或会话表主键）</param>
        /// <param name="cbName">客户姓名</param>
        /// <param name="cbSex">性别</param>
        /// <param name="provinceID">省份</param>
        /// <param name="cityID">城市</param>
        /// <param name="county">县</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessTag">标签ID</param>
        /// <param name="imBussinessType">1是对话，2是留言</param>
        /// <returns></returns>
        public string CCDataInterface_GetAddWOrderComeIn_IMGR_URL(string phone, string csID, string cbName, int cbSex, int provinceID, int cityID, int county, int businessType, int businessTag, int imBussinessType)
        {
            string msg = string.Empty;
            long csid = 0;
            long.TryParse(csID, out csid);
            CCDataInterfaceService.CallSourceEnum callSource = CCDataInterfaceService.CallSourceEnum.C03_IM对话;
            if (imBussinessType == 2)
            {
                callSource = CCDataInterfaceService.CallSourceEnum.C04_IM留言;
            }
            msg = c.GetAddWOrderComeIn_IMGR_URL(authorize, callSource, phone, csid, cbName, cbSex, provinceID, cityID, county, businessType, businessTag);
            return msg;
        }


        /// <summary>
        /// 根据用户ID获取员工信息（包含工号，姓名，业务线ID，所属分组，管辖分组）
        /// </summary>
        /// <param name="userID">用户Id</param>
        /// <returns>json</returns>
        public EmployeeAgent GetEmployeeAgent(int userID)
        {
            string result = c.GetEmployeeAgentInfo(authorize, userID);
            if (result != "")
                return JsonConvert.DeserializeObject<EmployeeAgent>(result);
            else
                return null;
        }

        /// <summary>
        /// 根据电话，姓名，取客户池客户 （包含性别，custid,城市id，省份id）
        /// </summary>
        /// <param name="CustTel">电话</param>
        /// <param name="CustBasicInfo">姓名</param>
        /// <returns>json</returns>
        public Entities.CEntity GetCustomer(string tel, string name)
        {
            string result = c.GetCustBasicInfo(authorize, tel, name);
            if (result != "")
            {
                return (CEntity)JsonConvert.DeserializeObject(result, typeof(CEntity));
            }
            else
                return null;
        }

        /// <summary>
        /// 新增客户信息
        /// </summary>
        /// <param name="custInfo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool InsertCustomer(CustomerInfo custInfo, out string msg)
        {
            msg = "";
            return true;
        }
        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="model"></param>
        /// <param name="custid"></param>
        /// <returns></returns>
        public bool CCDataInterface_InsertCustData(out string msg, Entities.UserVisitLog model, out string custid)
        {
            msg = string.Empty;
            custid = string.Empty;
            bool flag = false;
            StringBuilder jsonstr = new StringBuilder();
            jsonstr.Append("{");

            jsonstr.Append("CustName:'" + model.UserName + "',");
            int _sex = 0;
            if (model.Sex)
            {
                _sex = 1;
            }
            else
            {
                _sex = 2;
            }

            jsonstr.Append("Sex:'" + _sex + "',");
            jsonstr.Append("Tels:['" + model.Phone + "'],");
            jsonstr.Append("ProvinceID:'" + model.ProvinceID + "',");
            jsonstr.Append("CityID:'" + model.CityID + "',");
            jsonstr.Append("CreateUserID:'',");
            jsonstr.Append("CustCategory:'4'");
            jsonstr.Append("}");
            flag = c.CCDataInterface_InsertCustData(out custid, out msg, authorize, jsonstr.ToString());
            return flag;
        }


        #region 短信发送调用
        const string ProductCode = "6116";//组合key的前缀字符

        private string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }

        public object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = GetWsClassName(url);
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
                object objr = mi.Invoke(obj, args);
                stream.Close();
                return objr;
            }
            catch (Exception ex)
            {
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


        //动态调用web服务
        public object InvokeWebService(string url, int MemberCode)
        {
            return InvokeWebService(url, null, "GetDealer400", new object[] { MemberCode });
        }
        /// <summary>
        /// md5加密算法 
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="content">发送内容</param>
        /// <returns></returns>
        public string MixMd5(string url, string mobile, string content)
        {
            return (string)InvokeWebService(url, null, "MixMd5", new object[] { ProductCode + mobile + content + SMSKey });
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNums">手机号码</param>
        /// <param name="content">发送手机短信内容</param>
        /// <param name="dt">发送时间(当前时间+1小时)</param>
        /// <param name="md5">MD5加密Key</param>
        /// <returns></returns>
        public int SendMsgImmediately(string url, string phoneNums, string content, DateTime dt, string md5)
        {
            int i = (int)InvokeWebService(url, null, "SendMsgImmediately", new object[] { ProductCode, phoneNums, content, string.Empty, dt, md5 });
            return i;
        }
        #endregion
    }
}
