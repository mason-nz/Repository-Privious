﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CC2015_HollyFormsApp.CCWebVerifyPhoneFormat {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap")]
    public interface VerifyPhoneFormatSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 Verifycode 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/VerifyFormat", ReplyAction="*")]
        CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatResponse VerifyFormat(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 Verifycode 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/VerifyFormatXiAn", ReplyAction="*")]
        CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnResponse VerifyFormatXiAn(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class VerifyFormatRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="VerifyFormat", Namespace="http://tempuri.org/", Order=0)]
        public CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatRequestBody Body;
        
        public VerifyFormatRequest() {
        }
        
        public VerifyFormatRequest(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class VerifyFormatRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Verifycode;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string phoneNumber;
        
        public VerifyFormatRequestBody() {
        }
        
        public VerifyFormatRequestBody(string Verifycode, string phoneNumber) {
            this.Verifycode = Verifycode;
            this.phoneNumber = phoneNumber;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class VerifyFormatResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="VerifyFormatResponse", Namespace="http://tempuri.org/", Order=0)]
        public CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatResponseBody Body;
        
        public VerifyFormatResponse() {
        }
        
        public VerifyFormatResponse(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class VerifyFormatResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool VerifyFormatResult;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string outNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string errorMsg;
        
        public VerifyFormatResponseBody() {
        }
        
        public VerifyFormatResponseBody(bool VerifyFormatResult, string outNumber, string errorMsg) {
            this.VerifyFormatResult = VerifyFormatResult;
            this.outNumber = outNumber;
            this.errorMsg = errorMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class VerifyFormatXiAnRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="VerifyFormatXiAn", Namespace="http://tempuri.org/", Order=0)]
        public CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnRequestBody Body;
        
        public VerifyFormatXiAnRequest() {
        }
        
        public VerifyFormatXiAnRequest(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class VerifyFormatXiAnRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Verifycode;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string phoneNumber;
        
        public VerifyFormatXiAnRequestBody() {
        }
        
        public VerifyFormatXiAnRequestBody(string Verifycode, string phoneNumber) {
            this.Verifycode = Verifycode;
            this.phoneNumber = phoneNumber;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class VerifyFormatXiAnResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="VerifyFormatXiAnResponse", Namespace="http://tempuri.org/", Order=0)]
        public CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnResponseBody Body;
        
        public VerifyFormatXiAnResponse() {
        }
        
        public VerifyFormatXiAnResponse(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class VerifyFormatXiAnResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool VerifyFormatXiAnResult;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string outNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string errorMsg;
        
        public VerifyFormatXiAnResponseBody() {
        }
        
        public VerifyFormatXiAnResponseBody(bool VerifyFormatXiAnResult, string outNumber, string errorMsg) {
            this.VerifyFormatXiAnResult = VerifyFormatXiAnResult;
            this.outNumber = outNumber;
            this.errorMsg = errorMsg;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface VerifyPhoneFormatSoapChannel : CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class VerifyPhoneFormatSoapClient : System.ServiceModel.ClientBase<CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap>, CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap {
        
        public VerifyPhoneFormatSoapClient() {
        }
        
        public VerifyPhoneFormatSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public VerifyPhoneFormatSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VerifyPhoneFormatSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VerifyPhoneFormatSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatResponse CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap.VerifyFormat(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatRequest request) {
            return base.Channel.VerifyFormat(request);
        }
        
        public bool VerifyFormat(string Verifycode, string phoneNumber, out string outNumber, out string errorMsg) {
            CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatRequest inValue = new CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatRequest();
            inValue.Body = new CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatRequestBody();
            inValue.Body.Verifycode = Verifycode;
            inValue.Body.phoneNumber = phoneNumber;
            CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatResponse retVal = ((CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap)(this)).VerifyFormat(inValue);
            outNumber = retVal.Body.outNumber;
            errorMsg = retVal.Body.errorMsg;
            return retVal.Body.VerifyFormatResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnResponse CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap.VerifyFormatXiAn(CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnRequest request) {
            return base.Channel.VerifyFormatXiAn(request);
        }
        
        public bool VerifyFormatXiAn(string Verifycode, string phoneNumber, out string outNumber, out string errorMsg) {
            CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnRequest inValue = new CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnRequest();
            inValue.Body = new CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnRequestBody();
            inValue.Body.Verifycode = Verifycode;
            inValue.Body.phoneNumber = phoneNumber;
            CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyFormatXiAnResponse retVal = ((CC2015_HollyFormsApp.CCWebVerifyPhoneFormat.VerifyPhoneFormatSoap)(this)).VerifyFormatXiAn(inValue);
            outNumber = retVal.Body.outNumber;
            errorMsg = retVal.Body.errorMsg;
            return retVal.Body.VerifyFormatXiAnResult;
        }
    }
}