﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BitAuto.ISDC.CC2012.WebService.Easypass.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Easypass.ServiceReference.IFaixianForCC")]
    public interface IFaixianForCC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaixianForCC/GetFanxianDealers", ReplyAction="http://tempuri.org/IFaixianForCC/GetFanxianDealersResponse")]
        int[] GetFanxianDealers(int locId, int carId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFaixianForCC/SubmitFanxianOrder", ReplyAction="http://tempuri.org/IFaixianForCC/SubmitFanxianOrderResponse")]
        string SubmitFanxianOrder(int userId, string name, string phone, int carId, int dealerId, int cityId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFaixianForCCChannel : BitAuto.ISDC.CC2012.WebService.Easypass.ServiceReference.IFaixianForCC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FaixianForCCClient : System.ServiceModel.ClientBase<BitAuto.ISDC.CC2012.WebService.Easypass.ServiceReference.IFaixianForCC>, BitAuto.ISDC.CC2012.WebService.Easypass.ServiceReference.IFaixianForCC {
        
        public FaixianForCCClient() {
        }
        
        public FaixianForCCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FaixianForCCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FaixianForCCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FaixianForCCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int[] GetFanxianDealers(int locId, int carId) {
            return base.Channel.GetFanxianDealers(locId, carId);
        }
        
        public string SubmitFanxianOrder(int userId, string name, string phone, int carId, int dealerId, int cityId) {
            return base.Channel.SubmitFanxianOrder(userId, name, phone, carId, dealerId, cityId);
        }
    }
}
