using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace BitAuto.ISDC.CC2012.WebService.BAA
{
    public class UGCSyncServiceHelper
    {
        UGCSyncServiceProxy proxy = new UGCSyncServiceProxy();

        #region Instance
        public static readonly UGCSyncServiceHelper Instance = new UGCSyncServiceHelper();
        private string UGCSyncServiceURL = System.Configuration.ConfigurationManager.AppSettings["UGCSyncServiceURL"];//汽车通手机号注册接口
        #endregion

        //#region Contructor
        //public UGCSyncServiceHelper()
        //{ }
        //#endregion

        /// <summary>
        /// 发送一条同步数据的消息
        /// </summary>
        /// <param name="woiModel">工单实体</param>
        /// <param name="cbiModel">客户池-个人用户实体</param>
        /// <param name="mobile">个人用户手机号码</param>
        public void Send(Entities.WorkOrderInfo woiModel, Entities.CustBasicInfo cbiModel, string mobile, string activityIds)
        {
            string content = GetXMLContentByInBound(woiModel, cbiModel, mobile, activityIds);
            BLL.Loger.Log4Net.Info("调用接口：" + UGCSyncServiceURL + "，参数为：" + content);
            //WebServiceHelper.InvokeWebService(UGCSyncServiceURL, "Send", new object[] { content });
            proxy.Send(content);
        }

        /// <summary>
        /// 发送一条同步数据的消息
        /// </summary>
        /// <param name="content">生成XML内容</param>
        public void Send(string content)
        {
            BLL.Loger.Log4Net.Info("调用接口：" + UGCSyncServiceURL + "，参数为：" + content);
            //WebServiceHelper.InvokeWebService(UGCSyncServiceURL, "Send", new object[] { content });
            proxy.Send(content);
        }

        /// <summary>
        /// 生成XML内容（呼出）
        /// </summary>
        /// <param name="ht">Hashtable（包含以下字段）
        /// GUID：其他任务表OtherTaskInfo字段GUID（必填）
        /// Mobile：手机号码（必填）
        /// RealName：用户真实姓名（必填）
        /// CreateTime：生成时间（必填）
        /// CityId：城市Id
        /// ProvinceId：省份Id
        /// Sex：用户性别（选填 男：1  女:0  保密:2）
        /// Bs_Id：意向品牌Bs_Id
        /// Cs_Id：意向车型Cs_Id（没有填0）
        /// Car_Id：0
        /// ActivityIds：推荐活动GUID字符串，用逗号分隔
        /// </param>
        /// <returns></returns>
        public string GetXMLContentByOutBound(Hashtable ht)
        {
            string xmlContent = string.Empty;
            VerifyLogicByOutBound(ht);

            Messages m = new Messages();
            Header h = new Header();
            Body b = new Body();
            OperaterInfo o = new OperaterInfo();
            UserInfo u = new UserInfo();
            CarInfo ci = new CarInfo();
            ActivityInfo ai = new ActivityInfo();
            //CareCarInfo cci = new CareCarInfo();
            //WorkOrderInfo w = new WorkOrderInfo();
            //DealerInfo d = new DealerInfo();
            int cityID = 0;
            int provinceID = 0;
            int sex = 0;
            int bsID = 0;
            int csID = 0;

            #region 初始化Header对象
            h.AppId = 12;
            h.EntityType = "ReturnVisit";
            h.OperateType = "Add";
            m.header = h;
            #endregion

            #region 初始化OperaterInfo对象
            o.Mobile = ht["Mobile"].ToString().Trim();
            o.RealName = ht["RealName"].ToString().Trim();
            o.UserIp = BLL.Util.GetIPAddress();
            o.OperateTime = DateTime.Parse(ht["CreateTime"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            #endregion

            #region 初始化UserInfo对象
            if (ht.ContainsKey("ProvinceId") &&
                int.TryParse(ht["ProvinceId"].ToString().Trim(), out provinceID))
            {
                u.ProvinceId = provinceID.ToString();
            }
            if (ht.ContainsKey("CityId") &&
                int.TryParse(ht["CityId"].ToString().Trim(), out cityID))
            {
                u.CityId = cityID.ToString();
            }
            if (ht.ContainsKey("Sex") &&
                int.TryParse(ht["Sex"].ToString().Trim(), out sex))
            {
                u.Sex = sex.ToString();
            }
            //u.Address = "";
            #endregion

            #region 初始化CarInfo对象
            if (ht.ContainsKey("Bs_Id") &&
                int.TryParse(ht["Bs_Id"].ToString().Trim(), out bsID))
            {
                ci.Bs_Id = bsID;
            }
            if (ht.ContainsKey("Cs_Id") &&
                int.TryParse(ht["Cs_Id"].ToString().Trim(), out csID))
            {
                ci.Cs_Id = csID;
            }
            ci.Car_Id = 0;
            #endregion

            #region 初始化ActivityInfo对象
            if (ht.ContainsKey("ActivityIds"))
            {
                ai.ActivityId = ht["ActivityIds"].ToString().Split(',');
                b.activityInfo = ai;
            }
            #endregion


            #region 初始化Body对象
            b.EntityId = (Guid)ht["GUID"];
            b.operaterInfo = o;
            if (!string.IsNullOrEmpty(u.CityId) ||
                !string.IsNullOrEmpty(u.ProvinceId) ||
                !string.IsNullOrEmpty(u.Sex) ||
                !string.IsNullOrEmpty(u.Address))
            {
                b.userInfo = u;
            }
            b.carInfo = ci;
            m.body = b;
            #endregion

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(Messages));
            serializer.Serialize(memoryStream, m, ns);
            byte[] buff = memoryStream.ToArray();
            xmlContent = Encoding.UTF8.GetString(buff);

            return xmlContent;
        }

        private void VerifyLogicByOutBound(Hashtable ht)
        {
            DateTime dt = new DateTime();
            if (ht == null)
            {
                throw new Exception("生成XML内容（呼出）中，Hashtable为NULL，没有初始化");
            }
            else if (ht.ContainsKey("GUID") == false ||
                     ht.ContainsKey("Mobile") == false ||
                     ht.ContainsKey("RealName") == false ||
                     ht.ContainsKey("CreateTime") == false)
            {
                throw new Exception("生成XML内容（呼出）中，Hashtable中，没有找到相应的Key");
            }
            else if (string.IsNullOrEmpty(ht["GUID"].ToString()))
            {
                throw new Exception("生成XML内容（呼出）中，其他任务GUID值不能为空");
            }
            else if (string.IsNullOrEmpty(ht["Mobile"].ToString()))
            {
                throw new Exception("生成XML内容（呼出）中，Mobile值不能为空");
            }
            else if (string.IsNullOrEmpty(ht["RealName"].ToString()))
            {
                throw new Exception("生成XML内容（呼出）中，RealName值不能为空");
            }
            else if (string.IsNullOrEmpty(ht["CreateTime"].ToString()))
            {
                throw new Exception("生成XML内容（呼出）中，其他任务CreateTime值不能为空");
            }
            else if (!DateTime.TryParse(ht["CreateTime"].ToString(), out dt))
            {
                throw new Exception("生成XML内容（呼出）中，其他任务CreateTime值格式不正确");
            }
            else if (ht.ContainsKey("ActivityIds") && (
                string.IsNullOrEmpty(ht["ActivityIds"].ToString()) ||
                ht["ActivityIds"].ToString().Split(',').Length < 1))
            {
                throw new Exception("生成XML内容（呼出）中，推荐活动ActivityIds值不能为空，且必须有值");
            }
        }

        /// <summary>
        /// 生成XML内容（呼入）
        /// </summary>
        /// <param name="woiModel">工单实体</param>
        /// <param name="cbiModel">客户池-个人用户实体</param>
        /// <param name="mobile">个人用户手机号码</param>
        /// <param name="activityIds">推荐活动ID串，逗号隔开</param>
        /// <returns>返回xml格式内容</returns>
        public string GetXMLContentByInBound(Entities.WorkOrderInfo woiModel, Entities.CustBasicInfo cbiModel, string mobile, string activityIds)
        {
            string xmlContent = string.Empty;
            //XmlDocument xmldoc = new XmlDocument();
            //XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            //xmldoc.AppendChild(xmldecl);
            //return xmldoc.OuterXml;
            VerifyLogicByInBound(woiModel, cbiModel, mobile, activityIds);

            Messages m = new Messages();
            Header h = new Header();
            Body b = new Body();
            OperaterInfo o = new OperaterInfo();
            UserInfo u = new UserInfo();
            CarInfo ci = new CarInfo();
            CareCarInfo cci = new CareCarInfo();
            WorkOrderInfo w = new WorkOrderInfo();
            DealerInfo d = new DealerInfo();
            ActivityInfo ai = new ActivityInfo();

            #region 初始化Header对象
            h.AppId = 12;
            h.EntityType = GetEntityTypeIDByWOCategoryID(woiModel.CategoryID);
            h.OperateType = "Add";
            m.header = h;
            #endregion

            #region 初始化OperaterInfo对象
            o.Mobile = mobile;
            o.RealName = cbiModel.CustName;
            o.UserIp = BLL.Util.GetIPAddress();
            o.OperateTime = woiModel.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            #endregion

            #region 初始化UserInfo对象
            if (cbiModel.ProvinceID != null && cbiModel.ProvinceID.Value > 0)
            {
                u.ProvinceId = cbiModel.ProvinceID.Value.ToString();
            }
            if (cbiModel.CityID != null && cbiModel.CityID.Value > 0)
            {
                u.CityId = cbiModel.CityID.Value.ToString();
            }
            if (cbiModel.Sex != null && cbiModel.Sex.Value > 0)
            {
                u.Sex = cbiModel.Sex.Value == 2 ? "0" : "1";
            }
            //u.Address = "";
            #endregion

            #region 初始化CarInfo对象
            if (woiModel.AttentionCarBrandID != null && woiModel.AttentionCarBrandID.Value > 0)
            {
                ci.Bs_Id = woiModel.AttentionCarBrandID.Value;
            }
            if (woiModel.AttentionCarSerialID != null && woiModel.AttentionCarSerialID.Value > 0)
            {
                ci.Cs_Id = woiModel.AttentionCarSerialID.Value;
            }
            if (woiModel.AttentionCarTypeID != null && woiModel.AttentionCarTypeID.Value > 0)
            {
                ci.Car_Id = woiModel.AttentionCarTypeID.Value;
            }
            #endregion

            #region 初始化CareCarInfo对象
            if (woiModel.SaleCarBrandID != null && woiModel.SaleCarBrandID.Value > 0)
            {
                cci.Bs_Id = woiModel.SaleCarBrandID.Value;
            }
            if (woiModel.SaleCarSerialID != null && woiModel.SaleCarSerialID.Value > 0)
            {
                cci.Cs_Id = woiModel.SaleCarSerialID.Value;
            }
            if (woiModel.SaleCarTypeID != null && woiModel.SaleCarTypeID.Value > 0)
            {
                cci.Car_Id = woiModel.SaleCarTypeID.Value;
            }
            #endregion

            #region 初始化WorkOrderInfo对象
            w.CategoryId = woiModel.CategoryID;
            w.CategoryName = BLL.WorkOrderCategory.Instance.GetCategoryNameByCategoryID(woiModel.CategoryID);
            if (woiModel.IsReturnVisit != null && woiModel.IsReturnVisit.Value >= 0)
            {
                w.Visited = woiModel.IsReturnVisit.Value;
            }
            #endregion

            #region 初始化DealerInfo对象
            int dealerID = -1;
            if (int.TryParse(woiModel.SelectDealerID, out dealerID))
            {
                d.DealerId = int.Parse(woiModel.SelectDealerID);
                d.DealerName = woiModel.SelectDealerName;
            }
            #endregion

            #region 初始化ActivityInfo对象
            if (!string.IsNullOrEmpty(activityIds))
            {
                ai.ActivityId = activityIds.Trim().Split(',');
                b.activityInfo = ai;
            }
            #endregion

            #region 初始化Body对象
            b.EntityId = woiModel.GUID;
            b.operaterInfo = o;
            b.userInfo = u;
            b.carInfo = ci;
            b.careCarInfo = cci;
            b.workOrderInfo = w;
            if (dealerID > 0)
            {
                b.dealerInfo = d;
            }
            m.body = b;
            #endregion

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(Messages));
            serializer.Serialize(memoryStream, m, ns);
            byte[] buff = memoryStream.ToArray();
            xmlContent = Encoding.UTF8.GetString(buff);

            return xmlContent;
        }

        /// <summary>
        /// 根据工单分类ID，返回XML中EntityTypeID
        /// </summary>
        /// <param name="categoryID">工单分类ID</param>
        /// <returns>返回XML中EntityTypeID</returns>
        private string GetEntityTypeIDByWOCategoryID(int categoryID)
        {
            string EntityTypeID = string.Empty;
            switch (categoryID)
            {
                case 34: EntityTypeID = "NewCar"; break;
                case 35: EntityTypeID = "BuyUsedCar"; break;
                case 36: EntityTypeID = "SellUsedCar"; break;
                case 37: EntityTypeID = "ManufacturerActivity"; break;
                case 38: EntityTypeID = "CarSurround"; break;
            }
            return EntityTypeID;
        }

        private void VerifyLogicByInBound(Entities.WorkOrderInfo woiModel, Entities.CustBasicInfo cbiModel, string mobile, string activityIds)
        {

            if (string.IsNullOrEmpty(mobile))
            {
                throw new Exception("生成XML内容（呼入）中，Mobile的值不能为空");
            }
            else if (string.IsNullOrEmpty(cbiModel.CustName))
            {
                throw new Exception("生成XML内容（呼入）中，真实姓名CustName的值不能为空");
            }
            else if (woiModel.CreateTime == null)
            {
                throw new Exception("生成XML内容（呼入）中，工单生成时间CreateTime的值不能为空");
            }
            else if (!(woiModel.WorkCategory != null && woiModel.WorkCategory.Value > 0))
            {
                throw new Exception("生成XML内容（呼入）中，工单分类CategoryID的值不能为空");
            }
            else if (!string.IsNullOrEmpty(activityIds) &&
                     activityIds.Trim().Split(',').Length < 1)
            {
                throw new Exception("生成XML内容（呼入）中，推荐活动ActivityIds值不能为空，且必须有值");
            }
            //else if (woiModel.IsReturnVisit == null)
            //{
            //    throw new Exception("生成XML内容（呼入）中，工单是否接受回访的值不能为空");
            //}
            //else if (!int.TryParse(woiModel.SelectDealerID, out dealerID))
            //{
            //    throw new Exception("生成XML内容（呼入）中，选择经销商Id的值不能为空");
            //}
        }
    }

    class UGCSyncServiceProxy : BAA.UGCSyncService.UGCSyncService
    {
        public UGCSyncServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["UGCSyncServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["UGCSyncServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }

    [XmlRoot("Messages")]
    public class Messages
    {
        [XmlElement("Header")]
        public Header header { get; set; }
        [XmlElement("Body")]
        public Body body { get; set; }
    }

    /// <summary>
    /// XML节点——Header
    /// </summary>
    public class Header
    {
        private int appid;
        private string entityType;
        private string operateType;

        /// <summary>
        /// CC用户呼入[12]（必填）
        /// </summary>
        public int AppId
        {
            set { appid = value; }
            get { return appid; }
        }

        /// <summary>
        /// CC用户呼入[CCUserCall]（必填）
        /// </summary>
        public string EntityType
        {
            set { entityType = value; }
            get { return entityType; }
        }

        /// <summary>
        /// 新增[Add]（必填）
        /// </summary>
        public string OperateType
        {
            set { operateType = value; }
            get { return operateType; }
        }
    }

    /// <summary>
    /// XML节点——Body
    /// </summary>
    public class Body
    {
        private Guid entityId;
        /// <summary>
        /// 业务数据实体唯一标识[GUID]（必填）
        /// </summary>
        public Guid EntityId
        {
            set { entityId = value; }
            get { return entityId; }
        }
        [XmlElement("OperaterInfo")]
        public OperaterInfo operaterInfo { get; set; }
        [XmlElement("UserInfo")]
        public UserInfo userInfo { get; set; }
        [XmlElement("CarInfo")]
        public CarInfo carInfo { get; set; }
        [XmlElement("CareCarInfo")]
        public CareCarInfo careCarInfo { get; set; }
        [XmlElement("WorkOrderInfo")]
        public WorkOrderInfo workOrderInfo { get; set; }
        [XmlElement("DealerInfo")]
        public DealerInfo dealerInfo { get; set; }
        [XmlElement("ActivityInfo")]
        public ActivityInfo activityInfo { get; set; }
    }

    /// <summary>
    /// XML节点（操作信息）——Body—OperaterInfo
    /// </summary>
    public class OperaterInfo
    {
        private string mobile;
        private string realName;
        private string userIp;
        private string operateTime;

        /// <summary>
        /// 手机（必填）
        /// </summary>
        public string Mobile
        {
            set { mobile = value; }
            get { return mobile; }
        }

        /// <summary>
        /// 真实姓名（必填）
        /// </summary>
        public string RealName
        {
            set { realName = value; }
            get { return realName; }
        }

        /// <summary>
        /// IP（必填）
        /// </summary>
        public string UserIp
        {
            set { userIp = value; }
            get { return userIp; }
        }

        /// <summary>
        /// 生成时间（必填）
        /// </summary>
        public string OperateTime
        {
            set { operateTime = value; }
            get { return operateTime; }
        }
    }

    /// <summary>
    /// XML节点（用户信息）——Body—UserInfo
    /// </summary>
    public class UserInfo
    {
        private string address;
        private string cityId;
        private string provinceId;
        private string sex;

        /// <summary>
        /// 地址（选填）
        /// </summary>
        public string Address
        {
            set { address = value; }
            get { return address; }
        }

        /// <summary>
        /// 城市Id（选填）
        /// </summary>
        public string CityId
        {
            set { cityId = value; }
            get { return cityId; }
        }

        /// <summary>
        /// 省份Id（选填）
        /// </summary>
        public string ProvinceId
        {
            set { provinceId = value; }
            get { return provinceId; }
        }

        /// <summary>
        /// 性别（选填 男：1  女:0  保密:2）
        /// </summary>
        public string Sex
        {
            set { sex = value; }
            get { return sex; }
        }
    }

    /// <summary>
    /// XML节点（关注车型信息）——Body—CarInfo
    /// </summary>
    public class CarInfo
    {
        private int bs_Id;
        private int cs_Id;
        private int car_Id;

        /// <summary>
        /// 车型品牌ID（选填）
        /// </summary>
        public int Bs_Id
        {
            set { bs_Id = value; }
            get { return bs_Id; }
        }

        /// <summary>
        /// 车型ID（选填）
        /// </summary>
        public int Cs_Id
        {
            set { cs_Id = value; }
            get { return cs_Id; }
        }

        /// <summary>
        /// 车款ID（选填）
        /// </summary>
        public int Car_Id
        {
            set { car_Id = value; }
            get { return car_Id; }
        }
    }

    /// <summary>
    /// XML节点（出售车型信息）——Body—CareCarInfo
    /// </summary>
    public class CareCarInfo
    {
        private int bs_Id;
        private int cs_Id;
        private int car_Id;

        /// <summary>
        /// 车型品牌ID（选填）
        /// </summary>
        public int Bs_Id
        {
            set { bs_Id = value; }
            get { return bs_Id; }
        }

        /// <summary>
        /// 车型ID（选填）
        /// </summary>
        public int Cs_Id
        {
            set { cs_Id = value; }
            get { return cs_Id; }
        }

        /// <summary>
        /// 车款ID（选填）
        /// </summary>
        public int Car_Id
        {
            set { car_Id = value; }
            get { return car_Id; }
        }
    }

    /// <summary>
    /// XML节点（推荐活动信息）——Body—ActivityInfo
    /// </summary>
    public class ActivityInfo
    {
        private string[] activityId;
        [XmlElement("ActivityId")]
        public string[] ActivityId
        {
            set { activityId = value; }
            get { return activityId; }
        }
    }

    /// <summary>
    /// XML节点（工单信息）——Body—WorkOrderInfo
    /// </summary>
    public class WorkOrderInfo
    {
        private int categoryId;
        private string categoryName;
        private int visited;

        /// <summary>
        /// 工单分类Id（必填）
        /// </summary>
        public int CategoryId
        {
            set { categoryId = value; }
            get { return categoryId; }
        }

        /// <summary>
        /// 工单分类名称（必填）
        /// </summary>
        public string CategoryName
        {
            set { categoryName = value; }
            get { return categoryName; }
        }

        /// <summary>
        /// 是否接受回访[0|1]（必填）
        /// </summary>
        public int Visited
        {
            set { visited = value; }
            get { return visited; }
        }
    }

    /// <summary>
    /// XML节点（经销商信息）——Body—DealerInfo
    /// </summary>
    public class DealerInfo
    {
        private int dealerId;
        private string dealerName;

        /// <summary>
        /// 经销商Id（必填）
        /// </summary>
        public int DealerId
        {
            set { dealerId = value; }
            get { return dealerId; }
        }

        /// <summary>
        /// 经销商名称（必填）
        /// </summary>
        public string DealerName
        {
            set { dealerName = value; }
            get { return dealerName; }
        }
    }

}
