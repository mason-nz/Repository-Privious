﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.0
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BitAuto.DSC.IM_2015.WebService.DealerInfoService {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://easyop.bitauto.com/dms/", ConfigurationName="DealerInfoService.DealerInfoServiceSoap")]
    public interface DealerInfoServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/UpdateDasAccountByDealerID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int UpdateDasAccountByDealerID(int DealerID, string OldPassword, string NewPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/UpdateDealerContactByDealerID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int UpdateDealerContactByDealerID(int DealerID, string ContactAddress, string PostCode, string SalesPhones, string FaxNumbers, string WebSiteUrl, string EmailAddress, string TrafficInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/UpdateDealerIntroductionByDealerID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int UpdateDealerIntroductionByDealerID(int DealerID, string Introduction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/UpdateDealerInfoMapCoorDinateByDealerID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int UpdateDealerInfoMapCoorDinateByDealerID(int DealerID, string MapProviderName, string Longitude, string Latitude, string TrafficInfo, string MapCoorDinateRemark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/CreateDealerInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CreateDealerInfo(string DealerFullName, string DealerShortName, int BusinessModeID, int LocationID, string DealerContactAddress, string DealerSalesPhones, int BrandGroupID, string DealerEnterpriseIntroduction, string DealerPostCode, string DealerWebSiteUrl, string DealerEmailAddress, string Longitude, string Latitude, string TrafficInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/UpdateDealerInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int UpdateDealerInfo(int DealerID, string ContactAddress, string PostCode, string SalesPhones, string FaxNumbers, string WebSiteUrl, string EmailAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/GetDealerInfoByUpdateTime", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetDealerInfoByUpdateTime(System.DateTime UpdateTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/GetDealerInfoByDealerID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetDealerInfoByDealerID(int DealerID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/GetDealerInfoByAllDealerID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetDealerInfoByAllDealerID();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/DeleteDealerInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string DeleteDealerInfo(int DealerID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/UpdateDealerStatus", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string UpdateDealerStatus(int dealerId, int status);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://easyop.bitauto.com/dms/GetDealer400", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetDealer400(int DealerID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DealerInfoServiceSoapChannel : global::BitAuto.DSC.IM_2015.WebService.DealerInfoService.DealerInfoServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DealerInfoServiceSoapClient : System.ServiceModel.ClientBase<global::BitAuto.DSC.IM_2015.WebService.DealerInfoService.DealerInfoServiceSoap>, global::BitAuto.DSC.IM_2015.WebService.DealerInfoService.DealerInfoServiceSoap {
        
        public DealerInfoServiceSoapClient() {
        }
        
        public DealerInfoServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DealerInfoServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DealerInfoServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DealerInfoServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int UpdateDasAccountByDealerID(int DealerID, string OldPassword, string NewPassword) {
            return base.Channel.UpdateDasAccountByDealerID(DealerID, OldPassword, NewPassword);
        }
        
        public int UpdateDealerContactByDealerID(int DealerID, string ContactAddress, string PostCode, string SalesPhones, string FaxNumbers, string WebSiteUrl, string EmailAddress, string TrafficInfo) {
            return base.Channel.UpdateDealerContactByDealerID(DealerID, ContactAddress, PostCode, SalesPhones, FaxNumbers, WebSiteUrl, EmailAddress, TrafficInfo);
        }
        
        public int UpdateDealerIntroductionByDealerID(int DealerID, string Introduction) {
            return base.Channel.UpdateDealerIntroductionByDealerID(DealerID, Introduction);
        }
        
        public int UpdateDealerInfoMapCoorDinateByDealerID(int DealerID, string MapProviderName, string Longitude, string Latitude, string TrafficInfo, string MapCoorDinateRemark) {
            return base.Channel.UpdateDealerInfoMapCoorDinateByDealerID(DealerID, MapProviderName, Longitude, Latitude, TrafficInfo, MapCoorDinateRemark);
        }
        
        public string CreateDealerInfo(string DealerFullName, string DealerShortName, int BusinessModeID, int LocationID, string DealerContactAddress, string DealerSalesPhones, int BrandGroupID, string DealerEnterpriseIntroduction, string DealerPostCode, string DealerWebSiteUrl, string DealerEmailAddress, string Longitude, string Latitude, string TrafficInfo) {
            return base.Channel.CreateDealerInfo(DealerFullName, DealerShortName, BusinessModeID, LocationID, DealerContactAddress, DealerSalesPhones, BrandGroupID, DealerEnterpriseIntroduction, DealerPostCode, DealerWebSiteUrl, DealerEmailAddress, Longitude, Latitude, TrafficInfo);
        }
        
        public int UpdateDealerInfo(int DealerID, string ContactAddress, string PostCode, string SalesPhones, string FaxNumbers, string WebSiteUrl, string EmailAddress) {
            return base.Channel.UpdateDealerInfo(DealerID, ContactAddress, PostCode, SalesPhones, FaxNumbers, WebSiteUrl, EmailAddress);
        }
        
        public System.Data.DataSet GetDealerInfoByUpdateTime(System.DateTime UpdateTime) {
            return base.Channel.GetDealerInfoByUpdateTime(UpdateTime);
        }
        
        public System.Data.DataSet GetDealerInfoByDealerID(int DealerID) {
            return base.Channel.GetDealerInfoByDealerID(DealerID);
        }
        
        public System.Data.DataSet GetDealerInfoByAllDealerID() {
            return base.Channel.GetDealerInfoByAllDealerID();
        }
        
        public string DeleteDealerInfo(int DealerID) {
            return base.Channel.DeleteDealerInfo(DealerID);
        }
        
        public string UpdateDealerStatus(int dealerId, int status) {
            return base.Channel.UpdateDealerStatus(dealerId, status);
        }
        
        public System.Data.DataSet GetDealer400(int DealerID) {
            return base.Channel.GetDealer400(DealerID);
        }
    }
}
