﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1026
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.1026 版自动生成。
// 
#pragma warning disable 1591

namespace BitAuto.ISDC.CC2012.WebService.IM.UtilsService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="UtilsSoap", Namespace="http://tempuri.org/")]
    public partial class Utils : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateCCWorkOrder2IMOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateCCWorkOrder2IMHaveKeyOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Utils() {
            this.Url = global::BitAuto.ISDC.CC2012.WebService.Properties.Settings.Default.BitAuto_ISDC_CC2012_WebService_IM_UtilsService_Utils;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event UpdateCCWorkOrder2IMCompletedEventHandler UpdateCCWorkOrder2IMCompleted;
        
        /// <remarks/>
        public event UpdateCCWorkOrder2IMHaveKeyCompletedEventHandler UpdateCCWorkOrder2IMHaveKeyCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateCCWorkOrder2IM", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("errormsg")]
        public string UpdateCCWorkOrder2IM(int imtype, int id, string orderid) {
            object[] results = this.Invoke("UpdateCCWorkOrder2IM", new object[] {
                        imtype,
                        id,
                        orderid});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateCCWorkOrder2IMAsync(int imtype, int id, string orderid) {
            this.UpdateCCWorkOrder2IMAsync(imtype, id, orderid, null);
        }
        
        /// <remarks/>
        public void UpdateCCWorkOrder2IMAsync(int imtype, int id, string orderid, object userState) {
            if ((this.UpdateCCWorkOrder2IMOperationCompleted == null)) {
                this.UpdateCCWorkOrder2IMOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateCCWorkOrder2IMOperationCompleted);
            }
            this.InvokeAsync("UpdateCCWorkOrder2IM", new object[] {
                        imtype,
                        id,
                        orderid}, this.UpdateCCWorkOrder2IMOperationCompleted, userState);
        }
        
        private void OnUpdateCCWorkOrder2IMOperationCompleted(object arg) {
            if ((this.UpdateCCWorkOrder2IMCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateCCWorkOrder2IMCompleted(this, new UpdateCCWorkOrder2IMCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateCCWorkOrder2IMHaveKey", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("errormsg")]
        public string UpdateCCWorkOrder2IMHaveKey(int imtype, int id, string orderid, string key) {
            object[] results = this.Invoke("UpdateCCWorkOrder2IMHaveKey", new object[] {
                        imtype,
                        id,
                        orderid,
                        key});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateCCWorkOrder2IMHaveKeyAsync(int imtype, int id, string orderid, string key) {
            this.UpdateCCWorkOrder2IMHaveKeyAsync(imtype, id, orderid, key, null);
        }
        
        /// <remarks/>
        public void UpdateCCWorkOrder2IMHaveKeyAsync(int imtype, int id, string orderid, string key, object userState) {
            if ((this.UpdateCCWorkOrder2IMHaveKeyOperationCompleted == null)) {
                this.UpdateCCWorkOrder2IMHaveKeyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateCCWorkOrder2IMHaveKeyOperationCompleted);
            }
            this.InvokeAsync("UpdateCCWorkOrder2IMHaveKey", new object[] {
                        imtype,
                        id,
                        orderid,
                        key}, this.UpdateCCWorkOrder2IMHaveKeyOperationCompleted, userState);
        }
        
        private void OnUpdateCCWorkOrder2IMHaveKeyOperationCompleted(object arg) {
            if ((this.UpdateCCWorkOrder2IMHaveKeyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateCCWorkOrder2IMHaveKeyCompleted(this, new UpdateCCWorkOrder2IMHaveKeyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateCCWorkOrder2IMCompletedEventHandler(object sender, UpdateCCWorkOrder2IMCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateCCWorkOrder2IMCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateCCWorkOrder2IMCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateCCWorkOrder2IMHaveKeyCompletedEventHandler(object sender, UpdateCCWorkOrder2IMHaveKeyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateCCWorkOrder2IMHaveKeyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateCCWorkOrder2IMHaveKeyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591